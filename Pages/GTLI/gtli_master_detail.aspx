<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="gtli_master_detail.aspx.cs" Inherits="Pages_GTLI_gtli_master_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--Tap--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.7.1.js"></script>
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.tabs.js"></script>

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--print--%>
    <script src="../../Scripts/jquery.print.js"></script>
    <link href="../../App_Themes/gtli_print.css" rel="stylesheet" />

    <%--Export Excel--%>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>

    <ul class="toolbar">
        <li>
            <!-- Button to trigger modal new business form -->
            <input type="button" onclick="PrintPolicy();"  style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
    </ul>
    <style type="text/css">
        @media print {
            .hideelement {
                visibility: hidden;
            }

            #tabs-1 {
                display: none !important;
            }

            #tabs-2 {
                display: none !important;
            }

            #tabs-3 {
                display: none !important;
            }

            #tabs-4 {
                display: none !important;
            }

            #tabs-5 {
                display: none !important;
            }

            #tabs-6 {
                display: none !important;
            }

            #tabs-7 {
                display: none !important;
            }

            #printContent {
                display: block !important;
            }

            #tabs {
                font-size: 10px;
            }
        }

        @media screen {

            #printContent {
                display: none;
            }
        }

        .IndentBottom {
            padding-bottom: 10px;
        }

        .order-list {
            counter-reset: item;
        }

        .list-item {
            display: block;
        }

            .list-item:before {
                content: counters(item, ".") " ";
                counter-increment: item;
            }
    .auto-style1 {
        height: 22px;
    }
    </style>
    <script type="text/javascript">
        //Tap
        $(function () {
            $("#tabs").tabs();
        });

        //Date picker
        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        //Function to print policy
        function PrintPolicy() {
            //Open hidden div and print

            var printContent = $("#printContent").html();
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, '');
            printWindow.document.write(printContent);
            printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/gtli_print.css" type="text/css" />');

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }




	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <br />
    <br />
    <br />
   
        <div id="tabs">
            <ul class="hideelement">
               <li><a href="#tabs-1" style="font-size: 12px;">First Page of Policy <asp:Label ID="lblFirstPagePolicyNo" runat="server"></asp:Label></a></li>
                 <li><a href="#tabs-2" style="font-size: 12px;">Policy In Force Letter No Return</a></li>
                <%--<li><a href="#tabs-3">Policy In Force Letter Return Premium</a></li>--%>
                <li><a href="#tabs-4" style="font-size: 12px;">Policy Schedule</a></li>
                <li><a href="#tabs-5" style="font-size: 12px;">Staff Movement Monthly Adjustment</a></li>
                <li><a href="#tabs-6" style="font-size: 12px;">Policy Master Summary</a></li>
                <li><a href="#tabs-7" style="font-size: 12px;">All Premium Details</a></li>
                
            </ul>
            <div id="tabs-1">

                <table width="96%">
                    <tr>
                        <td>
                            <asp:Image ID="Image3" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right">&nbsp;</td>
                        <td width="10%" style="text-align: right;">
                            &nbsp;</td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 13px">Group Term Life Insurance Policy
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            Policy Number <b><asp:Label ID="lblFirstPagePolicyNumber" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            Cambodia Life Insurance Company Plc.
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            <b>(Hereinafter called "the Company")</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            HEREBY AGREES to insure
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            <b><asp:Label ID="lblFirstPageCompanyName" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            <b>(Hereinafter called "the PolicyHolder")</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px;">
                            Who has submitted the application (a copy is attached hereto) which constitutes as part of this Policy, and has paid the premium to the Company.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px;">
                            In accordance with and subject to the provisions of this Policy, the Company agrees to pay the benefits as provided by this Policy to the person or persons entitled thereto.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px;">
                            IN WITNESS WHEREOF, the Company by the authorized persons has signed on this Policy with the Company stamp at the headquarter of the Company, and issue the Policy on <b><asp:Label ID="lblFirstPageEffectiveDate" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br /><br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px; text-indent:50px;">
                            …………………………………
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px; text-indent:50px;">
                           Tondy Suradiredja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px; text-indent:50px;">
                            Chief Executive Officer
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="tabs-2">

                
                <table width="97%">
                    <tr>
                         <td style="padding-left:50px;padding-right:50px;">
                            <asp:Image ID="Image8" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblSecondPageCreateDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            <asp:Label ID="lblSecondPageContactName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblSecondPageCompanyName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblSecondPageAddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblSecondPageContactPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><br /><br /></td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblSecondPageDearContact" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Thank you for being a valued customer of <b>Cambodia Life Insurance Company Plc. “Cambodian Life”</b>.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <b>“Cambodian Life”</b> is pleased to inform you that your Group Life Insurance Policy is in force under term and condition on <b>Policy No. <asp:Label ID="lblSecondPagePolicyNumber" runat="server" Text=""></asp:Label></b>.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Should you have any further question, please do not hesitate to contact our friendly customer service staff. We are delighted to have you with us and look forward to serving you in the near future.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Sincerely yours,
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            Tondy Suradiredja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Chief Executive Officer
                        </td>
                    </tr>
                </table>
                
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <%--<div id="Div5">

                <table width="96%">
                    <tr>
                        <td>
                            <asp:Image ID="Image5" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right">Printed on:</td>
                        <td width="10%" style="text-align: right;">
                            <asp:Label ID="Label33" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 18px">CAMBODIA LIFE INSURANCE COMPANY Plc.
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 18px">
                            <asp:Label ID="Label34" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <table cellpadding="0" cellspacing="0" width="97%" align="center" border="1">
                    <tr>
                        <td class="td-left" style="border-right: none; width:200px;">Policy No.<br />
                            Policy Created Date
                                                     
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :
                            
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label35" runat="server"></asp:Label><br />
                            <asp:Label ID="Label36" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">The Insured's Name<br />
                            Type of Business<br />
                            Contact Person<br />
                            Phone Contact<br />
                            Email<br />
                            Address
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label37" runat="server"></asp:Label><br />
                            <asp:Label ID="Label38" runat="server"></asp:Label><br />
                            <asp:Label ID="Label39" runat="server"></asp:Label><br />
                            <asp:Label ID="Label40" runat="server"></asp:Label><br />
                            <asp:Label ID="Label41" runat="server"></asp:Label><br />
                            <asp:Label ID="Label42" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left" style="border-right: none;">Sum Insured
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label43" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Plan
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <div id="Div6" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Basic Premium
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label44" runat="server"></asp:Label>&nbsp; (see details in the following page)
                        </td>
                    </tr>
                     <tr>
                        <td class="td-left" style="border-right: none;">Premium Discount
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label45" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Premium After Discount
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label46" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Payment Mode
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">Annual
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Coverage Period
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">1 year (extendable)
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left" style="border-right: none;">Policy Effective Date
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label47" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Policy Expiry Date
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label48" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="Div7">

                <table width="96%">
                    <tr>
                        <td>
                            <asp:Image ID="Image6" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right">Printed on:</td>
                        <td width="10%" style="text-align: right;">
                            <asp:Label ID="Label49" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 18px">CAMBODIA LIFE INSURANCE COMPANY Plc.
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 18px">
                            <asp:Label ID="Label50" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <table cellpadding="0" cellspacing="0" width="97%" align="center" border="1">
                    <tr>
                        <td class="td-left" style="border-right: none; width:200px;">Policy No.<br />
                            Policy Created Date
                                                     
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :
                            
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label51" runat="server"></asp:Label><br />
                            <asp:Label ID="Label52" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">The Insured's Name<br />
                            Type of Business<br />
                            Contact Person<br />
                            Phone Contact<br />
                            Email<br />
                            Address
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label53" runat="server"></asp:Label><br />
                            <asp:Label ID="Label54" runat="server"></asp:Label><br />
                            <asp:Label ID="Label55" runat="server"></asp:Label><br />
                            <asp:Label ID="Label56" runat="server"></asp:Label><br />
                            <asp:Label ID="Label57" runat="server"></asp:Label><br />
                            <asp:Label ID="Label58" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left" style="border-right: none;">Sum Insured
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label59" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Plan
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <div id="Div8" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Basic Premium
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label60" runat="server"></asp:Label>&nbsp; (see details in the following page)
                        </td>
                    </tr>
                     <tr>
                        <td class="td-left" style="border-right: none;">Premium Discount
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label61" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Premium After Discount
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label62" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Payment Mode
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">Annual
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Coverage Period
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">1 year (extendable)
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left" style="border-right: none;">Policy Effective Date
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label63" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Policy Expiry Date
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="Label64" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>--%>
            <div id="tabs-4">

                <table width="96%">
                    <tr>
                        <td style="padding-left:50px;padding-right:50px;">
                            <asp:Image ID="Image7" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right"></td>
                        <td width="10%" style="text-align: right;">
                        </td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 18px">Policy Schedule
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 18px">
                            Attaching to and forming part of Group Term Policy No. <asp:Label ID="lblForthPagePolicyNo" runat="server" Text="GL0000011"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Policy Effective Date
                                    </td>
                                    <td>:</td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        <asp:Label ID="lblForthPageEffectiveDate" runat="server" Text="7 October 2015"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Policy Anniversary
                                    </td>
                                    <td>:</td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        <asp:Label ID="lblForthPagePolicyAnnual" runat="server" Text="07 October, Annually"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Members
                                    </td>
                                    <td>:</td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        Full time employees of policyholder
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            Qualification of the Eligible Members:
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            1.     18 - 65 years of age; Group Term Life (GTL)
                            <asp:Label ID="lblProductList" runat="server"></asp:Label>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            2.     Healthy and not on Total and/or Permanent Disability
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            3.    Actively at work rather than ill or on leave and thereby disqualified until recovery from illness or on leave, and having returned to work as usual 
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            4.     Not actively at work for other reasons, the Company preserves the right to consider for eligibility of insurance under this Policy during absence from regular work.
                        </td>
                    </tr>
                    <tr>
                        <td><br /><br /></td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            <table>
                                <tr>
                                    <td style="width:100px;">
                                        <b>Waiting Period:</b> 
                                    </td>
                                    <td>
                                        The Coverage is effective from the date that the Eligible Members has been informed and approved by the Company in writing.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvSumInsured" runat="server" >
                                <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Classification and plan:
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Amount of Insurance/Benefit
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Plan A
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px" class="auto-style1">
                                        1.	Group Term Life Insurance 
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px" class="auto-style1">
                                        <asp:Label ID="lblForthPageSumInsured" runat="server" Text="USD 10,000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        2.	Supplementary Contact: 
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        -	Group Total Permanent Disability
                                    </td>
                                    <td style="text-align: center; font-family: Arial; font-size: 12px">
                                        <asp:Label ID="lblForthPageTPD" runat="server" Text="USD 10,000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        -	Group Daily Hospital Cash (pay 40$ per day maximum to)
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        <asp:Label ID="lblForthPageDHC" runat="server" Text="90 days per year"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;width:230px; font-weight: bold; font-size: 12px">
                                        Changes in Classification to be Effective:
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-right:50px; font-size: 12px">
                                        On policy anniversary of each year or when policyholder asks for changing benefits
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><br /></td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Free cover limit
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : Maximum sum insured without medical examination is <asp:Label ID="lblForthPageFreeCoverLimit" runat="server" Text="USD 10,000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Mode of Payment
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : Annually
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Premium Due Date
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : <asp:Label ID="lblForthPageDueDate" runat="server" Text="07 October"></asp:Label>, Annually
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Policy Issue date
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : <asp:Label ID="lblForthPageIssueDate" runat="server" Text="07 October 2015"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Special condition
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : NO
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>

            <div id="tabs-5">

                
                <table width="97%">
                    <tr>
                         <td style="padding-left:50px;padding-right:50px;">
                            <asp:Image ID="Image6" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold;padding-left:50px;padding-right:50px; font-size: 18px">Endorsement of Policy No. <asp:Label ID="lblFifthPagePolicyNumber" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold;padding-left:50px;padding-right:50px; font-size: 18px">
                            Term and Condition for 
                            <br />Staff Movement during Policy year
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td  style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <table>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        <b>1.</b>
                                    </td>
                                    <td>
                                        <b>Procedure</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        1.1.
                                    </td>
                                    <td>
                                        Policyholder/Authorized person shall inform the Company when there is staff movement by signing on Multi-purpose change form for Group Insurance, and attach with soft copy of employee list as provided by the Company.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        1.2.
                                    </td>
                                    <td>
                                        Policyholder/Authorized person shall inform the Company within 14 days from the employment date or resigning date of each staff.
                                        <table>
                                            <tr>
                                                <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                                    -
                                                </td>
                                                <td>
                                                    The effective date of insurance coverage of additional staff will be automatically counted from the employment date.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                                    -
                                                </td>
                                                <td>
                                                    The effective date of resigning staff and premium returned will be counted from the ending date of each staff.
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        1.3.
                                    </td>
                                    <td>
                                        If Policyholder/ Authorized person inform the Company later than 14 days from the employment or resigning date, the effective date of the new members and/or the returning premium amount of resigning staff will be followed by the date of signing on the Multi-purpose change form as provided by the Company, or the date as agreed by both parties.
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        <b>2.</b>
                                    </td>
                                    <td>
                                        <b>Premium Adjustment</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.1.
                                    </td>
                                    <td>
                                        Premium adjustment for staff movement will be a monthly basis which is calculated by the end of the month. 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.2.
                                    </td>
                                    <td>
                                        Premium for additional staff will be calculated from employment date or from informing date subject to condition in point 1.2 and 1.3.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.3.
                                    </td>
                                    <td>
                                        Returning premium for resigning staff will be calculated from the ending date or from informing date subject to condition in 1.2 and 1.3
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.4.
                                    </td>
                                    <td>
                                        The Policyholder/Authorized person shall make the payment to the Company within 14 days from the date of letter. Any payment later than this period, it will automatically terminate the insurance coverage of the additional staff.
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <br />IN WITNESS THEREOF, the Company executed this endorsement from <asp:Label ID="lblFiftPageCreateDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            Tondy Suradiredja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Chief Executive Officer
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="tabs-6">

                <table width="96%">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right">Printed on:</td>
                        <td width="10%" style="text-align: right;">
                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 18px">CAMBODIA LIFE INSURANCE COMPANY Plc.
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 18px">
                            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <table cellpadding="0" cellspacing="0" width="97%" align="center" border="1">
                    <tr>
                        <td class="td-left" style="border-right: none; width:200px;">Policy No.<br />
                            Policy Created Date
                                                     
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :
                            
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPolicyNumber" runat="server"></asp:Label><br />
                            <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">The Insured's Name<br />
                            Type of Business<br />
                            Contact Person<br />
                            Phone Contact<br />
                            Email<br />
                            Address
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblCompanyName" runat="server"></asp:Label><br />
                            <asp:Label ID="lblTypeOfBusiness" runat="server"></asp:Label><br />
                            <asp:Label ID="lblContactName" runat="server"></asp:Label><br />
                            <asp:Label ID="lblPhone" runat="server"></asp:Label><br />
                            <asp:Label ID="lblEmail" runat="server"></asp:Label><br />
                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left" style="border-right: none;">Sum Insured
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblSumInsured" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Plan
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <div id="dvPlan" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Basic Premium
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumPayment" runat="server"></asp:Label>&nbsp; (see details in the following page)
                        </td>
                    </tr>
                     <tr>
                        <td class="td-left" style="border-right: none;">Premium Discount
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumDiscount" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Premium After Discount
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumAfterDiscount" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Payment Mode
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">Annual
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Coverage Period
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">1 year (extendable)
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left" style="border-right: none;">Policy Effective Date
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblEffectiveDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left" style="border-right: none;">Policy Expiry Date
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblExpiryDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="tabs-7" align="center">
                <br />
                <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Details</h3>
                <br />
                <table id="tblPremiumDetail" runat="server" class="gridtable" style="width: 98%">
                    <tr>
                        <th style="width: 50px">No.</th>
                        <th style="width: 90px;">Certificate No.</th>
                        <th style="width: 170px; text-align: left; padding-left: 5px;">Employee Name</th>
                        <th>Plan</th>
                        <th style="width: 90px;">Effective Date</th>
                        <th style="text-align: center; width: 90px; padding-left: 5px;">Expiry Date</th>
                        <th style="text-align: center; width: 50px; padding-left: 5px;">Period (Days)</th>
                        <th>Life Coverage (USD)</th>
                        <th>Life Premium</th>
                        <th>100Plus Premium</th>
                        <th>TPD Premium</th>
                        <th>DHC Premium</th>
                        <th style="border-right-style: none">Total Premium</th>
                    </tr>
                </table>

            </div>
        </div>
        <div id="printContent">
            <div id="PrintTab1">
                <br /><br /><br />
                <table width="96%">
                    <tr>
                        <td>
                            <asp:Image ID="Image4" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right">&nbsp;</td>
                        <td width="10%" style="text-align: right;">
                            &nbsp;</td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 13px">Group Term Life Insurance Policy
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            Policy Number <b><asp:Label ID="lblPrintFirstPagePolicyNumber" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            Cambodia Life Insurance Company Plc.
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            <b>(Hereinafter called "the Company")</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            HEREBY AGREES to insure
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            <b><asp:Label ID="lblPrintFirstPageCompanyName" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-size: 12px">
                            <b>(Hereinafter called "the PolicyHolder")</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px;">
                            Who has submitted the application (a copy is attached hereto) which constitutes as part of this Policy, and has paid the premium to the Company.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px;">
                            In accordance with and subject to the provisions of this Policy, the Company agrees to pay the benefits as provided by this Policy to the person or persons entitled thereto.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px;">
                            IN WITNESS WHEREOF, the Company by the authorized persons has signed on this Policy with the Company stamp at the headquarter of the Company, and issue the Policy on <b><asp:Label ID="lblPrintFirstPageEffectiveDate" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br /><br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px; text-indent:50px;">
                            …………………………………
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px; text-indent:50px;">
                            Tondy Suradiredja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; font-size: 12px; padding:0px 50px; text-indent:50px;">
                            Chief Executive Officer
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="PrintTab2">

                <br /><br /><br />
                <table width="97%">
                    <tr>
                         <td style="padding-left:50px;padding-right:50px;">
                            <asp:Image ID="Image9" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblPrintSecondPageCreateDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            <asp:Label ID="lblPrintSecondPageContactName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblPrintSecondPageCompanyName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblPrintSecondPageAddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblPrintSecondPageContactPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><br /><br /></td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <asp:Label ID="lblPrintSecondPageDearContact" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Thank you for being a valued customer of <b>Cambodia Life Insurance Company Plc. “Cambodian Life”</b>.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <b>“Cambodian Life”</b> is pleased to inform you that your Group Life Insurance Policy is in force under term and condition on <b>Policy No. <asp:Label ID="lblPrintSecondPagePolicyNumber" runat="server" Text=""></asp:Label></b>.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Should you have any further question, please do not hesitate to contact our friendly customer service staff. We are delighted to have you with us and look forward to serving you in the near future.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Sincerely yours,
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            Tondy Suradiredja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Chief Executive Officer
                        </td>
                    </tr>
                </table>
                
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="PrintTab4">
                <br /><br /><br />
                <table width="96%">
                    <tr>
                        <td style="padding-left:50px;padding-right:50px;">
                            <asp:Image ID="Image5" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td width="30%" height="20px" style="text-align: right"></td>
                        <td width="10%" style="text-align: right;">
                        </td>
                    </tr>
                </table>
                <table width="97%">
                    <tr>
                        <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 18px">Policy Schedule
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 18px">
                            Attaching to and forming part of Group Term Policy No. <asp:Label ID="lblForthPagePrintPolicyNo" runat="server" Text="GL0000011"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Policy Effective Date
                                    </td>
                                    <td>:</td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        <asp:Label ID="lblForthPagePrintEffectiveDate" runat="server" Text="7 October 2015"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Policy Anniversary
                                    </td>
                                    <td>:</td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        <asp:Label ID="lblForthPagePrintPolicyAnnual" runat="server" Text="07 October, Annually"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Members
                                    </td>
                                    <td>:</td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        Full time employees of policyholder
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            Qualification of the Eligible Members:
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            1.     18 - 65 years of age; Group Term Life (GTL)
                             <asp:Label ID="lblPrintProductList" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            2.     Healthy and not on Total and/or Permanent Disability
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            3.    Actively at work rather than ill or on leave and thereby disqualified until recovery from illness or on leave, and having returned to work as usual 
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            4.     Not actively at work for other reasons, the Company preserves the right to consider for eligibility of insurance under this Policy during absence from regular work.
                        </td>
                    </tr>
                    <tr>
                        <td><br /><br /></td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                            <table>
                                <tr>
                                    <td>
                                        <b>Waiting Period:</b> 
                                    </td>
                                    <td>
                                        The Coverage is effective from the date that the Eligible Members has been informed and approved by the Company in writing.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvPrintSumInsured" runat="server" >
                                <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Classification and plan:
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Amount of Insurance/Benefit
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Plan A
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        1.	Group Term Life Insurance 
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        <asp:Label ID="Label1" runat="server" Text="USD 10,000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        2.	Supplementary Contact: 
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        -	Group Total Permanent Disability
                                    </td>
                                    <td style="text-align: center; font-family: Arial; font-size: 12px">
                                        USD 10,000
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        -	Group Daily Hospital Cash (pay 40$ per day maximum to)
                                    </td>
                                    <td style="text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        90 days per year
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;width:230px; font-weight: bold; font-size: 12px">
                                        Changes in Classification to be Effective:
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-right:50px; font-size: 12px">
                                        On policy anniversary of each year or when policyholder asks for changing benefits
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><br /></td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Free cover limit
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : Maximum sum insured without medical examination is <asp:Label ID="lblForthPagePrintFreeCoverLimit" runat="server" Text="USD 10,000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Mode of Payment
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : Annually
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Premium Due Date
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : <asp:Label ID="lblForthPagePrintDueDate" runat="server" Text="07 October"></asp:Label>, Annually
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Policy Issue date
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : <asp:Label ID="lblForthPagePrintIssueDate" runat="server" Text="07 October 2015"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                                        Special condition
                                    </td>
                                    <td style="text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px">
                                        : NO
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>
            <div id="PrintTab5">
                <br /><br /><br />
                <table width="97%">
                    <tr>
                         <td style="padding-left:50px;padding-right:50px;">
                            <asp:Image ID="Image10" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold;padding-left:50px;padding-right:50px; font-size: 18px">Endorsement of Policy No. <asp:Label ID="lblFifthPagePrintPolicyNumber" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold;padding-left:50px;padding-right:50px; font-size: 18px">
                            Term and Condition for 
                            <br />Staff Movement during Policy year
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td  style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <table>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family:  Arial; font-size: 12px">
                                        <b>1.</b>
                                    </td>
                                    <td>
                                        <b>Procedure</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        1.1.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        Policyholder/Authorized person shall inform the Company when there is staff movement by signing on Multi-purpose change form for Group Insurance, and attach with soft copy of employee list as provided by the Company.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        1.2.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        Policyholder/Authorized person shall inform the Company within 14 days from the employment date or resigning date of each staff.
                                        <table>
                                            <tr>
                                                <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                                    -
                                                </td>
                                                <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                                    The effective date of insurance coverage of additional staff will be automatically counted from the employment date.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                                    -
                                                </td>
                                                <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                                    The effective date of resigning staff and premium returned will be counted from the ending date of each staff.
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        1.3.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        If Policyholder/ Authorized person inform the Company later than 14 days from the employment or resigning date, the effective date of the new members and/or the returning premium amount of resigning staff will be followed by the date of signing on the Multi-purpose change form as provided by the Company, or the date as agreed by both parties.
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        <b>2.</b>
                                    </td>
                                    <td>
                                        <b>Premium Adjustment</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.1.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        Premium adjustment for staff movement will be a monthly basis which is calculated by the end of the month. 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.2.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        Premium for additional staff will be calculated from employment date or from informing date subject to condition in point 1.2 and 1.3.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.3.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        Returning premium for resigning staff will be calculated from the ending date or from informing date subject to condition in 1.2 and 1.3
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;vertical-align:top;width:30px; font-family: Arial; font-size: 12px">
                                        2.4.
                                    </td>
                                    <td style="text-align: left;vertical-align:top; font-family: Arial; font-size: 12px">
                                        The Policyholder/Authorized person shall make the payment to the Company within 14 days from the date of letter. Any payment later than this period, it will automatically terminate the insurance coverage of the additional staff.
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            <br />IN WITNESS THEREOF, the Company executed this endorsement from <asp:Label ID="lblFiftPagePrintCreateDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px">
                            Tondy Suradiredja
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-family: Arial; padding-left:50px;padding-right:50px; font-size: 12px">
                            Chief Executive Officer
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <br />
                <div style="display: block; page-break-before: always;"></div>
            </div>



            <div id="PrintTab6">
                <table width="96%">
                    <tr>
                        <td>
                            <asp:Image ID="Image2" ImageUrl="~/App_Themes/images/logo.gif" class="non-pointer" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                         <td width="30%" height="20px" style="text-align: right; font-size: 8pt;">Printed on:</td>
                        <td width="10%" style="text-align: right; font-size: 8pt;"">
                         <asp:Label ID="lblDate2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="96%">
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 16px">CAMBODIA LIFE INSURANCE COMPANY Plc.
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-family: Arial; font-weight: bold; font-size: 16px">
                            <asp:Label ID="lblTitle2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br style="height: 40px;" />
                <table class="table" cellpadding="0" cellspacing="0" width="96%" align="center">
                    <tr>
                        <td class="td-left">Policy No.<br />
                            Policy Created Date
                                                     
                        </td>
                        <td class="td-center">:<br />
                            :
                            
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPolicyNumber2" runat="server"></asp:Label><br />
                            <asp:Label ID="lblCreatedDate2" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">The Insured's Name<br />
                            Type of Business<br />
                            Contact Person<br />
                            Phone Contact<br />
                            Email<br />
                            Address
                        </td>
                        <td class="td-center">:<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                            :<br />
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblCompanyName2" runat="server"></asp:Label><br />
                            <asp:Label ID="lblTypeOfBusiness2" runat="server"></asp:Label><br />
                            <asp:Label ID="lblContactName2" runat="server"></asp:Label><br />
                            <asp:Label ID="lblPhone2" runat="server"></asp:Label><br />
                            <asp:Label ID="lblEmail2" runat="server"></asp:Label><br />
                            <asp:Label ID="lblAddress2" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left">Sum Insured
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblSumInsured2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">Plan
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <div id="dvPlan2" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">Basic Premium
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumPayment2" runat="server"></asp:Label>&nbsp; (see details in the following page)
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">Premium Discount
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumDiscount2" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                   <tr>
                        <td class="td-left">Premium After Discount
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumAfterDiscount2" runat="server"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">Payment Mode
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">Annual
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">Coverage Period
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">1 year (extendable)
                        </td>
                    </tr>

                    <tr>
                        <td class="td-left">Policy Effective Date
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblEffectiveDate2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-left">Policy Expiry Date
                        </td>
                        <td class="td-center">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblExpiryDate2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="display: block; page-break-before: always;"></div>

            </div>
            <div id="dvPrintDetail" runat="server" align="center">
                <br />
                <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Details</h3>

            </div>
        </div>
   
    <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
</asp:Content>

