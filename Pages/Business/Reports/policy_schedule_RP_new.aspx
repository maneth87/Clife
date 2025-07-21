<%@ Page Language="C#" AutoEventWireup="true" CodeFile="policy_schedule_RP_new.aspx.cs" Inherits="Pages_Business_Reports_policy_schedule_RP" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
   
    <div id="message" runat="server" style=" text-align: center; vertical-align: middle; color: #000; font-weight: bold; padding: 10px; position: absolute; width: 96%; border-radius: 10px; background-color: red;"></div>

      <div>
          <rsweb:ReportViewer ID="my_report_view" runat="server" Font-Names="Verdana" Font-Size="8pt" 
               WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="60%" ShowBackButton="False" ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowFindControls="False" ShowPageNavigationControls="False" ShowParameterPrompts="False" >

          </rsweb:ReportViewer>
      </div>
    </div>
    </form>
</body>
</html>