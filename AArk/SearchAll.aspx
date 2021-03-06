﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchAll.aspx.cs" Inherits="AArk.SearchAll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
 

    <p style="text-align:left" class="TextText">
        <font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font>
        <br /><br />
<asp:Label runat="server" CssClass="TextParagraph" ID="lblContent"></asp:Label>

<br /><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br /></p>


<asp:Panel runat="server" ID="pSearch" DefaultButton="btnSearch">
<table style="align-items:center;align-self:center; text-align:center; align-content:center; border-spacing:4px">
    <tr>
         <td style="text-align:left; vertical-align:top; height: 33px;"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:DropDownList runat="server" ID="ddlCountries" Width="300" OnSelectedIndexChanged="ddlCountries_SelectedIndexChanged" ></asp:DropDownList>
             <br /><br />
          <asp:Label runat="server" ID="lblAssessments"></asp:Label>&nbsp;&nbsp;&nbsp;
           <asp:DropDownList runat="server" ID="ddlAssessments"></asp:DropDownList> &nbsp;
           <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" /></p>
     
        </td>
          <td style="text-align:left; vertical-align:top; height: 33px;" ><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           &nbsp;<asp:Label runat="server" ID="lblSpecies"></asp:Label>&nbsp;&nbsp;
        <asp:TextBox runat="server" ID="txtSpecies" Width="200"></asp:TextBox></p>
        <br />
        </td>
        <td style="text-align:left; vertical-align:top; height: 33px;"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
                     &nbsp;<asp:CheckBox TextAlign="Left" runat="server" ID="chkUserResultsOnly" />
              </p>
        </td>
        </tr>
        <tr>
        <td style="text-align:left; vertical-align:top"><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
            &nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;
           </p></td>
        </tr>  
</table>
</asp:Panel>

    <table> 
        <tr style="text-align:left; vertical-align:top; width:50%">
        <td><asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="False" /> &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="False" /><br /></td>
        </tr>
       
         <tr><td><asp:Label id="lblGridMessage" runat="server" CssClass="TextBold"></asp:Label></td></tr>
          <tr>
            <td> 
             <div id="divPrint" style="align-content:center">    
             <p class="TextParagraph" style="text-align:left">
                <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1"><br />  
                <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="false" Font-Bold="true"></asp:Label><br /> <br />                 
                <asp:GridView ID="grdResults" runat="server" Visible="false" HeaderStyle-Font-Names="Arial" Font-Names="Arial" Font-Size="12px" AllowSorting="true" 
                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="50"
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

                    
                    <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblDateAssessed" Text='<%# Eval("DateCreated") %>' runat="server">
                        </asp:Label>                            
                    </ItemTemplate>    
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>                                   
                        <asp:Label ID="lblCurStatus" Text='<%# Eval("CurStatus") %>' runat="server">
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

                    <asp:TemplateField>
                        <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" Runat="server" CommandArgument='<%# Eval("AssessmentID") %>' 
                    OnClientClick="return confirm('Are you sure you want to delete this assessment?');"
                        CommandName="Delete"><asp:Label runat="server" ID="lblDelete"></asp:Label></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="true">
                           <ItemTemplate> 
                            <asp:HyperLink Visible="false" ID="hlArchive" Text="Archive" runat="server" NavigateUrl='SearchAll.aspx'></asp:HyperLink>
                         </ItemTemplate>    
                    </asp:TemplateField>

                </Columns>
                <PagerSettings PageButtonCount="20" />
                <HeaderStyle BackColor="Green" ForeColor="White" Font-Names="Arial" />

            </asp:GridView>
             </asp:Panel></p>  </div>
        </td>

    </tr>
    </table>
</asp:Content>
