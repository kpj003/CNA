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
    public partial class AssessorInfo : System.Web.UI.Page
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

            lblPageTitle.Text = DataProxy.LoadString("ASSESSORINFOTITLE", strCurrentLanguage);
            lblContent1.Text = DataProxy.LoadString("ASSESSORINFOCONTENT1", strCurrentLanguage);
            lblContent2.Text = DataProxy.LoadString("ASSESSORINFOCONTENT2", strCurrentLanguage);
            lblContent3.Text = DataProxy.LoadString("ASSESSORINFOCONTENT3", strCurrentLanguage);
            lblContent3Link.Text = DataProxy.LoadString("ASSESSORINFOCONTENT3LINK", strCurrentLanguage);
            lblContent3b.Text = DataProxy.LoadString("ASSESSORINFOCONTENT3B", strCurrentLanguage);
            lblContent4Hdr.Text = DataProxy.LoadString("ASSESSORINFOCONTENT4HEADER", strCurrentLanguage);
            lblContent4.Text = DataProxy.LoadString("ASSESSORINFOCONTENT4", strCurrentLanguage);
            lblContent4Future.Text = DataProxy.LoadString("ASSESSORINFOCONTENT4FUTURE", strCurrentLanguage);
            lblContent5Hdr.Text = DataProxy.LoadString("ASSESSORINFOCONTENT5HEADER", strCurrentLanguage);
            lblContent5.Text = DataProxy.LoadString("ASSESSORINFOCONTENT5", strCurrentLanguage);
            lblContent5Future.Text = DataProxy.LoadString("ASSESSORINFOCONTENT5FUTURE", strCurrentLanguage);
            lblContent6Hdr.Text = DataProxy.LoadString("ASSESSORINFOCONTENT6HEADER", strCurrentLanguage);
            lblContent6.Text = DataProxy.LoadString("ASSESSORINFOCONTENT6", strCurrentLanguage);
            lblContent6Link.Text = DataProxy.LoadString("ASSESSORINFOCONTENT6LINK", strCurrentLanguage);
            lblContent6b.Text = DataProxy.LoadString("ASSESSORINFOCONTENT6B", strCurrentLanguage);
          


        }
    }
}