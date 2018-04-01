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
using System.Text;

namespace Jdenticon.Rendering
{
    partial struct Color
    {
        #region Colors

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00000000.
        /// </summary>
        public static Color Transparent => new Color(0x00000000u);

        #endregion

        #region X11 colors

        // This list of colors is based on the X11 colors defined in
        // https://cgit.freedesktop.org/xorg/app/rgb/tree/rgb.txt

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFAFAFF.
        /// </summary>
        public static Color Snow => new Color(0xFFFAFAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F8F8FFFF.
        /// </summary>
        public static Color GhostWhite => new Color(0xF8F8FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F5F5F5FF.
        /// </summary>
        public static Color WhiteSmoke => new Color(0xF5F5F5FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DCDCDCFF.
        /// </summary>
        public static Color Gainsboro => new Color(0xDCDCDCFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFAF0FF.
        /// </summary>
        public static Color FloralWhite => new Color(0xFFFAF0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FDF5E6FF.
        /// </summary>
        public static Color OldLace => new Color(0xFDF5E6FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FAF0E6FF.
        /// </summary>
        public static Color Linen => new Color(0xFAF0E6FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FAEBD7FF.
        /// </summary>
        public static Color AntiqueWhite => new Color(0xFAEBD7FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFEFD5FF.
        /// </summary>
        public static Color PapayaWhip => new Color(0xFFEFD5FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFEBCDFF.
        /// </summary>
        public static Color BlanchedAlmond => new Color(0xFFEBCDFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFE4C4FF.
        /// </summary>
        public static Color Bisque => new Color(0xFFE4C4FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFDAB9FF.
        /// </summary>
        public static Color PeachPuff => new Color(0xFFDAB9FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFDEADFF.
        /// </summary>
        public static Color NavajoWhite => new Color(0xFFDEADFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFE4B5FF.
        /// </summary>
        public static Color Moccasin => new Color(0xFFE4B5FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFF8DCFF.
        /// </summary>
        public static Color Cornsilk => new Color(0xFFF8DCFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFFF0FF.
        /// </summary>
        public static Color Ivory => new Color(0xFFFFF0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFACDFF.
        /// </summary>
        public static Color LemonChiffon => new Color(0xFFFACDFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFF5EEFF.
        /// </summary>
        public static Color Seashell => new Color(0xFFF5EEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F0FFF0FF.
        /// </summary>
        public static Color Honeydew => new Color(0xF0FFF0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F5FFFAFF.
        /// </summary>
        public static Color MintCream => new Color(0xF5FFFAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F0FFFFFF.
        /// </summary>
        public static Color Azure => new Color(0xF0FFFFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F0F8FFFF.
        /// </summary>
        public static Color AliceBlue => new Color(0xF0F8FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #E6E6FAFF.
        /// </summary>
        public static Color Lavender => new Color(0xE6E6FAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFF0F5FF.
        /// </summary>
        public static Color LavenderBlush => new Color(0xFFF0F5FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFE4E1FF.
        /// </summary>
        public static Color MistyRose => new Color(0xFFE4E1FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFFFFFF.
        /// </summary>
        public static Color White => new Color(0xFFFFFFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #000000FF.
        /// </summary>
        public static Color Black => new Color(0x000000FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #2F4F4FFF.
        /// </summary>
        public static Color DarkSlateGray => new Color(0x2F4F4FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #2F4F4FFF.
        /// </summary>
        public static Color DarkSlateGrey => new Color(0x2F4F4FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #696969FF.
        /// </summary>
        public static Color DimGray => new Color(0x696969FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #696969FF.
        /// </summary>
        public static Color DimGrey => new Color(0x696969FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #708090FF.
        /// </summary>
        public static Color SlateGray => new Color(0x708090FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #708090FF.
        /// </summary>
        public static Color SlateGrey => new Color(0x708090FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #778899FF.
        /// </summary>
        public static Color LightSlateGray => new Color(0x778899FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #778899FF.
        /// </summary>
        public static Color LightSlateGrey => new Color(0x778899FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #BEBEBEFF.
        /// </summary>
        public static Color Gray => new Color(0xBEBEBEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #BEBEBEFF.
        /// </summary>
        public static Color Grey => new Color(0xBEBEBEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #808080FF.
        /// </summary>
        public static Color WebGray => new Color(0x808080FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #808080FF.
        /// </summary>
        public static Color WebGrey => new Color(0x808080FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #D3D3D3FF.
        /// </summary>
        public static Color LightGrey => new Color(0xD3D3D3FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #D3D3D3FF.
        /// </summary>
        public static Color LightGray => new Color(0xD3D3D3FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #191970FF.
        /// </summary>
        public static Color MidnightBlue => new Color(0x191970FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #000080FF.
        /// </summary>
        public static Color Navy => new Color(0x000080FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #000080FF.
        /// </summary>
        public static Color NavyBlue => new Color(0x000080FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #6495EDFF.
        /// </summary>
        public static Color CornflowerBlue => new Color(0x6495EDFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #483D8BFF.
        /// </summary>
        public static Color DarkSlateBlue => new Color(0x483D8BFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #6A5ACDFF.
        /// </summary>
        public static Color SlateBlue => new Color(0x6A5ACDFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #7B68EEFF.
        /// </summary>
        public static Color MediumSlateBlue => new Color(0x7B68EEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #8470FFFF.
        /// </summary>
        public static Color LightSlateBlue => new Color(0x8470FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #0000CDFF.
        /// </summary>
        public static Color MediumBlue => new Color(0x0000CDFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #4169E1FF.
        /// </summary>
        public static Color RoyalBlue => new Color(0x4169E1FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #0000FFFF.
        /// </summary>
        public static Color Blue => new Color(0x0000FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #1E90FFFF.
        /// </summary>
        public static Color DodgerBlue => new Color(0x1E90FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00BFFFFF.
        /// </summary>
        public static Color DeepSkyBlue => new Color(0x00BFFFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #87CEEBFF.
        /// </summary>
        public static Color SkyBlue => new Color(0x87CEEBFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #87CEFAFF.
        /// </summary>
        public static Color LightSkyBlue => new Color(0x87CEFAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #4682B4FF.
        /// </summary>
        public static Color SteelBlue => new Color(0x4682B4FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #B0C4DEFF.
        /// </summary>
        public static Color LightSteelBlue => new Color(0xB0C4DEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #ADD8E6FF.
        /// </summary>
        public static Color LightBlue => new Color(0xADD8E6FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #B0E0E6FF.
        /// </summary>
        public static Color PowderBlue => new Color(0xB0E0E6FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #AFEEEEFF.
        /// </summary>
        public static Color PaleTurquoise => new Color(0xAFEEEEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00CED1FF.
        /// </summary>
        public static Color DarkTurquoise => new Color(0x00CED1FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #48D1CCFF.
        /// </summary>
        public static Color MediumTurquoise => new Color(0x48D1CCFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #40E0D0FF.
        /// </summary>
        public static Color Turquoise => new Color(0x40E0D0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00FFFFFF.
        /// </summary>
        public static Color Cyan => new Color(0x00FFFFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00FFFFFF.
        /// </summary>
        public static Color Aqua => new Color(0x00FFFFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #E0FFFFFF.
        /// </summary>
        public static Color LightCyan => new Color(0xE0FFFFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #5F9EA0FF.
        /// </summary>
        public static Color CadetBlue => new Color(0x5F9EA0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #66CDAAFF.
        /// </summary>
        public static Color MediumAquamarine => new Color(0x66CDAAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #7FFFD4FF.
        /// </summary>
        public static Color Aquamarine => new Color(0x7FFFD4FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #006400FF.
        /// </summary>
        public static Color DarkGreen => new Color(0x006400FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #556B2FFF.
        /// </summary>
        public static Color DarkOliveGreen => new Color(0x556B2FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #8FBC8FFF.
        /// </summary>
        public static Color DarkSeaGreen => new Color(0x8FBC8FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #2E8B57FF.
        /// </summary>
        public static Color SeaGreen => new Color(0x2E8B57FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #3CB371FF.
        /// </summary>
        public static Color MediumSeaGreen => new Color(0x3CB371FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #20B2AAFF.
        /// </summary>
        public static Color LightSeaGreen => new Color(0x20B2AAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #98FB98FF.
        /// </summary>
        public static Color PaleGreen => new Color(0x98FB98FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00FF7FFF.
        /// </summary>
        public static Color SpringGreen => new Color(0x00FF7FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #7CFC00FF.
        /// </summary>
        public static Color LawnGreen => new Color(0x7CFC00FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00FF00FF.
        /// </summary>
        public static Color Green => new Color(0x00FF00FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00FF00FF.
        /// </summary>
        public static Color Lime => new Color(0x00FF00FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #008000FF.
        /// </summary>
        public static Color WebGreen => new Color(0x008000FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #7FFF00FF.
        /// </summary>
        public static Color Chartreuse => new Color(0x7FFF00FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00FA9AFF.
        /// </summary>
        public static Color MediumSpringGreen => new Color(0x00FA9AFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #ADFF2FFF.
        /// </summary>
        public static Color GreenYellow => new Color(0xADFF2FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #32CD32FF.
        /// </summary>
        public static Color LimeGreen => new Color(0x32CD32FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #9ACD32FF.
        /// </summary>
        public static Color YellowGreen => new Color(0x9ACD32FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #228B22FF.
        /// </summary>
        public static Color ForestGreen => new Color(0x228B22FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #6B8E23FF.
        /// </summary>
        public static Color OliveDrab => new Color(0x6B8E23FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #BDB76BFF.
        /// </summary>
        public static Color DarkKhaki => new Color(0xBDB76BFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F0E68CFF.
        /// </summary>
        public static Color Khaki => new Color(0xF0E68CFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #EEE8AAFF.
        /// </summary>
        public static Color PaleGoldenrod => new Color(0xEEE8AAFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FAFAD2FF.
        /// </summary>
        public static Color LightGoldenrodYellow => new Color(0xFAFAD2FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFFE0FF.
        /// </summary>
        public static Color LightYellow => new Color(0xFFFFE0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFFF00FF.
        /// </summary>
        public static Color Yellow => new Color(0xFFFF00FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFD700FF.
        /// </summary>
        public static Color Gold => new Color(0xFFD700FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #EEDD82FF.
        /// </summary>
        public static Color LightGoldenrod => new Color(0xEEDD82FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DAA520FF.
        /// </summary>
        public static Color Goldenrod => new Color(0xDAA520FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #B8860BFF.
        /// </summary>
        public static Color DarkGoldenrod => new Color(0xB8860BFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #BC8F8FFF.
        /// </summary>
        public static Color RosyBrown => new Color(0xBC8F8FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #CD5C5CFF.
        /// </summary>
        public static Color IndianRed => new Color(0xCD5C5CFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #8B4513FF.
        /// </summary>
        public static Color SaddleBrown => new Color(0x8B4513FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #A0522DFF.
        /// </summary>
        public static Color Sienna => new Color(0xA0522DFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #CD853FFF.
        /// </summary>
        public static Color Peru => new Color(0xCD853FFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DEB887FF.
        /// </summary>
        public static Color Burlywood => new Color(0xDEB887FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F5F5DCFF.
        /// </summary>
        public static Color Beige => new Color(0xF5F5DCFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F5DEB3FF.
        /// </summary>
        public static Color Wheat => new Color(0xF5DEB3FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F4A460FF.
        /// </summary>
        public static Color SandyBrown => new Color(0xF4A460FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #D2B48CFF.
        /// </summary>
        public static Color Tan => new Color(0xD2B48CFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #D2691EFF.
        /// </summary>
        public static Color Chocolate => new Color(0xD2691EFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #B22222FF.
        /// </summary>
        public static Color Firebrick => new Color(0xB22222FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #A52A2AFF.
        /// </summary>
        public static Color Brown => new Color(0xA52A2AFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #E9967AFF.
        /// </summary>
        public static Color DarkSalmon => new Color(0xE9967AFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FA8072FF.
        /// </summary>
        public static Color Salmon => new Color(0xFA8072FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFA07AFF.
        /// </summary>
        public static Color LightSalmon => new Color(0xFFA07AFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFA500FF.
        /// </summary>
        public static Color Orange => new Color(0xFFA500FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF8C00FF.
        /// </summary>
        public static Color DarkOrange => new Color(0xFF8C00FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF7F50FF.
        /// </summary>
        public static Color Coral => new Color(0xFF7F50FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #F08080FF.
        /// </summary>
        public static Color LightCoral => new Color(0xF08080FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF6347FF.
        /// </summary>
        public static Color Tomato => new Color(0xFF6347FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF4500FF.
        /// </summary>
        public static Color OrangeRed => new Color(0xFF4500FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF0000FF.
        /// </summary>
        public static Color Red => new Color(0xFF0000FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF69B4FF.
        /// </summary>
        public static Color HotPink => new Color(0xFF69B4FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF1493FF.
        /// </summary>
        public static Color DeepPink => new Color(0xFF1493FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFC0CBFF.
        /// </summary>
        public static Color Pink => new Color(0xFFC0CBFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FFB6C1FF.
        /// </summary>
        public static Color LightPink => new Color(0xFFB6C1FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DB7093FF.
        /// </summary>
        public static Color PaleVioletRed => new Color(0xDB7093FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #B03060FF.
        /// </summary>
        public static Color Maroon => new Color(0xB03060FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #800000FF.
        /// </summary>
        public static Color WebMaroon => new Color(0x800000FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #C71585FF.
        /// </summary>
        public static Color MediumVioletRed => new Color(0xC71585FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #D02090FF.
        /// </summary>
        public static Color VioletRed => new Color(0xD02090FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF00FFFF.
        /// </summary>
        public static Color Magenta => new Color(0xFF00FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #FF00FFFF.
        /// </summary>
        public static Color Fuchsia => new Color(0xFF00FFFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #EE82EEFF.
        /// </summary>
        public static Color Violet => new Color(0xEE82EEFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DDA0DDFF.
        /// </summary>
        public static Color Plum => new Color(0xDDA0DDFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DA70D6FF.
        /// </summary>
        public static Color Orchid => new Color(0xDA70D6FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #BA55D3FF.
        /// </summary>
        public static Color MediumOrchid => new Color(0xBA55D3FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #9932CCFF.
        /// </summary>
        public static Color DarkOrchid => new Color(0x9932CCFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #9400D3FF.
        /// </summary>
        public static Color DarkViolet => new Color(0x9400D3FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #8A2BE2FF.
        /// </summary>
        public static Color BlueViolet => new Color(0x8A2BE2FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #A020F0FF.
        /// </summary>
        public static Color Purple => new Color(0xA020F0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #800080FF.
        /// </summary>
        public static Color WebPurple => new Color(0x800080FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #9370DBFF.
        /// </summary>
        public static Color MediumPurple => new Color(0x9370DBFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #D8BFD8FF.
        /// </summary>
        public static Color Thistle => new Color(0xD8BFD8FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #A9A9A9FF.
        /// </summary>
        public static Color DarkGrey => new Color(0xA9A9A9FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #A9A9A9FF.
        /// </summary>
        public static Color DarkGray => new Color(0xA9A9A9FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #00008BFF.
        /// </summary>
        public static Color DarkBlue => new Color(0x00008BFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #008B8BFF.
        /// </summary>
        public static Color DarkCyan => new Color(0x008B8BFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #8B008BFF.
        /// </summary>
        public static Color DarkMagenta => new Color(0x8B008BFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #8B0000FF.
        /// </summary>
        public static Color DarkRed => new Color(0x8B0000FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #90EE90FF.
        /// </summary>
        public static Color LightGreen => new Color(0x90EE90FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #DC143CFF.
        /// </summary>
        public static Color Crimson => new Color(0xDC143CFFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #4B0082FF.
        /// </summary>
        public static Color Indigo => new Color(0x4B0082FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #808000FF.
        /// </summary>
        public static Color Olive => new Color(0x808000FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #663399FF.
        /// </summary>
        public static Color RebeccaPurple => new Color(0x663399FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #C0C0C0FF.
        /// </summary>
        public static Color Silver => new Color(0xC0C0C0FFu);

        /// <summary>
        /// Gets a <see cref="Color"/> with a #RRGGBBAA value of #008080FF.
        /// </summary>
        public static Color Teal => new Color(0x008080FFu);

        #endregion
    }
}
