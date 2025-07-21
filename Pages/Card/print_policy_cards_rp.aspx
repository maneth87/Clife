<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print_policy_cards_rp.aspx.cs" Inherits="Pages_Card_print_policy_cards_rp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="div_message" runat="server" style="text-align:center; color:red;margin-top:300px;"></div>
   <%-- <div>
    <rsweb:ReportViewer ID="my_report_view" runat="server" Font-Names="Verdana" Font-Size="8pt" 
               WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="60%" Height="100%" ShowBackButton="False" ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowFindControls="False" ShowPageNavigationControls="False" ShowParameterPrompts="False" >
          </rsweb:ReportViewer>
    </div>--%>
    </form>
</body>
</html>
