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
    public partial class EditUser : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strUserName = "";
        int intUserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            lblPageTitle.Text = DataProxy.LoadString("EDITUSERPROFILELABEL", strCurrentLanguage);
            lblContent.Text = DataProxy.LoadString("EDITUSERPROFILECONTENT", strCurrentLanguage);
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            lblEmail.Text = DataProxy.LoadString("EMAILLABEL", strCurrentLanguage);
            lblFirstName.Text = DataProxy.LoadString("FIRSTNAMELABEL", strCurrentLanguage);
            lblLastName.Text = DataProxy.LoadString("LASTNAMELABEL", strCurrentLanguage);
            lblLogin.Text = DataProxy.LoadString("USERNAMELABEL", strCurrentLanguage);
            lblOrganization.Text = DataProxy.LoadString("ORGANIZATIONLABEL", strCurrentLanguage);
            lblPassword.Text = DataProxy.LoadString("PASSWORDLABEL", strCurrentLanguage);
            lblPhone.Text = DataProxy.LoadString("PHONELABEL", strCurrentLanguage);
            lblTitle.Text = DataProxy.LoadString("TITLELABEL", strCurrentLanguage);
            btnSave.Text = DataProxy.LoadString("SAVEBUTTON", strCurrentLanguage);
            lblASGMbr.Text = DataProxy.LoadString("ASGMEMBERLABEL", strCurrentLanguage);
            lblLanguage.Text = DataProxy.LoadString("LANGUAGESLINK", strCurrentLanguage);
            lblUserCountries.Text = DataProxy.LoadString("USERCOUNTRIESTEXT", strCurrentLanguage);

            if (!Page.IsPostBack)
            {
                PopulateDropDownLists();
                PopulateUserInfo();
            }
        }

        protected void PopulateUserInfo()
        {
            try
            {
                DataTable dt = null;
                DataRow dr = null;

                if (Request.QueryString["UserID"] != null || Session["UserName"] != null)
                {
                    if (Request.QueryString["UserID"] != null)
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"]);
                    else
                    {
                        strUserName = Session["UserName"].ToString();
                        intUserID = DataProxy.GetUserID(strUserName);
                    }
                }

                dt = DataProxy.GetAllUserInfo(intUserID);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dr = dt.Rows[0];
                        txtFirstName.Text = dr["FirstName"].ToString();
                        txtLastName.Text = dr["LastName"].ToString();
                        txtLoginName.Text = dr["UserName"].ToString();
                        txtOrganization.Text = dr["OrganizationName"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        txtPhone.Text = dr["Phone"].ToString(); 
                        txtTitle.Text = dr["Title"].ToString();
                        ddlCountries.SelectedValue = dr["CountryID"].ToString();
                        ddlLanguages.SelectedValue = dr["LanguageID"].ToString();
                        rbASGMbr.SelectedValue = dr["AmphibianSpecialistGroupMember"].ToString();
                        txtUserCountries.Text = dr["CountryExpertise"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }


        protected void PopulateDropDownLists()
        {
            DataTable dtCountries = null;
            DataTable dtLanguages = null;
            bool bWithAssessments = false;

            dtCountries = DataProxy.GetCountries(bWithAssessments);
            dtLanguages = DataProxy.GetLanguages();
            
            ddlCountries.DataSource = dtCountries;
            ddlCountries.DataTextField = dtCountries.Columns[4].ToString();
            ddlCountries.DataValueField = dtCountries.Columns[0].ToString();
            ddlCountries.DataBind();

            ddlLanguages.DataSource = dtLanguages;
            ddlLanguages.DataTextField = dtLanguages.Columns[1].ToString();
            ddlLanguages.DataValueField = dtLanguages.Columns[0].ToString();
            ddlLanguages.DataBind();
        }
        
        protected bool RequiredFieldsEntered()
        {
            bool bAllFieldsEntered = false;

            if (txtEmail.Text.Trim() != "" &&
                txtFirstName.Text.Trim() != "" &&
                txtLastName.Text.Trim() != "" &&
                txtLoginName.Text.Trim() != "" &&
                txtPassword.Text.Trim() != "" &&
                txtTitle.Text.Trim() != "" && 
                txtEmail.Text.Trim() != "" &&
                ddlCountries.SelectedIndex >= 0)
                
                bAllFieldsEntered = true;

            return bAllFieldsEntered;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (RequiredFieldsEntered())
                {
                    bool bSuccess = false;
                    if (Request.QueryString["UserID"] != null || Session["UserName"] != null)
                    {
                        if (Request.QueryString["UserID"] != null)
                            intUserID = Convert.ToInt32(Request.QueryString["UserID"]);
                        else
                        {
                            strUserName = Session["UserName"].ToString();
                            intUserID = DataProxy.GetUserID(strUserName);
                        }
                    }
                    int intNewID = DataProxy.UpdateUser(txtFirstName.Text.Trim(), txtLastName.Text.Trim(),txtLoginName.Text.Trim(), txtPassword.Text.Trim(),
                                 txtTitle.Text.Trim(), txtEmail.Text.Trim(), txtOrganization.Text.Trim(), ddlLanguages.SelectedValue,
                                 ddlCountries.SelectedValue, rbASGMbr.SelectedValue, txtPhone.Text.Trim(), txtUserCountries.Text.Trim(), intUserID);

                    if (intNewID <= 0)
                        lblError.Text = DataProxy.LoadString("GENERALINSERTERROR", strCurrentLanguage);
                    else
                    {
                        bSuccess = DataProxy.AlertNewUser(intNewID);

                        if (bSuccess)
                            lblError.Text = DataProxy.LoadString("UPDATEUSERPROFILESUCCESS", strCurrentLanguage);
                        else
                            lblError.Text = DataProxy.LoadString("GENERALINSERTERROR", strCurrentLanguage);
                    }

                }
                else
                {
                    lblError.Text = DataProxy.LoadString("REQUIREDFIELDSERROR", strCurrentLanguage);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}