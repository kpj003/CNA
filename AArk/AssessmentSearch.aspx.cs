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
    public partial class AssessmentSearch : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        string strUserName = "";
        int intColCount = 5;
        string strFileName = "AssessmentSearch.xls";
       
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

            lblPageTitle.Text = DataProxy.LoadString("NEWSEARCHTITLE", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("COMPLETEDASSESSMENTSLABEL", strCurrentLanguage);
      
            lblContent.Text = DataProxy.LoadString("SEARCHMAIN", strCurrentLanguage);
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            btnSearch.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);
            lblSpecies.Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);

            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);

            grdResults.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);

            //chkUserResultsOnly.Text = DataProxy.LoadString("CHECKBOXRESULTSTEXT", strCurrentLanguage);
           
            if (!Page.IsPostBack)
                PopulateCountriesList();
        }

        protected void PopulateCountriesList()
        {
            try
            {
                DataTable dtCountries = null;
                
                //For this search - we will display ALL countries that have an assessment for public to view results.
                bool bAssessments = true;

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
        
        protected void grdResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink hlEdit = null;
            HyperLink hlSpeciesResults = null;
            HyperLink hlConsolidated = null;
            HyperLink hlAdd = null;
            string strUserName = "";
            Label lblDateAssessed = null;
            int intTotalAssessments = 0;
            Label lblArchive = null;
        
            
            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (Session["UserName"] != null)
                strUserName = Session["UserName"].ToString();

            //JK: No idea why I was doing this - maybe to prevent dups with a previous query's dataset, but causing rows to be hidden in error - commenting out.
            //if (Session["PreviousRow"] != null)
            //    strPreviousRow = Session["PreviousRow"].ToString();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set first column/"Country" header
                e.Row.Cells[0].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);

                //Set second column/"Species" header
                e.Row.Cells[1].Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);

                //Set third column/"Consolidated" header
                e.Row.Cells[2].Text = DataProxy.LoadString("CONSOLIDATEDHEADER", strCurrentLanguage);

                //Set fourth column/"Common Name" header
                e.Row.Cells[3].Text = DataProxy.LoadString("COMMONNAMELABEL", strCurrentLanguage);

                //Set fifth column/"Local Common Name" header
                e.Row.Cells[4].Text = DataProxy.LoadString("LOCALCOMMONNAMELABEL", strCurrentLanguage);

                //Set sixth column/"User Name" header
                e.Row.Cells[5].Text = DataProxy.LoadString("ASSESSEDBYLABEL", strCurrentLanguage);

                //Set seventh column/"Date Assessed" header
                e.Row.Cells[6].Text = DataProxy.LoadString("ASSESSMENTDATELABEL", strCurrentLanguage);
                
                //Set eighth column/"Status" header
                e.Row.Cells[7].Text = DataProxy.LoadString("ARCHIVEDSTATUSLABEL", strCurrentLanguage);

            }
        
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strAssessmentID = "";
                int intSpeciesID = 0;
                int intCountryID = 0;
               
                if (dr["SpeciesID"] != null)
                    int.TryParse(dr["SpeciesID"].ToString(), out intSpeciesID);

                if (dr["CountryID"] != null)
                    int.TryParse(dr["CountryID"].ToString(), out intCountryID);

                if (dr["TotalAssessments"] != null)
                    intTotalAssessments = Convert.ToInt32(dr["TotalAssessments"]);

                strAssessmentID = dr["AssessmentID"].ToString();

                if (e.Row.FindControl("lblDateAssessed") != null)
                {
                    
                    DateTime dtDate;
                    string strDate;

                    lblDateAssessed = (Label)e.Row.FindControl("lblDateAssessed");

                    if (dr["DateCreated"] != null)
                    {
                        dtDate = Convert.ToDateTime(dr["DateCreated"]);
                        strDate = dtDate.ToString("dd MMM yyyy");
                        lblDateAssessed.Text = strDate;
                    }
                }

                if (e.Row.FindControl("lblArchive") != null)
                {
                    lblArchive = (Label)e.Row.FindControl("lblArchive");
                    lblArchive.Visible = true;

                    if (DataProxy.CheckAssessmentArchived(Convert.ToInt32(strAssessmentID)))
                        lblArchive.Text = DataProxy.LoadString("ARCHIVETEXT", strCurrentLanguage);
                    else
                        lblArchive.Text = "";
                }

                if (e.Row.FindControl("hlAdd") != null)
                {
                    hlAdd = (HyperLink)e.Row.FindControl("hlAdd");
                    
                    string strURL = "CreateAssessment.aspx?AssessmentID=0"  + "&SpeciesID=" +
                                  dr["SpeciesID"].ToString() + "&CountryID=" + dr["CountryID"].ToString();

                    hlAdd.NavigateUrl = strURL;
                    hlAdd.Text = DataProxy.LoadString("ADDASSESSMENTLABEL", strCurrentLanguage);
                }

                if (e.Row.FindControl("hlEdit") != null)
                {   
                    hlEdit = (HyperLink)e.Row.FindControl("hlEdit");

                    if (strUserName != "")
                        //strAssessmentID = DataProxy.GetAssessmentIDForUser(Convert.ToInt32(dr["SpeciesID"]), Convert.ToInt32(dr["CountryID"]), strUserName);
                        strAssessmentID = dr["AssessmentIDUser"].ToString();

                    string strURL = "EditAssessment.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                  dr["SpeciesID"].ToString() + "&CountryID=" + dr["CountryID"].ToString();

                    if (strUserName != "")
                    {
                        if (strUserName.ToUpper() == dr["UserName"].ToString().ToUpper() && strAssessmentID != "0")
                        {
                            hlEdit.Visible = true;
                            hlEdit.NavigateUrl = strURL;
                            hlEdit.Text = DataProxy.LoadString("EDITASSESSMENTLABEL", strCurrentLanguage);
                        }
                    }
                    else
                        hlEdit.Visible = false;
                }

                
                if (e.Row.FindControl("hlConsolidated") != null)
                {
                    hlConsolidated = (HyperLink)e.Row.FindControl("hlConsolidated");
                    hlConsolidated.Text = DataProxy.LoadString("CONSOLIDATEDASSESSMENTLINK", strCurrentLanguage);

                    string strURL = "ConsolidatedSpeciesAssessment.aspx?SpeciesID=" + dr["SpeciesID"].ToString() + "&CountryID=" + dr["CountryID"].ToString();

                    hlConsolidated.NavigateUrl = strURL;

                    if (intTotalAssessments > 1)
                        hlConsolidated.Visible = true;
                    else
                        hlConsolidated.Visible = false;
                }

                if (e.Row.FindControl("hlSpeciesResults") != null)
                {
                    hlSpeciesResults = (HyperLink)e.Row.FindControl("hlSpeciesResults");
                   
                    string strURL = "AssessmentResults.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                  dr["SpeciesID"].ToString() + "&CountryID=" + dr["CountryID"].ToString();

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

        protected void GetSearchResults()
        {
            DataTable dtResults = null;
            string strWrappedText = DataProxy.WrapSearchText(txtSpecies.Text.Trim());

            int intCountryID = 0;

            if (ddlCountries.SelectedValue != "")
                intCountryID = Convert.ToInt32(ddlCountries.SelectedValue);

            dtResults = DataProxy.GetSearchResults(intCountryID, strWrappedText);

            grdResults.DataSource = dtResults;
            grdResults.DataBind();
            grdResults.Visible = true;
            Session["dvResults"] = dtResults.DefaultView;
            lblReportTitle.Text += " " + ddlCountries.SelectedItem.Text;
            lblGridMessage.Text = "";

            if (txtSpecies.Text.Trim() != "")
                lblReportTitle.Text += " - " + txtSpecies.Text.Trim();

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(grdResults);
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


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetSearchResults();
                btnPrint.Visible = true;
                btnExport.Visible = true;
                lblReportTitle.Visible = true;
                lblGridMessage.Text = "";
                
                //bool bUserOnly = chkUserResultsOnly.Checked;
             
                /* N/A for this page - not assessment/user driven...
                if (strUserName == "" && chkUserResultsOnly.Checked)
                {
                    lblError.Text = DataProxy.LoadString("SEARCHRESULTSNOCURRENTUSER", strCurrentLanguage);
                }*/
           
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

                if (chkUserResultsOnly.Checked)
                //Limit results to current User's ID...
                {
                    if (strUserName == "")
                    {
                        lblError.Text = DataProxy.LoadString("SEARCHRESULTSNOCURRENTUSER", strCurrentLanguage);
                    }
                    else
                    {
                        int iUserID = DataProxy.GetUserID(strUserName);
                        if (dtResults != null)
                            dtResults.DefaultView.RowFilter = "UserID = " + iUserID.ToString(); 
                    }
                }*/
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        
    }
}