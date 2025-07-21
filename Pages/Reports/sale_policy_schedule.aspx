<%@ Page Title="Clife Sale Report => Policy Details" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="sale_policy_schedule.aspx.cs" Inherits="policy_schedule" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <ul class="toolbar" style="text-align:left">
        <li>
             
            <input type="button" onclick="PrintReport();" onmouseover="tooltip.pop(this, 'Print This Report')" style="background: url('../../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>          
            <input type="button"  onclick="ExportExcelActiveMember();" style="background: url('../../../App_Themes/functions/download_excel.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>   
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
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

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <link href="../../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    
    <script src="../../../Scripts/jquery.print.js"></script>
    <%--<link href="../../App_Themes/gtli_print.css" rel="stylesheet" />--%>

    <%--Export Excel--%>
    <script src="../../../Scripts/jquery.battatech.excelexport.min.js"></script>
    <script src="../../../Scripts/jquery.battatech.excelexport.js"></script>

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%--here is the block code--%>

             <!-- Modal Search -->
            <div id="myModalSearch" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Policy List</h3>
                </div>

                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>From Policy Year</td>
                            <td>
                                <asp:TextBox ID="txtFrom_PolicyYear"  runat="server"  width="136px" TabIndex="1" ></asp:TextBox>
                           </td>
                            <td>To Policy Year</td>
                            <td>
                                <asp:TextBox ID="txtTo_PolicyYear"  runat="server"  width="136px" TabIndex="2" ></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnSearch" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnSearch_Click"    />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" >Cancel</button>
                </div>
            </div>
            <!-- End of Modal Search -->


           <br /><br /><br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Policy Schedule</h3>
                </div>
               
             <!-- Modal Report Policy Form -->
           
                <div class="panel-body">
                     
                    <div id="dvReport">        
                        <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                        <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>
              
                        <br />
                        <!-- Append data to Div -->
                        <div id="dvReportDetail" runat="server">
              
                        </div>

                        <br /><br /><br />
                         <asp:Label ID="lblAgentName" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Label ID="lblAgentPhone" runat="server" ></asp:Label>

                    </div>                 
                                   
                     <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
                     <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>
                </div>
            </div>

	</ContentTemplate>
           <Triggers>
              <asp:PostBackTrigger ControlID="btnSearch" />
          </Triggers>
    </asp:UpdatePanel>	
</asp:Content>
