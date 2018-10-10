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
    public partial class AssessmentQuestions : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strQuestionID = "0";
        string strResponseID = "0";
        int intAssessmentID = 0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            //Set global variables based on URL values.
            if (Request.QueryString["QuestionID"] != null)
                strQuestionID = Request.QueryString["QuestionID"].ToString();

            if (Request.QueryString["ResponseID"] != null)
            {
                strResponseID = Request.QueryString["ResponseID"].ToString();
                if (strResponseID.Trim() == "")
                    strResponseID = "0";
                intAssessmentID = DataProxy.GetAssessmentID(Convert.ToInt32(strResponseID));
            }
            lblPageTitle.Text = DataProxy.LoadString("ASSESSMENTQUESTIONSLABEL", strCurrentLanguage);

            lbltest.Text = "QuestionID=" + strQuestionID + "   AssessmentID=" + intAssessmentID.ToString() + "   ResponseID=" + strResponseID;
        }
    }
}