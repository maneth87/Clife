<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="premium_pma_product_report.aspx.cs" Inherits="Pages_Reports_pma_product_premium_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <ul class="toolbar" style="text-align:left">
       
        <li>
             
            <input type="button" onclick="PrintReport();" onmouseover="tooltip.pop(this, 'Print This Report')" style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>          
            <input type="button"  onclick="ExportExcel();" style="background: url('../../App_Themes/functions/download_excel.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>      
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalReportProductPremium" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
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
        function ExportExcel() {

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
    <%--<link href="../../App_Themes/gtli_print.css" rel="stylesheet" />--%>

    <%--Export Excel--%>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%--here is the block code--%>
            <br /><br /><br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Product Premium Report (PMA)</h3>
                    
                </div>
                
             <!-- Modal Report Product Premium Report Form -->
           
            <div class="panel-body">
                     
            <div id="dvReport">        
                    <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                    <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>
                   
                    <br />
                    <!-- Append data to Div Summary -->
                    <div id="dvReportSummary" runat="server">
              
                    </div>

                    <br />
                    <!-- Append data to Div Detail -->
                    <div id="dvReportDetail" runat="server">
              
                    </div>
            </div>                 
                
            <div id="myModalReportProductPremium"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalReportHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Product Premium Report Search</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table style="margin-left:40px;"  text-align: left;">
                        <tr>
                            <td>From Date:</td>
                            <td>
                                <asp:HiddenField ID="hdfFromDate" runat="server" />
                                <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker span2" onkeypress="return false;" TabIndex="1" ></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidatorFromDate" ControlToValidate="txtFrom_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                            <td>To Date:</td>
                            <td>
                                <asp:HiddenField ID="hdfToDate" runat="server" />
                                <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker span2" onkeypress="return false;" TabIndex="2" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" ControlToValidate="txtTo_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table><br />    

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnOk" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="1" OnClick="btnOk_Click"  />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="ClearGrid();">Cancel</button>
                </div>
            </div>
            <!--End Modal Report Product Premium Report Form-->
                           
             <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
             <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>

            </div>
            </div>
                   
	</ContentTemplate>

          <Triggers>
             <asp:PostBackTrigger ControlID="btnOk" />            
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>

