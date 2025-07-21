<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="approve_policy.aspx.cs" Inherits="Pages_Flate_Rate_approve_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">

        <li style="display: none;">
            <input type="button" data-toggle="modal" data-target="#modal_policy_search" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
       
        <li style="display: none;">
            <input type="button" data-toggle="modal" data-target="#modal_policy_search" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li style="display: none;">
            <input type="button" onclick="clear_all();" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Style="display: none;" />
            <input type="button" onclick="open_confirm('Decline');" id="btnAdd" style="background: url('../../App_Themes/functions/decline.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <asp:Button ID="btnApproved" runat="server" OnClick="btnApproved_Click" Style="display: none;" />
            <input type="button" onclick="open_confirm('Approve');" style="background: url('../../App_Themes/functions/approve.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>

    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
     <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <style>
        .modal.large {
            width: 60%; /* desired relative width */
            left: 20%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }

        .g_row {
            padding: 5px;
            vertical-align: middle;
        }

        .g_header {
            background-color: #ebe8e8;
            vertical-align: middle;
        }

        .g_row_center {
            text-align: center;
        }
    </style>

    <script>
        var approve;
        var decline;
        var grid;

        $(document).ready(function () {

            approve = $('#<%=btnApproved.ClientID%>');
            decline = $('#<%=btnDecline.ClientID%>');
            // grid = $('#<%=gvPolicy.ClientID%>;
            
            //header checed
            $('#Main_gvPolicy_ckbAll').click(function () {
                this.checked = !(this.checked == true);
                $('#<%=gvPolicy.ClientID%> input:checkbox').attr("checked", function () {

                    if (this.checked) {
                        this.checked = false;
                    }
                    else {
                        this.checked = true;
                    }
                });
            });

            //show_me();
            
            $('.datepicker').datepicker();
            $('#<%=txtIssuedDate.ClientID%>').val('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');

        });


        function view_detail(policy_id) {
            window.location.href = 'policy_detail.aspx?policy_id=' + policy_id;

        }
        function open_confirm(option) {
            //check selected check box or not
            var checked_rows = 0;
            $('#<%=gvPolicy.ClientID%> input:checkbox').each(function () {
               if (this.checked) {
                   checked_rows += 1;
               }
            });
          
           if (checked_rows > 0) {
               $('#btnOk').val(option);
               $('#confirm_title').html('Confirm ' + option);
               $('#modal_confirm').modal('show');

               if (option != 'Approve') {
                   $('#row_issued_date').hide();
                  
               }
               else {
                   $('#row_issued_date').show();
               }



           }
           else {
               alert('Please check record(s) to ' + option);
           }

        }
        //save approve and decline
       function save() {
           var btn = $('#btnOk').val();

           if (btn == 'Approve') {
               if (confirm('Do you want to approve?')) {
                   approve.click();
               }
           }
           else if (btn == 'Decline') {
               if (confirm('Do you want to decline?')) {
                   decline.click();
               }
           }
         
       }
       function show_control(option)
       {
           if (option == 1) {
               $('.toolbar').show();
               $('#content').show();
           }
           else if (option == 0)
           {
               $('.toolbar').hide();
               $('#content').hide();
           }
       }
       //function single_select(appNumber, customerName)
       function single_select(ckb, appNumber, customerName)
       {
           var GridView = ckb.parentNode.parentNode.parentNode;

           var ckbList = GridView.getElementsByTagName("input");

           for (i = 0; i < ckbList.length; i++) {
               if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                   ckbList[i].checked = false;
                  
               }
           }
          
           if (ckb.checked) {
               $('#<%=txtApplicationNumber.ClientID%>').val(appNumber);
               $('#<%=txtCustomerName.ClientID%>').val(customerName);
               
           }
       }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="message" runat="server" style=" text-align: center; vertical-align: middle; color: #000; font-weight: bold; padding: 10px; position: absolute; width: 96%; border-radius: 10px; background-color: red;"></div>
    <div id="content">
    <h1>Approve Policy</h1>
    
    <table class="table-layout" style="background-color: #f6f6f6; padding: 0px; border:0px;">
        <tr>
            <td>
                <asp:GridView ID="gvPolicy" runat="server"  OnRowDataBound="gvPolicy_RowDataBound"
                    AutoGenerateColumns="False" CssClass=" grid-layout" Width="100%">
                    <SelectedRowStyle BackColor="#EEFFAA" />

                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="ckbAll" runat="server" style="display:none;" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckbPolicy" onclick='<%# "single_select(this,\"" + Eval("ApplicationNumber") + "\", \"" + Eval("CustomerName") + "\");" %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Policy ID" Visible="false">
                            <ItemStyle CssClass="g_row" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPolicyID" Text='<%# Eval("PolicyID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Application Number" SortExpression="ApplicationNumber">
                            <ItemStyle CssClass="g_row" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblApplicationNumber" Text='<%# Eval("ApplicationNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name" SortExpression="CustomerName">
                            <ItemStyle CssClass="g_row" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFirstNameEn" Text='<%# Eval("CustomerName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gender" SortExpression="gender">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblGender" Text='<%# Eval("Gender")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product" SortExpression="ProductID">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblProduct" Text='<%# Eval("ProductID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S.I (USD)">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSI" Text='<%# Eval("SumInsured")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effective Date">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEffectiveDate" Text='<%# Convert.ToDateTime( Eval("EffectiveDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assured Year">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblAssuredYear" Text='<%#  Eval("AssuredYear") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pay Year">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPayYear" Text='<%#  Eval("PayYear") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Premium (USD)">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPremium" Text='<%#  Eval("PremiumByMode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Extra Premium (USD)">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblExtraPremium" Text='<%#  Eval("ExtraPremiumByMode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDiscount" Text='<%#  Eval("Discount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Premium (USD)">
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTotalPremium" Text='<%# Convert.ToDouble( Eval("PremiumByMode")) + Convert.ToDouble( Eval("ExtraPremiumByMode"))  - Convert.ToDouble( Eval("Discount")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle CssClass="g_row g_row_center" />
                            <HeaderStyle CssClass="g_header" />
                            <ItemTemplate>
                                <button type="button" class="btn btn-small" onclick='<%# "view_detail(\"" + Eval("PolicyID")  + "\");" %>'><span class="icon-eye-open"></span> View</button>
                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </td>
        </tr>
    </table>

    <div id="modal_confirm" class="modal hide fade large " tabindex="-1" role="dialog" aria-labelledby="Confirm" aria-hidden="true" data-keyboard="false" data-backdrop="static">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="confirm_title">Confirm</h3>
        </div>
        <div class="modal-body ">
            <div>
                <table style="width: 100%;" class=" table-condensed">
                    <tr>
                        <td>
                            <asp:Label ID="lblApplicationNumber" runat="server" Text="Application Number"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtApplicationNumber" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="row_issued_date">
                        <td>
                            <asp:Label runat="server" ID="lblIssuedDate" Text="Issued Date"></asp:Label>
                        </td>

                        <td>
                            <asp:TextBox runat="server" ID="txtIssuedDate"  CssClass=" datepicker"></asp:TextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblRemarks" Text="Remarks"></asp:Label>
                        </td>

                        <td>
                            <asp:TextBox runat="server" ID="txtRemarks" Width="90%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>

                </table>
            </div>
        </div>
        <div class="modal-footer">
            <input type="button" id="btnOk" value="Search" onclick="save();" class="btn btn-primary" />
        </div>
    </div>
        </div>
</asp:Content>

