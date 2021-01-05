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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable enable

namespace Jdenticon
{
    /// <summary>
    /// Specifies the color style of an identicon.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     This class is used to customize the colors of generated icons. You can use the 
    ///     <a href="https://jdenticon.com/icon-designer.html">icon designer</a>
    ///     to simplify finding good values.
    /// </para>
    /// 
    /// <h4>Individual icons</h4>
    /// <para>
    ///     Individual icons are styled by setting the <see cref="Identicon.Style"/> property of an <see cref="Identicon"/> instance.
    /// </para>
    /// <code language="csharp" title="Styling an individual icon">
    /// var icon = Identicon.FromValue("string to hash", size: 100);
    /// icon.Style = new IdenticonStyle 
    /// {
    ///     Hues = new HueCollection { { 207, HueUnit.Degrees } },
    ///     BackColor = Color.FromRgba(42, 71, 102, 255),
    ///     ColorLightness = Range.Create(0.84f, 0.84f),
    ///     GrayscaleLightness = Range.Create(0.84f, 0.84f),
    ///     ColorSaturation = 0.48f,
    ///     GrayscaleSaturation = 0.48f
    /// };
    /// icon.SaveAsPng("test.png");
    /// </code>
    /// 
    /// <h4>Default style</h4>
    /// <para>
    ///     A default style can be created by setting the <see cref="Identicon.DefaultStyle">Identicon.DefaultStyle</see> property.
    ///     You can still override the style of individual icons with <see cref="Identicon.Style">Identicon.Style</see>.
    ///     This approach is suitable for most cases, including ASP.NET.
    /// </para>
    /// <note type="note">
    ///     The <see cref="Identicon.DefaultStyle">Identicon.DefaultStyle</see> property is not supported by
    ///     <see cref="T:Jdenticon.Wpf.IdenticonElement"/>. Instead create a WPF style setting the correspnding style properties
    ///     directly on <see cref="T:Jdenticon.Wpf.IdenticonElement"/>.
    /// </note>
    /// <code language="csharp" title="Setting a default style">
    /// Identicon.DefaultStyle = new IdenticonStyle 
    /// {
    ///     Hues = new HueCollection { { 207, HueUnit.Degrees } },
    ///     BackColor = Color.FromRgba(42, 71, 102, 255),
    ///     ColorLightness = Range.Create(0.84f, 0.84f),
    ///     GrayscaleLightness = Range.Create(0.84f, 0.84f),
    ///     ColorSaturation = 0.48f,
    ///     GrayscaleSaturation = 0.48f
    /// };
    /// 
    /// Identicon.FromValue("string to hash", size: 100).SaveAsPng("test.png");
    /// </code>
    /// 
    /// <h4>WPF</h4>
    /// <para>
    ///     In WPF the recommended way of styling an icon is by creating a 
    ///     <a href="https://docs.microsoft.com/en-us/dotnet/desktop/wpf/fundamentals/styles-templates-create-apply-style">WPF style</a>
    ///     setting style properties on <see cref="T:Jdenticon.Wpf.IdenticonElement"/>. You can also set an <see cref="IdenticonStyle"/>
    ///     by using the <see cref="P:Jdenticon.Wpf.IdenticonElement.IdenticonStyle">IdenticonElement.IdenticonStyle</see>
    ///     property.
    /// </para>
    /// <code language="xml" title="WPF identicon with custom style">
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
    /// 
    /// <h4>Windows Forms</h4>
    /// <para>
    ///     You can affect the style by setting the style properties directly on <see cref="T:Jdenticon.WinForms.IdenticonView"/>.
    /// </para>
    /// <img src="../images/WinFormsStyle.png" alt="Styling Windows Forms"/>
    /// <para>
    ///     Another option is to use the <see cref="P:Jdenticon.WinForms.IdenticonView.Style">IdenticonView.Style</see> property.
    /// </para>
    /// </remarks>
    public class IdenticonStyle : IEquatable<IdenticonStyle?>
    {
        #region Fields

        private HueCollection hues;
        private Color backColor;
        private float padding;
        private float colorSaturation;
        private float grayscaleSaturation;
        private Range<float> colorLightness;
        private Range<float> grayscaleLightness;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="IdenticonStyle"/> instance initialized with the default 
        /// values of each property.
        /// </summary>
        public IdenticonStyle()
        {
            hues = new HueCollection();
            backColor = DefaultBackColor;
            padding = DefaultPadding;
            colorSaturation = DefaultColorSaturation;
            grayscaleSaturation = DefaultGrayscaleSaturation;
            colorLightness = DefaultColorLightness;
            grayscaleLightness = DefaultGrayscaleLightness;
        }

        private IdenticonStyle(IdenticonStyle otherStyleToClone)
        {
            hues = new HueCollection(otherStyleToClone.hues);
            backColor = otherStyleToClone.backColor;
            padding = otherStyleToClone.padding;
            colorSaturation = otherStyleToClone.colorSaturation;
            grayscaleSaturation = otherStyleToClone.grayscaleSaturation;
            colorLightness = otherStyleToClone.colorLightness;
            grayscaleLightness = otherStyleToClone.grayscaleLightness;
        }

        #endregion

        #region Default values

        /// <summary>
        /// Gets the default value of the <see cref="Padding"/> property. Resolves to 0.08f.
        /// </summary>
        public static float DefaultPadding => 0.08f;

        /// <summary>
        /// Gets the default value of the <see cref="ColorLightness"/> property. Resolves to [0.4f, 0.8f].
        /// </summary>
        public static Range<float> DefaultColorLightness => Range.Create(0.4f, 0.8f);

        /// <summary>
        /// Gets the default value of the <see cref="GrayscaleLightness"/> property. Resolves to [0.3f, 0.9f].
        /// </summary>
        public static Range<float> DefaultGrayscaleLightness => Range.Create(0.3f, 0.9f);

        /// <exclude/>
        [Obsolete("Use DefaultColorSaturation instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static float DefaultSaturation => DefaultColorSaturation;

        /// <summary>
        /// Gets the default value of the <see cref="ColorSaturation"/> property. Resolves to 0.5f.
        /// </summary>
        public static float DefaultColorSaturation => 0.5f;

        /// <summary>
        /// Gets the default value of the <see cref="GrayscaleSaturation"/> property. Resolves to 0f.
        /// </summary>
        public static float DefaultGrayscaleSaturation => 0f;

        /// <summary>
        /// Gets the default value of the <see cref="BackColor"/> property. Resolves to <see cref="Color.White"/>.
        /// </summary>
        public static Color DefaultBackColor => Color.White;

        #endregion

        #region Style properties

        /// <summary>
        /// Gets or sets a collection of the allowed hues in the generated icons. If the collection is empty
        /// all hues are allowed.
        /// </summary>
        /// <remarks>
        /// This property always returns a collection, even after setting the property to <c>Nothing</c>.
        /// </remarks>
        public HueCollection Hues
        {
            get => hues;
            set => hues = value ?? new HueCollection();
        }

        private bool ShouldSerializeHues() => hues != null && hues.Count > 0;
        private void ResetHues() => Hues = new HueCollection();


        /// <summary>
        /// The background color of the icon. Set to <see cref="Color.Transparent"/> to remove the background.
        /// Default value is white.
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        private bool ShouldSerializeBackColor() => BackColor != DefaultBackColor;
        private void ResetBackColor() => BackColor = DefaultBackColor;


        /// <summary>
        /// Gets or sets the padding between the edge of the image and the bounds of the rendered icon.
        /// The value is specified in percent in the range [0.0, 0.4]. Default value is 0.08f.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Value was less than 0f or greater than 0.4f.</exception>
        public float Padding
        {
            get { return padding; }
            set
            {
                if (value < 0f || value > 0.4f)
                {
                    throw new ArgumentOutOfRangeException(nameof(Padding),
                        value, "Only padding values in the range [0.0, 0.4] are valid.");
                }
                padding = value;
            }
        }

        private bool ShouldSerializePadding() => Padding != DefaultPadding;
        private void ResetPadding() => Padding = DefaultPadding;


        /// <exclude/>
        [Obsolete("Use ColorSaturation instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#if HAVE_EXTENDED_COMPONENTMODEL
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public float Saturation
        {
            get { return ColorSaturation; }
            set { ColorSaturation = value; }
        }


        /// <summary>
        /// The saturation of the colored shapes in the icon. Valid values are [0.0f, 1.0f]. Default value is 0.05f.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than 0.0f or greater than 1.0f.</exception>
        /// <remarks>
        /// This property was previously called <c>Saturation</c> but was renamed in version 2.1.0.
        /// </remarks>
        public float ColorSaturation
        {
            get { return colorSaturation; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(ColorSaturation), value,
                        "Only saturation values in the range [0.0, 1.0] are allowed.");
                }

                colorSaturation = value;
            }
        }

        private bool ShouldSerializeColorSaturation() => ColorSaturation != DefaultColorSaturation;
        private void ResetColorSaturation() => ColorSaturation = DefaultColorSaturation;


        /// <summary>
        /// The saturation of the by default grayscale shapes in the icon. The same hue is used for colored and grayscale shapes. 
        /// Valid values are [0.0f, 1.0f]. Default value is 0f.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than 0.0f or greater than 1.0f.</exception>
        /// <remarks>
        /// This property is available since version 2.1.0.
        /// </remarks>
        public float GrayscaleSaturation
        {
            get { return grayscaleSaturation; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(GrayscaleSaturation), value,
                        "Only saturation values in the range [0.0, 1.0] are allowed.");
                }

                grayscaleSaturation = value;
            }
        }

        private bool ShouldSerializeGrayscaleSaturation() => GrayscaleSaturation != DefaultGrayscaleSaturation;
        private void ResetGrayscaleSaturation() => GrayscaleSaturation = DefaultGrayscaleSaturation;


        /// <summary>
        /// The lightness range of the colored shapes in the icon. Default value is 0.4f - 0.8f.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The lightness of the shapes can be inverted by specifying a range where <see cref="Range{TValue}.From"/> 
        /// is greater than <see cref="Range{TValue}.To"/>. The normally darker shapes will then be lighter than 
        /// the normally lighter ones.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or both range bounds were less than 0.0f or greater than 1.0f.
        /// </exception>
        public Range<float> ColorLightness
        {
            get { return colorLightness; }
            set
            {
                if (value.From < 0 || value.From > 1)
                {
                    throw new ArgumentOutOfRangeException("ColorLightness.From", value.From,
                        "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                if (value.To < 0 || value.To > 1)
                {
                    throw new ArgumentOutOfRangeException("ColorLightness.To", value.To,
                        "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                colorLightness = value;
            }
        }

        private bool ShouldSerializeColorLightness() => ColorLightness != DefaultColorLightness;
        private void ResetColorLightness() => ColorLightness = DefaultColorLightness;


        /// <summary>
        /// The lightness range of the grayscale shapes in the icon. Default value is 0.3f - 0.9f.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The lightness of the shapes can be inverted by specifying a range where <see cref="Range{TValue}.From"/> 
        /// is greater than <see cref="Range{TValue}.To"/>. The normally darker shapes will then be lighter than 
        /// the normally lighter ones.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or both range bounds were less than 0.0f or greater than 1.0f.
        /// </exception>
        public Range<float> GrayscaleLightness
        {
            get { return grayscaleLightness; }
            set
            {
                if (value.From < 0 || value.From > 1)
                {
                    throw new ArgumentOutOfRangeException("GrayscaleLightness.From", value.From, "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                if (value.To < 0 || value.To > 1)
                {
                    throw new ArgumentOutOfRangeException("GrayscaleLightness.To", value.To, "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                grayscaleLightness = value;
            }
        }

        private bool ShouldSerializeGrayscaleLightness() => GrayscaleLightness != DefaultGrayscaleLightness;
        private void ResetGrayscaleLightness() => GrayscaleLightness = DefaultGrayscaleLightness;

#endregion

        /// <summary>
        /// Gets a hash code for this <see cref="IdenticonStyle"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return
                padding.GetHashCode() ^
                colorSaturation.GetHashCode() ^
                grayscaleSaturation.GetHashCode() ^
                backColor.GetHashCode() ^
                colorLightness.GetHashCode() ^
                grayscaleLightness.GetHashCode();
        }

        /// <summary>
        /// Checks if this style is identical to another style.
        /// </summary>
        /// <param name="obj">The <see cref="IdenticonStyle"/> to compare.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as IdenticonStyle);
        }

        /// <summary>
        /// Checks if this style is identical to another style.
        /// </summary>
        /// <param name="other">The <see cref="IdenticonStyle"/> to compare.</param>
        public bool Equals(IdenticonStyle? other)
        {
            return
                other != null &&
                other.padding == padding &&
                other.backColor == backColor &&
                other.colorLightness == colorLightness &&
                other.grayscaleLightness == grayscaleLightness &
                other.colorSaturation == colorSaturation &&
                other.grayscaleSaturation == grayscaleSaturation &&
                other.hues.Equals(hues);
        }

        /// <summary>
        /// Creates and returns a deep copy of this <see cref="IdenticonStyle"/>.
        /// </summary>
        public IdenticonStyle Clone()
        {
            return new IdenticonStyle(this);
        }
    }
}
