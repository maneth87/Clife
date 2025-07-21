<%@ Page Title="Clife | Core Data => Underwriting Code" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="underwrite_code.aspx.cs" Inherits="Pages_CoreData_underwrite_code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
      <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImageBDelete" runat="server"  data-toggle="modal" data-target="#myModalDeleteUnderwrite"  ImageUrl="~/App_Themes/functions/delete.png" />
        </li>
        <li> 
            <asp:ImageButton ID="ImgeBtnEdit"  runat="server" data-toggle="modal" data-target="#myModalEditUnderWriteCode"  ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
         <li>
             <!-- Button to trigger modal new underwrite table form -->           
            <asp:ImageButton ID="ImgBtnAdd" runat="server"   data-toggle="modal" data-target="#myModalNewUnderwrite" ImageUrl="~/App_Themes/functions/add.png" />
        </li>
    </ul>

      <script type="text/javascript">
         
          function SelectSingleCheckBox(ckb, Status_Code, Detail, Is_inforce, Is_Reserved, Created_Note) {
              var GridView = ckb.parentNode.parentNode.parentNode;

              var ckbList = GridView.getElementsByTagName("input");
              for (i = 0; i < ckbList.length; i++) {

                  if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {
                      var current_id = ckbList[i].id;

                      var sub_current_id = current_id.substring(18, 22);
                      if (sub_current_id == "ckb1") {
                          ckbList[i].checked = false;
                          $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                      }
                  }
              }

              if (ckb.checked) {
                  $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

                  /// Set text to textboxes for edition
                  $('#Main_txtEditStatusCode').val(Status_Code);
                  $('#Main_txtEditDetail').val(Detail);

                  if (Is_inforce == "True") {

                      $('#Main_chbEditInforce').prop('checked', true);
                  }
                  else { $('#Main_chbEditInforce').prop('checked', false); }

                  if (Is_Reserved == "True") {
                      $('#Main_chbEditReserved').prop('checked', true);
                  }
                  else { $('#Main_chbEditReserved').prop('checked', false); }


                  $('#Main_txtEditNote').val(Created_Note);

                  $('#Main_hdfStatusCode').val(Status_Code);

                  /// Delete Relationship ID
                  $('#Main_txtDeletUnderwrite').val(Status_Code + ' ?');
                  $('#Main_hdfDeleteStatusCode').val(Status_Code);

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

          function GetUnderWriting_Code() {
              var Status_Code = $('#Main_txtStatusCodeModal').val();
              var Detail = $('#Main_txtDetail').val();


              $.ajax({
                  type: "POST",
                  url: "../../SaleAgentWebService.asmx/GetStatusUnderwriting_Code",
                  data: "{Status_Code:'" + Status_Code + "',Detail:'" + Detail + "'}",
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

          function GetUnderWriting_Code_Edit() {
              var Status_Code = $('#Main_hdfStatusCode').val();
              var Detail = $('#Main_txtEditDetail').val();

              $.ajax({
                  type: "POST",
                  url: "../../SaleAgentWebService.asmx/GetGetStatusUnderwriting_Code_Edit",
                  data: "{Status_Code:'" + Status_Code + "',Detail:'" + Detail + "'}",
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
          
          function ClearText()
          {
              $('#Main_txtStatusCodeModal').val("");
              $('#Main_txtDetail').val("");
              $('#Main_chbInforce').val(false);
              $('#Main_chbReserved').val(false);
              $('#Main_txtNote').val("");

              $('#Main_txtEditStatusCode').val("");
              $('#Main_txtEditDetail').val("");
              $('#Main_chbEditInforce').val("");
              $('#Main_chbEditReserved').val("");
              $('#Main_txtEditNote').val("");
          }

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%--here is the block code--%>

          <!-- Modal New Underwrit Form -->
            <div id="myModalNewUnderwrite"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalUnderwriteHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalRelationshiHeader">New Underwrite Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table text-align:left;">
                    <tr>
                        <td>Status Code:</td>
                        <td>
                            <asp:TextBox ID="txtStatusCodeModal"  runat="server" MaxLength="49" TabIndex="1"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorStatusCode" ControlToValidate="txtStatusCodeModal" runat="server" ForeColor="Red" ValidationGroup="3">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                      <tr>
                        <td>Detail:</td>
                        <td>
                            <asp:TextBox ID="txtDetail"  runat="server" MaxLength="49" TabIndex="2"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtDetail" runat="server" ForeColor="Red" ValidationGroup="3">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Is inforce:</td>
                        <td>
                            <asp:CheckBox id="chbInforce" runat="server" TabIndex="3"/> (Check Is True) &nbsp;&nbsp;&nbsp; Is Reserved:  <asp:CheckBox id="chbReserved" runat="server" TabIndex="4"/> (Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtNote"  runat="server" MaxLength="49" TabIndex="4" ></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtNote" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">   
                <div style="display: none;">
                       <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
                </div>   

               <input type="button" onclick="GetUnderWriting_Code()" id="btnOk_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK"   ValidationGroup="3"   />
            
               <button class="btn"  data-dismiss="modal" aria-hidden="true" onclick="ClearText()">Cancel</button>
            </div>
            </div>
          <!--End Modal New Underwrite Form--> 

         <!-- Modal Edit UnderWrite Code Form -->
            <div id="myModalEditUnderWriteCode"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditRelationshipHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalEditRelationshipHeader">Update Underwrite Code Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table text-align:left;">
                    <tr>
                        <td>Status Code:</td>
                        <td>
                            <asp:TextBox ID="txtEditStatusCode"  runat="server" MaxLength="49" TabIndex="1" Enabled="false" BackColor="White"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtEditStatusCode" Enabled="false" runat="server" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hdfStatusCode" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Detail:</td>
                        <td>
                            <asp:TextBox ID="txtEditDetail"  runat="server" MaxLength="49" TabIndex="2"  BackColor="White"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtEditDetail" Enabled="false" runat="server" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Is inforce:</td>
                        <td>
                            <asp:CheckBox id="chbEditInforce" runat="server" TabIndex="3" /> (Check Is True) &nbsp;&nbsp;&nbsp;  <asp:CheckBox id="chbEditReserved" runat="server" TabIndex="4"/> (Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtEditNote"  runat="server" MaxLength="49" TabIndex="5" ></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtEditNote" runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">   

               <div style="display: none;">
                    <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
               </div> 
                  
               <input type="button" onclick="GetUnderWriting_Code_Edit()" id="btnEdit_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK"   ValidationGroup="2"    />
            
               <button class="btn"  data-dismiss="modal" aria-hidden="true" onclick="ClearText()">Cancel</button>
            </div>
            </div>
         <!--End Modal Edit Relationship Form--> 

         <!-- Modal Delete Underwrite Form -->
            <div id="myModalDeleteUnderwrite" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeleteUnderwriteHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalDeleteUnderwriteHeader">Delete Underwrite Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdfDeleteStatusCode" runat="server" />
                                 Are you sure, you want to delete Status Code <asp:TextBox ID="txtDeletUnderwrite" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnDelete_Click"   />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
          <!--End Modal Delete Underwrite Form-->

          <div class="panel panel-default" >
                <div class="panel-heading">
                    
                <h3 class="panel-title">Underwriting Code</h3>
                </div>
                <div class="panel-body">
                    <%--Grid View--%>                    
                    <asp:GridView ID="GvOUderwrite" CssClass="grid-layout" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceUnderwrite">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1"  runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Status_Code") + "\", \"" + Eval("Detail") + "\",\"" + Eval("Is_Inforce") + "\", \"" + Eval("Is_Reserved") + "\", \"" + Eval("Created_Note") + "\");"%>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Status Code" DataField="Status_Code" />
                            <asp:BoundField DataField="Detail" HeaderText="Detail" />
                            <asp:BoundField DataField="Parent_Status_Code" HeaderText="Parent status code" />
                            <asp:CheckBoxField DataField="Is_Inforce" HeaderText="Is inforce" />
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
             <asp:SqlDataSource ID="SqlDataSourceUnderwrite" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Underwrite]" ></asp:SqlDataSource>

	</ContentTemplate>

   <Triggers>
    <asp:PostBackTrigger ControlID="btnOk" />
    <asp:PostBackTrigger ControlID="btnEdit" />
    <asp:PostBackTrigger ControlID="btnDelete" />
   </Triggers>

    </asp:UpdatePanel>	
</asp:Content>

