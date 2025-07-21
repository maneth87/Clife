<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="add_member_form.aspx.cs" Inherits="Pages_GTLI_add_member_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
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

            #printContent {
                display: block !important;
            }

            #PrintContentActiveMember {
                display: block;
            }
        }

        @media screen {

            #printContent {
                display: none;
            }

            #PrintContentActiveMember {
                display: none;
            }
        }

        .IndentBottom {
            padding-bottom: 10px;
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


        //Select Change
        function OnSelectedIndexChange() {
            var value = $("#Main_ddlOption").val();

            //Upload File
            if (value == 0) {
                $("#Main_upload").show();
                $("#Main_gvActiveMember").hide();

            }

            //Select From Table
            if (value == 1) {
                $("#Main_upload").hide();
                $("#Main_gvActiveMember").show();

            }

        }

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <br />
    <br />
    <br />
   
        <div id="tabs">
            <ul class="hideelement">
                <li><a href="#tabs-1">Policy Master Summary</a></li>
                <li><a href="#tabs-2">All Premium Details</a></li>
                <li><a href="#tabs-3">Add New Members</a></li>
                
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
                <table cellpadding="0" cellspacing="0" width="97%" align="center" border="1">
                    <tr>
                        <td class="td-left" style="border-right: none;">Policy No.<br />
                            Policy Created Date<br />
                            Agent Name
                                                     
                        </td>
                        <td class="td-center" style="border-left: none;">:<br />
                            :<br />
                            :
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPolicyNumber" runat="server"></asp:Label><br />
                            <asp:Label ID="lblCreatedDate" runat="server"></asp:Label><br />
                            <asp:Label ID="lblAgentName" runat="server"></asp:Label>

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
                        <td class="td-left" style="border-right: none;">Premium Payment
                        </td>
                        <td class="td-center" style="border-left: none;">:
                        </td>
                        <td class="td-right">
                            <asp:Label ID="lblPremiumPayment" runat="server"></asp:Label>&nbsp; (see details in the following page)
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
            <div id="tabs-2" align="center">
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
            <div id="tabs-3" align="center" class="hideelement">
                <br />
                <h2 style="color: Black; font-weight: bold; font-family: Arial">Group Term Life Insurance: <span style="font-weight: bold">Add More Member</span></h2>
                <br />
                <table width="100%">
                    <tr>

                        <td style="text-align: left">Plan<span style="color: red">*</span>
                        </td>

                    </tr>
                    <tr>

                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlPlan" runat="server" Width="98%" TabIndex="1"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rgvPlan" runat="server" ControlToValidate="ddlPlan" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="1" InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">Employee Data:<span style="color: red">*</span></td>
                    </tr>
                    <tr>
                        <td style="text-align: left">

                            <input id="uploadedDoc" runat="server" style="width: 30%" class="fileupload" type="file" tabindex="4" />

                        </td>
                    </tr>
                    <tr>

                        <td style="text-align: left">Effective Date:<span style="color: red">*</span>

                        </td>
                    </tr>
                    <tr>

                        <td style="text-align: left">
                            <asp:TextBox ID="txtEffectiveDate" Width="97%" runat="server" TabIndex="2" CssClass="datepicker" onkeypress="return false;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rgvDateEffective" runat="server" ControlToValidate="txtEffectiveDate" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>

                        <td style="text-align: left">Total Number of Staffs:<span style="color: red">*</span>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtTotalNumberOfStaff" runat="server" Width="97%" Font-Size="10pt" TabIndex="3" MaxLength="5" ToolTip="Input total Number of staff when first time registration and total actual number below 20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorStaffNumber" ValidationGroup="1" runat="server" ErrorMessage=" * "
                                ControlToValidate="txtTotalNumberOfStaff" ForeColor="Red" Font-Size="9pt" Font-Names="Arial"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" style="text-align: left">Sale Agent<span style="color: red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" style="text-align: left">
                            <asp:DropDownList ID="ddlSaleAgent" Width="98%" Height="36px" TabIndex="4" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">.</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorSaleAgent" runat="server" ErrorMessage=" * "
                                ControlToValidate="ddlSaleAgent" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" InitialValue="0" ValidationGroup="1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" style="text-align: left">Discount($)<span style="color: red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                           <asp:TextBox ID="txtDiscountAmount" runat="server" Width="99%" Font-Size="10pt" TabIndex="5" MaxLength="5" Text="0" ToolTip="Specific Plan Discount Amount"></asp:TextBox>

                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * " ValidationGroup="1"
                                ControlToValidate="txtDiscountAmount" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <br />
                            <asp:Button ID="btnAddEmployee" runat="server" Width="20%" Text="Add New Members" TabIndex="6" Height="35px" ValidationGroup="1" OnClick="btnAddEmployee_Click" />

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessageAdd" ForeColor="Red" runat="server"></asp:Label>
                        </td>
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
                        Policy Created Date<br />
                        Agent Name
                                                     
                    </td>
                    <td class="td-center">:<br />
                        :<br />
                        :
                    </td>
                    <td class="td-right">
                        <asp:Label ID="lblPolicyNumber2" runat="server"></asp:Label><br />
                        <asp:Label ID="lblCreatedDate2" runat="server"></asp:Label><br />
                        <asp:Label ID="lblAgentName2" runat="server"></asp:Label>
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
                    <td class="td-left">Premium Payment
                    </td>
                    <td class="td-center">:
                    </td>
                    <td class="td-right">
                        <asp:Label ID="lblPremiumPayment2" runat="server"></asp:Label>&nbsp; (see details in the following page)
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
            <div id="dvPrintDetail" runat="server" align="center">
                <br />
                <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Details</h3>

            </div>
        </div>
   
    <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
</asp:Content>

