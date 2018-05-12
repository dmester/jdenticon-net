#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2018
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using Jdenticon.SourceIndex.Parser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jdenticon.SourceIndex
{
    [DebuggerDisplay("{Identifier}")]
    abstract class SymbolReference
    {
        public abstract string Type { get; }

        public string Identifier { get; }

        public int StartLine { get; }

        public int EndLine { get; }

        public FileReferenceInfo File { get; }

        public bool IsExported { get; }

        public SymbolReference(FileReferenceInfo file, SyntaxNode node)
        {
            Identifier = NameResolver.GetFullName(node);
            File = file;

            // Get line span
            var loc = node.GetLocation().GetLineSpan();
            StartLine = loc.StartLinePosition.Line;
            EndLine = loc.EndLinePosition.Line;

            // Expand line span to include xml doc
            while (
                StartLine > 0 &&
                StartLine - 1 < file.Lines.Length &&
                Regex.IsMatch(file.Lines[StartLine - 1], "\\s*///")
                )
            {
                StartLine--;
            }

            // Determine whether the symbol is visible to external assemblies
            IsExported = IsExportedCore(node);
        }

        private static bool IsExportedCore(SyntaxNode node)
        {
            while (node is MemberDeclarationSyntax)
            {
                if (node is BaseMethodDeclarationSyntax method)
                {
                    if (!IsExportedCore(method.Modifiers)) return false;
                }
                else if (node is BasePropertyDeclarationSyntax property)
                {
                    if (!IsExportedCore(property.Modifiers)) return false;
                }
                else if (node is BaseTypeDeclarationSyntax type)
                {
                    if (!IsExportedCore(type.Modifiers)) return false;
                }

                node = node.Parent;
            }

            return true;
        }

        private static bool IsExportedCore(SyntaxTokenList modifiers)
        {
            return modifiers.Any(modifier =>
                modifier.ToString() == "public" ||
                modifier.ToString() == "protected" ||

                // Partials are inconclusive
                modifier.ToString() == "partial");
        }

        public virtual string GetSecondaryOrder(HashSet<string> knownTypes)
        {
            return string.Format("{0:0000}{1}",
                File.Path.Length,
                Identifier);
        }
    }
}
