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
    public partial class AssessmentOverviewTABS : System.Web.UI.Page
    {
        string strCurrentLanguage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            lblPageTitle.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWTITLE", strCurrentLanguage);
            lblContent1Hdr.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT1HEADER", strCurrentLanguage);
            lblContent1.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT1", strCurrentLanguage);
            lblContent2.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT2", strCurrentLanguage);
            lblContent2Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT2LINK", strCurrentLanguage);
            lblContent2b.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT2B", strCurrentLanguage);
            lblContent3.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT3", strCurrentLanguage);
            lblContent4.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT4", strCurrentLanguage);
            lblContent4Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT4LINK", strCurrentLanguage);
            lblContent4b.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT4B", strCurrentLanguage);
            lblContent5.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT5", strCurrentLanguage);
            lblContent5Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT5LINK", strCurrentLanguage);
            lblContent5b.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT5B", strCurrentLanguage);
            lblContent6.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT6", strCurrentLanguage);
            lblContent7.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7", strCurrentLanguage);
            lblContent7Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7LINK", strCurrentLanguage);
            lblContent7b.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7B", strCurrentLanguage);
            lblContent7Link2.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7LINK2", strCurrentLanguage);
            lblContent7c.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7C", strCurrentLanguage);
            lblContent8Hdr.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT8HEADER", strCurrentLanguage);
            lblContent8.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT8", strCurrentLanguage);
            lblContent8Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT8LINK", strCurrentLanguage);
            lblContent8b.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT8B", strCurrentLanguage);
            lblContent9Hdr.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT9HEADER", strCurrentLanguage);
            lblContent9.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT9", strCurrentLanguage);
            lblContent10.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT10", strCurrentLanguage);
            lblContent11.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT11", strCurrentLanguage);
            lblContent12.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT12", strCurrentLanguage); 
            lblContent12Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT12LINK", strCurrentLanguage);
            lblContent12B.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT12BLINK", strCurrentLanguage);
            lblContent13.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT13", strCurrentLanguage);
            lblContent14.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT14", strCurrentLanguage);
            lblContent15.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15", strCurrentLanguage);
            lblContent15B.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15B", strCurrentLanguage); 
            lblContent15C.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15C", strCurrentLanguage);
            lblContent15D.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15D", strCurrentLanguage);
            lblContent15E.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15E", strCurrentLanguage);
            lblContent15Link.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15LINK", strCurrentLanguage);
            lblContent15Link2.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15LINK2", strCurrentLanguage);
            lblContent15Link3.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15LINK3", strCurrentLanguage);
            lblContent15Link4.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15LINK4", strCurrentLanguage);
            lblContent15Link5.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15LINK5", strCurrentLanguage);
            lblContent15Link6.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT15LINK6", strCurrentLanguage);
            lblContent16.Text = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT16", strCurrentLanguage);
          

        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;

        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            MainView.ActiveViewIndex = 1;

        }
    }
}