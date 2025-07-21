<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmCIDetailReport.aspx.cs" Inherits="Pages_CI_Report_frmCIDetailReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <script  type="text/javascript">
       
    function open_report(opt) {
        if (fdate.val() == '' && tdate.val() == '' && customer_name.val() == '' && policy_number.val() == '') {

            
            alert('Report filter is required.');
        }
        else {
            if (opt == 'pdf') {
                btn.click();
            }
            else if (opt == 'excel')
            {
                btnexcel.click();
            }
        }

    };
    function cls() {
        fdate.val('');
        tdate.val('');
        customer_name.val('');
        policy_number.val('');

    };
    var fdate ;
    var tdate ;
        var customer_name ;
        var policy_number;
        var btn;
        var btnexcel;
    $(document).ready(function () {
        $('.datepicker').datepicker();

         fdate = $('#<%=txtFromDate.ClientID%>');
         tdate = $('#<%=txtTodate.ClientID%>');
         customer_name = $('#<%=txtCustomerName.ClientID%>');
         policy_number = $('#<%=txtPolicyNumber.ClientID%>');
        btn = $('#<%=btnPrint.ClientID%>');
        btnexcel = $('#<%=btnExcel.ClientID%>');

        $('#tbl_filter').css('margin-left', ($('#div_content').width() / 2) - ($('#tbl_filter').width() / 2));
        $('#tbl_filter').css('border', 'solid 2px black');
    });
</script>
    <ul class="toolbar">
        <li>
            <input type="button" style="background: url('../../App_Themes/functions/download_excel.png') no-repeat; border: none; height: 40px; width: 90px;"  onclick="open_report('excel');" />
            <asp:Button id="btnExcel" runat="server" style="display:none;" OnClick="btnExcel_Click" />
        </li>
        <li>
            <input type="button" style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;"  onclick="open_report('pdf');" />
            <asp:Button id="btnPrint" runat="server" style="display:none;" OnClick="btnPrint_Click" />
        </li>
        <li>
            <input type="button" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" onclick="cls();" />
        </li>
    </ul>
    <h1>Policy Detail Report</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <br />
    <div class="container" id="div_content">
        
        <table style="margin:10px; border:solid 2px;" id ="tbl_filter">
            <tr>
                <td colspan="4"><h3>Report Filter</h3></td>
            </tr>
            <tr>
                 <td style="padding-left:10px;"><asp:Label ID="lblFromDate" runat="server" Text="Issued Date From" ></asp:Label></td>
                 <td><asp:TextBox ID="txtFromDate" runat="server" PlaceHolder="DD/MM/YYYY" CssClass=" datepicker" ></asp:TextBox></td>
                 <td><asp:Label ID="lblToDate" runat="server" Text="To" ></asp:Label></td>
                 <td><asp:TextBox ID="txtTodate" runat="server" PlaceHolder="DD/MM/YYYY" CssClass="datepicker" ></asp:TextBox></td>
            </tr>
             <tr>
                 <td style="padding-left:10px;"><asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" ></asp:Label></td>
                 <td><asp:TextBox ID="txtCustomerName" runat="server" PlaceHolder="Last name or first name" ></asp:TextBox></td>
                 <td><asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number" ></asp:Label></td>
                 <td><asp:TextBox ID="txtPolicyNumber" runat="server" placeholder="85512XXXXXX" ></asp:TextBox></td>
            </tr>
            <tr>
                <td style="padding-left:10px;">
                    <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rbtlReportType" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" Width="200px" >
                        <asp:ListItem Text="Policy Detail" Value="1" Selected="True" ></asp:ListItem>
                        <asp:ListItem Text="Premium Detail" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>

    </div>
</asp:Content>

