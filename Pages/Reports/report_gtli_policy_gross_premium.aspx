<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_gtli_policy_gross_premium.aspx.cs" Inherits="Pages_Reports_report_gtli_policy_gross_premium" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" ></asp:ScriptManagerProxy>

 
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    
    <script src="../../Scripts/jquery.print.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>
   
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
            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />      
             <%--<asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" ValidationGroup="1" OnClick="ImgBtnSearch_Click" />   --%>
           
        </li>
  </ul>
      <script type="text/javascript">
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

          //Show Search Form
          function ShowSearchForm() {
              $("#Main_txtFromDate").val("");
              $("#Main_txtToDate").val("");
              $("#myModalSearchGrossPremium").modal("show");
          }
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <%-- Form Design Section--%>
    <br />
    <br />
    <br />  

    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Gross Premium</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <div id="dvReport" align="center">

                <table id="tblTitle" width="100%" style="font-family: Arial; font-size: 16px;" runat="server" visible="false">
                    <tr>
                        <td style="text-align: center">                           
                                <span style="font-weight: bold; font-size: 16pt">
                                    <asp:Label ID="lblCompanyName" runat="server" Text="Cambodia Life Insurance PLC."></asp:Label>
                                </span><br />                        
                                <span style="font-weight: bold; margin-top: -7px; font-size: 12pt">
                                    <asp:Label ID="lblTitle" runat="server" Text="Group Term Life: Gross Premium"></asp:Label>

                                </span><br />
                                <span style="font-weight: bold; margin-top: -7px; font-size:10pt;">                                    
                                     <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                                     <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>             
                   
                                </span><br />

                        </td>
                    </tr>
                </table>            
                <!-- Append data to Div -->
                <div id="AppendDivPolicy" runat="server" align="center">
              
                </div>           
                
            </div>
          
        </div>
    </div>

     <%--Modal Search Gross Premium Form --%>
    <div id="myModalSearchGrossPremium" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchCompanyHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">Gross Premium Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
            <table width="100%">
                <tr>
                    <td width="15%">Start Date:
                    </td>
                    <td width="33%">
                         <asp:TextBox ID="txtFromDate" runat="server" Width="80%" class="datepicker" onkeypress="return false;" ValidationGroup="1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorFromDate" runat="server" Text="*" ForeColor="Red" ControlToValidate="txtFromDate" ValidationGroup="1" ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                    <td width="15%">End Date:
                    </td>
                    <td width="33%">
                        <asp:TextBox ID="txtToDate" runat="server" Width="80%" class="datepicker" onkeypress="return false;" ValidationGroup="1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" runat="server" Text="*" ForeColor="Red" ControlToValidate="txtToDate" ValidationGroup="1" ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
       
            </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search" ValidationGroup="1" OnClick="btnSearch_Click"  />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Gross Premium Search--%>
</asp:Content>

