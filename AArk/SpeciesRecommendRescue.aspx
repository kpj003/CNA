<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SpeciesRecommendRescue.aspx.cs" Inherits="AArk.SpeciesRecommendRescue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
     <script type="text/javascript">
         function CallPrint(strid) {
             var prtContent = document.getElementById(strid);
             prtContent.visible = true;
            var strOldOne = prtContent.innerHTML;
            var WinPrint = window.open('', '', 'left=0,top=0,width=500,height=600,toolbar=1,scrollbars=1,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
        }
    </script>



     <p style="text-align:center;width:980px" class="TextHeader">
          <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

      <table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px; width:980px"> 
        <tr>
         <td style="text-align:left; vertical-align:bottom; text-wrap:none">
             <p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
            <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp;&nbsp;
            <asp:DropDownList runat="server" ID="ddlCountries"></asp:DropDownList>
            &nbsp;&nbsp;
            <asp:CheckBox runat="server" Checked="true" ID="chkFoundersAvail"></asp:CheckBox>
             &nbsp;
             <asp:CheckBox runat="server" Checked="true" ID="chkHabitatReintroAvail"></asp:CheckBox>
             &nbsp;
            <asp:CheckBox runat="server" Checked="true" ID="chkTaxonAnalysisComplete"></asp:CheckBox>
             <br /><br />
            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" />
          </td></tr>
          <tr><td style="text-align:left; vertical-align:bottom; text-wrap:none"> 
            <asp:Label class="TextText" runat="server" ID="lblSortBy"></asp:Label>&nbsp;
            <asp:RadioButtonList ID="rbSort" class="userFormField" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Priority" Selected="True" class="TextParagraph" Value="Priority"></asp:ListItem>
                <asp:ListItem Text="Taxonomic" class="TextParagraph" Value="Taxonomic"></asp:ListItem>
            </asp:RadioButtonList>
              <br />
              </td>
        </tr>
 
        <tr style="text-align:left; vertical-align:top; width:50%">
        <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="False" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="False" /><br /></td>
        </tr>
        <tr style="text-align:left; vertical-align:top; width:50%">
            <td><asp:Label CssClass="TextParagraph" id="lblGridMessageAssessUser" runat="server"></asp:Label></td></tr>
         <tr>
            <td>
            <div id="divPrint" style="align-content:center">    
            <p class="TextParagraph" style="text-align:left">
                <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
                <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="false" Font-Bold="true"></asp:Label><br /> <br /> 
                                    
            <asp:GridView ID="grdAssessUser" runat="server" Visible="false" HeaderStyle-Font-Names="Arial" Font-Names="Arial" Font-Size="12px"  
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="50"
                PagerSettings-Mode="Numeric" EmptyDataText=""  OnPageIndexChanging="grdAssessUser_PageIndexChanging"  
                OnRowDataBound="grdAssessUser_RowDataBound">
                <Columns> 
                    <asp:TemplateField HeaderText="Country">
                        <ItemTemplate>                                   
                        <asp:Label ID="lblGridCountry" Text='<%# Eval("CountryName") %>' runat="server">
                        </asp:Label>    
                        <asp:Label ID="lblCountry" Visible="false" Text='<%# Eval("CountryID") %>' runat="server">
                        </asp:Label>  
                        </ItemTemplate>  
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Species" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:HyperLink ID="hlSpeciesResults" Text='<%# Eval("SpeciesDisplayName") %>' runat="server" NavigateUrl=''></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>

                    
                    <asp:TemplateField HeaderText="Consolidated" HeaderStyle-Wrap="false">
                        <ItemTemplate>                                   
                        <asp:Literal runat="server" ID="ltlRecs"></asp:Literal>  
                        </ItemTemplate>      
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Common Name" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:Label ID="lblCommonName" Text='<%# Eval("EnglishCommonName") %>' runat="server"></asp:Label>
                         </ItemTemplate>    
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Captive Breeding" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:Label ID="lblQ15" Text='<%# Eval("Q15Response") %>' runat="server"></asp:Label>
                         </ItemTemplate>    
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Assessed By" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:Label ID="lblAssessBy" Text='<%# Eval("AssessedBy") %>' runat="server"></asp:Label>
                         </ItemTemplate>    
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Assessed Date" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:Label ID="lblAssessDate" Text='<%# Eval("DateCreatedDate") %>' runat="server"></asp:Label>
                         </ItemTemplate>    
                    </asp:TemplateField>

                 </Columns>
                <PagerSettings PageButtonCount="50" />
                <HeaderStyle BackColor="Green" ForeColor="White" Font-Names="Arial" />

            </asp:GridView>
             </asp:Panel></p>  </div>
           
            </td>

        </tr>


    </table>
</asp:Content>
