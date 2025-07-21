<%@ Page Title="Clife | Core Data => Employee" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="employee.aspx.cs" Inherits="Pages_CoreData_employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBView" runat="server" data-toggle="modal" data-target="#myModalCancelEmployee" ImageUrl="~/App_Themes/functions/cancel.png" />
        </li>
        <%-- <li>
            <asp:ImageButton ID="ImageBDelete" runat="server" data-toggle="modal" data-target="#myModalDeleteEmployee" ImageUrl="~/App_Themes/functions/delete.png" />
        </li>--%>
        <li>
            <asp:ImageButton ID="ImgeBtnEdit" runat="server" data-toggle="modal" data-target="#myModalEditEmployee" ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
        <li>
            <!-- Button to trigger modal new employee table form -->
            <asp:ImageButton ID="ImgBtnAdd" runat="server" data-toggle="modal" data-target="#myModalNewEmployee" ImageUrl="~/App_Themes/functions/add.png" />
        </li>
    </ul>

     <%--Javascript Section--%>
    <script type="text/javascript">
        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, Employee_ID, ID_Card, ID_Type, Last_Name, First_Name, Gender, Created_Note, Birth_Date, Country_ID, Office_ID) {
            var GridView = ckb.parentNode.parentNode.parentNode;
           
            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                }            
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

                /// Set text to textboxes for edition
                $('#Main_txtEditEmployeeCode').val(Employee_ID);
                $('#Main_txtEditIDCard').val(ID_Card);
                $('#Main_ddlEditIDType').val(ID_Type);
                $('#Main_txtEditLastName').val(Last_Name);
                $('#Main_txtEditFirstName').val(First_Name);
                $('#Main_ddlEditGender').val(Gender);
                $('#Main_txtEditNote').val(Created_Note);
                $('#Main_txtEditBirth_Date').val(Birth_Date);
                $('#Main_ddlEditNationality').val(Country_ID);
                $('#Main_ddlEditOfficeCode').val(Office_ID);

                $('#Main_hdOldEmployeeCode').val(Employee_ID);

                /// Delete Employee ID
                $('#Main_txtDeleteEmployeeID').val(Employee_ID + ' ?');
                $('#Main_hdfDeleteEmployeeID').val(Employee_ID);

                /// Cancel Employee ID
                $('#Main_txtCencelEmployeeID').val(Employee_ID + ' ?');
                $('#Main_hdfCancelEmployee').val(Employee_ID);

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                //Remove text from textboxes in edit form
                $('#Main_txtEditEmployeeCode').val("");
                $('#Main_txtEditIDCard').val("");
                $('#Main_ddlEditIDType').val("");
                $('#Main_txtEditLastName').val("");
                $('#Main_txtEditFirstName').val("");
                $('#Main_ddlEditGender').val("");
                $('#Main_txtEditNote').val("");
                $('#Main_txtEditBirth_Date').val("");
                $('#Main_ddlEditNationality').val("");
                $('#Main_ddlEditOfficeCode').val("");
            }
        }

        function ClearText()
        {
            $('#Main_txtEmployeeCodeModal').val("");
            $('#Main_txtIDCardModal').val("");
            $('#Main_txtFirstName').val("");
            $('#Main_txtLastName').val("");
            $('#Main_txtDateBirth').val("");
            $('#Main_txtNote').val("");

            $('#Main_txtEditEmployeeCode').val("");
            $('#Main_txtEditIDCard').val("");
            $('#Main_ddlEditIDType').val("");
            $('#Main_txtEditLastName').val("");
            $('#Main_txtEditFirstName').val("");
            $('#Main_ddlEditGender').val("");
            $('#Main_txtEditNote').val("");
            $('#Main_txtEditBirth_Date').val("");
            $('#Main_ddlEditNationality').val("");
            $('#Main_ddlEditOfficeCode').val("");
        }

        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        function GetEmployee_Code() {
            var Employee_ID = $('#Main_txtEmployeeCodeModal').val();
            var ID_Card = $('#Main_txtIDCardModal').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetEmployee_Code",
                data: "{Employee_ID:'" + Employee_ID + "',ID_Card:'" + ID_Card + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != "") {
                        alert(data.d);
                    }
                    else {
                        onclick_ok();
                    }
                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function GetEmployee_Code_Edit() {
            var Employee_ID = $('#Main_hdOldEmployeeCode').val();
            var ID_Card = $('#Main_txtEditIDCard').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetEmployee_Code_Edit",
                data: "{Employee_ID:'" + Employee_ID + "',ID_Card:'" + ID_Card + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != "") {
                        alert(data.d);
                    }
                    else {
                        onclick_ok_edit();
                    }
                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function onclick_ok() {
            var btnOk = document.getElementById('<%= btnOk.ClientID %>'); //dynamically click button

            btnOk.click();
        }

        function onclick_ok_edit() {
            var btnEdit = document.getElementById('<%= btnEdit.ClientID %>'); //dynamically click button

            btnEdit.click();
        }

 </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%-- Form Design Section--%>
    <br />
    <br />
    <br />

    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--all operation here--%>



            <!-- Modal New Employee Form -->
            <div id="myModalNewEmployee" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEmployeeHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalEmployeeHeader">New Employee Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  style="text-align: left; margin-left: 10px;">
                        <tr>
                            <td>Employee Code:</td>
                            <td>
                                <asp:TextBox ID="txtEmployeeCodeModal" runat="server" MaxLength="49" TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmployeeCode" runat="server" ControlToValidate="txtEmployeeCodeModal" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <%--<span style="color:red">*</span--%>
                            <td>ID. Card Number:</td>
                            <td>
                               <asp:TextBox ID="txtIDCardModal" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidatorIDCard" runat="server" ControlToValidate="txtIDCardModal" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>ID. Type:</td>
                            <td>
                            <asp:DropDownList ID="ddlIDType" class="span2" runat="server" TabIndex="4">
                                <asp:ListItem Value="1">ID Card</asp:ListItem>
                                <asp:ListItem Value="2">Passport</asp:ListItem>
                                 <asp:ListItem Value="3">Visa</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td>Last Name:</td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" TabIndex="5"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidatorLastName" runat="server" ControlToValidate="txtLastName" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                         <tr>
                            <td>First Name</td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" TabIndex="6"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorFirstName" runat="server" ControlToValidate="txtFirstName" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Gender:</td>
                            <td>
                            <asp:DropDownList ID="ddlGender" class="span2" runat="server" TabIndex="7">
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td>Birth of Date:</td>
                            <td>
                                  <asp:TextBox ID="txtDateBirth"  runat="server"  CssClass="datepicker span2" TabIndex="8" ></asp:TextBox>&nbsp;(DD-MM-YYYY)
                            </td>
                        </tr>
                        <tr>
                            <td>Nationality:</td>
                            <td>
                                <asp:DropDownList ID="ddlNationality" width="97.3%" height="25px" runat="server" TabIndex="9" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                    <asp:ListItem>.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Office Code:</td>
                             <td>
                                <asp:DropDownList ID="ddlOffice" width="97.3%" height="25px" runat="server" TabIndex="10" AppendDataBoundItems="True" DataSourceID="SqlDataSourceOffice" DataTextField="Detail" DataValueField="Office_ID">
                                    <asp:ListItem>.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Note:</td>
                            <td><asp:TextBox ID="txtNote" runat="server" TabIndex="11"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">

                    <div style="display: none;">
                       <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
                    </div>   

                    <input type="button" onclick="GetEmployee_Code()" id="btnOk_first" class="btn btn-primary" style="height: 27px; width:72px;" runat="server" value="OK" ValidationGroup="1" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick=" ClearText()">Cancel</button>
                </div>
            </div>
            <!--End Modal New Employee Form-->

             <!-- Modal Edit Employee Form -->
            <div id="myModalEditEmployee" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditEmployeeHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalEditEmployeeHeader">Update Employee Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  style="text-align: left; margin-left: 10px;">
                        <tr>
                            <td>Employee Code:</td>
                            <td>
                                <asp:TextBox ID="txtEditEmployeeCode" runat="server" MaxLength="49" TabIndex="1" ReadOnly="True"></asp:TextBox>
                                <asp:HiddenField ID="hdOldEmployeeCode" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEditEmployeeCode" runat="server" ControlToValidate="txtEditEmployeeCode" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <%--<span style="color:red">*</span--%>
                            <td>ID. Card Number:</td>
                            <td>
                               <asp:TextBox ID="txtEditIDCard" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidatorEditIDCard" runat="server" ControlToValidate="txtEditIDCard" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>ID. Type:</td>
                            <td>
                            <asp:DropDownList ID="ddlEditIDType" class="span2" runat="server" TabIndex="4">
                                <asp:ListItem Value="1">ID Card</asp:ListItem>
                                <asp:ListItem Value="2">Passport</asp:ListItem>
                                 <asp:ListItem Value="3">Visa</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td>Last Name:</td>
                            <td>
                                <asp:TextBox ID="txtEditLastName" runat="server" TabIndex="5"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidatorEditLastName" runat="server" ControlToValidate="txtEditLastName" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                         <tr>
                            <td>First Name</td>
                            <td>
                                <asp:TextBox ID="txtEditFirstName" runat="server" TabIndex="6"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEditFirstName" runat="server" ControlToValidate="txtEditFirstName" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Gender:</td>
                            <td>
                            <asp:DropDownList ID="ddlEditGender" class="span2" runat="server" TabIndex="7">
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td>Birth of Date:</td>
                            <td>
                                  <asp:TextBox ID="txtEditBirth_Date"  runat="server"  CssClass="datepicker span2" TabIndex="8" ></asp:TextBox>&nbsp;(DD-MM-YYYY)
                            </td>
                        </tr>
                        <tr>
                            <td>Nationality:</td>
                            <td>
                                <asp:DropDownList ID="ddlEditNationality" width="97.3%" height="25px" runat="server" TabIndex="9" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                    <asp:ListItem>.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Office Code:</td>
                             <td>
                                <asp:DropDownList ID="ddlEditOfficeCode" width="97.3%" height="25px" runat="server" TabIndex="10" AppendDataBoundItems="True" DataSourceID="SqlDataSourceOffice" DataTextField="Detail" DataValueField="Office_ID">
                                    <asp:ListItem>.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Note:</td>
                            <td><asp:TextBox ID="txtEditNote" runat="server" TabIndex="11"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                   <div style="display: none;">
                        <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
                    </div>
                    <input type="button" onclick="GetEmployee_Code_Edit()" id="btnEdit_first" class="btn btn-primary" style="height: 27px; width:72px;" runat="server" value="OK"  ValidationGroup="2" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick=" ClearText()">Cancel</button>
                </div>
            </div>
            <!--End Modal Edit Employee Form-->

            <!-- Modal Delete Employee Form -->
            <div id="myModalDeleteEmployee" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeleteEmployeeHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalDeleteEmployeeHeader">Delete Employee Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdfDeleteEmployeeID" runat="server" />
                                 Are you sure, you want to delete Employee Code <asp:TextBox ID="txtDeleteEmployeeID" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnDelete_Click"   />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Delete Employee Form-->

              <!-- Modal Cancel Employee Form -->
            <div id="myModalCancelEmployee" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalCancelEmployeeHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalCancelEmployeeHeader">Cancel Employee Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdfCancelEmployee" runat="server" />
                                 Are you sure, you want to cancel Employee Code <asp:TextBox ID="txtCencelEmployeeID" runat="server"  TabIndex="1" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Note:</td>
                        </tr>
                        <tr>
                              <td>
                                <asp:TextBox ID="txtCancelNote" runat="server" TabIndex="2" Width="90%" ></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidatorCancelEmployee" runat="server" ControlToValidate="txtCancelNote" ForeColor="Red" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCancel" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="3" OnClick="btnCancel_Click" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Cancel Employee Form-->


            <div class="panel panel-default">
                <div class="panel-heading">
                    
                <h3 class="panel-title">Employee</h3>
                </div>
                <div class="panel-body">
                    <%--Grid View--%>
                    <asp:GridView ID="GvEmployee" CssClass="grid-layout" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceEmployee">
                        <Columns>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                                <ItemTemplate> 
                                    <asp:CheckBox ID="ckb1" runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Employee_ID" ) + "\",\"" + Eval("ID_Card" ) + "\",\"" + Eval("ID_Type" ) + "\",\"" + Eval("Last_Name" ) + "\",\"" + Eval("First_Name" ) + "\",\"" + Eval("Gender" ) + "\",\"" + Eval("Created_Note" ) + "\",\"" + Eval("Birth_Date","{0:dd-MM-yyyy}" ) + "\",\"" + Eval("Country_ID" ) + "\",\"" + Eval("Office_ID" ) + "\");" %>' />
                                    <asp:HiddenField ID="hdfOldEmployeeID" runat="server" Value='<%# Bind("Employee_ID")%>' />
                                    <asp:HiddenField ID="hdfBirth_Date" runat="server" Value='<%# Bind("Birth_Date","{0:dd-MM-yyyy}")%>' />
                                    <asp:HiddenField ID="hdfCountry_ID" runat="server" Value='<%# Bind("Country_ID")%>' />
                                    <asp:HiddenField ID="hdfOffice_ID" runat="server" Value='<%# Bind("Office_ID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Employee_ID" HeaderText="Employee Code" />
                            <asp:BoundField DataField="ID_Card" HeaderText="ID. card number" />
                            <asp:BoundField DataField="ID_Type_Name" HeaderText="ID. type" />
                            <asp:BoundField DataField="Last_Name" HeaderText="Last Name" />
                            <asp:BoundField DataField="First_Name" HeaderText="First Name" />
                            <asp:BoundField DataField="Gender" HeaderText="Gender" />
                            <asp:BoundField DataField="Created_On" HeaderText="Updated on" DataFormatString="{0:dd-MM-yyyy}" />
                            <asp:BoundField DataField="Created_By" HeaderText="Updated by" />
                            <asp:BoundField DataField="Created_Note" HeaderText="Note" />
                            <asp:BoundField DataField="Birth_Date" DataFormatString="dd-mm-yyyy" HeaderText="Birth_Date" Visible="False" />
                            <asp:BoundField DataField="Country_ID" HeaderText="Country_ID" Visible="False" />
                            <asp:BoundField DataField="Office_ID" HeaderText="Office_ID" Visible="False" />
                            <asp:BoundField DataField="ID_Type" HeaderText="ID Type Value" Visible="False" />
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" />
                        <RowStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </div>
            </div>
 
             <%-- Section Hidenfields Initialize  --%>
             <asp:HiddenField ID="hdfuserid" runat="server" />
             <asp:HiddenField ID="hdfusername" runat="server" />
             <asp:HiddenField ID="hdftotalagentrow" runat="server" />
     
            <%-- End Section Hidenfields Initialize  --%>
   
            <!--- Section Sqldatasource--->
            <asp:SqlDataSource ID="SqlDataSourceEmployee" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Employee_List]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceNationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Nationality "></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceOffice" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT  Office_ID, Detail FROM  dbo.Ct_Office "></asp:SqlDataSource>
           <%-- End Section  --%>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
            <asp:PostBackTrigger ControlID="btnEdit"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnDelete"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnCancel"></asp:PostBackTrigger>
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

