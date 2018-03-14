#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
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
using System.Runtime.CompilerServices;
using System.Text;

namespace Jdenticon.AspNet.WebApi
{
    /// <summary>
    /// Classes for using Jdenticon from ASP.NET WebApi.
    /// </summary>
    /// <example>
    /// <para>
    /// To use Jdenticon from ASP.NET WebApi, install the <c>Jdenticon.AspNet.WebApi</c> NuGet package.
    /// </para>
    /// <code language="bat" title="NuGet Package Manager">
    /// PM&gt; Install-Package Jdenticon.AspNet.WebApi
    /// </code>
    /// <para>
    /// The <see cref="IdenticonResult"/> class is then available to return an identicon
    /// from a WebApi controller action.
    /// </para>
    /// <code language="cs" title="Api controller">
    /// public class IdenticonController : ApiController
    /// {
    ///     public IdenticonResult Get(string name, int size)
    ///     {
    ///         return IdenticonResult.FromValue(name, size);
    ///     }
    /// }
    /// </code>
    /// </example>
    [CompilerGenerated]
    internal static class NamespaceDoc
    {
    }
}
