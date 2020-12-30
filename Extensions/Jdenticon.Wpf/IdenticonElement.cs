#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2020
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
    /// <example>
    /// <para>
    ///     Place the <see cref="IdenticonElement"/> where you want an identicon to be rendered in your application. 
    ///     Don't forget to add the Jdenticon XML namespace.
    /// </para>
    /// <code language="xml" title="Example usage in WPF">
    /// &lt;Window x:Class="SampleApp.MainWindow"
    ///         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    ///         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    ///         xmlns:jd="clr-namespace:Jdenticon.Wpf;assembly=Jdenticon.Wpf"
    ///         Title="MainWindow" Height="250" Width="600"&gt;
    ///     &lt;Window.Resources&gt;
    ///         &lt;Style TargetType="{x:Type jd:IdenticonElement}"&gt;
    ///             &lt;Setter Property="Width" Value="100" /&gt;
    ///             &lt;Setter Property="Height" Value="100" /&gt;
    ///             &lt;Setter Property="Margin" Value="10" /&gt;
    ///         &lt;/Style&gt;
    ///     &lt;/Window.Resources&gt;
    ///     &lt;StackPanel Orientation="Horizontal" HorizontalAlignment="Center"&gt;
    ///         &lt;jd:IdenticonElement Value="icon1" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon2" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon3" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon4" /&gt;
    ///     &lt;/StackPanel&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// <para>
    ///     This is the resulting application:
    /// </para>
    /// <img src="../images/WpfIcons.png" alt="Sample WPF application with four identicons" />
    /// 
    /// <h4>Binding</h4>
    /// <para>
    ///     To make the identicon useful you probably want to bind a value to the icon. The value decides what the
    ///     icon will look like.
    /// </para>
    /// <code language="xml" title="Example usage in WPF">
    /// &lt;jd:IdenticonElement Value="{Binding Path=UserID}" /&gt;
    /// </code>
    /// 
    /// <h4>Styling</h4>
    /// <para>
    ///     The colors of the identicons can be customized using the stying properties. A suggestion is to create
    ///     a style for your identicons that is used throughout the application to ensure a consistent look.
    ///     You can use the <a href="https://jdenticon.com/icon-designer.html">icon designer</a> to create a style.
    /// </para>
    /// <code language="xml" title="Custom style">
    /// &lt;Window x:Class="SampleApp.MainWindow"
    ///         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    ///         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    ///         xmlns:jd="clr-namespace:Jdenticon.Wpf;assembly=Jdenticon.Wpf"
    ///         Title="MainWindow" Height="250" Width="600"&gt;
    ///     &lt;Window.Resources&gt;
    ///         &lt;Style TargetType="{x:Type jd:IdenticonElement}"&gt;
    ///             &lt;Setter Property="Width" Value="100" /&gt;
    ///             &lt;Setter Property="Height" Value="100" /&gt;
    ///             &lt;Setter Property="Margin" Value="10" /&gt;
    ///             
    ///             &lt;!-- Custom identicon style --&gt;
    ///             &lt;!-- https://jdenticon.com/icon-designer.html?config=2a4766ff10cf303054545454 --&gt;
    ///             &lt;Setter Property="Hues" Value="207deg"/&gt;
    ///             &lt;Setter Property="Background" Value="#2a4766"/&gt;
    ///             &lt;Setter Property="ColorLightness" Value="0.84, 0.84"/&gt;
    ///             &lt;Setter Property="GrayscaleLightness" Value="0.84, 0.84"/&gt;
    ///             &lt;Setter Property="ColorSaturation" Value="0.48"/&gt;
    ///             &lt;Setter Property="GrayscaleSaturation" Value="0.48"/&gt;
    ///         &lt;/Style&gt;
    ///     &lt;/Window.Resources&gt;
    ///     &lt;StackPanel Orientation="Horizontal" HorizontalAlignment="Center"&gt;
    ///         &lt;jd:IdenticonElement Value="icon1" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon2" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon3" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon4" /&gt;
    ///     &lt;/StackPanel&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// <para>
    ///     Here is what the icons above look like:
    /// </para>
    /// <img src="../images/WpfStyled.png" alt="Identicons with custom style"/>
    /// </example>
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
        /// <example>
        /// <para>
        ///     Here is an example where the value property is bound to the value of a textbox.
        /// </para>
        /// <code language="xml" title="Binding value">
        /// &lt;Window x:Class="MySample.MainWindow"
        ///         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        ///         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        ///         xmlns:jd="clr-namespace:Jdenticon.Wpf;assembly=Jdenticon.Wpf"
        ///         Title="Jdenticon WPF Sample" Height="530" Width="450"&gt;
        ///     &lt;Grid Margin="25"&gt;
        ///         &lt;Grid.RowDefinitions&gt;
        ///             &lt;RowDefinition Height="auto"/&gt;
        ///             &lt;RowDefinition Height="auto"/&gt;
        ///             &lt;RowDefinition Height="*"/&gt;
        ///         &lt;/Grid.RowDefinitions&gt;
        /// 
        ///         &lt;Label Grid.Row="0" Padding="5,0,5,5"&gt;Value to hash:&lt;/Label&gt;
        ///         &lt;TextBox x:Name="tbValue" Grid.Row="1" Padding="5" Text="Jdenticon"&gt;&lt;/TextBox&gt;
        ///         &lt;jd:IdenticonElement Grid.Row="2" Margin="40" Value="{Binding ElementName=tbValue, Path=Text}" /&gt;
        ///     &lt;/Grid&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <para>
        ///     Here is the outcome:
        /// </para>
        /// <img src="../images/WpfValueBinding.png" alt="WPF value binding"/>
        /// </example>
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


        /// <exclude/>
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

        /// <exclude/>
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
