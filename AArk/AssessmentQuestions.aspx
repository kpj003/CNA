<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssessmentQuestions.aspx.cs" Inherits="AArk.AssessmentQuestions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <link href="styles/StyleSheetMain.css" rel="stylesheet" type="text/css" />
    
    
    <p style="text-align:center" class="TextHeader">
        <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
       
        
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br />
    <b>
    Still mulling over what should be here? <br />
    Just this one question the user clicked on or all the Assessment Questions/Responses in detailed tabs and this one is highlighted?</b>
    <br /><br />Testing URL Values: &nbsp;<asp:Label Font-Bold="true" Font-Italic="true" runat="server" ID="lbltest"></asp:Label>

    </p>

</asp:Content>
