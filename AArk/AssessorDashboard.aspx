<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssessorDashboard.aspx.cs" Inherits="AArk.AssessorDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
      <p style="text-align:center;width:980px" class="TextHeader">
            <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
      <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

    <table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px; width:980px"> 
     <tr>
       
        <td style="text-align:left; vertical-align:top; width:50%">
                  <asp:label runat="server" ID="lblIncompleteAssessments" CssClass="TextHeaders"></asp:label>
                  <br />
             
        </td></tr>
    
         <tr style="text-align:left; vertical-align:top"><td>    
             <asp:label runat="server" ID="lblTextParagraph" Text="" CssClass="TextPlain"></asp:label>
                  <br /></td></tr>
          <tr style="text-align:left; vertical-align:top; width:50%"><td><asp:Label CssClass="TextParagraph" id="lblGridMessage" runat="server"></asp:Label></td></tr>
         
        <tr style="text-align:left; vertical-align:top; width:50%">
            <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="False" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="False" /><br /></td>
        </tr>
  
      <tr>
            <td> 
             <div id="divPrint" style="align-content:center">    
                <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
                <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="false" Font-Bold="true"></asp:Label><br /> <br />                 
                <asp:GridView ID="grdResults" runat="server" Visible="false" HeaderStyle-Font-Names="Arial" Font-Names="Arial" Font-Size="12px" AllowSorting="true" 
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="25"
                PagerSettings-Mode="Numeric" EmptyDataText=""  OnPageIndexChanging="grdResults_PageIndexChanging" OnSorting="grdResults_Sorting" 
                OnRowDataBound="grdResults_RowDataBound" OnRowCommand="grdResults_RowCommand" OnRowDeleting="grdResults_RowDeleting" >
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

                    <asp:TemplateField HeaderText="Common Name">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblCommonName" Text='<%# Eval("EnglishCommonName") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>
                        
                    <asp:TemplateField HeaderText="Local Name">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblLocalCommonName" Text='<%# Eval("LocalCommonName") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="User">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblUserName" Text='<%# Eval("UserName") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>
        
                     <asp:TemplateField HeaderStyle-Wrap="true">
                         <ItemTemplate>    
                                 <asp:HyperLink ID="hlAdd" Text="Add" runat="server" NavigateUrl='CreateAssessment.aspx'></asp:HyperLink>
                             </ItemTemplate>  
                    </asp:TemplateField>  
                     <asp:TemplateField HeaderStyle-Wrap="true">
                           <ItemTemplate> 
                            <asp:HyperLink Visible="false" ID="hlEdit" Text="Edit My Assessment" runat="server" NavigateUrl='EditAssessment.aspx'></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>

                    <asp:TemplateField><ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" Runat="server" CommandArgument='<%# Eval("AssessmentID") %>' 
                    OnClientClick="return confirm('Are you sure you want to delete this assessment?');"
                        CommandName="Delete"><asp:Label runat="server" ID="lblDelete"></asp:Label></asp:LinkButton>
                </ItemTemplate>
                </asp:TemplateField>
                </Columns>
                <PagerSettings PageButtonCount="20" />
                <HeaderStyle BackColor="Green" ForeColor="White" Font-Names="Arial" />

            </asp:GridView>
             </asp:Panel></p>  </div>
        </td>
    </tr>
    <tr>
            <td><p style="text-align:left"> <br />
                <asp:HyperLink ID="hlProfile" runat="server" NavigateUrl='EditUser.aspx' CssClass="LinksText"></asp:HyperLink>
                <br />
                <asp:Label CssClass="TextParagraph" id="lblEditUserInfo" runat="server"></asp:Label>
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
