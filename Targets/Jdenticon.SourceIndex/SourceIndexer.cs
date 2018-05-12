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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jdenticon.SourceIndex
{
    internal class SourceIndexer
    {
        public string GitRootUrl { get; set; }

        public string[] PreprocessorSymbols { get; set; }

        private string ConstructGitUrl(string relativePath, int startLine, int endLine)
        {
            return GitRootUrl.TrimEnd('/') + "/" +
                relativePath.TrimStart('\\').Replace('\\', '/') +
                (startLine < endLine ?
                    ("#L" + (startLine + 1) + "-L" + (endLine + 1)) :
                    ("#L" + (startLine + 1)));
        }

        public IEnumerable<SourceReference> Index(string directoryPath, string pattern)
        {
            var options = new CSharpParseOptions(preprocessorSymbols: PreprocessorSymbols);

            // Get full path so that we can construct relative paths further down
            directoryPath = Path.GetFullPath(directoryPath);

            var files = Directory
                .EnumerateFiles(directoryPath, pattern, SearchOption.AllDirectories)
                .Select(x => ParseFile(x, options))
                .ToList();

            // Contains known types in mscorlib
            var frameworkTypes = typeof(string)
                .Assembly
                .GetExportedTypes()
                .Select(x => x.FullName);

            // Contains known types in mscorlib and Jdenticon
            var knownTypes = new HashSet<string>(files
                .SelectMany(x => x.Symbols.OfType<TypeReference>())
                .Select(x => x.Identifier)
                .Concat(frameworkTypes));

            var symbols = files
                .SelectMany(file => file.Symbols)
                .Where(o => o.IsExported && o.Identifier != null)
                .Select(o => new
                {
                    Id = o.Type + "_" + Regex.Replace(o.Identifier, "[^a-zA-Z0-9]", "_"),
                    Source = o
                })
                .GroupBy(o => o.Id)

                // Group by identifier and sort each group by the secondary order
                .Select(group =>
                {
                    var resolveSecondary = group.Count() > 1;

                    return group
                        .Select(o => new
                        {
                            Id = o.Id,
                            SecondaryOrder = resolveSecondary ?
                                o.Source.GetSecondaryOrder(knownTypes) : "",
                            Source = o.Source
                        })
                        .OrderBy(o => o.SecondaryOrder);
                });
            
            // For symbols sharing the same identifier, add a sequence number 
            // as done by Sandcastle.
            foreach (var group in symbols)
            {
                var counter = 0;

                foreach (var item in group)
                {
                    var id = item.Id;
                    if (counter > 0) id += "_" + counter;
                    counter++;

                    yield return new SourceReference
                    {
                        Id = id,
                        Url = ConstructGitUrl(
                            MakeRelative(directoryPath, item.Source.File.Path),
                            item.Source.StartLine,
                            item.Source.EndLine)
                    };
                }
            }
        }

        private static string MakeRelative(string fullRootPath, string fullChildPath)
        {
            if (!fullChildPath.StartsWith(fullRootPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"'{fullChildPath}' cannot be made relative to '{fullRootPath}'.");
            }
            return fullChildPath.Substring(fullRootPath.Length).TrimStart('/', '\\');
        }

        private FileReferenceInfo ParseFile(string path, CSharpParseOptions options)
        {
            var content = File.ReadAllText(path);
            var contentLines = content.Split('\n');
            var tree = CSharpSyntaxTree.ParseText(content, options);

            var descendants = tree.GetRoot()
                .DescendantNodes(node =>
                    node is CompilationUnitSyntax ||
                    node is NamespaceDeclarationSyntax ||
                    node is EnumDeclarationSyntax ||
                    node is InterfaceDeclarationSyntax ||
                    node is StructDeclarationSyntax ||
                    node is ClassDeclarationSyntax)
                .ToList();

            var namespaces = descendants
                .OfType<NamespaceDeclarationSyntax>()
                .Select(x => x.Name.ToString().Split('.'))
                .SelectMany(x => Enumerable
                    .Range(1, x.Length)
                    .Select(n => string.Join(".", x.Take(n))));

            var usings = descendants
                .OfType<UsingDirectiveSyntax>()
                .Select(x => x.Name.ToString())
                .Concat(namespaces)
                .ToArray();

            var fileInfo = new FileReferenceInfo(path, contentLines, usings);

            var references = new IEnumerable<SymbolReference>[]
            {
                // Methods
                descendants
                    .OfType<BaseMethodDeclarationSyntax>()
                    .Select(method => new MethodReference(fileInfo, method)),

                // Types
                descendants
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Select(method => new TypeReference(fileInfo, method)),

                // Properties
                descendants
                    .OfType<PropertyDeclarationSyntax>()
                    .Select(method => new PropertyReference(fileInfo, method)),
            };

            fileInfo.Symbols.AddRange(references.SelectMany(x => x));

            return fileInfo;
        }

    }
}
