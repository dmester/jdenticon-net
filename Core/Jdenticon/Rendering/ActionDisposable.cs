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
using System.Text;
using System.Threading;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// An <see cref="IDisposable"/> implementation that executes an <see cref="Action"/>
    /// upon disposal.
    /// </summary>
    public class ActionDisposable : IDisposable
    {
        Action action;

        /// <summary>
        /// Creates an instance of <see cref="ActionDisposable"/>.
        /// </summary>
        /// <param name="action">The action that will be invoked when the <see cref="ActionDisposable"/> is disposed.</param>
        public ActionDisposable(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Gets an <see cref="ActionDisposable"/> that does nothing when it is disposed.
        /// </summary>
        public static ActionDisposable Empty
        {
            get { return new ActionDisposable(null); }
        }

        /// <summary>
        /// Calls the dispose action of this <see cref="ActionDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref action, null)?.Invoke();
        }
    }
}
