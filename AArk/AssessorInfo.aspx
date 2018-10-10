<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssessorInfo.aspx.cs" Inherits="AArk.AssessorInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">

    <p style="text-align:center" class="TextHeader"><br />
        <font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font>
        <br />
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent1"></asp:Label></p>   
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent2"></asp:Label></p>
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent3"></asp:Label>
                            <a style="text-decoration:none; color:Green" id="lnkAssessorMore" runat="server" href="Signup.aspx">
                                <asp:Label runat="server" CssClass="TextText" ID="lblContent3Link"></asp:Label></a>
                            <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent3b"></asp:Label></p>

<p style="text-align:left"><b><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent4Hdr"></asp:Label></b></p>   
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent4"></asp:Label></p>
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent4Future"></asp:Label></p>
<p style="text-align:left"><b><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent5Hdr"></asp:Label></b></p>   
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent5"></asp:Label></p>
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent5Future"></asp:Label></p>
    
<p style="text-align:left"><b><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent6Hdr"></asp:Label></b></p>   
<p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent6"></asp:Label>
                            <a style="text-decoration:none; color:Green" target="_blank" id="lnkAssessorMore2" runat="server" href="http://www.amphibianark.org/about-us/aark-activities/planning-workshops/">
                                <asp:Label runat="server" CssClass="TextText" ID="lblContent6Link"></asp:Label></a>
                            <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent6b"></asp:Label></p>
</asp:Content>
