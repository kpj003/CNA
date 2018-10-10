using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Utilities;

namespace AArk
{
    public partial class HelpMenu : System.Web.UI.UserControl
    {
        string strCurrentLanguage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            lblTopic1.Text = DataProxy.LoadString("TOPIC1LABEL", strCurrentLanguage);
            lblTopic2.Text = DataProxy.LoadString("TOPIC2LABEL", strCurrentLanguage);
            lblTopic3.Text = DataProxy.LoadString("TOPIC3LABEL", strCurrentLanguage);
            lblTopic4.Text = DataProxy.LoadString("TOPIC4LABEL", strCurrentLanguage);
        }
    }
}