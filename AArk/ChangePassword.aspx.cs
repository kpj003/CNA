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
    public partial class ChangePassword : System.Web.UI.Page
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


            lblContent.Text = DataProxy.LoadString("CHANGEPASSWORDCONTENT", strCurrentLanguage);
            lblPageTitle.Text = DataProxy.LoadString("CHANGEPASSWORDLABEL", strCurrentLanguage);
            lblLogin.Text = DataProxy.LoadString("LOGINLABEL", strCurrentLanguage);
            lblPassword.Text = DataProxy.LoadString("PASSWORDLABEL", strCurrentLanguage);
            btnChangePassword.Text = DataProxy.LoadString("CHANGEPASSWORDLABEL", strCurrentLanguage);
         }


        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string strURL = "";
                string strUID = txtLogin.Text.Trim();
                string strPwd = txtPassword.Text.Trim();
                lblError.ForeColor = System.Drawing.Color.Black;

                string strUser = DataProxy.FindUser(strUID);

                if (strUser != "")
                {
                    string s = DataProxy.UpdateUserPassword(strUID,strPwd);

                    if (DataProxy.CheckUserApproved(strUID))
                    {
                        Session["UserName"] = strUID;
                        //Figure out where they should go? Main dashboard?

                        //Check user's role and redirect to appropriate dashboard...
                        string strUserRole = DataProxy.GetUserType(strUID);

                        if (strUserRole.ToUpper() == "ASSESSOR")
                            strURL = "AssessorDashboard.aspx?UID=" + strUID;
                        else
                        {
                            if (strUserRole.ToUpper() == "FACILITATOR")
                                strURL = "FacilitatorDashboard.aspx?UID=" + strUID;
                            else
                                strURL = "AdminDashboard.aspx?UID=" + strUID;
                        }
                        Response.Redirect(strURL);
                    }
                    else
                    {
                        lblError.ForeColor = System.Drawing.Color.Red;
                        lblError.Text = DataProxy.LoadString("NOTAPPROVEDTEXT", strCurrentLanguage);
                    }
                }
                else
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Text = DataProxy.LoadString("INVALIDLOGINERRORTEXT", strCurrentLanguage);
                }
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;

            }

        }

    }
}