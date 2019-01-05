<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateAssessment.aspx.cs" Inherits="AArk.CreateAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntApp" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <!-- ensure the src attribute in the following script tag points to your JS file -->

    <script type="text/javascript" src="Scripts/jquery-1.5.js"></script>


    <script type="text/javascript">
        msg = '';
        curlanguage = '<%=this.strCurrentLanguage%>';

        if (curlanguage.toUpperCase() == 'SPANISH')
            msg = 'Has hecho cambios en esta página que no has guardado. Si navegas fuera de esta página, perderás los cambios.';
        else
            msg = 'You have made changes on this page that you have not yet saved. If you navigate away from this page, you will lose your unsaved changes.';

        allLoadedValues = new Array();
        $(document).ready(function () {
            allLoadedValues = $.map($("input[type=checkbox], input[type=text], input[type=radio], select, textarea"), function (item, index) {
                temp = new Object();
                temp.ID = $(item).attr("id");
                temp.Type = $(item).attr("type");
                if (temp.Type == "radio" || temp.Type == "checkbox")
                    temp.Value = $(item).attr("checked");
                else
                    //window.alert(temp.ID + ' ' + temp.Type);
                    temp.Value = $(item).val();
                return temp;
            });
        });

        // Create a variable that can be set upon form submission
        var submitFormOkay = false;

        window.onbeforeunload = function () {
            warning = false;
            for (var i = 0; i < allLoadedValues.length; i++) {
                if (allLoadedValues[i].Type == "radio" || allLoadedValues[i].Type == "checkbox") {
                    if ($("#" + allLoadedValues[i].ID).attr("checked") != allLoadedValues[i].Value) {
                        warning = true;
                        break;
                    }
                }
                else {
                    if ($("#" + allLoadedValues[i].ID).val() != allLoadedValues[i].Value) {
                        warning = true;
                        break;
                    }
                }
            }

            if (warning && !submitFormOkay) {
                return msg;
            }
        }
    </script>
    <script type="text/javascript">

        function openNewWin(url) {
            var x = window.open(url, 'iNaturalist');
            //x.focus();
        }
    </script>
    <p style="text-align: center" class="TextHeader">
        <br />
        <asp:Label runat="server" ID="lblPageTitle" CssClass="page-header"></asp:Label>
    </p>


    <p style="text-align: left" class="TextParagraph">
        <asp:Label runat="server" ForeColor="Red" ID="lblError" CssClass="TextError"></asp:Label>
    </p>

    <table style="width: 980px" cellpadding="0" cellspacing="0">
        <tr>
            <td style="text-wrap: avoid; text-align: left; vertical-align: top; width: 40%">
                <p style="position: static; margin-top: 3px; clear: none" class="TextText">
                    <strong>
                        <asp:Label Font-Italic="true" runat="server" ID="lblSpeciesDisplayName" CssClass="TextHeader"></asp:Label>&nbsp;&nbsp;
                    <asp:Label runat="server" ID="lblSpeciesAssessmentDisplayName" CssClass="TextHeader"></asp:Label>
                    </strong>
                    <br />
                    <br />

                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="left">
                                <asp:Label runat="server" Font-Bold="true" ForeColor="Green" ID="lblCountry" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;
                <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlCountries" OnSelectedIndexChanged="ddlCountries_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </p>

            <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblOrder" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;
            <asp:Label runat="server" ID="lblOrderValue" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblFamily" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;
            <asp:Label runat="server" ID="lblFamilyValue" CssClass="TextParagraph"></asp:Label>
                <br />
                <br />
                <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblGlobalRLC" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;
                <asp:Label runat="server" ID="lblGlobalRLCValue" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;&nbsp;<br />
                        <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblNationalRLC" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;
                <asp:Label runat="server" ID="lblNationalRLCValue" CssClass="TextParagraph"></as>&nbsp;&nbsp;&nbsp;<br /></asp:Label>&nbsp;&nbsp;
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCountries" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblDistribution" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" ID="lblDistributionValue" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;<br />
                <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblEDScore" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;
            <asp:Label runat="server" ID="lblEDScoreValue" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;&nbsp;<br />

                <br />
            </td>
            <td style="text-align: center; vertical-align: top; width: 35%">
                <br />
                <a id="hlImage" runat="server" target="_blank">
                    <asp:Image ID="imgAWeb" runat="server" /></a>
                <br />

                <asp:Label ID="lblCopyright" Font-Size="X-Small" CssClass="TextCopy" runat="server"></asp:Label>
                <asp:HyperLink ID="lnkCopyright" Target="_blank" Font-Size="X-Small" CssClass="TextCopy" Visible="false" Text="1" runat="server"></asp:HyperLink>
                <asp:Label ID="lblCopyDummy" Font-Size="X-Small" CssClass="TextCopy" Visible="false" Text=")" runat="server"></asp:Label>

                <br />
                <br />
                <asp:HyperLink ID="lnkAWeb" Target="_blank" CssClass="LinksText" Text="" runat="server"></asp:HyperLink>


            </td>
            <td style="text-align: right; vertical-align: top; width: 25%">
                <p style="text-align: right; align-content: center">
                    <a runat="server" id="lnkRLMap" target="_blank">
                        <br />
                        <asp:ImageMap Height="101" Width="196" runat="server" ID="imgRLMap" /></a>
                    <asp:ImageMap Height="101" Width="196" Visible="false" runat="server" ID="imgRLMapOnly" /><br />
                    <br />
                    <asp:HyperLink CssClass="LinksText" runat="server" Target="_blank" ID="lnkRedList"></asp:HyperLink>
                </p>
            </td>

        </tr>
        <tr>
            <td colspan="3">
                <br />
                <asp:Label Font-Bold="true" ForeColor="Green" runat="server" ID="lblAdditionalComments" CssClass="TextParagraph"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtAdditionalComments" TextMode="MultiLine" CssClass="TextParagraph" Width="90%"></asp:TextBox>&nbsp;&nbsp;<br />
            </td>
        </tr>
        <tr>
            <td style="text-align: center" colspan="7">
                <br />
                <asp:CheckBox ID="chkComplete" runat="server" />&nbsp;&nbsp;
             <asp:Button runat="server" ID="btnSave" OnClientClick="submitFormOkay = true;" OnClick="btnSave_Click" />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label Font-Bold="true" runat="server" ID="lblInstructions" CssClass="TextParagraph"></asp:Label><br />
                <br />
            </td>
        </tr>
    </table>

    <table style="width: 980px; border-width: 1" cellpadding="1" cellspacing="0">
        <%--  <tr>
        <td style="text-align:center"><p style="align-content:center;text-align:center"> 
            <asp:Button BorderStyle="None" Width="159" ID="Section1"  OnClientClick="submitFormOkay = true;" CssClass="Initial" runat="server" OnClick="Section1_Click" /></p></td>
        <td style="text-align:center"><asp:Button BorderStyle="None" OnClientClick="submitFormOkay = true;"  Width="123" ID="Section2" CssClass="Initial" runat="server" OnClick="Section2_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None"  OnClientClick="submitFormOkay = true;" Width="145" ID="Section3" CssClass="Initial" runat="server" OnClick="Section3_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None"  OnClientClick="submitFormOkay = true;" Width="94" ID="Section4" CssClass="Initial" runat="server" OnClick="Section4_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None"   OnClientClick="submitFormOkay = true;" Width="105" ID="Section5" CssClass="Initial" runat="server" OnClick="Section5_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None"  OnClientClick="submitFormOkay = true;" Width="79" ID="Section6" CssClass="Initial" runat="server" OnClick="Section6_Click" /></td>
        <td style="text-align:center"><asp:Button BorderStyle="None"  OnClientClick="submitFormOkay = true;" Width="263" ID="Section7" CssClass="Initial" runat="server" OnClick="Section7_Click" /></td>
     </tr>--%>
        <tr style="white-space: normal" class="create-ass-sections">
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="159" ID="Section1" CssClass="wrap" runat="server" OnClick="Section1_Click" /></td>
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="123" ID="Section2" CssClass="wrap" runat="server" OnClick="Section2_Click" /></td>
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="145" ID="Section3" CssClass="wrap" runat="server" OnClick="Section3_Click" /></td>
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="94" ID="Section4" CssClass="wrap" runat="server" OnClick="Section4_Click" /></td>
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="105" ID="Section5" CssClass="wrap" runat="server" OnClick="Section5_Click" /></td>
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="79" ID="Section6" CssClass="wrap" runat="server" OnClick="Section6_Click" /></td>
            <td rowspan="2" style="text-align: center; text-wrap: normal">
                <asp:Button Height="40px" BorderStyle="None" OnClientClick="submitFormOkay = true;" Width="263" ID="Section7" CssClass="wrap" runat="server" OnClick="Section7_Click" /></td>
        </tr>
    </table>
    <asp:MultiView ID="MainView" runat="server">
        <asp:View ID="vwSection1" runat="server">
            <table style="width: 980px">
                <tr>
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS1Header"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="left">

                                    <table style="width: 900px">
                                        <tr>
                                            <td style="vertical-align: bottom; align-items: baseline" class="TextText">
                                                <asp:Label ID="lblQ1" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ1">
                                       <img id="imgQ1" runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQ1_SelectedIndexChanged" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA1Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA1DefinitionValue" runat="server" CssClass="TextParagraph" Width="950px"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments1" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt1" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments1" runat="server" TextMode="MultiLine" Width="950px"></asp:TextBox><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblQ2" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ2">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ2" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments2" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt2" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments2" runat="server" TextMode="MultiLine" Width="950px"></asp:TextBox>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblQ3" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ3">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList Enabled="false" ID="ddlQ3" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label Visible="false" ID="lblComments3" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt3" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox Visible="false" ID="txtComments3" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblQ4" runat="server" CssClass="TextText" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ4">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ4" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments4" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblPrompt4" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments4" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwSection2" runat="server">
            <table style="width: 980px">
                <tr>
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS2Header"></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="left">

                                    <table style="width: 960px">
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblQ5" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ5">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ5" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments5" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                             <asp:Label ID="lblPrompt5" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments5" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ6" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ6">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ6" runat="server" AutoPostBack="True" Style="height: 22px" OnSelectedIndexChanged="ddlQ6_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA6Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA6DefinitionValue" runat="server" CssClass="TextParagraph" Width="950px"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments6" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                             <asp:Label ID="lblPrompt6" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments6" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwSection3" runat="server">
            <table style="width: 980px">
                <tr>
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS3Header"></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="left">

                                    <table style="width: 900px">
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ7" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ7">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ7" CssClass="TextParagraph" runat="server" AutoPostBack="True" Style="height: 22px"
                                                    OnSelectedIndexChanged="ddlQ7_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA7Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA7DefinitionValue" runat="server" CssClass="TextParagraph" Width="950"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments7" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt7" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments7" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ8" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ8" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments8" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt8" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments8" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ9" runat="server" Font-Bold="true" Width="950"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ9" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments9" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt9" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments9" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="vwSection4" runat="server">
            <table style="width: 980px">
                <tr>
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS4Header"></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <div class="left">

                                    <table style="width: 900px">
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ10" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ10" CssClass="TextParagraph" AutoPostBack="True" runat="server" Style="height: 22px"
                                                    OnSelectedIndexChanged="ddlQ10_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA10Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA10DefinitionValue" runat="server" CssClass="TextParagraph" Width="950"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments10" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt10" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments10" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <br />
                                                <asp:Label ID="lblQ11" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ11" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments11" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt11" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments11" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ12" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ12" CssClass="TextParagraph" AutoPostBack="True" runat="server" Style="height: 22px" OnSelectedIndexChanged="ddlQ12_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA12Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA12DefinitionValue" runat="server" CssClass="TextParagraph" Width="950"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments12" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt12" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments12" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="vwSection5" runat="server">
            <table style="width: 980px">
                <tr>
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS5Header"></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <div class="left">

                                    <table style="width: 900px">
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblQ13" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ13" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments13" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt13" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments13" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ14" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ14" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments14" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt14" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments14" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ15" runat="server" Font-Bold="true"></asp:Label><br />
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ15">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ15" AutoPostBack="true" CssClass="TextParagraph" runat="server" Style="height: 22px" OnSelectedIndexChanged="ddlQ15_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA15Definition" runat="server" CssClass="TextParagraph" Font-Bold="true"></asp:Label>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPA15DefinitionValue" runat="server" CssClass="TextParagraph" Width="950px"></asp:Label><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments15" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt15" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments15" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwSection6" runat="server">
            <table style="width: 980px">
                <tr>
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS6Header"></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <table style="width: 900px">
                            <tr>
                                <td style="vertical-align: top" class="TextText">
                                    <br />
                                    <asp:Label ID="lblQ16" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlQ16" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top" class="TextText">
                                    <asp:Label ID="lblComments16" runat="server" Font-Bold="true"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt16" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top" class="TextParagraph">
                                    <asp:TextBox ID="txtComments16" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
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
                    <td>
                        <br />
                        <p style="text-align: center" class="TextHeader">
                            <asp:Label runat="server" ID="lblS7Header"></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <div class="left">

                                    <table style="width: 950px">
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ17" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ17" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments17" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt17" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments17" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ18" runat="server" Font-Bold="true" Width="950"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ18" CssClass="TextParagraph" runat="server" Style="height: 22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments18" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt18" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments18" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ19" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ19">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ19" AutoPostBack="true" CssClass="TextParagraph" runat="server" Style="height: 22px" OnSelectedIndexChanged="ddlQ19_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments19" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt19" Font-Italic="true" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments19" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <br />
                                                <asp:Label ID="lblQ20" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                   <a runat="server" id="lnkQ20">
                                       <img runat="server" src="images/Question_mark%2025%20px.png" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlQ20" AutoPostBack="true" CssClass="TextParagraph" runat="server" Style="height: 22px" OnSelectedIndexChanged="ddlQ20_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextText">
                                                <asp:Label ID="lblComments20" runat="server" Font-Bold="true"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="lblPrompt20" runat="server" Font-Italic="true" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" class="TextParagraph">
                                                <asp:TextBox ID="txtComments20" runat="server" Width="950px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
