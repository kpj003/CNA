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
    public partial class AssessorDashboard : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strUserName = "";
        int intLanguageID = 0;
        public string ARKIVESpeciesName = "";
        string strAssessorUID = "";
        int intCountryID = 0;
        int intUserID = 0;
        int intColCount = 5;
        string strFileName = "AssessorDashboard.xls";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();

            if (Request.QueryString["UID"] != null)
            {
                strUserName = Request.QueryString["UID"].ToString();
                strAssessorUID = Request.QueryString["UID"].ToString();
                intUserID = DataProxy.GetUserID(strAssessorUID);
                Session["AssessorUserID"] = intUserID;
                Session["AssessorUserName"] = strAssessorUID;
                hlProfile.NavigateUrl = "EditUser.aspx?UserID=" + Session["AssessorUserID"].ToString();
                hlProfile.Text = DataProxy.LoadString("EDITUSERPROFILELABEL", strCurrentLanguage);
                lblEditUserInfo.Text = DataProxy.LoadString("EDITUSERPROFILEINSTRUCTIONS", strCurrentLanguage);
            }

            intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);
            lblPageTitle.Text = DataProxy.LoadString("DASHBOARDLINK", strCurrentLanguage);
            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("INCOMPLETEASSESSMENTS", strCurrentLanguage);


            lblIncompleteAssessments.Text = DataProxy.LoadString("INCOMPLETEASSESSMENTS", strCurrentLanguage);
            lblTextParagraph.Text = DataProxy.LoadString("ASSESSORPARAGRAPHTEXT", strCurrentLanguage);
            grdResults.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);
            

            if (!Page.IsPostBack)
                GetSearchResults();
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

        protected void grdResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink hlEdit = null;
            HyperLink hlSpeciesResults = null;
            HyperLink hlAdd = null;
            LinkButton lnkDelete = null;
            Label lblDelete = null;

            string strUserName = "";


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

                //Set fourth column/"Total Assessments" header
                //e.Row.Cells[3].Text = DataProxy.LoadString("TOTALASSESSMENTSLABEL", strCurrentLanguage);

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strAssessmentID = "";
                int intSpeciesID = 0;
                int intCountryID = 0;
                int intAssessUserID = 0;
                int intUserID = 0;
                int intAssessmentID = 0;

                if (dr["SpeciesID"] != null)
                    int.TryParse(dr["SpeciesID"].ToString(), out intSpeciesID);

                if (dr["UserID"] != null)
                    int.TryParse(dr["UserID"].ToString(), out intAssessUserID);

                if (dr["CountryID"] != null)
                    int.TryParse(dr["CountryID"].ToString(), out intCountryID);

                if (dr["AssessmentID"] != null)
                    strAssessmentID = dr["AssessmentID"].ToString();

                if (e.Row.FindControl("hlAdd") != null)
                {
                    hlAdd = (HyperLink)e.Row.FindControl("hlAdd");

                    string strURL = "CreateAssessment.aspx?AssessmentID=0" + "&SpeciesID=" +
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
                        if (intAssessUserID == intCurrentUserID)
                        {
                            lnkDelete.Visible = true;
                            int intAssessID = Convert.ToInt32(strAssessmentID);
                            lnkDelete.CommandArgument = intAssessID.ToString();
                            lblDelete = (Label)e.Row.FindControl("lblDelete");
                            if (lblDelete != null)
                                lblDelete.Text = DataProxy.LoadString("DELETETEXT", strCurrentLanguage);
                        }
                        else
                            lnkDelete.Visible = false;
                    }
                    else
                        lnkDelete.Visible = false;
                }

                if (e.Row.FindControl("hlEdit") != null)
                {
                    hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    string strURL = "EditAssessment.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                  intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();

                    if (intCurrentUserID != 0)
                    {
                        if (intAssessUserID == intCurrentUserID)
                        {
                            hlEdit.Visible = true;
                            hlEdit.NavigateUrl = strURL;

                            //Check if assessment has been marked 'Completed' and flag appropriately for user
                            int intAssessID = Convert.ToInt32(strAssessmentID);
                            if (DataProxy.CheckAssessmentComplete(intAssessID))
                                hlEdit.Text = DataProxy.LoadString("EDITASSESSMENTLABEL", strCurrentLanguage);
                            else
                                hlEdit.Text = DataProxy.LoadString("EDITINCOMPLETEASSESSMENTLABEL", strCurrentLanguage);

                        }
                    }
                    else
                    {
                        hlEdit.Visible = false;
                    }
                    
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
            DataTable dtResults = null;
            
            int intUserID = DataProxy.GetUserID(strUserName);
            int intCountryID = 0;
           
            dtResults = DataProxy.GetAssessmentSearchResults(intUserID);
           
            grdResults.DataSource = dtResults;
            grdResults.DataBind();
            grdResults.Visible = true;
            Session["dvResultsDash"] = dtResults.DefaultView;
            btnPrint.Visible = true;
            btnExport.Visible = true;
            lblReportTitle.Visible = true;
        }

        protected void grdResults_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //JK: Don't have to do this here anymore - being done via RowCommand

        }
        protected void grdResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            grdResults.PageIndex = e.NewPageIndex;
            if (Session["dvResults"] != null)
                grdResults.DataSource = (DataView)Session["dvResults"];

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

    }
}