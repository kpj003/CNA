<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssessmentOverview.aspx.cs" Inherits="AArk.AssessmentOverview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">  

         <p style="text-align:center" class="TextHeader"><br />
        <font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
                         <br />
                        <p style="text-align:left"><asp:Label Font-Bold="true" runat="server" CssClass="TextParagraph" ID="lblContent1Hdr"></asp:Label></p>   
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent1"></asp:Label></p>
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent2"></asp:Label>
                             <a style="text-decoration:none; color:Green" id="lnk2" runat="server" target="_blank" href="http://www.amphibianark.org/pdf/Ex_Situ_Planning_Workshop_Report.pdf">
                                <asp:Label runat="server" CssClass="TextText" ID="lblContent2Link"></asp:Label></a>
                            <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent2b"></asp:Label>
                        </p>
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent3"></asp:Label></p>
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent4"></asp:Label>
                              <a style="text-decoration:none; color:Green" id="lnk4" runat="server" href="Questions.aspx">
                                  <asp:Label runat="server" CssClass="TextText" ID="lblContent4Link"></asp:Label></a>
                              <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent4b"></asp:Label>
                        </p>
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent5"></asp:Label>
                              <a style="text-decoration:none; color:Green" id="lnk5" runat="server" href="Questions.aspx">
                                  <asp:Label runat="server" CssClass="TextText" ID="lblContent5Link"></asp:Label></a>
                              <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent5b"></asp:Label>
                        </p>
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent6"></asp:Label></p>   
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent7"></asp:Label>
                              <a style="text-decoration:none; color:Green" id="lnk7" runat="server" href="Triggers.aspx">
                                  <asp:Label runat="server" CssClass="TextText" ID="lblContent7Link"></asp:Label></a>
                              <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent7b"></asp:Label>
                              <a style="text-decoration:none; color:Green" id="lnk72" runat="server" target="_blank" href="http://www.amphibianark.org/resources/other-documents/">
                                  <asp:Label runat="server" CssClass="TextText" ID="lblContent7Link2"></asp:Label></a>
                              <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent7c"></asp:Label>
                        </p>
                        <p style="text-align:left"><asp:Label Font-Bold="true" runat="server" CssClass="TextParagraph" ID="lblContent8Hdr"></asp:Label></p>   
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent8"></asp:Label>
                            <a style="text-decoration:none; color:Green" id="lnk8" runat="server" target="_blank" href="http://www.amphibianark.org/about-us/aark-activities/planning-workshops/">
                                  <asp:Label runat="server" CssClass="TextText" ID="lblContent8Link"></asp:Label></a>
                            <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent8b"></asp:Label></p>
                     
                        <p style="text-align:left"><asp:Label Font-Bold="true" runat="server" CssClass="TextParagraph" ID="lblContent9Hdr"></asp:Label></p>   
                        <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent9"></asp:Label></p>
        
        
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent10"></asp:Label></p>
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent11"></asp:Label></p>
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent12"></asp:Label>
                            <a style="text-decoration:none; color:Green" id="lnk12" runat="server" target="_blank" href="http://www.amphibianark.org/tools/program_implementation_tool.htm">
                                  <asp:Label runat="server" CssClass="TextText" ID="lblContent12Link"></asp:Label></a>
                            <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent12B"></asp:Label>
                      </p>
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent13"></asp:Label></p>
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent14"></asp:Label></p>
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent15"></asp:Label>
                                <a style="text-decoration:none; color:Green" id="lnk15" runat="server" target="_blank" href="http://www.amphibianark.org/pdf/AArk-Amphibian-Population-Management-Guidelines.pdf">
                                    <asp:Label runat="server" CssClass="TextText" ID="lblContent15Link"></asp:Label></a>
                                    <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent15B"></asp:Label>
                                <a style="text-decoration:none; color:Green" id="lnk152" runat="server" target="_blank" href="http://www.amphibianark.org/pdf/AArk-Amphibian-Population-Management-Guidelines-%28Espanol%29.pdf">
                                    <asp:Label runat="server" CssClass="TextText" ID="lblContent15Link2"></asp:Label></a>
                                <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent15C"></asp:Label>
                                <a style="text-decoration:none; color:Green" id="lnk153" runat="server" target="_blank" href="http://www.amphibianark.org/tools/Founder%20calculation%20tool.htm">
                                    <asp:Label runat="server" CssClass="TextText" ID="lblContent15Link3"></asp:Label></a>&nbsp;
                                <a style="text-decoration:none; color:Green" id="lnk154" runat="server" target="_blank" href="http://www.amphibianark.org/tools/Founder_calculation_tool_es.htm">
                                    <asp:Label runat="server" CssClass="TextText" ID="lblContent15Link4"></asp:Label></a>
                                    <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent15D"></asp:Label>
                                <a style="text-decoration:none; color:Green" id="lnk155" runat="server" target="_blank" href="http://www.amphibianark.org/tools/AArk%20Founder%20calculation%20tool.xls">
                                    <asp:Label runat="server" CssClass="TextText" ID="lblContent15Link5"></asp:Label></a>
                                <asp:Label runat="server" CssClass="TextParagraph" ID="lblContent15E"></asp:Label>
                                <a style="text-decoration:none; color:Green" id="lnk156" runat="server" target="_blank" href="http://www.amphibianark.org/tools/Founder_calculation_tool_es.htm">
                                    <asp:Label runat="server" CssClass="TextText" ID="lblContent15Link6"></asp:Label></a></p>
                      <p style="text-align:left"><asp:Label runat="server" CssClass="TextParagraph" ID="lblContent16"></asp:Label></p>
                  


</asp:Content>
