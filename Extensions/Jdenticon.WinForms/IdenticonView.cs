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
using Jdenticon.WebForms.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JdenticonColor = Jdenticon.Rendering.Color;

namespace Jdenticon.WinForms
{
    /// <summary>
    /// Renders an identicon in a Windows Forms application.
    /// </summary>
    /// <remarks>
    /// The icon is initialized with the default style specified by <see cref="Identicon.DefaultStyle"/>. Changes to the 
    /// default style after the control has been created will not affect the rendered icon.
    /// </remarks>
    [DefaultProperty(nameof(Value))]
    [DefaultBindingProperty(nameof(Value))]
    [Description("Renders an identicon using Jdenticon.")]
    public partial class IdenticonView : Control
    {
        private object value;
        private byte[] hash;
        private IdenticonStyle style;
        private HueString hues;

        private const string CatAppearance = "Appearance";
        private const string CatData = "Data";
        private const string CatPropertyChanged = "Property Changed";

        private static readonly object EventValueChanged = new object();
        private static readonly object EventHashChanged = new object();
        private static readonly object EventHuesChanged = new object();
        private static readonly object EventGrayscaleLightnessChanged = new object();
        private static readonly object EventColorSaturationChanged = new object();
        private static readonly object EventGrayscaleSaturationChanged = new object();
        private static readonly object EventColorLightnessChanged = new object();

        /// <summary>
        /// Creates a new instance of <see cref="IdenticonView"/>.
        /// </summary>
        public IdenticonView()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // Initialize with default style
            style = Identicon.DefaultStyle.Clone();
            hues = new HueString(style.Hues);
        }

        /// <summary>
        /// Gets or sets the <see cref="IconGenerator"/> that is responsible for generating the icon patterns.
        /// This property is <c>null</c> when the default generator is used.
        /// </summary>
        [DefaultValue(null)]
        [Browsable(false)]
        public IconGenerator IconGenerator { get; set; }

        /// <summary>
        /// Gets or sets a value that the generated identicon will be based upon. A string representation of the value will be hashed using SHA1.
        /// </summary>
        /// <remarks>
        /// Either <see cref="Value"/> or <see cref="Hash"/> can be specified. The last specified hash or value wins.
        /// </remarks>
        [Bindable(true)]
        [Category(CatData)]
        [Description("Value whose string representation the icon will be based upon.")]
        [TypeConverter(typeof(StringConverter))]
        [DefaultValue(null)]
        public object Value
        {
            get { return value; }
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    hash = null;
                    OnHashChanged(EventArgs.Empty);
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised when the value of the <see cref="Value"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(Value))]
        public event EventHandler ValueChanged
        {
            add => Events.AddHandler(EventValueChanged, value);
            remove => Events.RemoveHandler(EventValueChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            (Events[EventValueChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <summary>
        /// Gets or sets a hash that the generated identicon will be based upon.
        /// The hash must contain at least 6 bytes.
        /// </summary>
        /// <exception cref="ArgumentException">Value contains less than 6 bytes.</exception>
        /// <remarks>
        /// Either <see cref="Value"/> or <see cref="Hash"/> can be specified. The last specified hash or value wins.
        /// </remarks>
        [Bindable(true)]
        [Category(CatData)]
        [Description("Hash that the icon will be based upon.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public byte[] Hash
        {
            get
            {
                if (hash == null) hash = HashGenerator.ComputeHash(value, "SHA1");
                return hash;
            }
            set
            {
                if (value != null && value.Length < 6)
                {
                    throw new ArgumentException("The hash must contain at least 6 bytes.", nameof(Hash));
                }

                var nonNullHash = value ?? HashGenerator.ComputeHash("", "SHA1");

                if (nonNullHash != hash)
                {
                    hash = nonNullHash;
                    OnHashChanged(EventArgs.Empty);

                    if (this.value != null)
                    {
                        this.value = null;
                        OnValueChanged(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Raised when the value of the <see cref="Hash"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(Hash))]
        public event EventHandler HashChanged
        {
            add => Events.AddHandler(EventHashChanged, value);
            remove => Events.RemoveHandler(EventHashChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="HashChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnHashChanged(EventArgs e)
        {
            (Events[EventHashChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <summary>
        /// Gets or sets an <see cref="IdenticonStyle"/> that will affect the appearance of the generated icon.
        /// </summary>
        /// <remarks>
        /// This property will return a copy of the <see cref="IdenticonStyle"/> currently in use. Changes to the returned instance
        /// will not affect the rendering of this icon, unless the property is set with the updated <see cref="IdenticonStyle"/> instance.
        /// </remarks>
        [Category(CatAppearance)]
        [Description("Affects the appearance of the generated icon.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IdenticonStyle Style
        {
            get
            {
                // Return a copy to avoid that updates are made to the IdenticonStyle without triggering a render
                var style = this.style.Clone();
                style.Hues = hues?.ToCollection();
                return style;
            }
            set
            {
                var style = value ?? new IdenticonStyle();
                BackColor = style.BackColor.ToGdi();
                ColorLightness = style.ColorLightness;
                ColorSaturation = style.ColorSaturation;
                GrayscaleLightness = style.GrayscaleLightness;
                GrayscaleSaturation = style.GrayscaleSaturation;
                Hues = new HueString(style.Hues);
                Padding = style.Padding;
            }
        }


        /// <summary>
        /// Gets or sets a string containing one or multiple hues, that the identicon will be limited to use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that when setting this property in the form designer, the values are localized. The string should contain a list of hues, 
        /// separated by commas, unless your operating system languge use comma as decimal separator. In those cases semicolon is used as list separator.
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
        /// </remarks>
        [Category(CatAppearance)]
        [Description("Limit the icon to a set of specific hues.")]
        public HueString Hues
        {
            get { return hues ?? HueString.Empty; }
            set
            {
                if (value != hues)
                {
                    hues = value;
                    OnHuesChanged(EventArgs.Empty);
                }
            }
        }

        private bool ShouldSerializeHues() => hues != null && hues != HueString.Empty;
        private void ResetHues() => Hues = null;

        /// <summary>
        /// Raised when the value of the <see cref="Hues"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(Hues))]
        public event EventHandler HuesChanged
        {
            add => Events.AddHandler(EventHuesChanged, value);
            remove => Events.RemoveHandler(EventHuesChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="HuesChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnHuesChanged(EventArgs e)
        {
            (Events[EventHuesChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <inheritdoc cref="IdenticonStyle.ColorLightness"/>
        [Category(CatAppearance)]
        [Description("Lightness range of the colored shapes in the icon. Specified as two decimal values in the range [0.0, 1.0].")]
        public Range<float> ColorLightness
        {
            get { return style.ColorLightness; }
            set
            {
                if (value != style.ColorLightness)
                {
                    style.ColorLightness = value;
                    OnColorLightnessChanged(EventArgs.Empty);
                }
            }
        }

        private bool ShouldSerializeColorLightness() => ColorLightness != IdenticonStyle.DefaultColorLightness;
        private void ResetColorLightness() => ColorLightness = IdenticonStyle.DefaultColorLightness;

        /// <summary>
        /// Raised when the value of the <see cref="ColorLightness"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(ColorLightness))]
        public event EventHandler ColorLightnessChanged
        {
            add => Events.AddHandler(EventColorLightnessChanged, value);
            remove => Events.RemoveHandler(EventColorLightnessChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="ColorLightnessChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnColorLightnessChanged(EventArgs e)
        {
            (Events[EventColorLightnessChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <inheritdoc cref="IdenticonStyle.GrayscaleLightness"/>
        [Category(CatAppearance)]
        [Description("Lightness range of the grayscale shapes in the icon. Specified as two decimal values in the range [0.0, 1.0].")]
        public Range<float> GrayscaleLightness
        {
            get { return style.GrayscaleLightness; }
            set
            {
                if (value != style.GrayscaleLightness)
                {
                    style.GrayscaleLightness = value;
                    OnGrayscaleLightnessChanged(EventArgs.Empty);
                }
            }
        }

        private bool ShouldSerializeGrayscaleLightness() => GrayscaleLightness != IdenticonStyle.DefaultGrayscaleLightness;
        private void ResetGrayscaleLightness() => GrayscaleLightness = IdenticonStyle.DefaultGrayscaleLightness;

        /// <summary>
        /// Raised when the value of the <see cref="GrayscaleLightness"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(GrayscaleLightness))]
        public event EventHandler GrayscaleLightnessChanged
        {
            add => Events.AddHandler(EventGrayscaleLightnessChanged, value);
            remove => Events.RemoveHandler(EventGrayscaleLightnessChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="GrayscaleLightnessChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnGrayscaleLightnessChanged(EventArgs e)
        {
            (Events[EventGrayscaleLightnessChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <inheritdoc cref="IdenticonStyle.GrayscaleSaturation"/>
        [Category(CatAppearance)]
        [Description("Saturation of the grayscale shapes in the icon. Specified as a value in the range [0.0, 1.0].")]
        public float GrayscaleSaturation
        {
            get { return style.GrayscaleSaturation; }
            set
            {
                if (value != style.GrayscaleSaturation)
                {
                    style.GrayscaleSaturation = value;
                    OnGrayscaleSaturationChanged(EventArgs.Empty);
                }
            }
        }

        private bool ShouldSerializeGrayscaleSaturation() => GrayscaleSaturation != IdenticonStyle.DefaultGrayscaleSaturation;
        private void ResetGrayscaleSaturation() => GrayscaleSaturation = IdenticonStyle.DefaultGrayscaleSaturation;

        /// <summary>
        /// Raised when the value of the <see cref="GrayscaleSaturation"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(GrayscaleSaturation))]
        public event EventHandler GrayscaleSaturationChanged
        {
            add => Events.AddHandler(EventGrayscaleSaturationChanged, value);
            remove => Events.RemoveHandler(EventGrayscaleSaturationChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="GrayscaleSaturationChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnGrayscaleSaturationChanged(EventArgs e)
        {
            (Events[EventGrayscaleSaturationChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <inheritdoc cref="IdenticonStyle.ColorSaturation"/>
        [Category(CatAppearance)]
        [Description("Saturation of the colored shapes in the icon. Specified as a value in the range [0.0, 1.0].")]
        public float ColorSaturation
        {
            get { return style.ColorSaturation; }
            set
            {
                if (value != style.ColorSaturation)
                {
                    style.ColorSaturation = value;
                    OnColorSaturationChanged(EventArgs.Empty);
                }
            }
        }

        private bool ShouldSerializeColorSaturation() => ColorSaturation != IdenticonStyle.DefaultColorSaturation;
        private void ResetColorSaturation() => ColorSaturation = IdenticonStyle.DefaultColorSaturation;

        /// <summary>
        /// Raised when the value of the <see cref="ColorSaturation"/> property changes.
        /// </summary>
        [Category(CatPropertyChanged)]
        [ChangedEventDescription(nameof(ColorSaturation))]
        public event EventHandler ColorSaturationChanged
        {
            add => Events.AddHandler(EventColorSaturationChanged, value);
            remove => Events.RemoveHandler(EventColorSaturationChanged, value);
        }

        /// <summary>
        /// Raises the <see cref="ColorSaturationChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnColorSaturationChanged(EventArgs e)
        {
            (Events[EventColorSaturationChanged] as EventHandler)?.Invoke(this, e);
            Invalidate();
        }


        /// <inheritdoc cref="IdenticonStyle.Padding"/>
        [Category(CatAppearance)]
        [Description("Padding between the icon outer bounds and the icon content. Specified as percents in the range [0.0, 0.4].")]
        public new float Padding
        {
            get { return style.Padding; }
            set
            {
                if (value != style.Padding)
                {
                    style.Padding = value;
                    OnPaddingChanged(EventArgs.Empty);
                }
            }
        }

        private bool ShouldSerializePadding() => Padding != IdenticonStyle.DefaultPadding;
        private void ResetPadding() => Padding = IdenticonStyle.DefaultPadding;


        /// <summary>
        /// Gets or sets the background of the icon.
        /// </summary>
        [Category(CatAppearance)]
        [Description("Background color of the icon.")]
        public override System.Drawing.Color BackColor
        {
            get { return style.BackColor.ToGdi(); }
            set
            {
                var jdenticonColor = value.ToJdenticon();

                if (jdenticonColor != style.BackColor)
                {
                    style.BackColor = jdenticonColor;
                    OnBackColorChanged(EventArgs.Empty);
                }
            }
        }
        
        private bool ShouldSerializeBackColor() => BackColor != IdenticonStyle.DefaultBackColor.ToGdi();
        /// <exclude/>
        public override void ResetBackColor() => BackColor = IdenticonStyle.DefaultBackColor.ToGdi();

        /// <exclude/>
        protected override Size DefaultSize => new Size(100, 100);

        /// <exclude/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;

            if (rect.Width > 0 && rect.Height > 0)
            {
                var icon = Identicon.FromHash(Hash, Math.Min(rect.Width, rect.Height));
                icon.Style = Style;
                icon.Style.BackColor = JdenticonColor.Transparent; // The background is painted by WinForms
                icon.IconGenerator = IconGenerator;
                icon.Draw(e.Graphics, rect);
            }
        }
    }
}
