using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JdenticonColor = Jdenticon.Rendering.Color;

namespace Jdenticon.WinForms.Sample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // https://jdenticon.com/icon-designer.html?config=6b80342e10770a022f431c30
            iconStyled1.Style = new IdenticonStyle
            {
                Hues = new HueCollection { { 119, HueUnit.Degrees } },
                BackColor = JdenticonColor.FromRgba(107, 128, 52, 46),
                ColorLightness = Range.Create(0.47f, 0.67f),
                GrayscaleLightness = Range.Create(0.28f, 0.48f),
                ColorSaturation = 0.10f,
                GrayscaleSaturation = 0.02f
            };

            // https://jdenticon.com/icon-designer.html?config=2a4766ff10cf303054545454
            iconStyled2.Style = new IdenticonStyle
            {
                Hues = new HueCollection { { 207, HueUnit.Degrees } },
                BackColor = JdenticonColor.FromRgba(42, 71, 102, 255),
                ColorLightness = Range.Create(0.84f, 0.84f),
                GrayscaleLightness = Range.Create(0.84f, 0.84f),
                ColorSaturation = 0.48f,
                GrayscaleSaturation = 0.48f
            };
        }
    }
}
