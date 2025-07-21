<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="upload_gtli_policy.aspx.cs" Inherits="Pages_upload_gtli_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <ul class="toolbar">
        <li>
            <!-- Button to trigger modal register employee form -->
            <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="~/App_Themes/functions/add.png"  ValidationGroup="1" OnClick="ImgBtnAdd_Click"  />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jtemplate script--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>

    <%--Javascript--%>
    <script type="text/javascript">
        //Date picker
        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        //Company Name autocomplete
        PageMethods.GetCompanyName(function (results) {

            $("#txtCompany").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(results, function (item) {
                        return matcher.test(item);
                    }));
                }
            });
        });

        //Get plans for dropdownlist
        function GetPlans() {
            var company_name = $('#txtCompany').val();
            $('#Main_ddlPlan').empty();
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
                            url: "../../GTLIWebService.asmx/GetPlans",
                            data: "{company_name:" + JSON.stringify(company_name) + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $('#Main_ddlPlan').setTemplate($("#jTemplateGTLIPlan").html());
                                $('#Main_ddlPlan').processTemplate(data);
                            }

                        });
                    } else {
                        $('#Main_txtPlan').val("");
                    }

                }

            });
        }

        //Get plan id
        function GetPlanID(obj) {

            $('#Main_hdfPlanID').val(obj.value);
        }
    </script>

    <%--Jtemplate--%>
     <script id="jTemplateGTLIPlan" type="text/html">
        <option value="0" selected="True">.</option> 
        {#foreach $T.d as record}       
            <option value='{ $T.record.GTLI_Plan_ID }'>{ $T.record.GTLI_Plan }</option>
        {#/for}           
    </script>

    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Group Term Life Insurance: Register New Policy</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <table width="100%">

                <tr>
                    <td width="100%">Company Name<span style="color: red">*</span>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtCompany" runat="server" Width="99%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" AutoPostBack="True"></asp:TextBox>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompany" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtCompany" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Plan<span style="color: red">*</span></td>
                    <td></td>
                </tr>
                <tr>
                    <td>                                          
                        <asp:DropDownList ID="ddlPlan" runat="server" Width="100%" TabIndex="2" onchange="GetPlanID(this);" >                          
                        </asp:DropDownList> 
                        <asp:HiddenField ID="hdfPlanID" runat="server" /> 
                        
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPlan" runat="server" ErrorMessage="Require Plan" Text="*" ForeColor="Red" ControlToValidate="ddlPlan" InitialValue="0" ValidationGroup="1"></asp:RequiredFieldValidator>                       
                    </td>
                </tr>
                <tr>
                    <td width="100%">Employee Data<span style="color: red">*</span>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <table>
                            <tr>
                                <td>
                                    <input id="uploadedDoc" runat="server" class="fileupload" type="file" tabindex="3" /></td>
                            </tr>
                        </table>


                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">Total Number of Staffs<span style="color: red">*</span>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorStaffNumber" runat="server" ForeColor="Red" ValidationGroup="1"
                            ErrorMessage=" - Please enter numerical value only." ValidationExpression="^\d+$" ControlToValidate="txtTotalNumberOfStaff"></asp:RegularExpressionValidator>
                      
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtTotalNumberOfStaff" runat="server" Width="99%" Font-Size="10pt" TabIndex="4" MaxLength="5" ToolTip="Input total Number of staff"></asp:TextBox>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorStaffNumber" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtTotalNumberOfStaff" ForeColor="Red" Font-Size="9pt" Font-Names="Arial"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">Effective Date<span style="color: red">*</span>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtEffectiveDate" runat="server" Width="99%" Font-Size="10pt" CssClass="datepicker" onclick="return false;" TabIndex="5"></asp:TextBox>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="EffectiveDateRequiredFieldValidator" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtEffectiveDate" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">Sale Agent<span style="color: red">*</span>                     
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:DropDownList ID="ddlSaleAgent" Width="100%" TabIndex="6" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">.</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSaleAgent" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="ddlSaleAgent" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                       <asp:Label ID="lblMessage" ForeColor="Red" runat="server"></asp:Label>
                    </td>
                    <td>
                       
                    </td>
                </tr>               
            </table>
            <br />
             <%-- Section Hidenfields Initialize  --%>
             <asp:HiddenField ID="hdfuserid" runat="server" />
             <asp:HiddenField ID="hdfusername" runat="server" />
             <asp:HiddenField ID="hfValue" runat="server" />
        </div>
    </div>
</asp:Content>

