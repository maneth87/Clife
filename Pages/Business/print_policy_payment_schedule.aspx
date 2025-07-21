<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="print_policy_payment_schedule.aspx.cs" Inherits="Pages_Business_print_policy_payment_schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <%--print--%>
    <script src="../../Scripts/jquery.print.js"></script>
  

    <ul class="toolbar">
        <li>
            <input type="button" data-toggle="modal" data-target="#myGetPolicyPaymentScheduleModal" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 100px;" />
            
        </li>
        <li style="display:none;">
            <input type="button" onclick="PrintPolicyPaymentSchedule();"  style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
    </ul>  
       

    <script type="text/javascript">
        //Get Year
        function GetYear(policy_number) {

            //Clear drop down list
            $('#Main_ddlYear').empty();

            $.ajax({
                type: "POST",
                url: "../../PolicyPaymentScheduleWebService.asmx/GetYear",
                data: "{policy_number:'" + policy_number + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                   
                    //jtemplate populate years
                    $('#Main_ddlYear').setTemplate($("#jTemplateYear").html());
                    $('#Main_ddlYear').processTemplate(data);

                    //if have data enable/disable year button
                    if (data.d.length > 0) {
                        //Enable
                        $('#btnSearch').prop("disabled", false);
                        $('#btnPrint').prop("disabled", false);
                        $('#<%=hdfSelectYear.ClientID%>').val(0);
                    } else {
                        //Disable
                        $('#btnSearch').prop("disabled", true);
                        $('#btnPrint').prop("disabled", true);
                    }
                }

            });
        }

        //Search
        function Search() {
            var btnSeach = document.getElementById('<%= btnSearchPaymentSchedule.ClientID %>'); //dynamically click button

            btnSeach.click();
        }

        //open new print page
        function print()
        {
            var btn_print = $('#<%=btn_Print.ClientID%>');
            btn_print.click();

        }

        //Select Year
        function GetSelectYear(year) {
            if (year == '') {
                $('#Main_hdfSelectYear').val(0);
            }
            else {
                $('#Main_hdfSelectYear').val(year);
            }
            
        }

        //Function to print policy
        function PrintPolicyPaymentSchedule() {
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
            //save printed records
            savePrintedRecord();
            return false;
        }

        function noreload(ev) {
            // if(ev.preventDefault) ev.preventDefault();
            ev.preventDefault ? ev.preventDefault() : (ev.returnValue = false);
        }

        //maneth save printed records
        function savePrintedRecord()
        {
            var user_name = $('#<%=hdfUserName.ClientID%>').val();
            var report_type = 'PolicyPaymentSchedule';
            var policy_number = $('#<%=txtPolicyNumber.ClientID%>').val();
            $.ajax(
                {
                    type: "POST",
                    url: "../../PolicyWebService.asmx/insertPrintedRecord",
                    data: "{policy_number:'" + policy_number + "', printed_by:'" + user_name + "', report_type:'" + report_type+ "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (!data.d)
                        {
                            alert('System can print the report, but cannot save the records which you printed out.');
                        }
                        
                    }
            });
        }
    </script>
    <script id="jTemplateYear" type="text/html">
        <option value="0">.</option>
        {#foreach $T.d as record}          
              <option value='{ $T.record.Year }'>{ $T.record.Year }</option>
        {#/for}
           
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
      <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>

    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Search Policy Payment Schedule</h3>            
        </div>
        <div id="printContent" class="panel-body">
            <%--Content Here--%>
            <div id="dvLogo" runat="server" style="text-align:left; display:none;">
                  <asp:Image ID="imgLogo" runat="server" ImageUrl="~/App_Themes/images/logo.gif" Width="120px" Height="80px" />
            </div>
            <div id="dvTitle" runat="server" style="text-align:center; display:none;">
               
                  <h2>Payment Schedule</h2>
                  <asp:Label ID="lblYear" runat="server"></asp:Label>
            </div>          
            <br />
            <div id="dvDetail" runat="server" style="text-align:left; display:none;">
                <table style="font-size:10px">
                    <tr>
                        <td style="width:140px;">Insurance Plan</td>
                        <td>
                            :&nbsp;<asp:Label ID="lblInsurancePlan" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td>Policy No.</td>
                        <td>
                            :&nbsp;<asp:Label ID="lblPolicyNumber" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td>Insured Name</td>
                        <td>
                            :&nbsp;<asp:Label ID="lblInsuredName" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td>Sum Insured</td>
                        <td>
                            :&nbsp;USD&nbsp;<asp:Label ID="lblSumInsure" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Effective Date:</td>
                        <td>
                            :&nbsp;<asp:Label ID="lblEffectiveDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Mode Of Pmt</td>
                        <td>
                            :&nbsp;<asp:Label ID="lblPayMode" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="dvContent" width="100%" runat="server" border="1">
                
            </div>
           <%-- <br />--%>
            <div id="dvSignature" runat="server" style="text-align:left; display:none;">
                <h5>Prepared By</h5>
             <%--   <br />--%>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="lblName" runat="server"></asp:Label><br />
                <asp:Label ID="lblPosition" runat="server" style=" font-weight:bold;"></asp:Label>
                <br /><br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAgentName" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tel:&nbsp;&nbsp;<asp:Label ID="lblAgentTelNumber" runat="server"></asp:Label>
            </div>
        </div>
    </div>   

     <!-- Modal Get Policy Payment Schedule Table -->
    <div id="myGetPolicyPaymentScheduleModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalGetPolicyPaymentScheduleHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H3">Search Policy Payment Schedule</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">               
                <tr>
                    <td style="vertical-align: middle; width: 110px;">Policy Number:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPolicyNumber" Width="90%" runat="server" MaxLength="30" onkeyup="GetYear(this.value);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Year:</td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" onchange="GetSelectYear(this.value);">                            
                        </asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td>Report Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlReportType" runat="server" >
                            <asp:ListItem Selected="True" Text="1" Value="1"></asp:ListItem>       
                             <asp:ListItem Text="2" Value="2"></asp:ListItem>                      
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">           
            <input type="submit"  id="btnSearch" class="btn btn-primary" onclick="Search(); noreload(event);"  disabled="disabled" style="height: 27px;display:none;" value="OK"/>
            <input type="submit"  id="btnPrint" class="btn btn-primary" onclick="print();"   disabled="disabled" style="height: 27px; " value="Print"/>
            <div style="display:none;">
                 <asp:Button ID="btnSearchPaymentSchedule" CssClass="btn btn-primary"  style="height: 27px;" Text="OK" runat="server" OnClick="btnSearchPaymentSchedule_Click"  />
                 <asp:Button runat="server" ID="btn_Print" OnClick="btn_Print_Click" />
            </div>           
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Get Policy Payment Schedule Table-->

    <asp:HiddenField ID="hdfSelectYear" runat="server" Value="0" />
    <asp:HiddenField ID="hdfUserName" runat="server" />
</asp:Content>

