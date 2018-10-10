<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportsMain.aspx.cs" Inherits="AArk.ReportsMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
            
       <p style="text-align:center;width:980px" class="TextHeader">
          <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
    <br />
     <table style="width:980px; align-self:center; align-items:center; align-content:center">
        <tr>
            <td style="width:40%; vertical-align:text-top">    
                <a style="text-decoration:none" id="lnkHome" runat="server" href="ApprovedUsers.aspx">
                    <asp:Label CssClass="LinksText" runat="server" ID="lblApprovedUsers"></asp:Label>
                </a></td>
                <td style="width:60%; vertical-align:text-top">   
                <asp:Label CssClass="TextParagraph" runat="server" ID="lblApprovedUsersInfo"></asp:Label>
                <br /><br />
            </td>
        </tr> 
         <tr>
            <td style="width:40%; vertical-align:text-top">   <p style="vertical-align:baseline">
                <a style="text-decoration:none" id="lnkAssessbyUser" runat="server" href="AssessmentsByUser.aspx">
                    <asp:Label CssClass="LinksText" runat="server" ID="lblAssessmentByUsers"></asp:Label>
                </a></p></td>
                <td style="width:60%; vertical-align:bottom">  
                   <asp:Label CssClass="TextParagraph" runat="server" ID="lblAssessmentUsersInfo"></asp:Label>
                <br /><br />
            </td>
        </tr>
       
         <tr>
            <td style="width:40%; vertical-align:text-top">   <p style="vertical-align:baseline">
                <a style="text-decoration:none" id="A1" runat="server" href="NationalAssessmentReport.aspx">
                    <asp:Label CssClass="LinksText" runat="server" ID="lblNationalAssessments"></asp:Label>
                </a></p></td>
                <td style="width:60%; vertical-align:bottom">  
                   <asp:Label CssClass="TextParagraph" runat="server" ID="lblNationalAssessmentsInfo"></asp:Label>
                <br /><br />
            </td>
        </tr>
       
         <tr>
            <td style="width:40%; vertical-align:text-top">   <p style="vertical-align:baseline">
                <a style="text-decoration:none" id="A2" runat="server" href="SpeciesRecommendRescue.aspx">
                    <asp:Label CssClass="LinksText" runat="server" ID="lblSpeciesRecommendRescue"></asp:Label>
                </a></p></td>
                <td style="width:60%; vertical-align:bottom">  
                   <asp:Label CssClass="TextParagraph" runat="server" ID="lblSpeciesRecommendRescueInfo"></asp:Label>
                <br /><br />
            </td>
        </tr>
        
     </table>
   
</asp:Content>
