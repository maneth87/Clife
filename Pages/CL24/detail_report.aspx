<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="detail_report.aspx.cs" Inherits="Pages_CL24_detail_report" %>


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

              $("#myModalDetailReport").modal("show");
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
                    <h3 class="panel-title">CL24 Detail Report</h3>                                        
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

            <div id="myModalDetailReport" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDetailReportHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Detail Report</h3>
                </div>

                <div class="modal-body">
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SDate" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Dates</a></li>
                        <li><a href="#SPolicyNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Policy Number</a></li>                       
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Customer Name</a></li>                       
                    </ul>
                    <div class="tab-content" style="height: 200px; overflow: hidden;">
                        <div class="tab-pane active" id="SDate">
                            
                            <table>
                                <tr>
                                    <td style="width: 15%;"><b>From Date</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtFrom_date" runat="server" CssClass="datepicker" placeholder="DD-MM-YYYY" Width="90%" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%;"><b>To Date</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTo_date" runat="server" CssClass="datepicker" placeholder="DD-MM-YYYY" Width="90%" TabIndex="4"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                            <br />
                            <div style="float: right; width: 90%; padding-left: 50px; margin-right: 0%; border: solid 1px; border-color: lightgray; border-radius: 5px;">
                                <table>
                                    <tr>
                                        <td>
                                            <b>Search by</b>:
                                                    <asp:Panel ID="ddlSearchBy" runat="server" Height="90px" Width="150px">
                                                        <asp:RadioButtonList ID="rbtnlData" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" Width="100%">
                                                            <asp:ListItem Value="1">&nbsp;Effective Date</asp:ListItem>
                                                            <asp:ListItem Value="2">&nbsp;Transaction Date</asp:ListItem>
                                                            <asp:ListItem Value="3">&nbsp;Issue Date</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </asp:Panel>
                                        </td>

                                        <td style="padding-left: 10%;"><b>Status</b>:
                                                    <asp:Panel ID="ddlPolicyStatus" runat="server" Height="93px" Width="100px">
                                                        <asp:RadioButtonList ID="rbtnlPolicyStatus" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" Width="100%">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp;&nbsp;All</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp;&nbsp;IF</asp:ListItem>
                                                            <asp:ListItem Value="2">&nbsp;&nbsp;TER</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </asp:Panel>
                                        </td>
                                        <td style="padding-left: 10%;"><b>In Month</b>: 
                                                    <asp:Panel ID="ddlMonthlyPrem" runat="server" Height="90px" Width="150px">
                                                        <asp:CheckBox ID="chkInMonth" runat="server" />
                                                        &nbsp; In Month </br>
                                                        <asp:CheckBox runat="server" ID="ckb_split" />
                                                        &nbsp; Split Policy
                                                    </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </div>

                        <div class="tab-pane" style="height: 70px;" id="SPolicyNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 20%;"><b>Policy Number</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumberSearch" placeholder="XXXXX" Width="50%" runat="server"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                                 
                            </table>

                        </div>

                        <div class="tab-pane" style="height: 70px;" id="SCustomerName">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 20%;"><b>Last Name</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtLastnameSearch" placeholder="Search . . ." Width="60%" runat="server"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                                 <tr>
                                    <td style="width: 20%;"><b>First Name</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstnameSearch" placeholder="Search . . ." Width="60%" runat="server"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                            </table>

                        </div>
                    </div>

                    
                    

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

