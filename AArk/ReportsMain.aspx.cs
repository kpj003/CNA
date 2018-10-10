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
    public partial class ReportsMain : System.Web.UI.Page
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


            lblPageTitle.Text = DataProxy.LoadString("REPORTSLABEL", strCurrentLanguage);
            lblApprovedUsers.Text = DataProxy.LoadString("FALABEL", strCurrentLanguage);
            lblAssessmentByUsers.Text = DataProxy.LoadString("ASSESSMENTSBYUSERLABEL", strCurrentLanguage);
            lblApprovedUsersInfo.Text = DataProxy.LoadString("APPROVEDUSERSLINKINFO", strCurrentLanguage);
            lblAssessmentUsersInfo.Text = DataProxy.LoadString("ASSESSMENTSBYUSERLINKINFO", strCurrentLanguage);
            lblSpeciesRecommendRescue.Text = DataProxy.LoadString("RECOMMENDRESCUETITLE", strCurrentLanguage);
            lblSpeciesRecommendRescueInfo.Text = DataProxy.LoadString("RECOMMENDRESCUEINFO", strCurrentLanguage);

            //lblReportViewer.Text = DataProxy.LoadString("ASSESSMENTRESULTSLINK", strCurrentLanguage);

            lblNationalAssessments.Text = DataProxy.LoadString("NATIONALASSESSMENTSLABEL", strCurrentLanguage);
            lblNationalAssessmentsInfo.Text = DataProxy.LoadString("NATIONALASSESSMENTSLABELINFO", strCurrentLanguage);
        }
    }
}