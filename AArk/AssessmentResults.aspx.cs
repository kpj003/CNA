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
    public partial class AssessmentResults : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        int intAssessmentID = 0;
        int intSpeciesID = 0;
        int intLanguageID = 0;
        public string ARKIVESpeciesName = "";
        string strAWeb = "http://amphibiaweb.org/cgi/amphib_query?where-genus=";
        string strAWebPic = "http://calphotos.berkeley.edu/cgi-bin/img_query?getthumbinfo=1&taxon=";
        string strPhotos = "http://calphotos.berkeley.edu/cgi-bin/img_query?where-taxon=";
        string strAWebPic2 = "&num=all&lifeform=Amphibian&format=xml";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);

            //Set global variables based on URL values.
            if (Request.QueryString["AssessmentID"] != null)
                intAssessmentID = Convert.ToInt32(Request.QueryString["AssessmentID"]);

            if (Request.QueryString["SpeciesID"] != null)
                intSpeciesID = Convert.ToInt32(Request.QueryString["SpeciesID"]);

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

            //Set standard label values based on current language.
            lblPageTitle.Text = DataProxy.LoadString("ASSESSMENTRESULTSLINK", strCurrentLanguage);
            lblOrder.Text = DataProxy.LoadString("ORDERLABEL", strCurrentLanguage);
            lblFamily.Text = DataProxy.LoadString("FAMILYLABEL", strCurrentLanguage);
            lblGlobalRLC.Text = DataProxy.LoadString("GLOBALRLCLABEL", strCurrentLanguage);
            lblNationalRLC.Text = DataProxy.LoadString("NATIONALRLCLABEL", strCurrentLanguage);
            lblEDScore.Text = DataProxy.LoadString("EDSCORELABEL", strCurrentLanguage);
            lblDistribution.Text = DataProxy.LoadString("DISTRIBUTIONLABEL", strCurrentLanguage);
            //lblRLMap.Text = DataProxy.LoadString("REDLISTMAPLABEL", strCurrentLanguage);
            //lblAddMap.Text = DataProxy.LoadString("ADDOBSERVATIONINATURALISTLABEL", strCurrentLanguage);
            lblAssessedFor.Text = DataProxy.LoadString("ASSESSMENTRESULTSCOUNTRYLABEL", strCurrentLanguage);
            lblAssessedBy.Text = DataProxy.LoadString("ASSESSMENTRESULTSBYLABEL", strCurrentLanguage);
            lblAssessedOn.Text = DataProxy.LoadString("ASSESSMENTRESULTSONLABEL", strCurrentLanguage);
            lblTriggerRec.Text = DataProxy.LoadString("RECOMMENDEDCONSERVATIONACTIONS", strCurrentLanguage);
            lblAssessmentStatus.Text = DataProxy.LoadString("ASSESSMENTSTATUSLABEL", strCurrentLanguage);
            lblAdditionalComments.Text = DataProxy.LoadString("ADDCOMMENTSLABEL", strCurrentLanguage);
            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
            //btnPrint.Attributes.Add("onclick", "javascript:CallPrint('divPrint');");
            lblReportTitle.Text = DataProxy.LoadString("ASSESSMENTRESULTSLABEL", strCurrentLanguage);

            grdAssess.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);

            SetAmphibiaWebValues();
            PopulateAssessmentValues();
            PopulateQuestionsGrid();

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


        protected void PopulateQuestionsGrid()
        {
            try
            {
                int intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);

                DataTable dtAssess = DataProxy.GetAssessmentQAGrid(intAssessmentID, intLanguageID);

                grdAssess.DataSource = dtAssess;
                grdAssess.DataBind();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        protected void grdAssess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string strUserName = "";
            string strPreviousRow = "";
            HyperLink hlQuestion = null;

            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (Session["UserName"] != null)
                strUserName = Session["UserName"].ToString();

            if (Session["PreviousRow"] != null)
                strPreviousRow = Session["PreviousRow"].ToString();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set first column/"Question #" header
                e.Row.Cells[0].Text = DataProxy.LoadString("QUESTIONNUMBERLABEL", strCurrentLanguage);

                //Set first column/"Short Name" header
                e.Row.Cells[1].Text = DataProxy.LoadString("SHORTNAMELABEL", strCurrentLanguage);

                //Set second column/"Question Text" header
                e.Row.Cells[2].Text = DataProxy.LoadString("QUESTIONTEXTLABEL", strCurrentLanguage);

                //Set third column/"Response" header
                e.Row.Cells[3].Text = DataProxy.LoadString("RESPONSELABEL", strCurrentLanguage);

                //Set fourth column/"Comments" header
                string strComments = DataProxy.LoadString("COMMENTSLABEL", strCurrentLanguage);
                if (strComments.IndexOf(":") > 0)
                    strComments = strComments.Substring(0, strComments.IndexOf(":"));

                e.Row.Cells[4].Text = strComments;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strQuestionID = "";

                if (dr["QuestionID"] != null)
                    strQuestionID = dr["QuestionNumber"].ToString();

                if (e.Row.FindControl("hlQuestion") != null)
                {
                    hlQuestion = (HyperLink)e.Row.FindControl("hlQuestion");

                    string strURL = "";
                    if (strCurrentLanguage == "English")
                    {
                        strURL = "/Help/EN/QuestionsAnswers.htm#q" + strQuestionID;
                    }
                    else
                    {
                        if (strCurrentLanguage == "Spanish")

                            strURL = "/Help/ES/QuestionsAnswers.htm#q" + strQuestionID;
                        else
                            strURL = "/Help/FR/QuestionsAnswers.htm#q";
                    }

                    hlQuestion.NavigateUrl = strURL;
                    hlQuestion.Text = strQuestionID;
                    hlQuestion.Visible = true;
                }
            }
        }
        protected void PopulateAssessmentValues()
        {
            try
            {
                string s = DataProxy.EvaluateAssessmentTriggers(intAssessmentID);

                string strValue = DataProxy.GetAssessmentTriggers(intAssessmentID, intLanguageID, "R", intSpeciesID, 0, Page.ResolveUrl("~/images/Question_mark%2025%20px.png"));
                lblTriggerRecValue.Text = strValue;

                /*DON'T NEED TO DO ALL THIS ANYMORE - ALL UPDATES ARE WITHIN EVAL PROCEDURE NOW.
                 * int intTriggerValue = 0;
                Int32.TryParse(strValue, out intTriggerValue);

                if (intTriggerValue == 0) //Not enough information saved yet / no triggers evaluated to true.
                    intTriggerValue = 10;

                int intAssessmentTriggerID = DataProxy.GetAssessmentTriggerID(intAssessmentID);

                if (intAssessmentTriggerID > 0) //Record exists - update with new evaluation
                    DataProxy.UpdateAssessmentTriggers(intAssessmentTriggerID, intTriggerValue);
                else
                    //Trigger record doesn't exist for Assessment - insert a new record.
                    DataProxy.InsertAssessmentTriggers(intAssessmentID, intTriggerValue);
               

                DataTable dtTrigger = DataProxy.GetTriggerInfo(intAssessmentID);
                if (dtTrigger != null)
                {
                    if (dtTrigger.Rows.Count > 0)
                    {
                        DataRow drT = dtTrigger.Rows[0];
                        lblTriggerRecValue.Text = drT["DisplayTriggers"].ToString();
                    }
                }
                */

                DataTable dtAssess = DataProxy.GetAssessmentInfo(intAssessmentID, intLanguageID);
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
                        //ARKIVESpeciesName = DataProxy.GetArkiveName(dr["GenusName"].ToString(), dr["SpeciesName"].ToString());
                        lblOrderValue.Text = dr["SpeciesOrder"].ToString();
                        lblFamilyValue.Text = dr["SpeciesFamily"].ToString();
                        lblNationalRLCValue.Text = dr["NationalRedListCategory"].ToString();
                        lblGlobalRLCValue.Text = dr["GlobalRedListCategory"].ToString();
                        strCommonName = dr["SpeciesAssessmentDisplayName"].ToString().Trim();
                        strRLID = dr["RedListID"].ToString().Trim();
                        strRLURL += strRLID;
                        strRLMap += strRLID;

                        lblAdditionalCommentsValue.Text = DataProxy.GetAdditionalComments(intAssessmentID);

                        if (strCommonName != "")
                            lblSpeciesDisplayName.Text = dr["SpeciesDisplayName"].ToString() + ",";

                        else
                            lblSpeciesDisplayName.Text = dr["SpeciesDisplayName"].ToString();

                        if (dr["Completed"] != null)
                        {
                            if (dr["Completed"].ToString().ToUpper() == "TRUE")
                                lblAssessmentStatusValue.Text = DataProxy.LoadString("COMPLETEDLABEL", strCurrentLanguage);
                            else
                                lblAssessmentStatusValue.Text = DataProxy.LoadString("INCOMPLETELABEL", strCurrentLanguage);
                        }
                        else
                            lblAssessmentStatusValue.Text = DataProxy.LoadString("INCOMPLETELABEL", strCurrentLanguage);

                        lblSpeciesAssessmentDisplayName.Text = strCommonName;
                        lblEDScoreValue.Text = dr["EDScore"].ToString();
                        lblDistributionValue.Text = DataProxy.GetSpeciesDistribution(intSpeciesID);
                        lblAssessedForValue.Text = dr["CountryName"].ToString();
                        lblAssessedByValue.Text = dr["UserFullName"].ToString();
                        lblAssessedOnValue.Text = dr["DateCreatedDate"].ToString();
                        lnkRedList.NavigateUrl = strRLURL;
                        lnkRedList.Text = DataProxy.LoadString("REDLISTMAPLABEL", strCurrentLanguage);
                        lnkRLMap.HRef = strRLMap;

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


        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string strFileName = "AssessmentResults.xls";
                Response.Clear();
                Response.Buffer = true; //ADDED
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.Charset = "";
                Response.ContentType = "application/vnd.xls";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    Panel1.RenderControl(hw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                //***BEGIN GRID-ONLY CODE
                //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                //grdAssess.RenderControl(htmlWrite);
                //Response.Write(stringWrite.ToString());
                //Response.End(); 
                //***END GRID-ONLY CODE

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            PrintGrid(grdAssess);
        }

        protected void PrintGrid(GridView gv)
        {
            gv.AllowPaging = false;

            PopulateAssessmentValues();
            PopulateQuestionsGrid();

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Panel1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'")
                .Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=900,height=700,status=0');");
            sb.Append("printWin.document.write(\"");
            sb.Append(gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();};");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
            gv.AllowPaging = true;
            gv.DataBind();
        }
    }
}