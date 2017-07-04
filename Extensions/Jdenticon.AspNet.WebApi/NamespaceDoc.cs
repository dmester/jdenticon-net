#region License
//
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// This software is provided 'as-is', without any express or implied
// warranty.In no event will the authors be held liable for any damages
// arising from the use of this software.
// 
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software.If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
//
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
