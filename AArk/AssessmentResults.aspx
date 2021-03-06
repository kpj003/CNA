﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="AssessmentResults.aspx.cs" Inherits="AArk.AssessmentResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">

  <link href="styles/StyleSheetMain.css" rel="stylesheet" type="text/css" />
 
    
    <!-- ensure the src attribute in the following script tag points to your JS file -->
    <script type="text/javascript">
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var strOldOne = prtContent.innerHTML;
            var WinPrint = window.open('', '', 'left=0,top=0,width=650,height=700,toolbar=1,scrollbars=1,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
        }
    </script>

 <script type="text/javascript">

      function openNewWin(url) {
          var x = window.open(url, 'iNaturalist');
            //x.focus();
      }
</script>
    <!--
 <script
      src="http://www.inaturalist.org/observations.widget">
 </script>-->
       
    <p style="text-align:left" class="TextHeader">
        <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
    <p style="text-align:left" class="TextParagraph">
        <asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br /></p>
    <table style="align-items:center;align-self:center; text-align:center; align-content:center; width:980px; border-spacing:4px"> 
      <tr>  
        <td style="text-align:center; vertical-align:bottom; width:30%">
           
            <asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="True" /> 
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="True" />
        </td>   
        <td style="text-align:left;font-family:Arial; vertical-align:bottom; width:35%">
                <br />
                    <a id="hlImage" runat="server" target="_blank">
                    <asp:Image ID="imgAWeb" runat="server" /></a>    
                    <br />
                    <asp:Label ID="lblCopyright" Font-Size="X-Small" CssClass="TextCopy" runat="server"></asp:Label>
                    <asp:HyperLink ID="lnkCopyright" Target="_blank" Font-Size="X-Small" CssClass="TextCopy" Visible="false" Text="1" runat="server" ></asp:HyperLink>
                    <asp:Label ID="lblCopyDummy" Font-Size="X-Small" CssClass="TextCopy" Visible="false" Text=")" runat="server"></asp:Label>
            
                    <br />
                    <br /><asp:HyperLink ID="lnkAWeb" Target="_blank" CssClass="LinksText" Text="" runat="server" ></asp:HyperLink>

        </td>
        <td style="text-align:left; vertical-align:bottom; width:35%">
            <p style="text-align:center">
                  <a runat="server" id="lnkRLMap" target="_blank"><br />
                  <asp:ImageMap Height="101" Width="196" runat="server" ID="imgRLMap" /></a>
                  <asp:ImageMap Height="101" Width="196" Visible="false" runat="server" ID="imgRLMapOnly" /><br /><br />
                  <asp:HyperLink CssClass="LinksText" runat="server" Target="_blank" ID="lnkRedList"></asp:HyperLink>
            </p>
        </td></tr>
        </table>

        <asp:Panel CssClass="TextText" runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
        <p style="font-family:Arial;font-size:small">
        <asp:Label runat="server" CssClass="TextHeader" ID="lblReportTitle" Font-Italic="true" Visible="true" Font-Bold="true"></asp:Label><br />
        </p>
        <table style="align-items:center;align-self:center; text-align:center; align-content:center; width:980px; border-spacing:4px"> 
        <tr>
        <td style="text-wrap:avoid; text-align:left; vertical-align:top; width:45%">
            <p style="font-family:Arial;font-size:small">
          
            <strong><asp:Label Font-Italic="true" runat="server" ID="lblSpeciesDisplayName" CssClass="TextHeader"></asp:Label>&nbsp;&nbsp;
                    <asp:Label runat="server" ID="lblSpeciesAssessmentDisplayName" CssClass="TextHeader"></asp:Label> 
            </strong><br />
            <asp:Label runat="server" Font-Bold="true" ForeColor="Green" ID="lblAssessedFor" CssClass="TextParagraph"></asp:Label>&nbsp;
            <asp:label runat="server" ID="lblAssessedForValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:Label runat="server" Font-Bold="true" ForeColor="Green" ID="lblAssessedOn" CssClass="TextParagraph"></asp:Label>
            <asp:label runat="server" ID="lblAssessedOnValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:Label runat="server" Font-Bold="true" ForeColor="Green" ID="lblAssessedBy" CssClass="TextParagraph"></asp:Label>
            <asp:label runat="server" ID="lblAssessedByValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
         
            <br />
              <asp:Label runat="server" Font-Bold="true" ForeColor="Green" ID="lblAssessmentStatus" CssClass="TextParagraph"></asp:Label>
            <asp:label runat="server" ID="lblAssessmentStatusValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
         
            <br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblOrder" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblOrderValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblFamily" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblFamilyValue" CssClass="TextParagraph"></asp:label>
            <br /><br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblGlobalRLC" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblGlobalRLCValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;&nbsp;<br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblNationalRLC" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblNationalRLCValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;&nbsp;<br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblDistribution" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:label runat="server" ID="lblDistributionValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;<br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblEDScore" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblEDScoreValue" CssClass="TextParagraph"></asp:label>&nbsp;<br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblTriggerRec" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblTriggerRecValue" CssClass="TextParagraph"></asp:label>&nbsp;<br />
            <asp:label Font-Bold="true" ForeColor="Green" runat="server" ID="lblAdditionalComments" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;
            <asp:label runat="server" ID="lblAdditionalCommentsValue" CssClass="TextParagraph"></asp:label>&nbsp;<br /><br />
            </p>
        </td>
           
    </tr>
      <tr><td colspan="3">
            <p class="TextParagraph">
            <asp:GridView Width="100%" 
                    Font-Names="Arial" Font-Size="12px" runat="server" ID="grdAssess" AutoGenerateColumns="false" PageSize="20" 
                    OnRowDataBound="grdAssess_RowDataBound" AllowSorting="true" CellPadding="2" CellSpacing="0" EmptyDataText="">

                 <Columns>
                  
                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                           <ItemTemplate> 
                            <asp:HyperLink Target="_blank" Visible="false" ID="hlQuestion" runat="server"></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Short Name"> 
                    <ItemTemplate>                                   
                        <asp:Label ID="lblQName" Text='<%# Eval("QuestionShortName") %>' runat="server"></asp:Label>                            
                    </ItemTemplate> 
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Question Text"> 
                    <ItemTemplate>                                   
                        <asp:Label ID="lblQText" Text='<%# Eval("QuestionText") %>' runat="server"></asp:Label>                            
                    </ItemTemplate> 
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="Response"> 
                    <ItemTemplate>                                   
                        <asp:Label ID="lblRText" Text='<%# Eval("ResponseText") %>' runat="server"></asp:Label>                            
                    </ItemTemplate> 
                    </asp:TemplateField>     
                    <asp:TemplateField HeaderText="Comments"> 
                    <ItemTemplate>                                   
                        <asp:Label ID="lblRComments" Text='<%# Eval("ResponseComments") %>' runat="server"></asp:Label>                            
                    </ItemTemplate> 
                    </asp:TemplateField>      
                 </Columns>
                  <HeaderStyle BackColor="Green" ForeColor="White" Font-Names="Arial" />

            </asp:GridView></p>
        </td>
    </tr>
    </table>
            </asp:Panel>
</asp:Content>
