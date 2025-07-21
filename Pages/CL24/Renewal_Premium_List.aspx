<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/Content.master" CodeFile="Renewal_Premium_List.aspx.cs" Inherits="Pages_CL24_Renewal_Premium_List" %>


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
        <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click" style="display: none;"/>
      
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

              $("#<%=export_excel.ClientID%>").click();
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
    <h1>Policy Renewal Premium List</h1>
            <div style="height:30px"></div>        

            <div id="myModalRenewalPremium" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalRenewalPremiumHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Renewal Premium List</h3>
                </div>

                <div class="modal-body">
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SPolicyYear" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Policy Year</a></li>                       
                        <li><a href="#SPolicy_Number" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Policy Number</a></li>                       
                    </ul>
                    <div class="tab-content" style="height: 50px; overflow: hidden;">
                        <div class="tab-pane active" style="height: 70px;" id="SPolicyYear">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 20%;"><b>Policy Year</b>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPolicyYearSearch" runat="server" AutoPostBack="false" CssClass="ddl"></asp:DropDownList>
                                    </td>
                                   
                                </tr>
                                 
                            </table>

                        </div>

                        <div class="tab-pane" style="height: 70px;" id="SPolicy_Number">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 20%;"><b>Policy Number</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumberSearch" placeholder="Policy Number . . ." Width="60%" runat="server"></asp:TextBox>
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
                                    <asp:Label ID="lblPolNumber" runat="server" Text='<%#Eval("Policy_Number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Insured Name" >
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuredName" runat="server" Text='<%#Eval("Insured_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Insurance Plan">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%#Eval("Product_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Start Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Start_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="End Date" Visible="true" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Convert.ToDateTime(Eval("End_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Sum Insure">
                                <ItemTemplate>
                                    <asp:Label ID="lblSumInsure" runat="server" Text='<%# Eval("Sum_Insure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("Payment_Mode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Total Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Eval("Total_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Effective Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Effective_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Issue Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Issue_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Renewal Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyYear" runat="server" Text='<%# Eval("Policy_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Last Due Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDueDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Due_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Due Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblNextDue" runat="server" Text='<%# Convert.ToDateTime(Eval("Next_Due")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Report Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReportDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Report_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("Created_By") %>'></asp:Label>
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