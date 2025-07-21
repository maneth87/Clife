<%@ Page Title="Clife | Reports => Lapsed Policy" Language="C#" AutoEventWireup="true"  MasterPageFile="~/Pages/Content.master"  ValidateRequest="false" CodeFile="report_policy_lapsed.aspx.cs" Inherits="Pages_Reports_report_policy_lapsed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
      <ul class="toolbar" style="text-align:left">
      <li>
           <input type="button" id="btnExport" onclick="Export_Excel();" style="background: url('../../App_Themes/functions/download_excel.png') 
			        no-repeat; border: none; height: 40px; width: 90px;" />	
       </li>
       <li>
            <div style="display: none;">
               <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click"  />
            </div>
            <input id="btnCalculateLapse" type="button"  runat="server" style="background: url('../../App_Themes/functions/calculate.png') no-repeat; border: none; height: 40px; width: 90px;" onclick="onclick_search()"  />
        </li>
       <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
       </li>
    </ul>


   <script type="text/javascript">

       var cus_last;
       var cus_first;
       var poli;
       var search_poli_num;
       var due_date_value;
       var customer_id;
       var pay_date_value;

       function Export_Excel() {

           $("#tblExport").btechco_excelexport({
               containerid: "tblExport"
              , datatype: $datatype.Table
           });
       }

       function Lapsed_Prem(policy_num, input_pay_date) {
           
           $.ajax({
               type: "POST",
               url: "../../PolicyWebService.asmx/Lapsed_Amount",
               data: "{Policy_Number:'" + policy_num + "', Current_Date:'" + input_pay_date + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (data) {
                   
                   if (data.d != "") {

                     $('#Main_txtPremLapsed').val('$' + data.d);

                     if ($('#Main_txtPremLapsed').val() == "$0")
                     {
                        
                         $('#tblExport').remove();
                     }
                   }
               },
               error: function (msg) {

                   alert(msg);
               }
           });
       }

       function GetPolicy_Info() {

           var policy_num = $('#Main_txtPoliNumberSearch').val();
           var first_name = $('#Main_txtLastName').val();
           var last_name = $('#Main_txtFirstName').val();

           if (policy_num != "" || first_name != "" || last_name != "") {
               $.ajax({
                   type: "POST",
                   url: "../../PolicyWebService.asmx/GetPolicy",
                   data: "{Policy_Number:'" + policy_num + "', First_Name:'" + first_name + "', Last_Name:'" + last_name + "'}",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (data) {

                       if (data.d != "") {
                           var get_data = data.d;
                           get_data = get_data.split(',');

                           $('#Main_txtPoliNumberSearch').val(get_data[0]);
                           $('#Main_txtLastName').val(get_data[1]);
                           $('#Main_txtFirstName').val(get_data[2]);
                       }
                   },
                   error: function (msg) {

                       alert(msg);
                   }
               });
           }
           else { alert("Please input key word search first"); }
       }

       function formatJSONDate(jsonDate) {
           var date = eval(jsonDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
           return dateFormat(date, "dd/mm/yyyy");
       }

       $(document).ready(function () {

           $('.datepicker').datepicker();

       });

       function intOnly(i) {
           if (i.value.length > 0) {
               i.value = i.value.replace(/[^\d]+/g, '');
           }
       }

       function Print_default() {
           $(".PrintPD").print();
       }

       function onclick_search() {

           var btnSearch = document.getElementById('<%= btnSearch.ClientID %>'); //dynamically click button

           $('#tblExport').remove();

           btnSearch.click();

           $('#PrintPD').show();

           var input_pay_date = $('#Main_txtFrom_date').val();

           var policy_num = $('#Main_txtPoliNumberSearch').val();

           Lapsed_Prem(policy_num, input_pay_date);
       }

       function Clear_Text()
       {
           $('#Main_txtDue_Date').val("");
           $('#Main_txtPoliNumber').val("");
           $('#Main_txtLastName').val("");
           $('#Main_txtFirstName').val("");
           $('#Main_txtCustomer_ID').val("");
           $('#Main_txtPay_Date').val("");
           $('#Main_txtPremLapsed').val("");
           $('#Main_txtFrom_date').val('');

           $('#Main_hdfDue_Date').val("");
           $('#Main_hdfPoli').val("");
           $('#Main_hdffirst').val("");
           $('#Main_hdflast').val("");
           $('#Main_hdfPay_Date').val("");
           $('#Main_hdfCustomer_ID').val("");
       }


           function Export_Excel() {

               $("#tblExport").btechco_excelexport({
                   containerid: "tblExport"
                  , datatype: $datatype.Table
               });
           }

           function formattedDate(date) {
               var d = new Date(date || Date.now()),
                   month = '' + (d.getMonth() + 1),
                   day = '' + d.getDate(),
                   year = d.getFullYear();

               if (month.length < 2) month = '0' + month;
               if (day.length < 2) day = '0' + day;

               return [day, month, year].join('/');
           }

           $(document).ready(function () {

               $('#Main_txtFrom_date').datepicker()
               .on('changeDate', function (pay_date) {

                   var input_pay_date = (pay_date.date);
                   input_pay_date = formattedDate(input_pay_date);

                   var policy_num = $('#Main_txtPoliNumberSearch').val();

                   // Check duration whether it is Lapsed or not

                   /// Due Date
                   var string_due = due_date_value
                   var arr = string_due.split('/');
                   var get_due_date = new Date(arr[2], arr[1], arr[0]);

                   /// Current Date
                   var string_current = input_pay_date
                   var arr_current = string_current.split('/');
                   var get_current_date = new Date(arr_current[2], arr_current[1], arr_current[0]);

                   if (get_due_date > get_current_date) {
                       alert('Current date' + '(' + input_pay_date + ')' + ' cannot be less than Due Date(' + due_date_value + ')');
                   }
                   else {
                      // Lapsed_Prem(policy_num, input_pay_date);
                   }
               });
           });

           function GetPreList_by_PoliID() {

               $('#dvPolicyList').val("");

               var Policy_Number_search = $('#Main_txtPoliNumberSearch').val();
               var Customer_ID_search = $('#Main_txtCustIDSearch').val();
               var Cus_Last = $('#Main_txtLastNameSearch').val();
               var Cus_First = $('#Main_txtFirstNameSearch').val();

               $.ajax({
                   type: "POST",
                   url: "../../PolicyWebService.asmx/SearchPolicy_Num",
                   data: "{Policy_Number:'" + Policy_Number_search + "', Customer_ID:'" + Customer_ID_search + "', Last_Name:'" + Cus_Last + "', First_Name:'" + Cus_First + "'}",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (data) {

                       if (data.d != "" || data.d != null) {
                           
                           $('#dvPolicyList').setTemplate($("#jTemplatePolicySearch").html());
                           $('#dvPolicyList').processTemplate(data);

                           $('#tblExport').remove();
                           $('#Main_txtPremLapsed').val("");
                           $('#Main_txtFrom_date').val("");

                          // Clear_Text();
                       }

                       // Next Due
                       //Calculate_Next_Due();
                   },
                   error: function (msg) {

                       alert(msg);
                   }
               });
           }


           function GetPolicy(last_name, first_name, poli_num, Due_Date, PayDate, Customer_ID_Value, index, total) {

               if ($('#Ch' + index).is(':checked')) {
                  
                   due_date_value = Due_Date;
                   search_poli_num = poli_num;
                   cus_first = first_name;
                   cus_last = last_name;
                   poli = poli_num;
                   pay_date_value = PayDate;
                   customer_id = Customer_ID_Value;

               }

               //Uncheck all other checkboxes
               for (var i = 0; i < total  ; i++) {
                   if (i != index) {
                       $('#Ch' + i).prop('checked', false);
                   }
               }
           }

           function FillSearchedData() {

               if (search_poli_num != "") {

                   $('#Main_txtDue_Date').val(due_date_value);
                   $('#Main_txtPoliNumber').val(poli);
                   $('#Main_txtLastName').val(cus_last);
                   $('#Main_txtFirstName').val(cus_first);
                   $('#Main_txtCustomer_ID').val(customer_id);
                   $('#Main_txtPay_Date').val(pay_date_value);

                   //
                   $('#Main_hdfDue_Date').val(due_date_value);
                   $('#Main_hdfPoli').val(poli);
                   $('#Main_hdflast').val(cus_last);
                   $('#Main_hdffirst').val(cus_first);
                   $('#Main_hdfCustomer_ID').val(customer_id);
                   $('#Main_hdfPay_Date').val(pay_date_value);
                   
                   
                  } else {
                   alert("Plese select a checkbox");
               }
           }


   </script>
  
     <script id="jTemplatePolicySearch" type="text/html">
        <table class="table table-bordered">
            <thead style="text-align:center">
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Pol.no</th>
                    <th style="border-width: thin; border-style: solid;">Last Name</th>
                    <th style="border-width: thin; border-style: solid;">First Name</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid;">Mode</th>
                    <th style="border-width: thin; border-style: solid;">Sum Insure</th>
                    <th style="border-width: thin; border-style: solid;">Premium</th>
                    <th style="border-width: thin; border-style: solid;">Due Date</th>
                    <th style="border-width: thin; border-style: solid;">Product</th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td style="text-align:center">{ $T.row$index + 1 }</td>
                        <td style="text-align:center">{ $T.row.Policy_Number }</td>
                        <td style="text-align:center">{ $T.row.Last_Name }</td>
                        <td style="text-align:center">{ $T.row.First_Name }</td>
                        <td style="text-align:center">{ $T.row.Gender }</td>
                        <td style="text-align:center">{ $T.row.Mode }</td>
                        <td style="text-align:right"> ${ $T.row.Sum_Insure }</td>
                        <td style="text-align:right"> ${ $T.row.Normal_Prem }</td>
                        <td style="text-align:right">{ formatJSONDate($T.row.Normal_Due_Date) }</td>
                        <td style="text-align:center">{ $T.row.En_Abbr }</td>
                   
                        <td>
                            <input id="Ch{ $T.row$index }" type="checkbox" onclick="GetPolicy('{$T.row.Last_Name}', '{$T.row.First_Name}', '{$T.row.Policy_Number}', '{formatJSONDate($T.row.Normal_Due_Date)}', '{formatJSONDate($T.row.Pay_Date)}', '{$T.row.Customer_ID}', '{$T.row$index}', '{$T.row$total}');" /></td>
                    </tr> 
                {#/for}
            </tbody>
        </table>
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
     <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>

    <script src="../../Scripts/ExportExcel/jquery.battatech.excelexport.js"></script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%-- here is the block code--%>
    
            <div id="myModalSearch" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearch" aria-hidden="true" style="width:700px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy Form</h3>
                </div>

             <div class="modal-body">
             <ul class="nav nav-tabs" id="myTabPolicySearch">
                    <li class="active"><a href="#SPoliNo" data-toggle="tab" style="text-decoration: none;">Search By Policy Num</a></li>
                    <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none;">Search By Customer Name</a></li>
                    <li><a href="#SCusID" data-toggle="tab" style="text-decoration: none;">Search By Customer ID</a></li>
                </ul>

                <div class="tab-content" style="height: 60px; overflow: hidden;">
                    <div class="tab-pane active" id="SPoliNo">
                        <table style="width: 98%">
                            <tr>
                                <td style="width: 13%; vertical-align: middle">Policy Num:</td>
                                <td style="width: 30%; vertical-align: bottom">
                                    <asp:TextBox ID="txtPoliNumberSearch" Width="90%" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 56%; vertical-align: top">
                                    <input type="button" class="btn" style="height: 27px;" onclick="GetPreList_by_PoliID();" value="Search" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </div>

                    <div class="tab-pane" id="SCustomerName">
                        <table style="width: 98%">
                            <tr>
                                <td style="width: 10%; vertical-align: middle">Last Name:</td>
                                <td style="width: 26%; vertical-align: bottom">
                                    <asp:TextBox ID="txtLastNameSearch" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 10%; vertical-align: middle">First Name:</td>
                                <td style="width: 26%; vertical-align: bottom">
                                    <asp:TextBox ID="txtFirstNameSearch" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 18%; vertical-align: top">
                                    <input type="button" class="btn" style="height: 27px;" onclick="GetPreList_by_PoliID();" value="Search" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </div>

                    <div class="tab-pane active" id="SCusID">
                        <table style="width: 98%">
                            <tr>
                                <td style="width: 13%; vertical-align: middle">Customer ID:</td>
                                <td style="width: 30%; vertical-align: bottom">
                                    <asp:TextBox ID="txtCustIDSearch" Width="90%" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 56%; vertical-align: top">
                                    <input type="button" class="btn" style="height: 27px;" onclick="GetPreList_by_PoliID();" value="Search" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </div>
                 </div>
                 <div id="dvPolicyList"></div>
            </div>

             <div class="modal-footer">
                <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillSearchedData();" data-dismiss="modal" value="Select" />

                <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>

            <br /><br /><br />

            <div class="panel panel-default" style="">
                <div class="panel-heading">
                    <h3 class="panel-title">Calculate Amount Lapsed</h3>
                </div>
                <div class="panel-body" >
                   <table  >
                     <%--  <tr>
                           <th colspan="4"><h1 style="text-align:center;">Calculate Amount Lapsed</h1></th>
                       </tr>--%>
                       <tr>
                            <td>Policy Num:</td>
                            <td>
                                <asp:TextBox ID="txtPoliNumber"  runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdfPoli" runat="server" />
                            </td>
                            <td>Last Name:</td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdflast" runat="server" />
                            </td>
                            <td>First Name:</td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdffirst" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Customer ID:</td>
                            <td>
                                <asp:TextBox ID="txtCustomer_ID"  runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdfCustomer_ID" runat="server" />
                            </td>
                            
                            <td>Due Date:</td>
                            <td><asp:TextBox ID="txtDue_Date"  runat="server" ReadOnly="true"></asp:TextBox></td>
                              <asp:HiddenField ID="hdfDue_Date" runat="server" />
                            <td>Last Pay Date:</td>
                            <td>
                                <asp:TextBox ID="txtPay_Date"  runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdfPay_Date" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Select Date:</td>
                            <td>
                                <asp:TextBox ID="txtFrom_date"  runat="server" TabIndex="1" ></asp:TextBox>
                            </td>

                             <td>Total Prem Lapsed:</td>
                            <td>
                                <asp:TextBox ID="txtPremLapsed"  runat="server" ForeColor="Red" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                   </table>

                   <br />
                    <div id="PrintPD" runat="server">
                    </div>
                  </div>
            </div>
            
            <asp:Button ID="btnOriginalPrem" runat="server" Text="Button" OnClick="btnOriginalPrem_Click" Visible="false"/>

	</ContentTemplate>

         <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnSearch" />--%>
         </Triggers>

    </asp:UpdatePanel>	
</asp:Content>