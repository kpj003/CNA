<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="AArk.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
  
    <p style="text-align:left" class="TextText">
        <font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font>
        <br /><br />
<asp:Label runat="server" CssClass="TextParagraph" ID="lblContent"></asp:Label>

<br /><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br /></p>

<table class="userFormTable" border="0" cellspacing="2" cellpadding="2">
<tbody>
 
<tr>
    <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblFirstName"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtFirstName" class="userFormField" runat="server" Width="200"></asp:TextBox>
        <br />
    </td>
</tr>  
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblLastName"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtLastName" class="userFormField" runat="server" Width="200"></asp:TextBox>
        <br />
    </td>
</tr>  
<tr>
    <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
        <asp:Label runat="server" ID="lblEmail" Text=""></asp:Label>&nbsp; </p>
    </td>
    <td class="userFormField" valign="top" align="left">
        <asp:TextBox ID="txtEmail" runat="server"  Width="200"></asp:TextBox>
    </td>
</tr>
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblTitle"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtTitle" class="userFormField" runat="server" Width="200"></asp:TextBox>
        <br />
    </td>
</tr>  
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblOrganization"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtOrganization" class="userFormField" runat="server" Width="200"></asp:TextBox>
        <br />
    </td>
</tr>  
      
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblPhone"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtPhone" class="userFormField" runat="server" Width="200"></asp:TextBox>
        <br />
    </td>
</tr>  
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:DropDownList runat="server" ID="ddlCountries" Width="200"></asp:DropDownList>
        <br />
    </td>
</tr>  
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblLanguage"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:DropDownList runat="server" ID="ddlLanguages" Width="200"></asp:DropDownList>
        <br />
    </td>
</tr>  
<tr>
    <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
        <asp:Label runat="server" ID="lblLogin" Text=""></asp:Label>&nbsp; </p>
    </td>
    <td class="userFormField" valign="top" align="left">
        <asp:TextBox ID="txtLoginName" runat="server"  Width="200"></asp:TextBox>
    </td>
</tr>
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblPassword"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtPassword" class="userFormField" runat="server" Width="200" TextMode="Password"></asp:TextBox>
        <br />
    </td>
</tr>  
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblUserCountries"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left">
        <asp:TextBox ID="txtUserCountries" class="userFormField" runat="server" Width="700"></asp:TextBox>
        <br />
    </td>
</tr>
<tr>
     <td valign="top" align="left" class="style1"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblASGMbr"></asp:Label>&nbsp; </p>
    </td>
  
    <td align="left" valign="top" >
           <asp:RadioButtonList ID="rbASGMbr" class="userFormField" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Yes" class="TextParagraph" Value="1"></asp:ListItem>
                <asp:ListItem Text="No" Selected="True" class="TextParagraph" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        <br />
    </td>
</tr>     
<tr>
    <td>&nbsp;</td>
    <td><p style="align-items:center; text-align:center">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" /></p>
    </td></tr>
  
    
</tbody>
</table>
</asp:Content>
