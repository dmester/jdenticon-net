using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Specifies the unit of a hue value.
    /// </summary>
    public enum HueUnit
    {
        /// <summary>
        /// Hue specified in degrees. The first turn is specified in the range [0, 360).
        /// </summary>
        Degrees,
        /// <summary>
        /// Hue specified in turns. The first turn is specified in the range [0, 1).
        /// </summary>
        Turns,
        /// <summary>
        /// Hue specified in radians. The first turn is specified in the range [0, 2π).
        /// </summary>
        Radians,
        /// <summary>
        /// Hue specified in gradians. The first turn is specified in the range [0, 400).
        /// </summary>
        Gradians
    }
}
