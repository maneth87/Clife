<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_policy_update_status.aspx.cs" Inherits="Pages_Alteration_micro_policy_update_status" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="dv_main" runat="server">
        <asp:UpdatePanel ID="UP" runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Search Policy</h3>

                    </div>
                    <div class="panel-body">
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="3"></td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:Label ID="LblPolicyNo" runat="server" Text="Policy No"></asp:Label>
                                                <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPol" runat="server"></asp:TextBox>
                                            </td>

                                            <td style="vertical-align: top">
                                                <asp:Button ID="btnSearch" CssClass=" btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </table>
                    </div>
                    <div class="panel-heading">
                        <h3 class="panel-title">Policy Information</h3>

                    </div>
                    <div>

                        <table id="tbl_search" style="margin: 5px;">
                            <tr>

                                <td>
                                    <asp:HiddenField ID="hdfPolicyID" runat="server" />
                                    <asp:Label ID="lblPolcyNumber" runat="server" Text="Policy Number"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px;">
                                    <asp:Label ID="lblApplicationNo" runat="server" Text="Application No"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtApplicationNo" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCustomerNo" runat="server" Text="Customer No"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerNo" runat="server"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px;">
                                    <asp:Label ID="lblCusname" runat="server" Text="Customer Name"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIssueDate" runat="server" Text="Issue Date"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtIssueDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px;">
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEffectiveDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCurrentstatus" runat="server" Text="Current Status"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtcurrentstatus" runat="server"></asp:TextBox>

                                </td>
                                <td style="padding-left: 20px;">
                                    <asp:Label ID="lblPlicystatusRemark" runat="server" Text="Policy Status Remark"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlicystatusRemark" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPolicyStatus" runat="server" Text="New Status"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>

                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" class="span2" Width="218px" AppendDataBoundItems="true">
                                        <%--   <asp:ListItem Value="">---Select---</asp:ListItem>--%>
                                        <%--  <asp:ListItem Value="DELETE">Delete</asp:ListItem>
                                    <asp:ListItem Value="IF">In-force</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-left: 20px;">
                                    <asp:Label ID="lblpolicystatusdate" runat="server" Text="Policy Status Date"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpolicystatusdate" runat="server" CssClass="datepicker" placeholder="DD/MM/YYYY"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td colspan="4" style="text-align: right;">
                                    <asp:Button ID="btnUpdate" CssClass=" btn-primary" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                                </td>
                            </tr>
                        </table>

                    </div>

                    <div class="panel-heading">
                        <h3 class="panel-title">Upload Terminate Policy</h3>

                    </div>
                    <div>

                        <table id="Table1" style="margin: 5px;">
                            <tr>
                                <td>
                                    <asp:FileUpload ID="fdlPolicy" runat="server"></asp:FileUpload>
                                    <a href="Bundle_Policy_Terminated_Template.xlsx">Download File Template</a>
                                </td>

                            </tr>

                            <tr>

                                <td colspan="4" style="text-align: right;">
                                    <asp:Button ID="btnUpload" CssClass=" btn-primary" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                     <asp:Button ID="btnExport" CssClass=" btn-primary" runat="server" Text="Export" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="tab-pane">

                        <asp:GridView ID="gvResult" runat="server" CssClass="grid-layout" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                        </asp:GridView>
                    </div>
                </div>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
                  <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="upg" runat="server" AssociatedUpdatePanelID="UP">
            <ProgressTemplate>
                <div class="my_progress">
                    <div class="tr"></div>
                    <div class="main">
                        <div class="dhead">
                            <h2>PROCESSING</h2>
                        </div>
                        <p>
                            <img id="loader" src="../../App_Themes/images/loader.gif" alt="Progressing" />
                        </p>
                        <p>Please wait...</p>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</asp:Content>



