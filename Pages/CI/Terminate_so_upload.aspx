<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="Terminate_so_upload.aspx.cs" Inherits="Pages_CI_Terminate_so_upload" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <script>
        var obj_remarks;
        var obj_policy_id;
        $(document).ready(function () {


            $('#tbl_filter').css('margin-left', ($('#tbl_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');

            $('#btn_client_search').click(function () {
                $('#<%=btnSearch.ClientID%>').click();
            });

           
        });
        function open_terminate_dialog(policy_id) {
            if (policy_id != null && policy_id != "") {
                $('#modal_policy_search').modal('show');


                obj_policy_id.val(policy_id);
            }
            else {
                alert('No policy is selected.');
            }
        }
        
    </script>
    <h1>Terminate Policies</h1>
    <div>
        <table class="table-layout" id="tbl_content">
            <tr>
                <td>
                    <table id="tbl_filter" style="width:500px; height:150px;">
                      
                        <tr>
                            <td style="padding-left:10px; padding-right:10px;">Choose File</td>
                            <td style="padding-left:10px; padding-right:10px;" colspan="2">
                                <asp:FileUpload ID="TerminateFile" runat="server" />     
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="3" style=" padding-left:10px; padding-right:10px;">
                                <a href="SIMPLE_ONE_TERMINATE_TEMPLATE.xlsx" style="color:blue; "><u>Download Template File</u> </a>
                            </td>
                        </tr>
                         <tr>
                           
                            <td colspan="3" style="text-align:center;">
                                 <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" Style="display: none;" OnClick="btnSearch_Click" />
                                <input type="button" class="btn btn-primary" value="Search" id="btn_client_search" />
                                <asp:Button ID="btnTerminate" Text="Terminate" runat="server" OnClientClick="return confirm('Confirm to process terminate.');" OnClick="btnTerminate_Click" style="margin-left:100px; border-radius:5px; color:red;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
          
            <tr style="vertical-align: middle;">
                <td style="vertical-align: middle;">
                    <div class="container">
                        <div id="tab-content" style="padding-top: 10px;">
                            <asp:GridView ID="gv_policy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
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
                                   <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
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


</asp:Content>

