<%@ Page Language="C#" AutoEventWireup="true" CodeFile="data_check_life_RP.aspx.cs" Inherits="Pages_Business_Reports_data_check_life_RP" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" DisplayStatusbar="False" EnableDrillDown="False" EnableParameterPrompt="False" EnableToolTips="False" HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" HasExportButton="False" HasGotoPageButton="False" HasPageNavigationButtons="False" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" HasZoomFactorList="False" PrintMode="ActiveX" ToolPanelView="None"  />
    </div>
        <div id="message" runat="server" style="margin-top:300px; text-align:center; color:red; display:none;"></div>
    </form>
</body>
</html>
