<%@ Page Language="C#" AutoEventWireup="true" CodeFile="data_check_life_RP_new.aspx.cs" Inherits="Reports_report_view" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <div id="div_message" runat="server" style="background-color:#94D1FF; color:#f00; border-radius:5px; text-align:center; margin-top:50px; height:30px; vertical-align:middle; padding:5px;"></div>
    
     <%-- <div>
          <rsweb:ReportViewer ID="my_report_view" runat="server" Font-Names="Verdana" Font-Size="8pt" 
               WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="60%" Height="100%" ShowBackButton="False" ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowFindControls="False" ShowPageNavigationControls="False" ShowParameterPrompts="False" >
          </rsweb:ReportViewer>
          
      </div>--%>
    </div>
    </form>
</body>
</html>
