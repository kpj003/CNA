using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Data.Utilities;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

namespace AArk
{
    public partial class NationalAssessmentReport : System.Web.UI.Page
    {
        int intColCount = 3;
        string strFileName = "NationalAssessmentReport.xls";
        string strCurrentLanguage = "";
        int intLanguageID = 0;
        string strOldCountry = "";
        string strOldSpecies = "";
        
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

            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            btnSearch.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);
            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
            //btnPrint.Attributes.Add("onclick", "javascript:CallPrint('divPrint');");

            //lblSpecies.Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);
            lblPageTitle.Text = DataProxy.LoadString("NATIONALASSESSMENTSLABEL", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("NATIONALASSESSMENTSLABEL", strCurrentLanguage);
            lblSortBy.Text = DataProxy.LoadString("SORTBYLABEL", strCurrentLanguage);
            rbSort.Items[0].Text = DataProxy.LoadString("PRIORITYLABEL", strCurrentLanguage);
            rbSort.Items[1].Text = DataProxy.LoadString("TAXONOMICLABEL", strCurrentLanguage);
           

            string strHeader = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7LINK", strCurrentLanguage);
            if (strHeader != "" && strHeader != null)
                lblTriggers.Text = strHeader.Substring(0, 1).ToUpper() + strHeader.Substring(1, strHeader.Length - 2);

            grdAssessUser.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);

            if (!Page.IsPostBack)
            {
                //GetAssessmentsUser();
                PopulateTriggersList();
                PopulateCountriesList();
            }
        }

        protected void PopulateTriggersList()
        {
            try
            {
                DataTable dt = null;
                string strAllTriggers = DataProxy.LoadString("ALLRECSLABEL", strCurrentLanguage);

                dt = DataProxy.GetTriggersList(intLanguageID);

                ddlTriggers.DataSource = dt;
                ddlTriggers.DataTextField = dt.Columns["TriggerShortName"].ToString();
                ddlTriggers.DataValueField = dt.Columns["TriggerID"].ToString();
                ddlTriggers.DataBind();
                ddlTriggers.Items.Insert(0, new ListItem(strAllTriggers, String.Empty));
                ddlTriggers.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
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

        protected void GetSearchResults()
        {
            DataTable dt = null;
            string strTriggerShortName = "";
            lblGridMessageAssessUser.Text = "";
     
            if (ddlTriggers.SelectedValue != "")
                strTriggerShortName = ddlTriggers.SelectedItem.Text;


            dt = DataProxy.GetRecommendationList(intLanguageID, strTriggerShortName, rbSort.SelectedValue);

            DataView dv = dt.DefaultView;
            string strFilter = "1 = 1";

            
            if (dv != null)
            {
                //Limit data based on search criteria.
                if (ddlCountries.SelectedValue != "")
                {
                    strFilter += " AND CountryID = " + ddlCountries.SelectedValue;
                }
            }
           
            dv.RowFilter = strFilter;
            grdAssessUser.DataSource = dv;
            grdAssessUser.DataBind();
            grdAssessUser.Visible = true;
            Session["dvAssessUser"] = dv;
            SetReportTitle(strTriggerShortName);

        }

        protected void SetReportTitle(string strTriggerShortName)
        {
            string strTotals = "";
            string strReportTitleText = lblReportTitle.Text;
            
            if (ddlCountries.SelectedValue != "")
            {
                if (strCurrentLanguage == "English")
                    strReportTitleText +=  " in " + ddlCountries.SelectedItem.Text + " "; 
                else
                {
                    if (strCurrentLanguage == "Spanish")
                        strReportTitleText += " para " + ddlTriggers.SelectedItem.Text + " ";                
                    else
                        strReportTitleText += " - " + ddlTriggers.SelectedItem.Text + " ";
                }
            }
                     
            if (strCurrentLanguage == "English")
            {
                strReportTitleText += " for " + ddlTriggers.SelectedItem.Text + " ";
            }
            else
            {
                if (strCurrentLanguage == "Spanish")
                {
                    strReportTitleText += " en " + ddlCountries.SelectedItem.Text + " ";
                }
                else
                {
                    strReportTitleText += " - " + ddlTriggers.SelectedItem.Text + " ";
                }
            }
        
            if (ddlCountries.SelectedValue != "")
            {
                strReportTitleText += "<br>";
      
                DataTable dt = DataProxy.GetTriggerCounts(intLanguageID, Convert.ToInt32(ddlCountries.SelectedValue));
                if (dt != null)
                {
                    DataView dv = dt.DefaultView;
                    string strFilter = "1=1 ";
                    if (strTriggerShortName.Trim() != "") 
                       strFilter += " AND TriggerShortName = '" + strTriggerShortName.Trim() + "'";


                    dv.RowFilter = strFilter;
                    int icount = dv.Count;
                    for (int i = 0; i < icount; i++)
                    {
                        if (Convert.ToInt32(dv[i]["TriggerTotal"]) > 0)
                        {
                            if (strTriggerShortName.Trim() == "")
                                strTotals += dv[i]["TriggerTotal"].ToString() + " " + DataProxy.LoadString("RECOMMENDATIONCOUNTLABEL", strCurrentLanguage) + " " + dv[i]["TriggerShortName"].ToString() + "<br>";
                            else
                                strTotals += " " + dv[i]["TriggerTotal"].ToString() + " " + DataProxy.LoadString("RECLABEL", strCurrentLanguage);
                        }
                    }
                }
            }

            strReportTitleText += strTotals;
            lblReportTitle.Text = strReportTitleText;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetSearchResults();
                btnPrint.Visible = true;
                btnExport.Visible = true;
                lblReportTitle.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        
        

        protected void grdAssessUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            Literal ltlRecs = null;
            string strCurrentCountry = "";
            string strCurrentSpecies = "";
            Label lblGridCountry = null;
            int intTotalAssessments = 0;
            string strAssessmentID = "0";
       
            HyperLink lblSpecies = null;
         
        
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set "Species" header
                e.Row.Cells[0].Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);

                //Set "Country" header
                e.Row.Cells[1].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);

                //Set "Recommendations" header
                string strHeader = DataProxy.LoadString("ASSESSMENTOVERVIEWCONTENT7LINK", strCurrentLanguage);
                if (strHeader != "" && strHeader != null)
                    e.Row.Cells[2].Text = strHeader.Substring(0, 1).ToUpper() + strHeader.Substring(1, strHeader.Length - 2);

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dr["Total"] != null)
                    intTotalAssessments = Convert.ToInt32(dr["Total"]);

                  if (e.Row.FindControl("lblGridCountry") != null)
                        lblGridCountry = (Label)e.Row.FindControl("lblGridCountry");

                  if (e.Row.FindControl("lblSpecies") != null)
                        lblSpecies = (HyperLink)e.Row.FindControl("lblSpecies");
                  
                strCurrentCountry = dr["CountryID"].ToString();
                strCurrentSpecies = dr["SpeciesID"].ToString();

                if (strOldSpecies == strCurrentSpecies && strOldCountry == strCurrentCountry)
                {
                    lblGridCountry.Visible = false;
                    lblSpecies.Visible = false;
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        tc.BorderStyle = BorderStyle.None;
                        tc.BorderWidth = 0;
                    }
                }
                else
                {
                    string strURL = "";
                    int intSpeciesID = 0;
                    int intCountryID = 0;
                    lblGridCountry.Visible = true;
                    lblSpecies.Visible = true;

                    if (dr["SpeciesID"] != null)
                        intSpeciesID = Convert.ToInt32(dr["SpeciesID"]);

                    if (dr["CountryID"] != null)
                        intCountryID = Convert.ToInt32(dr["CountryID"]);

                    //Set URL for Species - if 1 - AssessmentResults, if > 1 assessment - ConsolidatedSpeciesAssessment.aspx.
                    if (intTotalAssessments > 1)
                    {
                        strURL = "ConsolidatedSpeciesAssessment.aspx?SpeciesID=" + intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();
                    }
                    else
                    {
                        strAssessmentID = DataProxy.GetAssessmentIDForSpeciesCountry(intSpeciesID, intCountryID);
                        strURL = ConfigurationManager.AppSettings["URLPath"] + "AssessmentResults.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                        intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();

                       // strURL = @"~/AssessmentResults.aspx?AssessmentID=" + strAssessmentID + "&SpeciesID=" +
                                          // intSpeciesID.ToString() + "&CountryID=" + intCountryID.ToString();
                        
                        lblSpecies.NavigateUrl = strURL;
                    }
                }
                
                if (e.Row.FindControl("ltlRecs") != null)
                {
                    ltlRecs = (Literal)e.Row.FindControl("ltlRecs");
                    double intPct = 0;
                    if (dr["PctTotal"] != null)
                        intPct = Convert.ToDouble(dr["PctTotal"]);

                    string strLiteral = "";
                    strLiteral = dr["TriggerShortName"].ToString() + " (n=" + dr["Total"].ToString() + ", " + intPct.ToString("#.##") + "%)<br>";
                    ltlRecs.Text = strLiteral;
                }
                strOldCountry = strCurrentCountry;
                strOldSpecies = strCurrentSpecies;
       
            }
        }

        protected void grdAssessUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            
            grdAssessUser.PageIndex = e.NewPageIndex;
            if (Session["dvAssessUser"] != null)
                grdAssessUser.DataSource = (DataView)Session["dvAssessUser"];

            grdAssessUser.DataBind();
            lblGridMessageAssessUser.Text = "Page " + (grdAssessUser.PageIndex + 1).ToString() + " of " + grdAssessUser.PageCount;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(grdAssessUser);
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
                grdAssessUser.AllowPaging = false;
                GetSearchResults();

                ExportGrid(grdAssessUser);
                //Not working - showing only first page + all controls...
                //Response.Clear();
                ////Response.Buffer = true; //ADDED
                //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);  //ADDED 2

                //Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.xls";
                //grdAssessUser.AllowPaging = false;
                //using (StringWriter sw = new StringWriter())
                //{
      
                //    HtmlTextWriter hw = new HtmlTextWriter(sw);
                //    Panel1.RenderControl(hw);
                //    Response.Output.Write(sw.ToString());
                //    Response.Flush();
                //    Response.End();
                //}
                //END - Not working...

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
    }
}