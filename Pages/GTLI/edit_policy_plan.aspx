<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="edit_policy_plan.aspx.cs" Inherits="Pages_GTLI_edit_policy_plan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />

    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>

    <ul class="toolbar">
         <li>
            <input type="button"  onclick="DeletePolicyPlan()" style="background: url('../../App_Themes/functions/delete.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
          <li>
            <input type="button"  onclick="EditPolicyPlan()" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <!-- Button Search-->
             <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" /> 
        </li>
    </ul>
    <style>
        .IndentLeft {
            padding-left: 5px;          
        }

        .IndentRight {
            padding-right: 5px;            
        }
        .IndentBottom {
            padding-bottom: 7px;
        }
        .ui-autocomplete {
            z-index: 5000;
        }
    </style>
    <script type="text/javascript">
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

        //Edit Policy Plan
        function EditPolicyPlan() {
            var plan_id = $('#Main_hdfPlanID').val();
            if (plan_id == "") {
                alert("No Plan to edit.");
            } else {
                $('#myModalEditPlan').modal('show');
            }
        }

        //Delete Plan
        function DeletePolicyPlan() {
            var plan_id = $('#Main_hdfPlanID').val();
            if (plan_id == "") {
                alert("No plan to delete.");
            } else {
                $('#myModalDeletePlan').modal('show');
            }
        }

        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, plan_id, plan_name, sum_insured, tpd, dhc, accidental_100plus) {
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

                $('#Main_hdfPlanID').val(plan_id);
                $('#Main_hdfPlanName').val(plan_name);

                /// Set text to textboxes for edition                
                $('#Main_txtPlanNameEdit').val(plan_name);
                $('#Main_txtSumInsuredEdit').val(sum_insured);

                //Tpd check/uncheck
                if (tpd == 1) {
                    $('#Main_rbtnlTPD_0').prop("checked", true);
                }else{
                    $('#Main_rbtnlTPD_1').prop("checked", true);
                }
            
                //Accidenatal check/uncheck
                if (accidental_100plus == 1) {
                    $('#Main_rbtnlAccidental100Plus_0').prop("checked", true);
                } else {
                    $('#Main_rbtnlAccidental100Plus_1').prop("checked", true);
                }

                $('#Main_ddlDHC').val(dhc);

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                $('#Main_hdfPlanID').val("");
                $('#Main_hdfPlanName').val("");

                //Remove text from textboxes in edit form                                         
                $('#Main_txtPlanNameEdit').val("");
                $('#Main_txtSumInsuredEdit').val("");
                $('#Main_rbtnlTPD_0').prop("checked", true);
                $('#Main_rbtnlAccidental100Plus_0').prop("checked", true);
                $('#Main_ddlDHC').val(0);

            }
        }

        //Show Search Form
        function ShowSearchForm() {
            $("#txtCompanyName").val("");

            $("#myModalSearchPlan").modal("show");
        }

        //Dynamic Sum Insure
        function CheckDynamicSumInsure(ckb) {
            if (ckb.checked) {
                $('#Main_txtSumInsuredEdit').val(0);
                $('#Main_txtVirtualAmount').val(2500);

                $('#Main_txtSumInsuredEdit').prop("disable", "disable");

            } else {

                $('#Main_txtSumInsuredEdit').removeAttr("disable");
                $('#Main_txtVirtualAmount').val(0);
            }
        }

        //Get amount change
        function AmountChange(value) {
            $('#Main_txtVirtualAmount').val(value);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />   
    
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Edit Policy Plan</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <br />
                    
            <asp:GridView ID="gvPlan" runat="server" Font-Names="Arial" Width="100%"
                AutoGenerateColumns="False"  DataKeyNames="GTLI_Plan_ID" OnRowDataBound="gvPlan_RowDataBound">
                <Columns>
                     <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="IndentBottom" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("GTLI_Plan_ID" ) + "\", \"" + Eval("GTLI_Plan" ) + "\", \"" + Eval("Sum_Insured" ) + "\", \"" + Eval("TPD" ) + "\", \"" + Eval("DHC_Option_Value" ) + "\", \"" + Eval("Accidental_100Plus" ) + "\");" %>' />
                            
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Plan Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblplan_name" Text='<%# Bind("GTLI_Plan")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Insured Amount" ItemStyle-CssClass="IndentRight" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="250" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblsum_insured" Text='<%# Bind("Sum_Insured")%>' runat="server"></asp:Label>
                        </ItemTemplate>                      

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TPD" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblTPD" runat="server" Text='<%# Eval("TPD")%>'></asp:Label>
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="TPD" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblAccidental100Plus" runat="server" Text='<%# Eval("Accidental_100Plus")%>'></asp:Label>
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DHC" ItemStyle-Width="180" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbldhc" Text='<%# Bind("DHC_Option_Value")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                       
                    </asp:TemplateField>               
                 
                </Columns>
            </asp:GridView>
         
        </div>
    </div>

    <div style="text-align: center; width: 100%">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="1" ShowMessageBox="True" ShowSummary="False" />
    </div>
           <!-- Modal Edit Plan Form -->
    <div id="myModalEditPlan" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalPlanEditHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Update Plan Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td width="100px">Plan Name:</td>
                     <td>
                        <asp:TextBox Width="60%" ID="txtPlanNameEdit" runat="server" Height="15" MaxLength="50" ValidationGroup="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPlanNameEdit" runat="server" ErrorMessage="Rquire Plan" Text="*" ControlToValidate="txtPlanNameEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>   
                <tr>                    
                    <td colspan="2">
                         <asp:CheckBox ID="ckbDynamicSumInsure" runat="server" Text="Dynamic Sum Insure" onclick="CheckDynamicSumInsure(this);" />
                    </td>
                </tr>            
                <tr>
                    <td width="100px">Insured Amount:<span style="color: red">*</span></td>
                     <td>
                        <asp:TextBox Width="60%" ID="txtSumInsuredEdit" runat="server" Height="15" MaxLength="10" onkeyup="ValidateNumber(this)" ValidationGroup="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSumInsuedEdit" runat="server" ErrorMessage="Rquire Insured Amount" Text="*" ControlToValidate="txtSumInsuredEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="AmountRangeValidator" runat="server" ControlToValidate="txtVirtualAmount"
                                MaximumValue="999999999" MinimumValue="2500" Type="Double" ErrorMessage="Must be between 2500 to 999,999,999"
                                Display="static" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" Text="*" ValidationGroup="2"></asp:RangeValidator>
                    </td>
                </tr>  
                <tr>
                    <td width="100px">TPD:</td>
                     <td>
                      <asp:RadioButtonList ID="rbtnlTPD" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                     
                    </td>
                </tr>
                <tr>
                    <td width="100px">Accidental 100Plus:</td>
                     <td>
                      <asp:RadioButtonList ID="rbtnlAccidental100Plus" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                     
                    </td>
                </tr>
                 <tr>
                    <td width="100px">DHC:</td>
                     <td>
                        <asp:DropDownList ID="ddlDHC" Width="61%" Height="36px" runat="server" >
                                <asp:ListItem Value="0">No</asp:ListItem>
                                <asp:ListItem Value="10">Option 1: 10$/year</asp:ListItem>
                                <asp:ListItem Value="20">Option 2: 20$/year</asp:ListItem>
                                <asp:ListItem Value="30">Option 3: 30$/year</asp:ListItem>
                            </asp:DropDownList>
                       
                    </td>
                </tr>            
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnUpdate" class="btn btn-primary" Style="height: 27px;" TabIndex="9" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="2" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Edit Contact Form-->

     <!-- Modal Delete Plan Form -->
    <div id="myModalDeletePlan" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeletePlanHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="myModalDeleteBusinessHeader">Delete Plan Confirm</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="text-align: left;">
                <tr>
                    <td>Are you sure, you want to delete this plan?                        
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Delete" OnClick="btnDelete_Click" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Delete Plan Form-->

     <%--Modal Search Plan Form --%>
    <div id="myModalSearchPlan" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPlanHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Plan Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
           <table width="100%">
                <tr>
                    <td width="60px" >Company:</td>
                    <td width="87%">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="96%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" MaxLength="255"></asp:TextBox>
                
                    </td>  
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSearchContact" runat="server" Text="*" ForeColor="Red" ValidationGroup="3" ControlToValidate="txtCompanyName"  ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                          
                </tr>       
            </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"  />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Plan Search--%>

     <%--Hidden Fields--%>
    <asp:HiddenField ID="hdfPlanID" runat="server" />
    <asp:HiddenField ID="hdfPlanName" runat="server" />
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
    <asp:HiddenField ID="hdfCompanyID" runat="server" />

    <div style="display:none">
        <asp:TextBox ID="txtVirtualAmount" runat="server" ></asp:TextBox>
    </div>

</asp:Content>

