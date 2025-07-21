<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="TerminateSO.aspx.cs" Inherits="Pages_CI_TerminateSO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <script>
        var obj_remarks;
        var obj_policy_id;
        var obj_policy_no;
        $(document).ready(function () {


            $('#tbl_filter').css('margin-left', ($('#tbl_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');

            $('#btn_client_search').click(function () {
                $('#<%=btnSearch.ClientID%>').click();
            });

            obj_remarks = $('#<%=txtRemarks.ClientID%>');
            obj_policy_id = $('#<%=hfdPolicyID.ClientID%>');
            obj_policy_no = $('#<%=hfdPolicyNumber.ClientID%>');
        });
        function open_terminate_dialog(policy_id,polNo) {
            if (policy_id != null && policy_id != "") {
                $('#modal_policy_search').modal('show');


                obj_policy_id.val(policy_id);
                obj_policy_no.val(polNo);
              
            }
            else {
                alert('No policy is selected.');
            }
        }
        function terminate() {
            if (obj_policy_id.val() != "" && obj_policy_id != null) {

                $('#<%=btnTerminate.ClientID%>').click();
            }
            else {
                alert('Please select policy to terminate.');
            }
        }
    </script>
    <h1>Terminate Policies</h1>
    <div>
        <table class="table-layout" id="tbl_content">
            <tr>
                <td>
                    <table id="tbl_filter">
                        <tr>
                            <td colspan="3" style="font-weight: bold;">Filter</td>

                        </tr>
                       
                       
                        <tr>
                            <td>Customer Name</td>
                            <td>
                                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                            </td>
                            <td rowspan="3" style="vertical-align: middle;">
                               
                            </td>
                        </tr>
                        <tr>
                            <td>Gender</td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="F" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="M" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Policy Number</td>
                            <td>
                                <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="3" style="text-align:right;">
                                <%-- <a href="Terminate_so_upload.aspx" style="color:blue;" ><u>Upload File</u></a>  --%>     
                                <asp:LinkButton ID="lbtUpload" runat="server" Text="Upload File"  OnClick="lbtUpload_Click"></asp:LinkButton>      
                            </td>
                            
                        </tr>
                         <tr>
                           
                            <td colspan="3" style="text-align:center;">
                                 <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" Style="display: none;" OnClick="btnSearch_Click" />
                                <input type="button" class="btn btn-primary" value="Search" id="btn_client_search" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="vertical-align: middle;">
                <td style="vertical-align: middle;">
                    <div class="container">
                        <div id="tab-content" style="padding-top: 10px;">
                            <asp:GridView ID="gv_policy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnSelectedIndexChanged="gv_policy_SelectedIndexChanged">
                                <SelectedRowStyle BackColor="#EEFFAA" />
                                <Columns>
                                  
                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOlicyID" runat="server" Text='<%#Eval("policy_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy Number" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("policy_number") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Card" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIDCard" runat="server" Text='<%#Eval("ID_Card") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Surname(En)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("last_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Given Name(En)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("first_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Surname(Kh)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKHLastName" runat="server" Text='<%# Eval("khmer_last_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Given Name(Kh)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKHFirstName" runat="server" Text='<%# Eval("khmer_first_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gender">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender").ToString()=="0"? "F":Eval("Gender").ToString()=="1"?"M" : "N/A" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="DOB">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOB" runat="server" Text='<%#  Convert.ToDateTime (Eval("birth_date").ToString()).ToString("dd-MM-yyyy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#  Convert.ToDateTime (Eval("Effective_date").ToString()).ToString("dd-MM-yyyy HH:mm:ss") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Phone Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("mobile_phone1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("policy_status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>

                                            <a href="#" style="color: red; font-weight: bold;" onclick='<%# "open_terminate_dialog(\"" + Eval("policy_id")+"\",\"" + Eval("policy_number") + "\");"%>'>Terminate</a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                        </div>
                    </div>

                </td>
            </tr>

        </table>

    </div>
    <div id="modal_policy_search" class="modal hide fade " tabindex="-1" role="dialog" aria-labelledby="SearchCustomer" aria-hidden="true" data-keyboard="false" data-backdrop="static">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Terminate Policy</h3>
        </div>
        <div class="modal-body ">
            <asp:HiddenField runat="server" ID="hfdPolicyID" />
            <asp:HiddenField runat="server" ID="hfdPolicyNumber" />
            <div id="search_policy" class="tab-pane active">
                <table style="width: 100%;">
                    <tr>
                        <td>Remarks
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRemarks" Width="98%"></asp:TextBox>
                        </td>

                    </tr>
                </table>
            </div>

        </div>
        <div class="modal-footer">
            <asp:Button ID="btnTerminate" runat="server" OnClick="btnTerminate_Click" Style="display: none;" />
            <input type="button" id="btnClientTerminate" value="Terminate" onclick="terminate();" class="btn btn-primary" />
            <input type="button" id="btnCancel" value="Cancel" class="btn" data-dismiss="modal" />
        </div>
    </div>


</asp:Content>

