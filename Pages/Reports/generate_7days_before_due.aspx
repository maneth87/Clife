<%@ Page Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="generate_7days_before_due.aspx.cs" Inherits="Pages_Reports_generate_7days_before_due" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>

    
    <ul class="toolbar" style="text-align: left">

        <%--<li>
            <input type="button" style="border-style: none; border-color: inherit; border-width: medium; background: url('../../App_Themes/functions/download_excel.png') no-repeat; height: 40px; width: 90px;" onclick="open_report('excel');" />
            <asp:Button ID="btnExcel" runat="server" Style="display: none;" OnClick="btnExcel_Click" />
        </li>--%>

        <li>          
            <input type="button"  onclick="ExportExcelActiveMember();" style="background: url('../../App_Themes/functions/download_excel.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>
        <li>
            <input type="button" onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
    </ul>


    <style>
        .text {
            mso-number-format: \@;
        }
    </style>

      <script type="text/javascript">

          $(document).ready(function () {

              $('.datepicker').datepicker();

          });


          function ClearGrid() {

              $('#Main_GvoPolicyStatus').val("");
          }

          //Function Export report to excel
          function ExportExcelActiveMember() {

              $("#dvReport").btechco_excelexport({
                  containerid: "dvReport"
                 , datatype: $datatype.Table
              });
          }


          function ShowSearchForm() {
              $("#Main_txtFrom_date").val("");
              $("#Main_txtTo_date").val("");

              $("#myModalDailyReport").modal("show");
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
  
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
        <!-- Modal Report Policy Form -->           
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">CL24 Daily Report</h3>                                        
                </div>
                
                <div class="panel-body">
                    <!-- Append data to Div -->
          

                    <!-- End of Append Data to Div -->
                   
                    <!-- Append data to Div -->

                    <div id="dvReport" align="center" width="98%">
                        <asp:Label ID="lblfrom" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblto" runat="server" ForeColor="Red"></asp:Label>

                        <!-- Append data to Div -->
                        <div id="AppendDivPolicy" runat="server" align="center">
                        </div>
                    </div>
                    <!-- End of Append Data to Div -->

                    <!-- Add more lable for date -->

            <div id="myModalDailyReport" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDailyReportHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Daily Report</h3>
                </div>

                <div class="modal-body">
                <table>
                    <tr>
                        <td><b>From Date:</b></td>
                        <td>
                            <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker" width="86%" TabIndex="5" placeholder="[DD\MM\YYYY]" ></asp:TextBox>
                        </td>   
                        <td><b>To Date:</b></td>
                        <td>
                            <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker" width="86%" TabIndex="5" placeholder="[DD\MM\YYYY]"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                            <td width="70px"></td>
                            <td >
                                <asp:RadioButtonList ID="rbtnlData" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" width="100%">
                                    <asp:ListItem Value="1">&nbsp;Effective Date</asp:ListItem>
                                    <asp:ListItem Value="2">&nbsp;Transaction Date</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>

                            <td width="70px">Split Policy</td>
                            <td>
                                   <asp:CheckBox runat="server" ID="ckb_split" />
                            </td>
                        </tr>
                                
                </table>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search" OnClick="btnSearch_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="ClearGrid();">Cancel</button>
                </div>
            </div>
            <!--End Modal Report Policy Form-->
                <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
                <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>

                <asp:HiddenField ID="hdfuserid" runat="server" />
                <asp:HiddenField ID="hdfusername" runat="server" />
            </div>            


            
	    </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>
