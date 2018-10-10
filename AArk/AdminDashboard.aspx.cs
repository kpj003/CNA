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
    public partial class AdminDashboard : System.Web.UI.Page
    {
        public string ARKIVESpeciesName = "";
        string strCurrentLanguage = "";
        string strAssessorUID = "";
        string strUserName = "";
        int intCountryID = 0;
        int intLanguageID = 0;
        int intUserID = 0;
        int intColCount = 8;
        string strFileName = "ApproveUserRequest.xls";
        int intIncompleteColCount = 5;
        string strIncompletFileName = "IncompleteAssessments.xls";
        int intApproveCompleteColCount = 5;
        string strApproveCompletFileName = "ApproveCompleteAssessments.xls";

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
            else
            {
                if (Session["AssessorUserID"] != null)
                {
                    hlProfile.NavigateUrl = "EditUser.aspx?UserID=" + Session["AssessorUserID"].ToString();
                    hlProfile.Text = DataProxy.LoadString("EDITUSERPROFILELABEL", strCurrentLanguage);
                    lblEditUserInfo.Text = DataProxy.LoadString("EDITUSERPROFILEINSTRUCTIONS", strCurrentLanguage);
                }
            }

            btnPrint.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExport.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);

            btnPrintApproveCA.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExportApproveCA.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);
            btnPrintIncomplete.Text = DataProxy.LoadString("PRINTLABEL", strCurrentLanguage);
            btnExportIncomplete.Text = DataProxy.LoadString("EXPORTLABEL", strCurrentLanguage);

            lblIncompleteTitle.Text = DataProxy.LoadString("INCOMPLETEASSESSMENTS", strCurrentLanguage);
            lblReportTitle.Text = DataProxy.LoadString("APPROVEUSERREQUEST", strCurrentLanguage);
            lblApproveCATitle.Text = DataProxy.LoadString("APPROVECOMPLETEDASSESSMENTS", strCurrentLanguage);


            btnSearch.Text = DataProxy.LoadString("SEARCHLINK", strCurrentLanguage);
            intLanguageID = DataProxy.GetLanguageID(strCurrentLanguage);
            lblPageTitle.Text = DataProxy.LoadString("DASHBOARDLINK", strCurrentLanguage);
            lblIncompleteAssessments.Text = DataProxy.LoadString("INCOMPLETEASSESSMENTS", strCurrentLanguage);
            lblTextParagraph.Text = DataProxy.LoadString("ASSESSORPARAGRAPHTEXT", strCurrentLanguage);
            lblApproveUserRequest.Text = DataProxy.LoadString("APPROVEUSERREQUEST", strCurrentLanguage);
            lblApproveUserTextParagraph.Text = DataProxy.LoadString("APPROVEASSESSORPARAGRAPHTEXT", strCurrentLanguage);
            lblApproveCompleteAssessments.Text = DataProxy.LoadString("APPROVECOMPLETEDASSESSMENTS", strCurrentLanguage);
            lblApproveCompleteAssessmentsParagraph.Text = DataProxy.LoadString("APPROVECOMPLETEDASSESSMENTSPARAGRAPHTEXT", strCurrentLanguage);
            lblCountry.Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            rbSort.Items[0].Text = DataProxy.LoadString("DATELABEL", strCurrentLanguage);
            rbSort.Items[1].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);
            rbSort.Items[2].Text = DataProxy.LoadString("ASSESSORLABEL", strCurrentLanguage);


            if (Request.QueryString["CountryID"] != null || ddlCountries.SelectedValue != "")
            {
                if (Request.QueryString["CountryID"] != null)
                    intCountryID = Convert.ToInt32(Request.QueryString["CountryID"]);
                else
                    intCountryID = Convert.ToInt32(ddlCountries.SelectedValue);
            }
            else
            {
                PopulateCountriesList();
            }

            GetUserRequests();
            GetCompletedAssessments();
            GetSearchResults();

            grdApproveCompletedAssess.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);
            grdApproveUser.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);
            grdResults.EmptyDataText = DataProxy.LoadString("NORECORDSFOUND", strCurrentLanguage);


            //See which option was picked for various functions
            if (Request.QueryString["Approve"] != null)
            {
                string strApproved = Request.QueryString["Approve"].ToString();
                CompleteUserRequest(strApproved);
            }

            if (Request.QueryString["ApproveAssessment"] != null)
            {
                string strApproved = Request.QueryString["ApproveAssessment"].ToString();
                CompleteAssessmentRequest(strApproved);
            }

            if (Request.QueryString["EditAssessment"] != null)
            {
                //Allow Facilitator to edit assessment
            }

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintGrid(grdApproveUser);
        }

        protected void btnPrintApproveCA_Click(object sender, EventArgs e)
        {
            PrintGrid(grdApproveCompletedAssess);
        }

        protected void btnPrintIncomplete_Click(object sender, EventArgs e)
        {
            PrintGrid(grdResults);
        }

        protected void ExportGrid(GridView gv)
        {
            string strUseFileName = "";
            string strUseTitle = "";

            if (gv.ID == "grdApproveUser")
            {
                strUseFileName = strFileName;
                strUseTitle = lblReportTitle.Text;
            }

            if (gv.ID == "grdResults")
            {
                strUseFileName = strIncompletFileName;
                strUseTitle = lblIncompleteTitle.Text;
            }

            if (gv.ID == "grdApproveCompletedAssess")
            {
                strUseFileName = strApproveCompletFileName;
                strUseTitle = lblApproveCATitle.Text;
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", strUseFileName));
            HttpContext.Current.Response.ContentType = "application/excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Table table = new Table();
            TableRow title = new TableRow();
            title.BackColor = System.Drawing.Color.Green;
            TableCell titlecell = new TableCell();

            if (gv.ID == "grdApproveUser")
            {
                titlecell.ColumnSpan = intColCount;//Should span across all columns
            }

            if (gv.ID == "grdResults")
            {
                titlecell.ColumnSpan = intIncompleteColCount;//Should span across all columns
            }

            if (gv.ID == "grdApproveCompletedAssess")
            {
                titlecell.ColumnSpan = intApproveCompleteColCount;//Should span across all columns
            }

            Label lbl = new Label();
            lbl.Text = strUseTitle;
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
                GetUserRequests();

                ExportGrid(grdApproveUser);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnExportIncomplete_Click(object sender, EventArgs e)
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
        protected void btnExportApproveCA_Click(object sender, EventArgs e)
        {
            try
            {
                grdApproveCompletedAssess.AllowPaging = false;
                GetCompletedAssessments();

                ExportGrid(grdApproveCompletedAssess);
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
            Panel panel = new Panel();


            if (gv.ID == "grdApproveUser")
            {
                panel = Panel1;
                GetUserRequests();
            }

            if (gv.ID == "grdApproveCompletedAssess")
            {
                panel = Panel2;
                GetCompletedAssessments();
            }

            if (gv.ID == "grdResults")
            {
                panel = Panel3;
                GetSearchResults();
            }

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            panel.RenderControl(hw);
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

        protected void CompleteAssessmentRequest(string strApproved)
        {
            try
            {
                int intAssessmentID = 0;

                if (Request.QueryString["AssessmentID"] != null)
                    intAssessmentID = Convert.ToInt32(Request.QueryString["AssessmentID"]);

                string strRtn = DataProxy.ApproveAssessment(intAssessmentID, strApproved);
                GetUserRequests();
                GetSearchResults();
                GetCompletedAssessments();
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }
        protected void CompleteUserRequest(string strApproved)
        {
            try
            {
                if (Request.QueryString["UserID"] != null)
                    intUserID = Convert.ToInt32(Request.QueryString["UserID"]);

                string strRtn = DataProxy.ApproveUser(intUserID, strApproved);
                GetUserRequests();
                GetSearchResults();
                GetCompletedAssessments();

                bool bSuccess = false;

                if (strApproved == "1")
                    bSuccess = DataProxy.SendApprovedEmail(intUserID);

            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }



        protected void GetUserRequests()
        {
            DataTable dt = null;
            DateTime dtCurDate = System.DateTime.Today.AddDays(1);


            dt = DataProxy.GetAllUserRequestInfo();

            DataView dv = dt.DefaultView;

            if (dv != null)
            {
                dv.Sort = "Approved ASC";
                dv.RowFilter = "CreateDate >= '" + dtCurDate.AddMonths(-3).ToShortDateString() + "' AND CreateDate <= '" + dtCurDate.ToShortDateString() + "'";
            }

            grdApproveUser.DataSource = dt;
            grdApproveUser.DataBind();
            grdApproveUser.Visible = true;
            Session["dvUserRequests"] = dt.DefaultView;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Really don't have to do anything since the code to populate grid is in Page_Load and this will postback...

        }

        protected void grdApproveCompletedAssess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink hlDeny = null;
            HyperLink hlApprove = null;
            HyperLink hlEdit = null;
            HyperLink hlSpeciesResults = null;
            Label lblStatus = null;

            string strUserID = "";
            string strApproved = "";

            DataRowView dr = (DataRowView)e.Row.DataItem;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Set first column/"Species" header
                e.Row.Cells[0].Text = DataProxy.LoadString("SPECIESLABEL", strCurrentLanguage);

                //Set second column/"Country" header
                e.Row.Cells[1].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);

                //Set third column/"User Name" header
                e.Row.Cells[2].Text = DataProxy.LoadString("ASSESSEDBYLABEL", strCurrentLanguage);

                //Set fifth column/"Date" header
                e.Row.Cells[4].Text = DataProxy.LoadString("DATELABEL", strCurrentLanguage);


                //Set fourth column/"Status" header
                //e.Row.Cells[3].Text = DataProxy.LoadString("STATUS", strCurrentLanguage);

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strAssessmentID = "";

                if (dr["AssessmentID"] != null)
                    strAssessmentID = dr["AssessmentID"].ToString();

                if (dr["UserID"] != null)
                {
                    strUserID = dr["UserID"].ToString();
                }

                if (dr["Status"] != null)
                {
                    if (e.Row.FindControl("lblStatus") != null)
                    {
                        lblStatus = (Label)e.Row.FindControl("lblStatus");

                        if (dr["Status"].ToString() == "False")
                            lblStatus.Text = DataProxy.LoadString("NOTAPPROVEDLABEL", strCurrentLanguage);
                        else
                            lblStatus.Text = DataProxy.LoadString("APPROVEDLABEL", strCurrentLanguage);
                    }
                }

                if (dr["Approved"] != null)
                {
                    strApproved = dr["Approved"].ToString();
                }

                if (e.Row.FindControl("hlApprove") != null)
                {
                    hlApprove = (HyperLink)e.Row.FindControl("hlApprove");
                    hlApprove.Text = DataProxy.LoadString("APPROVETEXT", strCurrentLanguage);
                    hlApprove.NavigateUrl = "FacilitatorDashboard.aspx?ApproveAssessment=1&AssessmentID=" + strAssessmentID;
                    if (strApproved == "True")
                        hlApprove.Visible = false;
                    else
                        hlApprove.Visible = true;

                }

                if (e.Row.FindControl("hlDeny") != null)
                {
                    hlDeny = (HyperLink)e.Row.FindControl("hlDeny");
                    if (strApproved == "True")
                        hlDeny.Text = DataProxy.LoadString("UNAPPROVELABEL", strCurrentLanguage);
                    else
                        hlDeny.Text = DataProxy.LoadString("DENYTEXT", strCurrentLanguage);

                    hlDeny.NavigateUrl = "FacilitatorDashboard.aspx?ApproveAssessment=0&AssessmentID=" + strAssessmentID;
                }

                if (e.Row.FindControl("hlEdit") != null)
                {
                    string strURL = "EditAssessment.aspx?FEdit=Y&AssessmentID=" + strAssessmentID +
                                    "&CountryID=" + dr["CountryID"].ToString() + "&SpeciesID=" + dr["SpeciesID"].ToString();

                    hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    hlEdit.Text = DataProxy.LoadString("EDITTEXT", strCurrentLanguage);

                    hlEdit.NavigateUrl = strURL;
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

                //Set third column/"Organization" header
                e.Row.Cells[2].Text = DataProxy.LoadString("ORGANIZATIONLABEL", strCurrentLanguage);

                //Set fourth column/"Email" header
                e.Row.Cells[3].Text = DataProxy.LoadString("EMAILLABEL", strCurrentLanguage);

                //Set fifth column/"Country" header
                e.Row.Cells[4].Text = DataProxy.LoadString("COUNTRYLABEL", strCurrentLanguage);

                //Set sixth column/"Expertise" header
                e.Row.Cells[5].Text = DataProxy.LoadString("USERCOUNTRIESTEXT", strCurrentLanguage);

                //Set seventh column/"ASGMember" header
                e.Row.Cells[6].Text = DataProxy.LoadString("ASGMEMBERLABEL", strCurrentLanguage);

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
                if (dr["UserID"] != null)
                {
                    strUserID = dr["UserID"].ToString();
                }

                if (dr["Status"] != null)
                {
                    if (e.Row.FindControl("lblStatus") != null)
                    {
                        lblStatus = (Label)e.Row.FindControl("lblStatus");

                        if (dr["Status"].ToString() == "False")
                            lblStatus.Text = DataProxy.LoadString("NOTAPPROVEDLABEL", strCurrentLanguage);
                        else
                            lblStatus.Text = DataProxy.LoadString("APPROVEDLABEL", strCurrentLanguage);
                    }
                }

                if (dr["Approved"] != null)
                {
                    strApproved = dr["Approved"].ToString();
                }

                if (e.Row.FindControl("hlApprove") != null)
                {
                    hlApprove = (HyperLink)e.Row.FindControl("hlApprove");
                    hlApprove.Text = DataProxy.LoadString("APPROVETEXT", strCurrentLanguage);
                    hlApprove.NavigateUrl = "FacilitatorDashboard.aspx?Approve=1&UserID=" + strUserID;
                    if (strApproved == "True")
                        hlApprove.Visible = false;
                    else
                        hlApprove.Visible = true;

                }

                if (e.Row.FindControl("hlDeny") != null)
                {
                    hlDeny = (HyperLink)e.Row.FindControl("hlDeny");
                    if (strApproved == "True")
                        hlDeny.Text = DataProxy.LoadString("UNAPPROVELABEL", strCurrentLanguage);
                    else
                        hlDeny.Text = DataProxy.LoadString("DENYTEXT", strCurrentLanguage);

                    hlDeny.NavigateUrl = "FacilitatorDashboard.aspx?Approve=0&UserID=" + strUserID;
                }

            }
        }

        protected void grdApproveCompletedAssess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            grdApproveCompletedAssess.PageIndex = e.NewPageIndex;
            if (Session["dvCompletedAssess"] != null)
                grdApproveCompletedAssess.DataSource = (DataView)Session["dvCompletedAssess"];

            grdApproveCompletedAssess.DataBind();
            lblApproveCompleteAssessmentsGridMessage.Text = "Page " + (grdApproveCompletedAssess.PageIndex + 1).ToString() + " of " + grdApproveCompletedAssess.PageCount;
        }

        protected void grdApproveUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Handle paging...
            grdApproveUser.PageIndex = e.NewPageIndex;
            if (Session["dvApproveUserResults"] != null)
                grdApproveUser.DataSource = (DataView)Session["dvApproveUserResults"];

            grdApproveUser.DataBind();
            lblGridMessageApproveUser.Text = "Page " + (grdApproveUser.PageIndex + 1).ToString() + " of " + grdApproveUser.PageCount;
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

        protected void PopulateCountriesList()
        {
            try
            {
                DataTable dtCountries = null;

                //Show all countries so assessor can select any country to write an assessment for...
                //Changing this to filtered countries - only show countries that have completed assessments awaiting approval.
                dtCountries = DataProxy.GetFilteredCountries();

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

        protected void GetCompletedAssessments()
        {
            DataTable dtResults = null;
            DataView dv = null;

            dtResults = DataProxy.GetCompletedAssessmentSearchResultsSorted(rbSort.SelectedIndex);

            grdApproveCompletedAssess.DataSource = dtResults;

            if (intCountryID != 0)
            {
                if (dtResults != null)
                {
                    dv = dtResults.DefaultView;
                    dv.RowFilter = "CountryID = " + intCountryID.ToString();
                }
            }
            grdApproveCompletedAssess.DataBind();
            grdApproveCompletedAssess.Visible = true;
            Session["dvCompletedAssess"] = dtResults.DefaultView;
        }

        protected void GetSearchResults()
        {
            DataTable dtResults = null;

            if (Session["AssessorUserName"] != null)
                strAssessorUID = Session["AssessorUserName"].ToString();

            int intUserID = DataProxy.GetUserID(strAssessorUID);

            dtResults = DataProxy.GetAssessmentSearchResults(intUserID);

            grdResults.DataSource = dtResults;
            grdResults.DataBind();
            grdResults.Visible = true;
            Session["dvResultsDash"] = dtResults.DefaultView;
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

        protected void rbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCompletedAssessments();
        }
    }
}