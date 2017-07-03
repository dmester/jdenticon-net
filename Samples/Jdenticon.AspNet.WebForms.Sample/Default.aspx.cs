using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jdenticon.AspNet.WebForms.Sample
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            repeater.DataSource = new object[]
            {
                123,
                124,
                "Item3",
                "Item4"
            };
            repeater.DataBind();
        }
    }
}