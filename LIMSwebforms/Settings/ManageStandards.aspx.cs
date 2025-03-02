using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LIMSwebforms.Settings
{
    public partial class ManageStandards : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GridView1.DataBind(); // Refresh GridView with search results
        }

        protected void btnAddStandards_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateStandards");
        }
    }
}