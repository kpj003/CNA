<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="AArk.ReportViewer" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <p style="text-align:center" class="TextHeader">
        <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>

  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>     
        
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br /></p>
    <p style="text-align:left" class="TextParagraph">&nbsp;</p>

  <table style="align-items:center;align-self:center; text-align:center; align-content:center; width:980px; border-spacing:4px"> 
    <tr>
  
        <td>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" ProcessingMode="Local" Width="980px" Height="1200px">
            </rsweb:ReportViewer>

        </td>
    </tr>
  </table>

</asp:Content>
