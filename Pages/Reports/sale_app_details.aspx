<%@ Page Title="Clife Sale Report => Application Details" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="sale_app_details.aspx.cs" Inherits="Pages_Reports_sale_app_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <ul class="toolbar" style="text-align:left">
       
        <li>
             
            <input type="button" onclick="PrintReport();" onmouseover="tooltip.pop(this, 'Print This Report')" style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>          
            <input type="button"  onclick="ExportExcelActiveMember();" style="background: url('../../App_Themes/functions/download_excel.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>      
            
    </ul>
    <script type="text/javascript">
        //Section date picker //  $(document).ready(function ()) means that everything in this function will start auto when Loading page
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        //Function to print report
        function PrintReport() {
            //Open hidden div and print

            var printContent = $("#dvReport").html();
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, '');
            printWindow.document.write(printContent);
            printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/print.css" type="text/css" />');

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }
               
        //Function Export report to excel
        function ExportExcelActiveMember() {

            $("#dvReport").btechco_excelexport({
                containerid: "dvReport"
               , datatype: $datatype.Table
            });
        }
    </script>

    <style>
           
        /*print content*/
        @media print {
               
            .PrintPD {
               
                display: block;
            }
        }

        /*view content*/
        @media screen {
            
            .PrintPD {
               
                display: block;
            }
        }
      
                
        .PrintPD {
            width: 30%;
            border: solid;
            border-color: pink;
            margin-left: 12%;
            margin-right: 50%;
            padding-left: 1%;
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-size: 12px;
        }

        .rdb_style {
            height: 0px;
            padding: 5px 5px 5px 5px;
        }

        .radio_style input[type="radio"],
        .checkbox input[type="checkbox"] {
            float: left;
            margin-left: -20px;
            width: 13px;
            height: 13px;
            padding: 0;
            margin: 0;
            vertical-align: middle;
            position: relative;
            top: 4px;
            *overflow: hidden;
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-size: 10px;
            color: red;
        }

        .panel_border {
            /*BorderStyle: BorderWidth="1" BorderColor="Gray"*/
            border: solid;
            border-width: 1px;
            border-color: gray;
            width: 5px;
            border-left: hidden;
            border-right: hidden;
            border-bottom: hidden;
        }

        .indent_bottom {
            padding-bottom: 6px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    
    <script src="../../Scripts/jquery.print.js"></script>

    <%--Export Excel--%>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%--here is the block code--%>
            <br /><br /><br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Sales Application</h3>
                    
                </div>
                
             <!-- Modal Report Application Status Form -->
           
            <div class="panel-body">
                     
            <div id="dvReport">        
                    <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                    <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>
              
                    <br />
                    <!-- Append data to Div -->
                    <div id="dvReportDetail" runat="server">
              
                    </div>
            </div>                 
                
                                   
             <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
             <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>

            </div>
            </div>
                   
	</ContentTemplate>

          <Triggers>
             
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>

