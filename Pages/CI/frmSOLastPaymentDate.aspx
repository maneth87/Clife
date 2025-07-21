<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmSOLastPaymentDate.aspx.cs" Inherits="Pages_CI_frmSOLastPaymentDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <script  type="text/javascript">

        function excel() {
            btnexcel.click();

        };
        function cls() {
            agent_code.val(".");
            policy_number.val('');

        };
      
        var policy_number;
        var btn;
        var btnexcel;
        var agent_code
        $(document).ready(function () {
            $('.datepicker').datepicker();

        policy_number = $('#<%=txtPolicyNumber.ClientID%>');
      
            btnexcel = $('#<%=btnExcel.ClientID%>');
            agent_code = $('#<%=ddlAgent.ClientID%>');

        $('#tbl_filter').css('margin-left', ($('#div_content').width() / 2) - ($('#tbl_filter').width() / 2));
        $('#tbl_filter').css('border', 'solid 2px black');
    });
</script>
    <ul class="toolbar">
        <li>
            <input type="button" style="background: url('../../App_Themes/functions/download_excel.png') no-repeat; border: none; height: 40px; width: 90px;"  onclick="excel();" />
            <asp:Button id="btnExcel" runat="server" style="display:none;" OnClick="btnExcel_Click" />
        </li>
       
        <li>
            <input type="button" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" onclick="cls();" />
        </li>
    </ul>
    <h1>Policy Last Payment Report</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <br />
    <div class="container" id="div_content">
        
        <table style="margin:10px; border:solid 2px; width:600px;" id ="tbl_filter">
            <tr>
                <td colspan="3"><h3>Report Filter</h3></td>
            </tr>
           
             <tr>
                 <td style="padding-left:10px;"><asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number" ></asp:Label></td>
                 <td><asp:TextBox ID="txtPolicyNumber" runat="server" placeholder="85512XXXXXX;85512XXXXX1" Width="480px" ></asp:TextBox></td>
            </tr>
             <tr>
                 <td style="padding-left:10px;"><asp:Label ID="Label1" runat="server" Text="Policy Number From File" ></asp:Label></td>
                 <td> <asp:FileUpload ID="PolicyNumberFile" runat="server" />
                    <br /> <a href="SIMPLE_ONE_POLICY_NUMBER_TEMPLATE.xlsx" style="color: blue;"><u>Download Template File</u> </a>
                 </td>
            </tr>
            <tr>
               
                    <td style="padding-left:10px;"><asp:Label ID="lblAgent" runat="server" Text="Agent Name" ></asp:Label></td>
               <td>
                   <asp:DropDownList ID="ddlAgent" runat="server"  Width="495px" AppendDataBoundItems="true" >
                       <asp:ListItem Text="." Value=""></asp:ListItem>
                   </asp:DropDownList>
               </td>
             
            </tr>
           <tr>
               <td colspan="2" style="text-align:center">
                   <asp:Button ID="btnSearch" runat="server" Text="Search" style="border-radius:8px; display:none;" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
               </td>
           </tr>
        </table>
       
    </div>
</asp:Content>

