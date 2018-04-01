using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Jdenticon.Rendering
{
    partial struct Color
    {
        /// <summary>
        /// Tries to parse a string to a <see cref="Color"/> without throwing any exception upon failures.
        /// </summary>
        /// <param name="s">The string to be parsed as a <see cref="Color"/>.</param>
        /// <param name="result">The parsed color.</param>
        /// <returns><c>true</c> if <paramref name="s"/> could be parsed as a color, otherwise <c>false</c>.</returns>
        /// <remarks>
        /// <para>
        /// This method will parse all formats listed in the 
        /// <see href="https://www.w3.org/TR/2018/PR-css-color-3-20180315/#svg-color">CSS 3 color specification</see> 
        /// except system colors and the <c>currentColor</c> keyword.
        /// </para>
        /// <list type="table">
        ///     <title>Supported color formats.</title>
        ///     <listheader>
        ///         <term>Format</term>
        ///         <term>Example</term>
        ///         <term>Remarks</term>
        ///     </listheader>
        ///     <item>
        ///         <term>#RGB</term>
        ///         <term>#23f</term>
        ///         <term>Fully opaque color. Will be expanded to #2233ff.</term>
        ///     </item>
        ///     <item>
        ///         <term>#RGBA</term>
        ///         <term>#23fa</term>
        ///         <term>Short RGB color with alpha value. Will be expanded to #2233ffaa.</term>
        ///     </item>
        ///     <item>
        ///         <term>#RRGGBB</term>
        ///         <term>#2233ff</term>
        ///         <term>Fully opaque color.</term>
        ///     </item>
        ///     <item>
        ///         <term>#RRGGBBAA</term>
        ///         <term>#2233ffaa</term>
        ///         <term>RGB color with alpha value.</term>
        ///     </item>
        ///     <item>
        ///         <term>&lt;named-color&gt;</term>
        ///         <term>firebrick</term>
        ///         <term>See list of supported color keywords in <see href="https://www.w3.org/TR/2018/PR-css-color-3-20180315/#svg-color">CSS 3 color specification</see>.</term>
        ///     </item>
        ///     <item>
        ///         <term>'transparent'</term>
        ///         <term>transparent</term>
        ///         <term>Fully transparent black.</term>
        ///     </item>
        ///     <item>
        ///         <term>rgb(red, green, blue)</term>
        ///         <term>rgb(0, 128, 255)</term>
        ///         <term>Fully opaque RGB color in decimal format.</term>
        ///     </item>
        ///     <item>
        ///         <term>rgba(red, green, blue, alpha)</term>
        ///         <term>rgba(0, 128, 255, 0.5)</term>
        ///         <term>RGB color with alpha value. RGB components specified in decimal format and alpha as a floating point number in the range [0, 1].</term>
        ///     </item>
        ///     <item>
        ///         <term>hsl(hue, saturation, lightness)</term>
        ///         <term>hsl(120, 75%, 75%)</term>
        ///         <term>Fully opaque Hue-Saturation-Lightness color. Hue specified in degrees, saturation and lightness in percents.</term>
        ///     </item>
        ///     <item>
        ///         <term>hsla(hue, saturation, lightness, alpha)</term>
        ///         <term>hsla(120, 75%, 75%, 0.5)</term>
        ///         <term>Hue-Saturation-Lightness color with alpha value. Hue specified in degrees, saturation and lightness in percents.</term>
        ///     </item>
        /// </list>
        /// <para>
        /// In addition to the formats listed above some of the formats listed in the 
        /// <see href="https://www.w3.org/TR/css-color-4/">CSS Color Module Level 4 specification</see>
        /// are supported.
        /// </para>
        /// <note type="caution">
        ///     <para>
        ///     The following formats are considered experimental and can change at any time without
        ///     causing a major release of Jdenticon.
        ///     </para>
        /// </note>
        /// <list type="table">
        ///     <title>Experimental color formats.</title>
        ///     <listheader>
        ///         <term>Format</term>
        ///         <term>Example</term>
        ///         <term>Remarks</term>
        ///     </listheader>
        ///     <item>
        ///         <term>rgb(red, green, blue, alpha)</term>
        ///         <term>rgb(0, 128, 255, 0.5)</term>
        ///         <term>RGB color with alpha value. RGB components specified in decimal format and alpha as a floating point number in the range [0, 1].</term>
        ///     </item>
        ///     <item>
        ///         <term>hsl(hue, saturation, lightness, alpha)</term>
        ///         <term>hsl(120, 75%, 75%, 0.5)</term>
        ///         <term>Hue-Saturation-Lightness color with alpha value. Hue specified in degrees, saturation and lightness in percents.</term>
        ///     </item>
        ///     <item>
        ///         <term>hsl(hue, saturation, lightness, alpha)</term>
        ///         <term>hsl(120deg, 75%, 75%, 0.5)</term>
        ///         <term>Same as above, but with explicit unit for the hue - degrees, range: [0, 360].</term>
        ///     </item>
        ///     <item>
        ///         <term>hsl(hue, saturation, lightness, alpha)</term>
        ///         <term>hsl(120rad, 75%, 75%, 0.5)</term>
        ///         <term>Same as above, but with hue specified in radians, range: [0, 2π).</term>
        ///     </item>
        ///     <item>
        ///         <term>hsl(hue, saturation, lightness, alpha)</term>
        ///         <term>hsl(120turn, 75%, 75%, 0.5)</term>
        ///         <term>Same as above, but with hue specified in turns, range: [0, 1).</term>
        ///     </item>
        ///     <item>
        ///         <term>hsl(hue, saturation, lightness, alpha)</term>
        ///         <term>hsl(120grad, 75%, 75%, 0.5)</term>
        ///         <term>Same as above, but with hue specified in gradians, range: [0, 400).</term>
        ///     </item>
        ///     <item>
        ///         <term>hwb(hue, whiteness, blackness[, alpha])</term>
        ///         <term>hwb(120, 75%, 75%, 0.5)</term>
        ///         <term>Hue-Whiteness-Blackness color with optional alpha value. Hue specified in degress, or in any of the units available for hsl(), the whiteness and blackness in percent.</term>
        ///     </item>
        /// </list>
        /// </remarks>
        public static bool TryParse(string s, out Color result)
        {
            return TryParse(s, out result, false);
        }

        /// <inheritdoc cref="TryParse(string, out Color)" />
        /// <summary>
        /// Parse a string to a <see cref="Color"/>.
        /// </summary>
        /// <returns>The parsed <see cref="Color"/>.</returns>
        /// <exception cref="FormatException"><paramref name="s"/> could not be parsed as a color.</exception>
        public static Color Parse(string s)
        {
            TryParse(s, out var result, true);
            return result;
        }

        // The dictionary of named colors is placed in a nested class to ensure 
        // the dictionary is not created and populated until actually needed.
        private class NamedCssColors
        {
            // Named colors as defined in CSS Color Module Level 4.
            // https://www.w3.org/TR/css-color-4/#named-colors
            private static readonly Dictionary<string, uint> namedColors = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase)
            {
                { "transparent", 0u },
                { "antiquewhite", 0xFAEBD7FFu },
                { "aqua", 0x00FFFFFFu },
                { "aquamarine", 0x7FFFD4FFu },
                { "azure", 0xF0FFFFFFu },
                { "beige", 0xF5F5DCFFu },
                { "bisque", 0xFFE4C4FFu },
                { "black", 0x000000FFu },
                { "blanchedalmond", 0xFFEBCDFFu },
                { "blue", 0x0000FFFFu },
                { "blueviolet", 0x8A2BE2FFu },
                { "brown", 0xA52A2AFFu },
                { "burlywood", 0xDEB887FFu },
                { "cadetblue", 0x5F9EA0FFu },
                { "chartreuse", 0x7FFF00FFu },
                { "chocolate", 0xD2691EFFu },
                { "coral", 0xFF7F50FFu },
                { "cornflowerblue", 0x6495EDFFu },
                { "cornsilk", 0xFFF8DCFFu },
                { "crimson", 0xDC143CFFu },
                { "cyan", 0x00FFFFFFu },
                { "darkblue", 0x00008BFFu },
                { "darkcyan", 0x008B8BFFu },
                { "darkgoldenrod", 0xB8860BFFu },
                { "darkgray", 0xA9A9A9FFu },
                { "darkgreen", 0x006400FFu },
                { "darkgrey", 0xA9A9A9FFu },
                { "darkkhaki", 0xBDB76BFFu },
                { "darkmagenta", 0x8B008BFFu },
                { "darkolivegreen", 0x556B2FFFu },
                { "darkorange", 0xFF8C00FFu },
                { "darkorchid", 0x9932CCFFu },
                { "darkred", 0x8B0000FFu },
                { "darksalmon", 0xE9967AFFu },
                { "darkseagreen", 0x8FBC8FFFu },
                { "darkslateblue", 0x483D8BFFu },
                { "darkslategray", 0x2F4F4FFFu },
                { "darkslategrey", 0x2F4F4FFFu },
                { "darkturquoise", 0x00CED1FFu },
                { "darkviolet", 0x9400D3FFu },
                { "deeppink", 0xFF1493FFu },
                { "deepskyblue", 0x00BFFFFFu },
                { "dimgray", 0x696969FFu },
                { "dimgrey", 0x696969FFu },
                { "dodgerblue", 0x1E90FFFFu },
                { "firebrick", 0xB22222FFu },
                { "floralwhite", 0xFFFAF0FFu },
                { "forestgreen", 0x228B22FFu },
                { "fuchsia", 0xFF00FFFFu },
                { "gainsboro", 0xDCDCDCFFu },
                { "ghostwhite", 0xF8F8FFFFu },
                { "gold", 0xFFD700FFu },
                { "goldenrod", 0xDAA520FFu },
                { "gray", 0x808080FFu },
                { "green", 0x008000FFu },
                { "greenyellow", 0xADFF2FFFu },
                { "grey", 0x808080FFu },
                { "honeydew", 0xF0FFF0FFu },
                { "hotpink", 0xFF69B4FFu },
                { "indianred", 0xCD5C5CFFu },
                { "indigo", 0x4B0082FFu },
                { "ivory", 0xFFFFF0FFu },
                { "khaki", 0xF0E68CFFu },
                { "lavender", 0xE6E6FAFFu },
                { "lavenderblush", 0xFFF0F5FFu },
                { "lawngreen", 0x7CFC00FFu },
                { "lemonchiffon", 0xFFFACDFFu },
                { "lightblue", 0xADD8E6FFu },
                { "lightcoral", 0xF08080FFu },
                { "lightcyan", 0xE0FFFFFFu },
                { "lightgoldenrodyellow", 0xFAFAD2FFu },
                { "lightgray", 0xD3D3D3FFu },
                { "lightgreen", 0x90EE90FFu },
                { "lightgrey", 0xD3D3D3FFu },
                { "lightpink", 0xFFB6C1FFu },
                { "lightsalmon", 0xFFA07AFFu },
                { "lightseagreen", 0x20B2AAFFu },
                { "lightskyblue", 0x87CEFAFFu },
                { "lightslategray", 0x778899FFu },
                { "lightslategrey", 0x778899FFu },
                { "lightsteelblue", 0xB0C4DEFFu },
                { "lightyellow", 0xFFFFE0FFu },
                { "lime", 0x00FF00FFu },
                { "limegreen", 0x32CD32FFu },
                { "linen", 0xFAF0E6FFu },
                { "magenta", 0xFF00FFFFu },
                { "maroon", 0x800000FFu },
                { "mediumaquamarine", 0x66CDAAFFu },
                { "mediumblue", 0x0000CDFFu },
                { "mediumorchid", 0xBA55D3FFu },
                { "mediumpurple", 0x9370DBFFu },
                { "mediumseagreen", 0x3CB371FFu },
                { "mediumslateblue", 0x7B68EEFFu },
                { "mediumspringgreen", 0x00FA9AFFu },
                { "mediumturquoise", 0x48D1CCFFu },
                { "mediumvioletred", 0xC71585FFu },
                { "midnightblue", 0x191970FFu },
                { "mintcream", 0xF5FFFAFFu },
                { "mistyrose", 0xFFE4E1FFu },
                { "moccasin", 0xFFE4B5FFu },
                { "navajowhite", 0xFFDEADFFu },
                { "navy", 0x000080FFu },
                { "oldlace", 0xFDF5E6FFu },
                { "olive", 0x808000FFu },
                { "olivedrab", 0x6B8E23FFu },
                { "orange", 0xFFA500FFu },
                { "orangered", 0xFF4500FFu },
                { "orchid", 0xDA70D6FFu },
                { "palegoldenrod", 0xEEE8AAFFu },
                { "palegreen", 0x98FB98FFu },
                { "paleturquoise", 0xAFEEEEFFu },
                { "palevioletred", 0xDB7093FFu },
                { "papayawhip", 0xFFEFD5FFu },
                { "peachpuff", 0xFFDAB9FFu },
                { "peru", 0xCD853FFFu },
                { "pink", 0xFFC0CBFFu },
                { "plum", 0xDDA0DDFFu },
                { "powderblue", 0xB0E0E6FFu },
                { "purple", 0x800080FFu },
                { "rebeccapurple", 0x663399FFu },
                { "red", 0xFF0000FFu },
                { "rosybrown", 0xBC8F8FFFu },
                { "royalblue", 0x4169E1FFu },
                { "saddlebrown", 0x8B4513FFu },
                { "salmon", 0xFA8072FFu },
                { "sandybrown", 0xF4A460FFu },
                { "seagreen", 0x2E8B57FFu },
                { "seashell", 0xFFF5EEFFu },
                { "sienna", 0xA0522DFFu },
                { "silver", 0xC0C0C0FFu },
                { "skyblue", 0x87CEEBFFu },
                { "slateblue", 0x6A5ACDFFu },
                { "slategray", 0x708090FFu },
                { "slategrey", 0x708090FFu },
                { "snow", 0xFFFAFAFFu },
                { "springgreen", 0x00FF7FFFu },
                { "steelblue", 0x4682B4FFu },
                { "tan", 0xD2B48CFFu },
                { "teal", 0x008080FFu },
                { "thistle", 0xD8BFD8FFu },
                { "tomato", 0xFF6347FFu },
                { "turquoise", 0x40E0D0FFu },
                { "violet", 0xEE82EEFFu },
                { "wheat", 0xF5DEB3FFu },
                { "white", 0xFFFFFFFFu },
                { "whitesmoke", 0xF5F5F5FFu },
                { "yellow", 0xFFFF00FFu },
                { "yellowgreen", 0x9ACD32FFu }
            };

            public static bool TryParse(string input, out Color result)
            {
                if (namedColors.TryGetValue(input, out var namedColor))
                {
                    result = new Color(namedColor);
                    return true;
                }

                result = new Color();
                return false;
            }
        }
        
        private static bool TryParse(string input, out Color result, bool throwOnError)
        {
            // Named colors as defined in CSS Color Module Level 4.
            // https://www.w3.org/TR/css-color-4/#named-colors
            if (NamedCssColors.TryParse(input, out result))
            {
                return true;
            }

            // Hexadecimal colors on the formats:
            // * #RGB
            // * #RGBA
            // * #RRGGBB
            // * #RRGGBBAA
            var hex = Regex.Match(input, "^#?[0-9a-fA-F]+$");
            if (hex.Success)
            {
                if (TryParseHexColor(input, out result))
                {
                    return true;
                }

                if (throwOnError)
                {
                    throw new FormatException($"'{input}' is not a valid hexadecimal color.");
                }
                return false;
            }

            // RGB colors on the formats:
            // * rgb(127, 255, 0)
            // * rgb(127, 255, 0, 0.5)
            // * rgba(127, 255, 0, 0.5)
            // * rgba(50%, 100%, 0%, 0.5)
            var rgba = Regex.Match(input, @"^(rgba?)\(([^,]+),([^,]+),([^,]+)(?:,([^,]+))?\)$");
            if (rgba.Success)
            {
                if (TryParseRgbComponent(rgba.Groups[2].Value, out var red) &&
                    TryParseRgbComponent(rgba.Groups[3].Value, out var green) &&
                    TryParseRgbComponent(rgba.Groups[4].Value, out var blue) &&
                    TryParseIntegerAlpha(rgba.Groups[5].Value, out var alpha))
                {
                    result = new Color(alpha, red, green, blue);
                    return true;
                }

                if (throwOnError)
                {
                    throw new FormatException($"'{input}' is not a valid {rgba.Groups[1].Value}() color.");
                }
                return false;
            }

            // HSL (hue-saturation-lightness) colors on the formats:
            // * hsl(0, 100%, 50%)
            // * hsl(0, 100%, 50%, 0.5)
            // * hsla(0, 100%, 50%, 0.5)
            var hsla = Regex.Match(input, @"^(hsla?)\(([^,]+),([^,]+),([^,]+)(?:,([^,]+))?\)$");
            if (hsla.Success)
            {
                if (TryParseHue(hsla.Groups[2].Value, out var hue) &&
                    TryParsePercent(hsla.Groups[3].Value, out var saturation) &&
                    TryParsePercent(hsla.Groups[4].Value, out var lightness) &&
                    TryParseAlpha(hsla.Groups[5].Value, out var alpha))
                {
                    result = FromHsl(hue, saturation, lightness, alpha);
                    return true;
                }

                if (throwOnError)
                {
                    throw new FormatException($"'{input}' is not a valid ${hsla.Groups[1].Value}() color.");
                }
                return false;
            }

            // HWB (hue-white-black) colors on the formats:
            // * hwb(0, 100%, 50%)
            // * hwb(0, 100%, 50%, 0.5)
            var hwb = Regex.Match(input, @"^hwb\(([^,]+),([^,]+),([^,]+)(?:,([^,]+))?\)$");
            if (hwb.Success)
            {
                if (TryParseHue(hwb.Groups[1].Value, out var hue) &&
                    TryParsePercent(hwb.Groups[2].Value, out var white) &&
                    TryParsePercent(hwb.Groups[3].Value, out var black) &&
                    TryParseAlpha(hwb.Groups[4].Value, out var alpha))
                {
                    result = FromHwb(hue, white, black, alpha);
                    return true;
                }

                if (throwOnError)
                {
                    throw new FormatException($"'{input}' is not a valid hwb() color.");
                }
                return false;
            }

            if (throwOnError)
            {
                throw new FormatException(Regex.IsMatch(input, "^[a-zA-Z]+$") ?
                    $"'{input}' is not a known named color." :
                    $"'{input}' is not a valid color.");
            }
            
            return false;
        }

        /// <summary>
        /// Output in range [0, 1]
        /// </summary>
        private static bool TryParsePercent(string input, out float result)
        {
            // Detect and remove percent sign
            var percentFound = false;
            input = Regex.Replace(input, "%\\s*$", m =>
            {
                percentFound = true;
                return "";
            });
            
            if (!percentFound ||
                !float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                result = 0;
                return false;
            }

            result *= 0.01f;

            if (result < 0) result = 0;
            else if (result > 1) result = 1;

            return true;
        }

        /// <summary>
        /// Output in range [0, 255]
        /// </summary>
        private static bool TryParseIntegerAlpha(string input, out int result)
        {
            if (TryParseAlpha(input, out var alpha))
            {
                result = (int)(255 * alpha);
                return true;
            }

            result = 0;
            return false;
        }

        /// <summary>
        /// Output in range [0, 1]
        /// </summary>
        private static bool TryParseAlpha(string input, out float result)
        {
            // No alpha should be treated as fully opaque
            if (string.IsNullOrEmpty(input))
            {
                result = 1f;
                return true;
            }

            // Detect and remove percent sign
            var isPercent = false;
            var percentMatch = Regex.Match(input, @"%\s*$");
            if (percentMatch.Success)
            {
                input = input.Substring(0, percentMatch.Index);
                isPercent = true;
            }

            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                result = 0;
                return false;
            }

            // Convert percent to range [0, 1]
            if (isPercent)
            {
                result *= 0.01f;
            }

            // Truncate to range [0, 1]
            if (result < 0) result = 0;
            else if (result > 1) result = 1;

            return true;
        }

        /// <summary>
        /// Output in range [0, 255]
        /// </summary>
        private static bool TryParseRgbComponent(string input, out int result)
        {
            // Detect and remove percent sign
            var isPercent = false;
            var percentMatch = Regex.Match(input, "%\\s*$");
            if (percentMatch.Success)
            {
                input = input.Substring(0, percentMatch.Index);
                isPercent = true;
            }

            // Decimals are allowed (see https://www.w3.org/TR/css3-values/#number-value)
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var numeric))
            {
                result = 0;
                return false;
            }

            // Convert percents to [0, 255]
            if (isPercent)
            {
                numeric = numeric * 2.55f;
            }

            // Truncate value to range [0, 255]
            result = (int)numeric;
            if (result < 0) result = 0;
            else if (result > 255) result = 255;

            return true;
        }

        /// <summary>
        /// Output in range [0, 1]
        /// </summary>
        private static bool TryParseHue(string input, out float result)
        {
            var unit = "deg";

            // Find and remove unit
            input = Regex.Replace(input, "(deg|grad|rad|turn)\\s*$", m =>
            {
                unit = m.Groups[1].Value.ToLowerInvariant();
                return "";
            }, RegexOptions.IgnoreCase);

            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                result = 0;
                return false;
            }
            
            // Convert to turns
            switch (unit)
            {
                case "grad":
                    // Gradians: range 0 - 400
                    result = result / 400f;
                    break;
                case "rad":
                    // Radians: range 0 - 2π
                    result = result / (2 * (float)Math.PI);
                    break;
                case "turn":
                    // Turns: range 0 - 1
                    break;
                default:
                    // Degree: range 0 - 360
                    result = result / 360f;
                    break;
            }

            // Normalize to range [0, 1]
            result = (result % 1);
            if (result < 0)
            {
                result += 1;
            }

            return true;
        }
        
        private static bool TryParseHexColor(string input, out Color result)
        {
            if (input.Length < 10)
            {
                // Remove leading #
                if (input[0] == '#')
                {
                    input = input.Substring(1);
                }

                if (uint.TryParse(input, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var numeric))
                {
                    switch (input.Length)
                    {
                        // RGB
                        case 3:
                            result = new Color(
                                ((numeric & 0xf00) << 20) |
                                ((numeric & 0xf00) << 16) |
                                ((numeric & 0x0f0) << 16) |
                                ((numeric & 0x0f0) << 12) |
                                ((numeric & 0x00f) << 12) |
                                ((numeric & 0x00f) << 8) |
                                0xffu);
                            return true;

                        // RGBA
                        case 4:
                            result = new Color(
                                ((numeric & 0xf000) << 16) |
                                ((numeric & 0xf000) << 12) |
                                ((numeric & 0x0f00) << 12) |
                                ((numeric & 0x0f00) << 8) |
                                ((numeric & 0x00f0) << 8) |
                                ((numeric & 0x00f0) << 4) |
                                ((numeric & 0x000f) << 4) |
                                ((numeric & 0x000f)));
                            return true;

                        // RRGGBB
                        case 6:
                            result = new Color((numeric << 8) | 0xffu);
                            return true;

                        // RRGGBBAA
                        case 8:
                            result = new Color(numeric);
                            return true;
                    }
                }
            }
            
            result = new Color();
            return false;
        }
    }
}
