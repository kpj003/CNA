using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Net.Mail;
using System.Xml;


namespace Data.Utilities {
    public static class DatabaseQueries
    {
        //Search Results
        public const string GET_USER_INFO = " SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password ";

        public const string GET_USER_INFO_USERNAME = " SELECT * FROM Users WHERE UserName = @UserName ";

        public const string GET_USER_INFO_ID = " SELECT * FROM Users WHERE UserID = @UserID ";

        public const string GET_ALL_USER_INFO = "SELECT C.CountryName, U.*, U.Approved AS Status " +
                                                " FROM Users U INNER JOIN Countries C ON U.CountryID = C.CountryID " +
                                                " WHERE UserID = @UserID";

        public const string GET_ALL_USER_REQ_INFO = "SELECT C.CountryName, U.*, U.Approved AS Status " +
                                                    " FROM Users U INNER JOIN Countries C ON U.CountryID = C.CountryID " +
                                                    //" WHERE DATEDIFF(M, CreateDate, GETDATE()) <= 3 " +
                                                    " ORDER BY Approved ASC, U.CreateDate DESC";

        public const string GET_ALL_APPROVED_USERS = " SELECT U.UserID, U.CountryID, dbo.GetUserNameComma(U.UserID) AS UserFullName, LastName, FirstName, Email, CountryName, UT.Description AS UserType, OrganizationName, CountryExpertise, AmphibianSpecialistGroupMember " +
                                                     " FROM Users U INNER JOIN Countries C ON U.CountryID = C.CountryID " +
                                                     " INNER JOIN UserTypes UT ON U.UserTypeID = UT.UserTypeID  " +
                                                     " WHERE Approved = 1 " +
                                                     " ORDER BY LastName, FirstName";

        public const string GET_ASSESSMENT_USERS = " SELECT U.UserID, U.CountryID, dbo.GetUserNameComma(U.UserID) AS UserFullName, LastName, FirstName, Email, CountryName, UT.Description AS UserType, OrganizationName, CountryExpertise, AmphibianSpecialistGroupMember " +
                                                      " FROM Users U INNER JOIN Countries C ON U.CountryID = C.CountryID " +
                                                      " INNER JOIN UserTypes UT ON U.UserTypeID = UT.UserTypeID  " +
                                                      " WHERE Approved = 1 " +
                                                      " AND EXISTS (SELECT UserID FROM Assessments A WHERE A.UserID = U.UserID AND A.Approved = 1 AND A.Completed = 1) " +
                                                      " ORDER BY LastName, FirstName";

        public const string GET_ASSESSMENT_USER = " SELECT U.UserID, A.SpeciesID, A.AssessmentID, C.CountryID, LastName, FirstName, dbo.GetSpeciesDisplayName(A.SpeciesID) AS SpeciesDisplayName, C.CountryName, A.DateCreated " +
                                                     " FROM Assessments A INNER JOIN Users U ON A.UserID = U.UserID " +
                                                     " INNER JOIN Countries C ON C.CountryID = A.CountryID " +
                                                     " WHERE A.Approved = 1 " +
                                                     " AND A.Completed = 1 " +
                                                     " ORDER BY LastName, SpeciesDisplayName, CountryName";
        /*
        public const string SEARCH_RESULTS_BY_COUNTRY = " SELECT DISTINCT CountryName, SpeciesDisplayName, SpeciesID, CountryID, EnglishCommonName, NbrAssessments, " +
                                                        " 0 AS AssessmentIDUser, '' AS UserName " +
                                                        " FROM " +
                                                        "( " +
                                                        " SELECT DISTINCT TOP 25 CountryName," +
                                                        " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                                        " A.AssessmentID, A.DateCreated, U.UserID, U.UserName, " +
                                                        " C.CountryID, EnglishCommonName, " +
                                                        " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, " +
                                                        " dbo.GetNumberAssessmentsSpecies(A.SpeciesID, A.CountryID) AS NbrAssessments " +
                                                        " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                                        " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                                        " INNER JOIN Users U ON U.UserID = A.UserID " +
                                                        " WHERE C.CountryID = @CountrySearchValue " + " ORDER BY DATECREATED DESC,CountryName, SpeciesDisplayName) A ";

        public const string SEARCH_RESULTS_BY_C_USER = " SELECT DISTINCT CountryName, SpeciesDisplayName, SpeciesID, CountryID, EnglishCommonName, NbrAssessments" +
                                                       " ,dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, A.UserName, A.UserID" +
                                                       " FROM " +
                                                       "( " +
                                                       " SELECT DISTINCT TOP 25 CountryName," +
                                                       " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                                       " A.AssessmentID, A.DateCreated, U.UserID, U.UserName, " +
                                                       " C.CountryID, EnglishCommonName, " +
                                                       " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, " +
                                                       " dbo.GetNumberAssessmentsSpecies(A.SpeciesID, A.CountryID) AS NbrAssessments " +
                                                       " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                                       " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                                       " INNER JOIN Users U ON U.UserID = A.UserID " +
                                                       " WHERE C.CountryID = @CountrySearchValue " + " ORDER BY DATECREATED DESC,CountryName, SpeciesDisplayName) A ";

        public const string SEARCH_RESULTS_BY_SPECIES = " SELECT DISTINCT CountryName, SpeciesDisplayName, SpeciesID, CountryID, EnglishCommonName, NbrAssessments, " +
                                                        " 0 AS AssessmentIDUser, '' AS UserName " +
                                                        " FROM " +
                                                        "( " +
                                                        " SELECT DISTINCT TOP 25 CountryName," +
                                                        " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                                        " A.AssessmentID, A.DateCreated, U.UserID, U.UserName, " +
                                                        " C.CountryID, EnglishCommonName, " +
                                                        " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, " +
                                                        " dbo.GetNumberAssessmentsSpecies(A.SpeciesID, A.CountryID) AS NbrAssessments " +
                                                        " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                                        " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                                        " INNER JOIN Users U ON U.UserID = A.UserID " +
                                                        " WHERE S.SpeciesID IN " + 
                                                        " (SELECT SpeciesID FROM Species " +
                                                        " WHERE GenusName LIKE @SpeciesSearchValue " + 
                                                        " OR SpeciesName LIKE @SpeciesSearchValue " +
                                                        " OR ScientificName LIKE @SpeciesSearchValue " +
                                                        " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                                        " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                                        " OR EnglishCommonName LIKE @SpeciesSearchValue)" +
                                                        " ORDER BY DATECREATED DESC,CountryName, SpeciesDisplayName) A ";

        public const string SEARCH_RESULTS_BY_S_USER =  " SELECT DISTINCT CountryName, SpeciesDisplayName, SpeciesID, CountryID, EnglishCommonName, NbrAssessments," +
                                                        //" dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, A.UserID" +
                                                        "A.AssessmentIDUser, A.UserID, A.UserName" +
                                                        " FROM " +
                                                        "( " +
                                                        " SELECT DISTINCT TOP 25 CountryName," +
                                                        " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                                        " A.AssessmentID, A.DateCreated, U.UserID, U.UserName, " +
                                                        " C.CountryID, EnglishCommonName, " +
                                                        " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, " +
                                                        " dbo.GetNumberAssessmentsSpecies(A.SpeciesID, A.CountryID) AS NbrAssessments " +
                                                        " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                                        " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                                        " INNER JOIN Users U ON U.UserID = A.UserID " +
                                                        " WHERE S.SpeciesID IN " +
                                                        " (SELECT SpeciesID FROM Species " +
                                                        " WHERE GenusName LIKE @SpeciesSearchValue " +
                                                        " OR SpeciesName LIKE @SpeciesSearchValue " +
                                                        " OR ScientificName LIKE @SpeciesSearchValue " +
                                                        " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                                        " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                                        " OR EnglishCommonName LIKE @SpeciesSearchValue)" +
                                                        " ORDER BY DATECREATED DESC,CountryName, SpeciesDisplayName) A ";

        public const string SEARCH_RESULTS_BY_COUNTRY_AND_SPECIES = " SELECT DISTINCT CountryName, SpeciesDisplayName, SpeciesID, CountryID, EnglishCommonName, NbrAssessments, " +
                                                                   " 0 AS AssessmentIDUser, '' AS UserName " +
                                                                   " FROM " +
                                                                    "( " +
                                                                    " SELECT DISTINCT TOP 25 CountryName," +
                                                                    " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                                                    " A.AssessmentID, A.DateCreated, U.UserID, U.UserName, " +
                                                                    " C.CountryID, EnglishCommonName, " +
                                                                    " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, " +
                                                                    " dbo.GetNumberAssessmentsSpecies(A.SpeciesID, A.CountryID) AS NbrAssessments " +
                                                                    " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                                                    " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                                                    " INNER JOIN Users U ON U.UserID = A.UserID " + 
                                                                    " WHERE C.CountryID = @CountrySearchValue " +
                                                                    " AND S.SpeciesID IN " +	
	                                                                    " (SELECT SpeciesID FROM Species " +
	                                                                    " WHERE GenusName LIKE @SpeciesSearchValue " +
                                                                        " OR SpeciesName LIKE @SpeciesSearchValue " +
                                                                        " OR ScientificName LIKE @SpeciesSearchValue  " +
                                                                        " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                                                        " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                                                        " OR EnglishCommonName LIKE @SpeciesSearchValue)" +
                                                                        " ORDER BY DATECREATED DESC,CountryName, SpeciesDisplayName) A ";

        public const string SEARCH_RESULTS_BY_CAS_USER =    " SELECT DISTINCT CountryName, SpeciesDisplayName, SpeciesID, CountryID, EnglishCommonName, NbrAssessments, " +
                                                            " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, A.UserID, A.UserName" +
                                                            " FROM " +
                                                            " ( " +
                                                            " SELECT DISTINCT TOP 25 CountryName," +
                                                            " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                                            " A.AssessmentID, A.DateCreated, U.UserID, U.UserName, " +
                                                            " C.CountryID, EnglishCommonName, " +
                                                            " dbo.GetAssessmentIDUser(A.SpeciesID, A.CountryID, A.UserID) AS AssessmentIDUser, " +
                                                            " dbo.GetNumberAssessmentsSpecies(A.SpeciesID, A.CountryID) AS NbrAssessments " +
                                                            " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                                            " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                                            " INNER JOIN Users U ON U.UserID = A.UserID " +
                                                            " WHERE C.CountryID = @CountrySearchValue " +
                                                            " AND S.SpeciesID IN " +
                                                            " (SELECT SpeciesID FROM Species " +
                                                                       " WHERE GenusName LIKE @SpeciesSearchValue " +
                                                                       " OR SpeciesName LIKE @SpeciesSearchValue " +
                                                                       " OR ScientificName LIKE @SpeciesSearchValue  " +
                                                                       " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                                                       " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                                                       " OR EnglishCommonName LIKE @SpeciesSearchValue)" +
                                                            " ORDER BY DATECREATED DESC,CountryName, SpeciesDisplayName) A ";                           

        public const string SEARCH_RESULTS_EXPORT = "";*/

        
          }

    public class GridViewExportUtil
    {
        public static void ExportToCsv(string fileName, GridView gv, string Title)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (StringWriter objSw = new StringWriter())
            {

                //  Create a table to contain the grid
                Table table = new Table();

                //  include the gridline settings
                table.GridLines = gv.GridLines;
                objSw.Write("Report:  " + Title + "");
                objSw.Write(objSw.NewLine);
                objSw.Write("Report Date: " + DateTime.Now.ToString());
                objSw.Write(objSw.NewLine);
                objSw.Write(objSw.NewLine); objSw.Write(objSw.NewLine);

                //  add the header row to the table
                int NoOfColumn = gv.Columns.Count;
                //Create Header
                for (int i = 0; i < NoOfColumn; i++)
                {
                    objSw.Write(gv.Columns[i].HeaderText);
                    //check not last column
                    if (i < NoOfColumn - 1)
                    {
                        objSw.Write(",");
                    }
                }

                objSw.Write(objSw.NewLine);


                //Create Data
                foreach (GridViewRow dr in gv.Rows)
                {
                    for (int i = 0; i < NoOfColumn; i++)
                    {
                        objSw.Write(PrepareControlForExportToCsv(dr.Cells[i]).Replace(",", ""));

                        if (i < NoOfColumn - 1)
                        {
                            objSw.Write(",");
                        }
                    }
                    objSw.Write(objSw.NewLine);
                }


                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(objSw.ToString());
                HttpContext.Current.Response.End();
            }
        }
        private static string PrepareControlForExportToCsv(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is Literal)
                {
                    string strStrippedText = (current as Literal).Text.Replace("&nbsp;", "").Replace("<font color='black'>", "").Replace("</font>", "").Replace("<br/>", "").Replace("<b>", "").Replace("</b>", "").Replace("A:>", "  A:> ").Replace("Q:>", "Q:> "); 
                    return strStrippedText;
                }
                else if (current is Label)
                {
                    return (current as Label).Text;
                }

                if (current.HasControls())
                {
                    GridViewExportUtil.PrepareControlForExportToCsv(current);
                }
            }
            if (control is TableCell)
                return (control as TableCell).Text;
            else
                return "";
        } 

        public static void ExportWord(string fileName, GridView gv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the grid
                    Table table = new Table();

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        GridViewExportUtil.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        public static void Export(string fileName, GridView gv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the grid
                    Table table = new Table();

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        GridViewExportUtil.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }
        public static void ExportCSV(string fileName, GridView gv)
        {
            ExportToCsv(fileName, gv, "Exporting");
        }

        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    GridViewExportUtil.PrepareControlForExport(current);
                }
            }
        }
    }


    public static class DataProxy
    {

        public static DataTable GetFilteredAssessmentSearchResults(int intCountryID, string strSpeciesSearch, int intUserID, string assessmentFilter)
        {
            DataTable dt = null;

            string strCheck = "";
            /*
            )
             * “Species with assessments” and “Species without assessments”. 
             * When combined with a country, this will allow national facilitators and assessors to quickly see which species do not yet have assessments. 
             * This filter needs to relate to the Country dropdown - if a species occurs in countries A, B and C, and no country is selected, we show the species and assessments for all countries. 
             * If country B is selected, we do not include any assessments (or lack of) in countries A and C.     
            */
            if (assessmentFilter == "WITH") //species with assessments
            {
                if (strSpeciesSearch == "%%")
                {  //No species name limit selected - remove 'like' search to increase speed returning dataset.
                    strCheck = "SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                                " dbo.GetUserFullName(A.UserID) as UserName, A.DateCreated," +
                                " dbo.GetAssessmentStatus(isnull(A.AssessmentID,0)) AS CurStatus, " +
                                " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                                " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                                " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                                " INNER JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) ";

                    if (intCountryID != 0)
                        strCheck += " WHERE C.CountryID = @CountryID ";
                }
                else
                {
                    strCheck = "SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                                " dbo.GetUserFullName(A.UserID) as UserName, A.DateCreated," +
                                " dbo.GetAssessmentStatus(isnull(A.AssessmentID,0)) AS CurStatus, " +
                                " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                                " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                                " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                                " INNER JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                                " WHERE (ScientificName LIKE @SpeciesSearchValue " +
                                " OR GenusName LIKE @SpeciesSearchValue " +
                                " OR SpeciesName LIKE @SpeciesSearchValue " +
                                " OR ScientificName LIKE @SpeciesSearchValue " +
                                " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                " OR Synonyms LIKE @SpeciesSearchValue " + //Add new field Synonyms to search criteria
                                " OR EnglishCommonName LIKE @SpeciesSearchValue) ";

                    if (intCountryID != 0)
                        strCheck += "AND C.CountryID = @CountryID ";
                }
            }
            else
            { 
                //WITHOUT ASSESSMENT LOGIC
                if (strSpeciesSearch == "%%")
                {
                    //No species name limit selected - remove 'like' search to increase speed returning dataset.
                    strCheck = "SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                                " dbo.GetUserFullName(A.UserID) as UserName, A.DateCreated," +
                                " dbo.GetAssessmentStatus(isnull(A.AssessmentID,0)) AS CurStatus, " +
                                " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                                " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                                " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                                " LEFT JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                                " WHERE A.AssessmentID IS NULL";

                    if (intCountryID != 0)
                        strCheck += " AND C.CountryID = @CountryID ";
                }
                else
                {
                    strCheck = "SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                                " dbo.GetUserFullName(A.UserID) as UserName, A.DateCreated," +
                                " dbo.GetAssessmentStatus(isnull(A.AssessmentID,0)) AS CurStatus, " +
                                " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                                " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                                " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                                " LEFT JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                                " WHERE (ScientificName LIKE @SpeciesSearchValue " +
                                " OR GenusName LIKE @SpeciesSearchValue " +
                                " OR SpeciesName LIKE @SpeciesSearchValue " +
                                " OR ScientificName LIKE @SpeciesSearchValue " +
                                " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                " OR Synonyms LIKE @SpeciesSearchValue " + //Add new Synonyms field to search criteria
                                " OR EnglishCommonName LIKE @SpeciesSearchValue) " +
                                " AND A.AssessmentID IS NULL ";

                    if (intCountryID != 0)
                        strCheck += "AND C.CountryID = @CountryID ";
                }
            }

            strCheck += " ORDER BY SpeciesDisplayName, CountryName ";

            if (intCountryID != 0)
            {
                dt = GetDataTable(strCheck, false,
                                   CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearch, SqlDbType.VarChar),
                                   CreateSqlParameter("@UserID", intUserID, SqlDbType.Int),
                                   CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));
            }
            else
            {
                dt = GetDataTable(strCheck, false,
                                       CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearch, SqlDbType.VarChar),
                                       CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));
            }

          
            return dt;
        }

        public static DataTable GetAssessmentSearchResults(int intCountryID, string strSpeciesSearch, int intUserID)
        {
            DataTable dt = null;

            string strCheck = "";
           
            if (strSpeciesSearch == "%%")
            {  //No species name limit selected - remove 'like' search to increase speed returning dataset.
                strCheck = "SELECT DISTINCT CASE " +
                            " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                            " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                            " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                            " dbo.GetUserFullName(A.UserID) as UserName, A.DateCreated," +
                            " dbo.GetAssessmentStatus(isnull(A.AssessmentID,0)) AS CurStatus, " +
                            " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                            " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                            " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                            " LEFT JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) ";
                
                if (intCountryID != 0)
                    strCheck += " WHERE C.CountryID = @CountryID ";
            }
            else
            {
                strCheck = "SELECT DISTINCT CASE " +
                            " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                            " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                            " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                            " dbo.GetUserFullName(A.UserID) as UserName, A.DateCreated," +
                            " dbo.GetAssessmentStatus(isnull(A.AssessmentID,0)) AS CurStatus, " +
                            " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                            " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                            " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                            " LEFT JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                            " WHERE (ScientificName LIKE @SpeciesSearchValue " +
                            " OR GenusName LIKE @SpeciesSearchValue " +
                            " OR SpeciesName LIKE @SpeciesSearchValue " +
                            " OR ScientificName LIKE @SpeciesSearchValue " +
                            " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                            " OR LocalCommonName LIKE @SpeciesSearchValue " +
                            " OR Synonyms LIKE @SpeciesSearchValue " + //Add new field Synonyms to search criteria
                            " OR EnglishCommonName LIKE @SpeciesSearchValue) ";

                if (intCountryID != 0)
                    strCheck += "AND C.CountryID = @CountryID ";
            }   

            strCheck += " ORDER BY SpeciesDisplayName, CountryName ";

            if (intCountryID != 0)
            {
                dt = GetDataTable(strCheck, false,
                                   CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearch, SqlDbType.VarChar),
                                   CreateSqlParameter("@UserID", intUserID, SqlDbType.Int),
                                   CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));
            }
            else
            {
                dt = GetDataTable(strCheck, false,
                                       CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearch, SqlDbType.VarChar),
                                       CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));
            }

            //if (dt != null)
            //{
            //    string strCountryFilter = "CountryID = " + intCountryID.ToString();
               
            //    if (intCountryID > 0)
            //        dt = GetFilteredDataTable(dt, strCountryFilter);

            //}
            return dt;

        }

        public static DataTable GetConsolidatedReportResults(int intCountryID, int intSpeciesID, int intLanguageID)
        {
            DataTable dt = null;

            string strCheck = "";

            strCheck = " SELECT S.EnglishCommonName,S.ScientificName,S.LocalCommonName, A.AdditionalComments, " +
                        " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName," +
                        " dbo.GetSpeciesAssessmentDisplayName(S.SpeciesID) AS SpeciesAssessmentDisplayName," +
                        " U.UserName, CONVERT(VARCHAR(15), datecreated, 106) AS DateCreatedDate, " +
                        " GenusName, SpeciesName, SubSpeciesName, SpeciesOrder, SpeciesClass, SpeciesFamily, " +
                        " dbo.GetSpeciesDisplayName(S.SpeciesID) AS DisplayName, " +
                        " C.CountryName, Distribution, ProtectedHabitat, EDScore, isnull(S.RedListID,'') as RedListID," +
                        "dbo.GetGlobalRedListCategoryDisplayName(S.SpeciesID, 0, @LanguageID) AS GlobalRedListCategory, " +
                        "dbo.GetNationalRedListCategoryDisplayName(A.SpeciesID, A.CountryID, @LanguageID) AS NationalRedListCategory, " +
                        " A.AssessmentID, A.UserID, A.Priority, A.Completed, dbo.GetUserFullName(A.UserID) AS UserFullName, " +  
                        "dbo.GetConsolidatedCommentsForAssessment (@SpeciesID, @CountryID) AS Comments" +
                             
                        " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                        " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                        " INNER JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                        " INNER JOIN Users U ON U.UserID = A.UserID " +
                        " WHERE C.CountryID = @CountryID " +
                        " AND S.SpeciesID = @SpeciesID  " +
                        " AND A.Approved = 1 " +
                        " ORDER BY SpeciesDisplayName, CountryName ";

            dt = GetDataTable(strCheck, false,
                                   CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                   CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                                   CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));                                   

           
            return dt;

        }

        public static DataTable GetCompletedAssessmentSearchResultsSorted(int SortByVal)
        {
            DataTable dt = null;

            string strCheck = " SELECT DISTINCT CASE " +
                        " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                       "  ELSE CountryName END AS CountryName, DateCreated, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                       "  dbo.GetUserFullName(A.UserID) as UserName, Completed, A.Approved, A.Approved as Status, U.Email, " +
                       " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID, CONVERT(VARCHAR(15), datecreated, 106) AS DateCreatedDate " +
                       " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID " +
                       " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                       " INNER JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                       " INNER JOIN Users U ON U.UserID = A.UserID" +
                       " WHERE isnull(A.Completed, 0) = 1 " +
                       " AND isnull(A.Approved, 0) = 0 ";
 
                if (SortByVal == 1) //Country
                    strCheck += " ORDER BY CountryName";
                else
                {
                    if (SortByVal == 2) //Assessor
                        strCheck += " ORDER BY UserName ";
                    else
                        //Date is selected /default sort
                        strCheck += " ORDER BY DateCreated DESC ";
                }
            
            dt = GetDataTable(strCheck, false);

            return dt;

        } 
        public static DataTable GetCompletedAssessmentSearchResults()
        {
            DataTable dt = null;

            string strCheck = " SELECT DISTINCT CASE " +
                        " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                       "  ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                       "  dbo.GetUserFullName(A.UserID) as UserName, Completed, A.Approved, A.Approved as Status, U.Email, " +
                       " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID, CONVERT(VARCHAR(15), datecreated, 106) AS DateCreatedDate " +
                       " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID " +
                       " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                       " INNER JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                       " INNER JOIN Users U ON U.UserID = A.UserID" +
                       " WHERE isnull(A.Completed, 0) = 1 " +
                       " AND isnull(A.Approved, 0) = 0 ";

            dt = GetDataTable(strCheck, false);

            return dt;

        }

        public static string GetConcatAssessedOnValues(DataTable dt)
        {
            string strConcat = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                DataRow dr = dt.Rows[i];
                strConcat += dr["UserFullName"].ToString() + ", " + dr["DateCreatedDate"].ToString() + "; ";
                
            }
            return strConcat;
        
        }

        public static DataTable GetAssessmentSearchResults(int intUserID)
        {
            DataTable dt = null;

            string strCheck = "SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " dbo.CheckAssessmentExistsForUser(S.SpeciesID, C.CountryID, @UserID) as UserExists," +
                                " dbo.GetUserFullName(A.UserID) as UserName, " +
                                " C.CountryID, EnglishCommonName,S.LocalCommonName,A.UserID,isnull(A.AssessmentID,0) AS AssessmentID " +
                                " FROM SPECIES S INNER JOIN SpeciesCountries SC ON SC.SpeciesID = S.SpeciesID  " +
                                " INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                                " LEFT JOIN Assessments A ON (S.SpeciesID = A.SpeciesID AND C.CountryID = A.CountryID) " +
                                " WHERE A.UserID = @UserID AND isnull(Completed,0) = 0 " +
                                " ORDER BY SpeciesDisplayName, CountryName ";

            dt = GetDataTable(strCheck, false,
                               CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));

            return dt;

        }

        public static DataTable GetSearchResults(int intCountryID, string strSpeciesSearch)
        {
            DataTable dt = null;
            string strCheck =  " SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " A.AssessmentID, A.DateCreated, U.UserID, dbo.GetUserFullName(A.UserID) as UserName, " +
                                "dbo.GetNumberAssessmentsSpecies(S.SpeciesID, C.CountryID) AS TotalAssessments, " +
                                " C.CountryID, EnglishCommonName,LocalCommonName, U.UserID, dbo.GetUserFullName(U.UserID) as UserName " +
                                " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID  " +
                                " INNER JOIN Users U ON U.UserID = A.UserID " +
                                " INNER JOIN SpeciesCountries SC ON (SC.CountryID = A.CountryID and SC.SpeciesID = A.SpeciesID) " +
                                " WHERE (ScientificName LIKE @SpeciesSearchValue " +
                                " OR GenusName LIKE @SpeciesSearchValue " +
                                " OR SpeciesName LIKE @SpeciesSearchValue " +
                                " OR ScientificName LIKE @SpeciesSearchValue " +
                                " OR SubSpeciesName LIKE @SpeciesSearchValue " +
                                " OR LocalCommonName LIKE @SpeciesSearchValue " +
                                " OR Synonyms LIKE @SpeciesSearchValue " + //Add new field Synonyms to search criteria
                                " OR EnglishCommonName LIKE @SpeciesSearchValue) " +
                                " AND A.Approved = 1 " +
                                " ORDER BY SpeciesDisplayName, CountryName, DATECREATED DESC ";

            if (strSpeciesSearch == "&&")
                //No species name limit selected - remove 'like' search to increase speed returning dataset.
                strCheck = " SELECT DISTINCT CASE " +
                                " WHEN SC.OriginID = 1 THEN CountryName + ' (Introduced)' " +
                                " ELSE CountryName END AS CountryName, dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName, S.SpeciesID, " +
                                " A.AssessmentID, A.DateCreated, U.UserID, dbo.GetUserFullName(A.UserID) as UserName, " +
                                " C.CountryID, EnglishCommonName,LocalCommonName, U.UserID, dbo.GetUserFullName(U.UserID) as UserName " +
                                " FROM Assessments A INNER JOIN Countries C ON A.CountryID = C.CountryID " +
                                " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID  " +
                                " INNER JOIN Users U ON U.UserID = A.UserID " +
                                " INNER JOIN SpeciesCountries SC ON (SC.CountryID = A.CountryID and SC.SpeciesID = A.SpeciesID) " +
                                " WHERE A.Approved = 1 " +
                                " ORDER BY SpeciesDisplayName, CountryName, DATECREATED DESC ";

            dt = GetDataTable(strCheck, false,
                                CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearch, SqlDbType.VarChar));

            if (dt != null)
            {
                string strCountryFilter = "CountryID = " + intCountryID.ToString();

                if (intCountryID > 0)
                    dt = GetFilteredDataTable(dt, strCountryFilter);
            }

            return dt;
        }
        /*
        public static DataTable GetSearchResultsCountryandSpecies(string strSpeciesSearchValue, string strCountrySearchValue, bool bUserSelected)
        {
            if (!bUserSelected)
            return GetDataTable(DatabaseQueries.SEARCH_RESULTS_BY_COUNTRY_AND_SPECIES,
              false,
              CreateSqlParameter("@CountrySearchValue", strCountrySearchValue, SqlDbType.VarChar),
              CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearchValue, SqlDbType.VarChar));
            else
                return GetDataTable(DatabaseQueries.SEARCH_RESULTS_BY_CAS_USER,
              false,
              CreateSqlParameter("@CountrySearchValue", strCountrySearchValue, SqlDbType.VarChar),
              CreateSqlParameter("@SpeciesSearchValue", strSpeciesSearchValue, SqlDbType.VarChar));
        }
        */
        public static DataTable GetUserInfo(string strUserName, string strPassword)
        {
            return GetDataTable(DatabaseQueries.GET_USER_INFO,
                    false,
                    CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar),
                    CreateSqlParameter("@Password", strPassword, SqlDbType.VarChar));
        }

        public static DataTable GetUserInfoUsername(string strUserName)
        {
            return GetDataTable(DatabaseQueries.GET_USER_INFO_USERNAME,
                    false,
                    CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar));
        }


        public static DataTable GetAssessmentUsers()
        {
            return GetDataTable(DatabaseQueries.GET_ASSESSMENT_USERS,
                    false);
        }
        
        public static DataTable GetApprovedUsers()
        {
            return GetDataTable(DatabaseQueries.GET_ALL_APPROVED_USERS,
                    false);
        }

        public static DataTable GetAssessmentsByUser()
        {
            return GetDataTable(DatabaseQueries.GET_ASSESSMENT_USER,
                    false);
        }
        
        public static DataTable GetUserInfo(int intUserID)
        {
            return GetDataTable(DatabaseQueries.GET_USER_INFO_ID,
                    false,
                    CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));
        }

        public static DataTable GetAllUserInfo(int intUserID)
        {
            return GetDataTable(DatabaseQueries.GET_ALL_USER_INFO,
                    false,
                    CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));
        }
        public static DataTable GetAllUserRequestInfo()
        {
            return GetDataTable(DatabaseQueries.GET_ALL_USER_REQ_INFO,
                    false);
        }
        public static int GetUserID(string strUserName)
        {
            int intUserID = 0;

            string strGet = "SELECT UserID FROM Users WHERE UserName = @UserName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null && dtGet.Rows.Count > 0)
                intUserID = Convert.ToInt32(dtGet.Rows[0]["UserID"]);

            return intUserID;
        }

        public static int GetUserIDForAssessment(int intUserID, int intCountryID, int intSpeciesID)
        {
            int intFoundUserID = 0;

            string strGet = "SELECT U.UserID, U.UserName " +
                            " FROM Assessments A INNER JOIN Users U ON A.UserID = U.UserID" +
                            " WHERE U.UserID = @UserID" +
                            " AND A.SpeciesID = @SpeciesID" +
                            " AND A.CountryID = @CountryID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null && dtGet.Rows.Count > 0)
                intFoundUserID = Convert.ToInt32(dtGet.Rows[0]["UserID"]);

            return intFoundUserID;
        }
        public static string GetLanguage(int LanguageID)
        {
            string strCurrentLanguage = "";

            string strGet = "SELECT * FROM Languages WHERE LanguageID = @LanguageID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@LanguageID", LanguageID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strCurrentLanguage = dtGet.Rows[0]["Language"].ToString();

            return strCurrentLanguage;
        }

        public static string GetCountry(int CountryID)
        {
            string strValue = "";

            string strGet = "SELECT * FROM Countries WHERE CountryID = @CountryID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@CountryID", CountryID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strValue = dtGet.Rows[0]["CountryName"].ToString();

            return strValue;
        }

        public static int GetLanguageID(string strLanguageName)
        {
            int intID = 0;

            string strGet = "SELECT * FROM Languages WHERE Language = @LanguageName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@LanguageName", strLanguageName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                intID = Convert.ToInt32(dtGet.Rows[0]["LanguageID"]);

            return intID;
        }

        public static int GetLanguageIDResponses(int intAssessmentID)
        {
            int intID = 0;

            string strGet = "SELECT * FROM Responses R INNER JOIN PossibleAnswers P ON R.PossibleAnswerID = P.PossibleAnswerID " +
                            "WHERE AssessmentID = @AssessmentID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0) intID = Convert.ToInt32(dtGet.Rows[0]["LanguageID"]);
            }
            return intID;
        }

        public static int GetSpeciesID(string strSpeciesName, string strGenusName)
        {
            int intID = 0;

            string strGet = "SELECT * FROM Species WHERE SpeciesName = @SpeciesName AND GenusName = @GenusName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesName", strSpeciesName, SqlDbType.VarChar),
                                        CreateSqlParameter("@GenusName", strGenusName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            { 
                if (dtGet.Rows.Count > 0)
                    intID = Convert.ToInt32(dtGet.Rows[0]["SpeciesID"]);
            }
            return intID;
        }
        public static int GetSpeciesIDFromAssessmentID(int intAssessmentID)
        {
            int intID = 0;

            string strGet = "SELECT SpeciesID FROM Assessments WHERE AssessmentID = @AssessmentID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                intID = Convert.ToInt32(dtGet.Rows[0]["SpeciesID"]);

            return intID;
        }

        public static int GetAssessmentID(int intResponseID)
        {
            int intID = 0;

            string strGet = " SELECT AssessmentID FROM Responses WHERE ResponseID = @ResponseID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@ResponseID", intResponseID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    intID = Convert.ToInt32(dtGet.Rows[0]["AssessmentID"]);
            }
            return intID;
        }

        public static bool CheckUserApproved(string strUserName)
        {
            bool bApproved = false;

            string strGet = " SELECT ISNULL(Approved,0) AS Approved FROM Users WHERE UserName = @UserName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    bApproved = Convert.ToBoolean(dtGet.Rows[0]["Approved"]);
            }
            return bApproved;
        }

        public static string FindUser(string strUserName)
        {
            string strUser = "";

            string strGet = " SELECT UserName FROM Users WHERE UserName = @UserName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strUser = dtGet.Rows[0]["UserName"].ToString();
            }

            return strUser;
        }

        public static bool CheckAssessmentArchived(int intAssessmentID)
        {
            bool bCheck = false;

            string strGet = " SELECT Archived FROM Assessments WHERE AssessmentID = @AssessmentID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    bCheck = Convert.ToBoolean(dtGet.Rows[0]["Archived"]);
            }
            return bCheck;
        }

        public static bool CheckAssessmentComplete(int intAssessmentID)
        {
            bool bCompleted = false;

            string strGet = " SELECT Completed FROM Assessments WHERE AssessmentID = @AssessmentID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    bCompleted = Convert.ToBoolean(dtGet.Rows[0]["Completed"]);
            }
            return bCompleted;
        }

        public static int GetAssessmentTriggerID(int intAssessmentID)
        {
            int intID = 0;

            string strGet = " SELECT AssessmentTriggerID FROM AssessmentTriggers WHERE AssessmentID = @AssessmentID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    intID = Convert.ToInt32(dtGet.Rows[0]["AssessmentTriggerID"]);
            }

            return intID;
        }

        public static string GetSpeciesDisplayName(int intSpeciesID)
        {
            string strRtnValue = "";

            string strGet = "SELECT SpeciesID, dbo.GetSpeciesDisplayName(SpeciesID) AS DisplayName " +
                            " FROM Species WHERE SpeciesID = @SpeciesID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strRtnValue = dtGet.Rows[0]["DisplayName"].ToString();

            return strRtnValue;
        }

        public static string GetGlobalRedListCategoryID(int intSpeciesID, int intCountryID, int intLanguageID)
        {
            string strRtnValue = "";

            string strGet = " SELECT RedListCategoryID, RLCShortName, PossibleAnswerID  " +
                            " FROM RedListCategories RLC  INNER JOIN PossibleAnswers PA ON RLC.LanguageID = PA.LanguageID and RLC.SortOrder = PA.SortOrder  " +
                            " WHERE RLCShortName = (SELECT RLCShortName " +
                                                  " FROM RedListLevels RL INNER JOIN RedListCategories RLC ON RL.RedListCategoryID = RLC.RedListCategoryID " +
                                                  " WHERE SpeciesID = @SpeciesID " +
                                                  " AND CountryID = @CountryID" +
                                                  " AND GlobalValue = 1)" +
                            " AND RLC.LanguageID = @LanguageID ";
                           
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

           
            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strRtnValue = dtGet.Rows[0]["PossibleAnswerID"].ToString();
            }

            return strRtnValue;
        }

        public static string GetNationalRedListCategoryID(int intSpeciesID, int intCountryID, int intLanguageID)
        {
            string strRtnValue = "";
            string strGet = " SELECT RedListCategoryID, RLCShortName, PossibleAnswerID  " +
                            " FROM RedListCategories RLC  INNER JOIN PossibleAnswers PA ON RLC.LanguageID = PA.LanguageID and RLC.SortOrder = PA.SortOrder  " +
                            " WHERE RLCShortName = (SELECT RLCShortName " +
                                                  " FROM RedListLevels RL INNER JOIN RedListCategories RLC ON RL.RedListCategoryID = RLC.RedListCategoryID " +
                                                  " WHERE SpeciesID = @SpeciesID " +
                                                  " AND CountryID = @CountryID" +
                                                  " AND GlobalValue = 0)" +
                            " AND RLC.LanguageID = @LanguageID ";

            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strRtnValue = dtGet.Rows[0]["PossibleAnswerID"].ToString();
            }

            return strRtnValue;
        }

        public static string GetEDScore(int intSpeciesID)
        {
            string strRtnValue = "";

            string strGet = "SELECT EDScore " +
                            "FROM Species " +
                            "WHERE SpeciesID = @SpeciesID ";

            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strRtnValue = dtGet.Rows[0]["EDScore"].ToString();
            }

            return strRtnValue;
        }

        public static string GetProtectedHabitat(int intSpeciesID)
        {
            string strRtnValue = "";

            string strGet = "SELECT  SpeciesID, GenusName,SpeciesName, ProtectedHabitat," +
                            " CASE " +
                            " WHEN ProtectedHabitat IS NULL then '0'" +
                            " WHEN ProtectedHabitat = 'No' then '2'" +
                            " WHEN ProtectedHabitat = 'Yes' then '1'" +
                            " WHEN ProtectedHabitat = 'Unknown' then '3'" +
                            " END AS ProtectedHabitatCase " +
                            " FROM    Species " +
                            " WHERE SpeciesID = @SpeciesID ";
            
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strRtnValue = dtGet.Rows[0]["ProtectedHabitatCase"].ToString();
            }

            return strRtnValue;
        }

        public static bool IsUniqueUserName(string strUserName)
        {
            bool bIsUnique = true;

            string strGet = "SELECT UserName FROM Users WHERE UserName = @UserName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    bIsUnique = false;
            }

            return bIsUnique;
        }

        public static string GetAssessmentIDForUser(int intSpeciesID, int intCountryID, string strUserName)
        {
            string strAssessID = "";
            int intUserID = DataProxy.GetUserID(strUserName);

            string strGet = "SELECT dbo.GetAssessmentIDUser(@SpeciesID, @CountryID, @UserID) AS AssessmentIDUser ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                                        CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strAssessID = dtGet.Rows[0]["AssessmentIDUser"].ToString();

            return strAssessID;
        }

        public static string GetAssessmentIDForSpeciesCountry(int intSpeciesID, int intCountryID)
        {
            string strAssessID = "";
            
            string strGet = " SELECT TOP 1 AssessmentID " +
                            " FROM Assessments" +
                            " WHERE SpeciesID = @SpeciesID" +
                            " AND CountryID = @CountryID" +
                            " ORDER BY DateCreated DESC ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strAssessID = dtGet.Rows[0]["AssessmentID"].ToString();
            }

            return strAssessID;
        }

        public static string GetUserName(int intUserID)
        {
            string strUserName = "";

            string strGet = "SELECT UserName FROM Users WHERE UserID = @UserID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strUserName = dtGet.Rows[0]["UserName"].ToString();

            return strUserName;
        }

        public static string GetUserType(string strUserName)
        {
            string strUserType = "";

            string strGet = "SELECT Description  " +
                            "FROM Users U INNER JOIN UserTypes UT ON U.UserTypeID = UT.UserTypeID " +
                            "WHERE UserName = @UserName ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strUserType = dtGet.Rows[0]["Description"].ToString();

            return strUserType;
        }

        public static string GetUserType(int intUserTypeID)
        {
            string strValue = "";

            string strGet = "SELECT * FROM UserTypes WHERE UserTypeID = @UserTypeID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UserTypeID", intUserTypeID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
                strValue = dtGet.Rows[0]["Description"].ToString();

            return strValue;
        }

        public static string GetPossibleAnswerDefinition(string intPossibleAnswerID)
        {
            string strValue = "";

            string strGet = "SELECT PossibleAnswerDefinition FROM PossibleAnswers WHERE PossibleAnswerID = @PossibleAnswerID ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@PossibleAnswerID", intPossibleAnswerID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                if (dtGet.Rows.Count > 0)
                    strValue = dtGet.Rows[0]["PossibleAnswerDefinition"].ToString();
            }
            return strValue;
        }

        public static string WrapSearchText(string strText)
        {
            string strWrappedText = "%" + strText + "%";

            return strWrappedText;
        }

        public static CultureInfo GetCurrentCultureName()
        {
           
            ResourceManager rm;
            CultureInfo ci;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name);
            rm = new ResourceManager("Resources.Strings", System.Reflection.Assembly.Load("App_GlobalResources"));
            ci = Thread.CurrentThread.CurrentCulture;

            return ci;
        }

        
        public static string LoadString(string strCurrentString, string strCurrentLanguage)
        {
            ResourceManager rm;
            CultureInfo ci;
            string strFindString = "";

            Thread.CurrentThread.CurrentCulture = new CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name);
            rm = new ResourceManager("Resources.Strings", System.Reflection.Assembly.Load("App_GlobalResources"));
            ci = Thread.CurrentThread.CurrentCulture;

            string strReturnValue = "";

            switch (strCurrentString.ToUpper())
            {
                case "RECLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "RecLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "RecLabelSpanish";
                            break;
                        case "French":
                            strFindString = "RecLabelFrench";
                            break;
                    }
                    break;

                case "FOUNDERSAVAILLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "FoundersAvailLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "FoundersAvailLabelSpanish";
                            break;
                        case "French":
                            strFindString = "FoundersAvailLabelFrench";
                            break;
                    }
                    break;

                case "HABITATREINTROAVAIL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HabitatReintroAvailEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HabitatReintroAvailSpanish";
                            break;
                        case "French":
                            strFindString = "HabitatReintroAvailFrench";
                            break;
                    }
                    break;

                case "TAXONANALYSISCOMPLETE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TaxonAnalysisCompleteEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TaxonAnalysisCompleteSpanish";
                            break;
                        case "French":
                            strFindString = "TaxonAnalysisCompleteFrench";
                            break;
                    }
                    break;

                case "RECOMMENDRESCUETITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SpeciesRecommendRescueTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SpeciesRecommendRescueTitleSpanish";
                            break;
                        case "French":
                            strFindString = "SpeciesRecommendRescueTitleFrench";
                            break;
                    }
                    break;

                case "RECOMMENDRESCUEINFO":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SpeciesRecommendRescueInfoEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SpeciesRecommendRescueInfoSpanish";
                            break;
                        case "French":
                            strFindString = "SpeciesRecommendRescueInfoFrench";
                            break;
                    }
                    break;
                
                case "RECOMMENDATIONCOUNTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "RecommendationsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "RecommendationsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "RecommendationsLabelFrench";
                            break;
                    }
                    break;

                case "ARCHIVEMESSAGE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ArchiveMessageEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ArchiveMessageSpanish";
                            break;
                        case "French":
                            strFindString = "ArchiveMessageFrench";
                            break;
                    }
                    break;

                case "UNARCHIVEMESSAGE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UnarchiveMessageEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UnarchiveMessageSpanish";
                            break;
                        case "French":
                            strFindString = "UnarchiveMessageFrench";
                            break;
                    }
                    break;

                case "CONFIRMARCHIVE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ConfirmArchiveEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ConfirmArchiveSpanish";
                            break;
                        case "French":
                            strFindString = "ConfirmArchiveFrench";
                            break;
                    }
                    break;

                case "CONFIRMUNARCHIVE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ConfirmUnarchiveEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ConfirmUnarchiveSpanish";
                            break;
                        case "French":
                            strFindString = "ConfirmUnarchiveFrench";
                            break;
                    }
                    break;


                case "Q19LABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Q19LabelTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Q19LabelTextSpanish";
                            break;
                        case "French":
                            strFindString = "Q19LabelTextFrench";
                            break;
                    }
                    break;

                case "Q20LABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Q20LabelTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Q20LabelTextSpanish";
                            break;
                        case "French":
                            strFindString = "Q20LabelTextFrench";
                            break;
                    }
                    break;

                case "AWEBLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AWebLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AWebLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AWebLabelFrench";
                            break;
                    }
                    break;

                case "RECOMMENDEDCONSERVATIONACTIONS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "RecommendedConservationActionsEnglish";
                            break;
                        case "Spanish":
                            strFindString = "RecommendedConservationActionsSpanish";
                            break;
                        case "French":
                            strFindString = "RecommendedConservationActionsFrench";
                            break;
                    }
                    break;

                case "CONSOLIDATEDHEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ConsolidatedHeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ConsolidatedHeaderSpanish";
                            break;
                        case "French":
                            strFindString = "ConsolidatedHeaderFrench";
                            break;
                    }
                    break;

                case "INCLUDEASSESSMENTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "IncludeAssessLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "IncludeAssessLabelSpanish";
                            break;
                        case "French":
                            strFindString = "IncludeAssessLabelFrench";
                            break;
                    }
                    break;
                case "CONSOLIDATEDASSESSMENTLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ConsolidatedAssessmentLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ConsolidatedAssessmentLinkSpanish";
                            break;
                        case "French":
                            strFindString = "ConsolidatedAssessmentLinkFrench";
                            break;
                    }
                    break;

                case "CONSOLIDATEDASSESSMENTTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ConsolidatedAssessmentTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ConsolidatedAssessmentTitleSpanish";
                            break;
                        case "French":
                            strFindString = "ConsolidatedAssessmentTitleFrench";
                            break;
                    }
                    break;

                case "USERNAMENOTFOUND":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UsernameNotFoundEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UsernameNotFoundSpanish";
                            break;
                        case "French":
                            strFindString = "UsernameNotFoundFrench";
                            break;
                    }
                    break;

                case "ASSESSORSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorsLabelFrench";
                            break;
                    }
                    break;

                case "FACILITATORSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "FacilitatorsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "FacilitatorsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "FacilitatorsLabelFrench";
                            break;
                    }
                    break;

                case "ALLRECSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AllRecsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AllRecsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AllRecsLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSINSTRUCTIONS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessInstructionsEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessInstructionsSpanish";
                            break;
                        case "French":
                            strFindString = "AssessInstructionsFrench";
                            break;
                    }
                    break;

                case "PASSWORDEMAILSENT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "PasswordEmailSentEnglish";
                            break;
                        case "Spanish":
                            strFindString = "PasswordEmailSentSpanish";
                            break;
                        case "French":
                            strFindString = "PasswordEmailSentFrench";
                            break;
                    }
                    break;

                case "CHANGEPASSWORDLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ChangePasswordLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ChangePasswordLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ChangePasswordLabelFrench";
                            break;
                    }
                    break;

                case "CHANGEPASSWORDCONTENT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ChangePasswordContentEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ChangePasswordContentSpanish";
                            break;
                        case "French":
                            strFindString = "ChangePasswordContentFrench";
                            break;
                    }
                    break;

                case "ALLASSESSORSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AllAssessorsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AllAssessorsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AllAssessorsLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTSBYUSERREPORTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsByUserReportLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsByUserReportLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsByUserReportLabelFrench";
                            break;
                    }
                    break;

                case "FALABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "FALabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "FALabelSpanish";
                            break;
                        case "French":
                            strFindString = "FALabelFrench";
                            break;
                    }
                    break;

                case "COMPLETEDASSESSMENTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CompletedAssessmentsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CompletedAssessmentsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "CompletedAssessmentsLabelFrench";
                            break;
                    }
                    break;

                case "COMPLETEDASSESSMENTSLABELALL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CompletedAssessmentsLabelEnglishAll";
                            break;
                        case "Spanish":
                            strFindString = "CompletedAssessmentsLabelSpanishAll";
                            break;
                        case "French":
                            strFindString = "CompletedAssessmentsLabelFrenchAll";
                            break;
                    }
                    break;

                case "ASSESSMENTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTSLABELFILTER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsLabelEnglishFilter";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsLabelSpanishFilter";
                            break;
                        case "French":
                            strFindString = "AssessmentsLabelFrenchFilter";
                            break;
                    }
                    break;

                case "ASSESSMENTSLABELFILTERNC":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsLabelEnglishFilterNC";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsLabelSpanishFilterNC";
                            break;
                        case "French":
                            strFindString = "AssessmentsLabelFrenchFilterNC";
                            break;
                    }
                    break;
                case "ASSESSMENTSDDLLVALUE1":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsDDLValue1English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsDDLValue1Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsDDLValue1French";
                            break;
                    }
                    break;

                case "ASSESSMENTSDDLLVALUE2":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsDDLValue2English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsDDLValue2Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsDDLValue2French";
                            break;
                    }
                    break;

                case "ASSESSMENTSDDLLVALUE3":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsDDLValue3English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsDDLValue3Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsDDLValue3French";
                            break;
                    }
                    break;

                case "EXPORTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ExportLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ExportLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ExportLabelFrench";
                            break;
                    }
                    break;

                case "TAXONOMICLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TaxonomicLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TaxonomicLabelSpanish";
                            break;
                        case "French":
                            strFindString = "TaxonomicLabelFrench";
                            break;
                    }
                    break;

                case "SORTBYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SortByLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SortByLabelSpanish";
                            break;
                        case "French":
                            strFindString = "SortByLabelFrench";
                            break;
                    }
                    break;

                case "PRIORITYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "PriorityLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "PriorityLabelSpanish";
                            break;
                        case "French":
                            strFindString = "PriorityLabelFrench";
                            break;
                    }
                    break;

                case "NATIONALASSESSMENTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "NationalAssessmentsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "NationalAssessmentsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "NationalAssessmentsLabelFrench";
                            break;
                    }
                    break;

                case "NATIONALASSESSMENTSLABELINFO":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "NationalAssessmentsLabelInfoEnglish";
                            break;
                        case "Spanish":
                            strFindString = "NationalAssessmentsLabelInfoSpanish";
                            break;
                        case "French":
                            strFindString = "NationalAssessmentsLabelInfoFrench";
                            break;
                    }
                    break;

                case "PRINTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "PrintLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "PrintLabelSpanish";
                            break;
                        case "French":
                            strFindString = "PrintLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTSBYUSERLINKINFO":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsByUserLinkInfoEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsByUserLinkInfoSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsByUserLinkInfoFrench";
                            break;
                    }
                    break;

                case "APPROVEDUSERSLINKINFO":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApprovedUsersLinkInfoEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApprovedUsersLinkInfoSpanish";
                            break;
                        case "French":
                            strFindString = "ApprovedUsersLinkInfoFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTDATELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentDateLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentDateLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentDateLabelFrench";
                            break;
                    }
                    break;

                case "DATELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DateLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DateLabelSpanish";
                            break;
                        case "French":
                            strFindString = "DateLabelFrench";
                            break;
                    }
                    break;
                case "ASSESSMENTSBYUSERLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentsByUserLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentsByUserLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentsByUserLabelFrench";
                            break;
                    }
                    break;

                case "USERTYPELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UserTypeLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UserTypeLabelSpanish";
                            break;
                        case "French":
                            strFindString = "UserTypeLabelFrench";
                            break;
                    }
                    break;

                case "APPROVEDUSERSTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApprovedUsersTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApprovedUsersTitleSpanish";
                            break;
                        case "French":
                            strFindString = "ApprovedUsersTitleFrench";
                            break;
                    }
                    break;

                case "UPDATEUSERPROFILESUCCESS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UpdateProfileSuccessEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UpdateProfileSuccessSpanish";
                            break;
                        case "French":
                            strFindString = "UpdateProfileSuccessFrench";
                            break;
                    }
                    break;

                case "EDITUSERPROFILEINSTRUCTIONS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditUserProfileInstructionsEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditUserProfileInstructionsSpanish";
                            break;
                        case "French":
                            strFindString = "EditUserProfileInstructionsFrench";
                            break;
                    }
                    break;

                case "EDITUSERPROFILELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditUserProfileLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditUserProfileLabelSpanish";
                            break;
                        case "French":
                            strFindString = "EditUserProfileLabelFrench";
                            break;
                    }
                    break;

                case "EDITUSERPROFILECONTENT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditUserProfileLabelContentEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditUserProfileLabelContentSpanish";
                            break;
                        case "French":
                            strFindString = "EditUserProfileLabelContentFrench";
                            break;
                    }
                    break;

                case "REPORTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ReportsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ReportsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ReportsLabelFrench";
                            break;
                    }
                    break;

                case "UNAPPROVELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UnapproveLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UnapproveLabelSpanish";
                            break;
                        case "French":
                            strFindString = "UnapproveLabelFrench";
                            break;
                    }
                    break;

                case "APPROVEDLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApprovedLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApprovedLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ApprovedLabelFrench";
                            break;
                    }
                    break;

                case "NOTAPPROVEDLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "NotApprovedLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "NotApprovedLabelSpanish";
                            break;
                        case "French":
                            strFindString = "NotApprovedLabelFrench";
                            break;
                    }
                    break;

                case "APPROVETEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApproveTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApproveTextSpanish";
                            break;
                        case "French":
                            strFindString = "ApproveTextFrench";
                            break;
                    }
                    break;

                case "ARCHIVETEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ArchiveTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ArchiveTextSpanish";
                            break;
                        case "French":
                            strFindString = "ArchiveTextFrench";
                            break;
                    }
                    break;

                 case "EDITTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditTextSpanish";
                            break;
                        case "French":
                            strFindString = "EditTextFrench";
                            break;
                    }
                    break;

                case "DENYTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DenyTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DenyTextSpanish";
                            break;
                        case "French":
                            strFindString = "DenyTextFrench";
                            break;
                    }
                    break;

                case "APPROVEASSESSORPARAGRAPHTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApproveAssessorParagraphTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApproveAssessorParagraphTextSpanish";
                            break;
                        case "French":
                            strFindString = "ApproveAssessorParagraphTextFrench";
                            break;
                    }
                    break;

                case "APPROVECOMPLETEDASSESSMENTSPARAGRAPHTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApproveCompletedAssessmentsParagraphTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApproveCompletedAssessmentsParagraphTextSpanish";
                            break;
                        case "French":
                            strFindString = "ApproveCompletedAssessmentsParagraphTextFrench";
                            break;
                    }
                    break;

                case "APPROVECOMPLETEDASSESSMENTS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApproveCompletedAssessmentsEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApproveCompletedAssessmentsSpanish";
                            break;
                        case "French":
                            strFindString = "ApproveCompletedAssessmentsFrench";
                            break;
                    }
                    break;

                case "APPROVEUSERREQUEST":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ApproveUserRequestEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ApproveUserRequestSpanish";
                            break;
                        case "French":
                            strFindString = "ApproveUserRequestFrench";
                            break;
                    }
                    break;

                case "INCOMPLETEASSESSMENTS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "MyIncompleteAssessmentsEnglish";
                            break;
                        case "Spanish":
                            strFindString = "MyIncompleteAssessmentsSpanish";
                            break;
                        case "French":
                            strFindString = "MyIncompleteAssessmentsFrench";
                            break;
                    }
                    break;

                case "ASSESSORPARAGRAPHTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorParagraphTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorParagraphTextSpanish";
                            break;
                        case "French":             
                            strFindString = "AssessorParagraphTextFrench";
                            break;
                    }
                    break;

                case "TOPIC1LABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HelpTopic1LabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HelpTopic1LabelSpanish";
                            break;
                        case "French":
                            strFindString = "HelpTopic1LabelFrench";
                            break;
                    }
                    break;

                case "TOPIC2LABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HelpTopic2LabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HelpTopic2LabelSpanish";
                            break;
                        case "French":
                            strFindString = "HelpTopic2LabelFrench";
                            break;
                    }
                    break;

                case "TOPIC3LABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HelpTopic3LabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HelpTopic3LabelSpanish";
                            break;
                        case "French":
                            strFindString = "HelpTopic3LabelFrench";
                            break;
                    }
                    break;

                case "TOPIC4LABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HelpTopic4LabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HelpTopic4LabelSpanish";
                            break;
                        case "French":
                            strFindString = "HelpTopic4LabelFrench";
                            break;
                    }
                    break;

                case "ASSESSSAVETRIGGERDEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessSaveOKTriggerDelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessSaveOKTriggerDelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessSaveOKTriggerDelFrench";
                            break;
                    }
                    break;

                case "ADDCOMMENTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AddCommentsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AddCommentsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AddCommentsLabelFrench";
                            break;
                    }
                    break;

                case "COMPLETEDLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CompletedLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CompletedLabelSpanish";
                            break;
                        case "French":
                            strFindString = "CompletedLabelFrench";
                            break;
                    }
                    break;

                case "INCOMPLETELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "IncompleteLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "IncompleteLabelSpanish";
                            break;
                        case "French":
                            strFindString = "IncompleteLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTSTATUSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentStatusLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentStatusLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentStatusLabelFrench";
                            break;
                    }
                    break;
                case "ARCHIVEDSTATUSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ArchivedStatusEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ArchivedStatusSpanish";
                            break;
                        case "French":
                            strFindString = "ArchivedStatusFrench";
                            break;
                    }
                    break;

                case "INCOMPLETERECTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "IncompleteRecTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "IncompleteRecTextSpanish";
                            break;
                        case "French":
                            strFindString = "IncompleteRecTextFrench";
                            break;
                    }
                    break;

                case "NOTAPPROVEDTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "NotApprovedTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "NotApprovedTextSpanish";
                            break;
                        case "French":
                            strFindString = "NotApprovedTextFrench";
                            break;
                    }
                    break;
                case "USERCOUNTRIESTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UserCountriesTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UserCountriesTextSpanish";
                            break;
                        case "French":
                            strFindString = "UserCountriesTextFrench";
                            break;
                    }
                    break;

                case "WARNINGSAVEDISABLED":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "WarningSaveDisabledEnglish";
                            break;
                        case "Spanish":
                            strFindString = "WarningSaveDisabledSpanish";
                            break;
                        case "French":
                            strFindString = "WarningSaveDisabledFrench";
                            break;
                    }
                    break;

                case "DELETETEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DeleteTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DeleteTextSpanish";
                            break;
                        case "French":
                            strFindString = "DeleteTextFrench";
                            break;
                    }
                    break;

                case "QUESTIONNUMBERLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "QuestionNumberLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "QuestionNumberLabelSpanish";
                            break;
                        case "French":
                            strFindString = "QuestionNumberLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSEDBYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessedByEnglishLabel";
                            break;
                        case "Spanish":
                            strFindString = "AssessedBySpanishLabel";
                            break;
                        case "French":
                            strFindString = "AssessedByFrenchLabel";
                            break;
                    }
                    break;

                case "HELPLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HelpLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HelpLinkSpanish";
                            break;
                        case "French":
                            strFindString = "HelpLinkFrench";
                            break;
                    }
                    break;

                case "DASHBOARDLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DashboardLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DashboardLinkSpanish";
                            break;
                        case "French":
                            strFindString = "DashboardLinkFrench";
                            break;
                    }
                    break;

                case "SEARCHALLTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchAllTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchAllTitleSpanish";
                            break;
                        case "French":
                            strFindString = "SearchAllTitleFrench";
                            break;
                    }
                    break;

                case "NEWSEARCHTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchTitleSpanish";
                            break;
                        case "French":
                            strFindString = "SearchTitleFrench";
                            break;
                    }
                    break;

                case "SEARCHALLMAIN":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchAllMainEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchAllMainSpanish";
                            break;
                        case "French":
                            strFindString = "SearchAllMainFrench";
                            break;
                    }
                    break;

                case "CAPTIVEBREEDINGLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CaptiveBreedingLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CaptiveBreedingLabelSpanish";
                            break;
                        case "French":
                            strFindString = "CaptiveBreedingLabelFrench";
                            break;
                    }
                    break;

                case "COMMONNAMELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CommonNameLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CommonNameLabelSpanish";
                            break;
                        case "French":
                            strFindString = "CommonNameLabelFrench";
                            break;
                    }
                    break;

                case "LOCALCOMMONNAMELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LocalCommonNameLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LocalCommonNameLabelSpanish";
                            break;
                        case "French":
                            strFindString = "LocalCommonNameLabelFrench";
                            break;
                    }
                    break;

                case "TRIGGERREC":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TriggerRecEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TriggerRecSpanish";
                            break;
                        case "French":
                            strFindString = "TriggerRecFrench";
                            break;
                    }
                    break;

                case "TOTALASSESSMENTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TotalAssessmentsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TotalAssessmentsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "TotalAssessmentsLabelFrench";
                            break;
                    }
                    break;

                case "ASESSMENTSEARCHNEW":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentSearchNewEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentSearchNewSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentSearchNewFrench";
                            break;
                    }
                    break;

                case "ASESSMENTSEARCHALLNEW":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentSearchAllNewEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentSearchAllNewSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentSearchAllNewFrench";
                            break;
                    }
                    break;

                case "ADDASSESSMENTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AddAssessmentLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AddAssessmentLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AddAssessmentLabelFrench";
                            break;
                    }
                    break;

                case "EDITASSESSMENTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditAssessmentLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditAssessmentLabelSpanish";
                            break;
                        case "French":
                            strFindString = "EditAssessmentLabelFrench";
                            break;
                    }
                    break;

                case "EDITINCOMPLETEASSESSMENTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditIncompleteAssessmentLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditIncompleteAssessmentLabelSpanish";
                            break;
                        case "French":
                            strFindString = "EditIncompleteAssessmentLabelFrench";
                            break;
                    }
                    break;

                case "INVALIDLOGINERRORTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "InvalidLoginErrorTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "InvalidLoginErrorTextSpanish";
                            break;
                        case "French":
                            strFindString = "InvalidLoginErrorTextFrench";
                            break;
                    }
                    break;

                case "MARKASSESSMENTCOMPLETETEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "MarkAssessmentCompleteTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "MarkAssessmentCompleteTextSpanish";
                            break;
                        case "French":
                            strFindString = "MarkAssessmentCompleteTextFrench";
                            break;
                    }
                    break;
                case "EDITASSESSMENTTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EditAssessmentTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EditAssessmentTitleSpanish";
                            break;
                        case "French":
                            strFindString = "EditAssessmentTitleFrench";
                            break;
                    }
                    break;

                case "RESPONSELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ResponseLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ResponseLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ResponseLabelFrench";
                            break;
                    }
                    break;

                case "CONSOLIDATEDRESPONSELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ConsolidatedResponseLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ConsolidatedResponseLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ConsolidatedResponseLabelFrench";
                            break;
                    }
                    break;

                case "SHORTNAMELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ShortNameLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ShortNameLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ShortNameLabelFrench";
                            break;
                    }
                    break;

                case "QUESTIONTEXTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "QuestionTextLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "QuestionTextLabelSpanish";
                            break;
                        case "French":
                            strFindString = "QuestionTextLabelFrench";
                            break;
                    }
                    break;

                case "COMMENTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CommentsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CommentsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "CommentsLabelFrench";
                            break;
                    }
                    break;

                case "SUCCESSSAVE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SuccessSaveEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SuccessSaveSpanish";
                            break;
                        case "French":
                            strFindString = "SuccessSaveFrench";
                            break;
                    }
                    break;

                case "POSSIBLEANSWERLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "PADefinitionEnglish";
                            break;
                        case "Spanish":
                            strFindString = "PADefinitionSpanish";
                            break;
                        case "French":
                            strFindString = "PADefinitionFrench";
                            break;
                    }
                    break;

                case "SECTION1TITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Section1TitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Section1TitleSpanish";
                            break;
                        case "French":
                            strFindString = "Section1TitleFrench";
                            break;
                    }
                    break;

                case "SECTION2TITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Section2TitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Section2TitleSpanish";
                            break;
                        case "French":
                            strFindString = "Section2TitleFrench";
                            break;
                    }
                    break;

                case "SECTION3TITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Section3TitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Section3TitleSpanish";
                            break;
                        case "French":
                            strFindString = "Section3TitleFrench";
                            break;
                    }
                    break;

                case "SECTION4TITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Section4TitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Section4TitleSpanish";
                            break;
                        case "French":
                            strFindString = "Section4TitleFrench";
                            break;
                    }
                    break;

                case "SAVEAS5ESSMENT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SaveAssessmentEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SaveAssessmentSpanish";
                            break;
                        case "French":
                            strFindString = "SaveAssessmentFrench";
                            break;
                    }
                    break;

                case "SECTION5TITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Section5TitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Section5TitleSpanish";
                            break;
                        case "French":
                            strFindString = "Section5TitleFrench";
                            break;
                    }
                    break;

                case "SECTION6TITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "Section6TitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "Section6TitleSpanish";
                            break;
                        case "French":
                            strFindString = "Section6TitleFrench";
                            break;
                    }
                    break;

                case "CREATEASSESSMENTTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CreateAssessmentTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CreateAssessmentTitleSpanish";
                            break;
                        case "French":
                            strFindString = "CreateAssessmentTitleFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTRESULTSCOUNTRYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentResultsCountryLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentResultsCountryLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentResultsCountryLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTRESULTSBYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentResultsByLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentResultsByLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentResultsByLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTRESULTSONLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentResultsOnLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentResultsOnLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentResultsOnLabelFrench";
                            break;
                    }
                    break;

                case "DUPLCATEUSERNAMEERROR":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DuplicateUserNameErrorEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DuplicateUserNameErrorSpanish";
                            break;
                        case "French":
                            strFindString = "DuplicateUserNameErrorFrench";
                            break;
                    }
                    break;

                case "GENERALINSERTERROR":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "GeneralInsertErrorEnglish";
                            break;
                        case "Spanish":
                            strFindString = "GeneralInsertErrorSpanish";
                            break;
                        case "French":
                            strFindString = "GeneralInsertErrorFrench";
                            break;
                    }
                    break;

                case "SUCCESSINSERTUSER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SuccessInsertUserEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SuccessInsertUserSpanish";
                            break;
                        case "French":
                            strFindString = "SuccessInsertUserFrench";
                            break;
                    }
                    break;

                case "LOGINLABEL":
                case "LOGINBUTTON":
                case "LOGINLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LoginLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LoginLabelSpanish";
                            break;
                        case "French":
                            strFindString = "LoginLabelFrench";
                            break;
                    }
                    break;

                case "SEARCHRESULTSNOCURRENTUSER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchResultsNoCurrentUserEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchResultsNoCurrentUserSpanish";
                            break;
                        case "French":
                            strFindString = "SearchResultsNoCurrentUserFrench";
                            break;
                    }
                    break;

                case "SAVEBUTTON":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SaveButtonEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SaveButtonSpanish";
                            break;
                        case "French":
                            strFindString = "SaveButtonFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTQUESTIONSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentQuestionsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentQuestionsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentQuestionsLabelFrench";
                            break;
                    }
                    break;

                case "REDLISTMAPLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "RedListMapLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "RedListMapLabelSpanish";
                            break;
                        case "French":
                            strFindString = "RedListMapLabelFrench";
                            break;
                    }
                    break;

                case "ADDOBSERVATIONINATURALISTLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AddObservationINaturalistLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AddObservationINaturalistLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AddObservationINaturalistLabelFrench";
                            break;
                    }
                    break;

                case "GLOBALRLCLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "GlobalRLCLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "GlobalRLCLabelSpanish";
                            break;
                        case "French":
                            strFindString = "GlobalRLCLabelFrench";
                            break;
                    }
                    break;

                case "NATIONALRLCLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "NationalRLCLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "NationalRLCLabelSpanish";
                            break;
                        case "French":
                            strFindString = "NationalRLCLabelFrench";
                            break;
                    }
                    break;

                case "EDSCORELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "EDScoreLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "EDScoreLabelSpanish";
                            break;
                        case "French":
                            strFindString = "EDScoreLabelFrench";
                            break;
                    }
                    break;

                case "DISTRIBUTIONLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DistributionLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DistributionLabelSpanish";
                            break;
                        case "French":
                            strFindString = "DistributionLabelFrench";
                            break;
                    }
                    break;

                case "FAMILYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "FamilyLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "FamilyLabelSpanish";
                            break;
                        case "French":
                            strFindString = "FamilyLabelFrench";
                            break;
                    }
                    break;

                case "ORDERLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "OrderLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "OrderLabelSpanish";
                            break;
                        case "French":
                            strFindString = "OrderLabelFrench";
                            break;
                    }
                    break;

                case "PHONELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "PhoneLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "PhoneLabelSpanish";
                            break;
                        case "French":
                            strFindString = "PhoneLabelFrench";
                            break;
                    }
                    break;

                case "TOPRIGHTHOMETITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TopRightHomeTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TopRightHomeTitleSpanish";
                            break;
                        case "French":
                            strFindString = "TopRightHomeTitleFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTRESULTSLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentResultsLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentResultsLabelSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentResultsLabelFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewTitleSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewTitleFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT1":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent1English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent1Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent1French";
                            break;
                    }
                    break;
                case "ASSESSMENTOVERVIEWCONTENT1HEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent1HeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent1HeaderSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent1HeaderFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT2":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent2English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent2Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent2French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT2LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent2LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent2LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent2LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT2B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent2BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent2BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent2BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT3":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent3English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent3Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent3French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT4":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent4English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent4Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent4French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT4LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent4LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent4LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent4LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT4B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent4BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent4BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent4BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT5":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent5English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent5Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent5French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT5LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent5LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent5LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent5LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT5B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent5BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent5BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent5BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT6":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent6English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent6Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent6French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT7":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent7English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent7Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent7French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT7LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent7LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent7LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent7LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT7B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent7BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent7BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent7BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT7LINK2":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent7Link2English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent7Link2Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent7Link2French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT7C":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent7CEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent7CSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent7CFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT8HEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent8HeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent8HeaderSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent8HeaderFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT8":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent8English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent8Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent8French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT8LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent8LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent8LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent8LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT8B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent8BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent8BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent8BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT9HEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent9HeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent9HeaderSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent9HeaderFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT9":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent9English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent9Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent9French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT10":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent10English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent10Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent10French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT11":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent11English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent11Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent11French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT12":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent12English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent12Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent12French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT12LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent12LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent12LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent12LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT12B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent12BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent12BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent12BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT13":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent13English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent13Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent13French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT14":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent14English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent14Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent14French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15BFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15LINK2":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15Link2English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15Link2Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15Link2French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15C":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15CEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15CSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15CFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15LINK3":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15Link3English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15Link3Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15Link3French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15LINK4":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15Link4English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15Link4Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15Link4French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15D":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15DEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15DSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15DFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15LINK5":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15Link5English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15Link5Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15Link5French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15E":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15EEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15ESpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15EFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT15LINK6":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent15Link6English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent15Link6Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent15Link6French";
                            break;
                    }
                    break;

                case "ASSESSMENTOVERVIEWCONTENT16":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentOverviewContent16English";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentOverviewContent16Spanish";
                            break;
                        case "French":
                            strFindString = "AssessmentOverviewContent16French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT1":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent1English";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent1Spanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent1French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT2":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent2English";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent2Spanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent2French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT3":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent3English";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent3Spanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent3French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT3LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent3LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent3LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent3LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT3B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent3BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent3BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent3BFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT4HEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent4HeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent4HeaderSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent4HeaderFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT4":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent4English";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent4Spanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent4French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT4FUTURE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent4FutureEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent4FutureSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent4FutureFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT5HEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent5HeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent5HeaderSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent5HeaderFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT5":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent5English";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent5Spanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent5French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT5FUTURE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent5FutureEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent5FutureSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent5FutureFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT6HEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent6HeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent6HeaderSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent6HeaderFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT6":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent6English";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent6Spanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent6French";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT6LINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent6LinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent6LinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent6LinkFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOCONTENT6B":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoContent6BEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoContent6BSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoContent6BFrench";
                            break;
                    }
                    break;

                case "ASSESSORINFOTITLE":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessorInfoTitleEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessorInfoTitleSpanish";
                            break;
                        case "French":
                            strFindString = "AssessorInfoTitleFrench";
                            break;
                    }
                    break;

                case "TOPRIGHTHOMECONTENT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TopRightHomeContentEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TopRightHomeContentSpanish";
                            break;
                        case "French":
                            strFindString = "TopRightHomeContentFrench";
                            break;
                    }
                    break;

                case "PASSWORDLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "PasswordLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "PasswordLabelSpanish";
                            break;
                        case "French":
                            strFindString = "PasswordLabelFrench";
                            break;
                    }
                    break;

                case "FORGETPASSWORD":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ForgetPasswordEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ForgetPasswordSpanish";
                            break;
                        case "French":
                            strFindString = "ForgetPasswordFrench";
                            break;
                    }
                    break;

                case "SIGNUPTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SignupTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SignupTextSpanish";
                            break;
                        case "French":
                            strFindString = "SignupTextFrench";
                            break;
                    }
                    break;

                case "SIGNUPHEADER":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SignupHeaderEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SignupHeaderSpanish";
                            break;
                        case "French":
                            strFindString = "SignupHeaderFrench";
                            break;
                    }
                    break;

                case "SIGNUPCONTENT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SignupContentEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SignupContentSpanish";
                            break;
                        case "French":
                            strFindString = "SignupContentFrench";
                            break;
                    }
                    break;

                case "FIRSTNAMELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "FirstNameLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "FirstNameLabelSpanish";
                            break;
                        case "French":
                            strFindString = "FirstNameLabelFrench";
                            break;
                    }
                    break;

                case "LASTNAMELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LastNameLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LastNameLabelSpanish";
                            break;
                        case "French":
                            strFindString = "LastNameLabelFrench";
                            break;
                    }
                    break;

                case "ASGMEMBERLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ASGMemberLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ASGMemberLabelSpanish";
                            break;
                        case "French":
                            strFindString = "ASGMemberLabelFrench";
                            break;
                    }
                    break;

                case "ORGANIZATIONLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "OrganizationLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "OrganizationLabelSpanish";
                            break;
                        case "French":
                            strFindString = "OrganizationLabelFrench";
                            break;
                    }
                    break;

                case "TITLELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "TitleLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "TitleLabelEnglish";
                            break;
                        case "French":
                            strFindString = "TitleLabelFrench";
                            break;
                    }
                    break;

                case "COUNTRYLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CountryLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CountryLabelSpanish";
                            break;
                        case "French":
                            strFindString = "CountryLabelFrench";
                            break;
                    }
                    break;

                case "EMAILLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "Spanish":
                            strFindString = "EmailLabelSpanish";
                            break;
                        case "English":
                            strFindString = "EmailLabelEnglish";
                            break;
                        case "French":
                            strFindString = "EmailLabelFrench";
                            break;
                    }
                    break;

                case "READMORETEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ReadMoreTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ReadMoreTextSpanish";
                            break;
                        case "French":
                            strFindString = "ReadMoreTextFrench";
                            break;
                    }
                    break;

                case "WELCOMETEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "WelcomeTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "WelcomeTextSpanish";
                            break;
                        case "French":
                            strFindString = "WelcomeTextFrench";
                            break;
                    }
                    break;

                case "LANGUAGECONTENTTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LanguageContentTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LanguageContentTextSpanish";
                            break;
                        case "French":
                            strFindString = "LanguageContentTextFrench";
                            break;
                    }
                    break;

                case "RECENTASSESSMENTS":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "RecentAssessmentsEnglish";
                            break;
                        case "Spanish":
                            strFindString = "RecentAssessmentsSpanish";
                            break;
                        case "French":
                            strFindString = "RecentAssessmentsFrench";
                            break;
                    }
                    break;

                case "REQUIREDFIELDSERROR":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "RequiredFieldsErrorEnglish";
                            break;
                        case "Spanish":
                            strFindString = "RequiredFieldsErrorSpanish";
                            break;
                        case "French":
                            strFindString = "RequiredFieldsErrorFrench";
                            break;
                    }
                    break;

                case "USERNAMELABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "UsernameLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "UsernameLabelSpanish";
                            break;
                        case "French":
                            strFindString = "UsernameLabelFrench";
                            break;
                    }
                    break;

                case "MAP":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "MapEnglish";
                            break;
                        case "Spanish":
                            strFindString = "MapSpanish";
                            break;
                        case "French":
                            strFindString = "MapFrench";
                            break;
                    }
                    break;

                case "NORECORDSFOUND":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "NoRecordsFoundEnglish";
                            break;
                        case "Spanish":
                            strFindString = "NoRecordsFoundSpanish";
                            break;
                        case "French":
                            strFindString = "NoRecordsFoundFrench";
                            break;
                    }
                    break;

                case "SIGNUPBUTTON":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SignupButtonEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SignupButtonSpanish";
                            break;
                        case "French":
                            strFindString = "SignupButtonFrench";
                            break;
                    }
                    break;

                case "CHECKBOXRESULTSTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CheckBoxResultsTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CheckBoxResultsTextSpanish";
                            break;
                        case "French":
                            strFindString = "CheckBoxResultsTextFrench";
                            break;
                    }
                    break;

                case "LOGINTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LoginTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LoginTextSpanish";
                            break;
                        case "French":
                            strFindString = "LoginTextFrench";
                            break;
                    }
                    break;

                case "LOGINCONTENTTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LoginContentTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LoginContentTextSpanish";
                            break;
                        case "French":
                            strFindString = "LoginContentTextFrench";
                            break;
                    }
                    break;

                case "DEFAULTCONTENTTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "DefaultContentTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "DefaultContentTextSpanish";
                            break;
                        case "French":
                            strFindString = "DefaultContentTextFrench";
                            break;
                    }
                    break;

                case "SEARCHCONTENTTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchContentTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchContentTextSpanish";
                            break;
                        case "French":
                            strFindString = "SearchContentTextFrench";
                            break;
                    }
                    break;

                case "SPECIESLABEL":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SpeciesLabelEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SpeciesLabelSpanish";
                            break;
                        case "French":
                            strFindString = "SpeciesLabelFrench";
                            break;
                    }
                    break;
                case "SEARCHMAIN":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchMainEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchMainSpanish";
                            break;
                        case "French":
                            strFindString = "SearchMainFrench";
                            break;
                    }
                    break;

                case "CONTACTUSCONTENTTEXT":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ContactUsContentTextEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ContactUsContentTextSpanish";
                            break;
                        case "French":
                            strFindString = "ContactUsContentTextFrench";
                            break;
                    }
                    break;
                case "SUBMITBUTTON":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SubmitButtonEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SubmitButtonSpanish";
                            break;
                        case "French":
                            strFindString = "SubmitButtonFrench";
                            break;
                    }
                    break;

                case "CANCELBUTTON":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "CancelButtonEnglish";
                            break;
                        case "Spanish":
                            strFindString = "CancelButtonSpanish";
                            break;
                        case "French":
                            strFindString = "CancelButtonFrench";
                            break;
                    }
                    break;

                case "HOMELINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "HomeLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "HomeLinkSpanish";
                            break;
                        case "French":
                            strFindString = "HomeLinkFrench";
                            break;
                    }
                    break;

                case "ABOUTASSESSMENTLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AboutAssessmentLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AboutAssessmentLinkSpanish";
                            break;
                        case "French":
                            strFindString = "AboutAssessmentLinkFrench";
                            break;
                    }
                    break;

                case "LANGUAGESLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "LanguagesLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "LanguagesLinkSpanish";
                            break;
                        case "French":
                            strFindString = "LanguagesLinkFrench";
                            break;
                    }
                    break;

                case "ASSESSMENTRESULTSLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "AssessmentResultsLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "AssessmentResultsLinkSpanish";
                            break;
                        case "French":
                            strFindString = "AssessmentResultsLinkFrench";
                            break;
                    }
                    break;

                case "SEARCHLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "SearchLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "SearchLinkSpanish";
                            break;
                        case "French":
                            strFindString = "SearchLinkFrench";
                            break;
                    }
                    break;

                case "CONTACTUSLINK":
                    switch (strCurrentLanguage)
                    {
                        case "English":
                            strFindString = "ContactUsLinkEnglish";
                            break;
                        case "Spanish":
                            strFindString = "ContactUsLinkSpanish";
                            break;
                        case "French":
                            strFindString = "ContactUsLinkFrench";
                            break;
                    }
                    break;
            }
            

            strReturnValue = rm.GetString(strFindString, ci);
      
            return strReturnValue;
        }
        /*
        public static DataTable GetSearchResultsSpecies(string strSearchValue, bool bUserSelected)
        {
            if (!bUserSelected)
                return GetDataTable(DatabaseQueries.SEARCH_RESULTS_BY_SPECIES,
                  false,
                  CreateSqlParameter("@SpeciesSearchValue", strSearchValue, SqlDbType.VarChar));
            else
                return GetDataTable(DatabaseQueries.SEARCH_RESULTS_BY_S_USER,
                    false,
                    CreateSqlParameter("@SpeciesSearchValue", strSearchValue, SqlDbType.VarChar));
        }

        public static DataTable GetSearchResultsCountry(string strSearchValue, bool bUserSelected)
        {
            if (!bUserSelected)
                return GetDataTable(DatabaseQueries.SEARCH_RESULTS_BY_COUNTRY,
                false,
                CreateSqlParameter("@CountrySearchValue", strSearchValue, SqlDbType.VarChar));
            else
                return GetDataTable(DatabaseQueries.SEARCH_RESULTS_BY_C_USER,
                false,
                CreateSqlParameter("@CountrySearchValue", strSearchValue, SqlDbType.VarChar));
        }
        */
        private static DataTable GetSortedDataTable(DataTable OriginalDataTable, string SortExpression)
        {
            DataTable _sortedDataTable = OriginalDataTable;
            try
            {
                DataView _view = _sortedDataTable.AsDataView();
                _view.Sort = SortExpression;
                _sortedDataTable = _view.ToTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _sortedDataTable;

        }

        public static DataTable GetFilteredDisplayInfo(int intUploadProgramID, int intUploadTypeID, int intNoRecs)
        {
            DataTable dtAllResults = null;
            DataTable dtFilteredResults = null;

            dtAllResults = GetQAForProgram(intUploadProgramID, intUploadTypeID);
            dtFilteredResults = GetTopRecordsFromDataTable(dtAllResults, intNoRecs);

            return dtFilteredResults;
        }


        public static DataTable GetFilteredDisplayInfoTypeOnly(int intUploadTypeID, int intNoRecs)
        {
            DataTable dtAllResults = null;
            DataTable dtFilteredResults = null;

            dtAllResults = GetQAForType(intUploadTypeID);
            dtFilteredResults = GetTopRecordsFromDataTable(dtAllResults, intNoRecs);

            return dtFilteredResults;
        }

        public static DataTable GetTopRecordsFromDataTable(DataTable OriginalDataTable, int NoOfRecords)
        {
            DataTable _filteredDataTable = new DataTable();
            _filteredDataTable.Rows.Clear();

            // If the number of records requested from the original datatable is less than or equal to the number
            // of rows in the table, just return the original table
            if (OriginalDataTable.Rows.Count <= NoOfRecords) return OriginalDataTable;
            
            // If there are more rows in the original datatable than request, grab each of the top rows
            // in the datatable and import them into a new, filtered datatable

            // Create the destination columns in the filtered datatable first
            foreach (DataColumn _column in OriginalDataTable.Columns)
            {
                _filteredDataTable.Columns.Add(_column.ColumnName, _column.DataType);

            }

            // Import each of the rows
            for (int _rowNumber = 0; _rowNumber < NoOfRecords; _rowNumber++)
            {
                DataRow _importedRow = OriginalDataTable.Rows[_rowNumber];
                _filteredDataTable.ImportRow(_importedRow);
            }

            return _filteredDataTable;
        }

        
        public static DataTable GetDataTable(string SqlQuery, bool IsStoredProc, params SqlParameter[] SqlParams)
        {
            DataTable _returnTable = new DataTable();
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                if (_connection.State != ConnectionState.Open) _connection.Open();
                SqlDataReader sqlDataReader = null;
                SqlCommand dataReaderCommand = GetSqlCommand(_connection);
                dataReaderCommand.CommandTimeout = 600;

                dataReaderCommand.Parameters.Clear();
                //JK: Increasing timeout due to some longer dashboard queries that may take > 30 secs on TEST...
                dataReaderCommand.CommandTimeout = 6000;
                dataReaderCommand.CommandText = SqlQuery;
                dataReaderCommand.Parameters.AddRange(SqlParams);

                if (IsStoredProc)
                    dataReaderCommand.CommandType = CommandType.StoredProcedure;
                else
                    dataReaderCommand.CommandType = CommandType.Text;
                sqlDataReader = dataReaderCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                    _returnTable.Load(sqlDataReader);

                dataReaderCommand.Dispose();
                sqlDataReader.Dispose();
            }
            
            return _returnTable;
        }

      
        public static DataTable GetFilteredDataTable(DataTable OriginalDataTable, string FilterCriteria)
        {
            DataView _filteredDataTable = OriginalDataTable.AsDataView();
            if (OriginalDataTable.Rows.Count > 0)
            {
                try
                {
                    _filteredDataTable.RowFilter = FilterCriteria;
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }

            return _filteredDataTable.ToTable();

        }

        public static string FlippedProgramName(string strProgramName)
        {
            string strFlippedName = strProgramName;

            try
            {
                if (strProgramName.IndexOf(",") >= 0)
                {
                    string[] str = strProgramName.Split(',');
                    if (str.Length > 1)
                        strFlippedName = str[1].Trim() + " " + str[0].Trim();
                }
            }
            catch
            {
                //Just return empty string...
            }
            return strFlippedName.Trim();
        }

        
        public static int GetUploadType(string strUploadTypeName)
        {
            int intUploadTypeID = 0;
            string strGet = "SELECT UploadTypeID FROM UploadType WHERE UploadTypeName = @UploadTypeName";

            DataSet projectTypeDataSet = DataProxy.GetDataSet(strGet, new SqlParameter("@UploadTypeName", strUploadTypeName), false);
            if (projectTypeDataSet.Tables.Count > 0)
            {
                if (projectTypeDataSet.Tables[0].Rows.Count > 0)
                {
                    intUploadTypeID = Convert.ToInt32(projectTypeDataSet.Tables[0].Rows[0]["UploadTypeID"].ToString());
                }
            }
          
            return intUploadTypeID;

        }
        
        public static DataTable GetUploadEventInfo(int intUploadEventID)
        {
            DataTable dtUploadEvent = null;
            string strCheck = "SELECT UploadEventID, UploadTypeID, UploadProgramID, Name, Email, DateSubmitted, UploadURL, UploadAttachment, Verified" +
                              " FROM UploadEvent WHERE UploadEventID=" + intUploadEventID;
            DataSet ds = GetDataSet(strCheck);
            dtUploadEvent = ds.Tables[0];

            return dtUploadEvent;
        }

        public static string GetTriggerName(int intTriggerNumber, int intLanguageID)
        {
            string strName = "";
            string strGet = "SELECT TriggerShortName FROM Triggers WHERE TriggerNumber = @TriggerNumber AND LanguageID = @LanguageID";

            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                     CreateSqlParameter("@TriggerNumber", intTriggerNumber, SqlDbType.Int),
                                     CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strName = ds.Tables[0].Rows[0]["TriggerShortName"].ToString();
                }
            }
            return strName;
        }

        public static string GetAdditionalComments(int intAssessmentID)
        {
            string strAddComments = "";
            string strGet = "SELECT AdditionalComments FROM Assessments WHERE AssessmentID = @AssessmentID";

            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                     CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strAddComments = ds.Tables[0].Rows[0]["AdditionalComments"].ToString();
                }
            }
            return strAddComments;
        }


        public static string GetTriggerDescription(int intTriggerNbr, int intLanguageID)
        {
            string strRtnValue = "";
            DataTable dt = null;

            string strCheck = "SELECT * FROM Triggers WHERE TriggerNumber = " + intTriggerNbr + " AND LanguageID = " + intLanguageID;
            DataSet ds = GetDataSet(strCheck);
            dt = ds.Tables[0];

            if (dt != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRtnValue = ds.Tables[0].Rows[0]["TriggerDescription"].ToString();
                }
            }
            return strRtnValue;
        }

        public static string GetAssessmentTriggers(int intAssessmentID, int intLanguageID, string strPageName, int intSpeciesID, int intCountryID, string strImageURL)
        {
            string strRtnValue = "";
            string strTriggerName = "";
            string strPostbackPage = "";
            string strHoverText = "";

            DataTable dt = null;

            string strCheck = "SELECT * " +
                              " FROM AssessmentTriggers WHERE AssessmentID = " + intAssessmentID;
            DataSet ds = GetDataSet(strCheck);
            dt = ds.Tables[0];

            string strCurrentLanguage = DataProxy.GetLanguage(intLanguageID);
            //<a ID="MyAnchor" OnServerClick="menuPersonTab_OnMenuItemClick" runat="server"> Click This </a>
            /*
                LinkButton lnk1 = new LinkButton();
                lnk1.Text = "Click me!";

                //lnk1.PostBackUrl = "SomeOtherPage.aspx";

                // Use the eventhandler to perform redirection, 
                // instead of the PostBackUrl to show it works.
                lnk1.Click += new EventHandler(lnk1_Click);

                // Add control to container:
                pnl1.Controls.Add(lnk1);             
             */
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (strPageName == "E") //EditAssessment.aspx
                        strPostbackPage = "EditAssessment.aspx?AssessmentID=" + intAssessmentID + "&CountryID=" + intCountryID + "&SpeciesID=" + intSpeciesID;
                    else
                        strPostbackPage = "ViewAssessment.aspx?AssessmentID=" + intAssessmentID + "&CountryID=" + intCountryID + "&SpeciesID=" + intSpeciesID;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr["Trigger1"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(1, intLanguageID);
                            strPostbackPage += "&Trigger1=Y";
                            strHoverText = GetTriggerDescription(1, intLanguageID);
                            //strTriggerName = String.Format("<a runat='server' href='" + strPostbackPage + "' OnServerClick='GetTriggerDescription(1, " + intLanguageID.ToString() + ")' id='lnk1' >" + strTriggerName + "</a>");
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT1' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT1' runat='server' src='" + strImageURL + "'/> </a>");
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger2"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(2, intLanguageID);
                            strHoverText = GetTriggerDescription(2, intLanguageID);
                            strPostbackPage += "&Trigger2=Y";
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT2' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT2' runat='server' src='" + strImageURL + "'/> </a>"); 
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger3"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(3, intLanguageID);
                            strHoverText = GetTriggerDescription(3, intLanguageID);
                            strPostbackPage += "&Trigger3=Y";
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT3' >" + strTriggerName +
                                                        "&nbsp;<img id='imgT3' runat='server' src='" + strImageURL + "'/> </a>");                        
                            //strTriggerName = String.Format("<a runat='server' href='" + strPostbackPage + "' OnServerClick='GetTriggerDescription(3, " + intLanguageID.ToString() + ")' id='lnk3' >" + strTriggerName + "</a>");
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger4"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(4, intLanguageID);
                            //strPostbackPage += "&TriggerID4=Y";
                            strHoverText = GetTriggerDescription(4, intLanguageID);
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT4' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT4' runat='server' src='" + strImageURL + "'/> </a>");
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger5"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(5, intLanguageID);
                            strPostbackPage += "&TriggerID5=Y";
                            strHoverText = GetTriggerDescription(5, intLanguageID);
                            //strTriggerName = String.Format("<a runat='server' href='" + strPostbackPage + "' OnServerClick='GetTriggerDescription(5, " + intLanguageID.ToString() + ")' id='lnk5' >" + strTriggerName + "</a>");
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT5' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT5' runat='server' src='" + strImageURL + "'/> </a>"); 
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger6"].ToString().ToUpper() == "TRUE")
                        {;
                            strTriggerName = GetTriggerName(6, intLanguageID);
                            strPostbackPage += "&TriggerID6=Y";
                            strHoverText = GetTriggerDescription(6, intLanguageID);
                            //strTriggerName = String.Format("<a runat='server' href='" + strPostbackPage + "' OnServerClick='GetTriggerDescription(6, " + intLanguageID.ToString() + ")' id='lnk6' >" + strTriggerName + "</a>");
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT6' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT6' runat='server' src='" + strImageURL + "'/> </a>");
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger7"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(7, intLanguageID);
                            strHoverText = GetTriggerDescription(7, intLanguageID);
                            strPostbackPage += "&TriggerID7=Y";
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT7' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT7' runat='server' src='" + strImageURL + "'/> </a>"); 
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger8"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(8, intLanguageID);
                            strHoverText = GetTriggerDescription(8, intLanguageID); 
                            strPostbackPage += "&TriggerID8=Y";
                            //strTriggerName = String.Format("<a runat='server' href='" + strPostbackPage + "' OnServerClick='GetTriggerDescription(8, " + intLanguageID.ToString() + ")' id='lnk8' >" + strTriggerName + "</a>");
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT8' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT8' runat='server' src='" + strImageURL + "'/> </a>"); 
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger9"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(9, intLanguageID);
                            strHoverText = GetTriggerDescription(9, intLanguageID);
                            strPostbackPage += "&TriggerID9=Y";
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT9' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT9' runat='server' src='" + strImageURL + "'/> </a>"); 
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                        if (dr["Trigger10"].ToString().ToUpper() == "TRUE")
                        {
                            strTriggerName = GetTriggerName(10, intLanguageID);
                            strHoverText = GetTriggerDescription(10, intLanguageID);
                            //strPostbackPage += "&TriggerID10=Y";
                            strHoverText = GetTriggerDescription(10, intLanguageID);
                            strTriggerName = String.Format("<a runat='server' title='" + strHoverText + "' id='lnkT10' >" + strTriggerName +
                                                          "&nbsp;<img id='imgT10' runat='server' src='" + strImageURL + "'/> </a>");
                            strRtnValue = strRtnValue + strTriggerName + ", ";
                        }
                    }
                }
            }

            if (strRtnValue != "")
            { 
                strRtnValue = strRtnValue.Substring(0, strRtnValue.Length - 2); 

            }
            else
                strRtnValue = DataProxy.LoadString("INCOMPLETERECTEXT", strCurrentLanguage);

            return strRtnValue;
        }

        public static string SetQ19Comments(string ddlValue, int intIndex, string strCurrentLanguage, string strCurrentComments)
        {
            string strRtnComments = "";
            string strAppendComments = DataProxy.LoadString("Q19LABEL", strCurrentLanguage);
            string value = ddlValue;
            
            if (intIndex == 3)
            {
                if (strCurrentComments == "" || strCurrentComments == strAppendComments)
                    strRtnComments = strAppendComments;
                else
                {
                    if (!strCurrentComments.Contains(strAppendComments))
                        strRtnComments = strAppendComments + " " + strCurrentComments;
                }
            }
            else
            {
                if (!strCurrentComments.Contains(strAppendComments))
                    strRtnComments = strCurrentComments;
                else
                {
                    if (strCurrentComments != strAppendComments)
                        strRtnComments = strCurrentComments.Substring(strAppendComments.Length + 1);
                    else
                        strRtnComments = "";
                }
            }
           
            return strRtnComments;
       
        }
        public static string SetQ20Comments(string ddlValue, int intIndex, string strCurrentLanguage, string strCurrentComments)
        {
            string strRtnComments = "";
            string strAppendComments = DataProxy.LoadString("Q20LABEL", strCurrentLanguage);
            string value = ddlValue;

            //If user selects "No" / "Unknown", add default text to comments
            if (value.Contains("NO") || intIndex == 3)
            {
                if (strCurrentComments == "" || strCurrentComments == strAppendComments)
                    strRtnComments = strAppendComments;
                else
                {
                    if (!strCurrentComments.Contains(strAppendComments))
                        strRtnComments = strAppendComments + " " + strCurrentComments;
                    else
                        strRtnComments = strCurrentComments;
                }
            }
            else
            {
                if (!strCurrentComments.Contains(strAppendComments))
                    strRtnComments = strCurrentComments;
                else
                {
                    if (strCurrentComments != strAppendComments)
                        strRtnComments = strCurrentComments.Substring(strAppendComments.Length + 1);
                    else
                        strRtnComments = "";
                }
            }
       
            return strRtnComments;
        }

        public static string GetQuestionSectionDescription(int intSectionNbr, int intLanguageID)
        {
            string strLanguage = GetLanguage(intLanguageID);

            string strDescription = "";
            string strGet = "SELECT Description " +
                           " FROM QuestionSections WHERE LanguageID = @LanguageID AND QuestionSectionNumber = @QuestionSectionNbr ";
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@QuestionSectionNbr", intSectionNbr, SqlDbType.Int),
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));

            DataTable dtGet = null;

            if (ds != null)
                dtGet = ds.Tables[0];

            if (dtGet != null)
            {
                strDescription = dtGet.Rows[0]["Description"].ToString();

                //if (strDescription.Length > 1)
                //{
                //    if (strLanguage.ToUpper() != "ENGLISH")
                //    {
                //        int intLength = strDescription.Length;
                //        if (intLength / 2 != 0)
                //            intLength++;

                //        string strTemp = strDescription.Substring(0, intLength / 2);
                //        int intLastSpace = strTemp.LastIndexOf(" ");
                //        if (intLastSpace > 0)
                //        {
                //            strDescription = strTemp.Substring(0, intLastSpace) + Environment.NewLine + strDescription.Substring(intLastSpace + 1);
                //        }
                //    }
                //}
            }

            return strDescription;
        }

       
        public static DataTable GetQuestionsForSection(int intSectionID, int intLanguageID)
        {
            DataTable dt = null;
            string strCheck = "SELECT QS.QuestionSectionNumber, Q.* " +
                              " FROM [Questions] Q INNER JOIN QuestionSections QS ON Q.QuestionSectionID = QS.QuestionSectionID " +
                              " WHERE QS.QuestionSectionNumber = @QuestionSectionID" +
                              " AND Q.LanguageID = @LanguageID " +
                              " ORDER BY Q.QuestionNumber  ";

            DataSet ds = DataProxy.GetDataSet(strCheck, false, new SqlParameter("@LanguageID", intLanguageID),
                                              new SqlParameter("@QuestionSectionID", intSectionID)) ;
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static string GetResponseIDForQuestion(int intQuestionNumber, int intAssessmentID, int intLanguageID)
        {
            string strRtnValue = "";

            string strCheck = "SELECT ResponseID, Q.QuestionID, Q.LanguageID, Q.QuestionNumber " +
                              " FROM Responses R INNER JOIN PossibleAnswers P ON R.PossibleAnswerID = P.PossibleAnswerID " +
                              " INNER JOIN Questions Q ON Q.QuestionID = P.QuestionID " +
                              " WHERE AssessmentID = @AssessmentID " +
                              " AND Q.QuestionNumber = @QuestionNumber " +
                              " AND P.LanguageID = @LanguageID";
        
            DataSet ds = DataProxy.GetDataSet(strCheck, false, new SqlParameter("@AssessmentID", intAssessmentID),
                                              new SqlParameter("@QuestionNumber", intQuestionNumber),
                                              new SqlParameter("@LanguageID", intLanguageID));
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                strRtnValue = dt.Rows[0]["ResponseID"].ToString();
            }

            return strRtnValue;
        }

        public static string GetSpeciesDistribution(int intSpeciesID)
        {
            string strRtnValue = "";

            string strCheck = "SELECT CountryName, SC.* " +
                              "FROM SpeciesCountries SC INNER JOIN Countries C ON SC.CountryID = C.CountryID " +
                              "WHERE SpeciesID = @SpeciesID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false, new SqlParameter("@SpeciesID", intSpeciesID));
            DataTable dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                strRtnValue += dt.Rows[i]["CountryName"].ToString() + ", "; 
            }

            if (strRtnValue.Length > 0)
                strRtnValue = strRtnValue.Substring(0, strRtnValue.Length - 2);

            return strRtnValue;
        }
        public static DataTable GetResponseForQuestion(int intQuestionNumber, int intAssessmentID, int intLanguageID)
        {
            DataTable dt = null;
            string strCheck = " SELECT DISTINCT convert(varchar,Q.QuestionNumber) + '_' + convert(varchar,PA.SortOrder) AS QS, " +
                              " dbo.GetAssessmentResponseCommentsForQuestion(@AssessmentID, Q.QuestionNumber) AS ResponseComments, PA.* " +
                              " FROM PossibleAnswers PA INNER JOIN Questions Q ON PA.QuestionID = Q.QuestionID " +
                              " INNER JOIN QuestionSections QSS ON QSS.QuestionSectionID = Q.QuestionSectionID " +
                              " WHERE PA.LanguageID = @LanguageID " +
                              " AND Q.QuestionNumber = @QuestionNumber " +
                              " AND convert(varchar,Q.QuestionNumber) + '_' + convert(varchar,PA.SortOrder) IN " +
                              "  (SELECT convert(varchar,Q.QuestionNumber) + '_' + convert(varchar,P.SortOrder) " +
                              "       FROM PossibleAnswers P INNER JOIN Questions Q ON P.QuestionID = Q.QuestionID" +
                              "       INNER JOIN Responses R ON R.PossibleAnswerID = P.PossibleAnswerID" +
                              "       WHERE AssessmentID = @AssessmentID)";

            DataSet ds = DataProxy.GetDataSet(strCheck, false, new SqlParameter("@AssessmentID", intAssessmentID),
                                              new SqlParameter("@QuestionNumber", intQuestionNumber),
                                              new SqlParameter("@LanguageID", intLanguageID));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static DataTable GetPossibleResponsesForQuestion(int intQuestionID, int intLanguageID)
        {
            DataTable dt = null;
            string strCheck = "SELECT Q.QuestionNumber, P.* " +
                              " FROM PossibleAnswers P INNER JOIN Questions Q ON Q.QuestionID = P.QuestionID  " +
                              " WHERE Q.QuestionNumber = @QuestionID " +
                              " AND P.LanguageID = @LanguageID " +
                              " ORDER BY SortOrder";

            DataSet ds = DataProxy.GetDataSet(strCheck, false, new SqlParameter("@LanguageID", intLanguageID),
                                              new SqlParameter("@QuestionID", intQuestionID));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }
        public static DataTable GetAssessmentInfo(int intAssessmentID, int intLanguageID)
        {
            DataTable dt = null;
            string strCheck = "SELECT S.EnglishCommonName,S.ScientificName,S.LocalCommonName, A.AdditionalComments, " +
                                " dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName," +
                                " dbo.GetSpeciesAssessmentDisplayName(S.SpeciesID) AS SpeciesAssessmentDisplayName," +
                                " U.UserName, CONVERT(VARCHAR(15), datecreated, 106) AS DateCreatedDate, " +
                                " GenusName, SpeciesName, SubSpeciesName, SpeciesOrder, SpeciesClass, SpeciesFamily, " +
                                " dbo.GetSpeciesDisplayName(S.SpeciesID) AS DisplayName, " +
                                " C.CountryName, Distribution, ProtectedHabitat, EDScore, isnull(S.RedListID,'') as RedListID," +
                                "dbo.GetGlobalRedListCategoryDisplayName(S.SpeciesID, 0, @LanguageID) AS GlobalRedListCategory, " +
                                "dbo.GetNationalRedListCategoryDisplayName(A.SpeciesID, A.CountryID, @LanguageID) AS NationalRedListCategory, " +
                                " A.AssessmentID, A.UserID, A.Priority, A.Completed, dbo.GetUserFullName(A.UserID) AS UserFullName " +  
                                " FROM Assessments A INNER JOIN Users U ON A.UserID = U.UserID" +
                                " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                                " INNER JOIN Countries C ON C.CountryID = A.CountryID " +
                                " WHERE AssessmentID = @AssessmentID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                                 CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int),
                                                 CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                 dt = ds.Tables[0];

            return dt;
        }

       
        public static DataTable GetSpeciesInfo(int intSpeciesID, int intCountryID, int intLanguageID)
        {
            DataTable dt = null;
            string strCheck = "SELECT S.EnglishCommonName,S.ScientificName,S.LocalCommonName, " +
                                 "dbo.GetSpeciesDisplayName(S.SpeciesID) AS SpeciesDisplayName," +
                                 "dbo.GetSpeciesAssessmentDisplayName(S.SpeciesID) AS SpeciesAssessmentDisplayName," +
                                 "GenusName, SpeciesName, SubSpeciesName, SpeciesOrder, SpeciesClass, SpeciesFamily, " +
                                 "dbo.GetSpeciesDisplayName(S.SpeciesID) AS DisplayName, " +
                                 "Distribution, ProtectedHabitat, EDScore, isnull(S.RedListID,'') as RedListID," +
                                 //Need to add languageID to these and show Spanish, etc. 
                                 "dbo.GetGlobalRedListCategoryDisplayName(SpeciesID, 0, @LanguageID) AS GlobalRedListCategory, " +
                                 "dbo.GetNationalRedListCategoryDisplayName(SpeciesID, @CountryID, @LanguageID) AS NationalRedListCategory" +
                                " FROM Species S " +
                                " WHERE SpeciesID = @SpeciesID";
            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }
        public static DataTable GetTriggers(int intAssessmentID)
        {
            DataTable dt = null;
            string strCheck = "SELECT * FROM AssessmentTriggers " +
                                " WHERE AssessmentID = @AssessmentID";
            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static DataTable GetRecommendationList(int intLanguageID, string strTriggerShortName, string strSortOption)
        {
            DataTable dt = null;
            string strCheck = " usp_GetNationalRecommendations @LanguageID, @TriggerShortNameParam, @SortOption";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int),
                                        CreateSqlParameter("@TriggerShortNameParam", strTriggerShortName, SqlDbType.VarChar),
                                        CreateSqlParameter("@SortOption", strSortOption, SqlDbType.VarChar));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];
            
            return dt;
        }

        public static DataTable GetSpeciesRecommendRescue(int intLanguageID, bool bFoundersAvail, bool bHabitatReintroAvail, 
                                                            bool bTaxonAnalysisComplete, string strSortOption)
        {
            DataTable dt = null;
            string strCheck = " usp_GetSpeciesRecommendedRescue @LanguageID, @FoundersAvail, @HabitatReintroAvail, @TaxonAnalysisComplete, @SortOption";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int),
                                        CreateSqlParameter("@FoundersAvail", bFoundersAvail, SqlDbType.Bit),
                                        CreateSqlParameter("@HabitatReintroAvail", bHabitatReintroAvail, SqlDbType.Bit),
                                        CreateSqlParameter("@TaxonAnalysisComplete", bTaxonAnalysisComplete, SqlDbType.Bit),
                                        CreateSqlParameter("@SortOption", strSortOption, SqlDbType.VarChar));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }
        public static DataTable GetTriggerCounts(int intLanguageID, int intCountryID)
        {
            DataTable dt = null;
            string strCheck = " usp_GetTriggerCounts @LanguageID, @CountryID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static string GetConsolidatedRecommendations(int intLanguageID, int intSpeciesID, int intCountryID)
        {
            string strValues = "";
           
            DataTable dt = null;
            string strCheck = " usp_GetConsolidatedRecommendations @LanguageID, @SpeciesID, @CountryID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int),
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    double intPct = 0;
                    if (dr["PctTotal"] != null)
                        intPct = Convert.ToDouble(dr["PctTotal"]);

                    strValues += dr["TriggerShortName"].ToString() + " (n=" + dr["Total"].ToString() + ", " + intPct.ToString("#.##") + "%), ";
                   
                }
            }
            return strValues;
        }

        public static DataTable GetTriggerInfo(int intAssessmentID)
        {
            DataTable dt = null;
            string strCheck = "SELECT * " +
                              " FROM AssessmentTriggers" +
                              " WHERE AssessmentID = @AssessmentID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static DataTable GetAssessmentQAGrid(int intAssessmentID, int intCurrentLanguageID)
        {
            DataTable dt = null;
            string strCheck = "SELECT DISTINCT Q.QuestionID, QuestionNumber, QuestionShortName, QuestionText, " +
                              " dbo.GetAssessmentResponseIDForQuestion(@AssessmentID,Q.QuestionNumber) as ResponseID," +
                              " dbo.GetAssessmentResponseForQuestion(@AssessmentID,Q.QuestionNumber, @LanguageID) as ResponseText," +
                              " dbo.GetAssessmentResponseCommentsForQuestion(@AssessmentID,Q.QuestionNumber) as ResponseComments" +
                              " FROM Questions Q " +
                              " WHERE LanguageID = @LanguageID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int),
                                        CreateSqlParameter("@LanguageID", intCurrentLanguageID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public static DataTable GetConsolidatedQAGrid(int intCurrentLanguageID, int intSpeciesID, int intCountryID)
        {
            DataTable dt = null;
            string strCheck = "SELECT DISTINCT Q.QuestionID, QuestionNumber, QuestionShortName, QuestionText, " +
                              " dbo.GetConsolidatedResponsesForAssessment(@SpeciesID, @CountryID, QuestionNumber) AS ResponseComments, " +
                              " dbo.GetConsolidatedResponsesPct(@SpeciesID, @CountryID, QuestionNumber, @LanguageID) AS ResponseText  " +  
                              " FROM Questions Q " +
                              " WHERE LanguageID = @LanguageID";

            DataSet ds = DataProxy.GetDataSet(strCheck, false,
                                        CreateSqlParameter("@LanguageID", intCurrentLanguageID, SqlDbType.Int),
                                        CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                                        CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }
        public static bool SendApprovedEmail(int intUserID)
        {
            bool bSuccess = false;
            string strEmail = "";
            string strLocalHostEmail = Settings.LocalHostEmail;

            string strFromEmail = Settings.ContactEmail;
            string strToEmail = Settings.ContactEmail;
            string strSubject = "AArk Assessor Request";
          
            string strFirstName = "";

            DataTable dt = GetAllUserInfo(intUserID);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                { 
                    strFirstName = dt.Rows[0]["FirstName"].ToString();
                    strEmail = dt.Rows[0]["Email"].ToString();
                }
            }

            string strEmailBodyLocal = "<p>Dear " + strFirstName + 
                                    "</p><br/> Your request to become an assessor for the amphibian Conservation Needs Assessment process has been approved. You are now able to login to the system at <a>www.ConservationNeeds.org</a> and begin to add species assessments." +
                                    "<br/><br/> We would like to suggest that before you begin to add assessments that you take a little time to read the articles from the Assessment Process and Assessors' Guide sections in the Help pages.<br/><br/>" +
                                    "If you have any questions please feel free to contact your national Amphibian Specialist Group Chair (http://www.conservationneeds.org/ApprovedUsers.aspx) , or contact Amphibian Ark staff at " +
                                    "ConservationNeeds@AmphibianArk.org.<br/><br/> Thank you!<br/><br/>Amphibian AArk Staff";
            
            string strEmailBody = "<p>Dear " + strFirstName + 
                                    "</p><br/> Your request to become an assessor for the amphibian Conservation Needs Assessment process has been approved. You are now able to login to the system at <a>www.ConservationNeeds.org</a> and begin to add species assessments." +
                                    "<br/><br/> We would like to suggest that before you begin to add assessments that you take a little time to read the articles from the Assessment Process and Assessors' Guide sections in the Help pages.<br/><br/>" +
                                    "If you have any questions please feel free to contact your national Amphibian Specialist Group Chair (www.conservationneeds.org/ApprovedUsers.aspx) , or contact Amphibian Ark staff at " +
                                    "ConservationNeeds@AmphibianArk.org.<br/><br/> Thank you!<br/><br/>Amphibian AArk Staff";

            if (strLocalHostEmail == "YES")
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFromEmail, strToEmail, strSubject, strEmailBodyLocal);
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                mail.IsBodyHtml = true;
                client.Host = "localhost";
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = HttpContext.Current.Server.MapPath("~/MessagesSent/");
                client.Send(mail);
                bSuccess = true;
            }
            else
            {
                //mail.IsBodyHtml = true;
                //smtp.UseDefaultCredentials = true;
                
                SendEmail(strFromEmail, strToEmail, strSubject, strEmailBody, true);
                bSuccess = true;
                SendEmail(strFromEmail, strEmail, strSubject, strEmailBody, true); // Try sending to user email address too.
                bSuccess = true;
            }

            return bSuccess;
        }

        public static bool SendEmail(string strFromEmail, string strToEmail, string strSubject, string strEmailBody, bool bHTML)
        {
            bool bSuccess = false;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFromEmail, strToEmail, strSubject, strEmailBody);
            SmtpClient smtp = new SmtpClient("dedrelay.secureserver.net");

            if (bHTML)
                mail.IsBodyHtml = true;

            smtp.Send(mail);
            bSuccess = true;
            return bSuccess;
        }

        public static bool SendPassword(int intUserID, string strSendParam)
        {
            bool bSuccess = false;
            string strEmail = "";
            string strLocalHostEmail = Settings.LocalHostEmail;

            string strFromEmail = Settings.ContactEmail;
            string strToEmail = Settings.ContactEmail;
            string strSubject = "AARK Recent Request";

            string strFirstName = "";
            string strLastName = "";
            string strPwd = "";
            string strUserName = "";

            DataTable dt = GetAllUserInfo(intUserID);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strFirstName = dt.Rows[0]["FirstName"].ToString();
                    strFirstName = dt.Rows[0]["FirstName"].ToString();
                    strLastName = dt.Rows[0]["LastName"].ToString();
                    strEmail = dt.Rows[0]["Email"].ToString();
                    strPwd = dt.Rows[0]["Password"].ToString();
                    strUserName = dt.Rows[0]["UserName"].ToString();
                }
            }

            string strEmailBody = "<p>Dear " + strFirstName + 
                                    "</p><br> A request was recently made to recover a forgotten password for the Conservation Needs Assessment program. " + 
                                    "Your password is: " + strPwd +  
                                    "<br><br>If you did not request for your password to be sent to you, please delete this email. " +
                                    "<br><br> Thank you!<br><br>Amphibian AArk Staff";


            string strEmailBodyAdmin = "User Information<br>Name: " + strFirstName + " " + strLastName + "<br>Username: " + strUserName + "<br>Email: " + strEmail + 
                                       "<br><br>" + strEmailBody;

            if (strLocalHostEmail == "YES")
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFromEmail, strToEmail, strSubject, strEmailBody);
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                mail.IsBodyHtml = true;
                client.Host = "localhost";
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = HttpContext.Current.Server.MapPath("~/MessagesSent/");
                client.Send(mail);
                bSuccess = true;
            }
            else
            {
                if (strSendParam == "ADMIN")
                {
                    //Send one to Kevin / Admin email set in config
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFromEmail, strToEmail, strSubject, strEmailBodyAdmin);
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("dedrelay.secureserver.net");
                    //smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = false;
                    //smtp.Port = 25;
                    smtp.Send(mail);
                }
                else
                {
                    //Send one to user 
                    System.Net.Mail.MailMessage mailUser = new System.Net.Mail.MailMessage(strFromEmail, strEmail, strSubject, strEmailBody);
                    mailUser.IsBodyHtml = true;
                    SmtpClient smtpUser = new SmtpClient("dedrelay.secureserver.net");
                    smtpUser.EnableSsl = false;
                    //smtpUser.Port = 25;
                    smtpUser.Send(mailUser);
                    bSuccess = true;
                }
            }

            return bSuccess;
        }
        
        public static DataTable GetRecentAssessments()
        {
            DataTable dt = null;
            string strCheck = "SELECT TOP 12 A.*, S.EnglishCommonName,S.ScientificName,S.LocalCommonName, " +
                              " U.UserName,isnull(FirstName,'') + ' ' + isnull(LastName,'') as FullName," +
                              " CONVERT(VARCHAR(15), DateCreated, 106) AS DateCreatedDate, " +
                              "  GenusName, SpeciesName, SubSpeciesName, " +
                              "  dbo.GetSpeciesDisplayName(S.SpeciesID) AS DisplayName, " + 
						        // - causing error Format(datecreated,'MMM dd yyyy') as DateCreatedDate," +
                              " C.CountryName  " +
                              " FROM Assessments A INNER JOIN Users U ON A.UserID = U.UserID " +
                              " INNER JOIN Species S ON S.SpeciesID = A.SpeciesID " +
                              " INNER JOIN Countries C ON C.CountryID = A.CountryID " +
                              " WHERE A.Approved = 1 " +
                              " ORDER BY DateCreated DESC";
            DataSet ds = GetDataSet(strCheck);
            dt = ds.Tables[0];

            return dt;
        }

        public static string GetUploadProgramName(int intUploadProgramID)
        {
            string strGet = "SELECT UploadProgramName FROM UploadProgram WHERE UploadProgramID = @UploadProgramID";
            string strUploadProgramName = "";

            DataSet programDataSet = DataProxy.GetDataSet(strGet, new SqlParameter("@UploadProgramID", intUploadProgramID), false);
            if (programDataSet.Tables.Count > 0)
            {
                if (programDataSet.Tables[0].Rows.Count > 0)
                {
                    strUploadProgramName = programDataSet.Tables[0].Rows[0]["UploadProgramName"].ToString();
                }
            }
            return strUploadProgramName;
        }

        public static int GetUploadProgram(string strUploadProgramName)
        {
            int intUploadProgramID = 0;
            string strGet = "SELECT UploadProgramID FROM UploadProgram WHERE UploadProgramName = @UploadProgramName";

            DataSet programDataSet = DataProxy.GetDataSet(strGet, new SqlParameter("@UploadProgramName", strUploadProgramName), false);
            if (programDataSet.Tables.Count > 0)
            {
                if (programDataSet.Tables[0].Rows.Count > 0)
                {
                    intUploadProgramID = Convert.ToInt32(programDataSet.Tables[0].Rows[0]["UploadProgramID"].ToString());
                }
            }

            return intUploadProgramID;

        }


        public static string CreateLiteralFAQDisplayOnly(string strField1, string strField2, string strQURL, string strAURL, int intQID, int intOldQID)
        {
            string strLiteral = "";

            if (strQURL == strAURL) strQURL = "http://"; // don't show it both places

            string strQIDLink = "&nbsp;&nbsp;<a title='' href='/FAQAnswerForm.aspx?QID=" + intQID.ToString() +
                                "' target='_self'><font title='' size='3'>Submit Answer</font></a>   ";

            string strQuestion = "<br/><br/><font color='black'><b>Q:>&nbsp;&nbsp;" + strField1;
            if (!strQURL.EndsWith("http://")) strQuestion += "     URL: " + strQURL;
            strQuestion += "</b></font><br/><br/>";

            string strAnswer = "<font color='black'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A:>&nbsp;&nbsp;" + strField2;
            if (!strAURL.EndsWith("http://")) strAnswer += "     URL: " + strAURL;
            strAnswer += "</font><br/>";

            if (intOldQID != intQID)
            {
                if (strField2.Trim() != "" && strAURL.Trim() != "")
                    strLiteral = strQuestion + strAnswer;
                else
                    strLiteral = strQuestion;
            }
            else
                strLiteral = strAnswer;

            return strLiteral;
        }

        public static string CreateLiteralFAQ(string strField1, string strField2, string strQURL, string strAURL, int intQID, int intOldQID)
        {
            string strLiteral = "";

            if (strQURL == strAURL) strQURL = "http://"; // don't show it both places

            string strQIDLink = "&nbsp;&nbsp;<a title='' href='/FAQAnswerForm.aspx?QID=" + intQID.ToString() +
                                "' target='_self'><font title='' size='3'>Submit Answer</font></a>   ";

            string strQuestion = "<br/><br/><font color='black'><b>Q:>&nbsp;&nbsp;" + strField1;
            if (!strQURL.EndsWith("http://")) strQuestion += "     URL: <a href='" + strQURL + "' target='_blank'><font size='3'>" + strQURL;
            strQuestion += strQIDLink + "</b></font><br/><br/>";

            string strAnswer = "<font color='black'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A:>&nbsp;&nbsp;" + strField2;
            if (!strAURL.EndsWith("http://")) strAnswer += "     URL: <a href='" + strAURL + "' target='_blank'><font size='3'>" + strAURL;
            strAnswer += "</font><br/>";

            if (intOldQID != intQID)
            {
                if (strField2.Trim() != "" && strAURL.Trim() != "")
                    strLiteral = strQuestion + strAnswer;
                else
                    strLiteral = strQuestion;
            }
            else
                strLiteral = strAnswer;

            return strLiteral;
        }

        public static string CreateLiteralExpertForum(string strField1, string strField2, string strQURL, string strAURL, int intQID, int intOldQID)
        {
            string strLiteral = "";

            if (strQURL == strAURL) strQURL = "http://"; // don't show it both places

            string strQIDLink = "<p><a title='' href='/ExpertForumAnswerSubmit.aspx?QID=" + intQID.ToString() +
                                "' target='_self'><font title='' size='3'>Submit Answer</font></a></p>   ";

            string strQuestion = "<br/><br/><font color='black'><b>Q:>&nbsp;&nbsp;&nbsp;" + strField1;
            if (!strQURL.EndsWith("http://")) strQuestion += "     URL: <a href='" + strQURL + "' target='_blank'><font size='3'>" + strQURL;
            strQuestion += strQIDLink + "</b></font><br/><br/>";

            string strAnswer = "<font color='black'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A:>&nbsp;&nbsp;" + strField2;
            if (!strAURL.EndsWith("http://")) strAnswer += "     URL: <a href='" + strAURL + "' target='_blank'><font size='3'>" + strAURL;
            strAnswer += "</font><br/>";

            if (intOldQID != intQID)
            {
                if (strField2.Trim() != "" && strAURL.Trim() != "")
                    strLiteral = strQuestion + strAnswer;
                else
                    strLiteral = strQuestion;
            }
            else
                strLiteral = strAnswer;

            return strLiteral;
        }

        public static string CreateLiteralReference(string strField1, string strField2, string strURL)
        {
            string strLiteral = "";

            strLiteral = "<br><li><font size=3 color='black'>" + strField1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + strField2;
            if (!strURL.EndsWith("http://")) strLiteral += "     URL: <a href='" + strURL + "' target='_blank'><font size='3'>" + strURL;
            strLiteral += "</font></a></li><br/>";

            return strLiteral;
        }

        public static string CreateLiteralApplication(string strField1, string strField2, string strURL)
        {
            string strLiteral = "";
            //string strButton = "<asp:Button ID='btnOpenFile' runat='server' Text='Open File' onclick='OpenFile('" + intEventID +"')' />";

            strLiteral = "<br/><li><font size=3 color='black'>" + strField1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + strField2;
            if (!strURL.EndsWith("http://")) strLiteral += "     URL: <a href=" + strURL + " target='_blank'><font size='3'>" + strURL;
            strLiteral += "</font></a></li>&nbsp;";

            //if (bAttachmentUploaded)
            //  strLiteral = strLiteral +  strButton + "</font><br/><br/>";

            return strLiteral;
        }

        public static string CreateLiteralReferenceDisplayOnly(string strField1, string strField2, string strURL)
        {
            string strLiteral = "";

            strLiteral = "<br/><font size=3 color='black'>" + strField1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + strField2;
            if (!strURL.EndsWith("http://")) strLiteral += "     URL: <a href='" + strURL + "' target='_blank'><font size='3'>" + strURL;
            strLiteral += "</font>&nbsp;";

            return strLiteral;
        }

        public static string CreateLiteralApplicationDisplayOnly(string strField1, string strField2, string strURL)
        {
            string strLiteral = "";

            strLiteral = "<br/><font size=3 color='black'>" + strField1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + strField2;
            if (!strURL.EndsWith("http://")) strLiteral += "     URL: <a href='" + strURL + "' target='_blank'><font size='3'>" + strURL;
            strLiteral += "</font>&nbsp;";

            return strLiteral;
        }

        public static string CreateLiteralRecentAdditions(string strName, string strUser, string strDate, string strCountry, string strURL)
        {
            string strLiteral = "";

            strLiteral += " <a href='" + strURL + "' runat='server'>";
            strLiteral += " <font size=2 color='black'><i> " + strName + "</i></font></a>,";
            strLiteral += " <font size=2 color='brown'>" + strCountry + "</font>,"; 
            strLiteral += " <font size=2 color='black'>" + strUser.PadLeft(30-strUser.Length) + ",&nbsp;</font>";
            strLiteral += " <font size=2 color='black'>" + strDate + "&nbsp;</font><br>";    

            return strLiteral;
        }

        public static byte[] GetFileFromDB(int intUploadEventID)
        {
         
             byte[] file = null;

             /*string strSQL = "SELECT UploadAttachment, UploadFileName FROM UploadEvent WHERE UploadEventID = @UploadEventID";
             SqlParameter[] param = new SqlParameter[] 
             {
                 new SqlParameter ("@UploadEventID", intUploadEventID)
             };*/
             string _connString = Settings.AARKConnectionString;
             //SqlDataReader reader = DataProxy.ExecuteDataReader(strSQL, param, false);
             using (var conn = new SqlConnection(Settings.AARKConnectionString))
             using (var cmd = conn.CreateCommand())
             {
                 conn.Open();
                 cmd.CommandText = "SELECT UploadAttachment, UploadFileName FROM UploadEvent WHERE UploadEventID = " + intUploadEventID;
                 using (var reader = cmd.ExecuteReader())
                 {
                     while (reader.Read())
                     {
                         if (reader["UploadAttachment"] != DBNull.Value)
                            file = (byte[])reader["UploadAttachment"];
                     }
                 }
             }
            /*
            if (reader.Read())
             {
                
             }*/

            return file;
     }
        public static string GetFileName(int intUploadEventID)
        {

            string strFileName = "";

            string strCheck = "SELECT UploadFileName FROM [UploadEvent] WHERE UploadEventID=" + intUploadEventID;
            DataSet ds = GetDataSet(strCheck);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                strFileName = dt.Rows[0]["UploadFileName"].ToString();
            }

            return strFileName;

        }

        public static string ParseXMLImage(string strImageURL)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string strParsedURL = "";

            XmlDocument doc1 = new XmlDocument();
            doc1.Load(strImageURL);
            XmlElement root = doc1.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("thumb_url");

            foreach (XmlNode node in nodes)
            {
                strParsedURL = node.InnerText;
            }

            return strParsedURL;
        }

        public static string ParseXMLCopy(string strImageURL)
        {
            string strParsedURL = "";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(strImageURL);
            XmlElement root = doc1.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("copyright");

            foreach (XmlNode node in nodes)
            {
                strParsedURL = node.InnerText;
            }

            return strParsedURL;
        }

        public static string ParseXMLCount(string strImageURL)
        {
            string strParsedURL = "";

            XmlDocument doc1 = new XmlDocument();
            doc1.Load(strImageURL);
            XmlElement root = doc1.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("count");

            foreach (XmlNode node in nodes)
            {
                strParsedURL = node.InnerText;
            }

            return strParsedURL;
        }

        public static string GetSpeciesName(int intSpeciesID)
        {
            string strName = "";

            string strCheck = "SELECT SpeciesName FROM [Species] WHERE SpeciesID=" + intSpeciesID;
            DataSet ds = GetDataSet(strCheck);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                strName = dt.Rows[0]["SpeciesName"].ToString();
            }

            return strName;
        }

        public static string GetGenusName(int intSpeciesID)
        {
            string strGenusName = "";

            string strCheck = "SELECT GenusName FROM [Species] WHERE SpeciesID=" + intSpeciesID;
            DataSet ds = GetDataSet(strCheck);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                strGenusName = dt.Rows[0]["GenusName"].ToString();
            }

            return strGenusName;
        }

        public static string GetArkiveName(string strGenusName, string strSpeciesName)
        {
            return strGenusName + "%20" + strSpeciesName;
        }

        public static int GetUploadEventID(int intQuestionID)
        {
            int intEventID = 0;

            string strGet = "SELECT QuestionID, UploadEventID FROM Question WHERE QuestionID = @QuestionID ";
            DataSet QuestionDataSet = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@QuestionID", intQuestionID, SqlDbType.Int));

            DataTable dtGet = null;
            
            if (QuestionDataSet != null)
                dtGet = QuestionDataSet.Tables[0];

            if (dtGet != null)
                intEventID = Convert.ToInt32(dtGet.Rows[0]["UploadEventID"]);

            return intEventID;
        }

        public static DataTable GetLanguages()
        {
            DataTable dtLanguages = null;

            string strGet = "SELECT * FROM Languages ";
            DataSet ds = GetDataSet(strGet);
            dtLanguages = ds.Tables[0];

            return dtLanguages;
        }

       public static DataTable GetCountries(bool bAssessments)
       {
            DataTable dt = null;

            string strGet = "SELECT * FROM Countries ";

            if (bAssessments)
                strGet += " WHERE CountryID IN (SELECT CountryID FROM Assessments WHERE Approved = 1)";

            strGet += " ORDER BY CountryName";
         
            DataSet ds = GetDataSet(strGet);
            dt = ds.Tables[0];

            return dt;
        }

        public static DataTable GetFilteredCountries()
        {
           DataTable dt = null;

           string strGet = "SELECT * FROM Countries " + 
                           "WHERE CountryID IN (SELECT CountryID FROM Assessments WHERE Approved = 0 and Completed = 1) ";

           strGet += " ORDER BY CountryName";

           DataSet ds = GetDataSet(strGet);
           dt = ds.Tables[0];

           return dt;
        }
        public static DataTable GetTriggersList(int intLanguageID)
        {
            DataTable dt = null;

            string strGet = "SELECT * from [Triggers] WHERE LanguageID = @LanguageID";
            strGet += " ORDER BY TriggerNumber";
         
            DataSet ds = DataProxy.GetDataSet(strGet, false,
                                               CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int));
            dt = ds.Tables[0];

            return dt;
        }
        public static DataTable GetSpeciesCountries(int intSpeciesID)
        {
            DataTable dtGet = null;

            string strGet = "SELECT C.* " +
                            "FROM SpeciesCountries SC " +
                            "INNER JOIN Countries C ON C.CountryID = SC.CountryID " +
                            "WHERE SC.SpeciesID = @SpeciesID";

            DataSet refListDataSet = DataProxy.GetDataSet(strGet, false,
                                    CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int));
            dtGet = refListDataSet.Tables[0];

            return dtGet;

        }

        public static DataTable GetQAForProgram(int intUploadProgramID, int intUploadTypeID)
        {
            DataTable dtGet = null;

            string strGet = "SELECT ISNULL(ANSUE.DateSubmitted, UE.DateSubmitted) AS DateSubmitted, UP.UploadProgramID, UP.UploadProgramName,UT.UploadTypeID, UT.UploadTypeName, " +
                            " UE.UploadEventID as QUploadEventID, UE.UploadFileName, UE.UploadAttachment, UE.UploadURL as UploadURL, " +
                            " Q.*, Q.Description as QuestionDescription," +
                            " A.AnswerID, A.Description as AnswerDescription," +
                            " ANSUE.UploadEventID as AUploadEventID, ANSUE.UploadURL as AnswerUploadURL," +
                            " isnull(Q.Description,'') + '  ' + isnull(A.Description,'') + ' URL: ' + isnull(UE.UploadURL,'') as QuestionDescriptionConcat " +
                            " FROM Question Q LEFT JOIN Answer A ON Q.QuestionID = A.QuestionID" +
                            " INNER JOIN UploadEvent UE ON UE.UploadEventID = Q.UploadEventID" +
                            " INNER JOIN UploadProgram UP ON UP.UploadProgramID = UE.UploadProgramID" +
                            " INNER JOIN UploadType UT ON UT.UploadTypeID = UE.UploadTypeID" +
                            " LEFT JOIN UploadEvent ANSUE ON ANSUE.UploadEventID = A.UploadEventID " +
                            " WHERE UP.UploadProgramID = @UploadProgramID" + 
                            " AND UT.UploadTypeID = @UploadTypeID" +
                            " AND (UE.Verified = 1 OR UE.Verified IS NULL) " + 
                            " AND (ANSUE.Verified = 1 OR ANSUE.Verified IS NULL) " +
                            " ORDER BY UE.UploadEventID DESC, Q.QuestionID DESC, A.AnswerID ASC";

                DataSet refListDataSet = DataProxy.GetDataSet(strGet, false,
                                        CreateSqlParameter("@UploadTypeID", intUploadTypeID, SqlDbType.Int),
                                        CreateSqlParameter("@UploadProgramID", intUploadProgramID, SqlDbType.Int));
                dtGet = refListDataSet.Tables[0];
         
            return dtGet;

        }

        public static DataTable GetQAForType(int intUploadTypeID)
        {
            DataTable dtGet = null;

            string strGet = "SELECT ISNULL(ANSUE.DateSubmitted, UE.DateSubmitted) AS DateSubmitted, UP.UploadProgramID, UP.UploadProgramName,UT.UploadTypeID, UT.UploadTypeName, " +
                            " UE.UploadEventID as QUploadEventID, UE.UploadFileName, UE.UploadAttachment, UE.UploadURL as UploadURL, " +
                            " Q.*, Q.Description as QuestionDescription," +
                            " A.AnswerID, A.Description as AnswerDescription," +
                            " ANSUE.UploadEventID as AUploadEventID, ANSUE.UploadURL as AnswerUploadURL," +
                            " isnull(Q.Description,'') + '  ' + isnull(A.Description,'') + ' URL: ' + isnull(UE.UploadURL,'') as QuestionDescriptionConcat " +
                            " FROM Question Q LEFT JOIN Answer A ON Q.QuestionID = A.QuestionID" +
                            " INNER JOIN UploadEvent UE ON UE.UploadEventID = Q.UploadEventID" +
                            " INNER JOIN UploadProgram UP ON UP.UploadProgramID = UE.UploadProgramID" +
                            " INNER JOIN UploadType UT ON UT.UploadTypeID = UE.UploadTypeID" +
                            " LEFT JOIN UploadEvent ANSUE ON ANSUE.UploadEventID = A.UploadEventID " +
                            " WHERE UT.UploadTypeID = @UploadTypeID" +
                           " AND (UE.Verified = 1 OR UE.Verified IS NULL) " +
                           " AND (ANSUE.Verified = 1 OR ANSUE.Verified IS NULL) " +
                            //Show in descending order (most recent) by Event / Question, but show Answers in ASC (earliest first) order:
                            " ORDER BY UE.UploadEventID DESC, Q.QuestionID DESC, A.AnswerID ASC";

            DataSet refListDataSet = DataProxy.GetDataSet(strGet, false,
                                    CreateSqlParameter("@UploadTypeID", intUploadTypeID, SqlDbType.Int));
            dtGet = refListDataSet.Tables[0];

            return dtGet;

        }
     
        public static string GetQuestionDescription(int intQuestionID)
        {
            string strQuestionDescription = "";

            string strCheck = "SELECT Description FROM [Question] WHERE QuestionID=" + intQuestionID;
            DataSet ds = GetDataSet(strCheck);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                strQuestionDescription = dt.Rows[0]["Description"].ToString();
            }

            return strQuestionDescription;
        }

        public static bool ValueExists(string strValue, string strColumnName, string strTableName, bool bIsText)
        {
            bool bExists = false;

            if (bIsText)
                strValue = "'" + strValue + "'";

            string strSQL = "SELECT COUNT(1) AS ValueID FROM " + strTableName + " WHERE " + strColumnName + " = " + strValue;
            DataSet ds = GetDataSet(strSQL);
            DataTable dt = ds.Tables[0];
            if (dt.Rows[0]["ValueID"].ToString() != "0")
                bExists = true;

            return bExists;
        }
       
        public static int GetRoleIdbyRoleName(string roleName)
        {
            int _roleId = 0;

            string _getRoleIdbyRoleNameQuery = "Select RoleId, RoleName from Roles where RoleName=@RoleName";

            DataTable _roleDataTable = DataProxy.GetDataTable(_getRoleIdbyRoleNameQuery, false, new SqlParameter("@RoleName", roleName));

            _roleId = (_roleDataTable.Rows.Count > 0) ? int.Parse(_roleDataTable.Rows[0]["RoleId"].ToString()) : 0;

            return _roleId;
        }

     
        public static int InsertEvent(int intUploadTypeID, int intUploadProgramID, string strName, string strEmail, string strUploadURL, string strUploadFileName, byte[] strUploadAttachment)
        {
            int intNewEventID = 0;
            string strURLPrefix = "http://";
           
            if (strUploadURL.IndexOf(strURLPrefix) < 0)
                strUploadURL = strURLPrefix + strUploadURL;

            string strUpdateQuery = "INSERT INTO UploadEvent (UploadTypeID, UploadProgramID, Name, Email, DateSubmitted, UploadURL, UploadFileName, UploadAttachment) " +
                                    "VALUES(@UploadTypeID, @UploadProgramID, @Name, @Email, @DateSubmitted, @UploadURL, @UploadFileName, @UploadAttachment); " +
                                    "SELECT CAST(scope_identity() as int)";

            string strDateTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@UploadTypeID", intUploadTypeID, SqlDbType.Int),
                CreateSqlParameter("@UploadProgramID", intUploadProgramID, SqlDbType.Int),
                CreateSqlParameter("@Name", strName, SqlDbType.VarChar),
                CreateSqlParameter("@Email", strEmail, SqlDbType.VarChar),
                CreateSqlParameter("@DateSubmitted", strDateTime, SqlDbType.VarChar),
                CreateSqlParameter("@UploadURL", strUploadURL, SqlDbType.VarChar),
                CreateSqlParameter("@UploadFileName", strUploadFileName, SqlDbType.VarChar),
                CreateSqlParameter("@UploadAttachment", strUploadAttachment, SqlDbType.Binary));

            if (_returnValue != null)
            {
                intNewEventID = (int)_returnValue;
                
            }
            return intNewEventID;
        }

        public static int InsertEvent(int intUploadTypeID, int intUploadProgramID, string strName, string strEmail, string strUploadURL)
        {
            int intNewEventID = 0;
            string strURLPrefix = "http://";
            
            if (strUploadURL.IndexOf(strURLPrefix) < 0)
                strUploadURL = strURLPrefix + strUploadURL;

            string strUpdateQuery = "INSERT INTO UploadEvent (UploadTypeID, UploadProgramID, Name, Email, DateSubmitted, UploadURL) " +
                                    "VALUES(@UploadTypeID, @UploadProgramID, @Name, @Email, @DateSubmitted, @UploadURL); " +
                                    "SELECT CAST(scope_identity() as int)";

            string strDateTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@UploadTypeID", intUploadTypeID, SqlDbType.Int),
                CreateSqlParameter("@UploadProgramID", intUploadProgramID, SqlDbType.Int),
                CreateSqlParameter("@Name", strName, SqlDbType.VarChar),
                CreateSqlParameter("@Email", strEmail, SqlDbType.VarChar),
                CreateSqlParameter("@DateSubmitted", strDateTime, SqlDbType.VarChar),
                CreateSqlParameter("@UploadURL", strUploadURL, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                intNewEventID = (int)_returnValue;

            }
            return intNewEventID;
        }

        public static int InsertQuestion(int intEventID, string strDescription)
        {
            int intNewQuestionID = 0;

            string strUpdateQuery = "INSERT INTO Question (UploadEventID, Description) " +
                                    "VALUES(@UploadEventID, @Description); " +
                                    "SELECT CAST(scope_identity() as int)";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@UploadEventID", intEventID, SqlDbType.Int),
                CreateSqlParameter("@Description", strDescription, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                intNewQuestionID = (int)_returnValue;

            }
            return intNewQuestionID;
        }

        public static bool AlertNewUser(int intNewUserID)
        {
            bool bSuccess = false;
            string strCountry = "";
            string strLanguage = "";
            string strUserType = "";
            string strEmail = Settings.ContactEmail;
            string strLocalHostEmail = Settings.LocalHostEmail;

            string strFromEmail = Settings.ContactEmail;
            string strSubject = "NEW AARK USER REQUEST";
            string strEmailBody = "New AArk User Request          ";
            
            DataTable dt = DataProxy.GetAllUserInfo(intNewUserID);
            DataRow dr = null;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    strLanguage = DataProxy.GetLanguage(Convert.ToInt32(dr["LanguageID"]));
                    strCountry = DataProxy.GetCountry(Convert.ToInt32(dr["CountryID"]));
                    strUserType = DataProxy.GetUserType(Convert.ToInt32(dr["UserTypeID"]));

                    strEmailBody += "Name: " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + "     ";
                    strEmailBody += "Organization: " + dr["OrganizationName"].ToString() + "     ";
                    strEmailBody += "Title: " + dr["Title"].ToString() + "     ";
                    strEmailBody += "UserName:  " + dr["UserName"].ToString() + "     ";
                    strEmailBody += "UserType:  " + strUserType + "     ";
                    strEmailBody += "Country:   " + strCountry + "     ";
                    strEmailBody += "Language:   " + strLanguage + "     ";
                    strEmailBody += "Email:   " + dr["Email"].ToString() + "     ";

                    if (dr["AmphibianSpecialistGroupMember"].ToString().ToUpper() == "FALSE")
                        strEmailBody += "ASG Member: No     ";
                    else
                        strEmailBody += "ASG Member: Yes     ";

                    strEmailBody += "Country(s) of Expertise:   " + dr["CountryExpertise"].ToString() + "     ";
                    if (strLocalHostEmail == "YES")
                    {
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFromEmail, strEmail, strSubject, strEmailBody);
                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                        client.Host = "localhost";
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        client.PickupDirectoryLocation = HttpContext.Current.Server.MapPath("~/MessagesSent/");
                        client.Send(mail);
                        bSuccess = true;
                    }
                    else
                    {
                        SendEmail(strFromEmail, strEmail, strSubject, strEmailBody, false);
                        bSuccess = true;
                        //OLD WAY:
                        //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFromEmail, strEmail, strSubject, strEmailBody);
                        //SmtpClient smtp = new SmtpClient("relay-hosting.secureserver.net");
                        //smtp.Send(mail);
                        //bSuccess = true;
                    }
                }
            }

            return bSuccess;
        }

        public static int InsertUser(string strFirstName, string strLastName, string strUserName, string strPassword, string strTitle, string strEmail,
                                    string strOrganizationName, string intLanguageID, string intCountryID, string strASGMbr, string strPhone, string strCountryExpertise)
        {
            int intNewUserID = 0;
            string strDate = System.DateTime.Now.ToString();

            string strUpdateQuery = "INSERT INTO Users  " +
                                    "(FirstName, LastName, UserName, Password, UserTypeID, Email, OrganizationName, Title, LanguageID, CountryID, " +
                                    " AmphibianSpecialistGroupMember, Approved, CurrentUser, Phone, CountryExpertise, CreateDate) " +
                                    "VALUES " +
                                    "(@FirstName, @LastName, @UserName, @Password, @UserTypeID, @Email, @OrganizationName, @Title, @LanguageID, @CountryID, " +
                                    " @AmphibianSpecialistGroupMember, @Approved, @CurrentUser, @Phone, @CountryExpertise, @CreateDate); " +
                                    "SELECT CAST(scope_identity() as int)";

            bool bASGMember = false;
            if (strASGMbr == "1")
                bASGMember = true;

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
               CreateSqlParameter("@FirstName", strFirstName, SqlDbType.VarChar),
               CreateSqlParameter("@LastName", strLastName, SqlDbType.VarChar),
               CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar),
               CreateSqlParameter("@Password", strPassword, SqlDbType.VarChar),
                //For now set UserTypeID = 3 (Assessor) 
               CreateSqlParameter("@UserTypeID", 3, SqlDbType.SmallInt),
               CreateSqlParameter("@Email", strEmail, SqlDbType.VarChar),
               CreateSqlParameter("@OrganizationName", strOrganizationName, SqlDbType.VarChar),
               CreateSqlParameter("@Title", strTitle, SqlDbType.VarChar),
               CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int),
               CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
               CreateSqlParameter("@AmphibianSpecialistGroupMember", bASGMember, SqlDbType.Bit),
                //All new signups are not approved until admin manually approves them.
               CreateSqlParameter("@Approved", false, SqlDbType.Bit),
               CreateSqlParameter("@CurrentUser", false, SqlDbType.Bit),
               CreateSqlParameter("@Phone", strPhone, SqlDbType.VarChar),
               CreateSqlParameter("@CreateDate", strDate, SqlDbType.VarChar),
               CreateSqlParameter("@CountryExpertise", strCountryExpertise, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                intNewUserID = (int)_returnValue;

            }
            return intNewUserID;
        }

        public static int UpdateUser(string strFirstName, string strLastName, string strUserName, string strPassword, string strTitle, string strEmail, string strOrganizationName,
                                     string intLanguageID, string intCountryID, string strASGMbr, string strPhone, string strCountryExpertise, int intUserID)
        {
            string strUpdateQuery = " UPDATE Users " +
                                    " SET FirstName = @FirstName, " +
                                    " LastName = @LastName, " +
                                    " UserName = @UserName, " +
                                    " Password = @Password, " +
                                    " Email = @Email, " +
                                    " OrganizationName = @OrganizationName, " +
                                    " Title = @Title," +
                                    " LanguageID = @LanguageID, " +
                                    " CountryID = @CountryID, " +
                                    " AmphibianSpecialistGroupMember = @AmphibianSpecialistGroupMember," +
                                    " Phone = @Phone, " +
                                    " CountryExpertise = @CountryExpertise " +
                                    " WHERE UserID = @UserID";

            bool bASGMember = false;
            if (strASGMbr == "1")
                bASGMember = true;

             object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@FirstName", strFirstName, SqlDbType.VarChar),
                CreateSqlParameter("@LastName", strLastName, SqlDbType.VarChar),
                CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar),
                CreateSqlParameter("@Password", strPassword, SqlDbType.VarChar),
                CreateSqlParameter("@Email", strEmail, SqlDbType.VarChar),
                CreateSqlParameter("@OrganizationName", strOrganizationName, SqlDbType.VarChar),
                CreateSqlParameter("@Title", strTitle, SqlDbType.VarChar),
                CreateSqlParameter("@LanguageID", intLanguageID, SqlDbType.Int),
                CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                CreateSqlParameter("@AmphibianSpecialistGroupMember", bASGMember, SqlDbType.Bit),
                CreateSqlParameter("@Phone", strPhone, SqlDbType.VarChar),
                CreateSqlParameter("@CountryExpertise", strCountryExpertise, SqlDbType.VarChar),
                CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));

            return intUserID;
        }

        public static int InsertAssessment(int intSpeciesID, int intUserID, int intCountryID, string strAdditionalComments)
        {
            int intAssessmentID = 0;


            string strUpdateQuery = "INSERT INTO Assessments (Archived, SpeciesID, UserID, CountryID, DateCreated, Completed, Approved, AdditionalComments) " +
                                    "VALUES(0, @SpeciesID, @UserID, @CountryID, getdate(), 0, 0, @AdditionalComments); " +
                                    "SELECT CAST(scope_identity() as int)";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@SpeciesID", intSpeciesID, SqlDbType.Int),
                CreateSqlParameter("@UserID", intUserID, SqlDbType.Int),
                CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int),
                CreateSqlParameter("@AdditionalComments", strAdditionalComments, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                intAssessmentID = (int)_returnValue;

            }
            return intAssessmentID;
        }

        public static int DeleteAssessment(int intAssessmentID)
        {
            int intRtnAssessmentID = 0;

            string strQuery = "DELETE FROM Responses WHERE AssessmentID = @AssessmentID";

            object _returnValue = ExecuteScalarCommand(strQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            strQuery = "DELETE FROM AssessmentTriggers WHERE AssessmentID = @AssessmentID";
            _returnValue = ExecuteScalarCommand(strQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            strQuery = "DELETE FROM Assessments WHERE AssessmentID = @AssessmentID";
            _returnValue = ExecuteScalarCommand(strQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (_returnValue != null)
            {
                intRtnAssessmentID = (int)_returnValue;

            }
            return intRtnAssessmentID;
        }

        public static int DeleteAssessmentTrigger(int intAssessmentID)
        {
            int intRtnAssessmentID = 0;

            string strQuery = "DELETE FROM AssessmentTriggers WHERE AssessmentID = @AssessmentID";

            object _returnValue = ExecuteScalarCommand(strQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));
            
            if (_returnValue != null)
            {
                intRtnAssessmentID = (int)_returnValue;

            }
            return intRtnAssessmentID;
        }
        
        public static int UpdateAssessment(int intAssessmentID, int intUserID, int intCountryID, bool bCompleted, string strAdditionalComments, bool bApproved)
        {
            string strCurDate = System.DateTime.Now.ToString();

            string strUpdateQuery = "";
                
            if (intUserID > 0)
                strUpdateQuery = "UPDATE Assessments " +
                                    " SET DateCreated = @CurDate, " +
                                    " UserID = @UserID," +
                                    " CountryID = @CountryID," +
                                    " Completed = @Completed," +
                                    " AdditionalComments = @AdditionalComments," +
                                    " Approved = @Approved " +
                                    " WHERE AssessmentID = @AssessmentID; SELECT @AssessmentID; ";
            else
                strUpdateQuery =   " UPDATE Assessments " +
                                    " SET CountryID = @CountryID," +
                                    " Completed = @Completed," +
                                    " AdditionalComments = @AdditionalComments," +
                                    " Approved = @Approved " +
                                    " WHERE AssessmentID = @AssessmentID; SELECT @AssessmentID; ";  
             object _returnValue = 
                _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                //CreateSqlParameter("@Priority", (object)intPriority ?? DBNull.Value, SqlDbType.Int),
                CreateSqlParameter("@Completed", bCompleted, SqlDbType.Bit),
                CreateSqlParameter("@Approved", bApproved, SqlDbType.Bit),
                CreateSqlParameter("@UserID", intUserID, SqlDbType.Int),
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int),
                CreateSqlParameter("@CurDate", strCurDate, SqlDbType.VarChar),
                CreateSqlParameter("@AdditionalComments", strAdditionalComments, SqlDbType.VarChar),
                CreateSqlParameter("@CountryID", intCountryID, SqlDbType.Int));
          
            if (_returnValue != null)
            {
                intAssessmentID = (int)_returnValue;

            }
            return intAssessmentID;

        }

        public static int InsertResponse(int intAssessmentID, int intPossibleAnswerID, string strComments)
        {
            int intResponseID = 0;

            string strUpdateQuery = "INSERT INTO Responses (AssessmentID, PossibleAnswerID, ResponseComments, ResponseDate) " +
                                    "VALUES(@AssessmentID, @PossibleAnswerID, @Comments, GETDATE()); " +
                                    "SELECT CAST(scope_identity() as int)";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int),
                CreateSqlParameter("@PossibleAnswerID", intPossibleAnswerID, SqlDbType.Int),
                CreateSqlParameter("@Comments", strComments, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                intResponseID = (int)_returnValue;

            }
            return intResponseID;
        }
        /*JK: Not using these - doing all updates within the usp_EvaluateTriggers stored procedure now
        public static string UpdateAssessmentTriggers(int intAssessmentTriggerID, int intTriggerID)
        {
            string strRtn = "";

            string strUpdateQuery = "usp_UpdateAssessmentTriggers @AssessmentTriggerID, @TriggerValue ";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentTriggerID", intAssessmentTriggerID, SqlDbType.Int),
                CreateSqlParameter("@TriggerValue", intTriggerID, SqlDbType.Int));

            if (_returnValue != null)
                strRtn = _returnValue.ToString();

            return strRtn;
        }

        public static string InsertAssessmentTriggers(int intAssessmentID, int intTriggerID)
        {
            string strRtnID = "";

            string strUpdateQuery = "usp_InsertAssessmentTriggers @AssessmentID, @TriggerValue ";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int),
                CreateSqlParameter("@TriggerValue", intTriggerID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnID = _returnValue.ToString();
            }

            return strRtnID;
        }
        */
        public static string ApproveAssessment(int intAssessmentID, string strApproved)
        {
            string strRtnVal = "";

            string strUpdateQuery = "";

            if (strApproved == "1")
                strUpdateQuery = "UPDATE Assessments SET Approved = 1  WHERE AssessmentID = @AssessmentID";
            else
                strUpdateQuery = "UPDATE Assessments SET Approved = 0  WHERE AssessmentID = @AssessmentID";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }
        public static string ApproveUser(int intUserID, string strApproved)
        {
            string strRtnVal = "";

            string strUpdateQuery = "";
            
            if (strApproved == "1")
                strUpdateQuery = "UPDATE Users SET Approved = 1  WHERE UserID = @UserID";
            else
                strUpdateQuery = "UPDATE Users SET Approved = 0  WHERE UserID = @UserID";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@UserID", intUserID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }

        public static string UpdateUserPassword(string strUserName, string strPassword)
        {
            string strRtnVal = "";

            string strUpdateQuery = "UPDATE Users SET Password = @Password WHERE UserName=@UserName";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@UserName", strUserName, SqlDbType.VarChar),
                CreateSqlParameter("@Password", strPassword, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }

        public static string ArchiveAssessment(int intAssessmentID)
        {
            string strRtnVal = "";

            string strUpdateQuery = "UPDATE Assessments SET Archived = 1 WHERE AssessmentID=@AssessmentID";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }

        public static string UnarchiveAssessment(int intAssessmentID)
        {
            string strRtnVal = "";

            string strUpdateQuery = "UPDATE Assessments SET Archived = 0 WHERE AssessmentID=@AssessmentID";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }

        public static string UpdateAssessmentPriority(int intAssessmentID)
        {
            string strRtnVal = "";

            string strUpdateQuery = "usp_UpdateAssessmentPriority @AssessmentID";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }

        public static string EvaluateAssessmentTriggers(int intAssessmentID)
        {
            string strRtnVal = "";

            string strUpdateQuery = "usp_EvaluateAssessmentTriggers @AssessmentID";

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@AssessmentID", intAssessmentID, SqlDbType.Int));

            if (_returnValue != null)
            {
                strRtnVal = _returnValue.ToString();

            }
            return strRtnVal;
        }

        public static int UpdateResponse(int ResponseID, int intPossibleAnswerID, string strComments)
        {
            int intResponseID = 0;

            string strUpdateQuery = "UPDATE Responses " +
                                    "SET PossibleAnswerID = @PossibleAnswerID, ResponseComments = @Comments  " +
                                    "WHERE ResponseID = @ResponseID " ;

            object _returnValue = ExecuteScalarCommand(strUpdateQuery, false,
                CreateSqlParameter("@ResponseID", ResponseID, SqlDbType.Int),
                CreateSqlParameter("@PossibleAnswerID", intPossibleAnswerID, SqlDbType.Int),
                CreateSqlParameter("@Comments", strComments, SqlDbType.VarChar));

            if (_returnValue != null)
            {
                intResponseID = ResponseID;

            }
            return intResponseID;
        }


        #region Core Database Operations

        public static int GetInsertedRecordID(string tableName)
        {
            int _insertedRecordId = 0;
            DataSet _insertedRecordIDDataSet = DataProxy.GetDataSet("SELECT IDENT_CURRENT('" + tableName + "')");
            if (_insertedRecordIDDataSet.Tables.Count > 0)
            {
                if (_insertedRecordIDDataSet.Tables[0].Rows.Count > 0)
                {
                    _insertedRecordId = int.Parse(_insertedRecordIDDataSet.Tables[0].Rows[0][0].ToString());
                }
            }

            return _insertedRecordId;
        }

        public static DataSet GetDataSet(string sqlCommand)
        {
            DataSet resultDataSet = new DataSet();
            using (SqlConnection _sqlConnection = GetSqlConnection(false))
            {

                try
                {
                    GetSqlAdapter(sqlCommand, _sqlConnection).Fill(resultDataSet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return resultDataSet;
        }

        public static DataSet GetDataSet(string sqlCommand, Dictionary<string, object> sqlParameters)
        {
            SqlConnection _sqlConnection = GetSqlConnection(false);
            DataSet resultDataSet = new DataSet();
            SqlDataAdapter dataAdapter = GetSqlAdapter(sqlCommand, _sqlConnection);
            try
            {
                dataAdapter.SelectCommand.Parameters.Clear();
                foreach (string sqlParameterName in sqlParameters.Keys)
                {
                    dataAdapter.SelectCommand.Parameters.Add(new SqlParameter(sqlParameterName, sqlParameters[sqlParameterName]));
                }
                dataAdapter.Fill(resultDataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }

            return resultDataSet;
        }

        public static DataSet GetDataSet(string sqlCommand, SqlParameter sqlParameter, bool isStoredProc)
        {
            SqlConnection _sqlConnection = GetSqlConnection(false);
            DataSet resultDataSet = new DataSet();
            SqlDataAdapter dataAdapter = GetSqlAdapter(sqlCommand, _sqlConnection);
            try
            {
                dataAdapter.SelectCommand.Parameters.Clear();
                if (isStoredProc) dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter);
                dataAdapter.Fill(resultDataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }

            return resultDataSet;
        }

        public static DataSet GetDataSet(string sqlCommand, SqlParameter[] sqlParameters, bool isStoredProc)
        {
            SqlConnection _sqlConnection = GetSqlConnection(false);
            DataSet resultDataSet = new DataSet();
            SqlDataAdapter dataAdapter = GetSqlAdapter(sqlCommand, _sqlConnection);
            try
            {
                dataAdapter.SelectCommand.Parameters.Clear();
                if (isStoredProc) dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddRange(sqlParameters);
                dataAdapter.Fill(resultDataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }

            return resultDataSet;
        }

        public static DataSet GetDataSet(string sqlCommand, bool isStoredProc, params SqlParameter[] sqlParameters)
        {
            SqlConnection _sqlConnection = GetSqlConnection(false);
            DataSet resultDataSet = new DataSet();
            SqlDataAdapter dataAdapter = GetSqlAdapter(sqlCommand, _sqlConnection);
            try
            {
                dataAdapter.SelectCommand.Parameters.Clear();
                if (isStoredProc) dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddRange(sqlParameters);

                dataAdapter.Fill(resultDataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }

            return resultDataSet;
        }

        public static SqlDataReader ExecuteDataReader(string sqlCommand, SqlParameter[] sqlParameters, bool isStoredProc)
        {
            SqlDataReader sqlDataReader = null;
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand dataReaderCommand = GetSqlCommand(_connection);
                dataReaderCommand.Parameters.Clear();
                dataReaderCommand.CommandText = sqlCommand;
                dataReaderCommand.Parameters.AddRange(sqlParameters);
                if (isStoredProc) dataReaderCommand.CommandType = CommandType.StoredProcedure;
                else dataReaderCommand.CommandType = CommandType.Text;
                sqlDataReader = dataReaderCommand.ExecuteReader();
            }
            return sqlDataReader;
        }

        public static SqlCommand GetSqlCommand(SqlConnection Connection)
        {
            return Connection.CreateCommand();
        }

        public static SqlConnection GetSqlConnection(bool SingleConnection)
        {
            SqlConnection _connection = null;
            try
            {
                _connection = (SingleConnection) ? GetSqlConnection() : new SqlConnection(Settings.AARKConnectionString);
                if (_connection.State != ConnectionState.Open) _connection.Open();
            }
            catch (SqlException sqlEx)
            {
                try
                {
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                    _connection = (SingleConnection) ? GetSqlConnection() : new SqlConnection(Settings.AARKConnectionString);
                    if (_connection.State != ConnectionState.Open) _connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception ("A SQL Exception occured: " + sqlEx.Message + ".  Then another exception occurred: " + ex.Message + ", when trying to create a SQL connection.");
                }
            }

            return _connection;
        }

        public static SqlConnection GetSqlConnection()
        {
            return GetSqlConnection(false);
        }
        public static int ExecuteSQLCommand(string sqlCommand)
        {
            int rowsAffected = 0;
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand executedSql = _connection.CreateCommand();
                
                try
                {
                    executedSql.CommandText = sqlCommand;
                    rowsAffected = executedSql.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return rowsAffected;
        }

        public static int ExecuteScalarCommand(string sqlCommand)
        {
            int intNewID = 0;
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand executedSql = _connection.CreateCommand();

                try
                {
                    executedSql.CommandText = sqlCommand;
                    intNewID = (Int32)executedSql.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return intNewID;
        }

        public static object ExecuteScalarCommand(string sqlCommand, bool IsStoredProc, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand executedSql = GetSqlCommand(_connection);
                try
                {
                    executedSql.CommandText = sqlCommand;
                    if (IsStoredProc)
                        executedSql.CommandType = CommandType.StoredProcedure;
                    else
                        executedSql.CommandType = CommandType.Text;

                    executedSql.Parameters.Clear();
                    executedSql.Parameters.AddRange(sqlParameters);

                    return executedSql.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    executedSql.Dispose();
                }
            }

        }

        public static int ExecuteSQLCommand(string sqlCommand, bool isStoredProc, params SqlParameter[] sqlParameters)
        {
            int rowsAffected = 0;
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand executedSql = _connection.CreateCommand();
                
                try
                {
                    executedSql.CommandText = sqlCommand;
                    if (isStoredProc) executedSql.CommandType = CommandType.StoredProcedure;
                    executedSql.Parameters.Clear();
                    executedSql.Parameters.AddRange(sqlParameters);

                    rowsAffected = executedSql.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    executedSql.Dispose();
                }
            }

            return rowsAffected;
        }

        public static int ExecuteSQLCommand(string sqlCommand, SqlParameter[] sqlParameters, bool isStoredProc)
        {
            int rowsAffected = 0;
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand executedSql = _connection.CreateCommand();

                try
                {
                    executedSql.CommandText = sqlCommand;
                    if (isStoredProc) executedSql.CommandType = CommandType.StoredProcedure;
                    executedSql.Parameters.Clear();
                    executedSql.Parameters.AddRange(sqlParameters);

                    rowsAffected = executedSql.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return rowsAffected;
        }

        public static SqlParameter CreateSqlParameter(string parameterName, object parameterValue, SqlDbType dbValueType)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, dbValueType);
            sqlParameter.Value = parameterValue;
            return sqlParameter;
        }

        public static SqlParameter CreateSqlParameter(string parameterName, object parameterValue, SqlDbType dbValueType, int parameterSize)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, dbValueType, parameterSize);
            sqlParameter.Value = parameterValue;
            return sqlParameter;
        }

        public static int ExecuteSQLCommand(string sqlCommand, Dictionary<string, object> sqlParameters, bool isStoredProc)
        {
            int rowsAffected = 0;
            using (SqlConnection _connection = GetSqlConnection(false))
            {
                SqlCommand executedSql = _connection.CreateCommand();

                try
                {
                    executedSql.CommandText = sqlCommand;
                    if (isStoredProc) executedSql.CommandType = CommandType.StoredProcedure;
                    executedSql.Parameters.Clear();
                    foreach (string key in sqlParameters.Keys)
                    {
                        executedSql.Parameters.Add(new SqlParameter(key, sqlParameters[key]));
                    }

                    rowsAffected = executedSql.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return rowsAffected;
        }

        private static SqlDataAdapter GetSqlAdapter(string sqlCommand, SqlConnection Connection)
        {
            return new SqlDataAdapter(sqlCommand, Connection);
        }

        #endregion


        public static void ResetAllSQLConnections(SqlConnection _connection)
        {
            System.Data.SqlClient.SqlConnection.ClearPool(_connection);
        }

       
        private static string GetProjectTypebyProjectTypeID(int projectTypeID)
        {
            string _projectType = string.Empty;

            string _getProjectTypebyProjectTypeID = "SELECT Description FROM projecttype WHERE ProjectTypeID=@ProjectTypeId";
            DataSet projectTypeDataSet = DataProxy.GetDataSet(_getProjectTypebyProjectTypeID, new SqlParameter("@ProjectTypeId", projectTypeID), false);
            if (projectTypeDataSet.Tables.Count > 0)
            {
                if (projectTypeDataSet.Tables[0].Rows.Count > 0)
                {
                    _projectType = projectTypeDataSet.Tables[0].Rows[0]["Description"].ToString();
                }
            }

            return _projectType;
        }


        public static DataTable SortDataTable(DataTable UnsortedDataTable, string SortCriteria)
        {
            DataView _unsortedView = UnsortedDataTable.AsDataView();
            if (_unsortedView.Table.Rows.Count > 0)
            {
                try
                {
                    _unsortedView.Sort = SortCriteria;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return _unsortedView.ToTable();
        }

        public static string GetProjectUrlForProjectID(int p)
        {
            throw new NotImplementedException();
        }

        public static decimal GetPercentageOfTotal(string FilterCriteria, DataTable EvalRecords)
        {
            int _totalRecords = EvalRecords.Rows.Count;
            decimal _percentageOfTotal = 0;
            if (_totalRecords > 0)
            {
                int _noOfRecordInFilteredSet = DataProxy.GetFilteredDataTable(EvalRecords, FilterCriteria).Rows.Count;
                _percentageOfTotal = (decimal)_noOfRecordInFilteredSet / (decimal)_totalRecords * 100;
            }

            return _percentageOfTotal;
        }

        public static DataTable GetTrimmedDataTable(DataTable OriginalDataTable, int MaximumRows)
        {
            int _rowCount = OriginalDataTable.Rows.Count;
            DataTable _trimmedTable = new DataTable();

            if (_rowCount > 0 && _rowCount > MaximumRows)
            {
                foreach (DataColumn _column in OriginalDataTable.Columns)
                {
                    _trimmedTable.Columns.Add(_column.ColumnName,_column.DataType);
                }

                for (int x = 0; x <= MaximumRows - 1; x++)
                {
                    
                    _trimmedTable.ImportRow(OriginalDataTable.Rows[x]);
                }
                return _trimmedTable;
            }
            else
            {
                return OriginalDataTable;
            }
            
        }

    }
}
