<%@ Page Title="Clife | Upload Wing Account => Application" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="upload_wing_account.aspx.cs" Inherits="Pages_Admin_upload_wing_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript">

        function SearchWingAccount() {

            var btnSearchWing = document.getElementById('<%= btnSearchWing.ClientID %>'); //dynamically click button
            btnSearchWing.click();

        }

        function SelectSingleCheckBox(ckb, wing_sk, wing_num, created_by) {

            $('#Main_txtEditWingSk').val(wing_sk);
            $('#Main_txtEditWingNum').val(wing_num);
            $('#Main_txtuser').val(created_by);

            // Hidden Field
            $('#Main_hdftxtEditWingSk').val(wing_sk);
            $('#Main_hdftxtEditWingNum').val(wing_num);

            // ///// ///// ***** // //// /////        

            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");

                }
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e0e0e0");

            }
        }

        function EditWingAccount() {

            var wing_sk = $('#Main_txtEditWingSk').val();
            var wing_num = $('#Main_txtEditWingNum').val();

            //var wing_sk = $('#Main_hdftxtEditWingSk').val();
            //var wing_num = $('#Main_hdftxtEditWingNum').val();

            $.ajax({
                type: "POST",
                url: "../../WingWebService.asmx/CheckWingAccount",
                data: "{wing_sk:'" + wing_sk + "',wing_num:'" + wing_num + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "update") {

                        onclick_update_wing();

                    } else {

                        alert(data.d);

                        return;
                    }
                },
                error: function (msg) {
                    alert("Problem in CheckWingAccount!");
                }
            });

        }

        function onclick_update_wing() {

            var btnedit = document.getElementById('<%= btnEditWing.ClientID %>'); //dynamically click button
            btnedit.click();
        }

    </script>
    <ul class="toolbar">
        <%-- <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server" ValidationGroup="2" onmouseover="tooltip.pop(this, 'Clear Policy')" Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>--%>
        <li>
            <asp:ImageButton ID="ImgBtnEdit" runat="server" data-toggle="modal" data-target="#myModelEditWing" ImageUrl="~/App_Themes/functions/edit.png" ValidationGroup="edit" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" runat="server" data-toggle="modal" data-target="#myModalSearchAccountWing" ImageUrl="~/App_Themes/functions/search.png" ToolTip="Search" ValidationGroup="wing" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnUpload" runat="server" />
            </div>
            <asp:ImageButton ID="ImgBtnUpload" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/add.png" OnClick="ImgBtnUpload_Click" />

        </li>

    </ul>

    <br />
    <br />
    <br />

    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Upload Wing Account</h3>
        </div>
        <div class="panel-body">

            <%-- Upload Wing Account Design Section--%>
            <table>

                <tr>
                    <td style="text-align: right">Upload:</td>
                    <td>
                        <asp:FileUpload ID="FileUploadWingAccount" runat="server" Width="91%" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUploadPolicy" ControlToValidate="FileUploadWingAccount" Text="*" ForeColor="Red" runat="server" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>

        <div class="panel-body">
            <h3 class="panel-title">Upload Result</h3>
            <br />
            <asp:Table ID="tblResult" Style='width: 100%;' border='1' runat="server">

                <asp:TableHeaderRow>
                    <asp:TableHeaderCell HorizontalAlign="Center" Width="10%">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell HorizontalAlign="Center" Width="25%">Date Request</asp:TableHeaderCell>
                    <asp:TableHeaderCell HorizontalAlign="Center" Width="25%"> SK </asp:TableHeaderCell>
                    <asp:TableHeaderCell HorizontalAlign="Center" Width="25%">Wing Number (#)</asp:TableHeaderCell>
                    <asp:TableHeaderCell HorizontalAlign="Center" Width="15%">Result</asp:TableHeaderCell>

                </asp:TableHeaderRow>

                <asp:TableRow>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Account Wing</h3>
        </div>
        <div class="panel-body">
            <asp:Label ID="result" runat="server" Text=""></asp:Label>
            <%-- Grid View --%>
            <asp:GridView ID="GvWing" PageSize="100" OnPageIndexChanged="GvWing_PageIndexChanged" OnPageIndexChanging="GvWing_PageIndexChanging" CssClass="grid-layout; grid" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Black" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" HorizontalAlign="Center" PagerStyle-HorizontalAlign="Center">

                <Columns>

                    <asp:TemplateField>
                        <ItemTemplate>

                            <asp:CheckBox ID="ckb1" runat="server" Enabled='<%#  Eval("Status").ToString() == "1" %>' onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("SK" ) + "\",\"" + Eval("Wing_Number") + "\",\"" + Eval("Created_By") + "\");" %>' />

                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" VerticalAlign="Middle" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="No" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" VerticalAlign="Middle" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="Date_Request" HeaderText="Date Requested" DataFormatString="{0:dd-MM-yyyy}" />
                    <asp:BoundField DataField="Created_On" HeaderText="Created On" />
                    <asp:BoundField DataField="SK" HeaderText="SK" />
                    <asp:BoundField DataField="Wing_Number" HeaderText="Wing Number (#)" />
                    <asp:BoundField DataField="Created_By" HeaderText="Created By" />

                    <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <img src='<%# Eval("Status").ToString() == "1" ? "../../App_Themes/functions/active.png" : "../../App_Themes/functions/inactive.png" %>' alt="Photo" />
                        </ItemTemplate>
                        <HeaderStyle Width="50" Height="25" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>

                </Columns>
                <HeaderStyle HorizontalAlign="Left" />
                <RowStyle HorizontalAlign="Left" Height="25" />
            </asp:GridView>

        </div>
    </div>
    <!-- Modal Search Wing Account -->
    <div id="myModalSearchAccountWing" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchAccountWingHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H2">Search Account Wing Form</h3>
        </div>
        <div class="modal-body">

            <!---Modal Body--->
            <ul class="nav nav-tabs" id="myTabApplicationSearch">
                <li class="active"><a href="#SK" data-toggle="tab" style="text-decoration: none;">Wing SK</a></li>
                <li><a href="#WN" data-toggle="tab" style="text-decoration: none;">Wing Number</a></li>
            </ul>

            <div class="tab-content" style="height: 60px; overflow: hidden;">
                <div class="tab-pane active" id="SK">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 13%; vertical-align: middle">SK No:</td>
                            <td style="width: 30%; vertical-align: bottom">
                                <asp:TextBox ID="txtSkNumberSearch" Width="90%" runat="server"></asp:TextBox>

                            </td>
                            <td style="width: 56%; vertical-align: top"></td>
                        </tr>
                    </table>
                </div>
                <div class="tab-pane" id="WN">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 13%; vertical-align: middle">Wing No:</td>
                            <td style="width: 30%; vertical-align: bottom">
                                <asp:TextBox ID="txtWingNumberSearch" Width="90%" runat="server"></asp:TextBox>

                            </td>
                            <td style="width: 56%; vertical-align: top"></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="modal-footer">
                <div style="display: none;">
                    <asp:Button ID="btnSearchWing" runat="server" Text="Button" OnClick="btnSearchWing_Click" ValidationGroup="wing" />
                </div>
                <input type="button" class="btn btn-primary" style="height: 27px;" onclick="SearchWingAccount();" value="Search" />

                <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
        </div>
    </div>

    <!-- Modal Edit Wing Account -->
    <div id="myModelEditWing" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModelEditWingHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Edit Account Wing Form</h3>
        </div>
        <div class="modal-body">

            <table style="width: 98%">
                <tr>
                    <td style="width: 13%; vertical-align: middle">Created By:</td>
                    <td style="width: 50%; vertical-align: bottom">
                        <asp:TextBox ID="txtuser" ReadOnly="true" Width="90%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtuser" ValidationGroup="edit" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 56%; vertical-align: top"></td>
                </tr>
                <tr>
                    <td style="width: 13%; vertical-align: middle">SK No:</td>
                    <td style="width: 50%; vertical-align: bottom">
                        <asp:TextBox ID="txtEditWingSk" Width="90%" runat="server"></asp:TextBox>
                        <a href="#">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtEditWingSk" ValidationGroup="edit" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hdftxtEditWingSk" runat="server" /></td>
                    <td style="width: 56%; vertical-align: top"></td>
                </tr>
                <tr>
                    <td style="width: 13%; vertical-align: middle">Wing No:</td>
                    <td style="width: 50%; vertical-align: bottom">
                        <asp:TextBox ID="txtEditWingNum" Width="90%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtEditWingNum" ValidationGroup="edit" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>

                        <asp:HiddenField ID="hdftxtEditWingNum" runat="server" />
                    </td>
                    <td style="width: 56%; vertical-align: top"></td>
                </tr>
            </table>
            <br />

            <div class="modal-footer">
                <div style="display: none;">
                    <asp:Button ID="btnEditWing" runat="server" Text="Button" ValidationGroup="edit" OnClick="btnEditWing_Click" />
                </div>
                <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditWingAccount();" value="Update" runat="server" validationgroup="edit" />

                <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
        </div>
    </div>
    <%--Hidden fields of User--%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />

</asp:Content>

