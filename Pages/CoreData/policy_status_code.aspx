<%@ Page Title="Clife | Core Data => Policy Status" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_status_code.aspx.cs" Inherits="Pages_CoreData_policy_status_code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImageBDelete" runat="server"  data-toggle="modal" data-target="#myModalDeletePolicy"  ImageUrl="~/App_Themes/functions/delete.png" />
        </li>
        <li> 
            <asp:ImageButton ID="ImgeBtnEdit"  runat="server" data-toggle="modal" data-target="#myModalEditPolicy"  ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
         <li>
             <!-- Button to trigger modal new policy form -->           
            <asp:ImageButton ID="ImgBtnAdd" runat="server"   data-toggle="modal" data-target="#myModalNewPolicy" ImageUrl="~/App_Themes/functions/add.png" />
        </li>
    </ul>

     <script type="text/javascript">
       
         function SelectSingleCheckBox(ckb, Policy_Status_Type_ID, Policy_Status_Code, Detail, Terminated, Disabled, Is_Reserved, Created_Note) {
             var GridView = ckb.parentNode.parentNode.parentNode;

             var ckbList = GridView.getElementsByTagName("input");
             for (i = 0; i < ckbList.length; i++) {
                 if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {
                     var current_id = ckbList[i].id;
                     var sub_current_id = current_id.substring(15, 19);

                     if (sub_current_id == "ckb1") {
                         ckbList[i].checked = false;
                         $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                     }
                 }
             }

             if (ckb.checked) {
                 $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

                 /// Set text to textboxes for edition
                 $('#Main_txtEditPolicyStatusIDModal').val(Policy_Status_Type_ID);
                 $('#Main_txtEditPolicyCode').val(Policy_Status_Code);
                 $('#Main_txtEditDetail').val(Detail);

                 if (Terminated == "True") {

                     $('#Main_chbEditTerminated').prop('checked', true);
                 }
                 else { $('#Main_chbEditTerminated').prop('checked', false); }

                 if (Disabled == "True") {
                     $('#Main_chbEditDisable').prop('checked', true);
                 }
                 else { $('#Main_chbEditDisable').prop('checked', false); }

                 if (Is_Reserved == "True") {
                     $('#Main_chbEditReserved').prop('checked', true);
                 }
                 else { $('#Main_chbEditReserved').prop('checked', false); }


                 $('#Main_txtEditNote').val(Created_Note);

                 $('#Main_hdfPolicyID').val(Policy_Status_Type_ID);

                 /// Delete Relationship ID
                 $('#Main_txtDeletPolicy').val(Policy_Status_Type_ID + ' ?');
                 $('#Main_hdfDeletePolicyID').val(Policy_Status_Type_ID);

             } else {
                 $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                 //Remove text from textboxes in edit form
                 $('#Main_txtEditStatusCode').val("");
                 $('#Main_txtEditDetail').val("");
                 $('#Main_chbEditInforce').val("");
                 $('#Main_chbEditReserved').val("");
                 $('#Main_txtEditNote').val("");

             }
         }

         function GetPolicy_Status() {
             var Policy_Status_Type_ID = $('#Main_txtPolicyStatusIDModal').val();
             var Policy_Status_Code = $('#Main_txtPolicyCode').val();

             $.ajax({
                 type: "POST",
                 url: "../../SaleAgentWebService.asmx/GetPolicy_Status",
                 data: "{Policy_Status_Type_ID:'" + Policy_Status_Type_ID + "',Policy_Status_Code:'" + Policy_Status_Code + "'}",
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

         function GetPolicy_Status_Edit() {
             var Policy_Status_Type_ID = $('#Main_hdfPolicyID').val();
             var Policy_Status_Code = $('#Main_txtEditPolicyCode').val();

             $.ajax({
                 type: "POST",
                 url: "../../SaleAgentWebService.asmx/GetPolicy_Status_Edit",
                 data: "{Policy_Status_Type_ID:'" + Policy_Status_Type_ID + "',Policy_Status_Code:'" + Policy_Status_Code + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {

                     if (data.d != "") {
                         alert(data.d);
                     }
                     else {

                         onclick_ok_eidt();
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

         function onclick_ok_eidt() {
             var btnEdit = document.getElementById('<%= btnEdit.ClientID %>'); //dynamically click button

             btnEdit.click();
         }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%--here is the block code--%>

          <!-- Modal New Policy Form -->
            <div id="myModalNewPolicy"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalPolicyHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalRelationshipHeader">New Policy Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table text-align:left;">
                    <tr>
                        <td>Policy status ID:</td>
                        <td>
                            <asp:TextBox ID="txtPolicyStatusIDModal"  runat="server" MaxLength="49" TabIndex="1"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorStatusID" ControlToValidate="txtPolicyStatusIDModal" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Policy status code:</td>
                        <td>
                            <asp:TextBox ID="txtPolicyCode"  runat="server" MaxLength="49" TabIndex="2"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtPolicyCode" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Detail:</td>
                        <td>
                            <asp:TextBox ID="txtDetail"  runat="server" MaxLength="49" TabIndex="3"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtDetail" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Terminated:</td>
                        <td>
                            <asp:CheckBox id="chbTerminated" runat="server" TabIndex="4"/>&nbsp;&nbsp;&nbsp;(Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Disable:</td>
                        <td>
                            <asp:CheckBox id="chbDisable" runat="server" TabIndex="5"/>&nbsp;&nbsp;&nbsp;(Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Is reserved:</td>
                        <td>
                             <asp:CheckBox id="chbReserved" runat="server" TabIndex="6"/>&nbsp;&nbsp;&nbsp;(Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtNote"  runat="server" MaxLength="49" TabIndex="7" ></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtNote" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer"> 
                  
                <div style="display: none;">
                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
                </div>

                <input type="button" onclick="GetPolicy_Status()" id="btnOk_firUnderwritest" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK"   ValidationGroup="1"  />
                        
                <button class="btn"  data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>
          <!--End Modal New Policy Form--> 

          <!-- Modal Edit Policy Form -->
            <div id="myModalEditPolicy"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditPolicyHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalEditPolicyHeader">New Policy Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table text-align:left;">
                    <tr>
                        <td>Policy status ID:</td>
                        <td>
                            <asp:HiddenField ID="hdfPolicyID" runat="server" />
                            <asp:TextBox ID="txtEditPolicyStatusIDModal"  runat="server" MaxLength="49" TabIndex="1"  Enabled="false" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtEditPolicyStatusIDModal" runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Policy status code:</td>
                        <td>
                            <asp:TextBox ID="txtEditPolicyCode"  runat="server" MaxLength="49" TabIndex="2"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtEditPolicyCode" runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Detail:</td>
                        <td>
                            <asp:TextBox ID="txtEditDetail"  runat="server" MaxLength="49" TabIndex="3"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtEditDetail" runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Terminated:</td>
                        <td>
                            <asp:CheckBox id="chbEditTerminated" runat="server" TabIndex="4"/>&nbsp;&nbsp;&nbsp;(Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Disable:</td>
                        <td>
                            <asp:CheckBox id="chbEditDisable" runat="server" TabIndex="5"/>&nbsp;&nbsp;&nbsp;(Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Is reserved:</td>
                        <td>
                             <asp:CheckBox id="chbEditReserved" runat="server" TabIndex="6"/>&nbsp;&nbsp;&nbsp;(Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtEditNote"  runat="server" MaxLength="49" TabIndex="7" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer"> 
                  
               <div style="display: none;">
                    <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
                </div>

                <input type="button" onclick="GetPolicy_Status_Edit()" id="btnEdit_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK"   ValidationGroup="2"  />
            
                <button class="btn"  data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>
          <!--End Modal Edit Policy Form--> 

         <!-- Modal Delete Underwrite Form -->
            <div id="myModalDeletePolicy" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeletePolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalDeletePolicyHeader">Delete Policy Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdfDeletePolicyID" runat="server" />
                                    Are you sure, you want to delete Policy Status Type ID <asp:TextBox ID="txtDeletPolicy" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnDelete_Click"    />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
        <!--End Modal Delete Underwrite Form-->

             <div class="panel panel-default" >
                <div class="panel-heading">
                    
                <h3 class="panel-title">Policy Status</h3>
                </div>
                <div class="panel-body">
                    <%--Grid View--%>                    
                    <asp:GridView ID="GvOPolicy" CssClass="grid-layout" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourcePolicy">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1"  runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Status_Type_ID") + "\", \"" + Eval("Policy_Status_Code") + "\", \"" + Eval("Detail") + "\", \"" + Eval("Terminated") + "\", \"" + Eval("Disabled") + "\", \"" + Eval("Is_Reserved") + "\", \"" + Eval("Created_Note") + "\");"%>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Policy status type ID." DataField="Policy_Status_Type_ID" />
                            <asp:BoundField DataField="Policy_Status_Code" HeaderText="Policy status code" />
                            <asp:BoundField DataField="Detail" HeaderText="Detail" />
                            <asp:CheckBoxField DataField="Terminated" HeaderText="Terminated" />
                            <asp:CheckBoxField DataField="Disabled" HeaderText="Disabled" />
                            <asp:BoundField DataField="Parent_PST_ID" HeaderText="Parent PST.ID." />
                            <asp:CheckBoxField DataField="Is_Reserved" HeaderText="Is reserved" />
                            <asp:BoundField DataField="Created_On" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Updated On" />
                            <asp:BoundField DataField="Created_By" HeaderText="Updated By" />
                            <asp:BoundField DataField="Created_Note" HeaderText="Note" />
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" />
                        <RowStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </div>
            </div>
             <asp:SqlDataSource ID="SqlDataSourcePolicy" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Policy_Status_Code]" ></asp:SqlDataSource>

	</ContentTemplate>

   <Triggers>
    <asp:PostBackTrigger ControlID="btnOk" />
    <asp:PostBackTrigger ControlID="btnEdit" />
    <asp:PostBackTrigger ControlID="btnDelete" />
   </Triggers>

    </asp:UpdatePanel>	
</asp:Content>

