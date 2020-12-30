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

using Jdenticon.Rendering;
using Jdenticon.Wpf.Converters;
using Jdenticon.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using JdenticonColor = Jdenticon.Rendering.Color;

namespace Jdenticon.Wpf
{
    /// <summary>
    /// Element that renders and displays an identicon.
    /// </summary>
    public class IdenticonElement : FrameworkElement
    {
        private const string CatAppearance = "Appearance";
        private const string CatData = "Data";

        #region Style

        /// <summary>
        /// Gets or sets an <see cref="Jdenticon.IdenticonStyle"/> that will affect the appearance of the generated icon.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This property will return a copy of the <see cref="Jdenticon.IdenticonStyle"/> currently in use. Changes to the returned instance
        ///     will not affect the rendering of this icon, unless the property is set with the updated <see cref="Jdenticon.IdenticonStyle"/> instance.
        /// </para>
        /// <para>
        ///     This property is available since version 3.0.0.
        /// </para>
        /// </remarks>
        public IdenticonStyle IdenticonStyle
        {
            get
            {
                return new IdenticonStyle
                {
                    ColorLightness = ColorLightness,
                    GrayscaleLightness = GrayscaleLightness,
                    ColorSaturation = ColorSaturation,
                    GrayscaleSaturation = GrayscaleSaturation,
                    Padding = Padding,
                    Hues = Hues?.ToCollection(),
                    BackColor = Background is SolidColorBrush brush ? brush.Color.ToJdenticon() : JdenticonColor.Transparent,
                };
            }
            set
            {
                if (value == null) value = new IdenticonStyle();

                ColorLightness = value.ColorLightness;
                GrayscaleLightness = value.GrayscaleLightness;
                ColorSaturation = value.ColorSaturation;
                GrayscaleSaturation = value.GrayscaleSaturation;
                Padding = value.Padding;
                Background = new SolidColorBrush(value.BackColor.ToWpf());
                Hues = new HueString(value.Hues);
            }
        }

        /// <inheritdoc cref="IdenticonStyle.Padding"/>
        /// <remarks>
        /// This property is available since version 3.0.0.
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        public float Padding
        {
            get => (float)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Padding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding), typeof(float), typeof(IdenticonElement), 
            new PropertyMetadata(IdenticonStyle.DefaultPadding, OnStyleChanged),
            IsValidPadding);

        /// <summary>
        /// Gets or sets the background of the identicon.
        /// </summary>
        /// <remarks>
        /// This property is available since version 3.0.0.
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            nameof(Background), typeof(Brush), typeof(IdenticonElement), 
            new PropertyMetadata(Brushes.White, OnStyleChanged));

        /// <summary>
        /// Gets or sets a string containing one or multiple hues, that the identicon will be limited to use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The string should contain a list of hues, separated by commas.
        /// </para>
        /// <code language="none" title="Limit hues to red and green">
        /// 0deg, 130deg
        /// </code>
        /// <para>
        /// By default the values are specified in turns on the range [0, 1), unless one of the units listed below is explicitly specified.
        /// </para>
        /// <list type="table">
        /// <listheader>
        ///     <term>Unit</term>
        ///     <term>Suffix</term>
        ///     <term>Example</term>
        /// </listheader>
        /// <item>
        ///     <term>Degrees</term>
        ///     <term>deg</term>
        ///     <term>90deg</term>
        /// </item>
        /// <item>
        ///     <term>Radians</term>
        ///     <term>rad</term>
        ///     <term>1.4rad</term>
        /// </item>
        /// <item>
        ///     <term>Gradians</term>
        ///     <term>grad</term>
        ///     <term>100grad</term>
        /// </item>
        /// <item>
        ///     <term>Turns</term>
        ///     <term>turn</term>
        ///     <term>0.5turn</term>
        /// </item>
        /// </list>
        /// <para>
        /// This property is available since version 3.0.0.
        /// </para>
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        public HueString Hues
        {
            get => (HueString)GetValue(HuesProperty);
            set => SetValue(HuesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Hues"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HuesProperty = DependencyProperty.Register(
            nameof(Hues), typeof(HueString), typeof(IdenticonElement),
            new PropertyMetadata(HueString.Empty, OnStyleChanged));


        /// <inheritdoc cref="IdenticonStyle.GrayscaleLightness"/>
        /// <remarks>
        /// <inheritdoc cref="IdenticonStyle.GrayscaleLightness"/>
        /// <para>
        /// This property is available since version 3.0.0.
        /// </para>
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        [TypeConverter(typeof(FloatRangeConverter))]
        public Range<float> GrayscaleLightness
        {
            get => (Range<float>)GetValue(GrayscaleLightnessProperty);
            set => SetValue(GrayscaleLightnessProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GrayscaleLightness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GrayscaleLightnessProperty = DependencyProperty.Register(
            nameof(GrayscaleLightness), typeof(Range<float>), typeof(IdenticonElement), 
            new PropertyMetadata(IdenticonStyle.DefaultGrayscaleLightness, OnStyleChanged),
            IsValidPercentRange);


        /// <inheritdoc cref="IdenticonStyle.ColorLightness"/>
        /// <remarks>
        /// <inheritdoc cref="IdenticonStyle.ColorLightness"/>
        /// <para>
        /// This property is available since version 3.0.0.
        /// </para>
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        [TypeConverter(typeof(FloatRangeConverter))]
        public Range<float> ColorLightness
        {
            get => (Range<float>)GetValue(ColorLightnessProperty);
            set => SetValue(ColorLightnessProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColorLightness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorLightnessProperty = DependencyProperty.Register(
            nameof(ColorLightness), typeof(Range<float>), typeof(IdenticonElement),
            new PropertyMetadata(IdenticonStyle.DefaultColorLightness, OnStyleChanged),
            IsValidPercentRange);


        /// <inheritdoc cref="IdenticonStyle.ColorSaturation"/>
        /// <remarks>
        /// <para>
        /// This property is available since version 3.0.0.
        /// </para>
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        public float ColorSaturation
        {
            get => (float)GetValue(ColorSaturationProperty);
            set => SetValue(ColorSaturationProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColorSaturation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorSaturationProperty = DependencyProperty.Register(
            nameof(ColorSaturation), typeof(float), typeof(IdenticonElement),
            new PropertyMetadata(IdenticonStyle.DefaultColorSaturation, OnStyleChanged),
            IsValidPercent);


        /// <inheritdoc cref="IdenticonStyle.GrayscaleSaturation"/>
        /// <remarks>
        /// <para>
        /// This property is available since version 3.0.0.
        /// </para>
        /// </remarks>
        [Bindable(true), Category(CatAppearance)]
        public float GrayscaleSaturation
        {
            get => (float)GetValue(GrayscaleSaturationProperty);
            set => SetValue(GrayscaleSaturationProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GrayscaleSaturation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GrayscaleSaturationProperty = DependencyProperty.Register(
            nameof(GrayscaleSaturation), typeof(float), typeof(IdenticonElement),
            new PropertyMetadata(IdenticonStyle.DefaultGrayscaleSaturation, OnStyleChanged),
            IsValidPercent);

        #endregion
        
        /// <summary>
        /// Gets or sets the <see cref="IconGenerator"/> that is responsible for generating the icon patterns.
        /// This property is <c>null</c> when the default generator is used.
        /// </summary>
        public IconGenerator IconGenerator
        {
            get => (IconGenerator)GetValue(IconGeneratorProperty);
            set => SetValue(IconGeneratorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IconGenerator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconGeneratorProperty = DependencyProperty.Register(
            nameof(IconGenerator), typeof(IconGenerator), typeof(IdenticonElement), new PropertyMetadata(null, OnValueChanged));

        /// <summary>
        /// Gets or sets a value that the generated identicon will be based upon. A string representation of the value will be hashed using SHA1.
        /// </summary>
        [Bindable(true), Category(CatData)]
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(object), typeof(IdenticonElement), new PropertyMetadata("", OnValueChanged));


        protected override void OnRender(DrawingContext drawingContext)
        {
            var width = ActualWidth;
            var height = ActualHeight;
            var iconSize = (int)Math.Min(width, height);

            var background = Background;
            if (background != null)
            {
                drawingContext.DrawRectangle(
                    brush: background,
                    pen: null,
                    rectangle: new Rect(0, 0, width, height));
            }

            if (iconSize > 0)
            {
                var identicon = Identicon.FromValue(Value, iconSize);
                identicon.Style = IdenticonStyle;
                identicon.Style.BackColor = JdenticonColor.Transparent; // The background was drawn above
                identicon.IconGenerator = IconGenerator;
                identicon.Draw(drawingContext);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = Math.Min(availableSize.Width, availableSize.Height);
            return double.IsPositiveInfinity(size) ? Size.Empty : new Size(size, size);
        }

        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IdenticonElement)d).InvalidateVisual();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IdenticonElement)d).InvalidateVisual();
        }


        private static bool IsValidPadding(object value)
        {
            return
                value is float padding &&
                padding >= 0 && padding <= 0.4f;
        }

        private static bool IsValidPercent(object value)
        {
            return
                value is float f &&
                f >= 0 && f <= 1f;
        }

        private static bool IsValidPercentRange(object value)
        {
            return
                value is Range<float> range &&
                range.From >= 0 && range.From <= 1f &&
                range.To >= 0 && range.To <= 1f;
        }
    }
}
