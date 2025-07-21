<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_card.aspx.cs" Inherits="Pages_Card_policy_card" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

   <%-- <asp:UpdatePanel ID="upApplicationNew" runat="server">
        <ContentTemplate>
            <ul class="toolbar">

                <li>
                    <asp:ImageButton ID="ImgBtnSave" runat="server" ImageUrl="~/App_Themes/functions/print.png" OnClick="ImgBtnSave_Click" />
                </li>
                <li>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/functions/search.png" OnClick="ImageButton1_Click"/>
                </li>
            </ul>
            <!-- Add new application modal -->

        </ContentTemplate>
      
    </asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
   
   <%-- <asp:UpdatePanel ID="upBody" runat="server" UpdateMode="Always" >
        <ContentTemplate>--%>
              <ul class="toolbar">

                <li>
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/functions/print.png" OnClick="ImgBtnSave_Click"  />
                </li>
                <li>
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/functions/search.png" OnClick="ImageButton1_Click" />
                </li>
            </ul>
    <h1>Policy Cards Printing</h1>
            <div>
                <table class="table-layout">
                    <tr style="vertical-align:middle;">
                        <td></td>
                        <td style="vertical-align:middle;">
                            Policy Type
                            
                        </td>
                        <td colspan="3" style="vertical-align:middle;">
                            <asp:DropDownList ID="ddl_policy_type" runat="server" CssClass="dropdown"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr style="vertical-align:middle;">
                        <td></td>
                        <td style="vertical-align:middle;">Excel File</td>
                        <td colspan="3" style="vertical-align:middle; padding-bottom:10px;">
                           <asp:CheckBox runat="server" ID="ckb_excel" AutoPostBack="true" OnCheckedChanged="ckb_excel_CheckedChanged"  />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="5">
                           <asp:FileUpload ID="excel" runat="server" Enabled="false"  />
                            <%--<input id="excel" runat="server" style="width: 30%" class="fileupload" type="file" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="vertical-align:middle;">Policy</td>
                        <td style="vertical-align:middle;">
                            <asp:TextBox ID="txt_policy_number" Width="465px" runat="server"></asp:TextBox>
                            <asp:Label ID="lblEx" runat="server" Text="Ex: 001,002 [More than one policy]" ForeColor="Red"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr style="vertical-align:middle;">
                        <td></td>
                        <td style="vertical-align:middle;">Effective Date</td>
                        <td style="vertical-align:middle;">
                            <asp:TextBox ID="txt_effective_date_from" runat="server" Style="margin-right: 10px;" placeholder="[DD/MM/YYYY]"></asp:TextBox>To
                        <asp:TextBox ID="txt_effective_date_to" runat="server" Style="margin-left: 10px;" placeholder="[DD/MM/YYYY]"></asp:TextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr  style="vertical-align:middle;">
                        <td colspan="5" style="vertical-align:middle;">
                            <div id="div_record_found" runat="server" style="color: red; border-bottom: 1px groove #d5d5d5; padding: 10px; display: none;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">

                            <div>
                                <asp:GridView ID="gv_policy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center">
                                    <SelectedRowStyle BackColor="#EEFFAA" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Level">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                
                                                <asp:CheckBox ID="ckbAll" AutoPostBack="true" runat="server" OnCheckedChanged="ckbAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                
                                                <asp:CheckBox ID="ckb" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Policy ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicy_ID" runat="server" Text='<%#Eval("policy_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Policy No." SortExpression="policy_number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicy_Number" runat="server" Text='<%# Eval("policy_number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Khmer Name" SortExpression="kh_name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKh_Name" runat="server" Text='<%# Eval("kh_name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="En Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEn_Name" runat="server" Text='<%# Eval("en_name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer_ID" runat="server" Text='<%# Eval("customer_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                                <asp:GridView ID="gv_excel" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center">
                                    <SelectedRowStyle BackColor="#EEFFAA" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Level">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>

                                                <asp:CheckBox ID="ckbAll" AutoPostBack="true" runat="server" OnCheckedChanged="ckbAll_CheckedChanged1" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckb" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Policy ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicy_ID" runat="server" Text='<%#Eval("policy_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Policy No." SortExpression="policy_number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicy_Number" runat="server" Text='<%# Eval("policy_number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Khmer Name" SortExpression="kh_name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKh_Name" runat="server" Text='<%# Eval("kh_name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="En Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEn_Name" runat="server" Text='<%# Eval("en_name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer_ID" runat="server" Text='<%# Eval("customer_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issued Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffective_Date" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy}",Eval("Effective_Date")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Maturity Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaturity_Date" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy}", Eval("Maturity_Date")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>

            </div>


    <%--    </ContentTemplate>
        <Triggers>
          

        </Triggers>
    </asp:UpdatePanel>--%>
   
</asp:Content>

