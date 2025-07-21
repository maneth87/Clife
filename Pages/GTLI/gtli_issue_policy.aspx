<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="gtli_issue_policy.aspx.cs" Inherits="Pages_GTLI_gtli_issue_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

    <ul class="toolbar">
        <li>
            <input type="button" data-toggle="modal" data-target="#myCancelModal" style="background: url('../../App_Themes/functions/cancel.png') no-repeat; border: none; height: 40px; width: 90px;" />
            <div style="display: none;">
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" />
            </div>
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            </div>
            <input type="button" data-toggle="modal" data-target="#mySaveModal"  style="background: url('../../App_Themes/functions/issue_policy.png') no-repeat; border: none; height: 40px; width: 100px;" />
            
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
        function SelectSingleCheckBox(ckb, policy_id, premium_id) {
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

                $('#Main_hdfPolicyID').val(policy_id);
                                   
                $('#Main_hdfPremiumID').val(premium_id);
                             

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                $('#Main_hdfPolicyID').val("");

                $('#Main_hdfPremiumID').val("");
            }
        }

        //Cancel Policy Underwrite
        function CancelPolicy() {
            if ($('#Main_hdfPremiumID').val() == "") {
                alert("Please select a checkbox.");
            } else {
                var btnCancel = document.getElementById("<%= btnCancel.ClientID %>");
                btnCancel.click();
            }
           
        }

        //Save Policy
        function SavePolicy() {

            if ($('#Main_hdfPremiumID').val() == "") {
                alert("Please select a checkbox.");
            } else {
                var btnSave = document.getElementById("<%= btnSave.ClientID %>");
                btnSave.click();
            }
        }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Issue Policy</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <asp:GridView ID="gvPolicyTemp" runat="server" Font-Names="Arial" Width="100%"
                AutoGenerateColumns="False" DataKeyNames="GTLI_Policy_ID" OnRowCommand="gvPolicyTemp_RowCommand" OnRowDataBound="gvPolicyTemp_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="IndentBottom" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("GTLI_Policy_ID" ) + "\", \"" + Eval("GTLI_Premium_ID" ) + "\");" %>' />
                            
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
                    <asp:BoundField DataField="GTLI_Plan" HeaderText="Plan" SortExpression="GTLI_Plan" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Transaction_Staff_Number" HeaderText="Employees" SortExpression="Transaction_Staff_Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="70">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                       <asp:BoundField DataField="company" HeaderText="Customer" SortExpression="company" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="250" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle Width="250px"></ItemStyle>
                       </asp:BoundField>
                       <%-- <asp:BoundField DataField="Policy_Number" HeaderText="GTLI Number" SortExpression="Policy_Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="130">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" SortExpression="Effective_Date" DataFormatString="{0:d-MMM-yyyy}" HtmlEncode="false" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="80">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Expiry Date">
                            <ItemTemplate>
                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Eval("Expiry_Date", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center" />

                            <ItemStyle Width="100px" HorizontalAlign="Center" />

                        </asp:TemplateField>
                        <asp:BoundField DataField="Sum_Insured" HeaderText="Sum Insured" SortExpression="Sum_Insured" DataFormatString="{0:C0}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="130" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Life_Premium" HeaderText="Life Premium" SortExpression="Life_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Accidental_100Plus_Premium" HeaderText="100Plus Premium" SortExpression="Accidental_100Plus_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TPD_Premium" HeaderText="TPD Premium" SortExpression="TPD_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DHC_Premium" HeaderText="DHC Premium" SortExpression="DHC_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Total_Premium" HeaderText="Total Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="90">
                            <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="100px" HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                       

                        <asp:TemplateField HeaderText="Detail">
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewDetailLink" runat="server" CommandArgument='<%# Eval("GTLI_Premium_ID")%>'
                                    CommandName="DetailSale">View</asp:LinkButton>
                                <asp:HiddenField ID="hdfTransactionType" Value='<%# Eval("Transaction_Type")%>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </div>

       <!-- Modal Save-->
     <div id="mySaveModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSaveHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H3">Save Policy Underwrite</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="vertical-align: middle">Are you sure, you want to save this policy?</td>                    
                </tr>               
            </table>          
        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="SavePolicy();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Save-->
    <!-- Modal Cancel-->
     <div id="myCancelModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalCancelHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Cancel Policy Underwrite</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="vertical-align: middle">Are you sure, you want to cancel?</td>                    
                </tr>               
            </table>          
        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="CancelPolicy();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Cancel-->

    <%--Hidden Fields--%>
    <asp:HiddenField ID="hdfPolicyID" runat="server" />
    <asp:HiddenField ID="hdfPremiumID" runat="server" />
</asp:Content>

