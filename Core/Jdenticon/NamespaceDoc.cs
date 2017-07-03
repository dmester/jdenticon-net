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

namespace Jdenticon
{
    /// <summary>
    /// Contains all you need to generate icons with no or very basic
    /// customization. Import this namespace to be able to render identicons
    /// since all save operations are implemented as extension methods for the
    /// <see cref="Identicon"/> class.
    /// </summary>
    /// <example>
    /// This code illustrates how to use Jdenticon to generate an icon from a string 
    /// and save it as a PNG image. Note that all save operations are implemented as
    /// extension methods, which means a using for the <see cref="Jdenticon"/> namespace
    /// is required.
    /// <code language="cs">
    /// using Jdenticon;
    /// // ...
    /// var icon = Identicon.FromValue("string to hash");
    /// icon.SaveAsPng("test.png", 160);
    /// </code>
    /// </example>
    [CompilerGenerated]
    internal static class NamespaceDoc
    {
    }
}
