<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="underwriting_rider.aspx.cs" Inherits="Pages_Business_underwriting_rider" %>


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
        function SelectSingleCheckBox(ckb) {
          

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

            if (em_percentage <= 0) {
                $('#Main_txtEMRate').val("0");
                $('#Main_txtEMPremium').val("0");
                $('#Main_txtEMAmount').val("0");
                return;
            }


            $.ajax({
                type: "POST",
                url: "../../UnderwritingWebService.asmx/GetEMRate",
                data: "{product_id:'" + product_id + "',gender:'" + gender + "',customer_age:'" + customer_age + "',em_percentage:'" + em_percentage + "',assure_year:'" + assure_year + "',app_year:'" + app_year + "'}",
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



    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy2" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <ul class="toolbar">
        
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" onmouseover="tooltip.pop(this, 'Refresh Application List')" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnInforce" runat="server" ImageUrl="~/App_Themes/functions/inforce.png" OnClick="ImageBtnInforce_Click" />
           <%-- <input type="button" onclick="CheckSelection(DateSelectionModal)" style="background: url('../../App_Themes/functions/inforce.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnApprove" runat="server" ImageUrl="~/App_Themes/functions/approve.png" OnClick="ImageBtnApprove_Click" />
            <%--<input type="button" onclick="CheckSelection(ApproveModal)" style="background: url('../../App_Themes/functions/approve.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnDecline" runat="server" ImageUrl="~/App_Themes/functions/decline.png" OnClick="ImageBtnDecline_Click" />
            <%--<input type="button" onclick="CheckSelection(DeclineModal)" style="background: url('../../App_Themes/functions/decline.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        
        </li>

        <li>
            <asp:ImageButton ID="ImgBtnCancel" runat="server" ImageUrl="~/App_Themes/functions/cancel.png" OnClick="ImgBtnCancel_Click" />
            <%--<input type="button" onclick="CheckSelection(CancelModal)" style="background: url('../../App_Themes/functions/cancel.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnPostpone" runat="server" ImageUrl="~/App_Themes/functions/postpone.png" OnClick="ImageBtnPostpone_Click" />
            <%--<input type="button" onclick="CheckSelection(PostponeModal)" style="background: url('../../App_Themes/functions/postpone.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnNotTaken" runat="server" ImageUrl="~/App_Themes/functions/not_taken.png" OnClick="ImageBtnNotTaken_Click" />
            <%--<input type="button" onclick="CheckSelection(NotTakenModal)" style="background: url('../../App_Themes/functions/not_taken.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnCounterOffer" runat="server" Width="110" Height="24" ImageUrl="~/App_Themes/functions/counter_offer.png"  OnClick="ImageBtnCounterOffer_Click" />
            <%--<input type="button" onclick="CheckSelection(CounterOfferModal)" style="background: url('../../App_Themes/functions/counter_offer.png') no-repeat; border: none; height: 40px; width: 110px;" />--%>
        
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnMemo" runat="server" ImageUrl="~/App_Themes/functions/memo.png" OnClick="ImageBtnMemo_Click" />
            <%--<input type="button" onclick="CheckSelection(MemoModal)" style="background: url('../../App_Themes/functions/memo.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>  
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnUndo" runat="server" ImageUrl="~/App_Themes/functions/undo_underwrite.png" OnClick="ImageBtnUndo_Click" />
            <%--<input type="button" onclick="CheckUW()" style="background: url('../../App_Themes/functions/undo_underwrite.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>            
        </li>
      
    </ul>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBtnRefresh" />
            <asp:PostBackTrigger ControlID="ImageBtnApprove" />
            <asp:PostBackTrigger ControlID="ImageBtnInforce" />
            <asp:PostBackTrigger ControlID="ImageBtnDecline" />
            <asp:PostBackTrigger ControlID="ImgBtnCancel" />
            <asp:PostBackTrigger ControlID="ImageBtnPostpone" />
            <asp:PostBackTrigger ControlID="ImageBtnNotTaken"/>
            <asp:PostBackTrigger ControlID="ImageBtnCounterOffer" />
            <asp:PostBackTrigger ControlID="ImageBtnMemo" />
            <asp:PostBackTrigger ControlID="ImageBtnUndo"/>
        </Triggers>
        
    </asp:UpdatePanel>
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <%-- <asp:ScriptManager ID="MainScriptManager" runat="server" />--%>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="ContentPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <!-- Modal Message -->
            <div id="ModalMessage" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalMessage" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">System Message Alert!</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtMessage" Width="88%" TextMode="MultiLine" Height="60" runat="server" style="color:red; font-weight:normal;"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
            <!--End Modal Message -->

            <%--Operation here--%>
            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Riders Underwrite Applications</h3>
                </div>
                <div class="panel-body">

                    <asp:GridView ID="gvApplication" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowDataBound="gvApplication_RowDataBound" OnRowCommand="gvApplication_RowCommand">
                         <SelectedRowStyle BackColor="#EEFFAA" />

                        <Columns>
                            <asp:TemplateField HeaderText="Level" Visible="false">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AppID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAppID" runat="server" Text='<%#Eval("App_Register_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server" onclick="SelectSingleCheckBox(this);"/>
                                   
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lbtn1" runat="server" Font-Underline="true" ForeColor="#3333ff" OnClick='<%# "GetAppStatusDetail(\"" + Eval("App_Register_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Age_Insure" ) + "\", \"" + Eval("System_Sum_Insure" ) + "\", \"" + Eval("System_Premium" ) + "\", \"" + Eval("Pay_Mode" ) + "\", \"" + Eval("Pay_Year" ) + "\");" %>' Text='<%# GetStatusCode(Eval("App_Register_ID").ToString()) %>' ToolTip="Click to view details"></asp:LinkButton>--%>
                                    <asp:LinkButton ID="lbtn1" runat="server" Font-Underline="true" ForeColor="#3333ff"  Text='<%# GetStatusCode(Eval("App_Register_ID").ToString() ,Convert.ToInt32(Eval("level").ToString())) %>' ToolTip="Click to view details" CommandName="view" CommandArgument='<%# GetStatusCode(Eval("App_Register_ID").ToString(), Convert.ToInt32(Eval("level").ToString())) %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="70px" />
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application no." SortExpression="App_Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblAppNumber" runat="server" Text='<%# Eval("App_Number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Signature date" SortExpression="App_Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblSignature" runat="server" Text='<%# Eval("App_Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Agent" SortExpression="Sale_Agent_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgent" runat="server" Text='<%# Eval("Sale_Agent_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product" SortExpression="Product_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblProduct_ID" runat="server" Text='<%# Eval("Product_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Age" SortExpression="Age_Insure">
                                <ItemTemplate>
                                    <asp:Label ID="lblAge_Insure" runat="server" Text='<%# Eval("Age_Insure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment" SortExpression="Pay_Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblPay_Year" runat="server" Text='<%# Eval("Pay_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Coverage" SortExpression="Assure_Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssure_Year" runat="server" Text='<%# Eval("Assure_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Coverage to age" SortExpression="Assure_Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssure_Up_To_Age" runat="server" Text='<%# Eval("Assure_Up_To_Age") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Pay Up To Age" SortExpression="Pay_Up_To_Age">
                                <ItemTemplate>
                                    <asp:Label ID="lblPay_Up_To_Age" runat="server" Text='<%# Eval("Pay_Up_To_Age") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sum Insure" SortExpression="System_Sum_Insure">
                                <ItemTemplate>
                                    <asp:Label ID="lblSystem_Sum_Insure" runat="server" Text='<%# Eval("System_Sum_Insure", "{0:N0}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Annual Premium" SortExpression="Rounded_Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblRounded_Amount" runat="server" Text='<%# Eval("Rounded_Amount", "{0:N0}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Original Premium" SortExpression="Original_Amount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOriginal_Amount" runat="server" Text='<%# Eval("Original_Amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="System Premium" SortExpression="System_Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblSystem_Premium" runat="server" Text='<%# Eval("System_Premium", "{0:N0}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPay_Mode" runat="server" Text='<%# GetPayMode(DataBinder.Eval(Container.DataItem, "Pay_Mode").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPay_Mode_Code" runat="server" Text='<%# Eval("Pay_Mode")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Surname" SortExpression="Last_Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblLast_Name" runat="server" Text='<%# Eval("Last_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="First Name" SortExpression="First_Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblFirst_Name" runat="server" Text='<%# Eval("First_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Birth Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBirth_Date" runat="server" Text='<%# Eval("Birth_Date") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Country_ID" HeaderText="Nationality" SortExpression="Country_ID">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="Rider Type" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_rider_type" runat="server" Text='<%# Eval("Rider_type") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>

                       
                    </asp:GridView>

                    <asp:SqlDataSource ID="AppDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], [Amount], [Original_Amount] FROM [Cv_Basic_App] WHERE [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_Underwriting WHERE Status_Code = 'NT' OR Status_Code = 'DC' OR Status_Code = 'CC' OR Status_Code = 'PP') AND ([level]=1 OR [level] IS NULL) AND [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_App_Register_Cancel) AND [Original_Amount] > 0 And (App_Register_ID <> App_Number) AND ([Status_Code] IS NULL) OR ([Status_Code] = 'AP') OR ([Status_Code] = 'CO') OR ([Status_Code] = 'MO')  ORDER BY [App_Date] DESC"></asp:SqlDataSource>
                    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], [Amount], [Original_Amount] FROM [Cv_Basic_App] WHERE [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_Underwriting WHERE Status_Code = 'NT' OR Status_Code = 'DC') AND [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_App_Register_Cancel) AND [Original_Amount] > 0 And (App_Register_ID <> App_Number) AND ([Status_Code] IS NULL) OR ([Status_Code] = 'AP') OR ([Status_Code] = 'CO') OR ([Status_Code] = 'MO') ORDER BY [App_Date] DESC"></asp:SqlDataSource>--%>

                    <asp:SqlDataSource ID="PaymentModeDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [Ct_Payment_Mode]"></asp:SqlDataSource>

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
                                <asp:TextBox ID="txtDateEffectiveDate" Width="350" runat="server" CssClass="datepicker" ></asp:TextBox>&nbsp;&nbsp;
                                (DD/MM/YYYY)
                            </td>
                        </tr>
                       
                    </table>
                    <br />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveInforce" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Inforce" OnClick="btnSaveInforce_Click1" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Inforce -->

             <!-- Modal Confirm Inforce -->
            <div id="InforceConfirmModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalEffectiveDate" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Inforce Confirmation</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>Effective Date:</td>
                            <td>
                                <asp:TextBox ID="txtEffectiveDate"  runat="server" ReadOnly="true"></asp:TextBox>&nbsp;&nbsp;
                                (DD/MM/YYYY)
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>Customer Age:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCustomerAge" ReadOnly="true"></asp:TextBox>
                                Year(S)
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>Premium:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPremiumPreview" ReadOnly="true"></asp:TextBox>
                                USD
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td>Annually Premium:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAnnuallyPremiumPreview" ReadOnly="true"></asp:TextBox>
                                USD
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="dConfirmMessage" runat="server" style="color:red; text-align:center;"></div>
                            </td>
                        </tr>
                    </table>
                    <br />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnInforceConfrim" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Confirm" OnClick="btnSaveInforce_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Confirm Inforce -->

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
                                        <%--<asp:TextBox ID="txtSumInsured" onchange="CalculateEMRate();" runat="server"></asp:TextBox></td>--%>

                                     
                                    <asp:TextBox ID="txtSumInsured" onchange="<%# GetEMRateByProductType() %>" runat="server"  Enabled="false"  ></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Premium Mode:
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddlPayMode" runat="server" onchange="CalculateEMRate();" DataSourceID="PaymentModeDatasource" DataTextField="Mode" DataValueField="Pay_Mode_ID"></asp:DropDownList></td>--%>
                                        <asp:DropDownList ID="ddlPayMode" runat="server" DataSourceID="PaymentModeDatasource" DataTextField="Mode" DataValueField="Pay_Mode_ID" OnSelectedIndexChanged="ddlPayMode_SelectedIndexChanged" AutoPostBack="True"  Enabled="false" ></asp:DropDownList></td>
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
                                        <asp:DropDownList ID="ddlEMPercentage" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlPayMode_SelectedIndexChanged">
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
                                        <asp:DropDownList ID="ddlEMPercentageDisplayCO" Enabled="false" runat="server">
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
                    <asp:Button ID="btnSaveApprove" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSaveApprove_Click"/>
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
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNote" Width="88%" TextMode="MultiLine" Height="60" runat="server" style="color:red; font-size:14px; box-shadow:0px 1px 2px 1px #808080;"></asp:TextBox>&nbsp;&nbsp;<br />
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


            <%--Hidden Fields--%>
            <asp:HiddenField ID="hdfProductID" runat="server" />
            <asp:HiddenField ID="hdfGender" runat="server" />
            <asp:HiddenField ID="hdfAge" runat="server" />
            <asp:HiddenField ID="hdfStatusCode" runat="server" />
            <asp:HiddenField ID="hdfAppRegisterID" runat="server" />

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
            
            <asp:PostBackTrigger ControlID ="ddlPayMode" />
            <asp:PostBackTrigger ControlID="ddlEMPercentage" />

            <%--<asp:AsyncPostBackTrigger ControlID="ImgBtnRefresh" EventName="Click" />--%>
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

