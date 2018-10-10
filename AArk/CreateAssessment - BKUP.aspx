<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateAssessment - BKUP.aspx.cs" Inherits="AArk.CreateAssessmentBKUP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
      
     <p style="text-align:center" class="TextHeader">
        <br /><font size="4"><asp:Label runat="server" ID="lblPageTitle"></asp:Label></font></p>
       
        
    <p style="text-align:left" class="TextParagraph"><asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label></p>

     <table style="width:980px" cellpadding="0" cellspacing="0">
        <tr>
         <td colspan="4" style="text-align:left; vertical-align:bottom" ><p style="POSITION: static; MARGIN-TOP: 3px; CLEAR: none" class="TextText">
           <strong><asp:Label Font-Italic="true" runat="server" ID="lblSpeciesDisplayName" CssClass="TextHeader"></asp:Label>&nbsp;&nbsp;
                    <asp:Label runat="server" ID="lblSpeciesAssessmentDisplayName" CssClass="TextHeader"></asp:Label> 
            </strong><br />
            <br />    
            <asp:Label runat="server" ID="lblCountry"></asp:Label>&nbsp;&nbsp;
            <asp:DropDownList runat="server" ID="ddlCountries"></asp:DropDownList></p>
      
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
            <asp:label runat="server" ID="lblEDScoreValue" CssClass="TextParagraph"></asp:label>&nbsp;&nbsp;&nbsp;<br />
  
        <br />
        </td>  
         <td colspan="3" style="text-align:right">
             <img src="images/treefrog.jpg" />
        <br /><br /></td>
    </tr>
         <tr><td style="text-align:center" colspan="7"><asp:Button runat="server" id="btnSave" Text="Save Assessment" OnClick="btnSave_Click"/>
             <br />
             <br />
             </td></tr>
    </table>
    <table style="width:980px; border-width:1" cellpadding="1" cellspacing="0">
    <tr>
        <td style="text-align:center"><p style="align-content:center;text-align:center"> 
            <asp:Button BorderStyle="None" Width="159" ID="Section1" CssClass="Initial" runat="server" OnClick="Section1_Click" /></p></td>
        <td style="text-align:center"><asp:Button BorderStyle="None" Width="123" ID="Section2" CssClass="Initial" runat="server" OnClick="Section2_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None" Width="145" ID="Section3" CssClass="Initial" runat="server" OnClick="Section3_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None" Width="94" ID="Section4" CssClass="Initial" runat="server" OnClick="Section4_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None"  Width="105" ID="Section5" CssClass="Initial" runat="server" OnClick="Section5_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None" Width="79" ID="Section6" CssClass="Initial" runat="server" OnClick="Section6_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None" Width="263" ID="Section7" CssClass="Initial" runat="server" OnClick="Section7_Click" /></td>
     </tr>
   </table>
   <asp:MultiView ID="MainView" runat="server">
    <asp:View ID="vwSection1" runat="server">
    <table style="width:980px">
        <tr>
         <td> <br /><p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS1Header"></asp:Label>
        </td>
            </tr>
            <tr>
                <td style="vertical-align:top">
                    <table style="width: 800px;border:solid;border-width:1px">
                        <tr style="width:50%; border:dashed;border-width:1px">
                            <td style="vertical-align:top" class="TextText">
                                    <asp:Label ID="lblQ1" runat="server" Font-Bold="true"></asp:Label>
                                &nbsp;
                                <asp:DropDownList ID="ddlQ1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQ1_SelectedIndexChanged" style="height: 22px">
                                </asp:DropDownList>
                               </td></tr>
                        <tr><td>
                                <asp:Label ID="lblPA1Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPA1DefinitionValue" runat="server" CssClass="TextParagraph" Width="350px"></asp:Label><br /><br />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                 <asp:Label ID="lblComments1" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextParagraph" >
                                 <asp:TextBox ID="txtComments1" runat="server" TextMode="MultiLine" Width="350px"></asp:TextBox><br />
                                 <br />
                            </td>
                        </tr>
                        <tr style="width:50%;">
                            <td style="vertical-align:top" class="TextText">
                               <asp:Label ID="lblQ2" runat="server" Font-Bold="true"></asp:Label>
                               </td>
                               <td style="vertical-align:top">
                               <asp:DropDownList ID="ddlQ2" runat="server" style="height: 22px">
                                </asp:DropDownList>
                                   <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                 <asp:Label ID="lblComments2" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextParagraph" >
                                 <asp:TextBox ID="txtComments2" runat="server" TextMode="MultiLine" Width="450px"></asp:TextBox><br />
                            </td>
                        </tr>
                         <tr>
                            <td style="vertical-align:top" class="TextText">
                                    <asp:Label ID="lblQ3" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        
                            <td style="vertical-align:top">
                                <asp:DropDownList ID="ddlQ3" runat="server" style="height: 22px">
                                </asp:DropDownList>
                                <br />
                               </td></tr>
                            <tr>
                                <td style="vertical-align:top" class="TextText">
                                     <asp:Label ID="lblComments3" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top" class="TextParagraph" >
                                     <asp:TextBox ID="txtComments3" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox><br />
                                </td>
                            </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                               <asp:Label ID="lblQ4" runat="server" CssClass="TextText" Width="350" Font-Bold="true"></asp:Label>
                               </td>
                               <td style="vertical-align:top">  
                                <asp:DropDownList ID="ddlQ4" runat="server" style="height: 22px">
                                </asp:DropDownList>
                                   <br />
                            </td>
                        </tr>
                        <tr style="width:50%;">
                            <td style="vertical-align:top" class="TextText">
                                    <asp:Label ID="lblComments4" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextParagraph" >
                                    <asp:TextBox ID="txtComments4" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
     </table>
    </asp:View>  
    <asp:View ID="vwSection2" runat="server">
    <table style="width:980px">
        <tr>
        <td> <br /><p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS2Header"></asp:Label></p> 
        </td>
         </tr>   
        <tr>
            <td style="vertical-align:top">
                <table style="width: 900px">
                    <tr>
                    <td style="vertical-align:top" class="TextText">
                        <asp:Label ID="lblQ5" runat="server" Font-Bold="true" Width="650"></asp:Label>
                    </td>
                    <td style="vertical-align:top">
                                <asp:DropDownList ID="ddlQ5" runat="server" style="height: 22px">
                                </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments5" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments5" runat="server"  Width="550px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                      <tr>
                    <td style="vertical-align:top" class="TextText">
                        <asp:Label ID="lblQ6" runat="server" Font-Bold="true" Width="650"></asp:Label>
                    </td>
                    <td style="vertical-align:top">
                                <asp:DropDownList ID="ddlQ6" runat="server" style="height: 22px">

                                </asp:DropDownList>
                    </td>
                    </tr> 
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments6" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments6" runat="server" Width="550px" TextMode="MultiLine"></asp:TextBox>
                        </td> 
                    </tr>
                </table>
            </td></tr>
    </table>
    </asp:View>
    <asp:View ID="vwSection3" runat="server">
    <table style="width:980px">
        <tr>
        <td> 
            <br /><p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS3Header"></asp:Label></p>
        </td>
        </tr>
             <tr>
                <td style="vertical-align:top">
                    <table style="width: 900px">
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                    <asp:Label ID="lblQ7" runat="server" Font-Bold="true" Width="650"></asp:Label>
                            </td>
                            <td style="vertical-align:top">
                                <asp:DropDownList ID="ddlQ7" CssClass="TextParagraph" runat="server" AutoPostBack="True" style="height: 22px" OnSelectedIndexChanged="ddlQ7_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td>
                                <asp:Label ID="lblPA7Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPA7DefinitionValue" runat="server" CssClass="TextParagraph" Width="650"></asp:Label><br /><br />
                            </td>
                        </tr>  
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                 <asp:Label ID="lblComments7" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextParagraph" >
                                 <asp:TextBox ID="txtComments7" runat="server"  Width="550px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                    <asp:Label ID="lblQ8" runat="server" Font-Bold="true" Width="650"></asp:Label><br />
                            </td>
                            <td style="vertical-align:top">
                                <asp:DropDownList ID="ddlQ8" CssClass="TextParagraph" runat="server" style="height: 22px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                 <asp:Label ID="lblComments8" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextParagraph" >
                                 <asp:TextBox ID="txtComments8" runat="server" Width="550px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top" class="TextText">
                                    <asp:Label ID="lblQ9" runat="server" Font-Bold="true" Width="650"></asp:Label>
                            </td>
                            <td style="vertical-align:top">
                                <asp:DropDownList ID="ddlQ9" CssClass="TextParagraph" runat="server" style="height: 22px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments9" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments9" runat="server"  Width="550px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </td>
             </tr>
     </table>
    </asp:View>

    <asp:View ID="vwSection4" runat="server">
    <table style="width:980px">
        <tr>
            <td><br /> <p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS4Header"></asp:Label></p> </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <table style="width: 900px">
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ10" runat="server" Font-Bold="true" Width="650"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ10" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments10" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments10" runat="server"  Width="550px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ11" runat="server" Font-Bold="true" Width="650"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ11" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments11" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments11" runat="server"  Width="550px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ12" runat="server" Font-Bold="true" Width="650"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ12" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments12" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments12" runat="server" Width="550px"  TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
     </table>
    </asp:View>

    <asp:View ID="vwSection5" runat="server">
    <table style="width:980px">
        <tr>
        <td> <br /><p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS5Header"></asp:Label></p>  </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <table style="width: 900px">
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ13" runat="server" Font-Bold="true" Width="650"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ13" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments13" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments13" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ14" runat="server" Font-Bold="true" Width="650"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ14" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments14" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments14" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ15" runat="server" Font-Bold="true" Width="650"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ15" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments15" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments15" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
     </table>
    </asp:View>
    <asp:View ID="vwSection6" runat="server">
    <table style="width:980px">
        <tr>
        <td> <br /><p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS6Header"></asp:Label></p>  </td>
        </tr>
         <tr>
            <td style="vertical-align:top">
                <table style="width: 900px">
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ16" runat="server" Font-Bold="true" Width="750"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ16" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments16" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments16" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
     </table>
    </asp:View> 
    <asp:View ID="vwSection7" runat="server">
    <table style="width: 100%;">
        <tr>
        <td> <br /><p style="text-align:center" class="TextHeader"><asp:Label runat="server" ID="lblS7Header"></asp:Label></p>  </td>
        </tr>
          <tr>
            <td style="vertical-align:top">
                <table style="width: 900px">
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ17" runat="server" Font-Bold="true" Width="750"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ17" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments17" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments17" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ18" runat="server" Font-Bold="true" Width="750"></asp:Label><br /><br />
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ18" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments18" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments18" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ19" runat="server" Font-Bold="true" Width="750"></asp:Label>
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ19" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments19" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments19" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td style="vertical-align:top" class="TextText">
                            <asp:Label ID="lblQ20" runat="server" Font-Bold="true" Width="750"></asp:Label>
                        </td>
                        <td style="vertical-align:top">
                            <asp:DropDownList ID="ddlQ20" CssClass="TextParagraph" runat="server" style="height: 22px">
                            </asp:DropDownList>
                        </td>
                     </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextText">
                             <asp:Label ID="lblComments20" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="TextParagraph" >
                             <asp:TextBox ID="txtComments20" runat="server"   TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

     </table>
    </asp:View>
    </asp:MultiView>
</asp:Content>
