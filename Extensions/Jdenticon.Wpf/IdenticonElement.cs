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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Jdenticon
{
    public class IdenticonElement : FrameworkElement
    {
        private DrawingVisual visual = new DrawingVisual();

        /// <summary>
        /// Creates a new instance of <see cref="IdenticonElement"/>.
        /// </summary>
        public IdenticonElement()
        {
            Loaded += (sender, e) => AddVisualChild(visual);
            Unloaded += (sender, e) => RemoveVisualChild(visual);
        }
        
        /// <summary>
        /// Gets or sets the <see cref="IconGenerator"/> that is responsible for
        /// generating the icon patterns.
        /// </summary>
        public IconGenerator IconGenerator { get; set; }

        /// <summary>
        /// Gets or sets the value that is used as base for the icon generation. The value's
        /// string representation will be hashed using SHA1.
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set {SetValue(ValueProperty, value);}
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(IdenticonElement), new PropertyMetadata(false));

        /// <inheritdoc />
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            Render();
            base.OnRenderSizeChanged(sizeInfo);
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ValueProperty)
            {
                Render();
            }
        }

        private void Render()
        {
            using (var context = visual.RenderOpen())
            {
                var rect = new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight);
                Identicon.FromValue(Value).Draw(context,rect);
            }
        }

        /// <inheritdoc />
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        /// <inheritdoc />
        protected override Visual GetVisualChild(int index)
        {
            return visual;
        }
    }
}
