using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Reflection;
using Data.Utilities;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;

namespace AArk
{
    public partial class _Default : System.Web.UI.Page
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

            if (strCurrentLanguage == "Spanish")
            {
                lnkOverview.HRef = "/Help/ES/Background.htm";
                lnkAssessorMore.HRef = "/Help/ES/Assessors.htm";
            }
            else
            {
                if (strCurrentLanguage == "English")
                {
                    lnkOverview.HRef = "/Help/EN/Background.htm";
                    lnkAssessorMore.HRef = "/Help/EN/Assessors.htm";
                }
                else
                {
                    lnkOverview.HRef = "/Help/FR/Background.htm";
                    lnkAssessorMore.HRef = "/Help/FR/Assessors.htm";
                }
            }

            lblMain.Text = DataProxy.LoadString("DEFAULTCONTENTTEXT", strCurrentLanguage);
            lblWelcome.Text = DataProxy.LoadString("WELCOMETEXT", strCurrentLanguage);
            lblSearch.Text = DataProxy.LoadString("SEARCHCONTENTTEXT", strCurrentLanguage);
            lblLogin.Text = DataProxy.LoadString("LOGINTEXT", strCurrentLanguage);
            lblSignup.Text = DataProxy.LoadString("SIGNUPTEXT", strCurrentLanguage);
            btnSignup.Text = DataProxy.LoadString("SIGNUPBUTTON", strCurrentLanguage);
            lblMap.Text = DataProxy.LoadString("MAP", strCurrentLanguage);
            lblMore.Text = DataProxy.LoadString("READMORETEXT", strCurrentLanguage);
            lblRightHeader.Text = DataProxy.LoadString("TOPRIGHTHOMETITLE", strCurrentLanguage);
            lblTopRightInfo.Text = DataProxy.LoadString("TOPRIGHTHOMECONTENT", strCurrentLanguage);
            lblRightMore.Text = DataProxy.LoadString("READMORETEXT", strCurrentLanguage);
            lblRecentAssessments.Text = DataProxy.LoadString("RECENTASSESSMENTS", strCurrentLanguage);
            LoadRecentAssessments();
        }

        protected void LoadRecentAssessments()
        {
            try
            {
                //Code here to find latest N number of assessments created...
                //grdRecentAssessments.DataSource = DataProxy.GetRecentAssessments();
                //grdRecentAssessments.DataBind();

                //grdRecentAssessments.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);
                string strLocalName = "";
                string strDate = "";
                string strUser = "";
                string strCountry = "";
                string strLiteral = "";
               
                DataTable dtAssess = DataProxy.GetRecentAssessments();
                for (int i = 0; i < dtAssess.Rows.Count; i++)
                {
                    strLocalName = dtAssess.Rows[i]["DisplayName"].ToString();
                    strDate = dtAssess.Rows[i]["DateCreatedDate"].ToString();
                    strCountry = dtAssess.Rows[i]["CountryName"].ToString();
                    //Feb 2018 - Want to see full name, not username:
                    //strUser = dtAssess.Rows[i]["UserName"].ToString();
                    strUser = dtAssess.Rows[i]["FullName"].ToString();
                    string strURL = "AssessmentResults.aspx?AssessmentID=";
                    strURL += dtAssess.Rows[i]["AssessmentID"].ToString() + "&SpeciesID=" + dtAssess.Rows[i]["SpeciesID"].ToString();    
                    strLiteral += DataProxy.CreateLiteralRecentAdditions(strLocalName, strUser, strDate, strCountry, strURL);

                    //strLiteral += "<br />";
                }
                ltRecentAssessments.Text = strLiteral;

            }
            catch (Exception ex)
            {
                ltRecentAssessments.Text = ex.Message;
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Signup.aspx");
        }
    }
}
