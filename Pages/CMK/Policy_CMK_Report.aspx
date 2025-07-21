<%@ Page Title="CMK Policy | Report => Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="Policy_CMK_Report.aspx.cs" Inherits="Pages_CMK_Policy_CMK_Report" %>

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
            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      
           
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

          function ShowSearchForm() {
              $("#Main_txtFrom_date").val("");
              $("#Main_txtTo_date").val("");

              $("#myModalReportPolicy").modal("show");
          }

          $(document).ready(function () {
              $("[id*=RdbSearchBy] input").on("click", function () {
                  var selectedValue = $(this).val();
                  if (selectedValue == "1" || selectedValue == "3") {
                      $('#<%=chkInforceInMonth.ClientID %>').prop("checked", false)
                      $('#<%=chkInforceInMonth.ClientID %>').prop("disabled", true)
                  } else {
                      
                      var status_value = $('#<%=ddlPolicyStatus.ClientID %> input[type=radio]:checked').val();
                      if (status_value == "0" || status_value == "3")
                      {
                          $('#<%=chkInforceInMonth.ClientID %>').prop("checked", false)
                          $('#<%=chkInforceInMonth.ClientID %>').prop("disabled", true)
                      }
                      else
                      {
                          $('#<%=chkInforceInMonth.ClientID %>').prop("disabled", false)
                      }
                  }
              });

              $("[id*=ddlPolicyStatus] input").on("click", function () {
                  var selectedValue = $(this).val();
                  if (selectedValue == "0" || selectedValue == "3") {
                      $('#<%=chkInforceInMonth.ClientID %>').prop("checked", false)
                      $('#<%=chkInforceInMonth.ClientID %>').prop("disabled", true)
                  } else {
                      var status_value = $('#<%=RdbSearchBy.ClientID %> input[type=radio]:checked').val();
                      if (status_value == "1" || status_value == "3") {
                          $('#<%=chkInforceInMonth.ClientID %>').prop("checked", false)
                          $('#<%=chkInforceInMonth.ClientID %>').prop("disabled", true)
                      }
                      else {
                          $('#<%=chkInforceInMonth.ClientID %>').prop("disabled", false)
                      }
                  }
              });

          });

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
  <%--  <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>--%>

		<%--here is the block code--%>
        <!-- Modal Report Policy Form -->           
            <div style="height:30px"></div>        

            <div id="myModalReportPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy Report</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SDate" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Dates</a></li>
                        <li><a href="#SCertificateNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Certificate Number</a></li>                       
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
                                        <td><b>Search by</b>:
                                            <asp:Panel ID="ddlSearchBy" runat="server"  Height="90px" Width="150px">
                                                <asp:RadioButtonList ID="RdbSearchBy" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" Width="100%">
                                                    <asp:ListItem Value="1" Selected="True">&nbsp;&nbsp;Effective Date</asp:ListItem>
                                                    <asp:ListItem Value="2">&nbsp;&nbsp;Report date</asp:ListItem>
                                                    <asp:ListItem Value="3">&nbsp;&nbsp;Last Report Date</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </asp:Panel>
                                        </td>
                                        <td style="padding-left: 10%;"><b>Policy Status</b>:
                                            <asp:Panel ID="ddlPolicyStatus" runat="server" Height="115px" Width="100px">
                                                <asp:RadioButtonList ID="rbtnlPolicyStatus" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" Width="100%">
                                                    <asp:ListItem Value="0" Selected="True">&nbsp;&nbsp; All</asp:ListItem>
                                                    <asp:ListItem Value="1" >&nbsp;&nbsp; Inforce</asp:ListItem>
                                                    <asp:ListItem Value="2" >&nbsp;&nbsp; Lapsed</asp:ListItem>
                                                    <asp:ListItem Value="3" >&nbsp;&nbsp; Terminated</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </asp:Panel>
                                        </td>
                                        <td style="padding-left: 10%;"><b>Premium In Month</b>: 
                                            <asp:Panel ID="ddlMonthlyPrem" runat="server" Height="90px" Width="150px">
                                                <asp:CheckBox ID="chkInforceInMonth"  runat="server" /> &nbsp; In Month
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </div>

                        <div class="tab-pane" style="height: 70px;" id="SCertificateNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 25%;"><b>Certificate Number</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCertificateNumberSearch" placeholder="GR-CR-003-XXXXX" Width="90%" runat="server"></asp:TextBox>

                                    </td>
                                   
                                </tr>
                                 
                            </table>

                        </div>

                        <div class="tab-pane" style="height: 70px;" id="SCustomerName">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 25%;"><b>Last Name</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtLastnameSearch" Width="90%" runat="server"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                                 <tr>
                                    <td style="width: 25%;"><b>First Name</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstnameSearch" Width="90%" runat="server"></asp:TextBox>
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
            <!--End Modal Search Policy-->

            <asp:HiddenField ID="hdfuserid" runat="server" />
            <asp:HiddenField ID="hdfusername" runat="server" />
    
            <div id="dvReport" align="center" width="98%">        
                    <div class="panel panel-default" id="POLICY_REPORT" runat="server">
                <div class="panel-heading">
                    <h3 class="panel-title"> <asp:Label ID="Report_Type" class="panel-title" runat="server"></asp:Label> -
                    <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label></h3>
                </div>
                <div class="panel-body">
                    
                    <b>
                        <asp:Label ID="lblfrom" Visible="true"  runat="server" Text="Label" style="color: red;"></asp:Label> 
                        <asp:Label ID="lblto" Visible="true" runat="server" Text="Label" style="color: red;" ></asp:Label>
                    </b>

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

                            <asp:TemplateField HeaderText="Branch" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Customer ID" >
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("Customer_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CMK Customer ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblCMKCustomerID" runat="server" Text='<%#Eval("CMK_Customer_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Loan ID" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("Loan_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Certificate No" Visible="true" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("Certificate_No") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("Last_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="First Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("First_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Date Of Birth" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Convert.ToDateTime(Eval("Birth_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Age">
                                <ItemTemplate>
                                    <asp:Label ID="lblAge" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Opened Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpenedDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Opened_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Duration" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("LoanDuration") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Loan_Amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Outstanding Balance">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutstandingBalance" runat="server" Text='<%# Eval("Outstanding_Balance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date Of Entry" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfEntry" runat="server" Text='<%# Convert.ToDateTime(Eval("Date_Of_Entry")).ToString("dd-MM-yyyy")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Year">
                                <ItemTemplate>
                                    <asp:Label ID="lbCoverYear" runat="server" Text='<%# Eval("Covered_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Effective Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Effective_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Policy Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("Policy_Status") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Assured Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssuredAmount" runat="server" Text='<%# Eval("Assured_Amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Currency">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrancy" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Group" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("Pay_Mode_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Monthly Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("Monthly_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Premium After Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("Premium_After_Discount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Extra Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# Eval("Extra_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Eval("Total_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Paid Off Date" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaid" runat="server" Text='<%# Convert.ToDateTime(Eval("Paid_Off_In_Month")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Paid_Off_In_Month")).ToString("MM-yyyy") : " - "%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Report Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblReportDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Report_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Terminated Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminateDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Terminate_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remark" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Created_Note") %>'></asp:Label>
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
    
                    

	  <%--  </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
        </Triggers>

    </asp:UpdatePanel>	--%>

</asp:Content>


