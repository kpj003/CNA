<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyDashboard.aspx.cs" Inherits="AArk.MyDashBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
      <p style="text-align:center" class="TextHeader">
            <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
      <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

</asp:Content>
