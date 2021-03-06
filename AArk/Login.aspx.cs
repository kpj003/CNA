﻿using System;
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
    public partial class Login : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strSpeciesID = "";
        string strCountryID = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            if (Request.QueryString["SpeciesID"] != null)
                strSpeciesID = Request.QueryString["SpeciesID"].ToString();

            if (Request.QueryString["CountryID"] != null)
                strCountryID = Request.QueryString["CountryID"].ToString();
          
            lblContent.Text = DataProxy.LoadString("LOGINCONTENTTEXT", strCurrentLanguage);
            lblPageTitle.Text = DataProxy.LoadString("LOGINLABEL", strCurrentLanguage);
            lblLogin.Text = DataProxy.LoadString("LOGINLABEL", strCurrentLanguage);
            lblPassword.Text = DataProxy.LoadString("PASSWORDLABEL", strCurrentLanguage);
            btnLogin.Text = DataProxy.LoadString("LOGINLABEL", strCurrentLanguage);
            lnkForgot.Text = DataProxy.LoadString("FORGETPASSWORD", strCurrentLanguage);
            lblSignup.Text = DataProxy.LoadString("SIGNUPTEXT", strCurrentLanguage);
            btnSignup.Text = DataProxy.LoadString("SIGNUPBUTTON", strCurrentLanguage);
            lblError.Text = "";
            txtLoginName.Focus();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            try
            {
                string strURL = "";
                string strUID = txtLoginName.Text.Trim();
                string strPwd = txtPassword.Text.Trim();
                lblError.ForeColor = System.Drawing.Color.Black;

                DataTable dtResults = DataProxy.GetUserInfo(strUID, strPwd);

                if (dtResults != null)
                {
                    if (dtResults.Rows.Count > 0)
                    {
                        if (DataProxy.CheckUserApproved(strUID))
                        {
                            Session["UserName"] = strUID;
                            //Figure out where they should go? Main dashboard?
                            if (strSpeciesID != "")
                                //User was brought in here by Search Results and tried to add a new assessment...
                                strURL = "CreateAssessment.aspx?SpeciesID=" + strSpeciesID + "&CountryID=" + strCountryID;
                            else
                            {
                                //Check user's role and redirect to appropriate dashboard...
                                string strUserRole = DataProxy.GetUserType(strUID);
                                Session["UserRole"] = strUserRole;
                        
                                if (strUserRole.ToUpper() == "ASSESSOR")
                                    strURL = "AssessorDashboard.aspx?UID=" + strUID;
                                else
                                {
                                    if (strUserRole.ToUpper() == "GUEST")
                                    {
                                        strURL = "Default.aspx?UID=" + strUID;
                                    }
                                    else
                                    {
                                        if (strUserRole.ToUpper() == "FACILITATOR")
                                            strURL = "FacilitatorDashboard.aspx?UID=" + strUID;
                                        else
                                            strURL = "AdminDashboard.aspx?UID=" + strUID;
                                    }
                                }
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
                        lblError.Text = DataProxy.LoadString("INVALIDLOGINERRORTEXT",strCurrentLanguage);
                    }
                }

            }
            catch (Exception ex)
            {
                //Session["UserName"] = null;
                lblError.Text = ex.Message;

            }

        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Signup.aspx");
        }

        protected void lnkForgot_Click(object sender, EventArgs e)
        {

            if (txtLoginName.Text == "")
            {
                lblError.Text = DataProxy.LoadString("USERNAMENOTFOUND", strCurrentLanguage);
                return;
            }
            else
            {
                int intUserID = 0;
                intUserID = DataProxy.GetUserID(txtLoginName.Text.Trim());

                if (intUserID != 0)
                {
                    DataProxy.SendPassword(intUserID, "ADMIN");
                    DataProxy.SendPassword(intUserID, "USER");
                    lblError.Text = DataProxy.LoadString("PASSWORDEMAILSENT", strCurrentLanguage);
                }
                else
                    lblError.Text = DataProxy.LoadString("USERNAMENOTFOUND", strCurrentLanguage);

            }
        }
    }
}