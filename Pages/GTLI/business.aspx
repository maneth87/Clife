<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="business.aspx.cs" Inherits="Pages_GTLI_business" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <input type="button"  onclick="DeleteBusiness()" style="background: url('../../App_Themes/functions/delete.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>

        <li>
            <input type="button"  onclick="EditBusiness()" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <!-- Button to trigger modal new business form -->
            <asp:ImageButton ID="ImgBtnAdd" runat="server" data-toggle="modal" data-target="#myModalNewBusiness" ImageUrl="~/App_Themes/functions/add.png" />
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
    </style>

    <%--Javascript--%>
    <script type="text/javascript">
        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, business_id, business_name) {
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

                $('#Main_hdfBusinessID').val(business_id);

                /// Set text to textboxes for edition                
                $('#Main_txtBusinessNameEdit').val(business_name);

                /// Delete Business
                $('#Main_txtDeleteBusinessName').val(business_name + ' ?');

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                $('#Main_hdfBusinessID').val("");

                //Remove text from textboxes in edit form                                         
                $('#Main_txtBusinessNameEdit').val("");

                //Remove Delete Business
                $('#Main_txtDeleteBusinessName').val("");
            }
        }

        //Edit Business
        function EditBusiness() {
            var business_id = $('#Main_hdfBusinessID').val();
            if (business_id == "") {
                alert("No business to edit.");
            } else {
                $('#myModalEditBusiness').modal('show');
            }
        }

        //Delete Business
        function DeleteBusiness() {
            var business_id = $('#Main_hdfBusinessID').val();
            if (business_id == "") {
                alert("No business to delete.");
            } else {
                $('#myModalDeleteBusiness').modal('show');
            }
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

            <h3 class="panel-title">Business</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <asp:GridView ID="gvBusiness" runat="server" Font-Names="Arial" Width="100%"
                AutoGenerateColumns="False" DataKeyNames="GTLI_Business_ID">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="IndentBottom" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("GTLI_Business_ID" ) + "\", \"" + Eval("Business_Name" ) + "\");" %>' />
                            
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
                    <asp:TemplateField HeaderText="Business" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblBusiness" Text='<%# Bind("Business_Name")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </div>

    <!-- Modal New Business Form -->
    <div id="myModalNewBusiness" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalBusinessHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="myModalBusinessHeader">New Business Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td width="100px">Business Name:</td>
                    <td>
                        <asp:TextBox Width="86%" ID="txtBusinessName" runat="server" Height="15" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorBusinessName" runat="server" ErrorMessage="Rquire Business" Text="*" ControlToValidate="txtBusinessName" ValidationGroup="1" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>                

            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSave" class="btn btn-primary" Style="height: 27px;" TabIndex="9" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="1" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal New Business Form-->

    <!-- Modal Edit Business Form -->
    <div id="myModalEditBusiness" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalBusinessEditHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Update Business Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td width="100px">Business Name:</td>
                     <td>
                        <asp:TextBox Width="86%" ID="txtBusinessNameEdit" runat="server" Height="15" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorBusinessNameEdit" runat="server" ErrorMessage="Rquire Business" Text="*" ControlToValidate="txtBusinessNameEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>               

            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnUpdate" class="btn btn-primary" Style="height: 27px;" TabIndex="9" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="2" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Edit Business Form-->

    <!-- Modal Delete Business Form -->
    <div id="myModalDeleteBusiness" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeleteBusinessHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="myModalDeleteBusinessHeader">Delete Business Confirm</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="text-align: left;">
                <tr>
                    <td>Are you sure, you want to delete business
                        <asp:TextBox ID="txtDeleteBusinessName" runat="server" BorderStyle="None" Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Delete" OnClick="btnDelete_Click" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Delete Business Form-->

    <%--Hidden Fields--%>
    <asp:HiddenField ID="hdfBusinessID" runat="server" />
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />

</asp:Content>

