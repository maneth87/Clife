<%@ Page Language="C#" AutoEventWireup="true" CodeFile="policy_schedule_RP.aspx.cs" Inherits="Pages_Business_Reports_policy_schedule_RP" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../App_Themes/print.css" rel="stylesheet" />
    <style>
        .ColHeader {
            text-align:left;
            background-color:#333991;
            color:white;
            vertical-align:middle;
            padding-left:5px;
           
        }
         .ColHeaderSub
        {
             text-align:center;
            
        }
        .RowDetail {
            padding-left:5px;

        }
    </style>
    <script>
        function PrintDetails() {
            //Open hidden div and print
            var printContent = document.getElementById('<%= printarea.ClientID %>');

            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, '');
            printWindow.document.write(printContent.innerHTML);
           // printWindow.document.getElementById('print').style.display = 'block';
            printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/print.css" type="text/css" />');

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }
        function Print()
        {
            var dvReport = document.getElementById("div_report");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else
            {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding:10px 10px 10px 10px; text-align:left; display:none;">
            <asp:ImageButton  runat="server" ID="imgPrint" ImageUrl="~/App_Themes/functions/print.png" OnClick="imgPrint_Click" OnClientClick="PrintDetails();" />
        </div>
        <div id="message" runat="server" style="color:red; text-align:center; margin-top:200px;"></div>
        <div id="div_report" runat="server" style="text-align:center;">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="50px"  ToolPanelView="None" ToolPanelWidth="200px" Width="350px" 
                PrintMode="ActiveX" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" EnableDatabaseLogonPrompt="False" 
                EnableParameterPrompt="False" DisplayStatusbar="False" EnableTheming="False" EnableToolTips="False" RenderingDPI="100" BestFitPage="True" BorderStyle="None" HasDrilldownTabs="False" EnableDrillDown="False" />
       
        </div>
        <div id="printarea" runat="server" class="print_content">
            <div id="print" runat="server" class="print_content" ></div>
        </div>
       
    </form>
</body>
</html>