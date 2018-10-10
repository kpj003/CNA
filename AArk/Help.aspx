<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="AArk.Help" %>
<%@ Register TagPrefix="AArk" TagName="HelpMenu" Src="HelpMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <p class="TextText" style="font-size: medium">HELP PAGE...</p>
       <table style="width:980px; align-self:left; align-items:left; align-content:left">
            <tr>
            <td style="text-align:left;vertical-align:central; width:70px">
                <AArk:HelpMenu Visible="true" runat="server" />
            </td>
            <td style="width:910px; vertical-align:top">
              <p class="TextParagraph"><i>Content goes here...</i> <br /><br />
                        I like frogs. Even the naughty toads that aren't supposed to be in my yard!<br />
                        The polka-dotted USA frogs are the best though!.<br />
                        I can&#39;t wait to save my assessment. How do I do that again? Hopefully this help file helps!</p>
            </td>
        </tr>
        </table>      
</asp:Content>

