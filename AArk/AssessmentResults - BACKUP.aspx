<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="AssessmentResults - BACKUP.aspx.cs" Inherits="AArk.AssessmentResultsBK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">

  <link href="styles/StyleSheetMain.css" rel="stylesheet" type="text/css" />
    <!-- Replace the first 3 values with your own API key and the details    -->
    <!-- for the species you want to display.                                -->
    <!-- Set the 4 configuration options according to your needs.            -->
   <script type="text/javascript" src="Scripts/arkive-api-embed.js"></script>
   <script type="text/javascript">
       var arkiveApiKey = 'zNfhPl1qwabNbTwXDteYCVjtxFspLPPC6SYIPr3Almw1',
            arkiveApiSpeciesName = '<%=this.ARKIVESpeciesName%>', // note spaces replaced by %20
            arkiveApiSpeciesId = '', //optional, but recommended
            arkiveApiWidth = 300,
            arkiveApiHeight = 300,
            arkiveApiImages = false, // whether to include thumbnails
            arkiveApiText = false; // whether to include species facts / description
    </script>
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
       
    <p style="text-align:center" class="TextHeader">
        <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
       
        
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br /></p>
    <p style="text-align:left" class="TextParagraph">&nbsp;</p>
    <asp:Panel runat="server" ID="Panel1">
        
            <asp:Label runat="server" CssClass="TextHeader" ID="lblReportTitle" Font-Italic="true" Visible="true" Font-Bold="true"></asp:Label><br />
  <table style="align-items:center;align-self:center; text-align:center; align-content:center; width:980px; border-spacing:4px"> 
    <tr>
       
        <td style="text-wrap:avoid; text-align:left; vertical-align:top; width:45%">
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
  
     
        </td>
        <td style="text-align:left; vertical-align:top; width:35%">
                <br />
                <div style="width:inherit" id="arkiveIframe" ></div>
        </td>
        <td style="text-align:right; vertical-align:top; width:20%">
            <p style="text-align:right; align-content:center">
                  <a runat="server" id="lnkRLMap" target="_blank"><br />
                  <asp:ImageMap Width="100%" Height="130" runat="server" ID="imgRLMap" /></a>
                  <asp:ImageMap Width="100%" Height="130" Visible="false" runat="server" ID="imgRLMapOnly" /><br /><br />
                  <asp:HyperLink CssClass="LinksText" runat="server" Target="_blank" ID="lnkRedList"></asp:HyperLink>
            </p>
        </td>
        <!--
         <td style="text-align:center; vertical-align:top; width:60%">
             <br />
            <p style="text-align:center; align-content:center">
                 <asp:Button runat="server" ID="btnAddObservations" Text="Add Observations" OnClick="btnAddObservations_Click" /><br />
                 <a runat="server" id="lnkINaturalist" target="_blank">
                    <iframe runat="server" id="frameINaturalist"></iframe>
                 </a>

            </p>
         </td>-->
    </tr>
    <tr>
        <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="True" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="True" /></td></tr>
         
      <tr><td colspan="3">
            <p class="TextParagraph">
            <asp:GridView Width="100%" 
                    Font-Names="Arial" Font-Size="12px" runat="server" ID="grdAssess" AutoGenerateColumns="false" PageSize="20" 
                    OnRowDataBound="grdAssess_RowDataBound" AllowSorting="true" CellPadding="2" CellSpacing="0" EmptyDataText="No records found.">

                 <Columns>
                    <asp:HyperLinkField ItemStyle-HorizontalAlign="Center" HeaderText="Question #" DataNavigateUrlFields="QuestionID,ResponseID"
                    DataNavigateUrlFormatString="AssessmentQuestions.aspx?&QuestionID={0}&ResponseID={1}" 
                    DataTextField="QuestionNumber"/>
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

            </asp:GridView>
            </p>
        </td>

    </tr>
    </table>
        </asp:Panel>
</asp:Content>
