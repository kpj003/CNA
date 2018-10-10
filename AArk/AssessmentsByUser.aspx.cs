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
    public partial class AssessmentsByUser : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        int intColCount = 8;
        string strFileName = "AssessmentsByUser.xls";
     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();


            lblPageTitle.Text = DataProxy.LoadString("ASSESSMENTSBYUSERLABEL", strCurrentLanguage);
            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("ASSESSMENTSBYUSERREPORTLABEL", strCurrentLanguage);

            lblUsers.Text = DataProxy.LoadString("USERNAMELABEL", strCurrentLanguage);
            btnSearch.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);

            if (!Page.IsPostBack)
            {
                GetAssessmentsUser();
                PopulateUsersList();
            }
            grdAssessUser.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);

        }
        protected void PopulateUsersList()
        {

            try
            {
                DataTable dt = null;

                dt = DataProxy.GetAssessmentUsers();

                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = dt.Columns["UserFullName"].ToString();
                ddlUsers.DataValueField = dt.Columns["UserID"].ToString(); 
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem(DataProxy.LoadString("ALLASSESSORSLABEL",strCurrentLanguage), String.Empty));
                ddlUsers.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void GetSearchResults()
        {
            DataTable dt = null;

            int intUserID = 0;
            string strUser = "";

            if (ddlUsers.SelectedValue != "")
            {
                intUserID = Convert.ToInt32(ddlUsers.SelectedValue);
                strUser = ddlUsers.SelectedItem.Text;
                lblReportTitle.Text += " " + strUser;
            }

            dt = DataProxy.GetAssessmentsByUser();
            DataView dv = dt.DefaultView;

            if (intUserID > 0)
                dv.RowFilter = "UserID = " + intUserID;

            grdAssessUser.DataSource = dv;
            grdAssessUser.DataBind();
            grdAssessUser.Visible = true;
            Session["dvAssessUser"] = dv;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetSearchResults();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        
        protected void GetAssessmentsUser()
        {
            DataTable dt = null;

            dt = DataProxy.GetAssessmentsByUser();

            DataView dv = dt.DefaultView;

            //if (dv != null)
            //    dv.Sort = "Approved ASC";

            grdAssessUser.DataSource = dt;
            grdAssessUser.DataBind();
            grdAssessUser.Visible = true;
            Session["dvAssessUser"] = dt.DefaultView;
            btnPrint.Visible = true;
            btnExport.Visible = true;
            lblReportTitle.Visible = true;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(grdAssessUser);
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


        protected void grdAssessUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            Label lblDateCreated = null;
            HyperLink hlSpeciesResults = null;
         
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set first column/"Last Name" header
                e.Row.Cells[0].Text = DataProxy.LoadString("LASTNAMELABEL", strCurrentLanguage);

                //Set second column/"First Name" header
                e.Row.Cells[1].Text = DataProxy.LoadString("FIRSTNAMELABEL", strCurrentLanguage);

                //Set third column/"Species" header
                e.Row.Cells[2].Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);

                //Set fourth column/"Country" header
                e.Row.Cells[3].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);

                //Set fifth column/"Date" header
                e.Row.Cells[4].Text = DataProxy.LoadString("ASSESSMENTDATELABEL", strCurrentLanguage);

                //Set eighth column/"Status" header
                //e.Row.Cells[7].Text = DataProxy.LoadString("STATUS", strCurrentLanguage);

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strAssessmentID = "";

                if (dr["DateCreated"] != null)
                {
                    if (e.Row.FindControl("lblDateCreated") != null)
                    {
                        DateTime dtDate;
                        string strDate = dr["DateCreated"].ToString();
                        DateTime.TryParse(strDate, out dtDate);

                        lblDateCreated = (Label)e.Row.FindControl("lblDateCreated");
                        lblDateCreated.Text = dtDate.ToString("dd MMM yyyy");
                    }
                }
                if (e.Row.FindControl("hlSpeciesResults") != null)
                {
                    hlSpeciesResults = (HyperLink)e.Row.FindControl("hlSpeciesResults");

                    string strURL = "";
                    if (strAssessmentID != "0")
                    {
                        strURL = "AssessmentResults.aspx?AssessmentID=" + dr["AssessmentID"].ToString() + "&SpeciesID=" +
                                      dr["SpeciesID"].ToString() + "&CountryID=" + dr["CountryID"].ToString();
                    }

                    hlSpeciesResults.NavigateUrl = strURL;
                }

            }
        }

        protected void grdAssessUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            grdAssessUser.PageIndex = e.NewPageIndex;
            if (Session["dvApproveUserResults"] != null)
                grdAssessUser.DataSource = (DataView)Session["dvAssessUser"];

            grdAssessUser.DataBind();
            lblGridMessageAssessUser.Text = "Page " + (grdAssessUser.PageIndex + 1).ToString() + " of " + grdAssessUser.PageCount;
        }
    }
}