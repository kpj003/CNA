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
using Microsoft.Reporting.WebForms;

namespace AArk
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        string strCurrentLanguage = "";
        int intAssessmentID = 0;
        int intSpeciesID = 0;
        int intLanguageID = 0;
        public string ARKIVESpeciesName = "";

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
            
            lblPageTitle.Text = DataProxy.LoadString("ASSESSMENTRESULTSLINK", strCurrentLanguage);

            if (Request.QueryString["AssessmentID"] != null)
                intAssessmentID = Convert.ToInt32(Request.QueryString["AssessmentID"]);

            if (Request.QueryString["Single"] != null)
                GetSingleAssessmentReportData();
        }


        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e)
        {

            //e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("SubreportDataSet", this.SqlDataSource1));

        }

        protected void GetSingleAssessmentReportData()
        {
            try
            {
                DataTable dtAssess = DataProxy.GetAssessmentInfo(intAssessmentID, intLanguageID);
                DataTable dtQA = DataProxy.GetAssessmentQAGrid(intAssessmentID, intLanguageID);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Assessment.rdlc");
                ReportDataSource datasource = new ReportDataSource("dsSingleAssessment", dtAssess);
                ReportDataSource dsQA = new ReportDataSource("dsSubQA", dtQA); 
                //using (System.IO.Stream report = System.IO.File.OpenRead(Server.MapPath("~/subQA.rdlc")))
                //{

                //    this.ReportViewer1.LocalReport.LoadSubreportDefinition("subQA", report);
                //}
               // ReportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource); 
                ReportViewer1.LocalReport.DataSources.Add(dsQA);
                ReportViewer1.LocalReport.Refresh();
    
  

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

    }

}