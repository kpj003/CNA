<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssessmentSearch.aspx.cs" Inherits="AArk.AssessmentSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">

    <p style="text-align: left" class="TextText">
        <asp:Label CssClass="page-header" runat="server" ID="lblPageTitle"></asp:Label>
        <br />
        <br />
        <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent"></asp:Label>

        <br />
        <asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label><br />
    </p>

    <asp:Panel runat="server" ID="pSearch" DefaultButton="btnSearch">
        <table style="align-items: center; align-self: center; text-align: center; align-content: center; border-spacing: 4px">
            <tr>
                <td style="text-align: left; vertical-align: bottom">
                    <p style="position: static; margin-top: 3px; clear: none" class="TextText">
                        <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp;&nbsp;
               <asp:DropDownList runat="server" ID="ddlCountries"></asp:DropDownList>
                    </p>
                    <br />
                </td>

                <td style="text-align: left; vertical-align: bottom">
                    <p style="position: static; margin-top: 3px; clear: none" class="TextText">
                        <asp:Label runat="server" ID="lblSpecies"></asp:Label>&nbsp;&nbsp;
            <asp:TextBox runat="server" ID="txtSpecies" Width="200"></asp:TextBox>
                    </p>
                    <br />
                </td>

                <td style="text-align: left; vertical-align: top">
                    <p style="position: static; margin-top: 3px; clear: none" class="TextText">
                        <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" />
                    </p>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table>
        <tr style="text-align: left; vertical-align: top; width: 50%">
            <td>
                <asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" Visible="False" />
                &nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Visible="False" /><br />
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="lblGridMessage" runat="server" CssClass="TextBold"></asp:Label></td>
        </tr>

        <tr>
            <td>
                <div id="divPrint" style="align-content: center">
                    <p class="TextParagraph" style="text-align: left">
                        <asp:Panel runat="server" HorizontalAlign="Left" ID="Panel1">
                            <br />
                            <asp:Label runat="server" CssClass="TextParagraph" ID="lblReportTitle" Visible="false" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:GridView ID="grdResults" runat="server" Visible="false" Font-Size="12px" AllowSorting="true"
                                AutoGenerateColumns="False" EnableViewState="true" CellPadding="3" CellSpacing="1" AllowPaging="true" PageSize="50"
                                PagerSettings-Mode="Numeric" EmptyDataText="" OnPageIndexChanging="grdResults_PageIndexChanging" OnSorting="grdResults_Sorting"
                                OnRowDataBound="grdResults_RowDataBound">
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
                                            <asp:HyperLink ID="hlSpeciesResults" Text='<%# Eval("SpeciesDisplayName") %>' runat="server" NavigateUrl='AssessmentResults.aspx'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Consolidated" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlConsolidated" Visible="false" runat="server" NavigateUrl='ConsolidatedSpeciesAssessment.aspx'></asp:HyperLink>
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

                                    <asp:TemplateField HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label Visible="false" ID="lblArchive" Text="Archive" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings PageButtonCount="20" />
                                <HeaderStyle BackColor="Green" ForeColor="White" />

                            </asp:GridView>
                        </asp:Panel>
                    </p>
                </div>

            </td>

        </tr>
    </table>
</asp:Content>
