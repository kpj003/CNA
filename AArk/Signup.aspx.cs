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
    public partial class Signup : System.Web.UI.Page
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

            lblPageTitle.Text = DataProxy.LoadString("SIGNUPHEADER", strCurrentLanguage);
            lblContent.Text = DataProxy.LoadString("SIGNUPCONTENT", strCurrentLanguage);
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
                PopulateDropDownLists();
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
                    bool bUniqueUserName = DataProxy.IsUniqueUserName(txtLoginName.Text.Trim());
                    bool bSuccess = false;
                 

                    if (bUniqueUserName)
                    {
                        int intNewID = DataProxy.InsertUser(txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtLoginName.Text.Trim(), txtPassword.Text.Trim(),
                                     txtTitle.Text.Trim(), txtEmail.Text.Trim(), txtOrganization.Text.Trim(), 
                                     ddlLanguages.SelectedValue, ddlCountries.SelectedValue, rbASGMbr.SelectedValue, txtPhone.Text.Trim(), txtUserCountries.Text.Trim());

                        if (intNewID <= 0)
                            lblError.Text = DataProxy.LoadString("GENERALINSERTERROR", strCurrentLanguage);
                        else
                        {
                            bSuccess = DataProxy.AlertNewUser(intNewID);
                            if (bSuccess)
                                lblError.Text = DataProxy.LoadString("SUCCESSINSERTUSER", strCurrentLanguage); 
                            else
                                lblError.Text = DataProxy.LoadString("GENERALINSERTERROR", strCurrentLanguage);
                        }
                    }
                    else
                    {
                        lblError.Text = DataProxy.LoadString("DUPLCATEUSERNAMEERROR", strCurrentLanguage);
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