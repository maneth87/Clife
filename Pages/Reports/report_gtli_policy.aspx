<%@ Page Title="Clife | Reports => GTLI Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_gtli_policy.aspx.cs" Inherits="Pages_Reports_report_gtli_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" ></asp:ScriptManagerProxy>

 
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
  
    <script src="../../Scripts/jquery.print.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>

         <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>
    
    <ul class="toolbar" style="text-align:left">
       
        <li>          

            <input type="button"  onclick="ExportExcelGTLI();" style="background: url('../../App_Themes/functions/download_excel.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>
       <li>
         
           <input type="button"  onclick="PrintReport();" style="background: url('../../App_Themes/functions/print.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      

       </li>
        <li>

            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
           
        </li>
  </ul>

    <style>
        .text {
            mso-number-format:\@;

        }
        .ui-autocomplete {
            z-index: 5000;
        }
    </style>

      <script type="text/javascript">

          //Section date picker //  $(document).ready(function ()) means that everything in this function will start auto when Loading page

          $(document).ready(function () {

              $('.datepicker').datepicker();

          });
                        
          //Company Name autocomplete
          PageMethods.GetCompanyName(function (results) {

              $("#txtCompany").autocomplete({
                  source: function (request, response) {
                      var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                      response($.grep(results, function (item) {
                          return matcher.test(item);
                      }));
                  }
              });
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
              printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/gtli_print.css" type="text/css" />');

              printWindow.document.close();
              printWindow.focus();
              printWindow.print();
              printWindow.close();

              return false;
          }

          //Function Export report to excel
          function ExportExcelGTLI() {

              $("#dvReport").btechco_excelexport({
                  containerid: "dvReport"
                 , datatype: $datatype.Table
              });
          }

          function ShowSearchForm() {
              $("#Main_txtFrom_date").val("");
              $("#Main_txtTo_date").val("");
              $("#Main_txtPolicyNum").val("");
              $("#Main_txtAgentCode").val("");
              $("#txtCompany").val("");

              $("#myModalReportPolicy").modal("show");
          }
    </script>

    <style>               

        .div_policy {
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
   
   
    <%-- Form Design Section--%>
    <h1>Policy Report</h1>
  
		<%--here is the block code--%>
        <!-- Modal Report Policy Form -->           
            <div style="height:30px"></div>        

            <div class="panel panel-default">
              
                <div class="panel-body">
                    <!-- Append data to Div -->
          
                                            
            <!-- End of Append Data to Div -->
                    <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
                    <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>
            <!-- Append data to Div -->

              <div id="dvReport" align="center" width="98%">        
                    <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                    <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>
              
                    <br />
                    <!-- Append data to Div -->
                    <div id="AppendDivPolicy" runat="server" align="center">
              
                    </div>
               </div>  
            <!-- End of Append Data to Div -->

            <!-- Add more lable for date -->

            <div id="myModalReportPolicy" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalReportPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Policy Report</h3>
                </div>

                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                         <tr>                            
                            <td>Policy Number</td>
                            <td>
                                <asp:HiddenField ID="hdfPolicyNum" runat="server"  Value='<%# Bind("txtPolicyNum.text") %>'/>
                                <asp:TextBox ID="txtPolicyNum" runat="server" Width="86%" TabIndex="1"></asp:TextBox>
                              
                            </td>
                            <td>Agent Code:</td>
                            <td>
                                 <asp:TextBox ID="txtAgentCode" runat="server" Width="86%" TabIndex="2"></asp:TextBox>
                            </td>
    
                        </tr>   
                        <tr>                            
                            <td>Company</td>
                            <td colspan="3">
                                <asp:HiddenField ID="hdfCompany" runat="server"  Value='<%# Bind("txtCompany.text") %>'/>
                               
                                <asp:TextBox ID="txtCompany" runat="server" Width="94%" TabIndex="3" ClientIDMode="Static"></asp:TextBox>   
                                
                            </td>                          
    
                        </tr>                     
                        <tr>
                            <td>From Date:</td>
                            <td>
                                <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker" width="86%" TabIndex="4" onkeypress="return false;" ></asp:TextBox>
                               
                            </td>
                            <td>To Date:</td>
                            <td>
                                <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker" width="86%" TabIndex="5" onkeypress="return false;"  ></asp:TextBox>
                                
                            </td>
                        </tr>                     
                       
                       
                    </table>
                    <br />
           
                    <div style="float:left; width:35%; padding-left:20px; margin-right:10%; box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:199px;">
                            <table>
                                <tr>
                                    <td style="padding:20px 20px 20px 20px;">Order by:
                                        <asp:Panel ID="Panel1" runat="server"  CssClass="panel_border" Height="90px" Width="150px">
                                            <asp:RadioButtonList ID="RdbOrderBy" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" width="100%"  >
                                                <asp:ListItem Value="1" Selected="True"><label>&nbsp;&nbsp;Policy number</label></asp:ListItem>
                                              
                                                <asp:ListItem Value="2">&nbsp;&nbsp;Effective date</asp:ListItem>                                               
                                            </asp:RadioButtonList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                     </div>
                </div>

                <div class="modal-footer">
                    <%--<asp:Button ID="btnOk" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="1" OnClick="btnOk_Click" />--%>
                    <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="OK" OnClick="btnSearch_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="ClearGrid();">Cancel</button>
                </div>
            </div>
            <!--End Modal Report Policy Form-->

        
                </div>

                <asp:HiddenField ID="hdfuserid" runat="server" />
                <asp:HiddenField ID="hdfusername" runat="server" />
                <asp:HiddenField ID="hdfSaleAgentID" runat="server" />
            </div>            

   
                                
</asp:Content>

