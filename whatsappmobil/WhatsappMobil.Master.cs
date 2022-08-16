using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace whatsappmobil
{
    public partial class WhatsappMobil : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUser = Convert.ToString(Session["UserID"]);
            string strBranch = Convert.ToString(Session["UserBranch"]);
            lblLoginName.Text = strUser;
            if (Session["UserName"] == null)
            {
                //Session.Abandon();
               //FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx", true);
                return;
            }
        }

        protected void linkto_Click(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("~/Login.aspx");
        }
    }
}