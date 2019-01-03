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
using System.Xml;

namespace AArk
{
    public partial class EditAssessment : System.Web.UI.Page
    {
        public string ARKIVESpeciesName = "";
        public string strCurrentLanguage = "";
        int intAssessmentID = 0;
        int intSpeciesID = 0;
        int intUserID = 0;
        int intCountryID = 0;
        int intLanguageID = 0;
        string strAWeb = "http://amphibiaweb.org/cgi/amphib_query?where-genus=";
        string strAWebPic = "http://calphotos.berkeley.edu/cgi-bin/img_query?getthumbinfo=1&taxon=";
        string strPhotos = "http://calphotos.berkeley.edu/cgi-bin/img_query?where-taxon=";
        string strAWebPic2 = "&num=all&lifeform=Amphibian&format=xml";
        bool hasValue19;
        bool hasValue20;


        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack || Request.QueryString.ToString().IndexOf("Trigger") > 0)
            {
                Section1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
          
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);

            if (strCurrentLanguage == "Spanish")
                imgRLMap.ImageUrl = "images/world map_bw ES.jpg";
            else
            {
                if (strCurrentLanguage == "French")
                    imgRLMap.ImageUrl = "images/world map_bw FR.jpg";
                else
                    imgRLMap.ImageUrl = "images/world map_bw EN.jpg";
            }

            lnkAWeb.Text = DataProxy.LoadString("AWEBLABEL", strCurrentLanguage);

            //Set global variables based on URL values.
            if (Request.QueryString["AssessmentID"] != null)
                intAssessmentID = Convert.ToInt32(Request.QueryString["AssessmentID"].ToString());
            
            if (Request.QueryString["SpeciesID"] != null)
                intSpeciesID = Convert.ToInt32(Request.QueryString["SpeciesID"].ToString());
            
            if (Request.QueryString["CountryID"] != null)
                intCountryID = Convert.ToInt32(Request.QueryString["CountryID"].ToString());

            string strURL = "Login.aspx?SpeciesID=" + intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();
            string strPrevious = "";

            if (Request.UrlReferrer != null)
                strPrevious = Request.UrlReferrer.ToString();

            if (Session["UserName"] == null)
            {
                //Force user to login to create an assessment...
                Response.Redirect(strURL);
            }
            else
            {
                if (Session["UserRole"] == null)
                {
                    //Force user to login to create an assessment...
                    Response.Redirect(strURL);
                }
                else
                {
                    if (Session["UserRole"] == "Guest")
                    {   //user not able to create an assessment - redirect them back to search page.
                        Response.Redirect(strPrevious);
                    }

                    //Set UserID
                    string strUserName = Session["UserName"].ToString();
                    intUserID = DataProxy.GetUserID(strUserName);
                }
            }

            SetStandardLabels();
            SetSectionHeaderLabels();
            SetAmphibiaWebValues();

            if (!Page.IsPostBack)
            {
                PopulateCountriesList();
                PopulateAssessmentValues();
                Intialize_Sections();
                if (Request.QueryString.ToString().IndexOf("Trigger") > 0)
                {
                    CheckTriggerValues();
                }
            }
        
        }

        protected void CheckTriggerValues()
        {
            if (Request.QueryString["TriggerID1"] != null)
               lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(1, intLanguageID);
            if (Request.QueryString["TriggerID2"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(2, intLanguageID);
            if (Request.QueryString["TriggerID3"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(3, intLanguageID);
            if (Request.QueryString["TriggerID4"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(4, intLanguageID);
            if (Request.QueryString["TriggerID5"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(5, intLanguageID);
            if (Request.QueryString["TriggerID6"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(6, intLanguageID);
            if (Request.QueryString["TriggerID7"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(7, intLanguageID);
            if (Request.QueryString["TriggerID8"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(8, intLanguageID);
            if (Request.QueryString["TriggerID9"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(9, intLanguageID);
            if (Request.QueryString["TriggerID10"] != null)
                lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(10, intLanguageID);
    
        }

        protected void SetStandardLabels()
        {
            //Set standard label values based on current language.
            lblPageTitle.Text = DataProxy.LoadString("EDITASSESSMENTTITLE", strCurrentLanguage);
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage) + ":";
            lblPA1Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblPA7Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblPA15Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblPA6Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblPA10Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblPA12Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
         
            //lblPA19Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            //lblPA20Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblOrder.Text = DataProxy.LoadString("ORDERLABEL", strCurrentLanguage);
            lblFamily.Text = DataProxy.LoadString("FAMILYLABEL", strCurrentLanguage);
            lblGlobalRLC.Text = DataProxy.LoadString("GLOBALRLCLABEL", strCurrentLanguage);
            lblNationalRLC.Text = DataProxy.LoadString("NATIONALRLCLABEL", strCurrentLanguage);
            lblEDScore.Text = DataProxy.LoadString("EDSCORELABEL", strCurrentLanguage);
            lblDistribution.Text = DataProxy.LoadString("DISTRIBUTIONLABEL", strCurrentLanguage);
            lblAdditionalComments.Text = DataProxy.LoadString("ADDCOMMENTSLABEL", strCurrentLanguage);

            lblComments1.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments2.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments3.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments4.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments5.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments6.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments7.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments8.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments9.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments10.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments11.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments12.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments13.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments14.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments15.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments16.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments17.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments18.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments19.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            lblComments20.Text = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
            chkComplete.Text = DataProxy.LoadString("MARKASSESSMENTCOMPLETETEXT", strCurrentLanguage);
            lblTriggerRec.Text = DataProxy.LoadString("RECOMMENDEDCONSERVATIONACTIONS", strCurrentLanguage);

            lblError.Text = "";

            lblInstructions.Text = DataProxy.LoadString("ASSESSINSTRUCTIONS", strCurrentLanguage);

            btnSave.Text = DataProxy.LoadString("SAVEAS5ESSMENT", strCurrentLanguage);

        }

        protected void SetAmphibiaWebValues()
        {
            try
            {
                string strGenusName = "";
                string strSpeciesName = "";
                string strBoth = "";
                string strImageLink = "";
                string strCopy = "";
                string strNumber = "1";

                strGenusName = DataProxy.GetGenusName(intSpeciesID);
                strSpeciesName = DataProxy.GetSpeciesName(intSpeciesID);
                strBoth = strGenusName + "+" + strSpeciesName;

                strAWeb += strGenusName + "&where-species=" + strSpeciesName;

                string strPhotoLink = strPhotos + strBoth;

                strAWebPic += strBoth + strAWebPic2;


                strImageLink = DataProxy.ParseXMLImage(strAWebPic);
                strCopy = DataProxy.ParseXMLCopy(strAWebPic);
                strNumber = DataProxy.ParseXMLCount(strAWebPic);

                lblCopyright.Text = strCopy + " (1 of ";
                if (lblCopyright.Text != "")
                {
                    lblCopyright.Visible = true;
                    lnkCopyright.Visible = true;
                    lblCopyDummy.Visible = true;
                    lnkCopyright.NavigateUrl = strPhotoLink;
                    lnkCopyright.Text = strNumber;
                }

                imgAWeb.ImageUrl = strImageLink;
                hlImage.HRef = strPhotoLink;

                lnkAWeb.NavigateUrl = strAWeb;
            }
            catch (Exception ex)
            {
                //leave empty catch for now to prevent XML error
            }
        }


        protected void SetSectionHeaderLabels()
        {
            //Get current LanguageID for strCurrentLanguage
            int intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);

            lblS1Header.Text = DataProxy.GetQuestionSectionDescription(1, intLanguageID);
            lblS2Header.Text = DataProxy.GetQuestionSectionDescription(2, intLanguageID);
            lblS3Header.Text = DataProxy.GetQuestionSectionDescription(3, intLanguageID);
            lblS4Header.Text = DataProxy.GetQuestionSectionDescription(4, intLanguageID);
            lblS5Header.Text = DataProxy.GetQuestionSectionDescription(5, intLanguageID);
            lblS6Header.Text = DataProxy.GetQuestionSectionDescription(6, intLanguageID);
            lblS7Header.Text = DataProxy.GetQuestionSectionDescription(7, intLanguageID);

            Section1.Text = DataProxy.GetQuestionSectionDescription(1, intLanguageID);
            Section2.Text = DataProxy.GetQuestionSectionDescription(2, intLanguageID);
            Section3.Text = DataProxy.GetQuestionSectionDescription(3, intLanguageID);
            Section4.Text = DataProxy.GetQuestionSectionDescription(4, intLanguageID);
            Section5.Text = DataProxy.GetQuestionSectionDescription(5, intLanguageID);
            Section6.Text = DataProxy.GetQuestionSectionDescription(6, intLanguageID);
            Section7.Text = DataProxy.GetQuestionSectionDescription(7, intLanguageID);
        }

        protected void PopulateCountriesList()
        {
            try
            {
                DataTable dtCountries = null;

                dtCountries = DataProxy.GetSpeciesCountries(intSpeciesID);

                ddlCountries.DataSource = dtCountries;
                ddlCountries.DataTextField = dtCountries.Columns[4].ToString();
                ddlCountries.DataValueField = dtCountries.Columns[0].ToString();
                ddlCountries.DataBind();
                ddlCountries.Items.Insert(0, new ListItem("Global", "0"));

                //if (intCountryID == 0)
                //    ddlCountries.SelectedIndex = 0;
                //else
                    ddlCountries.SelectedValue = intCountryID.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void GetTriggerDescription(string strTriggerNbr, int intLanguageID)
        {
            int intTriggerNbr = Convert.ToInt16(strTriggerNbr);
            lblTriggerRecValueDesc.Text = DataProxy.GetTriggerDescription(intTriggerNbr, intLanguageID);
        }

        protected void PopulateAssessmentValues()
        {
            try
            {
                string strValue = DataProxy.GetAssessmentTriggers(intAssessmentID, intLanguageID, "E", intSpeciesID, intCountryID, Page.ResolveUrl("~//images//Question_mark%2025%20px.png"));
                lblTriggerRecValue.Text = strValue;
                CheckTriggerValues();

                //DataTable dtAssess = DataProxy.GetAssessmentInfo(intAssessmentID, intLanguageID);
                DataTable dtAssess = DataProxy.GetSpeciesInfo(intSpeciesID, intCountryID, intLanguageID);
             
                DataRow dr = null;
                //string strRLURL = "http://www.iucnredlist.org/apps/redlist/details/";
                string strRLURL = "https://apiv3.iucnredlist.org/api/v3/taxonredirect/";
                string strRLMap = "http://maps.iucnredlist.org/map.html?id=";
                string strRLID = "";
                if (dtAssess != null)
                {
                    if (dtAssess.Rows.Count > 0)
                    {
                        //Set page control values based on returned values in dataset.
                        dr = dtAssess.Rows[0];
                        string strCommonName = "";
                        
                        //chkComplete.Checked = Convert.ToBoolean(dr["Completed"]);
                        chkComplete.Checked = DataProxy.CheckAssessmentComplete(intAssessmentID);
                        txtAdditionalComments.Text = DataProxy.GetAdditionalComments(intAssessmentID);

                        lblOrderValue.Text = dr["SpeciesOrder"].ToString();
                        lblFamilyValue.Text = dr["SpeciesFamily"].ToString();
                        lblNationalRLCValue.Text = dr["NationalRedListCategory"].ToString();
                        lblGlobalRLCValue.Text = dr["GlobalRedListCategory"].ToString();
                        strCommonName = dr["SpeciesAssessmentDisplayName"].ToString().Trim();
                        ARKIVESpeciesName = DataProxy.GetArkiveName(dr["GenusName"].ToString(), dr["SpeciesName"].ToString());
                      
                        strRLID = dr["RedListID"].ToString().Trim();
                        strRLURL += strRLID;
                        strRLMap += strRLID;

                        if (strCommonName != "")
                            lblSpeciesDisplayName.Text = dr["SpeciesDisplayName"].ToString() + ",";

                        else
                            lblSpeciesDisplayName.Text = dr["SpeciesDisplayName"].ToString();

                        lblSpeciesAssessmentDisplayName.Text = strCommonName;
                        lblEDScoreValue.Text = dr["EDScore"].ToString();
                        lblDistributionValue.Text = DataProxy.GetSpeciesDistribution(intSpeciesID);
                        lnkRedList.NavigateUrl = strRLURL;
                        lnkRedList.Text = DataProxy.LoadString("REDLISTMAPLABEL", strCurrentLanguage);
                        lnkRLMap.HRef = strRLMap;
                        //lblAssessedForValue.Text = dr["CountryName"].ToString();
                        //lblAssessedByValue.Text = dr["UserFullName"].ToString();
                        //lblAssessedOnValue.Text = dr["DateCreatedDate"].ToString();

                        if (strRLID == "" || strRLID == "0")
                        {
                            lnkRedList.Visible = false;
                            lnkRLMap.Visible = false;
                            imgRLMapOnly.Visible = true;
                            string strURL = "";

                            if (strCurrentLanguage.ToUpper() == "SPANISH")
                                strURL = "~/images/No map data ES.jpg";
                            else
                            {
                                if (strCurrentLanguage.ToUpper() == "FRENCH")
                                    strURL = "~/images/No map data FR.jpg";
                                else
                                    strURL = "~/images/No map data EN.jpg";
                            }

                            imgRLMapOnly.ImageUrl = strURL;
                        }
                    }
                }
            }


            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        protected void PopulateAnswersForSection(int intSectionID, string strCurLanguage)
        {
            try
            {

                int intLanguageID = DataProxy.GetLanguageID(strCurLanguage);
                int intResponseLanguageID = DataProxy.GetLanguageIDResponses(intAssessmentID);
                
                //Disable Save button if the current language is not equal to / same as the language original assessment responses were saved in.
                if (intResponseLanguageID != 0 && (intLanguageID != intResponseLanguageID))
                {
                    //JK: Alert user to this as it's a bit confusing...
                    string strMsg = String.Format("<script>alert('" + String.Format(DataProxy.LoadString("WARNINGSAVEDISABLED",strCurLanguage)) + "');</script>");
                    Response.Write(strMsg);
                    btnSave.Enabled = false;
                }

                DataTable dt = DataProxy.GetQuestionsForSection(intSectionID, intLanguageID);
                int intQuestionNbr = 0; //Loop through Questions and populate the controls as necessary...
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    intQuestionNbr = Convert.ToInt32(dr["QuestionNumber"].ToString());
                    //Find if response has been saved for current question
                    DataTable dtResponse = DataProxy.GetResponseForQuestion(intQuestionNbr, intAssessmentID, intLanguageID);
                    if (dtResponse != null)
                    {
                        if (dtResponse.Rows.Count > 0)
                        {
                            DataRow drResponse = dtResponse.Rows[0];

                            switch (intQuestionNbr)
                            {
                                case 1:
                                    ddlQ1.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments1.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 2:
                                    ddlQ2.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments2.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 3:
                                    ddlQ3.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments3.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 4:
                                    ddlQ4.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments4.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 5:
                                    ddlQ5.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments5.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 6:
                                    ddlQ6.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments6.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 7:
                                    ddlQ7.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments7.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 8:
                                    ddlQ8.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments8.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 9:
                                    ddlQ9.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments9.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 10:
                                    ddlQ10.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments10.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 11:
                                    ddlQ11.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments11.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 12:
                                    ddlQ12.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments12.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 13:
                                    ddlQ13.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments13.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 14:
                                    ddlQ14.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments14.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 15:
                                    ddlQ15.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments15.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 16:
                                    ddlQ16.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments16.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 17:
                                    ddlQ17.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments17.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 18:
                                    ddlQ18.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments18.Text = drResponse["ResponseComments"].ToString();
                                    break;
                                case 19:
                                    ddlQ19.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments19.Text = drResponse["ResponseComments"].ToString();
                                    if (txtComments19.Text.Trim() != "")
                                        hasValue19 = true;
                                    else
                                        hasValue19 = false;

                                    SetQ19ResponseValue(hasValue19);
                                    break;
                                case 20:
                                    ddlQ20.SelectedValue = drResponse["PossibleAnswerID"].ToString();
                                    txtComments20.Text = drResponse["ResponseComments"].ToString();

                                    if (txtComments20.Text.Trim() != "")
                                        hasValue20 = true;
                                    else
                                        hasValue20 = false;

                                    SetQ20ResponseValue(hasValue20);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void PopulateQuestionsForSection(int intSectionID, string strCurLanguage)
        {
            try
            {
                int intLanguageID = DataProxy.GetLanguageID(strCurLanguage);

                DataTable dtQuestions = DataProxy.GetQuestionsForSection(intSectionID, intLanguageID);
                int intQuestionID = 0; //Loop through Questions and populate the controls as necessary...
                for (int i = 0; i < dtQuestions.Rows.Count; i++)
                {
                    DataRow dr = dtQuestions.Rows[i];
                    intQuestionID = Convert.ToInt32(dr["QuestionNumber"].ToString());
                    switch (intQuestionID)
                    {

                        case 1:
                            lblQ1.Text = "1.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt1.Text = dr["QuestionPrompt"].ToString();
                            lnkQ1.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 2:
                            lblQ2.Text = "2.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt2.Text = dr["QuestionPrompt"].ToString();
                            lnkQ2.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 3:
                            lblQ3.Text = "3.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt3.Text = dr["QuestionPrompt"].ToString();
                            lnkQ3.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 4:
                            lblQ4.Text = "4.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt4.Text = dr["QuestionPrompt"].ToString();
                            lnkQ4.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 5:
                            lblQ5.Text = "5.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt5.Text = dr["QuestionPrompt"].ToString();
                            lnkQ5.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 6:
                            lblQ6.Text = "6.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt6.Text = dr["QuestionPrompt"].ToString();
                            lnkQ6.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 7:
                            lblQ7.Text = "7.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt7.Text = dr["QuestionPrompt"].ToString();
                            lnkQ7.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 8:
                            lblQ8.Text = "8.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt8.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 9:
                            lblQ9.Text = "9.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt9.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 10:
                            lblQ10.Text = "10.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt10.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 11:
                            lblQ11.Text = "11.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt11.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 12:
                            lblQ12.Text = "12.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt12.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 13:
                            lblQ13.Text = "13.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt13.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 14:
                            lblQ14.Text = "14.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt14.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 15:
                            lblQ15.Text = "15.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt15.Text = dr["QuestionPrompt"].ToString();
                            lnkQ15.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 16:
                            lblQ16.Text = "16.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt16.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 17:
                            lblQ17.Text = "17.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt17.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 18:
                            lblQ18.Text = "18.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt18.Text = dr["QuestionPrompt"].ToString();
                            break;
                        case 19:
                            lblQ19.Text = "19.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt19.Text = dr["QuestionPrompt"].ToString();
                            lnkQ19.Title = dr["QuestionDefinition"].ToString();
                            break;
                        case 20:
                            lblQ20.Text = "20.  " + dr["QuestionShortName"].ToString() + "  -   " + dr["QuestionText"].ToString();
                            lblPrompt20.Text = dr["QuestionPrompt"].ToString();
                            lnkQ20.Title = dr["QuestionDefinition"].ToString();
                            break;
                        
                    }
                    PopulatePossibleAnswers(intQuestionID, intLanguageID);
                }
            }
            
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void PopulatePossibleAnswers(int intQuestionID, int intLanguageID) 
        {
            try
            {
                DataTable dtPossibleAnswers = DataProxy.GetPossibleResponsesForQuestion(intQuestionID, intLanguageID);
                if (dtPossibleAnswers != null)
                {
                    if (intQuestionID == 1)
                    {
                        ddlQ1.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ1.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ1.SelectedIndex = 0;
                    }
                    if (intQuestionID == 2)
                    {
                        ddlQ2.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ2.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ2.SelectedIndex = 0;
                    }

                    if (intQuestionID == 3)
                    {
                        ddlQ3.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ3.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ3.SelectedIndex = 0;
                    }
                    if (intQuestionID == 4)
                    {
                        ddlQ4.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ4.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ4.SelectedIndex = 0;
                    }
                    if (intQuestionID == 5)
                    {
                        ddlQ5.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ5.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ5.SelectedIndex = 0;
                    }
                    if (intQuestionID == 6)
                    {
                        ddlQ6.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ6.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ6.SelectedIndex = 0;
                    }
                    if (intQuestionID == 7)
                    {
                        ddlQ7.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ7.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ7.SelectedIndex = 0;
                    }
                    if (intQuestionID == 8)
                    {
                        ddlQ8.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ8.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ8.SelectedIndex = 0;
                    }
                    if (intQuestionID == 9)
                    {
                        ddlQ9.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ9.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ9.SelectedIndex = 0;
                    }
                    if (intQuestionID == 10)
                    {
                        ddlQ10.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ10.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ10.SelectedIndex = 0;
                    }
                    if (intQuestionID == 11)
                    {
                        ddlQ11.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ11.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ11.SelectedIndex = 0;
                    }
                    if (intQuestionID == 12)
                    {
                        ddlQ12.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ12.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ12.SelectedIndex = 0;
                    }
                    if (intQuestionID == 13)
                    {
                        ddlQ13.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ13.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ13.SelectedIndex = 0;
                    }
                    if (intQuestionID == 14)
                    {
                        ddlQ14.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ14.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ14.SelectedIndex = 0;
                    }
                    if (intQuestionID == 15)
                    {
                        ddlQ15.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ15.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ15.SelectedIndex = 0;
                    }
                    if (intQuestionID == 16)
                    {
                        ddlQ16.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ16.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ16.SelectedIndex = 0;
                    }
                    if (intQuestionID == 17)
                    {
                        ddlQ17.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ17.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ17.SelectedIndex = 0;
                    }
                    if (intQuestionID == 18)
                    {
                        ddlQ18.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ18.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ18.SelectedIndex = 0;
                    }
                    if (intQuestionID == 19)
                    {
                        ddlQ19.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ19.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ19.SelectedIndex = 0;
                    }
                    if (intQuestionID == 20)
                    {
                        ddlQ20.Items.Add(new ListItem(" ", "0"));

                        for (int i = 0; i < dtPossibleAnswers.Rows.Count; i++)
                        {
                            DataRow dr = dtPossibleAnswers.Rows[i];
                            ddlQ20.Items.Add(new ListItem(dr["PossibleAnswerText"].ToString(), dr["PossibleAnswerID"].ToString()));
                        }
                        ddlQ20.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /*protected void Main_Click(object sender, EventArgs e)
        {
            Main.CssClass = "Clicked";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;

        }*/

        protected void Intialize_Sections()
        {
            Section1.CssClass = "Clicked";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
            if (!IsPostBack)
            {
                PopulateQuestionsForSection(1, strCurrentLanguage);
                PopulateQ1Values();
                PopulateQ3Values();
                PopulateProtectedHabitatResponses();
                PopulateAnswersForSection(1, strCurrentLanguage);
                GetQ1Definition();
            }
        }

        protected void Section1_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Clicked";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;

            if (!IsPostBack)
            {
                PopulateQuestionsForSection(1, strCurrentLanguage);
                PopulateQ1Values();
                PopulateQ3Values();
                PopulateProtectedHabitatResponses();
                PopulateAnswersForSection(1, strCurrentLanguage);
                GetQ1Definition();
            }
        }

        protected void Section2_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Clicked";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;

            if (!SectionClicked(2) || ddlQ5.SelectedIndex == -1)
            {
                PopulateQuestionsForSection(2, strCurrentLanguage);
                //PopulateProtectedHabitatResponses();
                PopulateAnswersForSection(2, strCurrentLanguage);
                GetQ6Definition();
            }

            Session["S2CLICKED"] = "YES";

        }

        protected bool SectionClicked(int intSectionID)
        {
            bool bClicked = false;

            switch (intSectionID)
            {
                case 2:
                    if (Session["S2CLICKED"] != null)
                        return true;
                    break;
                case 3:
                    if (Session["S3CLICKED"] != null)
                        return true;
                    break;
                case 4:
                    if (Session["S4CLICKED"] != null)
                        return true;
                    break;
                case 5:
                    if (Session["S5CLICKED"] != null)
                        return true;
                    break;
                case 6:
                    if (Session["S6CLICKED"] != null)
                        return true;
                    break;
                case 7:
                    if (Session["S7CLICKED"] != null)
                        return true;
                    break;

            }

            return bClicked;
        }

        protected void Section3_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Clicked";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 2;

            if (!SectionClicked(3) || ddlQ7.SelectedIndex == -1)
            {
                PopulateQuestionsForSection(3, strCurrentLanguage);
                PopulateAnswersForSection(3, strCurrentLanguage);
                GetQ7Definition();
            }

            Session["S3CLICKED"] = "YES";

        }

        protected void Section4_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Clicked";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 3;

            if (!SectionClicked(4) || ddlQ10.SelectedIndex == -1)
            {
                PopulateQuestionsForSection(4, strCurrentLanguage);
                PopulateAnswersForSection(4, strCurrentLanguage);
            }

            Session["S4CLICKED"] = "YES";
        }

        protected void Section5_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Clicked";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 4;

            if (!SectionClicked(5) || ddlQ13.SelectedIndex == -1)
            {
                PopulateQuestionsForSection(5, strCurrentLanguage);
                PopulateAnswersForSection(5, strCurrentLanguage);
                GetQ15Definition();
            }
            Session["S5CLICKED"] = "YES";
        }

        protected void Section6_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Clicked";
            Section7.CssClass = "Initial";
            MainView.ActiveViewIndex = 5;

            if (!SectionClicked(6) || ddlQ16.SelectedIndex == -1)
            {
                PopulateQuestionsForSection(6, strCurrentLanguage);
                PopulateAnswersForSection(6, strCurrentLanguage);
            }

            Session["S6CLICKED"] = "YES";
        }
        protected void Section7_Click(object sender, EventArgs e)
        {
            //Main.CssClass = "Initial";
            Section1.CssClass = "Initial";
            Section2.CssClass = "Initial";
            Section3.CssClass = "Initial";
            Section4.CssClass = "Initial";
            Section5.CssClass = "Initial";
            Section6.CssClass = "Initial";
            Section7.CssClass = "Clicked";
            MainView.ActiveViewIndex = 6;

            if (!SectionClicked(7) || ddlQ17.SelectedIndex == -1)
            {
                PopulateQuestionsForSection(7, strCurrentLanguage);
                PopulateAnswersForSection(7, strCurrentLanguage);
                GetQ19Definition();
                GetQ20Definition();
            }

            Session["S7CLICKED"] = "YES";
        }

        protected void PopulateProtectedHabitatResponses()
        {
            try
            {
                string strProtectedHabitat = DataProxy.GetProtectedHabitat(intSpeciesID);
                /*Yes
                  No
                  Unknown */

                if (strProtectedHabitat != "-1" && strProtectedHabitat != DBNull.Value.ToString())
                {
                    ddlQ4.SelectedIndex = Convert.ToInt16(strProtectedHabitat);
                    //ddlQ5.SelectedIndex = Convert.ToInt16(strProtectedHabitat);
                    //ddlQ5.SelectedItem.Text = strProtectedHabitat; 

                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void PopulateQ3Values()
        {
            try
            {
                string strEDS = DataProxy.GetEDScore(intSpeciesID);

                if (strEDS != "")
                {
                    //Note if these EDScore ranges change in future - this needs to be modified!!
                    /*  ED value > 100
                        ED value 50-100
                        ED value 20 - 50
                        ED value < 20
                    */
                    if (ddlQ3.Items.Count >= 5)
                    {
                        if (Convert.ToDouble(strEDS) > 100)
                        {
                            ddlQ3.SelectedIndex = 1; //ED value > 100 = 2nd item in list
                            //doesn't work - ddlQ3.SelectedItem.Text = "ED value > 100";
                        }

                        if (Convert.ToDouble(strEDS) > 50 && Convert.ToDouble(strEDS) <= 100)
                        {
                            ddlQ3.SelectedIndex = 2; //ED value > 100 = 3rd item in list
                        }

                        if (Convert.ToDouble(strEDS) >= 20 && Convert.ToDouble(strEDS) <= 50)
                        {
                            ddlQ3.SelectedIndex = 3; //ED value > 100 = 4th item in list
                        }

                        if (Convert.ToDouble(strEDS) < 20)
                        {
                            ddlQ3.SelectedIndex = 4; //ED value > 100 = 5th item in list
                            //doesn't work - ddlQ3.SelectedItem.Text = "ED value < 20";

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void PopulateQ1Values()
        {
            try
            {
                string strGlobalRLCValue = DataProxy.GetGlobalRedListCategoryID(intSpeciesID, 0, intLanguageID);
                string strNationalRLCValue = DataProxy.GetNationalRedListCategoryID(intSpeciesID, intCountryID, intLanguageID);

                if (strNationalRLCValue != "")
                    ddlQ1.SelectedValue = strNationalRLCValue;
                else
                {
                    if (strGlobalRLCValue != "")
                        ddlQ1.SelectedValue = strGlobalRLCValue;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void ddlCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            intCountryID = Convert.ToInt32(ddlCountries.SelectedValue);
            PopulateAssessmentValues();
            PopulateQ1Values();
        }

        protected void GetQ1Definition()
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ1.SelectedValue);

            lblPA1DefinitionValue.Text = strDefinition;
        }

        protected void GetQ6Definition()
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ6.SelectedValue);

            lblPA6DefinitionValue.Text = strDefinition;
        }

        protected void GetQ7Definition()
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ7.SelectedValue);

            lblPA7DefinitionValue.Text = strDefinition;
        }

        protected void GetQ15Definition()
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ15.SelectedValue);

            lblPA15DefinitionValue.Text = strDefinition;
        }

        protected void GetQ19Definition()
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ19.SelectedValue);
            //lblPA19DefinitionValue.Text = strDefinition;
        }

        protected void GetQ20Definition()
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ20.SelectedValue);
            //lblPA20DefinitionValue.Text = strDefinition;
        }

        protected void ddlQ1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQ1Definition();
        }

        protected void ddlQ6_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQ6Definition();
        }

        protected void ddlQ7_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQ7Definition();
        }

        protected void ddlQ15_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQ15Definition();
        }

        protected void ddlQ19_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQ19Definition();
            SetQ19ResponseValue(hasValue19);
        }

        protected void ddlQ20_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQ20Definition();
            SetQ20ResponseValue(hasValue20);
        }

        protected void SetQ20ResponseValue(bool isInitialPop)
        {
            int index = ddlQ20.SelectedIndex;
            string value = ddlQ20.SelectedItem.Text.ToUpper();
            string strCurrentComments = txtComments20.Text;

            txtComments20.Text = DataProxy.SetQ20Comments(value, index, strCurrentLanguage, strCurrentComments);
     
        }

        protected void SetQ19ResponseValue(bool isInitialPop)
        {
            //If user selects "Unknown" (assuming ddl order stays the same since "Unknown" can be diff language), add default text to comments
            string strCurrentComments = txtComments19.Text.Trim();
            int index = ddlQ19.SelectedIndex;
            string value = ddlQ19.SelectedItem.Text.ToUpper();

            txtComments19.Text = DataProxy.SetQ19Comments(value, index, strCurrentLanguage, strCurrentComments);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //If it is a new assessment - save assessment information & get new AssessmentID, otherwise just use existing ID to save Responses...
                int intDDLCountryID = Convert.ToInt32(ddlCountries.SelectedValue);
                int intAssessTriggerID = 0;

                SaveAssessmentResponses(intAssessmentID);

                //If assessment is not marked as completed - do NOT update triggers/priority...
                if (chkComplete.Checked)
                {
                    //Update Triggers after Responses have been updated/saved.
                    string s = DataProxy.EvaluateAssessmentTriggers(intAssessmentID);
                    SetPriorityValue(intAssessmentID);
                    lblError.Text = DataProxy.LoadString("SUCCESSSAVE",strCurrentLanguage);
                }
                else
                { 
                    //Delete any previous AssessmentTrigger record that existed now that it's not marked as 'Complete'
                    intAssessTriggerID = DataProxy.DeleteAssessmentTrigger(intAssessmentID);
                    lblError.Text = DataProxy.LoadString("ASSESSSAVETRIGGERDEL", strCurrentLanguage);
                }

                string strValue = DataProxy.GetAssessmentTriggers(intAssessmentID, intLanguageID, "E", intSpeciesID, intCountryID, Page.ResolveUrl("~/images/Question_mark%2025%20px.png"));
                lblTriggerRecValue.Text = strValue;
                
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected int ResponseExists(int intQuestionID, int intAssessID)
        {
            int intResponseID = 0;

            string strResponseID = DataProxy.GetResponseIDForQuestion(intQuestionID, intAssessID, intLanguageID);

            if (strResponseID != "")
                intResponseID = Convert.ToInt32(strResponseID);

            return intResponseID;
        }

        protected void btnAddObservations_Click(object sender, EventArgs e)
        {
            string strURL = "http://www.inaturalist.org/observations/new?taxon_name=";
            string strIFrame = "";
            string strSpeciesName = DataProxy.GetSpeciesDisplayName(intSpeciesID).Replace(" ", "_");

            strURL += strSpeciesName;

            //Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(strURL)));

            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + strURL + "')</script>");
        }


        protected void SetPriorityValue(int intAssessmentID)
        {
            string strValue = DataProxy.UpdateAssessmentPriority(intAssessmentID);
        }

        protected void SaveAssessmentResponses(int intAssessmentID)
        { 
            //Identify if a response was selected for each question and insert the response given for the current user.
            //Per new editing requirement - do not change UserID and Date for assessments edited via the Facilitator Dashboard.

            if (Request.QueryString["FEdit"] != null)
                intUserID = 0;

            int intResponseID = 0;
            int intExistingResponseID = 0;
            int intDDLCountryID = Convert.ToInt32(ddlCountries.SelectedValue);

            string strAdditionalComments = txtAdditionalComments.Text.Trim();

            int intAssessID = DataProxy.UpdateAssessment(intAssessmentID, intUserID, intDDLCountryID, chkComplete.Checked, strAdditionalComments, false);

            if (ddlQ1.SelectedIndex != 0 && ddlQ1.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(1, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ1.SelectedValue), txtComments1.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ1.SelectedValue), txtComments1.Text.Trim());
            }

            if (ddlQ2.SelectedIndex != 0 && ddlQ2.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(2, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ2.SelectedValue), txtComments2.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ2.SelectedValue), txtComments2.Text.Trim());
            }

            if (ddlQ3.SelectedIndex != 0 && ddlQ3.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(3, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ3.SelectedValue), txtComments3.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ3.SelectedValue), txtComments3.Text.Trim());
            }

            if (ddlQ4.SelectedIndex != 0 && ddlQ4.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(4, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ4.SelectedValue), txtComments4.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ4.SelectedValue), txtComments4.Text.Trim());
            }

            if (ddlQ5.SelectedIndex != 0 && ddlQ5.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(5, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ5.SelectedValue), txtComments5.Text.Trim());
                else
           
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ5.SelectedValue), txtComments5.Text.Trim());
            }

            if (ddlQ6.SelectedIndex != 0 && ddlQ6.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(6, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ6.SelectedValue), txtComments6.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ6.SelectedValue), txtComments6.Text.Trim());
            }

            if (ddlQ7.SelectedIndex != 0 && ddlQ7.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(7, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ7.SelectedValue), txtComments7.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ7.SelectedValue), txtComments7.Text.Trim());
            }

            if (ddlQ8.SelectedIndex != 0 && ddlQ8.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(8, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ8.SelectedValue), txtComments8.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ8.SelectedValue), txtComments8.Text.Trim());
            }

            if (ddlQ9.SelectedIndex != 0 && ddlQ9.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(9, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ9.SelectedValue), txtComments9.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ9.SelectedValue), txtComments9.Text.Trim());
            }

            if (ddlQ10.SelectedIndex != 0 && ddlQ10.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(10, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ10.SelectedValue), txtComments10.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ10.SelectedValue), txtComments10.Text.Trim());
            }

            if (ddlQ11.SelectedIndex != 0 && ddlQ11.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(11, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ11.SelectedValue), txtComments11.Text.Trim());
                else
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ11.SelectedValue), txtComments11.Text.Trim());
            }

            if (ddlQ12.SelectedIndex != 0 && ddlQ12.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(12, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ12.SelectedValue), txtComments12.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ12.SelectedValue), txtComments12.Text.Trim());
            }
            if (ddlQ13.SelectedIndex != 0 && ddlQ13.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(13, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ13.SelectedValue), txtComments13.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ13.SelectedValue), txtComments13.Text.Trim());
            }
            if (ddlQ14.SelectedIndex != 0 && ddlQ14.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(14, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ14.SelectedValue), txtComments14.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ14.SelectedValue), txtComments14.Text.Trim());
            }

            if (ddlQ15.SelectedIndex != 0 && ddlQ15.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(15, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ15.SelectedValue), txtComments15.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ15.SelectedValue), txtComments15.Text.Trim());
            }
            if (ddlQ16.SelectedIndex != 0 && ddlQ16.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(16, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ16.SelectedValue), txtComments16.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ16.SelectedValue), txtComments16.Text.Trim());
            }
            if (ddlQ17.SelectedIndex != 0 && ddlQ17.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(17, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ17.SelectedValue), txtComments17.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ17.SelectedValue), txtComments17.Text.Trim());
            }

            if (ddlQ18.SelectedIndex != 0 && ddlQ18.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(18, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ18.SelectedValue), txtComments18.Text.Trim());
                else
                    intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ18.SelectedValue), txtComments18.Text.Trim());
            }

            if (ddlQ19.SelectedIndex != 0 && ddlQ19.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(19, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ19.SelectedValue), txtComments19.Text.Trim());
                else
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ19.SelectedValue), txtComments19.Text.Trim());
            }

            if (ddlQ20.SelectedIndex != 0 && ddlQ20.SelectedIndex != -1)
            {
                intExistingResponseID = ResponseExists(20, intAssessmentID);
                if (intExistingResponseID > 0)
                    intResponseID = DataProxy.UpdateResponse(intExistingResponseID, Convert.ToInt32(ddlQ20.SelectedValue), txtComments20.Text.Trim());
                else
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ20.SelectedValue), txtComments20.Text.Trim());
            }
        }

        protected void ddlQ10_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ10.SelectedValue);

            lblPA10DefinitionValue.Text = strDefinition;
        }

        protected void ddlQ12_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ12.SelectedValue);

            lblPA12DefinitionValue.Text = strDefinition;
        }
            
    }
}