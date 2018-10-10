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
    public partial class CreateAssessmentBKUPB4UPDATEPANEL : System.Web.UI.Page
    {
       
        string strCurrentLanguage = "";
        int intAssessmentID = 0;
        int intSpeciesID = 0;
        int intUserID = 0;
        int intCountryID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
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

            //Set global variables based on URL values.
            if (Request.QueryString["AssessmentID"] != null)
                intAssessmentID = Convert.ToInt32(Request.QueryString["AssessmentID"].ToString());
            
            if (Request.QueryString["SpeciesID"] != null)
                intSpeciesID = Convert.ToInt32(Request.QueryString["SpeciesID"].ToString());
            
            if (Request.QueryString["CountryID"] != null)
                intCountryID = Convert.ToInt32(Request.QueryString["CountryID"].ToString());
         
            if (Session["UserName"] == null)
            {
                //Force user to login to create an assessment...
                string strURL = "Login.aspx?SpeciesID=" + intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();
                Response.Redirect(strURL);
            }
            else
            {
                //Set UserID
                string strUserName = Session["UserName"].ToString();
                intUserID = DataProxy.GetUserID(strUserName);
            }

            //Set standard label values based on current language.
            lblPageTitle.Text = DataProxy.LoadString("CREATEASSESSMENTTITLE", strCurrentLanguage);
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            lblPA1Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblPA7Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage); 
            //lblPA19Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            //lblPA20Definition.Text = DataProxy.LoadString("POSSIBLEANSWERLABEL", strCurrentLanguage);
            lblOrder.Text = DataProxy.LoadString("ORDERLABEL", strCurrentLanguage);
            lblFamily.Text = DataProxy.LoadString("FAMILYLABEL", strCurrentLanguage);
            lblGlobalRLC.Text = DataProxy.LoadString("GLOBALRLCLABEL", strCurrentLanguage);
            lblNationalRLC.Text = DataProxy.LoadString("NATIONALRLCLABEL", strCurrentLanguage);
            lblEDScore.Text = DataProxy.LoadString("EDSCORELABEL", strCurrentLanguage);
            lblDistribution.Text = DataProxy.LoadString("DISTRIBUTIONLABEL", strCurrentLanguage);
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
            lblError.Text = "";

            SetSectionHeaderLabels();

            if (!Page.IsPostBack)
            { 
                PopulateCountriesList();
                PopulateAssessmentValues();
                Intialize_Sections();

            }

            /*lblOrder.Text = DataProxy.LoadString("ORDERLABEL", strCurrentLanguage);
             lblFamily.Text = DataProxy.LoadString("FAMILYLABEL", strCurrentLanguage);
             lblGlobalRLC.Text = DataProxy.LoadString("GLOBALRLCLABEL", strCurrentLanguage);
             lblNationalRLC.Text = DataProxy.LoadString("NATIONALRLCLABEL", strCurrentLanguage);
             lblEDScore.Text = DataProxy.LoadString("EDSCORELABEL", strCurrentLanguage);
             lblDistribution.Text = DataProxy.LoadString("DISTRIBUTIONLABEL", strCurrentLanguage);
             //lblRLMap.Text = DataProxy.LoadString("REDLISTMAPLABEL", strCurrentLanguage);
             lblAddMap.Text = DataProxy.LoadString("ADDOBSERVATIONINATURALISTLABEL", strCurrentLanguage);
             lblAssessedFor.Text = DataProxy.LoadString("ASSESSMENTRESULTSCOUNTRYLABEL", strCurrentLanguage);
             lblAssessedBy.Text = DataProxy.LoadString("ASSESSMENTRESULTSBYLABEL", strCurrentLanguage);
             lblAssessedOn.Text = DataProxy.LoadString("ASSESSMENTRESULTSONLABEL", strCurrentLanguage);
             PopulateAssessmentValues();
             if (strCurrentLanguage == "Spanish")
                imgRLMap.ImageUrl = "images/world map_bw ES.jpg";
             else
                imgRLMap.ImageUrl = "images/world map_bw EN.jpg";
             */ 

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
            Section7.Text = DataProxy.GetQuestionSectionDescription(7, intLanguageID).Substring(0,42);
        }

        protected void PopulateCountriesList()
        {
            try
            {
                DataTable dtCountries = null;
                bool bAssessments = false;

                dtCountries = DataProxy.GetCountries(bAssessments);

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

     
        protected void PopulateAssessmentValues()
        {
            try
            {
                DataTable dtAssess = DataProxy.GetAssessmentInfo(intAssessmentID);
                DataRow dr = null;
                string strRLURL = "http://www.iucnredlist.org/apps/redlist/details/";
                string strRLMap = "http://maps.iucnredlist.org/map.html?id=";
                string strRLID = "";
                if (dtAssess != null)
                {
                    if (dtAssess.Rows.Count > 0)
                    {
                        //Set page control values based on returned values in dataset.
                        dr = dtAssess.Rows[0];
                        string strCommonName = "";
                        lblOrderValue.Text = dr["SpeciesOrder"].ToString();
                        lblFamilyValue.Text = dr["SpeciesFamily"].ToString();
                        lblNationalRLCValue.Text = dr["NationalRedListCategory"].ToString();
                        lblGlobalRLCValue.Text = dr["GlobalRedListCategory"].ToString();
                        strCommonName = dr["SpeciesAssessmentDisplayName"].ToString().Trim();
                        strRLID = dr["RedListID"].ToString().Trim();
                        strRLURL += strRLID;
                        strRLMap += strRLID;

                        if (strCommonName != "")
                            lblSpeciesDisplayName.Text = dr["SpeciesDisplayName"].ToString() + ",";

                        else
                            lblSpeciesDisplayName.Text = dr["SpeciesDisplayName"].ToString();

                        lblSpeciesAssessmentDisplayName.Text = strCommonName;
                        lblEDScoreValue.Text = dr["EDScore"].ToString();
                        lblDistributionValue.Text = dr["Distribution"].ToString();
                        //lblAssessedForValue.Text = dr["CountryName"].ToString();
                        //lblAssessedByValue.Text = dr["UserFullName"].ToString();
                        //lblAssessedOnValue.Text = dr["DateCreatedDate"].ToString();
                        //lnkRedList.NavigateUrl = strRLURL;
                        //lnkRedList.Text = DataProxy.LoadString("REDLISTMAPLABEL", strCurrentLanguage);
                        //lnkRLMap.HRef = strRLMap;
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
                    intQuestionID = Convert.ToInt32(dr["QuestionID"].ToString());
                    switch (intQuestionID)
                    {
                        case 1:
                            lblQ1.Text = "1.  " + dr["QuestionText"].ToString();
                            break;
                        case 2:
                            lblQ2.Text = "2.  " + dr["QuestionText"].ToString();
                            break;
                        case 3:
                            lblQ3.Text = "3.  " + dr["QuestionText"].ToString();
                            break;
                        case 4:
                            lblQ4.Text = "4.  " + dr["QuestionText"].ToString();
                            break;
                        case 5:
                            lblQ5.Text = "5.  " + dr["QuestionText"].ToString();
                            break;
                        case 6:
                            lblQ6.Text = "6.  " + dr["QuestionText"].ToString();
                            break;
                        case 7:
                            lblQ7.Text = "7.  " + dr["QuestionText"].ToString();
                            break;
                        case 8:
                            lblQ8.Text = "8.  " + dr["QuestionText"].ToString();
                            break;
                        case 9:
                            lblQ9.Text = "9.  " + dr["QuestionText"].ToString();
                            break;
                        case 10:
                            lblQ10.Text = "10.  " + dr["QuestionText"].ToString();
                            break;
                        case 11:
                            lblQ11.Text = "11.  " + dr["QuestionText"].ToString();
                            break;
                        case 12:
                            lblQ12.Text = "12.  " + dr["QuestionText"].ToString();
                            break;
                        case 13:
                            lblQ13.Text = "13.  " + dr["QuestionText"].ToString();
                            break;
                        case 14:
                            lblQ14.Text = "14.  " + dr["QuestionText"].ToString();
                            break;
                        case 15:
                            lblQ15.Text = "15.  " + dr["QuestionText"].ToString();
                            break;
                        case 16:
                            lblQ16.Text = "16.  " + dr["QuestionText"].ToString();
                            break;
                        case 17:
                            lblQ17.Text = "17.  " + dr["QuestionText"].ToString();
                            break;
                        case 18:
                            lblQ18.Text = "18.  " + dr["QuestionText"].ToString();
                            break;
                        case 19:
                            lblQ19.Text = "19.  " + dr["QuestionText"].ToString();
                            break;
                        case 20:
                            lblQ20.Text = "20.  " + dr["QuestionText"].ToString();
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
            PopulateQuestionsForSection(1, strCurrentLanguage);
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
            PopulateQuestionsForSection(1, strCurrentLanguage);

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
                PopulateQuestionsForSection(2, strCurrentLanguage);

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
                PopulateQuestionsForSection(3, strCurrentLanguage);

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
                PopulateQuestionsForSection(4, strCurrentLanguage);

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
                PopulateQuestionsForSection(5, strCurrentLanguage);

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
                PopulateQuestionsForSection(6, strCurrentLanguage);

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
                PopulateQuestionsForSection(7, strCurrentLanguage);

            Session["S7CLICKED"] = "YES";
        }
        protected void ddlQ1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ1.SelectedValue);

            lblPA1DefinitionValue.Text = strDefinition;
                        
        }

        protected void ddlQ7_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ7.SelectedValue);

            lblPA7DefinitionValue.Text = strDefinition;
        }

        protected void ddlQ19_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ19.SelectedValue);

            //lblPA19DefinitionValue.Text = strDefinition;
        }

        protected void ddlQ20_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strDefinition = DataProxy.GetPossibleAnswerDefinition(ddlQ20.SelectedValue);

            //lblPA20DefinitionValue.Text = strDefinition;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //If it is a new assessment - save assessment information & get new AssessmentID, otherwise just use existing ID to save Responses...
                int intCountryID = Convert.ToInt32(ddlCountries.SelectedValue);

                if (intAssessmentID == 0)
                {
                    intAssessmentID = DataProxy.InsertAssessment(intSpeciesID, intUserID, intCountryID);
                    //Save question(s) answered for assessment...
                    SaveAssessmentResponses(intAssessmentID);
                    lblError.Text = DataProxy.LoadString("SUCCESSSAVE",strCurrentLanguage);
                }
                else
                {
                    //For now, going on assumption that edits will be done on a separate form.
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void SaveAssessmentResponses(int intAssessmentID)
        { 
            //Identify if a response was selected for each question and insert the response given for the current user.
            int intResponseID = 0;

            int intAssessID = DataProxy.UpdateAssessment(intAssessmentID, intUserID, intCountryID, chkComplete.Checked, false);

            if (ddlQ1.SelectedIndex != 0 && ddlQ1.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ1.SelectedValue), txtComments1.Text.Trim());

            if (ddlQ2.SelectedIndex != 0 && ddlQ2.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ2.SelectedValue), txtComments2.Text.Trim());

            if (ddlQ3.SelectedIndex != 0 && ddlQ3.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ3.SelectedValue), txtComments3.Text.Trim());

            if (ddlQ4.SelectedIndex != 0 && ddlQ4.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ4.SelectedValue), txtComments4.Text.Trim());

            if (ddlQ5.SelectedIndex != 0 && ddlQ5.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ5.SelectedValue), txtComments5.Text.Trim());

            if (ddlQ6.SelectedIndex != 0 && ddlQ6.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ6.SelectedValue), txtComments6.Text.Trim());

            if (ddlQ7.SelectedIndex != 0 && ddlQ7.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ7.SelectedValue), txtComments7.Text.Trim());

            if (ddlQ8.SelectedIndex != 0 && ddlQ8.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ8.SelectedValue), txtComments8.Text.Trim());

            if (ddlQ9.SelectedIndex != 0 && ddlQ9.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ9.SelectedValue), txtComments9.Text.Trim());

            if (ddlQ10.SelectedIndex != 0 && ddlQ10.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ10.SelectedValue), txtComments10.Text.Trim());

            if (ddlQ11.SelectedIndex != 0 && ddlQ11.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ11.SelectedValue), txtComments11.Text.Trim());

            if (ddlQ12.SelectedIndex != 0 && ddlQ12.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ12.SelectedValue), txtComments12.Text.Trim());

            if (ddlQ13.SelectedIndex != 0 && ddlQ13.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ13.SelectedValue), txtComments13.Text.Trim());

            if (ddlQ14.SelectedIndex != 0 && ddlQ14.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ14.SelectedValue), txtComments14.Text.Trim());

            if (ddlQ15.SelectedIndex != 0 && ddlQ15.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ15.SelectedValue), txtComments15.Text.Trim());

            if (ddlQ16.SelectedIndex != 0 && ddlQ16.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ16.SelectedValue), txtComments16.Text.Trim());

            if (ddlQ17.SelectedIndex != 0 && ddlQ17.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ17.SelectedValue), txtComments17.Text.Trim());

            if (ddlQ18.SelectedIndex != 0 && ddlQ18.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ18.SelectedValue), txtComments18.Text.Trim());

            if (ddlQ19.SelectedIndex != 0 && ddlQ19.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ19.SelectedValue), txtComments19.Text.Trim());

            if (ddlQ20.SelectedIndex != 0 && ddlQ20.SelectedIndex != -1)
                intResponseID = DataProxy.InsertResponse(intAssessmentID, Convert.ToInt32(ddlQ20.SelectedValue), txtComments20.Text.Trim());

        }
    
    }
}