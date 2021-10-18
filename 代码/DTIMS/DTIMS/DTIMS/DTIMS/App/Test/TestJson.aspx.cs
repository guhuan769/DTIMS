using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Inphase.CTQS.App.Test
{
    public partial class TestJson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string temp = Request.QueryString["userName"];
            }
        }
    }
}