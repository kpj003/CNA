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
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace AArk
{
    public partial class SearchAll : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strUserRole = "";
        string strUserName = "";
        int intColCount = 5;
        string strFileName = "SearchAll.xls";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            if (Session["UserName"] != null)
                strUserName = Session["UserName"].ToString();

            if (Session["UserRole"] != null)
                strUserRole = Session["UserRole"].ToString();

            lblPageTitle.Text = DataProxy.LoadString("SEARCHALLTITLE", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("ASSESSMENTSLABEL", strCurrentLanguage);

            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
     
            lblContent.Text = DataProxy.LoadString("SEARCHALLMAIN", strCurrentLanguage);
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            btnSearch.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);
            lblSpecies.Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);
            lblAssessments.Text = DataProxy.LoadString("INCLUDEASSESSMENTLABEL", strCurrentLanguage);
            chkUserResultsOnly.Text = DataProxy.LoadString("CHECKBOXRESULTSTEXT", strCurrentLanguage);
            grdResults.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);

            if (!Page.IsPostBack)
            {
                PopulateCountriesList();
                PopulateAssessmentsList();
            }

            if (Request.QueryString["ArchiveAssessID"] != null && Request.QueryString["Archive"] != null)
            {
                lblError.Text = "";

                if (Session["ArchiveAssessment"] == null)
                {
                    ArchiveAssessment();
                }
                else
                {
                    if (Session["ArchiveAssessment"].ToString() != Request.QueryString["ArchiveAssessID"] ||
                        (Session["ArchiveAssessment"].ToString() == Request.QueryString["ArchiveAssessID"] &&
                         Session["ArchiveType"].ToString() != Request.QueryString["Archive"]))
                    {
                        ArchiveAssessment();
                    }
                }
            }
            
        }
        protected void ArchiveAssessment()
        {
            int assessID = Convert.ToInt32(Request.QueryString["ArchiveAssessID"]);
            string archive = Request.QueryString["Archive"].ToString();

            CheckArchives(assessID, archive);
            Search();

            if (Session["ddlCountriesValue"] != null)
                ddlCountries.SelectedValue = Session["ddlCountriesValue"].ToString();

            if (Session["txtSpeciesValue"] != null)
                txtSpecies.Text = Session["txtSpeciesValue"].ToString();

            Session["ArchiveAssessment"] = Request.QueryString["ArchiveAssessID"];
            Session["ArchiveType"] = Request.QueryString["Archive"];  
        }

        protected void CheckArchives(int assessID, string archive)
        {
            if (archive == "Y")
            {
                Archive(assessID);
                lblError.Text = DataProxy.LoadString("ARCHIVEMESSAGE", strCurrentLanguage);
            }
            else
            {
                Unarchive(assessID);
                lblError.Text = DataProxy.LoadString("UNARCHIVEMESSAGE", strCurrentLanguage);            
            }
        }

        protected void PopulateCountriesList()
        {
            try
            {
                DataTable dtCountries = null;

                //Show all countries so assessor can select any country to write an assessment for...
                bool bAssessments = false;

                dtCountries = DataProxy.GetCountries(bAssessments);

                ddlCountries.DataSource = dtCountries;
                ddlCountries.DataTextField = dtCountries.Columns[4].ToString();
                ddlCountries.DataValueField = dtCountries.Columns[0].ToString();
                ddlCountries.DataBind();
                ddlCountries.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlCountries.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void PopulateAssessmentsList()
        {
            ddlAssessments.Items.Add(DataProxy.LoadString("ASSESSMENTSDDLLVALUE1", strCurrentLanguage));
            ddlAssessments.Items.Add(DataProxy.LoadString("ASSESSMENTSDDLLVALUE2", strCurrentLanguage));
            ddlAssessments.Items.Add(DataProxy.LoadString("ASSESSMENTSDDLLVALUE3", strCurrentLanguage));
         
        }

        protected void grdResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            grdResults.PageIndex = e.NewPageIndex;
            if (Session["dvResults"] != null)
                grdResults.DataSource = (DataView)Session["dvResults"];

            GetSearchResults();
            grdResults.DataBind();
            lblGridMessage.Text = "Page " + (grdResults.PageIndex + 1).ToString() + " of " + grdResults.PageCount;
        }

        protected void grdResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = null;
            if (Session["dvResults"] != null)
            {
                grdResults.DataSource = (DataView)Session["dvResults"];
            }
            else
                dataTable = (DataTable)grdResults.DataSource;

            DataView dataView = null;

            if (dataTable != null)
                dataView = new DataView(dataTable);
            else
                dataView = (DataView)Session["dvResults"];

            if (dataView != null)
            {
                if (Session["SortCol"] == null)
                {
                    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql("A");
                    Session["SortDirection"] = "A";
                }
                else
                {
                    if (Session["SortCol"].ToString() == e.SortExpression && Session["SortDirection"].ToString() != "D")
                    {
                        Session["SortDirection"] = "D";
                    }
                    else
                    {
                        Session["SortDirection"] = "A";
                    }
                    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(Session["SortDirection"].ToString());
                }
                grdResults.DataSource = dataView;
                grdResults.DataBind();
                Session["SortCol"] = e.SortExpression;
                lblGridMessage.Text = "Page " + (grdResults.PageIndex + 1).ToString() + " of " + grdResults.PageCount;
            }
        }
        private string ConvertSortDirectionToSql(string sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case "A":
                    newSortDirection = "ASC";
                    break;

                case "D":
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }


        protected void grdResults_RowCommand(object sender,
                         GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                // get the categoryID of the clicked row
                int assessID = Convert.ToInt32(e.CommandArgument);
                // Delete the record 
                DeleteAssessmentInfo(assessID);
                GetSearchResults();
            }
        }

        protected void DeleteAssessmentInfo(int assessmentID)
        {
            try
            {
                DataProxy.DeleteAssessment(assessmentID);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void grdResults_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //JK: Don't have to do this here anymore - being done via RowCommand
           
        }

        public static string Archive(int assessID)
        {

            string dummy = DataProxy.ArchiveAssessment(assessID);
            return dummy;
        }

        public static string Unarchive(int assessID)
        {
            string dummy = DataProxy.UnarchiveAssessment(assessID);
            return dummy;
        }

        protected void grdResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink hlEdit = null;
            HyperLink hlArchive = null;
            HyperLink hlSpeciesResults = null;
            HyperLink hlAdd = null;
            LinkButton lnkDelete = null;
            Label lblDelete = null;
            Label lblCurStatus = null;
            Label lblDateAssessed = null;


            string strUserName = "";
            string strPreviousRow = "";
            
            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (Session["UserName"] != null)
                strUserName = Session["UserName"].ToString();

            int intCurrentUserID = DataProxy.GetUserID(strUserName);

            //JK: No idea why I was doing this - maybe to prevent dups with a previous query's dataset, but causing rows to be hidden in error - commenting out.
            //if (Session["PreviousRow"] != null)
            //    strPreviousRow = Session["PreviousRow"].ToString();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set first column/"Country" header
                e.Row.Cells[0].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);

                //Set second column/"Species" header
                e.Row.Cells[1].Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);

                //Set third column/"Common Name" header
                e.Row.Cells[2].Text = DataProxy.LoadString("COMMONNAMELABEL", strCurrentLanguage);

                //Set fourth column/"Local Common Name" header
                e.Row.Cells[3].Text = DataProxy.LoadString("LOCALCOMMONNAMELABEL", strCurrentLanguage);

                //Set fifth column/"User Name" header
                e.Row.Cells[4].Text = DataProxy.LoadString("ASSESSEDBYLABEL", strCurrentLanguage);

                //Set sixth column/"Date Assessed" header
                e.Row.Cells[5].Text = DataProxy.LoadString("ASSESSMENTDATELABEL", strCurrentLanguage);

                //Set seventh column/"Status" header
                string strStatus = DataProxy.LoadString("ASSESSMENTSTATUSLABEL", strCurrentLanguage);
                strStatus = strStatus.Replace(":", "");
                e.Row.Cells[6].Text = strStatus;
                
            }
        
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strAssessmentID = "";
                int intSpeciesID = 0;
                int intCountryID = 0;
                int intAssessUserID = 0;
                int intUserID = 0;
                int intAssessmentID = 0;
                string strCurStatus = "";

               
                if (dr["SpeciesID"] != null)
                    int.TryParse(dr["SpeciesID"].ToString(), out intSpeciesID);

                if (dr["UserID"] != null)
                    int.TryParse(dr["UserID"].ToString(), out intAssessUserID);

                if (dr["CountryID"] != null)
                    int.TryParse(dr["CountryID"].ToString(), out intCountryID);

                if (dr["AssessmentID"] != null)
                    strAssessmentID = dr["AssessmentID"].ToString();


                if (e.Row.FindControl("lblDateAssessed") != null)
                {

                    DateTime dtDate;
                    string strDate;

                    lblDateAssessed = (Label)e.Row.FindControl("lblDateAssessed");

                    if (dr["DateCreated"] != null)
                    {
                        if (dr["DateCreated"] != DBNull.Value)
                        {
                            dtDate = Convert.ToDateTime(dr["DateCreated"]);
                            strDate = dtDate.ToString("dd MMM yyyy");
                            lblDateAssessed.Text = strDate;
                        }
                    }
                }


                if (e.Row.FindControl("lblCurStatus") != null)
                {

                    lblCurStatus = (Label)e.Row.FindControl("lblCurStatus");

                    if (dr["CurStatus"] != null)
                        strCurStatus = dr["CurStatus"].ToString();

                    if (strCurStatus != "")
                    {
                        if (strCurStatus == "Draft")
                            strCurStatus = DataProxy.LoadString("INCOMPLETELABEL", strCurrentLanguage);
                        else
                            strCurStatus = DataProxy.LoadString("COMPLETEDLABEL", strCurrentLanguage);
                    }
                    lblCurStatus.Text = strCurStatus;
                }

                if (e.Row.FindControl("hlAdd") != null)
                {
                    hlAdd = (HyperLink)e.Row.FindControl("hlAdd");
                    
                    string strURL = "CreateAssessment.aspx?AssessmentID=0"  + "&SpeciesID=" +
                                  intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();

                    hlAdd.NavigateUrl = strURL;
                    hlAdd.Text = DataProxy.LoadString("ADDASSESSMENTLABEL", strCurrentLanguage);

                    //Only allow user to add an assessment if they haven't already created one for that country/species combo
                    if (intCurrentUserID == 0)
                    { 
                        hlAdd.Visible = true; 
                    }
                    else
                    {
                        //Check here if user has an assessment

                        if (intAssessUserID != intCurrentUserID)
                        {
                            if (strAssessmentID != "")
                            { 
                                intAssessmentID = Convert.ToInt32(strAssessmentID); 
                            }
                            intUserID = DataProxy.GetUserIDForAssessment(intCurrentUserID, intCountryID, intSpeciesID);

                            if (strUserRole == "Guest")
                                hlAdd.Enabled = false;

                            if (intUserID > 0)
                                hlAdd.Visible = false; 
                            else
                                hlAdd.Visible = true; 
                        }
                        else
                        { 
                            hlAdd.Visible = false; 
                        }
                    }
                }
                if (e.Row.FindControl("lnkDelete") != null)
                {  
                    lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                    if (intCurrentUserID != 0)
                    {
                        //jan 2018 - add Delete option for Admin users logged in as well...             
                        if (intAssessUserID == intCurrentUserID || (strUserRole == "Admin" && intAssessUserID != 0)) 
                        {
                            lnkDelete.Visible = true;

                            if (strUserRole == "Guest")
                                lnkDelete.Enabled = false;

                            int intAssessID = Convert.ToInt32(strAssessmentID);
                            lnkDelete.CommandArgument = intAssessID.ToString();
                            lblDelete = (Label)e.Row.FindControl("lblDelete");
                            if (lblDelete != null)
                                lblDelete.Text = DataProxy.LoadString("DELETETEXT", strCurrentLanguage);
                        }
                        else
                        {
                            lnkDelete.Visible = false;
                        }
                    }
                    else
                        lnkDelete.Visible = false;
                }

                if (e.Row.FindControl("hlEdit") != null)
                {   
                    hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    string strURL = "EditAssessment.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                  intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();

                    bool isFacilitator = false; 
                    int intAssessID = Convert.ToInt32(strAssessmentID);

                    if (intCurrentUserID != 0)
                    { 
                        //Add logic to this for Facilitators who can now edit the assessment
                        DataTable dtCurUser = DataProxy.GetUserInfo(intCurrentUserID);
                        if (dtCurUser != null)
                        {
                            if (dtCurUser.Rows.Count > 0)
                            {
                                DataRow drUser = dtCurUser.Rows[0];
                                int intUserTypeID = Convert.ToInt16(drUser["UserTypeID"]);
                                string userType = DataProxy.GetUserType(intUserTypeID);
                                if (userType.ToUpper() == "FACILITATOR")
                                {
                                    isFacilitator = true;
                                }
                            }
                        }

                        if (intAssessUserID == intCurrentUserID || isFacilitator)
                        {
                            hlEdit.Visible = true;
                           
                            if (strUserRole == "Guest")
                                hlEdit.Enabled = false;

                            if (isFacilitator)
                            {
                                hlEdit.Text = DataProxy.LoadString("EDITTEXT", strCurrentLanguage);
                                strURL += "&FEdit=Y";
                                if (strAssessmentID == "0")
                                    hlEdit.Visible = false;

                                //Set Archive assessments
                                hlArchive = (HyperLink)e.Row.FindControl("hlArchive");
                                if (DataProxy.CheckAssessmentComplete(intAssessID))
                                {
                                    hlArchive.Visible = true;
                                    string strMessage = "";
                                    if (DataProxy.CheckAssessmentArchived(intAssessID))
                                    {
                                        strMessage = DataProxy.LoadString("CONFIRMUNARCHIVE", strCurrentLanguage);

                                        hlArchive.Text = DataProxy.LoadString("ARCHIVETEXT", strCurrentLanguage);
                                        hlArchive.Attributes["onclick"] = String.Format("return confirm('{0}');", strMessage);
                                        hlArchive.NavigateUrl = "SearchAll.aspx?ArchiveAssessID=" + strAssessmentID + "&Archive=N";
                                    }
                                    else
                                    {
                                        //Assessment not set to Archived - show icon
                                        strMessage = DataProxy.LoadString("CONFIRMARCHIVE", strCurrentLanguage);

                                        hlArchive.ImageUrl = @"\images\Archive icon.png";
                                        hlArchive.Attributes["onclick"] = String.Format("return confirm('{0}');", strMessage); 
                                        hlArchive.NavigateUrl = "SearchAll.aspx?ArchiveAssessID=" + strAssessmentID + "&Archive=Y";
                                    }
                                }
                            }
                            else
                            {
                                //Check if assessment has been marked 'Completed' and flag appropriately for user
                                if (DataProxy.CheckAssessmentComplete(intAssessID))
                                    hlEdit.Text = DataProxy.LoadString("EDITASSESSMENTLABEL", strCurrentLanguage);
                                else
                                    hlEdit.Text = DataProxy.LoadString("EDITINCOMPLETEASSESSMENTLABEL", strCurrentLanguage);
                            }

                            hlEdit.NavigateUrl = strURL;

                        }
                    }
                    else
                    { 
                        hlEdit.Visible = false; 
                    }
                    //OLD LOGIC
                    //if (strUserName != "")
                    //    //strAssessmentID = DataProxy.GetAssessmentIDForUser(Convert.ToInt32(dr["SpeciesID"]), Convert.ToInt32(dr["CountryID"]), strUserName);
                    //    strAssessmentID = dr["AssessmentIDUser"].ToString();

                  
                    //if (strUserName != "")
                    //{
                    //    if (strUserName.ToUpper() == dr["UserName"].ToString().ToUpper() && strAssessmentID != "0")
                    //    {
                    //        hlEdit.Visible = true;
                    //        hlEdit.NavigateUrl = strURL;
                    //        hlEdit.Text = DataProxy.LoadString("EDITASSESSMENTLABEL", strCurrentLanguage);
                    //    }
                    //}
                    //else
                    //    hlEdit.Visible = false;
                    //END OLD LOGIC
                }

                if (e.Row.FindControl("hlSpeciesResults") != null)
                {
                    hlSpeciesResults = (HyperLink)e.Row.FindControl("hlSpeciesResults");
                
                    string strURL = "";
                    if (strAssessmentID != "0")
                    {
                        strURL = "AssessmentResults.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                      dr["SpeciesID"].ToString() + "&CountryID=" + dr["CountryID"].ToString();
                    }

                    hlSpeciesResults.NavigateUrl = strURL;
                }
                //JK: No idea why I was doing this - maybe to prevent dups with a previous query's dataset, but causing rows to be hidden in error - commenting out.
                //Session["PreviousRow"] = dr["SpeciesID"].ToString() + "-" + dr["CountryID"].ToString();

                //if (strPreviousRow == Session["PreviousRow"].ToString())
                //    e.Row.Visible = false;
            }
        }

        protected string ParseText(string sText, string sColName)
        {
            string sParsedTest = sText;
            int iUse = 0;

            switch (sColName.ToUpper())
            {
                case "NOTES":
                    iUse = 100;
                    break;
                case "ADDRESS":
                    iUse = 35;
                    break;
                case "NAME":
                    iUse = 25;
                    break;
                case "CERTMAIL":
                    iUse = 25;
                    break;
            }

            if (sText.Length > iUse)
            {
                if (sText.IndexOf("\\") > 0)
                {
                    int iLength = sText.Substring(0, iUse).LastIndexOf("\\");
                    if (iLength > 0)
                        sParsedTest = sText.Substring(0, iLength);
                    else
                        sParsedTest = sText.Substring(0, iUse);
                }
                else
                {
                    int iLength = sText.Substring(0, iUse).LastIndexOf(" ");
                    if (iLength > 0)
                        sParsedTest = sText.Substring(0, iLength);
                    else
                        sParsedTest = sText.Substring(0, iUse);
                }
            }
            return sParsedTest;
        }

        protected void GetSearchResults()
        {
            bool bCountrySearch = false;
            bool bSpeciesSearch = false;

            //can't verify string value in case it's not English, so we're going to assume positions do NOT change in drop-down:
            int assessmentSearch = ddlAssessments.SelectedIndex;
            string assessmentFilter = "ALL"; //adding this variable to make it more memorable / readable in logic

            switch (assessmentSearch)
            {
                case 0:
                    assessmentFilter = "ALL";
                    break;

                case 1:
                    assessmentFilter = "WITH";
                    break;
                case 2:
                    assessmentFilter = "WITHOUT";
                    break;
            }

            bool bUserOnly = chkUserResultsOnly.Checked;
            DataTable dtResults = null;
            string strWrappedText = "";

            if (txtSpecies.Text.Trim() != "")
                strWrappedText = DataProxy.WrapSearchText(txtSpecies.Text.Trim());
            else
            { 
                if (Session["txtSpeciesValue"] != null)
                    strWrappedText = DataProxy.WrapSearchText(Session["txtSpeciesValue"].ToString());
            }

            if (ddlCountries.SelectedIndex > 0) //|| Session["ddlCountriesValue"] != null)
                bCountrySearch = true;

            if (txtSpecies.Text.Trim() != "") // || Session["txtSpeciesValue"] != null)
                bSpeciesSearch = true;

            int intUserID = DataProxy.GetUserID(strUserName);
            int intCountryID = 0;

            if (ddlCountries.SelectedValue != "")
                intCountryID = Convert.ToInt32(ddlCountries.SelectedValue);
            //else
            //{
            //    if (Session["ddlCountriesValue"] != null)
            //    {
            //        intCountryID = Convert.ToInt32(Session["ddlCountriesValue"]);
                 
            //    }
            //}

            if (strUserName == "" && chkUserResultsOnly.Checked)
            {
                lblError.Text = DataProxy.LoadString("SEARCHRESULTSNOCURRENTUSER", strCurrentLanguage);
            }

            if (assessmentFilter == "ALL")
                dtResults = DataProxy.GetAssessmentSearchResults(intCountryID, strWrappedText, intUserID);      
            else
                dtResults = DataProxy.GetFilteredAssessmentSearchResults(intCountryID, strWrappedText, intUserID, assessmentFilter);      

            if (chkUserResultsOnly.Checked)
            //Limit results to current User's ID...
            {
                if (dtResults != null && intUserID != 0)
                    dtResults.DefaultView.RowFilter = "UserID = " + intUserID.ToString();
            }
            /*OLD LOGIC
            if (bCountrySearch && bSpeciesSearch)
            {
                if (chkUserResultsOnly.Checked || strUserName != "")
                    dtResults = DataProxy.GetSearchResultsCountryandSpecies(strWrappedText, ddlCountries.SelectedValue, true);
                else
                    dtResults = DataProxy.GetSearchResultsCountryandSpecies(strWrappedText, ddlCountries.SelectedValue, false);
            }

            if (bCountrySearch && !bSpeciesSearch)
            {
                if (chkUserResultsOnly.Checked || strUserName != "")
                    dtResults = DataProxy.GetSearchResultsCountry(ddlCountries.SelectedValue, true);
                else
                    dtResults = DataProxy.GetSearchResultsCountry(ddlCountries.SelectedValue, false);
            }

            if (bSpeciesSearch && !bCountrySearch ||
                (!bSpeciesSearch && !bCountrySearch)) //Neither one checked - just get all with 'blank' species filter
            {
                if (chkUserResultsOnly.Checked || strUserName != "")
                    dtResults = DataProxy.GetSearchResultsSpecies(strWrappedText, true);
                else
                    dtResults = DataProxy.GetSearchResultsSpecies(strWrappedText, false);
            }

            */
            grdResults.DataSource = dtResults;
            grdResults.DataBind();
            grdResults.Visible = true;
            Session["dvResults"] = dtResults.DefaultView;


            if (txtSpecies.Text.Trim() == "" && ddlCountries.SelectedValue == "")
            {
                lblReportTitle.Text = DataProxy.LoadString("ASSESSMENTSLABEL", strCurrentLanguage);
            }

            else
            {
                if (ddlCountries.SelectedValue != "")
                {
                    if (txtSpecies.Text.Trim() != "")
                        lblReportTitle.Text = txtSpecies.Text.Trim().ToUpper() + " " + DataProxy.LoadString("ASSESSMENTSLABELFILTER", strCurrentLanguage) +
                        " " + ddlCountries.SelectedItem.Text;
                    else
                    {
                        lblReportTitle.Text += " " + ddlCountries.SelectedItem.Text;
                    }
                }
                else
                {
                    if (txtSpecies.Text.Trim() != "")
                        lblReportTitle.Text = txtSpecies.Text.Trim().ToUpper() + " " + DataProxy.LoadString("ASSESSMENTSLABELFILTERNC", strCurrentLanguage);
                }

            }

            //Put it all together:
            if (ddlAssessments.SelectedIndex > 0)
                lblReportTitle.Text = dtResults.Rows.Count.ToString() + " " + ddlAssessments.SelectedValue.ToLower() + " " + lblReportTitle.Text;
            else
                lblReportTitle.Text = dtResults.Rows.Count.ToString() + " " + lblReportTitle.Text;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(grdResults);
        }

        protected void ExportGrid(GridView gv)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", strFileName));
            HttpContext.Current.Response.ContentType = "application/excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Table table = new Table();
            TableRow title = new TableRow();
            title.BackColor = System.Drawing.Color.Green;
            TableCell titlecell = new TableCell();
            titlecell.ColumnSpan = intColCount;//Should span across all columns
            Label lbl = new Label();
            lbl.Text = lblReportTitle.Text;
            titlecell.Controls.Add(lbl);
            title.Cells.Add(titlecell);
            table.Rows.Add(title);
            table.GridLines = gv.GridLines;
            //   add the header row to the table
            if (!(gv.Caption == null))
            {
                TableCell cell = new TableCell();
                cell.Text = gv.Caption;
                cell.ColumnSpan = intColCount;
                TableRow tr = new TableRow();
                tr.Controls.Add(cell);
                table.Rows.Add(tr);
            }

            if (!(gv.HeaderRow == null))
            {
                table.Rows.Add(gv.HeaderRow);
            }
            //   add each of the data rows to the table
            foreach (GridViewRow row in gv.Rows)
            {
                table.Rows.Add(row);
            }
            //   add the footer row to the table
            if (!(gv.FooterRow == null))
            {
                table.Rows.Add(gv.FooterRow);
            }
            //   render the table into the htmlwriter
            table.RenderControl(htw);

            string headerTable = @"<table width='100%' class='TestCssStyle'><tr><td><h4>Report </h4> </td><td></td><td><h4>" + DateTime.Now.ToString("dd MMM yyyy") + "</h4></td></tr></table>";
            HttpContext.Current.Response.Write(headerTable);
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                grdResults.AllowPaging = false;
                GetSearchResults();

                ExportGrid(grdResults);
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

        protected void PrintGrid(GridView gv)
        {
            gv.AllowPaging = false;
            GetSearchResults();

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

        protected void Search()
        {
            GetSearchResults();
            btnPrint.Visible = true;
            btnExport.Visible = true;
            lblReportTitle.Visible = true;
            lblGridMessage.Text = "";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Session["txtSpeciesValue"] = txtSpecies.Text.Trim();

                Search();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void ddlCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ddlCountriesValue"] = ddlCountries.SelectedValue;
        }
        
    }
}