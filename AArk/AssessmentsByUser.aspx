<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssessmentsByUser.aspx.cs" Inherits="AArk.AssessmentsByUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">

    <p style="text-align:center;width:980px" class="TextHeader">
          <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

      <table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px; width:980px"> 
        <tr>
         <td style="text-align:left; vertical-align:bottom"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblUsers"></asp:Label>&nbsp;&nbsp;
           <asp:DropDownList runat="server" ID="ddlUsers"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" /><br /></p>
             <br />
            </td>
        </tr>
        <tr style="text-align:left; vertical-align:top; width:50%">
            <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="False" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="False" /><br /></td>
        </tr>
      

        <tr style="text-align:left; vertical-align:top; width:50%"><td><asp:Label CssClass="TextParagraph" id="lblGridMessageAssessUser" runat="server"></asp:Label></td></tr>
        <tr>
            <td>
            <div id="divPrint" style="align-content:center">    
             <p class="TextParagraph" style="text-align:left">
                <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
                <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="false" Font-Bold="true"></asp:Label><br /> <br />                 
          <asp:GridView ID="grdAssessUser" runat="server" Visible="false" Font-Size="12px"  
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="50"
                PagerSettings-Mode="Numeric" EmptyDataText=""  OnPageIndexChanging="grdAssessUser_PageIndexChanging"  
                OnRowDataBound="grdAssessUser_RowDataBound">
                <Columns> 
                 
                    <asp:TemplateField HeaderText="Last Name" HeaderStyle-Wrap="false">
                        <ItemTemplate>  
                        <asp:Label ID="lblUserID" Text='<%# Eval("UserID") %>' Visible="false" runat="server">
                        </asp:Label>                                  
                        <asp:Label ID="lblLastName" Text='<%# Eval("LastName") %>' runat="server">
                        </asp:Label>    
                        </ItemTemplate>      
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="First Name" HeaderStyle-Wrap="false">
                        <ItemTemplate>                                   
                        <asp:Label ID="lblFirstName" Text='<%# Eval("FirstName") %>' runat="server">
                        </asp:Label>    
                        </ItemTemplate>      
                    </asp:TemplateField>
                   
                      <asp:TemplateField HeaderText="Species" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:HyperLink ID="hlSpeciesResults" Text='<%# Eval("SpeciesDisplayName") %>' runat="server" NavigateUrl=''></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="Country">
                        <ItemTemplate>                                   
                        <asp:Label ID="lblGridCountry" Text='<%# Eval("CountryName") %>' runat="server">
                        </asp:Label>    
                        <asp:Label ID="lblCountry" Visible="false" Text='<%# Eval("CountryID") %>' runat="server">
                        </asp:Label>  
                        </ItemTemplate>  
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Assessment Date" HeaderStyle-Wrap="false">
                        <ItemTemplate>                                   
                        <asp:Label ID="lblDateCreated" Text='<%# Eval("DateCreated") %>' runat="server">
                        </asp:Label>    
                        </ItemTemplate>      
                    </asp:TemplateField>
                 </Columns>
                <PagerSettings PageButtonCount="25" />
                <HeaderStyle BackColor="Green" ForeColor="White" />

            </asp:GridView>
             </asp:Panel></p>  </div>

            </td>

        </tr>


    </table>

</asp:Content>
