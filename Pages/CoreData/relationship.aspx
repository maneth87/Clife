<%@ Page Title="Clife | Core Data => Relationship Status" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="relationship.aspx.cs" Inherits="Pages_CoreData_relationship" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
      <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImageBDelete" runat="server"  data-toggle="modal" data-target="#myModalDeleteRelationship"  ImageUrl="~/App_Themes/functions/delete.png" />
        </li>
        <li> 
            <asp:ImageButton ID="ImgeBtnEdit"  runat="server" data-toggle="modal" data-target="#myModalEditRelationship"  ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
         <li>
             <!-- Button to trigger modal new relationship table form -->           
            <asp:ImageButton ID="ImgBtnAdd" runat="server"   data-toggle="modal" data-target="#myModalNewRelationship" ImageUrl="~/App_Themes/functions/add.png" />
        </li>
    </ul>
    <script type="text/javascript">

        function SelectSingleCheckBox(ckb, Relationship, Relationship_Khmer, Is_Clean_Case, Is_Reserved, Created_Note) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {
                    var current_id = ckbList[i].id;
                    var sub_current_id = current_id.substring(21, 25);
                    if (sub_current_id == "ckb1")
                    {
                        ckbList[i].checked = false;
                        $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                    }
                }            
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

                /// Set text to textboxes for edition
                $('#Main_txtEditRelationship').val(Relationship);
                $('#Main_txtEditRep_Kh').val(Relationship_Khmer);

                if (Is_Clean_Case == "True") {
            
                    $('#Main_chbEditCleanCase').prop('checked', true);
                }
                else { $('#Main_chbEditCleanCase').prop('checked', false); }

                if (Is_Reserved == "True") {
                    $('#Main_chbEditReserved').prop('checked', true);
                }
                else { $('#Main_chbEditReserved').prop('checked', false); }

                
                $('#Main_txtEditNote').val(Created_Note);

                $('#Main_hdfRelationship').val(Relationship);

                /// Delete Relationship ID
                $('#Main_txtDeleteRelationship').val(Relationship + ' ?');
                $('#Main_hdfDeleteRelationship').val(Relationship);

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                //Remove text from textboxes in edit form
                $('#Main_txtEditRelationship').val("");
                $('#Main_txtEditRep_Kh').val("");
                $('#Main_chbEditCleanCase').val("");
                $('#Main_chbEditReserved').val("");
                $('#Main_txtEditNote').val("");

            }
        }

        function ClearText()
        {
            $('#Main_txtRelationshipModal').val("");
            $('#Main_chbCleanCase.Checked').val(false);
            $('#Main_chbReserved.Checked').val(false);
            $('#Main_txtNote.Text').val("");
            $('#Main_txtRep_Kh.Text').val("");

            $('#Main_txtEditRelationship').val("");
            $('#Main_txtEditRep_Kh').val("");
            $('#Main_chbEditCleanCase').val("");
            $('#Main_chbEditReserved').val("");
            $('#Main_txtEditNote').val("");
        }

        function GetRelationship() {
            var Relationship = $('#Main_txtRelationshipModal').val();
            var Relationship_Khmer = $('#Main_txtRep_Kh').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetRelationship",
                data: "{Relationship:'" + Relationship + "',Relationship_Khmer:'" + Relationship_Khmer + "'}",
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

        function GetRelationship_Edit() {
            var Relationship = $('#Main_hdfRelationship').val();
            var Relationship_Khmer = $('#Main_txtEditRep_Kh').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetRelationship_Edit",
                data: "{Relationship:'" + Relationship + "',Relationship_Khmer:'" + Relationship_Khmer + "'}",
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
            var btnOk = document.getElementById('<%= btnOk.ClientID %>'); //dynamically click button

            btnOk.click();
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <br /><br /><br />
   <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>

		 <%--here is the block code--%>

          <!-- Modal New Relationship Form -->
            <div id="myModalNewRelationship"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalRelationshipHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalRelationshipHeader">New Relationship Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table text-align:left;">
                    <tr>
                        <td>Relationship En:</td>
                        <td>
                            <asp:TextBox ID="txtRelationshipModal"  runat="server" MaxLength="49" TabIndex="1"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorOfficeCode" ControlToValidate="txtRelationshipModal" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     <tr>
                        <td>Relationship Kh:</td>
                        <td>
                            <asp:TextBox ID="txtRep_Kh"  runat="server" MaxLength="49" TabIndex="2"  ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRep_kh" ControlToValidate="txtRep_Kh" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Is clean case:</td>
                        <td>
                            <asp:CheckBox id="chbCleanCase" runat="server" TabIndex="3"/> (Check Is True) &nbsp;&nbsp;&nbsp; Is Reserved: <asp:CheckBox id="chbReserved" runat="server" TabIndex="4"/> (Check Is True)
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                             <asp:TextBox ID="txtNote"  runat="server" MaxLength="49" TabIndex="5" ></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtNote" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer"> 
                  
               <div style="display: none;">
                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
                </div>

               <input type="button" onclick="GetRelationship()"  id="btnOk_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK"   ValidationGroup="1"  />
            
                <button class="btn"  data-dismiss="modal" aria-hidden="true" onclick="ClearText()">Cancel</button>
            </div>
            </div>
          <!--End Modal New Application Form--> 

         <!-- Modal Edit Relationship Form -->
            <div id="myModalEditRelationship"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditRelationshipHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalEditRelationshipHeader">Update Relationship Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table text-align:left;">
                    <tr>
                        <td>Relationship En:</td>
                        <td>
                            <asp:TextBox ID="txtEditRelationship"  runat="server" MaxLength="49" TabIndex="1" Enabled="false" BackColor="White"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtEditRelationship" Enabled="false" runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hdfRelationship" runat="server"/>
                        </td>
                    </tr>
                     <tr>
                        <td>Relationship Kh:</td>
                        <td>
                            <asp:TextBox ID="txtEditRep_Kh"  runat="server" MaxLength="49" TabIndex="2"  BackColor="White"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtEditRep_Kh" ControlToValidate="txtEditRep_Kh"  runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            <asp:HiddenField ID="HiddenField1" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Is clean case:</td>
                        <td>
                            <asp:CheckBox id="chbEditCleanCase" runat="server" TabIndex="3" /> (Check Is True) &nbsp;&nbsp;&nbsp; Is Reserved: <asp:CheckBox id="chbEditReserved" runat="server" TabIndex="4"/> (Check Is True)
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

                <input type="button" onclick="GetRelationship_Edit()" id="btnEdit_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="OK"   ValidationGroup="2"  />
            
                <button class="btn"  data-dismiss="modal" aria-hidden="true" onclick="ClearText()">Cancel</button>
            </div>
            </div>
         <!--End Modal Edit Relationship Form--> 

          <!-- Modal Delete Relationship Form -->
            <div id="myModalDeleteRelationship" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeleteRelationshipHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalDeleteRelationshipHeader">Delete Relationship Form</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdfDeleteRelationship" runat="server" />
                                 Are you sure, you want to delete Relationship Code <asp:TextBox ID="txtDeleteRelationship" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnDelete_Click"   />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
          <!--End Modal Delete Relationship Form-->

             <div class="panel panel-default" >
                <div class="panel-heading">Relationship</div>
                <div class="panel-body">
                    <%--Grid View--%>                    
                    <asp:GridView ID="GvORelationship" CssClass="grid-layout" Width="100%" runat="server" DataSourceID="SqlDataSourceRelationship"  AutoGenerateColumns="False"  BorderColor="Black">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1"  runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Relationship" ) + "\",\"" + Eval("Relationship_Khmer" ) + "\", \"" + Eval("Is_Clean_Case" ) + "\", \"" + Eval("Is_Reserved") + "\", \"" + Eval("Created_Note" ) + "\");" %>' />
                                    
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Relationship" HeaderText="Relationship"/>
                            <asp:BoundField DataField="Relationship_Khmer" HeaderText="Relationship Khmer"/>
                            <asp:CheckBoxField DataField="Is_Clean_Case" HeaderText="Is clean case" />
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
             <asp:SqlDataSource ID="SqlDataSourceRelationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Relationship]" ></asp:SqlDataSource>

             <%-- Section Hidenfields Initialize  --%>
             <asp:HiddenField ID="hdfuserid" runat="server" />
             <asp:HiddenField ID="hdfusername" runat="server" />
             <asp:HiddenField ID="hdftotalagentrow" runat="server" />
            <%-- End Section Hidenfields Initialize  --%>

	</ContentTemplate>
       
    <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
            <asp:PostBackTrigger ControlID="btnEdit" />
            <asp:PostBackTrigger ControlID="btnDelete" />
   </Triggers>

    </asp:UpdatePanel>	
</asp:Content>
