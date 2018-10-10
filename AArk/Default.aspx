<%@ Page Title="Amphibian Conservation Needs Assessments" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="AArk._Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="cntApp">
   
    <table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px"> 
    <tr>
       
        <td style="text-align:left; vertical-align:top; width:50%">
            <strong><asp:Label runat="server" ID="lblWelcome" CssClass="TextHeader"></asp:Label> </strong><br />
                  <asp:label runat="server" ID="lblMain" CssClass="TextPlain"></asp:label>&nbsp;&nbsp;
                <a style="text-decoration:none; color:Green" id="lnkOverview" runat="server" Target="_blank" href="AssessmentOverview.aspx">
                    <asp:label Font-Underline="false" CssClass="TextText" runat="server" ID="lblMore"></asp:label>
                </a>
        </td>
        <td style="text-align:left; vertical-align:top; width:50%">
            <strong><asp:Label runat="server" ID="lblRightHeader" CssClass="TextHeader"></asp:Label> </strong><br />
            <asp:Label runat="server" ID="lblTopRightInfo" CssClass="TextPlain"></asp:Label>
             <a style="text-decoration:none; color:Green" id="lnkAssessorMore" runat="server" Target="_blank" href="AssessorInfo.aspx">
                <asp:label Font-Underline="false" CssClass="TextText" runat="server" ID="lblRightMore"></asp:label>
             </a>
        </td>
    </tr>
    <!--</table>
    <table style="width:95%;">-->
    <tr>
        <td style="vertical-align:top; align-content:center;text-align:center"> 
                <br />
                <a style="text-decoration:none" id="lnkSearch" runat="server" href="AssessmentSearch.aspx">
                     <asp:Label CssClass="LinksText" runat="server" ID="lblSearch" style="text-align: center"></asp:Label>
                </a><br /><br />
                
                <a style="text-decoration:none" id="A2" runat="server" href="AssessmentSearch.aspx">
                <img width="85" height="85" src="images/treefrog.jpg" border="0" style="border-color: #FFFFFF"/></a>
         
       </td>
        
        <td style="vertical-align:top; text-align:center; align-content:center">
             <br /> <a style="text-decoration:none" id="A3" runat="server" href="NationalAssessmentReport.aspx">
            <asp:Label CssClass="LinksText" runat="server" ID="lblMap"></asp:Label></a>
            <br /><br />
            <a style="text-decoration:none" id="A1" runat="server" href="NationalAssessmentReport.aspx">
            <img src="images/map2.jpg" /></a></td>
       </tr>
    <tr>
        <td style="vertical-align:top; text-align:left">
        <br /><asp:Label  CssClass="TextHeaders" runat="server" ID="lblRecentAssessments"></asp:Label>
        <p style="text-align:left; font-family:Arial">
            <asp:Literal runat="server" Visible="true" ID="ltRecentAssessments"></asp:Literal><br />
        </p>
        </td>
         <td style="vertical-align:top; text-align:center">
           <p><br />
               <a style="text-decoration:none" id="lnkLogin" runat="server" href="Login.aspx">
                     <asp:Label CssClass="LinksText" runat="server" ID="lblLogin"></asp:Label></a><br /><br />
                 </p>
                <asp:Label Font-Bold="true" CssClass="TextParagraph" runat="server" ID="lblSignup"></asp:Label>
                 <asp:Button runat="server" id="btnSignup" OnClick="btnSignup_Click"/></td>
     </tr>
  
</table>
   
</asp:Content>
