<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="AArk.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <p style="text-align:center;width:980px" class="TextHeader">
            <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
      <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

    <table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px; width:980px"> 
        <tr>
            <td style="text-align:left; vertical-align:top; width:50%">
                  <asp:label runat="server" ID="lblApproveUserRequest" CssClass="TextHeaders"></asp:label>
                  <br />
            </td>
        </tr>
        <tr style="text-align:left; vertical-align:top"><td>    
             <asp:label runat="server" ID="lblApproveUserTextParagraph" Text="" CssClass="TextPlain"></asp:label>
                  <br /><br /></td></tr>
            
        <tr style="text-align:left; vertical-align:top; width:50%">
            <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="true" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="true" /></td>
        </tr>
  
        <tr style="text-align:left; vertical-align:top; width:50%"><td><asp:Label CssClass="TextParagraph" id="lblGridMessageApproveUser" runat="server"></asp:Label></td></tr>
       
        <tr>
            <td> 
            <div id="divPrint" style="align-content:center">  
             <p class="TextParagraph" style="text-align:left">  
            <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
            <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="true" Font-Bold="true"></asp:Label><br /> <br />                 
            <asp:GridView ID="grdApproveUser" runat="server" Visible="false" Font-Size="12px"  
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="20"
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
                    
                    <asp:TemplateField HeaderText="Organization" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                        <asp:Label ID="lblOrganization" Text='<%# Eval("OrganizationName") %>' runat="server">
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
                    
                    <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>
                     <asp:TemplateField HeaderStyle-Wrap="true">
                         <ItemTemplate>    
                                 <asp:HyperLink ID="hlApprove"  Visible="false" Text="Approve" runat="server" NavigateUrl='FacilitatorDashboard.aspx?Approve=1'></asp:HyperLink>
                             </ItemTemplate>  
                    </asp:TemplateField>  
                     <asp:TemplateField HeaderStyle-Wrap="true">
                           <ItemTemplate> 
                            <asp:HyperLink ID="hlDeny" Text="Deny" runat="server" NavigateUrl='FacilitatorDashboard.aspx?Approve=0'></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>

                 </Columns>
                <PagerSettings PageButtonCount="20" />
                <HeaderStyle BackColor="Green" ForeColor="White" />

            </asp:GridView>
             </asp:Panel></p>  </div><br />
        </td>
    </tr>
         <tr>
            <td style="text-align:left; vertical-align:top; width:50%">
                  <asp:label runat="server" ID="lblApproveCompleteAssessments" CssClass="TextHeaders"></asp:label>
            </td>
        </tr>
        <tr style="text-align:left; vertical-align:top"><td>    
             <asp:label runat="server" ID="lblApproveCompleteAssessmentsParagraph" Text="" CssClass="TextPlain"></asp:label>
             <br />
             <br />
            </td>
        </tr>
               
      
         <tr style="text-align:left; vertical-align:top; width:50%">
             <td><asp:Label CssClass="TextParagraph" id="lblApproveCompleteAssessmentsGridMessage" runat="server"></asp:Label>
                 <br />
             </td>
         </tr>
         <tr style="text-align:left; vertical-align:top; width:50%">
            <td style="text-align:left; vertical-align:top" class="TextText">
                <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp;&nbsp;
                <asp:DropDownList runat="server" ID="ddlCountries"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" />
                <br />
                <asp:RadioButtonList AutoPostBack="true" ID="rbSort" class="userFormField" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSort_SelectedIndexChanged">
                    <asp:ListItem Text="Date" Selected="True" class="TextParagraph" Value="Date"></asp:ListItem>
                    <asp:ListItem Text="Country" class="TextParagraph" Value="Country"></asp:ListItem>
                    <asp:ListItem Text="Assessor" class="TextParagraph" Value="Assessor"></asp:ListItem> 
                </asp:RadioButtonList>
            </td>
         </tr>
          <tr style="text-align:left; vertical-align:top; width:50%">
            <td><asp:Button runat="server" ID="btnPrintApproveCA" OnClick="btnPrintApproveCA_Click" Visible="true" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExportApproveCA" OnClick="btnExportApproveCA_Click" Visible="true" /></td>
        </tr>
  
     
       <tr>
            <td> 
            <div id="divPrint2" style="align-content:center">    
             <p class="TextParagraph" style="text-align:left">
            <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel2"><br />  
            <asp:Label runat="server" CssClass="TextParagraph" ID="lblApproveCATitle" Visible="true" Font-Bold="true"></asp:Label><br /> <br />                 
           <asp:GridView ID="grdApproveCompletedAssess" runat="server" Visible="false" Font-Size="12px" AllowSorting="true" 
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="50"
                PagerSettings-Mode="Numeric" EmptyDataText=""  OnPageIndexChanging="grdApproveCompletedAssess_PageIndexChanging" 
                OnRowDataBound="grdApproveCompletedAssess_RowDataBound" >
                <Columns>  
                  
                       <asp:TemplateField HeaderText="Species" HeaderStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:HyperLink ID="hlSpeciesResults" Text='<%# Eval("SpeciesDisplayName") %>' runat="server"></asp:HyperLink>
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

                    <asp:TemplateField HeaderText="User">
                    <ItemTemplate>                                   
                          <asp:Label ID="lblUserName" Text='<%# Eval("UserName") %>' runat="server"></asp:Label>
                    </ItemTemplate>    
                    </asp:TemplateField>
                       
                    <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>                                   
                          <asp:Label ID="lblEmail" Text='<%# Eval("Email") %>' runat="server"></asp:Label>
                    </ItemTemplate>    
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>                                   
                          <asp:Label ID="lblDate" Text='<%# Eval("DateCreatedDate") %>' runat="server"></asp:Label>
                    </ItemTemplate>    
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>

                     <asp:TemplateField HeaderStyle-Wrap="true">
                         <ItemTemplate>    
                                 <asp:HyperLink ID="hlApprove"  Visible="false" Text="Approve" runat="server" NavigateUrl='FacilitatorDashboard.aspx?ApproveAssessment=1'></asp:HyperLink>
                             </ItemTemplate>  
                    </asp:TemplateField>  
                    
                     <asp:TemplateField HeaderStyle-Wrap="true">
                           <ItemTemplate> 
                            <asp:HyperLink ID="hlDeny" Text="Deny" runat="server" NavigateUrl='FacilitatorDashboard.aspx?ApproveAssessment=0'></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>

                     <asp:TemplateField HeaderStyle-Wrap="true">
                         <ItemTemplate>    
                                 <asp:HyperLink ID="hlEdit" Text="Edit" runat="server" NavigateUrl='FacilitatorDashboard.aspx?EditAssessment=1'></asp:HyperLink>
                             </ItemTemplate>  
                    </asp:TemplateField>  

                </Columns>
                <PagerSettings PageButtonCount="20" />
                <HeaderStyle BackColor="Green" ForeColor="White"  />

            </asp:GridView>
             </asp:Panel></p>  </div><br />
        </td>
    </tr>
 

        <tr>
            <td style="text-align:left; vertical-align:top; width:50%">
                  <asp:label runat="server" ID="lblIncompleteAssessments" CssClass="TextHeaders"></asp:label>
                  <br />
            </td>
        </tr>
        <tr style="text-align:left; vertical-align:top"><td>    
             <asp:label runat="server" ID="lblTextParagraph" Text="" CssClass="TextPlain"></asp:label>
                  <br /><br /></td></tr>

         <tr style="text-align:left; vertical-align:top; width:50%"><td><asp:Label CssClass="TextParagraph" id="lblGridMessage" runat="server"></asp:Label></td></tr>
       
        <tr style="text-align:left; vertical-align:top; width:50%">
            <td><asp:Button runat="server" ID="btnPrintIncomplete" OnClick="btnPrintIncomplete_Click" Visible="true" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExportIncomplete" OnClick="btnExportIncomplete_Click" Visible="true" /></td>
        </tr>
     
        <tr>
            <td> 
             <div id="divPrint3" style="align-content:center">    
             <p class="TextParagraph" style="text-align:left">
            <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel3"><br />  
            <asp:Label runat="server" CssClass="TextParagraph" ID="lblIncompleteTitle" Visible="true" Font-Bold="true"></asp:Label><br /> <br />                 
             <asp:GridView ID="grdResults" runat="server" Visible="false" Font-Size="12px" AllowSorting="true" 
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
                <HeaderStyle BackColor="Green" ForeColor="White" />

            </asp:GridView>
             </asp:Panel></p>  </div><br />
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
