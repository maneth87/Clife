<%@ Page Title="Clife | Business => Underwriting" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="underwriting.aspx.cs" Inherits="Pages_Business_underwriting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script type="text/javascript">

        //Check whether user make any selection      
        function CheckSelection(modal_name) {

            var GridView = document.getElementById('<%= gvApplication.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");
            var count = 0;

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    //ResultModal
                    $(modal_name).modal('show');
                    
                    return;
                }
            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }



        //Get details of status of application
        function GetAppStatusDetail(app_register_id, product_id, gender, age, system_sum_insured, system_premium, pay_mode, assure_year) {                                                         

            //Get app status code
            var app_status_code;
            
            $.ajax({
                type: "POST",
                url: "../../UnderwritingWebService.asmx/GetAppStatusCode",
                data: "{app_register_id:'" + app_register_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                   
                    app_status_code = data.d;

                    //alert(app_status_code);

                    if (app_status_code == 'CO') {
                        $.ajax({
                            type: "POST",
                            url: "../../UnderwritingWebService.asmx/GetUWCO",
                            data: "{app_register_id:'" + app_register_id + "',product_id:'" + product_id + "',gender:'" + gender + "',customer_age:'" + age + "',assure_year:'" + assure_year + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                //Set value for CO calculation                
                                $('#Main_txtSumInsuredDisplayCO').val(data.d.System_Sum_Insure);
                                $('#Main_txtPremiumDisplayCO').val(data.d.System_Premium);
                                $('#Main_ddlPayModeDisplayCO').val(data.d.Pay_Mode);
                                $('#Main_ddlEMPercentageDisplayCO').val(data.d.EM_Percent);
                                $('#Main_txtEMRateDisplayCO').val(data.d.EM_Rate);
                                $('#Main_txtEMPremiumDisplayCO').val(data.d.EM_Premium);
                                $('#Main_txtEMAmountDisplayCO').val(data.d.EM_Amount);
                                $('#Main_txtDisplayCONote').val(data.d.Benefit_Note);

                                //txtDisplayCONote

                                //display modal for CO details
                                $('#CODisplayModal').modal('show');

                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });

                    }
                    else if (app_status_code == 'MO') {
                        $.ajax({
                            type: "POST",
                            url: "../../UnderwritingWebService.asmx/GetAppStatusDetails",
                            data: "{app_register_id:'" + app_register_id + "',status_code:'" + app_status_code + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $('#Main_txtNote').val(data.d);
                                $('#ResultModal').modal('show');

                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });

                    }

                    else if (app_status_code == 'AP') {
                        $.ajax({
                            type: "POST",
                            url: "../../UnderwritingWebService.asmx/GetAppStatusDetails",
                            data: "{app_register_id:'" + app_register_id + "',status_code:'" + app_status_code + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $('#Main_txtAPResult').val(data.d);                                
                                $('#APResultModal').modal('show');
                               
                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });
                        
                    }

                  
                },
                error: function (msg) {
                    alert(msg);
                }
            });


        }

        //Fucntion to check only one checkbox and highlight textbox
        function SelectSingleCheckBox(ckb, app_register_id, product_id, system_sum_insured, system_premium, pay_mode, gender, age, status_code, pay_up_to_age, assure_up_to_age, pay_year, assure_year, original_amount, rounded_amount, app_date, birth_date, user_premium, app_number)
        {
            if (app_register_id == '') {//Policy flat rate
                $('#Main_hdfStatusCode').val(status_code);
                $('#<%=hdfAppNumber.ClientID%>').val(app_number);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "../../UnderwritingWebService.asmx/GetAppStatusCode",
                    data: "{app_register_id:'" + app_register_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_hdfStatusCode').val(data.d);

                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
            }



            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                }
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#f5f5f5");

                //Call web service for discount amount
                $.ajax({
                    type: "POST",
                    url: "../../UnderwritingWebService.asmx/GetDiscountAmount",
                    data: "{app_register_id:'" + app_register_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_hdfUserPremiumDiscount').val(data.d);

                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });

             

                //Set value for CO calculation                
                $('#Main_txtSumInsured').val(system_sum_insured);
                $('#Main_txtPremium').val(system_premium);
                $('#Main_ddlPayMode').val(pay_mode);

                //Bind value to Hidden Fields
                $('#Main_hdfProductID').val(product_id);
                $('#Main_hdfGender').val(gender);
                $('#Main_hdfAge').val(age);
                $('#Main_hdfAppRegisterID').val(app_register_id);
                $('#Main_hdfStatusCode').val(status_code);

                $('#Main_hdfPayYear').val(pay_year);
                $('#Main_hdfPayUpToAge').val(pay_up_to_age);
                $('#Main_hdfAssureYear').val(assure_year);
                $('#Main_hdfAssureUpToAge').val(assure_up_to_age);
                $('#Main_hdfSumInsure').val(system_sum_insured);
                $('#Main_hdfSystemPremium').val(system_premium);
                $('#Main_hdfUserPremium').val(user_premium);

 
                
                $('#Main_hdfPayMode').val(pay_mode);
                
                $('#Main_hdfOriginalAmount').val(original_amount);
                $('#Main_hdfRoundedAmount').val(rounded_amount);

                $('#Main_hdfBirthDate').val(birth_date);

                //To be deleted after inserting data
                $('#Main_hdfAppDate').val(app_date);
         

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");
            }
        }

        //Function to handle when user click undo underwrite button
        function CheckUW() {

            var GridView = document.getElementById('<%= gvApplication.ClientID %>');

            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    //ResultModal
                    $('#UndoUnderwriteModal').modal('show');
                    return;
                }
            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');
        }


        //Function to calculate E.M. Premium
        function CalculateEMRate() {

            var product_id = $('#Main_hdfProductID').val();
            var gender = $('#Main_hdfGender').val();
            var customer_age = $('#Main_hdfAge').val();
            var em_percentage = $('#Main_ddlEMPercentage').val();
            var assure_year = $('#Main_hdfAssureYear').val();
            var app_year = new Date($('#Main_hdfAppDate').val()).getFullYear();
            var pay_mode = "";

            if (product_id == 'CL24') {
                pay_mode = $("#Main_ddlPayMode").val();
            } else {
                pay_mode = "";
            }

            if (em_percentage <= 0) {
                $('#Main_txtEMRate').val("0");
                $('#Main_txtEMPremium').val("0");
                $('#Main_txtEMAmount').val("0");
                return;
            }
            

            $.ajax({
                type: "POST",
                url: "../../UnderwritingWebService.asmx/GetEMRate",
                data: "{product_id:'" + product_id + "',gender:'" + gender + "',customer_age:'" + customer_age + "',em_percentage:'" + em_percentage + "',assure_year:'" + assure_year + "',app_year:'" + app_year + "', pay_mode:'" + pay_mode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    //Display rate to textbox
                    $('#Main_txtEMRate').val(data.d);
                    $('#Main_hdfEMRate').val(data.d);
                    

                    //Get year element from app_date to select the right rate calculation formular
                    var year = new Date($('#Main_hdfAppDate').val()).getFullYear();

                    //Calculate E.M. Premium given sum insured for old product 2013
                    var em_premium = (data.d * $('#Main_txtSumInsured').val()) / 100;

                    
                    if (year == 2014 || year == 2015 || year == 2016) {
                        em_premium = (data.d * $('#Main_txtSumInsured').val()) / 1000;                        
                    }
                    else if (product_id == 'PP200') {
                        em_premium = (data.d * $('#Main_txtSumInsured').val()) / 1000;
                    }
                    else if (year == 2013 && product_id == 'MRTA12' || year == 2013 && product_id == 'MRTA24' || year == 2013 && product_id == 'MRTA36') {
                        em_premium = (data.d * $('#Main_txtSumInsured').val()) / 1000;
                    }
                    else {
                        em_premium = (data.d * $('#Main_txtSumInsured').val()) / 1000;
                    }

                                     

                    $('#Main_txtEMPremium').val(em_premium); //Don't round up twice
                    $('#Main_hdfEMPremium').val(em_premium);

                    var pay_mode = $('#Main_ddlPayMode').val();

                    //Calculate E.M. Amount given pay mode
                    // round up
                    em_premium = Math.ceil(em_premium);

                    if (pay_mode == 1) //Annual
                    {
                        $('#Main_txtEMAmount').val(Math.ceil(em_premium));
                        $('#Main_hdfEMAmount').val(Math.ceil(em_premium));
                    }
                    else if (pay_mode == 2) //Semi-annual
                    {
                        if (year == 2014) { //Factor changed in 2014
                            $('#Main_txtEMAmount').val(Math.ceil(em_premium * 0.54));
                            $('#Main_hdfEMAmount').val(Math.ceil(em_premium * 0.54));
                        }

                        else {
                            $('#Main_txtEMAmount').val(Math.ceil(em_premium * 0.52));
                            $('#Main_hdfEMAmount').val(Math.ceil(em_premium * 0.52));
                        }

                    }
                    else if (pay_mode == 3) //Quarterly
                    {
                        $('#Main_txtEMAmount').val(Math.ceil(em_premium * 0.27));
                        $('#Main_hdfEMAmount').val(Math.ceil(em_premium * 0.27));
                    }
                    else if (pay_mode == 4) //Monthly
                    {
                        $('#Main_txtEMAmount').val(Math.ceil(em_premium * 0.09));
                        $('#Main_hdfEMAmount').val(Math.ceil(em_premium * 0.09));
                    }
                    else {
                        $('#Main_txtEMAmount').val(Math.ceil(em_premium));
                        $('#Main_hdfEMAmount').val(Math.ceil(em_premium));
                    }

                },
                error: function (msg) {
                    alert(msg);
                }
            });

        };



        //Format Date in Jtemplate
        function formatJSONDate(jsonDate) {
            var date = eval(jsonDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
            return dateFormat(date, "dd/mm/yyyy");
        }

        //Display date picker
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

        //maneth: show co print form

        function showprint()
        {
            var GridView = document.getElementById('<%= gvApplication.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");
            var count = 0;

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    var status = $('#<%=hdfStatusCode.ClientID%>').val();
                   
                    if (status == 'CO') {

                        $('#coprint').modal('show');

                    }
                    else {
                        alert('Invalid status code.');
                    }
                    return;
                }
            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }
        function print()
        {
            var txtDocNumber = $('#<%=txtDocNumber.ClientID%>').val();
            var txtDocDate = $('#<%=txtDocDate.ClientID%>').val();
            var txtReason = $('#<%=txtReason.ClientID%>').val();
            var DocWriterName = $('#<%=ddlDocWriter.ClientID%>').text();
            var DocWriterPosition = $('#<%=ddlDocWriter.ClientID%>').val();

            if (txtDocNumber != '' && txtDocDate != '' && txtReason != '' && DocWriterName != '.') {
              
                
                var btn = $('#<%=btnCOPrint.ClientID%>');
                btn.click();

                $('#coprint').modal('hide');
               
            }
            else
            {
                $('#message').html('Please input all required fields.');
            }

           
           

        }
    </script>

    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" onmouseover="tooltip.pop(this, 'Refresh Application List')" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>

        <li>
            <%--<asp:ImageButton ID="ImageBtnInforce" data-toggle="modal" data-target="#DateSelectionModal" runat="server" ImageUrl="~/App_Themes/functions/inforce.png" />--%>
            <input type="button" onclick="CheckSelection(DateSelectionModal)" style="background: url('../../App_Themes/functions/inforce.png') no-repeat; border: none; height: 40px; width: 90px;" />
        
        </li>
        
        <li>
            <%--<asp:ImageButton ID="ImageBtnApprove" data-toggle="modal" data-target="#ApproveModal" runat="server" ImageUrl="~/App_Themes/functions/approve.png" />--%>
            <input type="button" onclick="CheckSelection(ApproveModal)" style="background: url('../../App_Themes/functions/approve.png') no-repeat; border: none; height: 40px; width: 90px;" />
        
        </li>

        <li>
            <%--<asp:ImageButton ID="ImageBtnDecline" data-toggle="modal" data-target="#DeclineModal" runat="server" ImageUrl="~/App_Themes/functions/decline.png" />--%>
            <input type="button" onclick="CheckSelection(DeclineModal)" style="background: url('../../App_Themes/functions/decline.png') no-repeat; border: none; height: 40px; width: 90px;" />
        
        </li>

        <li>
            <%--<asp:ImageButton ID="ImgBtnCancel" data-toggle="modal" data-target="#CancelModal" runat="server" ImageUrl="~/App_Themes/functions/cancel.png" />--%>
            <input type="button" onclick="CheckSelection(CancelModal)" style="background: url('../../App_Themes/functions/cancel.png') no-repeat; border: none; height: 40px; width: 90px;" />
        
        </li>

        <li>
            <%--<asp:ImageButton ID="ImageBtnPostpone" data-toggle="modal" data-target="#PostponeModal" runat="server" ImageUrl="~/App_Themes/functions/postpone.png" />--%>
            <input type="button" onclick="CheckSelection(PostponeModal)" style="background: url('../../App_Themes/functions/postpone.png') no-repeat; border: none; height: 40px; width: 90px;" />
        
        </li>

        <li>
            <%--<asp:ImageButton ID="ImageBtnNotTaken" data-toggle="modal" data-target="#NotTakenModal" runat="server" ImageUrl="~/App_Themes/functions/not_taken.png" />--%>
            <input type="button" onclick="CheckSelection(NotTakenModal)" style="background: url('../../App_Themes/functions/not_taken.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>

        <li>
            <%--<asp:ImageButton ID="ImageBtnCounterOffer" data-toggle="modal" data-target="#CounterOfferModal" runat="server" Width="110" Height="24" ImageUrl="~/App_Themes/functions/counter_offer.png" />--%>
            <input type="button" onclick="CheckSelection(CounterOfferModal)" style="background: url('../../App_Themes/functions/counter_offer.png') no-repeat; border: none; height: 40px; width: 110px;" />
        
        </li>

        <li>
            <%--<asp:ImageButton ID="ImageBtnMemo" data-toggle="modal" data-target="#MemoModal" runat="server" ImageUrl="~/App_Themes/functions/memo.png" />--%>
            <input type="button" onclick="CheckSelection(MemoModal)" style="background: url('../../App_Themes/functions/memo.png') no-repeat; border: none; height: 40px; width: 90px;" />  
        </li>

        <li>
            <input type="button" onclick="CheckUW()" style="background: url('../../App_Themes/functions/undo_underwrite.png') no-repeat; border: none; height: 40px; width: 90px;" />            
        </li>
         <li style="border:0;">
             <div style="width:50px;"></div>
        </li>
         <li>
            <input type="button" onclick="showprint();" style="background: url('../../App_Themes/functions/print_co_letter.png') no-repeat; border: none; height: 40px; width: 150px;" />            
        </li>
    </ul>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--Operation here--%>
            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Underwrite Applications</h3>
                </div>
                <div class="panel-body">

                    <asp:GridView ID="gvApplication" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" DataSourceID="AppDatasource" Width="100%" HorizontalAlign="Center" OnRowDataBound="gvApplication_RowDataBound" OnRowCommand="gvApplication_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("App_Register_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("System_Sum_Insure" ) + "\", \"" + Eval("System_Premium" ) + "\", \"" + Eval("Pay_Mode" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Age_Insure" ) + "\", \"" + Eval("Status_Code" ) + "\", \"" + Eval("Pay_Up_To_Age" ) + "\", \"" + Eval("Assure_Up_To_Age" ) + "\", \"" + Eval("Pay_Year" ) + "\", \"" + Eval("Assure_Year" ) + "\" , \"" + Eval("Original_Amount" ) + "\" , \"" + Eval("Rounded_Amount" ) + "\" , \"" + Eval("App_Date" ) + "\" , \"" + Eval("Birth_Date" ) + "\" , \"" + Eval("User_Premium" ) + "\" , \"" + Eval("App_Number" )+ "\");" %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" CssClass="" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Status                         
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn1" ForeColor="#3333ff" Font-Underline="true" ToolTip="Click to view details" runat="server" OnClick='<%# "GetAppStatusDetail(\"" + Eval("App_Register_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Age_Insure" ) + "\", \"" + Eval("System_Sum_Insure" ) + "\", \"" + Eval("System_Premium" ) + "\", \"" + Eval("Pay_Mode" ) + "\", \"" + Eval("Pay_Year" ) + "\");" %>' Text='<%# Eval("App_Register_ID").ToString().ToString().Trim()=="" ? Eval("status_code").ToString() : GetStatusCode(Eval("App_Register_ID").ToString()) %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="70" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="App_Number" HeaderText="Application no." SortExpression="App_Number" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="App_Date" HeaderText="Signature date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="App_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sale_Agent_ID" HeaderText="Agent" SortExpression="Sale_Agent_ID">

                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Product_ID" HeaderText="Product" SortExpression="Product_ID">

                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Age_Insure" HeaderText="Age" SortExpression="Age_Insure" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                
                            </asp:BoundField>

                            <asp:BoundField DataField="Pay_Year" HeaderText="Payment" SortExpression="Pay_Year" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Assure_Year" HeaderText="Coverage" SortExpression="Assure_Year" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="System_Sum_Insure" HeaderText="Sum Insure" SortExpression="System_Sum_Insure" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <%--<asp:BoundField DataField="System_Sum_Insure" HeaderText="Sum Insured" SortExpression="System_Sum_Insure" />
                            <asp:BoundField DataField="Rounded_Amount" HeaderText="Annual Premium" SortExpression="Rounded_Amount" />--%>

                            <asp:BoundField DataField="Rounded_Amount" HeaderText="Annual Premium" SortExpression="Rounded_Amount" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <%--<asp:BoundField DataField="Premium_Discount" HeaderText="Discount" SortExpression="Premium_Discount" />
                            <asp:BoundField DataField="User_Premium" HeaderText="Paid Premium" SortExpression="User_Premium" />--%>

                            <asp:BoundField DataField="System_Premium" HeaderText="System Premium" SortExpression="System_Premium" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="TPD">
                                <ItemTemplate>
                                    <asp:Label ID="lblTPD" runat="server" Text='<%# GetTPD(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ADB">
                                <ItemTemplate>
                                    <asp:Label ID="lblADB" runat="server" Text='<%# GetADB(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="User_Premium" HeaderText="User Premium" SortExpression="User_Premium" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# GetDiscount(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymode" runat="server" Text='<%# GetPayMode(DataBinder.Eval(Container.DataItem, "Pay_Mode").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="Last_Name" HeaderText="Surname" SortExpression="Last_Name">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="First_Name" HeaderText="First name" SortExpression="First_Name">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>

                                    <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>


                            <asp:BoundField DataField="Country_ID" HeaderText="Nationality" SortExpression="Country_ID" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="">
                                 <ItemTemplate>
                                     <asp:LinkButton ID="ViewRiderLink" runat="server" style="color:blue; text-decoration-line:underline;" CommandArgument='<%# Eval("App_Register_ID")%>'
                                         CommandName="Add">Rider</asp:LinkButton>
                                 </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                             </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level" Visible="false">
                                 <ItemTemplate>
                                     
                                     <asp:Label runat="server" ID="lbl_level" Text ='<%# DataBinder.Eval(Container.DataItem, "level").ToString() %>'></asp:Label>
                                 </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                             </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <asp:SqlDataSource ID="AppDatasource_old" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], [Amount], [Original_Amount] FROM [Cv_Basic_App] WHERE [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_Underwriting WHERE Status_Code = 'NT' OR Status_Code = 'DC' OR Status_Code = 'CC' OR Status_Code = 'PP') AND [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_App_Register_Cancel) AND [Original_Amount] > 0 And (App_Register_ID <> App_Number) AND ([Status_Code] IS NULL) OR ([Status_Code] = 'AP') OR ([Status_Code] = 'CO') OR ([Status_Code] = 'MO') ORDER BY [App_Date] DESC"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="AppDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], [Amount], [Original_Amount], [level] FROM [Cv_Basic_App] WHERE [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_Underwriting WHERE Status_Code = 'NT' OR Status_Code = 'DC' OR Status_Code = 'CC' OR Status_Code = 'PP') AND ([level]=1 OR [level] IS NULL) AND [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_App_Register_Cancel) AND [Original_Amount] > 0 And (App_Register_ID <> App_Number) AND ([Status_Code] IS NULL) OR ([Status_Code] = 'AP') OR ([Status_Code] = 'CO') OR ([Status_Code] = 'MO')  ORDER BY [App_Date] DESC"></asp:SqlDataSource>

                    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], [Amount], [Original_Amount] FROM [Cv_Basic_App] WHERE [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_Underwriting WHERE Status_Code = 'NT' OR Status_Code = 'DC') AND [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_App_Register_Cancel) AND [Original_Amount] > 0 And (App_Register_ID <> App_Number) AND ([Status_Code] IS NULL) OR ([Status_Code] = 'AP') OR ([Status_Code] = 'CO') OR ([Status_Code] = 'MO') ORDER BY [App_Date] DESC"></asp:SqlDataSource>--%>


                    <asp:SqlDataSource ID="PaymentModeDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [Ct_Payment_Mode]"></asp:SqlDataSource>
<%--                    <asp:SqlDataSource ID="DocumentWriterDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT N'រិច​ ដារ៉ង់' AS 'Name', N'មន្ដ្រីប្រតិបត្ដិវិភាគហានិភ័យផ្នែកវេជ្ជសាស្រ្ត' as 'Position'"></asp:SqlDataSource>--%>
                                        <asp:SqlDataSource ID="DocumentWriterDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="EXEC SP_GET_UW_DOC_WRITER_BY_LANGUAGE 'KH';"></asp:SqlDataSource>
                </div>
            </div>

            <!-- Modal Inforce -->
            <div id="DateSelectionModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalEffectiveDate" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Policy Effective Date</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>Date:&nbsp;
                                <asp:TextBox ID="txtDateEffectiveDate" Width="350" runat="server" CssClass="datepicker"></asp:TextBox>&nbsp;&nbsp;(DD-MM-YYYY)
                            </td>
                        </tr>

                    </table>
                    <br />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveInforce" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Inforce" OnClick="btnSaveInforce_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Inforce -->

            <!-- Modal Counter Offer -->
            <div id="CounterOfferModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalCO" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Add Counter Offer</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            E.M. Premium
                        </div>
                        <div class="panel-body">

                            <table style="width: 98%; text-align: left;">
                                <tr>
                                    <td width="25%">Sum Insured:
                                    </td>
                                    <td width="75%">
                                        <asp:TextBox ID="txtSumInsured" onchange="CalculateEMRate();" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Premium Mode:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPayMode" runat="server" onchange="CalculateEMRate();" DataSourceID="PaymentModeDatasource" DataTextField="Mode" DataValueField="Pay_Mode_ID" Enabled="false"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Premium:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPremium" Enabled="false" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>E.M. %:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEMPercentage" onchange="CalculateEMRate();" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>75</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>125</asp:ListItem>
                                            <asp:ListItem>150</asp:ListItem>
                                            <asp:ListItem>175</asp:ListItem>
                                            <asp:ListItem>200</asp:ListItem>
                                            <asp:ListItem>225</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                            <asp:ListItem>275</asp:ListItem>
                                            <asp:ListItem>300</asp:ListItem>
                                            <asp:ListItem>325</asp:ListItem>
                                            <asp:ListItem>350</asp:ListItem>
                                            <asp:ListItem>375</asp:ListItem>
                                            <asp:ListItem>400</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>E.M. Rate:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEMRate" Enabled="false" Text="0" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>E.M. Premium:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEMPremium" Enabled="false" Text="0" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hdfEMPremium" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>E.M. Amount:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEMAmount" Text="0" Enabled="false" runat="server"></asp:TextBox>
                                         <asp:HiddenField ID="hdfEMAmount" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Round Amount:
                                    </td>
                                    <td>
                                        <%--<asp:RadioButtonList ID="rbtnlRoundAmount" runat="server" CssClass="radio" AutoPostBack="true"
                                            onselectedindexchanged="ddp_selectIndexchanged">
                                        <asp:ListItem Text="Up" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Down" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>--%>

                                        <asp:DropDownList ID="rbtnlRoundAmount" runat="server" AutoPostBack="true"
                                            onselectedindexchanged="ddp_selectIndexchanged">
                                            <asp:ListItem Text="Up" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Down" Value="1"></asp:ListItem>       
                                        </asp:DropDownList>     
                                         <asp:HiddenField ID="hdfRoundStatusID" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Note:
                <asp:TextBox ID="txtCONote" Width="86%" runat="server"></asp:TextBox><br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSaveCO" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveCO_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Counter Offer -->


             <!--Start Modal Display Counter Offer -->
            <div id="CODisplayModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="DisplayModalCO" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Counter Offer Details</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            E.M. Premium
                        </div>
                        <div class="panel-body">

                            <table style="width: 98%; text-align: left;">
                                <tr>
                                    <td width="25%">Sum Insured:
                                    </td>
                                    <td width="75%">
                                        <asp:TextBox ID="txtSumInsuredDisplayCO" Enabled="false" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Premium Mode:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPayModeDisplayCO" runat="server" Enabled="false" DataSourceID="PaymentModeDatasource" DataTextField="Mode" DataValueField="Pay_Mode_ID"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Premium:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPremiumDisplayCO" Enabled="false" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>E.M. %:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEMPercentageDisplayCO" Enable="false" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>75</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>125</asp:ListItem>
                                            <asp:ListItem>150</asp:ListItem>
                                            <asp:ListItem>175</asp:ListItem>
                                            <asp:ListItem>200</asp:ListItem>
                                            <asp:ListItem>225</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                            <asp:ListItem>275</asp:ListItem>
                                            <asp:ListItem>300</asp:ListItem>
                                            <asp:ListItem>325</asp:ListItem>
                                            <asp:ListItem>350</asp:ListItem>
                                            <asp:ListItem>375</asp:ListItem>
                                            <asp:ListItem>400</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>E.M. Rate:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEMRateDisplayCO" Enabled="false" Text="0" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>E.M. Premium:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEMPremiumDisplayCO" Enabled="false" Text="0" runat="server"></asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>E.M. Amount:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEMAmountDisplayCO" Text="0" Enabled="false" runat="server"></asp:TextBox>                                         
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Note:
                <asp:TextBox ID="txtDisplayCONote" Width="86%" runat="server"></asp:TextBox><br />
                <br />
                <div class="modal-footer">                    
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>

            <!-- Modal Memo -->
            <div id="MemoModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalMemo" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Insert Memo</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtMemoNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSaveMemo" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveMemo_Click"  />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Inforce -->

            <!-- Modal Not Taken -->
            <div id="NotTakenModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalNotTaken" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note for Not Taken</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNotTakenNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSaveNotTaken" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveNotTaken_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Not Taken -->

            <!-- Modal Modal Postpone -->
            <div id="PostponeModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalPostpone" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note for Postpone</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtPostponeNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSavePostpone" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSavePostpone_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Postpone -->

            <!-- Modal Cancel -->
            <div id="CancelModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalCancel" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note for Cancel</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtCancelNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSaveCancel" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveCancel_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Cancel -->

            <!-- Modal Decline -->
            <div id="DeclineModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalDecline" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note for Decline</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDeclineNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSaveDecline" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveDecline_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Decline -->

            <!-- Modal Approve -->
            <div id="ApproveModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalApprove" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Approve Note</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtApproveNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnSaveApprove" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveApprove_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Approve -->

            <!-- Modal Result -->
            <div id="ResultModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalResult" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnCancelNote" runat="server" Text="Close" class="btn" data-dismiss="modal" aria-hidden="true" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>
            <!--End Modal Result -->

            <!-- Modal AP Result -->
            <div id="APResultModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalResult" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Approve Note</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtAPResult" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnCancelAPNote" runat="server" Text="Close" class="btn" data-dismiss="modal" aria-hidden="true" />
                </div>
            </div>
            <!--End Modal Result -->

            <!-- Modal Undo Underwrite -->
            <div id="UndoUnderwriteModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalUndoUW" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Confirmation</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Are you sure you want to undo underwrite this application?<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnUndoUW"  class="btn btn-primary" Style="height: 27px;" runat="server" Text="Yes" OnClick="btnUndoUW_Click" />                    

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Undo Underwrite -->

            <!-- Modal CO Print-->
            <div id="coprint" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalCOPrint" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Print CO Inform Letter</h3>
                </div>
                <br />
                    <div id="message" style="margin-left:10px;" class="text-error"></div>
                    <table style="margin:10px;">
                       
                        <tr>
                             <td>Document Number:</td>
                            <td>
                                <span style="color:red;">*</span>
                                <asp:TextBox ID="txtDocNumber" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            
                           <td>Document Date:</td>
                            <td>
                                <span style="color:red;">*</span>
                                <asp:TextBox runat="server" ID="txtDocDate" CssClass="datepicker"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Reason:</td>
                            <td>
                                <span style="color:red;">*</span>
                                <asp:TextBox ID="txtReason" runat="server" style="width:400px;" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Document Writer:</td>
                            <td>
                                <span style="color:red;">*</span>
                                <asp:DropDownList ID="ddlDocWriter" runat="server" DataSourceID="DocumentWriterDataSource" DataTextField="Name_KH" DataValueField="Position_KH" AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="."></asp:ListItem>
                                    <%--<asp:ListItem Value="មន្ដ្រីប្រតិបត្ដិវិភាគហានិភ័យផ្នែកវេជ្ជសាស្រ្ត" Text="រិច​ ដារ៉ង់"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                <br />
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" onclick="print();" value="Print" />
                    <%--<button  class="btn btn-primary" onclick="print();">Print</button>     --%>               
                    <asp:Button ID="btnCOPrint" runat="server" style="display:none;" OnClick="btnCOPrint_Click"/>
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End  Modal CO Print -->


            <%--Hidden Fields--%>
            <asp:HiddenField ID="hdfProductID" runat="server" />
            <asp:HiddenField ID="hdfGender" runat="server" />
            <asp:HiddenField ID="hdfAge" runat="server" />
            <asp:HiddenField ID="hdfStatusCode" runat="server" />
            <asp:HiddenField ID="hdfAppRegisterID" runat="server" />
            <asp:HiddenField ID="hdfAppNumber" runat="server" />

            <asp:HiddenField ID="hdfPayYear" runat="server" />
            <asp:HiddenField ID="hdfPayUpToAge" runat="server" />
            <asp:HiddenField ID="hdfAssureYear" runat="server" />
            <asp:HiddenField ID="hdfAssureUpToAge" runat="server" />
            <asp:HiddenField ID="hdfSumInsure" runat="server" />
            <asp:HiddenField ID="hdfSystemPremium" runat="server" />

            <asp:HiddenField ID="hdfUserPremium" runat="server" />

            <asp:HiddenField ID="hdfSystemPremiumDiscount" runat="server" />
            <asp:HiddenField ID="hdfOriginalAmount" runat="server" />
            <asp:HiddenField ID="hdfRoundedAmount" runat="server" />
            <asp:HiddenField ID="hdfUserPremiumDiscount" runat="server" />
            <asp:HiddenField ID="hdfPayMode" runat="server" />
            <asp:HiddenField ID="hdfAppDate" runat="server" />
            <asp:HiddenField ID="hdfEMRate" runat="server" />
            <asp:HiddenField ID="hdfBirthDate" runat="server" />

            
            <%--age_insure, pay_year, pay_up_to_age, assure_year, assure_up_to_age, user_sum_insure, system_sum_insure, user_premium, system_premium, system_premium_discount, pay_mode, user_name, created_note);--%>
        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveInforce" />
            <asp:PostBackTrigger ControlID="btnSaveApprove" />
            <asp:PostBackTrigger ControlID="btnSaveDecline" />
            <asp:PostBackTrigger ControlID="btnSaveCancel" />
            <asp:PostBackTrigger ControlID="btnSavePostpone" />
            <asp:PostBackTrigger ControlID="btnSaveNotTaken" />
            <asp:PostBackTrigger ControlID="btnSaveMemo" />
            <asp:PostBackTrigger ControlID="btnSaveCO" />           
            <asp:PostBackTrigger ControlID="btnCancelNote" />  
            <asp:PostBackTrigger ControlID="btnUndoUW" />
            <asp:PostBackTrigger ControlID="btnCancelAPNote" />
 

            
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

