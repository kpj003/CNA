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
    public partial class ApprovedUsers : System.Web.UI.Page
    {
        public string ARKIVESpeciesName = "";
        string strCurrentLanguage = "";
        int intColCount = 8;
        string strFileName = "ApprovedUsers.xls";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentLanguage"] == null)
            {
                Session["CurrentLanguage"] = "English";
                strCurrentLanguage = Session["CurrentLanguage"].ToString();
            }
            else
                strCurrentLanguage = Session["CurrentLanguage"].ToString();


            lblPageTitle.Text = DataProxy.LoadString("FALABEL", strCurrentLanguage);
          
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            btnSearch.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);
            lblRoleType.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);

            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("FALABEL", strCurrentLanguage);
            grdApproveUser.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);

     
            if (!Page.IsPostBack)
            {
                GetApprovedUsers();
                PopulateCountriesList();
                PopulateRolesList();
            }
        }

        protected void PopulateRolesList()
        {
            ddlRoles.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddlRoles.Items.Insert(1, new ListItem(DataProxy.LoadString("ASSESSORSLABEL", strCurrentLanguage), "Assessor"));
            ddlRoles.Items.Insert(2, new ListItem(DataProxy.LoadString("FACILITATORSLABEL", strCurrentLanguage), "Facilitator"));
            
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(grdApproveUser);
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
                grdApproveUser.AllowPaging = false;
                GetSearchResults();

                ExportGrid(grdApproveUser);
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


        protected void GetSearchResults()
        {
            DataTable dt = null;
            //bool bCountrySearch = false;

            //if (ddlCountries.SelectedIndex > 0)
            //    bCountrySearch = true;

            int intCountryID = 0;
            string strCountry = "";

            if (ddlCountries.SelectedValue != "")
            {
                intCountryID = Convert.ToInt32(ddlCountries.SelectedValue);
                strCountry = ddlCountries.SelectedItem.Text;
            }

            dt = DataProxy.GetApprovedUsers();
            DataView dv = dt.DefaultView;

            dv.RowFilter = "1=1 ";

            if (intCountryID > 0)
            {
                dv.RowFilter += "AND CountryExpertise LIKE '%" + strCountry + "%'";
            }

            if (ddlRoles.SelectedIndex > 0)
            {
                dv.RowFilter += "AND UserType = '" + ddlRoles.SelectedValue + "'";
            }

            grdApproveUser.DataSource = dv;
            grdApproveUser.DataBind();
            grdApproveUser.Visible = true;
            Session["dvUserRequests"] = dt.DefaultView;
            btnPrint.Visible = true;
            btnExport.Visible = true;
            lblReportTitle.Visible = true;
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

        protected void GetApprovedUsers()
        {
            DataTable dt = null;

            dt = DataProxy.GetApprovedUsers();

            DataView dv = dt.DefaultView;

            //if (dv != null)
            //    dv.Sort = "Approved ASC";

            grdApproveUser.DataSource = dt;
            grdApproveUser.DataBind();
            grdApproveUser.Visible = true;
            Session["dvUserRequests"] = dt.DefaultView;
            btnPrint.Visible = true;
            btnExport.Visible = true;
            lblReportTitle.Visible = true;
        }

        protected void grdApproveUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            grdApproveUser.PageIndex = e.NewPageIndex;
            if (Session["dvUserRequests"] != null)
                grdApproveUser.DataSource = (DataView)Session["dvUserRequests"];

            grdApproveUser.DataBind();
            lblGridMessageApproveUser.Text = "Page " + (grdApproveUser.PageIndex + 1).ToString() + " of " + grdApproveUser.PageCount;
        }


        protected void grdApproveUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink hlDeny = null;
            HyperLink hlApprove = null;
            Label lblASGMember = null;
            Label lblStatus = null;
            Label lblUserID = null;

            string strUserID = "";
            string strApproved = "";

            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set first column/"Last Name" header
                e.Row.Cells[0].Text = DataProxy.LoadString("LASTNAMELABEL", strCurrentLanguage);

                //Set second column/"First Name" header
                e.Row.Cells[1].Text = DataProxy.LoadString("FIRSTNAMELABEL", strCurrentLanguage);
                
                //Set third column/"Email" header
                e.Row.Cells[2].Text = DataProxy.LoadString("EMAILLABEL", strCurrentLanguage);

                //Set fourth column/"Country" header
                e.Row.Cells[3].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
              
                //Set fifth column/"Organization" header
                e.Row.Cells[4].Text = DataProxy.LoadString("ORGANIZATIONLABEL", strCurrentLanguage);

                //Set sixth column/"Expertise" header
                e.Row.Cells[5].Text = DataProxy.LoadString("USERCOUNTRIESTEXT", strCurrentLanguage);

                //Set seventh column/"ASGMember" header
                e.Row.Cells[6].Text = DataProxy.LoadString("ASGMEMBERLABEL", strCurrentLanguage);

                //Set eighth column/"UserType" header
                e.Row.Cells[7].Text = DataProxy.LoadString("USERTYPELABEL", strCurrentLanguage);

                //Set eighth column/"Status" header
                //e.Row.Cells[7].Text = DataProxy.LoadString("STATUS", strCurrentLanguage);

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dr["AmphibianSpecialistGroupMember"] != null)
                {
                    if (e.Row.FindControl("lblASGMember") != null)
                    {
                        lblASGMember = (Label)e.Row.FindControl("lblASGMember");

                        if (dr["AmphibianSpecialistGroupMember"].ToString() == "False")
                            lblASGMember.Text = "No";
                        else
                            lblASGMember.Text = "Yes";
                    }
                }

            }
        }

      
    }
}