<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AArk.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
  
  
    <p style="text-align:left" class="TextText">
        <font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font>
        <br /><br />
<asp:Label runat="server" ID="lblContent" CssClass="TextParagraph"></asp:Label><br /><br />
        <asp:Label Font-Bold="true" CssClass="TextParagraph" runat="server" ID="lblSignup"></asp:Label>&nbsp;&nbsp;
                 <asp:Button runat="server" id="btnSignup" OnClick="btnSignup_Click" />

    </p>
<asp:Panel ID="p" runat="server" DefaultButton="btnLogin">
<table class="userFormTable" border="0" cellspacing="2" cellpadding="2">
<tbody>
<tr><td colspan="2">
<asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></td></tr>
<tr>
    <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
        <asp:Label runat="server" ID="lblLogin" Text=""></asp:Label>&nbsp; </p>
    </td>
    <td class="userFormField" valign="top" align="left">
        <asp:TextBox ID="txtLoginName" runat="server"  Width="175"></asp:TextBox>
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
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /></p>
    </td></tr>
    <tr><td>&nbsp;</td>
        <td><p style="align-items:center; text-align:center"> 
            <asp:LinkButton Font-Size="Small" ForeColor="Green" Font-Names="Arial" ID="lnkForgot" name="lnkForgot" runat="server" CausesValidation="false" OnClick="lnkForgot_Click" style="font-size: x-small"/>

            </p>
        </td></tr>
    
</tbody>
</table>
</asp:Panel>
</asp:Content>
