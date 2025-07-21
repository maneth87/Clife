<%@ Page Title="Micro Policy | Reports => Micro Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_micro_report.aspx.cs" Inherits="Pages_Report_policy_micro_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>

    
    <ul class="toolbar" style="text-align:left">
       
        <li>          
            <input type="button"  onclick="ExportExcelActiveMember();" style="background: url('../../App_Themes/functions/download_excel.png') 
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
            mso-number-format: \@;
        }
    </style>

      <script type="text/javascript">

          //Section date picker //  $(document).ready(function ()) means that everything in this function will start auto when Loading page

          $(document).ready(function () {

              $('.datepicker').datepicker();

          });

          function SelectSingleCheckBox(ckb, Policy_Status_Type_ID) {

              if (ckb.checked) {

                  $('#Main_hdfPolicy_Status_ID').val(Policy_Status_Type_ID);
              }
              else {
                  $('#Main_hdfPolicy_Status_ID').val("");
                  ckb.checked = false;
                  ClearGrid();
              }
          }

          function ClearGrid() {

              $('#Main_GvoPolicyStatus').val("");
          }


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
          function ExportExcelActiveMember() {

              $("#dvReport").btechco_excelexport({
                  containerid: "dvReport"
                 , datatype: $datatype.Table
              });
          }

          function ShowSearchForm() {
              $("#Main_txtFrom_date").val("");
              $("#Main_txtTo_date").val("");

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
  
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
		<%--here is the block code--%>
        <!-- Modal Report Policy Form -->           
            

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Micro Policy Report</h3>                                        
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

            <div id="myModalReportPolicy" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalReportPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Policy Report</h3>
                </div>

                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                         <tr>
                             <td>Barcode</td>
                            <td>
                                <asp:HiddenField ID="hdfBarcode" runat="server"  Value='<%# Bind("txtBarcode.text") %>'/>
                               <asp:TextBox ID="txtBarcode"  runat="server"  width="86%" TabIndex="1" ></asp:TextBox> 
                                <%--onChange="intOnly(this);" onKeyUp="intOnly(this);" onKeyPress="intOnly(this);"--%>
                            </td>
                            <td>Policy Number</td>
                            <td>
                                <asp:HiddenField ID="hdfPolicyNum" runat="server"  Value='<%# Bind("txtPolicyNum.text") %>'/>
                                <asp:TextBox ID="txtPolicyNum" runat="server" Width="86%" TabIndex="2"></asp:TextBox>
                                <%--onChange="intOnly(this);" onKeyUp="intOnly(this);" onKeyPress="intOnly(this);"--%>
                            </td>
                        </tr>
                        <tr>
                            <td>Agent Code:</td>
                            <td>
                                 <asp:TextBox ID="txtAgentCode" runat="server" Width="86%" TabIndex="3"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>From Date:</td>
                            <td>
                                <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker" width="86%" TabIndex="4" ></asp:TextBox>
                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorFromDate" ControlToValidate="txtFrom_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>--%>
                            </td>
                            <td>To Date:</td>
                            <td>
                                <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker" width="86%" TabIndex="5" ></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" ControlToValidate="txtTo_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td> Product </td>
                            <td>
                                <asp:HiddenField ID="hdfProduct" runat="server" />
                                 <asp:DropDownList ID="ddlProduct" width="95%" height="25px" runat="server" TabIndex="6" >
                                     <asp:ListItem Value="-1">.</asp:ListItem>
                                     <asp:ListItem Value="T1011">Term One</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Payment Mode</td>
                            <td>
                                 <asp:HiddenField ID="hdfPaymentMode" runat="server"  Value='<%# Bind("ddlPaymentMod.SelectedValue") %>'/>
                                 <asp:DropDownList ID="ddlPaymentMod" width="95%" height="25px" runat="server" TabIndex="7" AppendDataBoundItems="True" DataSourceID="SqlDataSourcePaymentMode" DataTextField="Mode" DataValueField="Pay_Mode_ID">
                                 <asp:ListItem Value="-1">.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>View Type:</td>
                            <td>
                                  <asp:DropDownList ID="ddlViewType" width="95%" height="25px" runat="server" TabIndex="8" >
                                     <asp:ListItem Value="0">My Policies</asp:ListItem>
                                     <asp:ListItem Value="1">Group Polices</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                       
                    </table>
                    <br />
                    <div style="float:left; padding-left:25px; width:35%; margin-left:10%; box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:199px;">
                            <asp:GridView  ID="GvoPolicyStatus" Width="39%" Height="5px" runat="server" DataSourceID="SqlDataSourcePolicyStatusType"  AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center" BorderColor="White" BorderWidth="0" BorderStyle="NotSet" EditRowStyle-BorderColor="White" EmptyDataRowStyle-BorderColor="White" CellPadding="2" HeaderStyle-BackColor="White" HeaderStyle-BorderColor="White" RowStyle-BorderColor="White" CellSpacing="200" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" PagerStyle-BorderColor="White" EditRowStyle-BorderStyle="NotSet" EmptyDataRowStyle-BorderStyle="NotSet" FooterStyle-BorderStyle="None" HeaderStyle-BorderStyle="NotSet" PagerStyle-BorderStyle="NotSet" RowStyle-BorderStyle="NotSet">
                                <Columns>
                                    <asp:TemplateField>
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckb1"  runat="server" TextAlign="Right" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Status_Type_ID" ) + "\");" %>' /> 
                                        <asp:HiddenField ID="hdfPolicy_Status_ID" runat="server" Value='<%# Bind("Policy_Status_Type_ID") %>' />
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Policy_Status_Type_ID"  HeaderText="" Visible="false" />
                                    <asp:BoundField DataField="Detail" HeaderText="Status Code" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" />
                                <RowStyle HorizontalAlign="Center" />
                           </asp:GridView>
                        
                    </div>
                    <div style="float:right; width:35%; padding-left:20px; margin-right:10%; box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:199px;">
                            <table>
                                <tr>
                                    <td style="padding:20px 20px 20px 20px;">Order by:
                                        <asp:Panel ID="Panel1" runat="server"  CssClass="panel_border" Height="90px" Width="150px">
                                            <asp:RadioButtonList ID="RdbOrderBy" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" width="100%"  >
                                                <asp:ListItem Value="1" Selected="True"><label>&nbsp;&nbsp;Policy number</label></asp:ListItem>
                                                <asp:ListItem Value="2">&nbsp;&nbsp;Issue date</asp:ListItem>
                                                <asp:ListItem Value="3">&nbsp;&nbsp;Effective date</asp:ListItem>                                               
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
            
                   <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
                   <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>
                    

           <!--- Section Sqldatasource--->
                <asp:SqlDataSource ID="SqlDataSourcePolicyStatusType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_Status_Type_ID,Detail from Ct_Policy_Status_Type order by Detail asc "></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Pay_Mode_ID,Mode from Ct_Payment_Mode"></asp:SqlDataSource>
               
                <asp:SqlDataSource ID="SqlDataSourcePolicyNumber" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_Micro_ID,Policy_Number from Ct_Policy_Micro order by Policy_Number"></asp:SqlDataSource>
           <!-- End Section  -->
                </div>

                <asp:HiddenField ID="hdfuserid" runat="server" />
                <asp:HiddenField ID="hdfusername" runat="server" />
                <asp:HiddenField ID="hdfSaleAgentID" runat="server" />
            </div>            


            
	    </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>

