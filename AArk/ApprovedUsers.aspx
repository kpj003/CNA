<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApprovedUsers.aspx.cs" Inherits="AArk.ApprovedUsers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <p style="text-align:center;width:980px" class="TextHeader">
          <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

    <table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px; width:980px"> 
        <tr>
         <td style="text-align:left; vertical-align:bottom"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp;&nbsp;
           <asp:DropDownList runat="server" ID="ddlCountries"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:Label runat="server" ID="lblRoleType"></asp:Label>&nbsp;&nbsp;
           <asp:DropDownList runat="server" ID="ddlRoles"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;

           <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" /><br /></p>
             <br />
            </td>
        </tr>
        <tr style="text-align:left; vertical-align:top; width:50%">
        <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="False" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="False" /><br /></td>
        </tr>
       
        <tr style="text-align:left; vertical-align:top; width:50%"><td><asp:Label CssClass="TextParagraph" id="lblGridMessageApproveUser" runat="server"></asp:Label></td></tr>
        <tr>
            <td>
          <div id="divPrint" style="align-content:center">    
             <p class="TextParagraph" style="text-align:left">
                <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
                <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="false" Font-Bold="true"></asp:Label><br /> <br />                 
               <asp:GridView ID="grdApproveUser" runat="server" Visible="false" HeaderStyle-Font-Names="Arial" Font-Names="Arial" Font-Size="12px"  
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="25"
                PagerSettings-Mode="Numeric" EmptyDataText=""  OnPageIndexChanging="grdApproveUser_PageIndexChanging"  
                OnRowDataBound="grdApproveUser_RowDataBound">
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
                   
                    <asp:TemplateField HeaderText="Email" HeaderStyle-Wrap="false">
                        <ItemTemplate>                                   
                        <asp:Label ID="Email" Text='<%# Eval("Email") %>' runat="server">
                        </asp:Label>    
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
  
                    <asp:TemplateField HeaderText="Organization" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                        <asp:Label ID="lblOrganization" Text='<%# Eval("OrganizationName") %>' runat="server">
                        </asp:Label>    
                        </ItemTemplate>      
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Expertise">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblExpertise" Text='<%# Eval("CountryExpertise") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="ASGMember">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblASGMember" Text='<%# Eval("AmphibianSpecialistGroupMember") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>  
                    
                    <asp:TemplateField HeaderText="User Type">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblUserType" Text='<%# Eval("UserType") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>
                 </Columns>
                <PagerSettings PageButtonCount="25" />
                <HeaderStyle BackColor="Green" ForeColor="White" Font-Names="Arial" />

            </asp:GridView>
             </asp:Panel></p>  </div>

            </td>

        </tr>


    </table>

</asp:Content>
