<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AArk.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    
  
    <p style="text-align:left" class="TextText">
        <font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font>
        <br /><br />
<asp:Label runat="server" ID="lblContent" CssClass="TextParagraph"></asp:Label><br /><br />


    <asp:Panel ID="p" runat="server" DefaultButton="btnChangePassword">
<table class="userFormTable" border="0" cellspacing="2" cellpadding="2">
<tbody>
<tr><td colspan="2">
<asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></td></tr>
<tr>
    <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
        <asp:Label runat="server" ID="lblLogin" Text=""></asp:Label>&nbsp; </p>
    </td>
    <td class="userFormField" valign="top" align="left">
        <asp:TextBox ID="txtLogin" runat="server"  Width="175"></asp:TextBox>
    </td>
</tr>
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblPassword"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtPassword" class="userFormField" runat="server" Width="175" TextMode="Password"></asp:TextBox>
        <br />
    </td>
</tr>     
<tr>
    <td>&nbsp;</td>
    <td><p style="align-items:center; text-align:center">
        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" /></p><br />
    </td></tr>
</tbody>
</table>
</asp:Panel>


</asp:Content>
