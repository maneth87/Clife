<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="register_employee.aspx.cs" Inherits="Pages_GTLI_register_employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
       
        <li>
            <!-- Button to trigger modal register employee form -->
            <div style="display:none">
                <asp:Button ID="btnReNewPolicy" runat="server" OnClick="btnReNewPolicy_Click" />
            </div>
            
            <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="~/App_Themes/functions/add.png" OnClick="ImgBtnAdd_Click" ValidationGroup="1" OnClientClick="reConfirm(this,'add new');"  />
        </li>
        <li>
            <asp:Button ID="btnRenew" runat="server"  Text="Renew Policy" OnClientClick="reConfirm(this,'Please click [OK] to renew policy.'); " style=" background-color:none; border:none;" OnClick="btnRenew_Click" Visible="false"/>
        </li>
        <li>
            <asp:ImageButton ID="imgBtnRenew" runat="server" ImageUrl="~/App_Themes/functions/renewpolicy.png" OnClick="imgBtnRenew_Click" OnClientClick="reConfirm(this,'Please click [OK] to renew policy.');"  />
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
            GetPlans();
      
            var index = 0;
            var index1 = 0;

            index = $('#Main_hdfPlanIndex').val();
            //get selected index
            index1 = $("#<%=ddlOption.ClientID%>").prop("selectedIndex");

            if (index1 == 1 && index > 0) {
                alert("Done!");
                $("#<%=ddlPlan.ClientID%>").prop("selectedIndex", index);
            }
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
                                
                                //get next effective date, addYear(1)
                                getEffectiveDate();
                              
                            }

                        });
                    } else {
                        $('#Main_txtPlan').val("");
                    }

                }

            });
        }
        //Get Effective Date
        function getEffectiveDate() {
            var company_name = $('#txtCompany').val();
            $('#Main_txtEffectiveDate').empty();
            $.ajax({
                type: "POST",
                url: "../../GTLIWebService.asmx/getEffectiveDate",
                data: "{companyName:" + JSON.stringify(company_name) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtEffectiveDate').val(data.d);
                }

            });
        }
        //Get plan id
        function GetPlanID(obj) {
           
            $('#Main_hdfPlanID').val(obj.value);

            //get index of ddlPlan
            $('#Main_hdfPlanIndex').val($("#<%=ddlPlan.ClientID%>").prop("selectedIndex"));

        }

        //Renew Policy
        function ReNewPolicy() {
            if (confirm('Are you sure, you want to renew policy of this company?')) {
                var btnSave = document.getElementById("<%= btnReNewPolicy.ClientID %>");
                btnSave.click();
            }
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

                        $("#" + inputList[i].id).parent("td").parent("tr").css("background-color", "Gray");


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

        //Fucntion to check a checkbox
        function Check_Click(objRef) {

            if (objRef.checked) {

                $("#" + objRef.id).parent("td").parent("tr").css("background-color", "Gray");

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

        //confirm renew policy
        function reConfirm(btn, message) {
            confirm(message);
           
        }

        //Select Change
        function OnSelectedIndexChange() {
            var value = $("#Main_ddlOption").val();

            //Upload File
            if (value == 0) {
                $("#Main_uploadedDoc").show();
                $("#Main_btnSearch").hide();
                $("#Main_gvActiveMember").hide();

            }

            //Select From Table
            if (value == 1) {
                $("#Main_uploadedDoc").hide();
                $("#Main_btnSearch").show();
                $("#Main_gvActiveMember").show();
            }

            //Search data
            function searchRecord() {
             
                var btn = document.getElementById("<% =btnSearch.ClientID %>");
                btn.click();
            }

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

            <h3 class="panel-title">Register New Policy | <asp:Label ID="lblRenewPolicy" runat="server" Text="Renew Policy?" ForeColor="Blue"></asp:Label>
            <asp:CheckBox ID="ckbRenew" runat="server"  AutoPostBack="false" /></h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <table width="100%">
                <tr>
                    <td width="100%">Payment Code
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtPaymentCode" runat="server" Width="99%" Font-Size="10pt" MaxLength="50" TabIndex="1"></asp:TextBox>

                    </td>
                    <td>
                       
                    </td>
                </tr>
                <tr>
                    <td width="100%">Company Name<span style="color: red">*</span>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtCompany" runat="server" Width="99%" Font-Size="10pt" TabIndex="2" ClientIDMode="Static" AutoPostBack="True"></asp:TextBox>

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
                        <asp:DropDownList ID="ddlPlan" runat="server" Width="100%" TabIndex="3" onchange="GetPlanID(this);" >                          
                        </asp:DropDownList> 
                        <asp:HiddenField ID="hdfPlanID" runat="server" /> 
                        <asp:HiddenField ID="hdfPlanIndex" runat="server" /> 
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPlan" runat="server" ErrorMessage="Require Plan" Text="*" ForeColor="Red" ControlToValidate="ddlPlan" InitialValue="0" ValidationGroup="1"></asp:RequiredFieldValidator>                       
                    </td>
                </tr>
                <tr>
                    <td>Employee Data<span style="color: red">*</span>
                    </td>
                   
                </tr>
                <tr>
                     <td>
                        <asp:DropDownList ID="ddlOption" Width="100%" TabIndex="7" runat="server" AppendDataBoundItems="True" >
                            <asp:ListItem Value="0">From File</asp:ListItem>
                             <asp:ListItem Value="1">From Table</asp:ListItem>
                        </asp:DropDownList>

                    </td>

                </tr>
                <tr>
                    <td width="100%">
                        <table>
                            <tr>
                                <td>
                                    <input id="uploadedDoc" runat="server" class="fileupload" type="file" tabindex="4" />
                                    <asp:Button ID="btnSearch" runat="server" Text="Generate Employee List" OnClick="btnSearch_Click" 
                                        style="color:blue; border-radius:5px; box-shadow:0 0 0 2px"
                                        />
                                    
                                 </td>
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
                        <asp:TextBox ID="txtTotalNumberOfStaff" runat="server" Width="99%" Font-Size="10pt" TabIndex="5" MaxLength="5" ToolTip="Input total Number of staff"></asp:TextBox>

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
                        <asp:TextBox ID="txtEffectiveDate" runat="server" Width="99%" Font-Size="10pt" CssClass="datepicker" onkeypress="return false;" TabIndex="6"></asp:TextBox>

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
                        <asp:DropDownList ID="ddlSaleAgent" Width="100%" TabIndex="7" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">.</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSaleAgent" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="ddlSaleAgent" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">Discount (%)<span style="color: red">*</span>          
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red" ValidationGroup="1"
                            ErrorMessage=" - Please enter numerical value only." ValidationExpression="^\d+$" ControlToValidate="txtDiscountAmount"></asp:RegularExpressionValidator>           
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="100%">
                       <asp:TextBox ID="txtDiscountAmount" runat="server" Width="99%" Font-Size="10pt" TabIndex="8" MaxLength="5" Text="0" ToolTip="Specific Plan Discount Amount"></asp:TextBox>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtDiscountAmount" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" ></asp:RequiredFieldValidator>
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
          
            <div id="divEmployeeList">
                <asp:GridView ID="gvActiveMember" runat="server" AutoGenerateColumns="False" >

                    <Columns>
                         <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="IndentBottom" HeaderStyle-CssClass="IndentBottom">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" runat="server" onclick="Check_Click(this)" />
                                <asp:HiddenField ID="hdfCertificateID" runat="server" Value='<%# Bind("GTLI_Certificate_ID")%>' />
                                <asp:HiddenField ID="hdfPremiumID" runat="server" Value='<%# Bind("GTLI_Premium_ID")%>' />
                            </ItemTemplate>

                            <HeaderStyle CssClass="IndentBottom"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" CssClass="IndentBottom" Width="50px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="50px">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></ItemStyle>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Certificate No.">
                             <ItemTemplate>
                                 <asp:Label ID="lblCertificateNumber" runat="server" Text='<%# Bind("Certificate_Number") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Center" Width="100px" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Employee Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                             <ItemTemplate>
                                 <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("Employee_Name") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee ID" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                             <ItemTemplate>
                                 <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Bind("EmployeeID") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Position" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                             <ItemTemplate>
                                 <asp:Label ID="lblPosition" runat="server" Text='<%# Bind("Position") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gender" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                             <ItemTemplate>
                                 <asp:Label ID="lblGender" runat="server" Text='<%# Bind("Gender") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="DOB" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                             <ItemTemplate>
                                 <asp:Label ID="lblDOB" runat="server" Text='<%# Bind("DOB",  "{0:dd-MM-yyyy}") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                         </asp:TemplateField>
                        <asp:BoundField DataField="GTLI_Plan" HeaderText="Plan">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="60px" HorizontalAlign="Center" />
                        </asp:BoundField>
                         <asp:TemplateField HeaderText="Effective Date">
                             <ItemTemplate>
                                 <asp:Label ID="Label1" runat="server" Text='<%# Bind("Effective_Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Center" Width="100px" />
                         </asp:TemplateField>
                        <asp:BoundField DataField="Expiry_Date" HeaderText="Expiry Date" DataFormatString="{0:dd-MM-yyyy}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Days" HeaderText="Period (Days)">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="80px" HorizontalAlign="Center" />
                        </asp:BoundField>
                         <asp:TemplateField HeaderText="Sum Insured">
                             <ItemTemplate>
                                 <asp:Label ID="lblSumInsure" runat="server" Text='<%# Bind("Sum_Insured", "{0:C0}") %>'></asp:Label>
                             </ItemTemplate>
                             <ItemStyle Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Right" Width="120px" />
                         </asp:TemplateField>
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
           
           <%-- <asp:Button ID="btnRenew" runat="server"  Text="Renew Policy" OnClick="btnRenew_Click"/>--%>
                 
            </div>
             <%-- Section Hidenfields Initialize  --%>
             <asp:HiddenField ID="hdfuserid" runat="server"/>
             <asp:HiddenField ID="hdfusername" runat="server" />
             <asp:HiddenField ID="hfValue" runat="server" />
        </div>
    </div>
</asp:Content>

