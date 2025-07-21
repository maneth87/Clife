<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_commission.aspx.cs" Inherits="Pages_Reports_policy_commission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
   <ul class="toolbar" style="text-align:left">
       
        <li>
            <asp:ImageButton ID="ImgPrint"  runat="server" ImageUrl="~/App_Themes/functions/download_excel.png"  OnClick="ImgPrint_Click"  style="display:none;"/>
            <input type="button" onclick="print();" style="background: url('../../App_Themes/functions/download_excel.png') no-repeat; border: none; height: 40px; width: 100px;" />
        </li>
          
      
  </ul>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

     <script type="text/javascript">

         $(document).ready(function () {
             $('.datepicker').datepicker();
         });

         function print()
         {
             
             var f = document.getElementById("<% =txtFromDate.ClientID %>");
             var t = document.getElementById("<% =txtToDate.ClientID %>");
             var btn = document.getElementById("<% =ImgPrint.ClientID %>");
             if (f.value != '' && t.value != '') {
                 btn.click();
             }
             else {
                 alert('From Date and To Date are required.');
             }

         }
    </script>

     
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <br /><br /><br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Export To Excel</h3>
                    
                </div>
            <div class="panel-body">
                <table >
                    <tr>
                        <td>Entry Date</td>
                        <td>
                             <asp:TextBox ID="txtEntryDate" runat="server" CssClass="datepicker"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>

                        <td>
                         From Date

                        </td>
                        <td>
                           <%-- <input type="text" id="Text1" class=" datepicker" />--%>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker"></asp:TextBox>
                        </td>
                         <td>To Date</td>
                        <td>
                            <%--<input type="text" id="txtToDate" class="datepicker" />--%>
                             <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker"></asp:TextBox>
                        </td>
                    </tr>
                   
                </table>
                <div style="margin-top:20px;">
                    <p class="text-info" style=" font-size:small;">* Note: (From Date, To Date) stands for payment date.</p>
                </div>
            </div>

                </div>
        </ContentTemplate>
        <Triggers>
          <%-- <asp:PostBackTrigger ControlID="ImgPrint" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

