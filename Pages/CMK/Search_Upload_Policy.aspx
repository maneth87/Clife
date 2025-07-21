<%@ Page Title="CMK Policy | Report => Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="Search_Upload_Policy.aspx.cs" Inherits="Pages_CMK_Search_Upload_Policy" %>

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

        <div style="display:none" runat="server">
            <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click"/>
        </div>

       <li>
         
           <input type="button"  onclick="PrintReport();" style="background: url('../../App_Themes/functions/print.png') 
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
          function ExportExcel() {
              $('#<%=export_excel.ClientID%>').click();
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
    <br />
  <%--  <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>--%>

		<%--here is the block code--%>
        <!-- Modal Report Policy Form -->           
            <div style="height:30px"></div>        
            

            <div class="panel panel-default">

            <div class="panel-heading">
                <h3 class="panel-title">Upload Search Report</h3>
            </div>
            <div class="panel-body">

                <table>
                    <tr>
                        <td>File Upload To Search:</td>
                        <td>
                            <asp:FileUpload ID="FileUploadSearchPolicy" runat="server" Width="91%" />
                        </td>
                        <td>
                            <a href="Templates/SEARCH_UPLOAD_TEMPLATE.xlsx" style="color: blue;"><u>Download Template File</u> </a>
                        </td>
                        <td rowspan="2">
                            <ul class="toolbar">

                                <li>
                                    <!-- Button Search-->
                                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" ValidationGroup="1" OnClick="btnSearch_Click"/>
                                </li>

                            </ul>
                        </td>
                    </tr>

                </table>
            </div>
        </div>

            <div style='text-align:center;'>
                <h1 style="text-align:center">CMK Search Report List</h1> 
            </div>    
                <hr style="color: red" />
                <b>
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label>
                </b>
                <br />

            <div class="panel-body">
                    <asp:GridView ID="gvSearchResult" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9" AllowPaging="True" PageSize="20"   OnPageIndexChanging="gvSearchResult_PageIndexChanging">
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

                            <asp:TemplateField HeaderText="Customer ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("CMK_Customer_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Certificate No" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("Certificate_No") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Loan ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("Loan_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Full Name" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("Last_Name") + " " + Eval("First_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Last Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("Last_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="First Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("First_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Gender" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Birth Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Convert.ToDateTime(Eval("Birth_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("Loan_Type") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Opened Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpenedDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Opened_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Entry Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfEntry" runat="server" Text='<%# Convert.ToDateTime(Eval("Date_Of_Entry")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Duration" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("Loan_Duration") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cover Year">
                                <ItemTemplate>
                                    <asp:Label ID="lbCoverYear" runat="server" Text='<%# Eval("Covered_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Helper.GetCurrancy(Eval("Currancy").ToString()) == 0 ? Eval("Loan_Amount") : Helper.GetCurrancy(Eval("Currancy").ToString()) == 1 ? Eval("Loan_Amount_Riel") : 0.0 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Out Balance">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutstandingBalance" runat="server" Text='<%# Helper.GetCurrancy(Eval("Currancy").ToString()) == 0 ? Eval("Outstanding_Balance") : Helper.GetCurrancy(Eval("Currancy").ToString()) == 1 ? Eval("Outstanding_Balance_Riel") : 0.0 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Assure Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssuredAmount" runat="server" Text='<%# Helper.GetCurrancy(Eval("Currancy").ToString()) == 0 ? Eval("Assured_Amount") : Helper.GetCurrancy(Eval("Currancy").ToString()) == 1 ? Eval("Assured_Amount_Riel") : 0.0 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Currancy">
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

                            <asp:TemplateField HeaderText="Age">
                                <ItemTemplate>
                                    <asp:Label ID="lblAge" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pay Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Policy Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("Policy_Status") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Monthly Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("Monthly_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Prem After Disc">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("Premium_After_Discount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Extra Premium" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# Eval("Extra_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Premium" >
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

                            <asp:TemplateField HeaderText="Report Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReportDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Report_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Paid Off Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaid" runat="server" Text='<%# Convert.ToDateTime(Eval("Paid_Off_In_Month")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Paid_Off_In_Month")).ToString("MM-yyyy") : " - " %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Terminate Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminateDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Terminate_Date")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Terminate_Date")).ToString("dd-MM-yyyy") : " - " %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remarks" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Created_Noted") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                        </Columns>

                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                        <PagerSettings PageButtonCount="15" />

                        <RowStyle BackColor="#CCCCCC"></RowStyle>
                    </asp:GridView>
                </div>

            <asp:HiddenField ID="hdfuserid" runat="server" />
            <asp:HiddenField ID="hdfusername" runat="server" />         

	  <%--  </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
        </Triggers>

    </asp:UpdatePanel>	--%>

     <asp:SqlDataSource ID="SqlDataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select DISTINCT Ct_Channel_Item.Channel_Item_ID, Ct_Channel_Item.Channel_Name from Ct_Channel_Item 
    INNER JOIN Ct_Channel_Location on Ct_Channel_Item.Channel_Item_ID = Ct_Channel_Location.Channel_Item_ID
    INNER JOIN Ct_External_User on Ct_External_User.Channel_Location_id = Ct_Channel_Location.Channel_Location_ID
    ORDER BY Ct_Channel_Item.Channel_Name ASC"></asp:SqlDataSource>
</asp:Content>


