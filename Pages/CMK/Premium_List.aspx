<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/Content.master" CodeFile="Premium_List.aspx.cs" Inherits="Pages_CMK_Premium_List" %>


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
                <input type="button"  onclick="ExportExcel();" style="background: url('../../App_Themes/functions/download_excel.png') 
                                 no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>  
      
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalRenewalPremium" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
           
        </li>
  </ul>

    <style>
        .text {
            mso-number-format:\@;
        }
    </style>

      <script type="text/javascript">

          //Section date picker //  $(document).ready(function ()) means that everything in this function will start auto when Loading page

          $(document).ready(function () {

              $('.datepicker').datepicker();

          });

          //Function Export report to excel
          function ExportExcel() {

              $("#dvReport").btechco_excelexport({
                  containerid: "dvReport"
                 , datatype: $datatype.Table
              });
          }

          function alertMesg(message)
          {
              alert(message);
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
            <div style="height:30px"></div>        

            <div id="myModalRenewalPremium"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalReportHeader" aria-hidden="true">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h3 class="panel-title">Renewal Premium Search</h3>
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
                        <asp:Button ID="btnOk" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="1" OnClick="btnSearch_Click"  />

                        <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                    </div>
                </div>

            <asp:HiddenField ID="hdfuserid" runat="server" />
            <asp:HiddenField ID="hdfusername" runat="server" />
    
            <div id="dvReport" align="center" width="98%">        
                    <div class="panel panel-default" id="POLICY_REPORT" runat="server">
                <div class="panel-heading">
                    <h3 class="panel-title"> <asp:Label ID="Report_Type" class="panel-title" runat="server"></asp:Label> Renewal Premium
                    <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label></h3>
                </div>
                <div class="panel-body">
                    
                    <asp:GridView ID="gv_report" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9" AllowPaging="True" PageSize="20"   OnPageIndexChanging="gv_report_PageIndexChanging">
                        <SelectedRowStyle BackColor="#EEFFAA" />

                        <Columns>
                            <asp:TemplateField HeaderText="No#">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Policy Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolNumber" runat="server" Text='<%#Eval("Group_Code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product ID" >
                                <ItemTemplate>
                                    <asp:Label ID="lblProductID" runat="server" Text='<%#Eval("Product_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%#Eval("Product_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Effective Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Effective_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sum Insure" Visible="true" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lblSumInsure" runat="server" Text='<%#Eval("Sum_Insure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pay Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayYear" runat="server" Text='<%# Eval("Pay_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Pay Lot" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayLot" runat="server" Text='<%# Eval("Pay_Lot") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblMode" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Number Of Policy">
                                <ItemTemplate>
                                    <asp:Label ID="lblNumberOfPolicy" runat="server" Text='<%# Eval("Number_Of_Policy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Report Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReportDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Report_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>

                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                        <PagerSettings PageButtonCount="15" />

                        <RowStyle BackColor="#CCCCCC"></RowStyle>

                    </asp:GridView>
                </div>
            </div>
             </div>  
            <!-- End of Append Data to Div --> 

</asp:Content>