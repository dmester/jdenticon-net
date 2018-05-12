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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jdenticon.SourceIndex.Parser
{
    internal class NameResolver
    {
        public static string GetFullName(SyntaxNode node)
        {
            string name;

            if (node is ConstructorDeclarationSyntax ctor)
            {
                name = "#ctor";
            }
            else if (node is MethodDeclarationSyntax realMethod)
            {
                name = realMethod.Identifier.ToString();

                var types = realMethod.TypeParameterList;
                if (types != null)
                {
                    name += "``" + types.Parameters.Count;
                }
            }
            else if (node is PropertyDeclarationSyntax property)
            {
                name = property.Identifier.ToString();
            }
            else if (node is NamespaceDeclarationSyntax ns)
            {
                name = ns.Name.ToString();
            }
            else if (node is TypeDeclarationSyntax type)
            {
                name = type.Identifier.ToString();

                var types = type.TypeParameterList;
                if (types != null)
                {
                    name += "`" + types.Parameters.Count;
                }
            }
            else if (node is BaseTypeDeclarationSyntax baseType)
            {
                name = baseType.Identifier.ToString();
            }
            else
            {
                return null;
            }

            // Prepend namespace and parent types
            if (node.Parent != null)
            {
                var path = GetFullName(node.Parent);
                if (path != null)
                {
                    name = path + "." + name;
                }
            }

            return name;
        }
    }
}
