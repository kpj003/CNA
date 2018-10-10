using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Utilities;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;
namespace AArk
{
    public partial class Languages : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
                Session["CurrentLanguage"] = "English";

            lblLanguageText.Text = DataProxy.LoadString("LANGUAGECONTENTTEXT", strCurrentLanguage);
        }


        protected void btnFrench_Click(object sender, EventArgs e)
        {
            Session["CurrentLanguage"] = "French";
            Response.Redirect("Languages.aspx");
        }

        protected void btnEnglish_Click(object sender, ImageClickEventArgs e)
        {
            Session["CurrentLanguage"] = "English";
            Response.Redirect("Languages.aspx");
        }

        protected void btnSpanish_Click(object sender, ImageClickEventArgs e)
        {
            Session["CurrentLanguage"] = "Spanish";
            Response.Redirect("Languages.aspx");
        }
    }
}