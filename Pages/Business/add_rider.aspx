<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="add_rider.aspx.cs" Inherits="Pages_Business_add_rider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
   
    <asp:UpdatePanel ID="upApplicationNew" runat="server">
        <ContentTemplate>
            <ul class="toolbar">

                <li>
                    <asp:ImageButton ID="ImgBtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClientClick="return confirm('Confirm save data!');" OnClick="ImgBtnSave_Click" />
                </li>

            </ul>
            <!-- Add new application modal -->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script>
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });
        
    </script>
    <style>
        .required {
            color: red;
            padding-right: 5px;
        }

        .message {
            color: red;
            text-align: center;
        }

        .success {
            background-color: #1c9c12;
            color: white;
        }

        .img {
            width: 20px;
            height: 20px;
        }

            .img:hover {
                width: 25px;
                height: 25px;
            }
    </style>

    <asp:UpdatePanel ID="upContent" runat="server">
        <ContentTemplate>
            <h1>Add Rider Form</h1>
            <div id="divmessage" runat="server" class="message"></div>

            <!-- Modal Message -->
            <div id="ModalMessage" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalMessage" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">System Message Alert!</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtMessage" Width="88%" TextMode="MultiLine" Height="60" runat="server" style="color:red; font-weight:normal;"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
            <!--End Modal Message -->

            <table class="table-layout">
                <tr>
                    <td>
                        <div class=" container">

                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 40%; border-right: 1px solid #d5d5d5; padding: 5px;">
                                        <table border="0" style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="required">*</asp:Label>Effective Date</td>
                                                <td>

                                                    <asp:TextBox ID="txtEffective_Date" runat="server" CssClass="datepicker" Width="85%"></asp:TextBox>
                                                    <span class="required">(DD/MM/YYYY]</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label18" runat="server" CssClass="required">*</asp:Label>Last Due Date</td>
                                                <td>

                                                    <asp:TextBox ID="txtLast_Due_Date" runat="server" CssClass=" span2" Width="85%" Enabled="false" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label17" runat="server" CssClass="required">*</asp:Label>System Effective Date</td>
                                                <td>

                                                    <asp:TextBox ID="txtEffective_Date_System" runat="server" CssClass=" span2" Width="85%" Enabled="false" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">
                                                    <asp:Label ID="Label2" runat="server" CssClass="required">*</asp:Label>Policy Number</td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtPolicy_Number" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="required">*</asp:Label>Application Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtApplication_Number" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" CssClass="required">*</asp:Label>Insurance Plan</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInsurance_Plan" runat="server" CssClass="span2" Width="89%" ReadOnly="true" Enabled="false"></asp:DropDownList>
                                                    <asp:ImageButton ID="imgBtnRefresh" ImageUrl="~/App_Themes/functions/refresh_icon.png" runat="server" OnClick="imgBtnRefresh_Click" CssClass="img" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" CssClass="required">*</asp:Label>Term of Insurance</td>
                                                <td>
                                                    <asp:TextBox ID="txtTerm_Insurance" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" CssClass="required">*</asp:Label>Payment Period</td>
                                                <td>
                                                    <asp:TextBox ID="txtPayment_Period" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label16" runat="server" CssClass="required">*</asp:Label>Premium Mode:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPremiumMode" runat="server" AutoPostBack="true" Width="89%" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" CssClass="required">*</asp:Label>Rider Type</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlRider_Type" runat="server" CssClass="span2" Width="89%" AutoPostBack="true" OnSelectedIndexChanged="ddlRider_Type_SelectedIndexChanged">
                                                        <asp:ListItem Text="." Value="" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="ADB" Value="ADB"></asp:ListItem>
                                                        <asp:ListItem Text="TPD" Value="TPD"></asp:ListItem>
                                                        <asp:ListItem Text="Spouse" Value="SPOUSE"></asp:ListItem>
                                                        <asp:ListItem Text="Kid" Value="KID"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" CssClass="required">*</asp:Label>Rider Sum Insured</td>
                                                <td>
                                                    <asp:TextBox ID="txtRider_SumInsured" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server" CssClass="required">*</asp:Label>Apply To</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlApply_To" runat="server" CssClass="span2" Width="89%" ReadOnly="true" Enabled="false"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" CssClass="required">*</asp:Label>Categories</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCategories" runat="server" CssClass="span2" Width="89%" AutoPostBack="true" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label11" runat="server" CssClass="required">*</asp:Label>Position</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPosition" runat="server" CssClass="span2" Width="89%" AutoPostBack="true" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label12" runat="server" CssClass="required">*</asp:Label>Rate</td>
                                                <td>
                                                    <asp:TextBox ID="txtRate" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label13" runat="server" CssClass="required">*</asp:Label>Rider Premium</td>
                                                <td>
                                                    <asp:TextBox ID="txtRider_Premium" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label14" runat="server" CssClass="required">*</asp:Label>Original Amount</td>
                                                <td>
                                                    <asp:TextBox ID="txtOriginal_Amount" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label15" runat="server" CssClass="required">*</asp:Label>Rounded Amount</td>
                                                <td>
                                                    <asp:TextBox ID="txtRounded_Amount" runat="server" CssClass=" span2" Width="85%" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; vertical-align: central;">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 60%;">
                                        <asp:GridView ID="gvRider" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvRider_RowDeleting" OnSelectedIndexChanging="gvRider_SelectedIndexChanging" CssClass="grid-layout">
                                            <SelectedRowStyle BackColor="#00BFFF" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px" CommandName="select" CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete-icon.png" Width="20px" Height="20px" CommandName="delete" CausesValidation="false" OnClientClick=" return confirm('Confirm Delete!!!');" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <Columns>
                                                <asp:TemplateField HeaderText="Level" Visible="False">
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rider ID" Visible="false">
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRiderID" runat="server" Text='<%# Eval("rider_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Product ID" Visible="false">
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Application ID" Visible="false">
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApplicationNumber" runat="server" Text='<%# Eval("app_register_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rider Type" Visible="true" ControlStyle-Width="80px">

                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRiderType" runat="server" Text='<%# Eval("rider_type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sum Insured" Visible="true" ControlStyle-Width="90px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSumInsured" runat="server" Text='<%# Helper.FormatDec(Convert.ToDouble( Eval("Sum_Insured"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <Columns>
                                                <asp:TemplateField HeaderText="Annual Premium" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOriginalAmount" runat="server" Text='<%# Eval("original_amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Annual Premium Round Up" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRoundedAmount" runat="server" Text='<%# Eval("Annual_Rounded_Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Premium By Pay Mode" Visible="true" ControlStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPremium" runat="server" Text='<%# Eval("premium") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Discount" ControlStyle-Width="50px" Visible="false">
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rate ID" ControlStyle-Width="50px" Visible="false">
                                                    <HeaderStyle CssClass="gHeader" />
                                                    <ItemStyle CssClass="gRow" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRateID" runat="server" Text='<%# Eval("rate_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Age Insure" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAge_Insure" runat="server" Text='<%# Eval("Age_Insure") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Pay Year" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPay_Year" runat="server" Text='<%# Eval("Pay_Year") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Pay Up To Age" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPay_Up_To_Age" runat="server" Text='<%# Eval("Pay_Up_To_Age") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Assure Year" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssure_Year" runat="server" Text='<%# Eval("Assure_Year") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Assure Up To Age" ControlStyle-Width="50px" Visible="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssure_Up_To_Age" runat="server" Text='<%# Eval("Assure_Up_To_Age") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Effective Date" ControlStyle-Width="50px" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEffective_Date" runat="server" Text='<%# Eval("Effective_Date", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </td>

                </tr>



            </table>

        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Payment_Mode_List]"></asp:SqlDataSource>
</asp:Content>

