<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="upload_policies.aspx.cs" Inherits="Pages_Business_upload_policies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">


        <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server" ValidationGroup="2"  Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnUpload" runat="server" />
            </div>
            <asp:ImageButton ID="ImgBtnUpload" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/issue_policy.png" OnClick="ImgBtnUpload_Click" />

        </li>

    </ul>

    <script type="text/javascript">
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        function ValidateTextDecimal(j) {

            var msg = j.value;
            var w;
            var nokta = msg.indexOf(".");
            var ind;

            for (w = 0; w < msg.length; w++) {

                ind = msg.substring(w, w + 1);
                if (ind < "0" || ind > "9") {

                    if (nokta > 0)
                        if (w == nokta) continue;

                    msg = msg.substring(0, w);
                    j.value = msg;
                    break;
                }

            }

        }

        $(document).ready(function () {
            $('#Main_txtEntryDate').datepicker().on('changeDate', function (dob) {

                $('#Main_hdfEntryDate').val($('#Main_txtEntryDate').val());

            });
        });

        //Get Premium
        function GetPremium() {

            var value = $('#Main_txtSumInsured').val();
            $('#Main_hdfSumInsured').val(value);

            $.ajax({
                type: "POST",
                url: "../../CalculationWebService.asmx/GetPremiumMicro",
                data: "{amount:'" + value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtPremiumAmount').val(data.d);
                    $('#Main_hdfPremiumAmount').val(data.d);
                }

            });
        };

        //Get Pay Year for (payment period)
        function GetPayYear(product_id) {

            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetPayYear",
                data: "{product_id:" + JSON.stringify(product_id) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtPaymentPeriod').val(data.d);
                    $('#Main_hdfPaymentPeriod').val(data.d);
                }

            });
        };

        //Get Assure Year for (Term of Insurance)
        function GetAssureYear(product_id, customer_age) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetAssureYear",
                data: "{product_id:'" + product_id + "',customer_age:'" + customer_age + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtTermInsurance').val(data.d);
                    $('#Main_hdfTermInsurance').val(data.d);
                }

            });
        };

        //Search Marketing Code (Search click)
        function SearchMarketingCode() {
            var sale_agent_name = $('#Main_txtAgentName').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetSaleAgents",
                data: "{sale_agent_name:" + JSON.stringify(sale_agent_name) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_hdftotalagentrow').val(data.d.length);

                    $('#dvAgentList').setTemplate($("#jTemplateSaleAgent").html());
                    $('#dvAgentList').processTemplate(data);

                }

            });

        }

        //Get Selected Sale Agent
        function GetAgent(AgentID, AgentName, row_index_agent_list) {

            if ($('#' + row_index_agent_list).is(':checked')) {
                marketing_code = AgentID;
                marketing_name = AgentName;
            } else {
                marketing_code = "";
                marketing_name = "";
            }

            var total_agent_row = $('#Main_hdftotalagentrow').val();

            //Uncheck all other checkboxes
            for (var i = 1; i <= total_agent_row  ; i++) {
                if (i != row_index_agent_list) {
                    $('#' + i).prop('checked', false);
                }

            }

        };

        //Fill Selected Sale Agent to Textboxes
        function FillMarketingCode() {

            if (marketing_code == "") {
                alert('Please select a checkbox');
                return;
            }

            $('#Main_txtMarketingCode').val(marketing_code);
            $('#Main_txtMarketingName').val(marketing_name);
            $('#Main_hdfMarketingCode').val(marketing_code);
            $('#Main_hdfMarketingName').val(marketing_name);
        };

        //Process Micro Product Type
        function ProcessMicroProductType(value) {
            if (value == "0") {
                $('#Main_txtSumInsured').prop('readonly', 'readonly');
                $('#Main_txtPremiumAmount').prop('readonly', 'readonly');
                $('#Main_txtSumInsured').val('');
                $('#Main_txtPremiumAmount').val('');
                $('#Main_hdfSumInsured').val('');
                $('#Main_hdfPremiumAmount').val('');

                $('#Main_txtPaymentPeriod').val('');
                $('#Main_hdfPaymentPeriod').val('');
                $('#Main_txtTermInsurance').val('');
                $('#Main_hdfTermInsurance').val('');

            } else {
                $('#Main_txtSumInsured').removeAttr('readonly');
                $('#Main_txtPremiumAmount').removeAttr('readonly');
                $('#Main_txtSumInsured').val('');
                $('#Main_txtPremiumAmount').val('');
                $('#Main_hdfSumInsured').val('');
                $('#Main_hdfPremiumAmount').val('');

                var card = $('#Main_ddlCard option:selected').val();

                $.ajax({
                    type: "POST",
                    url: "../../ProductWebService.asmx/GetProductMicro",
                    data: "{card:'" + card + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        
                        GetPayYear(data.d.Product_ID);
                        GetAssureYear(data.d.Product_ID, 0);
                        $('#Main_hdfProduct').val(data.d.Product_ID);
                        
                    }

                });

               
            }
        }

        //User Input Premium
        function InputPremium() {

            $('#Main_hdfPremiumAmount').val($('#Main_txtPremiumAmount').val());
        }

        function GetOffice(company_id) {

            $.ajax({
                type: "POST",
                url: "../../ChannelWebService.asmx/GetChannelLocation",
                data: "{channel_item_id:'" + company_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_ddlOffice').setTemplate($("#jTemplateOffice").html());
                    $('#Main_ddlOffice').processTemplate(data);
                }

            });
        }

        function GetOfficeID(office_id) {
            $('#Main_hdfChannelLocation').val(office_id);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

      <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>

    <%--Jtemplate Sections--%>
    <script id="jTemplateSaleAgent" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">Sale Agent ID</th>
                    <th style="border-width: thin; border-style: solid;">Sale Agent Name</th>
                    <th style="border-width: thin; border-style: solid;"></th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as record}
                    <tr>
                        <td>{ $T.record.Sale_Agent_ID }</td>
                        <td>{ $T.record.Full_Name }</td>
                        <td>
                            <input id="{ $T.record.row_index }" type="checkbox" onclick="GetAgent('{ $T.record.Sale_Agent_ID }', '{ $T.record.Full_Name }', '{ $T.record.row_index }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>

    <script id="jTemplateOffice" type="text/html">
        <option selected="selected" value='0'>.</option>
        {#foreach $T.d as record}          
             <option value='{ $T.record.Channel_Location_ID }'>{ $T.record.Office_Name }</option>
        {#/for}
           
    </script>
    <br />
    <br />
    <br />


    <%-- Upload Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Upload Policies</h3>
        </div>
        <div class="panel-body">
            <%--Upload Content Here--%>
            <table>
                <tr>
                    <td style="text-align: right;">Product:</td>
                    <td style="vertical-align: bottom; width:350px; vertical-align:middle;">
                        <asp:DropDownList ID="ddlCard" runat="server" Width="95%" Height="35px" onchange="ProcessMicroProductType(this.value);">
                            <asp:ListItem Value="0">.</asp:ListItem>
                            <asp:ListItem Value="1">Term One Card</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCard" runat="server" ControlToValidate="ddlCard" InitialValue="0" Text="*" ForeColor="Red" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Payment Code:</td>
                    <td>
                        <asp:TextBox ID="txtPaymentCode" runat="server" MaxLength="50" Width="91%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Date of Entry:</td>
                    <td>
                        <asp:TextBox ID="txtEntryDate" runat="server" Width="91%" onkeypress="return false;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEntryDate" runat="server" ErrorMessage="Require Date of Entry" ControlToValidate="txtEntryDate" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfEntryDate" runat="server" />
                    </td>
                    <td>                        

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Term of Insurance:</td>
                    <td>
                        <asp:TextBox ID="txtTermInsurance" runat="server" Width="91%" ReadOnly="True" onkeyup="ValidateNumber(this);" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorTermOfInsurance" runat="server" ErrorMessage="Require Term of Insurance" ControlToValidate="txtTermInsurance" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfTermInsurance" runat="server" />

                    </td>
                    <td>Years</td>
                </tr>
                <tr>
                    <td style="text-align: right">Payment Period:</td>
                    <td>
                        <asp:TextBox ID="txtPaymentPeriod" Width="91%" runat="server" ReadOnly="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPaymentPeriod" runat="server" ErrorMessage="Require Payment Period" ControlToValidate="txtPaymentPeriod" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfPaymentPeriod" runat="server" />

                    </td>
                    <td >
                        Years
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Sum Insured:</td>
                    <td>
                        <asp:TextBox ID="txtSumInsured" onkeyup="ValidateTextDecimal(this); GetPremium()" ReadOnly="true" MaxLength="5" runat="server" Width="91%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSumInsured" runat="server" ControlToValidate="txtSumInsured" Text="*" ForeColor="Red" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right">Premium:</td>
                    <td>
                        <asp:TextBox ID="txtPremiumAmount" onkeyup="ValidateTextDecimal(this); InputPremium()" ReadOnly="true" MaxLength="8" runat="server" Width="91%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPremium" runat="server" ControlToValidate="txtPremiumAmount" Text="*" ForeColor="Red" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align:middle;">Marketing Code:</td>
                    <td>
                        <asp:TextBox ID="txtMarketingCode" runat="server" ReadOnly="True" Width="91%" Height="25px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorMarketingCode" runat="server" ErrorMessage="Require Maketing Code" ControlToValidate="txtMarketingCode" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>

                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align:middle;">Name of Marketing:</td>
                    <td>
                        <asp:TextBox ID="txtMarketingName" runat="server" ReadOnly="True" Width="91%" Height="25px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorNameOfMarketing" runat="server" ControlToValidate="txtMarketingName" ErrorMessage="Require Name of Marketing" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>

                    </td>
                    <td style="vertical-align: top; width: 120px;">
                        <input type="button" data-toggle="modal" data-target="#myMarketingCodeList"  style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align:middle;">Company (Micro):</td>
                    <td>
                        <asp:DropDownList ID="ddlCompanyMicro" Width="95%" Height="35px" AppendDataBoundItems="true" runat="server" onchange="GetOffice(this.value);" DataSourceID="SqlDataSourceCompany" DataTextField="Channel_Name" DataValueField="Channel_Item_ID">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyMicro" runat="server" ControlToValidate="ddlCompanyMicro" InitialValue="0" ErrorMessage="Require Company" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align:middle;">Office:</td>
                    <td>
                        <asp:DropDownList ID="ddlOffice" Width="95%" Height="35px" runat="server" AppendDataBoundItems="true" onchange="GetOfficeID(this.value);">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorOffice" runat="server" ControlToValidate="ddlOffice" ErrorMessage="Require Office" ForeColor="Red" Text="*" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right">Upload:</td>
                    <td>
                        <asp:FileUpload ID="FileUploadMicroPolicy" runat="server" Width="91%" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUploadPolicy" ControlToValidate="FileUploadMicroPolicy" Text="*" ForeColor="Red" runat="server" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--End Upload Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Upload Result</h3>
        </div>
        <div class="panel-body">
            
            <asp:Table ID="tblResult" style='width:100%;' border='1' runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell HorizontalAlign="Center">No.</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Khmer First Name</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Khmer Last Name</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">English First Name</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">English Last Name</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Date of Birth</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Gender</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Result</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>

    <!-- Modal Marketing Code -->
    <div id="myMarketingCodeList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Select Marketing Code</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->

            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="width: 25%; vertical-align: middle">Agent Code/Name:</td>
                    <td style="width: 57%; vertical-align: bottom">
                        <asp:TextBox ID="txtAgentName" Width="90%" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 18%; vertical-align: top">
                        <input type="button" class="btn" style="height: 27px;" onclick="SearchMarketingCode();" value="Search" />
                    </td>

                </tr>

            </table>
            <hr />
            <div id="dvAgentList"></div>


        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillMarketingCode();" data-dismiss="modal" value="Select" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End ModalMarketing Code-->

    <asp:SqlDataSource ID="SqlDataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand=" select Ct_Channel_Item.Channel_Name, Ct_Channel_Item.Channel_Item_ID from Ct_Channel_Item INNER JOIN Ct_Channel_Channel_Item ON Ct_Channel_Channel_Item.Channel_Item_ID = Ct_Channel_Item.Channel_Item_ID where Ct_Channel_Channel_Item.Channel_Sub_ID = 4"></asp:SqlDataSource>

    <%--Hidden fields of User--%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />

    <%--hidden field of Agent--%>
    <asp:HiddenField ID="hdfSaleAgentID" runat="server" />
    <asp:HiddenField ID="hdfSaleAgentName" runat="server" />
    <asp:HiddenField ID="hdftotalagentrow" runat="server" />

    <%--Hidden fields of Channel--%>
    <asp:HiddenField ID="hdfChannelChannelItem" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelLocation" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelItem" runat="server" Value="0" />

    <asp:HiddenField ID="hdfSumInsured" runat="server" />
    <asp:HiddenField ID="hdfPremiumAmount" runat="server" />
    <asp:HiddenField ID="hdfMarketingName" runat="server" />
    <asp:HiddenField ID="hdfMarketingCode" runat="server" />

    <asp:HiddenField ID="hdfProduct" runat="server" />
</asp:Content>

