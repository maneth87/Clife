<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="resign_member_transaction_detail.aspx.cs" Inherits="Pages_GTLI_resign_member_transaction_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--Tap--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.7.1.js"></script>
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.tabs.js"></script>
    
    <%--print--%>
    <script src="../../Scripts/jquery.print.js"></script>
    <link href="../../App_Themes/gtli_print.css" rel="stylesheet" />

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

            #printContent {
                display: block !important;
            }
        }

        @media screen {

            #printContent {
                display: none;
            }
        }
    </style>
    <script type="text/javascript">
        //Tap
        $(function () {
            $("#tabs").tabs();
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
            <li><a href="#tabs-1">Policy Transaction Summary</a></li>
            <li><a href="#tabs-2">Premium Details</a></li>
        </ul>
        <div id="tabs-1">
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

            <table cellpadding="0" cellspacing="0" width="100%" align="center" border="1">
                <tr>
                    <td class="td-left" style="border-right: none;">Policy No.<br />                           
                        Payment Code                                      
                    </td>
                    <td class="td-center" style="border-left: none;">:<br />
                        :                          
                    </td>
                    <td class="td-right">
                        <asp:Label ID="lblPolicyNumber" runat="server"></asp:Label><br />
                        <asp:Label ID="lblPaymentCode" runat="server"></asp:Label>
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
                    <td class="td-left" style="border-right: none;">Premium Return
                    </td>
                    <td class="td-center" style="border-left: none;">:
                    </td>
                    <td class="td-right">
                        <asp:Label ID="lblPremiumReturn" runat="server"></asp:Label>&nbsp; (see details in the following page)
                    </td>
                </tr>

                <tr>
                    <td class="td-left" style="border-right: none;">Resign Date
                    </td>
                    <td class="td-center" style="border-left: none;">:
                    </td>
                    <td class="td-right">
                        <asp:Label ID="lblEffectiveDate" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <div style="display: block; page-break-before: always;"></div>
        </div>
        <div id="tabs-2" align="center">
            <br />
            <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Return Details</h3>
            <br />
            <table id="tblPremiumDetail" runat="server" class="gridtable" style="width: 98%">
                <tr>
                    <th style="width: 50px">No.</th>
                    <th style="width: 90px;">Certificate No.</th>
                    <th style="width: 170px; text-align: left; padding-left: 5px;">Employee Name</th>
                    <th>Plan</th>
                    <th style="width: 90px;">Resigning Date</th>
                    <th style="text-align: center; width: 90px; padding-left: 5px;">Expiry Date</th>
                    <th style="text-align: center; width: 70px; padding-left: 5px;">Days not Cover</th>
                    <th>Life Coverage (USD)</th>
                    <th>Life Return Premium</th>
                    <th>100Plus Return Premium</th>
                    <th>TPD Return Premium</th>
                    <th>DHC Return Premium</th>
                    <th style="border-right-style: none">Total Return Premium</th>
                </tr>
            </table>
        </div>
    </div>
    <div id="printContent">
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
                <td width="10%" style="text-align: right; font-size: 8pt;">
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
                    Payment Code                                              
                </td>
                <td class="td-center">:<br />
                    : 
                </td>
                <td class="td-right">
                    <asp:Label ID="lblPolicyNumber2" runat="server"></asp:Label><br />
                    <asp:Label ID="lblPaymentCode2" runat="server"></asp:Label>
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
                    <asp:Label ID="lblContactPerson2" runat="server"></asp:Label><br />
                    <asp:Label ID="lblPhoneContact2" runat="server"></asp:Label><br />
                    <asp:Label ID="lblContactEmail2" runat="server"></asp:Label><br />
                    <asp:Label ID="lblCompanyAddress2" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="td-left">Premium Return
                </td>
                <td class="td-center">:
                </td>
                <td class="td-right">
                    <asp:Label ID="lblPremiumReturn2" runat="server"></asp:Label>&nbsp; (see details in the following page)
                </td>
            </tr>
            <tr>
                <td class="td-left">Resigned Date
                </td>
                <td class="td-center">:
                </td>
                <td class="td-right">
                    <asp:Label ID="lblEffectiveDate2" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div style="display: block; page-break-before: always;"></div>
        <div id="dvPrintDetail" runat="server" align="center">
            <br />
            <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Return Details</h3>

        </div>
    </div>

</asp:Content>

