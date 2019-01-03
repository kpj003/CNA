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
using System.Web.UI.HtmlControls;

namespace AArk
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        string strCurrentLanguage = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            //var scheme = this.Request.Url.Scheme;
            //if(scheme == "http")
            //{
            //    //var uri = this.Request.Url;
            //    UriBuilder uri = new UriBuilder();
            //    uri.Scheme = "https";
            //    //uri.
            //}
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
                imgTop.ImageUrl = "images/CNA top banner EN.png";
                imgBottom.ImageUrl = "images/CNA bottom banner EN.png";
                lnkHelp.NavigateUrl = "~/Help/EN/Help.htm";
            }
            else
            {
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
                if (strCurrentLanguage == "Spanish")
                {
                    imgTop.ImageUrl = "images/CNA top banner ES.png";
                    imgBottom.ImageUrl = "images/CNA bottom banner ES.png";
                    lnkHelp.NavigateUrl = "~/Help/ES/Help.htm";
                }
                if (strCurrentLanguage == "English")
                {
                    imgTop.ImageUrl = "images/CNA top banner EN.png";
                    imgBottom.ImageUrl = "images/CNA bottom banner EN.png";
                    lnkHelp.NavigateUrl = "Help/EN/Help.htm";
                }

                if (strCurrentLanguage == "French")
                {
                    imgTop.ImageUrl = "images/CNA top banner FR.png";
                    imgBottom.ImageUrl = "images/CNA bottom banner FR.png";
                    lnkHelp.NavigateUrl = "Help/FR/Help.htm";
                }
            }

            if (strCurrentLanguage == "Spanish")
            {
                lnkContact.HRef = "/Help/ES/ContactUs.htm";
            }
            else
            {
                if (strCurrentLanguage == "English")
                {
                    lnkContact.HRef = "/Help/EN/ContactUs.htm";
                }
                else
                {
                    //Code for French, etc. links goes here
                    lnkContact.HRef = "/Help/FR/ContactUs.htm";
                }
            }

            lblHome.Text = DataProxy.LoadString("HOMELINK", strCurrentLanguage);
            lblAssessmentSearch.Text = DataProxy.LoadString("ASESSMENTSEARCHALLNEW", strCurrentLanguage);
            lblContactUs.Text = DataProxy.LoadString("CONTACTUSLINK", strCurrentLanguage);
            lblLogin.Text = DataProxy.LoadString("LOGINLINK", strCurrentLanguage);
            lblSearch.Text = DataProxy.LoadString("ASESSMENTSEARCHNEW", strCurrentLanguage);
            lnkHelp.Text = DataProxy.LoadString("HELPLINK", strCurrentLanguage);
            lblDashboard.Text = DataProxy.LoadString("DASHBOARDLINK", strCurrentLanguage);
            lblReports.Text = DataProxy.LoadString("REPORTSLABEL", strCurrentLanguage);
            //lblAboutAssess.Text = DataProxy.LoadString("ABOUTASSESSMENTLINK", strCurrentLanguage); 
            //lblLanguages.Text = DataProxy.LoadString("LANGUAGESLINK", strCurrentLanguage);

            //Don't show 'My Dashboard' menu link if there isn't a user logged in...
            if (Session["UserName"] == null)
            {
                lblDashboard.Visible = false;
                lblPipe.Visible = false;
                lblPipe2.Visible = false;
                lblAssessmentSearch.Visible = false;
            }
            else
            {
                string strUserRole = "";
                if (Session["UserRole"] != null)
                    strUserRole = Session["UserRole"].ToString();

                if (strUserRole != "" && strUserRole != "Guest")
                    lblDashboard.Visible = true;
                else
                    lblDashboard.Visible = false;

                lblPipe.Visible = true;
                lblPipe2.Visible = true;
                lblAssessmentSearch.Visible = true;
            }

        }

        protected void btnEnglish_Click(object sender, ImageClickEventArgs e)
        {
            Session["CurrentLanguage"] = "English";
            string strURL = Request.Url.AbsolutePath;
            Response.Redirect(strURL);
        }

        protected void btnSpanish_Click(object sender, ImageClickEventArgs e)
        {

            Session["CurrentLanguage"] = "Spanish";
            string strURL = Request.Url.AbsolutePath;
            Response.Redirect(strURL);
        }

        protected void btnFrench_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Write("<script>alert('Coming soon!');</script>");

            Session["CurrentLanguage"] = "French";
            string strURL = Request.Url.AbsolutePath;
            Response.Redirect(strURL);
        }
    }
}
