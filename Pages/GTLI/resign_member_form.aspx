<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="resign_member_form.aspx.cs" Inherits="Pages_GTLI_resign_member_form" %>

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

        //Function to print active member
        function PrintActiveMember() {
            //Open hidden div and print

            var printContent = $("#PrintContentActiveMember").html();
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

        //Function Export Active Member to excel
        function ExportExcelActiveMember() {

            $("#ExportExcelActiveMember").btechco_excelexport({
                containerid: "ExportExcelActiveMember"
               , datatype: $datatype.Table
            });
        }

        //Fucntion to check a checkbox
        function Check_Click(objRef) {

            if (objRef.checked) {

                $("#" + objRef.id).parent("td").parent("tr").css("background-color", "Red");

            } else {

                $("#" + objRef.id).parent("td").parent("tr").css("background-color", "white");

            }

            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;

            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }

        //Function for check all checkboxs
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes                    
                        inputList[i].checked = true;

                        $("#" + inputList[i].id).parent("td").parent("tr").css("background-color", "Red");


                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes                    
                        inputList[i].checked = false;
                        $("#" + inputList[i].id).parent("td").parent("tr").css("background-color", "white");
                    }
                }
            }
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
                <li><a href="#tabs-4">Resign Members</a></li>
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
                        <td class="td-left" style="border-right: none; width: 200px;">Policy No.<br />
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
                            <asp:Label ID="lblAgent" runat="server"></asp:Label>
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
                 <%--   <tr>
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
                    </tr>--%>
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
           
            <div id="tabs-4" align="center" class="hideelement">
               
                <input type="button" onclick="PrintActiveMember();" class="hideelement" onmouseover="tooltip.pop(this, 'Print Active Member Table')" style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px"  />
                        &nbsp;
                         <input type="button" onclick="ExportExcelActiveMember();" class="hideelement" onmouseover="tooltip.pop(this, 'Export Excel Active Member Table')" style="background: url('../../App_Themes/functions/download_excel.png') no-repeat; border: none; height: 40px; width: 90px"  />
                <h2 style="color: Black; font-weight: bold;">Group Term Life Insurance: <span style="font-weight: bold">Resign Member</span></h2>
                <br />
                <asp:HiddenField ID="hfSaleID" runat="server" />
                <table width="100%">
                    <tr>
                        <td style="text-align: left">Resign Effective Date:<span style="color: red">*</span>&nbsp;&nbsp;<br />
                            <asp:TextBox ID="txtResignDate" Width="97%" runat="server" TabIndex="6" onkeypress="return false;" CssClass="datepicker" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtResignDate" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="2"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">Option:<br />
                            <asp:DropDownList ID="ddlOption" runat="server" Width="98%" TabIndex="7">
                                <asp:ListItem Value="1">Select From Table</asp:ListItem>
                                <asp:ListItem Value="0">Upload File</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div id="upload" runat="server" width="100%">
                    <table width="100%">
                        <tr>
                            <td style="text-align: left">Plan<span style="color: red">*</span></td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlPlan2" runat="server" Width="98%" TabIndex="8">
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" * "
                                                ControlToValidate="ddlPlan2" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" InitialValue="0" ValidationGroup="2"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" style="text-align: left">Employee Data<span style="color: red">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">

                                <input id="uploadedDoc2" style="width: 30%" runat="server" class="fileupload" type="file" tabindex="9" />

                            </td>
                        </tr>
                    </table>
                </div>
                <table width="100%">
                     <tr>
                        <td width="100%" style="text-align: left">Sale Agent<span style="color: red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" style="text-align: left">
                            <asp:DropDownList ID="ddlSaleAgentReturn" Width="98%" Height="36px" TabIndex="4" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">.</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" * "
                                ControlToValidate="ddlSaleAgentReturn" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" InitialValue="0" ValidationGroup="2"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">

                            <asp:Button ID="btnResignMember" Width="250px" runat="server" Text="Resign Members" TabIndex="10" Height="35px" ValidationGroup="2" OnClick="btnResignMember_Click" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessageResign" ForeColor="Red" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvActiveMember" runat="server" AutoGenerateColumns="False" DataKeyNames="GTLI_Certificate_ID,GTLI_Premium_ID" OnRowDataBound="gvActiveMember_RowDataBound">
                    <Columns>
                         <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="IndentBottom" HeaderStyle-CssClass="IndentBottom">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" runat="server" onclick="Check_Click(this)" />
                                <asp:HiddenField ID="hdfEmpId" runat="server" Value='<%# Bind("GTLI_Certificate_ID")%>' />
                                <asp:HiddenField ID="hdfPremiumID" runat="server" Value='<%# Bind("GTLI_Premium_ID")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="50px">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Certificate_Number" HeaderText="Certificate No.">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Employee_Name" HeaderText="Employee Name">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="GTLI_Plan" HeaderText="Plan">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="60px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MM-yyyy}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Expiry_Date" HeaderText="Expiry Date" DataFormatString="{0:dd-MM-yyyy}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Days" HeaderText="Period (Days)">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="80px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sum_Insured" HeaderText="Sum Insured" DataFormatString="{0:C0}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Life_Premium" HeaderText="Life Premium" DataFormatString="{0:C2}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                         <asp:BoundField DataField="Accidental_100Plus_Premium" HeaderText="100Plus Premium" DataFormatString="{0:C2}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TPD_Premium" HeaderText="TPD Premium" DataFormatString="{0:C2}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DHC_Premium" HeaderText="DHC Premium" DataFormatString="{0:C2}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Total_Premium" HeaderText="Total Premium" DataFormatString="{0:C2}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                       
                    </Columns>

                </asp:GridView>
                <div id="PrintContentActiveMember">
                    <div id="dvPrintActiveMember" style="width: 100%" runat="server" align="center">
                        <br />
                        <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Details</h3>

                        </div>
                </div>
                <div id="ExportExcelActiveMember" style="display:none">
                    <div id="dvExportExcelActiveMember" style="width: 100%" runat="server" align="center">
                        <br />
                        <h3 style="color: Black; font-weight: bold; font-family: Arial">Employee Premium Details</h3>
                                            
                        </div>
                </div>
                <br />

                <br />
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
                    <td class="td-left">Basic Premium
                    </td>
                    <td class="td-center">:
                    </td>
                    <td class="td-right">
                        <asp:Label ID="lblPremiumPayment2" runat="server"></asp:Label>&nbsp; (see details in the following page)
                    </td>
                </tr>
               <%-- <tr>
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
                </tr>--%>
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

