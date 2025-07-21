<%@ Page Title="Policy Return" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_return.aspx.cs" Inherits="Pages_Micro_policy_return" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">

     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script>
        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });
    </script>
    

     <%-- Upload Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Micro Policy Return</h3>
        </div>
        <div class="panel-body">
            <%--Policy Return Content Here--%>
            <table>
               
                <tr>
                    <td style="text-align: right">Policy Number:</td>
                    <td>
                        <asp:TextBox ID="txtPolicyNumber" runat="server" Width="91%"  MaxLength="50"></asp:TextBox>
                                               
                    </td>
                    <td>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidatorPolicyNumber" runat="server" ErrorMessage="Require Policy Number" ControlToValidate="txtPolicyNumber" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                       
                    </td>
                </tr>      
                <tr>
                    <td style="text-align: right">Resign Date:</td>
                    <td>
                        <asp:TextBox ID="txtResignDate" runat="server" onkeypress="return false;" Width="91%" CssClass="datepicker" ></asp:TextBox>
                    </td>
                    
                    <td>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Require Policy Number" ControlToValidate="txtPolicyNumber" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td style="vertical-align:top">
                        &nbsp;&nbsp;<asp:Button ID="btnCalculateReturn" CssClass="btn-primary" runat="server" Text="Calculate" OnClick="btnCalculateReturn_Click" />
                    </td>
                </tr>          
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                    <td></td>
                </tr>
            </table>
            <br />
            <div id="dvContent" width="100%" runat="server" border="1"></div>

        </div>
    </div>
    <%--End Policy Return Section--%>

</asp:Content>

