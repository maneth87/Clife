<%@ Page Title="Clife | WING => Manage Account" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="wing_account.aspx.cs" Inherits="Pages_Wing_wing_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearchWingAccount" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnEdit" data-toggle="modal" data-target="#myModalEditWingAccount" runat="server" ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
        
        <li>
            <asp:ImageButton ID="ImgBtnAdd" data-toggle="modal" data-target="#myModalWINGAccount" runat="server" ImageUrl="~/App_Themes/functions/add.png" />
        </li>


    </ul>

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script>

        //Fucntion to check only one checkbox and highlight textbox
        function SelectSingleCheckBox(ckb, policy_wing_id) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            $('#Main_hdfPolicyWingID').val(policy_wing_id);

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");

                }
            }

            if (ckb.checked) {
               
                //Call web service to populate component on edit modal
                $.ajax({
                    type: "POST",
                    url: "../../WingWebService.asmx/GetWINGObject",
                    data: "{policy_wing_id:'" + policy_wing_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtEditWingSK').val(data.d.WING_SK);
                        $('#Main_txtEditWingNumber').val(data.d.WING_Number);
                        $('#Main_txtEditWingPolicyNumber').val(data.d.Policy_Number);
                        $('#Main_txtEditWingCustomerName').val(data.d.Customer_Name);
                        $('#Main_ddlEditWingGender').val(data.d.Gender);
                        $('#Main_ddlEditWingIDType').val(data.d.ID_Type);
                        $('#Main_txtEditWingIDNumber').val(data.d.ID_Number);
                        $('#Main_txtEditWingDob').val(formatJSONDate(data.d.Birth_Date));
                        $('#Main_txtEditWingContactNumber').val(data.d.Contact_Number);

                        $('#Main_txtEditWingCreatedOn').val(formatJSONDate(data.d.Created_On));

                        $('#Main_hdfPolicyID').val(data.d.Policy_ID);

                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });


                

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");
            }
        }


        //Format Date in Jtemplate
        function formatJSONDate(jsonDate) {
            var value = jsonDate;
            if (value.substring(0, 6) == "/Date(") {
                var dt = new Date(parseInt(value.substring(6, value.length - 2)));
                var dtString = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                value = dtString;
            }

            return value;

        }



        //Display date picker
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>


            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Wing Account for Policy Holders</h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvWingAccount" runat="server" CssClass="grid-layout" AutoGenerateColumns="False" DataSourceID="WINGAccountDataSource" Width="100%" HorizontalAlign="Center" PageSize="100" OnRowDataBound="gvWingAccount_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_WING_ID" ) + "\");" %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Date_Request" HeaderText="Date Request" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Date_Request">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WING_SK" HeaderText="SK" SortExpression="WING_SK">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Wing_Number" HeaderText="Wing #" SortExpression="Wing_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Customer_Name" HeaderText="Name" ReadOnly="True" SortExpression="Customer_Name">
                                <ItemStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Gender" HeaderText="Gender" ReadOnly="True" SortExpression="Gender">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ID_Type" HeaderText="ID Type" ReadOnly="True" SortExpression="ID_Type">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            

                            <asp:BoundField DataField="ID_Number" HeaderText="ID Number" SortExpression="ID_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Birth_Date" HeaderText="Date of Birth" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Birth_Date">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Contact_Number" HeaderText="Contact No." SortExpression="Contact_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number" SortExpression="Policy_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Created_On" HeaderText="Created On" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Created_On">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>


                    <asp:SqlDataSource ID="WINGAccountDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK ORDER BY Created_On DESC, Ct_Wing_Account.SK DESC"></asp:SqlDataSource>

                    <asp:SqlDataSource ID="WINGAccountForDDL" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT SK FROM Ct_Wing_Account WHERE Status = 1 ORDER BY Created_On ASC, SK ASC"></asp:SqlDataSource>


                    <%--Modal for Add New Wing Account--%>
                    <div id="myModalWINGAccount" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalWingAccountHeader" aria-hidden="true">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 class="panel-title">Add WING Account for Policy Holder</h3>
                        </div>

                        <div class="modal-body">
                            <!---Modal Body--->
                            <table width="100%" style="text-align: left; padding-left: 20px;">
                               <tr>
                                    <td width="100px">&nbsp;&nbsp;POLICY No:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumber" runat="server" Width="90%" Height="20" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPolicyNumber" ControlToValidate="txtPolicyNumber" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;WING SK:</td>
                                    <td>
                                        <%--<asp:TextBox ID="txtWINGSK" runat="server" Width="90%" ></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlWINGSK" runat="server" DataSourceID="WINGAccountForDDL" Width="94%" DataTextField="SK" DataValueField="SK" TabIndex="2"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" ControlToValidate="ddlWINGSK" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </td>


                                </tr>
                                 
                            </table>
                            <br />

                        </div>

                        <div class="modal-footer">                            
                            <asp:Button ID="btnSave" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                        </div>
                    </div>
                    <%--End Modal for Add New Wing Account--%>

                    <%--Modal for Edit Wing Account--%>
                    <div id="myModalEditWingAccount" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditWingAccountHeader" aria-hidden="true">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 class="panel-title">Edit WING Account for Policy Holder</h3>
                        </div>

                        <div class="modal-body">
                            <!---Modal Body--->
                            <table width="100%" style="text-align: left; padding-left: 20px;">
                               
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;WING SK:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingSK" runat="server" Width="90%"></asp:TextBox>
                                        
                                    </td>


                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;WING No.:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingNumber" runat="server" Width="90%" ></asp:TextBox>
                                    </td>


                                </tr>
                                 <tr>
                                    <td width="100px">&nbsp;&nbsp;POLICY No.:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingPolicyNumber" runat="server" Width="90%" ></asp:TextBox>
                                        
                                    </td>

                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;Name:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingCustomerName" runat="server" Width="90%" ></asp:TextBox>
                                        
                                    </td>

                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;Gender:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlEditWingGender" runat="server">
                                            <asp:ListItem>Male</asp:ListItem>
                                            <asp:ListItem>Female</asp:ListItem>                                            
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;ID TYPE:</td>
                                    <td>                                        
                                        <asp:DropDownList ID="ddlEditWingIDType" runat="server">
                                            <asp:ListItem>ID Card</asp:ListItem>
                                            <asp:ListItem>Passport</asp:ListItem>
                                            <asp:ListItem>Visa</asp:ListItem>
                                            <asp:ListItem>Birth Certificate</asp:ListItem>
                                            <asp:ListItem>Driving License</asp:ListItem>
                                            <asp:ListItem>Monk Card</asp:ListItem>
                                            <asp:ListItem>Police / Civil Service Card</asp:ListItem>
                                            <asp:ListItem>Employment Book</asp:ListItem>
                                            <asp:ListItem>Voter ID</asp:ListItem>
                                            <asp:ListItem>Residential Book</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>

                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;ID Number:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingIDNumber" runat="server" Width="90%" ></asp:TextBox>
                                        
                                    </td>

                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;DOB:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingDob" runat="server" Width="90%" CssClass="datepicker" ></asp:TextBox>
                                        
                                    </td>

                                </tr>
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;Contact No.:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingContactNumber" runat="server" Width="90%" ></asp:TextBox>
                                        
                                    </td>

                                </tr>
                               
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;Created On:</td>
                                    <td>
                                        <asp:TextBox ID="txtEditWingCreatedOn" runat="server" Width="90%" CssClass="datepicker" ></asp:TextBox>
                                        
                                    </td>

                                </tr>
                            </table>
                            <br />

                        </div>

                        <div class="modal-footer">                            
                            <asp:Button ID="btnUpdate" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                        </div>
                    </div>
                    <%--End Modal for Add New Wing Account--%>

                    
            <!-- Modal Search WING Account -->
            <div id="myModalSearchWingAccount" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchWINGAccountHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search WING Account</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SAppNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By Pol #</a></li>
                        
                        <li><a href="#SKNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By WING SK</a></li>  
                        <li><a href="#WINGNO" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">WING #</a></li>   
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By Customer Name</a></li>                    
                    </ul>

                    <div class="tab-content" style="height: 60px; overflow: hidden;">
                        <div class="tab-pane active" id="SAppNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Policy No:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumberSearch" Width="95%" runat="server"></asp:TextBox>

                                    </td>
                                    

                                </tr>
                            </table>

                        </div>
                        <div class="tab-pane active" id="SKNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;WING SK:</td>
                                    <td>
                                        <asp:TextBox ID="txtSearchWingSK" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>
                            </table>

                        </div>
                        <div class="tab-pane active" id="WINGNO">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;WING No.:</td>
                                    <td>
                                        <asp:TextBox ID="txtWingNo" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>
                            </table>
                        </div>
                        <div class="tab-pane" style="height: 70px;" id="SCustomerName">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Surname:</td>
                                    <td>
                                        <asp:TextBox ID="txtSurnameSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                   
                                </tr>
                                 <tr>
                                    <td width="70px;">&nbsp;&nbsp;Firstname:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstnameSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>
                            </table>

                        </div>                        
                    </div>
                   <br />
                </div>
                <div class="modal-footer">                    
                    <asp:Button ID="btnSearchWING" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Search" OnClick="btnSearchWING_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Search Policy-->

                </div>
                
            </div>
            
            <asp:HiddenField ID="hdfPolicyWingID" runat="server" />
            <asp:HiddenField ID="hdfPolicyID" runat="server" />
            

        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnUpdate" />
            <asp:PostBackTrigger ControlID="btnSearchWING" />
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

