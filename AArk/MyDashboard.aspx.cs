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
    public partial class MyDashBoard : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strUserName = "";
        int intLanguageID = 0;
        public string ARKIVESpeciesName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strUserRole = "";
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            if (Request.QueryString["UID"] != null)
                strUserName = Request.QueryString["UID"].ToString();
            else
            {
                if (Session["UserName"] != null)
                    strUserName = Session["UserName"].ToString();
                else
                    Response.Redirect("Login.aspx");
            }

            intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);
            lblPageTitle.Text = DataProxy.LoadString("DASHBOARDLINK", strCurrentLanguage);

            strUserRole = DataProxy.GetUserType(strUserName);
            string strURL = "";

            if (strUserRole != "")
            { 
                
                if (strUserRole.ToUpper() == "ASSESSOR")
                    strURL = "AssessorDashboard.aspx?UID=" + strUserName;
                else
                {
                    if (strUserRole.ToUpper() == "FACILITATOR")
                        strURL = "FacilitatorDashboard.aspx?UID=" + strUserName;
                    else
                        strURL = "AdminDashboard.aspx?UID=" + strUserName;
                }
            
                Response.Redirect(strURL);
            }
            else
                Response.Redirect("Login.aspx");
        }
    }
}