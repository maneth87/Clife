<%@ Page Title="Clife | Core Data => Office Location" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="office_table.aspx.cs" Inherits="Pages_CoreData_office_table" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImageBDelete" runat="server"  data-toggle="modal" data-target="#myModalDeleteForm"  ImageUrl="~/App_Themes/functions/delete.png" />
        </li>
         <li>
            <asp:ImageButton ID="ImgBView" runat="server" data-toggle="modal" data-target="#myModalViewOffice"  ImageUrl="~/App_Themes/functions/view.png"  />
        </li>
        <li> 
            <asp:ImageButton ID="ImgeBtnEdit"  runat="server" data-toggle="modal" data-target="#myModalEditOffice"  ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
         <li>
             <!-- Button to trigger modal new office table form -->           
            <asp:ImageButton ID="ImgBtnAdd" runat="server"   data-toggle="modal" data-target="#myModalNewOffice" ImageUrl="~/App_Themes/functions/add.png" />
        </li>
    </ul>

    <%--Javascript Section--%>
    <script type="text/javascript">
        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, office_id, detail, note,created_by,created_on) {
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
                $('#Main_txtEditOfficeID').val(office_id);
                $('#Main_txtEditDetail').val(detail);
                $('#Main_txtEditNote').val(note);
                $('#Main_hdOldOfficeID').val(office_id);

                /// View 
                $('#Main_txtViewOfficeID').val(office_id);
                $('#Main_txtViewOfficeDetail').val(detail);
                $('#Main_txtViewCreated_On').val(created_on);
                $('#Main_txtViewCreated_By').val(created_by);
                $('#Main_txtViewNote').val(note);

                /// Delete Office ID
                $('#Main_txtDeleteOfficeID').val(office_id + ' ?');
                $('#Main_hdDeleteOfficeID').val(office_id);

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                //Remove text from textboxes in edit form
                $('#Main_txtEditOfficeID').val("");
                $('#Main_txtEditDetail').val("");
                $('#Main_txtEditNote').val("");
                $('#Main_hdOldOfficeID').val("");
                $('#Main_txtDeleteOfficeID').val("");
                $('#Main_hdDeleteOfficeID').val("");
            }
        }

        function ClearText() {
            $('#Main_txtEditOfficeID').val("");
            $('#Main_txtEditDetail').val("");
            $('#Main_txtEditNote').val("");
            $('#Main_hdOldOfficeID').val("");
            $('#Main_txtDeleteOfficeID').val("");
            $('#Main_hdDeleteOfficeID').val("");
        }


        function GetOffice_Code() {
            var Office_ID = $('#Main_txtOfficeCodeModal').val();
            var Detail = $('#Main_txtOfficeDetailModal').val();


            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetOffice_Code",
                data: "{Office_ID:'" + Office_ID + "',Detail:'" + Detail + "'}",
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

        function GetOffice_Code_Edit() {
            var Office_ID = $('#Main_hdOldOfficeID').val();
            var Detail = $('#Main_txtEditDetail').val();


            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetOffice_Code_Edit",
                data: "{Office_ID:'" + Office_ID + "',Detail:'" + Detail + "'}",
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

<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
   
    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--all operation here--%>

             <!-- Modal New Application Form -->
            <div id="myModalNewOffice"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalOfficeHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalOfficeHeader">New Office Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table style="text-align:left; margin-left: 10px;">
                    <tr>
                        <td>Office Code:</td>
                        <td>
                            <asp:TextBox ID="txtOfficeCodeModal"  runat="server" MaxLength="49" Width="400" TabIndex="1" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorOfficeCode" ControlToValidate="txtOfficeCodeModal" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <%--<span style="color:red">*</span--%>
                        <td>Detail:</td>
                        <td>
                             <asp:TextBox ID="txtOfficeDetailModal"  runat="server" MaxLength="254" Width="400" TabIndex="2" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorOfficeDetail" ControlToValidate="txtOfficeDetailModal" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtNoteModal"  runat="server" Width="400" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">  
                 
                <div style="display: none;">
                       <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
                </div>

               <input type="button" onclick="GetOffice_Code()" id="btnOk_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK" ValidationGroup="1" />
            
                <button class="btn"  data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>
            <!--End Modal New Application Form--> 

            <!-- Modal Edit Application Form -->
            <div id="myModalEditOffice"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myEditModalOfficeHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myEditModalOfficeHeader">Update Office Form</h3>

            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table  style="text-align:left; margin-left: 10px;">
                    <tr>
                        <td>Office Code:</td>
                        <td>
                            <asp:TextBox ID="txtEditOfficeID"  runat="server" MaxLength="49" Width="400" TabIndex="1" ReadOnly="False" Enabled="False"></asp:TextBox>
                            <asp:HiddenField ID="hdOldOfficeID" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEditOfficeID" runat="server" ControlToValidate="txtEditOfficeID" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Detail:</td>
                        <td>
                             <asp:TextBox ID="txtEditDetail"  runat="server" MaxLength="254" Width="400" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEditOfficeDetail" runat="server"  ControlToValidate="txtEditDetail" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtEditNote"  runat="server" Width="400" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">   
              <div style="display: none;">
                  <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
              </div>
                     
              <input type="button" onclick="GetOffice_Code_Edit()" id="btnEdit_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="Update"  ValidationGroup="2" />

              <button class="btn"  data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>
            <!--End Modal Edit Application Form--> 

            <!-- Modal View Detail Form-->
            <div id="myModalViewOffice"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myViewModalOfficeHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myViewModalOfficeHeader">View Office Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table style="text-align:left; margin-left: 10px;">
                    <tr>
                        <td>Office Code:</td>
                        <td>
                            <asp:TextBox ID="txtViewOfficeID" BorderStyle="None" runat="server" Width="400" ReadOnly="True" BackColor="White"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Office Detail:</span></td>
                        <td>
                              <asp:TextBox ID="txtViewOfficeDetail" BorderStyle="None" runat="server" Width="400" ReadOnly="True" BackColor="White"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Created On:</span></td>
                        <td>
                             <asp:TextBox ID="txtViewCreated_On" BorderStyle="None" runat="server" Width="400" CssClass="datepicker" ReadOnly="True" BackColor="White" TextMode="Date"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>Created By:</span></td>
                        <td>
                              <asp:TextBox ID="txtViewCreated_By" BorderStyle="None" runat="server" Width="400"  ReadOnly="True" BackColor="White"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtViewNote" BorderStyle="None" runat="server" Width="400" ReadOnly="True" BackColor="White" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">   
              <button class="btn btn-primary"  data-dismiss="modal" aria-hidden="true">Ok</button>
            </div>
            </div>
            <!--End of Modal View Form-->

             <!-- Modal Delete Form-->
            <div id="myModalDeleteForm"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myDeleteModalOfficeHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myDeleteModalOfficeHeader"> Delete Office Code </h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table>
                    <tr>
                        <td>
                               <asp:HiddenField ID="hdDeleteOfficeID" runat="server" />
                            Are you sure, you want to delete Office Code <asp:TextBox ID="txtDeleteOfficeID" BorderStyle="None" runat="server" Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">   
              <asp:Button ID="btnDelete" class="btn btn-primary" style="height:27px;" runat="server" Text="Ok" OnClick="btnDelete_Click"  />
              
                <button class="btn"  data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>
            <!--End of Modal Delete Form-->


            <div class="panel panel-default" >
                <div class="panel-heading">
                    
                <h3 class="panel-title">Office</h3>
                </div>
                
                <div class="panel-body">
                    <%--Grid View--%>                    
                    <asp:GridView ID="GvOfficeTable" CssClass="grid-layout" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceOfficeTable">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Office_ID" ) + "\", \"" + Eval("Detail" ) + "\", \"" + Eval("Created_Note" ) + "\", \"" + Eval("Created_By" ) + "\", \"" + Eval("Created_On","{0:dd-MM-yyyy}" ) + "\");" %>' />
                                    <asp:HiddenField ID="hdfOfficeID" runat="server" Value='<%# Bind("Office_ID")%>' />
                                    
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Office_ID" HeaderText="Office Code" />
                            <asp:BoundField DataField="Detail" HeaderText="Office Detail" />
                            <asp:BoundField DataField="Created_On" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Updated On" />
                            <asp:BoundField DataField="Created_By" HeaderText="Updated By" />
                            <asp:BoundField DataField="Created_Note" HeaderText="Note" />
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" />
                        <RowStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </div>
            </div>
             <asp:SqlDataSource ID="SqlDataSourceOfficeTable" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Office_Table_List]" ></asp:SqlDataSource>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
            <asp:PostBackTrigger ControlID="btnEdit"></asp:PostBackTrigger>
             <asp:PostBackTrigger ControlID="btnDelete"></asp:PostBackTrigger>
        </Triggers>
      
    </asp:UpdatePanel>



     
</asp:Content>

