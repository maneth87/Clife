<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="register_plan.aspx.cs" Inherits="Pages_GTLI_register_plan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImageBtnEdit" runat="server" ImageUrl="~/App_Themes/functions/edit.png" CausesValidation="false"  PostBackUrl="~/Pages/GTLI/edit_policy_plan.aspx" />
        </li>
        <li>
            <!-- Button to trigger modal register plan form -->
            <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="~/App_Themes/functions/add.png"  ValidationGroup="1" OnClick="ImgBtnAdd_Click"  />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>

    <%--Javascript--%>
    <script type="text/javascript">
        //Company Name autocomplete
        PageMethods.GetCompanyName(function (results) {

            $("#txtCompanyName").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(results, function (item) {
                        return matcher.test(item);
                    }));
                }
            });
        });


        //Get Plan
        function GetPlan() {
            var company_name = $('#txtCompanyName').val();           
            $.ajax({
                type: "POST",
                url: "../../GTLIWebService.asmx/CheckCompany",
                data: "{company_name:" + JSON.stringify(company_name) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "1") {
                        //Found Company then get plan
                        $.ajax({
                            type: "POST",
                            url: "../../GTLIWebService.asmx/GetPlan",
                            data: "{company_name:" + JSON.stringify(company_name) + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $('#Main_txtPlan').val(data.d);
                            }

                        });
                    } else {
                        $('#Main_txtPlan').val("");
                    }

                }

            });
        }

        //Dynamic Sum Insure
        function CheckDynamicSumInsure(ckb) {
            if (ckb.checked) {
                $('#Main_txtAmount').val(0);
                $('#Main_txtVirtualAmount').val(2500);

                $('#Main_txtAmount').prop("disable", "disable");

            } else {

                $('#Main_txtAmount').removeAttr("disable");
                $('#Main_txtVirtualAmount').val(0);
            }
        }

        //Get amount change
        function AmountChange(value) {
            $('#Main_txtVirtualAmount').val(value);
        }
    </script>
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Register Plan</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <table width="100%">
                <tr>
                    <td>Company Name<span style="color: red">*</span></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="99%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static"></asp:TextBox>

                    </td>
                    <td class="auto-style1">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompany" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtCompanyName" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Plan Name<span style="color: red">*</span></td>
                    <td></td>
                </tr>
                <tr>
                    <td>                                               
                         <asp:TextBox ID="txtPlan" runat="server" Width="99%" Font-Size="10pt" TabIndex="2"></asp:TextBox>                                                
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtPlan" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbDynamicSumInsure" runat="server" Text="Dynamic Sum Insure" onclick="CheckDynamicSumInsure(this);" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">Insured Amount<span style="color: red">*</span> (minimum 2,500)
                <asp:RegularExpressionValidator ID="AmountNumberRegularExpressionValidator" runat="server" ValidationGroup="1"
                    ControlToValidate="txtAmount" ErrorMessage=" - Please enter numerical value only."
                    ValidationExpression="^[0-9]*$" ForeColor="Red" Font-Size="9pt" Font-Names="Arial"></asp:RegularExpressionValidator>
                        <asp:RangeValidator ID="AmountRangeValidator" runat="server" ControlToValidate="txtVirtualAmount" ValidationGroup="1"
                            MaximumValue="999999999" MinimumValue="2500" Type="Double" ErrorMessage="  Must be between 2,500 to 999,999,999"
                            Display="static" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RangeValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtAmount" runat="server" Width="99%" Font-Size="10pt" TabIndex="3" onblur="AmountChange(this.value);"></asp:TextBox>

                    </td>
                    <td> 
                        <asp:RequiredFieldValidator ID="AmountNumberValidator" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtAmount" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">Total Permanent Disability (TPD) Coverage? &nbsp;
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:RadioButtonList ID="radTPD" runat="server" TabIndex="4" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td></td>
                </tr>
                 <tr>
                    <td width="100%">Accidental 100Plus Coverage? &nbsp;
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:RadioButtonList ID="rad100Plus" runat="server" TabIndex="5" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">Daily Hospital Cash (DHC) Coverage? &nbsp;
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:DropDownList ID="ddlDHC" Width="100%" Height="36px" TabIndex="6" runat="server">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="10">Option 1: 10$/year</asp:ListItem>
                            <asp:ListItem Value="20">Option 2: 20$/year</asp:ListItem>
                            <asp:ListItem Value="30">Option 3: 30$/year</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>             
                
            </table>
        </div>
    </div>

     <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
    <asp:HiddenField ID="hdfError" Value="false" runat="server"/>
    <div style="display:none">
        <asp:TextBox ID="txtVirtualAmount" runat="server" ></asp:TextBox>
    </div>
</asp:Content>

