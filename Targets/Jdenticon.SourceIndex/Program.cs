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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.SourceIndex
{
    /**
     * This build step parses all C# source and build an index of 
     * Sandcastle symbol ids to source urls to GitHub.
     */
    class Program
    {
        static void Main(string[] args)
        {
            var version = typeof(Program).Assembly.GetName().Version;

            var indexer = new SourceIndexer
            {
                GitRootUrl = $"https://github.com/dmester/jdenticon-net/blob/{version.Major}.{version.Minor}.{version.Build}/",
                PreprocessorSymbols = new string[] { "HAVE_FILE_STREAM" }
            };

            var symbols = indexer
                .Index(args[0], "*.cs")
                .ToDictionary(x => x.Id, x => x.Url);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(symbols);
            File.WriteAllText(args[1], json, Encoding.UTF8);

            Console.WriteLine("Successfully indexed {0} symbols.", symbols.Count);
        }
    }
}
