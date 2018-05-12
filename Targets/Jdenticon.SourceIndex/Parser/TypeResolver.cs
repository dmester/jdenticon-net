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

using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jdenticon.SourceIndex.Parser
{
    class TypeResolver
    {
        private IDictionary<string, string> genericTypes;

        public ISet<string> KnownTypes { get; set; }
        public string[] Usings { get; set; }
        public MemberDeclarationSyntax Context { get; set; }

        private string ResolveFullName(string type)
        {
            if (genericTypes == null)
            {
                genericTypes = GetGenericParametersInContext(Context);
            }

            if (genericTypes.TryGetValue(type.ToString(), out var genericType))
            {
                return genericType;
            }

            if (Usings == null || KnownTypes == null)
            {
                return type;
            }

            return Usings
                .Select(x => x + "." + type)
                .FirstOrDefault(x => KnownTypes.Contains(x)) ??
                type;
        }

        private static Dictionary<string, string> GetGenericParametersInContext(MemberDeclarationSyntax context)
        {
            var result = new Dictionary<string, string>();
            var genericClasses = new List<IEnumerable<TypeParameterSyntax>>();

            // Class type parameters
            var cursor = context.Parent;
            while (cursor is TypeDeclarationSyntax typeType)
            {
                cursor = cursor.Parent;

                if (typeType.TypeParameterList != null)
                {
                    genericClasses.Add(typeType.TypeParameterList.Parameters);
                }
            }

            var counter = 0;
            for (var i = genericClasses.Count - 1; i >= 0; i--)
            {
                foreach (var type in genericClasses[i])
                {
                    result[type.Identifier.ToString()] = "`" + (counter++);
                }
            }

            // Method type parameters
            if (context is MethodDeclarationSyntax realMethod)
            {
                if (realMethod.TypeParameterList != null)
                {
                    counter = 0;

                    foreach (var type in realMethod.TypeParameterList.Parameters)
                    {
                        result[type.Identifier.ToString()] = "``" + (counter++);
                    }
                }
            }

            return result;
        }

        public string ResolveType(ParameterSyntax parameter)
        {
            var name = ResolveType(parameter.Type);

            if (parameter.Modifiers.Any(y =>
                y.ToString() == "out" ||
                y.ToString() == "ref"))
            {
                name += "@";
            }

            return name;
        } 

        public string ResolveType(TypeSyntax type)
        {
            if (type is ArrayTypeSyntax arrayType)
            {
                var rank = arrayType.RankSpecifiers.Sum(x => x.Rank);
                return ResolveType(arrayType.ElementType) +
                    "[" +
                    string.Join(",", Enumerable.Range(0, rank).Select(x => "0:")) +
                    "]";
            }

            if (type is GenericNameSyntax genericName)
            {
                var refName = genericName.Identifier + "`" + genericName.TypeArgumentList.Arguments.Count;

                var name = ResolveFullName(refName);

                name = Regex.Replace(name, "`\\d+$", "");

                return
                    name + "" +
                    "{" +
                    string.Join(",", genericName
                        .TypeArgumentList
                        .Arguments
                        .Select(x => ResolveType(x))) +
                    "}";
            }

            if (type is PointerTypeSyntax pointerType)
            {
                return ResolveType(pointerType.ElementType) + "*";
            }

            if (type is PredefinedTypeSyntax predefinedType)
            {
                switch (predefinedType.Keyword.ToString())
                {
                    case "string": return "System.String";
                    case "byte": return "System.Byte";
                    case "sbyte": return "System.SByte";
                    case "char": return "System.Char";
                    case "int": return "System.Int32";
                    case "uint": return "System.UInt32";
                    case "short": return "System.Int16";
                    case "ushort": return "System.UInt16";
                    case "long": return "System.Int64";
                    case "ulong": return "System.UInt64";
                    case "float": return "System.Single";
                    case "double": return "System.Double";
                    case "decimal": return "System.Decimal";
                    case "bool": return "System.Boolean";
                    case "object": return "System.Object";
                    default: throw new NotSupportedException("Unsupported keyword '" + predefinedType.Keyword + "'.");
                }
            }

            if (type is RefTypeSyntax refType)
            {
                return ResolveType(refType.Type) + "&";
            }

            if (type is TupleTypeSyntax tupleType)
            {
                var elements = tupleType
                    .Elements
                    .Select(x => ResolveType(x.Type));
                return "System.ValueTuple{" + string.Join(",", elements) + "}";
            }

            if (type is NullableTypeSyntax nullableType)
            {
                return "System.Nullable{" + ResolveType(nullableType.ElementType) + "}";
            }

            return ResolveFullName(type.ToString());
        }

    }
}
