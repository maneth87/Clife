<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_payment_schedule.aspx.cs" Inherits="Pages_Business_policy_payment_schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <script src="../../Scripts/print.js"></script>
     <ul class="toolbar">
         <li>
            <input type="button" data-toggle="modal" data-target="#myGetPolicyPaymentScheduleModal" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 100px;" />
            
        </li>      
        <li>
           <input type="button" onclick="SavePaymentSchedule();" style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 100px;" />
        </li>  
        <li>
            <asp:Button runat="server" Visible="false" ID="save" OnClick="save_Click"  style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 100px;" />
        </li>     

    </ul>
    
    <script type="text/javascript">
        //Validate integer
        function ValidateNumber(i)
        {
            if (i.value.length > 0)
            {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        //Get premium
        function GetPremium(discount, index)
        {
            var arr = index.split('col');
            if (discount == "")
            {
                discount = 0;
                $('#Main_txtDiscount_' + arr[0] + "_" + arr[1]).val("0");
                $('#Main_hdfDiscount_' + arr[0] + "_" + arr[1]).val("0");
            }

            var premium = $('#Main_hdfPremium_' + arr[0] + "_7").val();
            var em_premium = $('#Main_hdfEMPremium_' + arr[0] + "_10").val();
            var premium_after_discount = +premium - +discount;
            $('#Main_txtPremiumAfterDiscount_' + arr[0] + "_9").val("$" + premium_after_discount);
            $('#Main_hdfPremiumAfterDiscount_' + arr[0] + "_9").val(premium_after_discount);
            var total_premium = +premium_after_discount + +em_premium;
            $('#Main_txtTotalPremium_' + arr[0] + "_11").val("$" + total_premium);
            $('#Main_hdfTotalPremium_' + arr[0] + "_11").val(total_premium);
        }

        //Get update premium
        function UpdatePremium(discount, index)
        {
            var arr = index.split('col');
            if (discount == "")
            {
                discount = 0;
                $('#Main_txtDiscount_' + arr[0] + "_" + arr[1]).val("0");
                $('#Main_hdfDiscount_' + arr[0] + "_" + arr[1]).val("0");
            }

            var premium = $('#Main_hdfPremium_' + arr[0] + "_5").val();
            var em_premium = $('#Main_hdfEMPremium_' + arr[0] + "_8").val();
            var premium_after_discount = +premium - +discount;

            $('#Main_txtPremiumAfterDiscount_' + arr[0] + "_7").val("$" + premium_after_discount);
            $('#Main_hdfPremiumAfterDiscount_' + arr[0] + "_7").val(premium_after_discount);
            var total_premium = +premium_after_discount + +em_premium;
            $('#Main_txtTotalPremium_' + arr[0] + "_9").val("$" + total_premium);
            $('#Main_hdfTotalPremium_' + arr[0] + "_9").val(total_premium);
        }

        //update policy
        function UpdatePolicyPaymentSchedule(index)
        {
            if (confirm("Are you sure, you want to update policy payment schedule?"))
            {
               // noreload(event);

                var policy_id = $('#Main_hdfPolicy_ID_' + index + "_1").val();
                var product_id = $('#Main_hdfProduct_ID_' + index + "_1").val();
                var pay_mode = $('#Main_hdfPay_Mode_' + index + "_1").val();
                var year = $('#Main_hdfYear_' + index + "_1").val();
                var premium = $('#Main_hdfPremium_' + index + "_5").val();
                var discount = $('#Main_txtDiscount_' + index + "_6").val();
                var premium_afetr_discount = $('#Main_hdfPremiumAfterDiscount_' + index + "_7").val();
                var extra_premium = $('#Main_hdfEMPremium_' + index + "_8").val();
                var total_premium = $('#Main_hdfTotalPremium_' + index + "_9").val();
                var remark = $('#Main_txtRemark_' + index + "_10").val();

                //  alert(policy_id + '/' + product_id + '/' + pay_mode + '/' + year + '/' + premium + '/' + discount + '/' + premium_afetr_discount + '/' +
                //   extra_premium + '/' + total_premium + '/' + remark);

                //alert("{policy_id:'" + policy_id + "',product_id:'" + product_id + "',pay_mode:'" + pay_mode + "',year:'" + year + "',premium:'" + premium + "',discount:'"
                //     + discount + "',premium_after_discount:'" + premium_afetr_discount + "',extra_premium:'" + extra_premium + "',total_premium:'" + total_premium + "',remark:'" + remark + "'}");

                $.ajax({
                    type: "POST",
                    url: '../../PolicyPaymentScheduleWebService.asmx/UpdatePolicyPaymentSchedule',
                    data: "{policy_id:'" + policy_id + "',product_id:'" + product_id + "',pay_mode:'" + pay_mode + "',year:'" + year + "',premium:'" + premium + "',discount:'" + discount + "',premium_after_discount:'" + premium_afetr_discount + "',extra_premium:'" + extra_premium + "',total_premium:'" + total_premium + "',remark:'" + remark + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data)
                    {
                        if (data.d == "1")
                        {
                            message("Update successfull!");
                        }
                        else if (data.d == "2")
                        {
                            message("Update fail, Current year is in using!");
                        }
                        else
                        {
                            message("Update failed. please try again!");
                        }
                    },
                error: function (str) {
                    alert(str);
                }

                });

              noreload(event);
            }
        }
            //no reload
        function noreload(ev)
        {
            // if(ev.preventDefault) ev.preventDefault();
            ev.preventDefault ? ev.preventDefault() : (ev.returnValue = false);
        }
        function message(message)
        {
                window.alert(message);
        }

            //Save paymen schedule
        function SavePaymentSchedule()
        {
            if ($('#Main_hdfPolicyID').val() != "")
            {
                if (confirm('Are you sure, you want to save policy payment schedule?'))
                {
                    var user_name = $('#Main_hdfUserName').val();
                    var policy_id = $('#Main_hdfPolicyID').val();
                    var product_id = $('#Main_hdfProductID').val();
                    var pay_mode = $('#Main_hdfPayMode').val();
                    var sum_insure = $('#Main_hdfSumInsure').val();
                    var rows = $('#Main_hdfRows').val();
                    var due_date = "";
                    var year = "";
                    var time = "";
                    var premium = "";
                    var discount = "";
                    var premium_after_discount = "";
                    var em_premium = "";
                    var total_premium = "";
                    var remark = "";
                    var disPeriod = "";

                    for (var i = 1; i <= rows; i++) {
                        // due_date += $('#Main_hdfDueDate_' + i + "_" + 5).val() + ",";
                        year += $('#Main_hdfYear_' + i + "_" + 2).val() + ",";
                        time += $('#Main_hdfTime_' + i + "_" + 2).val() + ",";
                        premium += $('#Main_hdfPremium_' + i + "_" + 7).val() + ",";
                        discount += $('#Main_txtDiscount_' + i + "_" + 8).val() + ",";
                        premium_after_discount += $('#Main_hdfPremiumAfterDiscount_' + i + "_" + 9).val() + ",";
                        em_premium += $('#Main_hdfEMPremium_' + i + "_" + 10).val() + ",";
                        total_premium += $('#Main_hdfTotalPremium_' + i + "_" + 11).val() + ",";
                        remark += $('#Main_txtRemark_' + i + "_" + 12).val() + ",";
                        disPeriod += $('#Main_ddlDisPeriod_' + i + "_" + 13).val() + ",";
                    }

                    $.ajax({
                        type: "POST",
                        url: "../../PolicyPaymentScheduleWebService.asmx/SavePolicyPaymentSchedule",
                        data: "{policy_id:'" + policy_id + "',product_id:'" + product_id + "',pay_mode:'" + pay_mode + "',sum_insure:'" + sum_insure + "',year:'" + year + "',time:'" + time + "',premium:'" + premium + "',discount:'" + discount + "',premium_after_discount:'" + premium_after_discount + "',em_premium:'" + em_premium + "',total_premium:'" + total_premium + "',remark:'" + remark + "',user_name:'" + user_name + "', disPeriod:'" + disPeriod + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data)
                        {
                            if (data.d == "1")
                            {
                                alert("Save successfull !");
                                $('#Main_hdfPolicyID').val("");
                            }
                            else
                            {
                                alert("Save failed. please try again.");
                                $('#Main_hdfPolicyID').val("");
                            }
                        }
                    });
                }
                else
                {
                    alert("No data for saving.");
                }
            }
      }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Create Policy Payment Schedule</h3>            
        </div>
        <div class="panel-body">
            <%--Content Here--%>
            <div id="dvContent" width="100%" runat="server">
                
            </div>
        </div>
    </div>   

    <!-- Modal Get Policy Payment Schedule Table -->
    <div id="myGetPolicyPaymentScheduleModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalGetPolicyPaymentScheduleHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H3">Create Payment Schedule Table</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">               
                <tr>
                    <td style="vertical-align: middle; width: 120px;">Policy Number:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPolicyNumber" Width="90%" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnCreatePaymentScheduleTable" CssClass="btn btn-primary" Style="height: 27px;" Text="OK" runat="server" OnClick="btnCreatePaymentScheduleTable_Click" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Get Policy Payment Schedule Table-->

    <asp:HiddenField ID="hdfRows" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfPayMode" runat="server" />
    <asp:HiddenField ID="hdfSumInsure" runat="server" />
    <asp:HiddenField ID="hdfPolicyID" runat="server" />
    <asp:HiddenField ID="hdfUserName" runat="server" />

    </asp:Content>

