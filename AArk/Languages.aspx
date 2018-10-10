<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Languages.aspx.cs" Inherits="AArk.Languages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <link href="styles/StyleSheetMain.css" rel="stylesheet" type="text/css" />

<p class="TextText">
    <asp:Label ID="lblLanguageText" runat="server"></asp:Label>

</p>
<br /> 
    <table style="width:50%; padding:3px">
        <tr><td>
            <asp:ImageButton runat="server" ID="btnEnglish" ImageUrl="images/English%20flag.png" OnClick="btnEnglish_Click" />
        </td>
        <td><asp:ImageButton runat="server"  ID="btnSpanish" OnClick="btnSpanish_Click" ImageUrl="images/Spanish%20flag.png"/></td></tr>
        <tr><td style="font-family: Arial, Helvetica, sans-serif; font-size: x-small; font-style: italic">(English)</td>
            <td style="font-family: Arial, Helvetica, sans-serif; font-size: x-small; font-style: italic">(Español)</td></tr>
        </table>
 
</asp:Content>
