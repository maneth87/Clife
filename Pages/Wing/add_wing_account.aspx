<%@ Page Title="Clife | WING => Add Account" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="add_wing_account.aspx.cs" Inherits="Pages_Wing_add_wing_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>
        
        
        <li>
            <asp:ImageButton ID="ImgBtnAdd" data-toggle="modal" data-target="#myModalWINGAccount" runat="server" ImageUrl="~/App_Themes/functions/add.png" />
        </li>


    </ul>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>


            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Account Wing</h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvWingAccount" runat="server" CssClass="grid-layout" AutoGenerateColumns="False" DataSourceID="WINGAccountDataSource" Width="100%" HorizontalAlign="Center" PageSize="100">
                        <Columns>
                            <asp:BoundField DataField="Date_Request" HeaderText="Date Request" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Date_Request">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WING_SK" HeaderText="SK" SortExpression="WING_SK">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Wing_Number" HeaderText="Wing #" SortExpression="Wing_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Customer_Name" HeaderText="Name" ReadOnly="True" SortExpression="Customer_Name">
                                <ItemStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Gender" HeaderText="Gender" ReadOnly="True" SortExpression="Gender">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ID_Type" HeaderText="ID Type" ReadOnly="True" SortExpression="ID_Type">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            

                            <asp:BoundField DataField="ID_Number" HeaderText="ID Number" SortExpression="ID_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Birth_Date" HeaderText="Date of Birth" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Birth_Date">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Contact_Number" HeaderText="Contact No." SortExpression="Contact_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number" SortExpression="Policy_Number">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>


                    <asp:SqlDataSource ID="WINGAccountDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK ORDER BY WING_SK DESC"></asp:SqlDataSource>


                    <div id="myModalWINGAccount" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalWingAccountHeader" aria-hidden="true">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 class="panel-title">Add WING Account for Policy</h3>
                        </div>

                        <div class="modal-body">
                            <!---Modal Body--->
                            <table width="100%" style="text-align: left; padding-left: 20px;">
                               
                                <tr>
                                    <td width="100px">&nbsp;&nbsp;WING SK:</td>
                                    <td>
                                        <asp:TextBox ID="txtWINGSK" runat="server" Width="90%" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" ControlToValidate="txtWINGSK" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </td>


                                </tr>
                                 <tr>
                                    <td width="100px">&nbsp;&nbsp;POLICY No:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumber" runat="server" Width="90%" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorFromDate" ControlToValidate="txtPolicyNumber" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </td>

                                </tr>
                            </table>
                            <br />

                        </div>

                        <div class="modal-footer">                            
                            <asp:Button ID="btnSave" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Save" ValidationGroup="1" OnClick="btnSave_Click" />
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                        </div>
                    </div>

                </div>

            </div>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

