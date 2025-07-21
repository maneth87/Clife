<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_complete_chart.aspx.cs" Inherits="Pages_Chart_policy_complete_chart" %>
<%-- Get Reference from source pages to access public property --%>
<%@ PreviousPageType VirtualPath="~/Pages/Reports/report_policy_complete.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    
    <%-- Google Chart --%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <%-- Print --%>
    <script src="../../Scripts/jquery.print.js"></script>

    <ul class="toolbar" style="text-align:left">
       <li>
           <asp:ImageButton ID="ImgPrint"  runat="server" ImageUrl="~/App_Themes/functions/print.png"  OnClientClick="PrintChart();"/>
       </li>
    </ul>

    <script type="text/javascript">
           google.load("visualization", "1", { packages: ["corechart"] });
           google.setOnLoadCallback(drawChart);
           function drawChart() {

               var check_form_load_or_search = $("#Main_hdfCheckFormLoadOrSearch").val();
               var order_by = $("#Main_hdfOrderBy").val();
               var policy_status = $("#Main_hdfPolicyStatus").val();
               var from_date = $("#Main_hdfFromDate").val();
               var to_date = $("#Main_hdfToDate").val();
               var product = $("#Main_hdfProduct").val();              
               var policy_number = $("#Main_hdfPolicyNumber").val();
               var chart_data = $("#Main_hdfChartData").val();
               var chart_type = $("#Main_hdfChartType").val();                            
                          
               $.ajax({
                   type: "POST",
                   url: "../../ChartWebService.asmx/GetPolicyChartDataTotal",
                   data: "{check_form_load_or_search:'" + check_form_load_or_search + "',policy_status:'" + policy_status + "',order_by:'" + order_by + "',from_date:'" + from_date + "',to_date:'" + to_date + "',product:'" + product + "',policy_number:'" + policy_number + "',chart_data:'" + chart_data + "'}",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (data) {

                       $("#Main_hdfTotalAmount").val(data.d);

                       var my_title = "";
                       var vertical_title = "";

                       if (chart_type == "1") {
                           var my_title = "Pie Chart: ";
                       }

                       if (chart_type == "2") {
                           my_title = "Combo Chart: ";
                       }

                       if (chart_type == "3") {
                           my_title = "Line Chart: ";
                       }


                       switch (chart_data) {
                           case "0"://Sum Insure
                               vertical_title = "Sum Insure";
                               my_title += "Total Sum Insure = ";
                               break;
                           case "1"://Premium                   
                               vertical_title = "Premium";
                               my_title += "Total Premium = ";
                               break;
                           case "2"://Policy
                               vertical_title = "Policy";
                               my_title += "Total Policy = ";
                               break;

                       }


                       my_title += " " + commaSeparateNumber($("#Main_hdfTotalAmount").val());



                       //Draw combo or line chart
                       if (chart_type == "2" || chart_type == "3") {


                           $.ajax({
                               type: "POST",
                               url: "../../ChartWebService.asmx/GetPolicyComboChartData",
                               data: "{check_form_load_or_search:'" + check_form_load_or_search + "',policy_status:'" + policy_status + "',order_by:'" + order_by + "',from_date:'" + from_date + "',to_date:'" + to_date + "',product:'" + product + "',policy_number:'" + policy_number + "',chart_data:'" + chart_data + "'}",
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               success: function (data) {

                                   var my_data = google.visualization.arrayToDataTable(data.d);

                                   //Combo Chart Option
                                   var combo_options = {
                                       title: my_title,
                                       vAxis: { title: vertical_title },
                                       hAxis: { title: "Month" },
                                       seriesType: "bars",
                                       series: { 5: { type: "bars" } },
                                       gridlines: { color: '#333', count: 4 }

                                   };

                                   //Line Chart Option
                                   var line_options = {
                                       title: my_title,
                                       vAxis: { title: vertical_title },
                                       hAxis: { title: "Month" }

                                   };


                                   //Combo Chart
                                   if (chart_type == "2") {
                                       var chart = new google.visualization.ComboChart($("#chart")[0]);

                                       chart.draw(my_data, combo_options);
                                   }

                                   //Line Chart
                                   if (chart_type == "3") {
                                       var chart = new google.visualization.LineChart($("#chart")[0]);
                                       chart.draw(my_data, line_options);
                                   }

                               }

                           });


                       }

                       //Draw Pie 
                       if (chart_type == "1") {


                           $.ajax({
                               type: "POST",
                               url: "../../ChartWebService.asmx/GetPolicyChartData",
                               data: "{check_form_load_or_search:'" + check_form_load_or_search + "',policy_status:'" + policy_status + "',order_by:'" + order_by + "',from_date:'" + from_date + "',to_date:'" + to_date + "',product:'" + product + "',policy_number:'" + policy_number + "',chart_data:'" + chart_data + "'}",
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               success: function (data) {


                                   var options = {
                                       title: my_title,
                                       is3D: true
                                   };

                                   var my_data = google.visualization.arrayToDataTable(data.d);

                                   //Pie Chart
                                   if (chart_type == "1") {
                                       var chart = new google.visualization.PieChart($("#chart")[0]);
                                       chart.draw(my_data, options);
                                   }


                               }

                           });


                       }


                   }
               });                                         
            
           }

           //Print Chart
           function PrintChart() {
               $(".PrintChart").print();
           }

           //Insert Comma
           function commaSeparateNumber(val) {
               while (/(\d+)(\d{3})/.test(val.toString())) {
                   val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
               }
               return val;
           }

        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <br />
    <br />
    <br />
    <%-- Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Policy Chart</h3>
        </div>
        <div class="panel-body">
            <div id="chart" class="PrintChart" style="width: 100%; height: 500px; text-align:center; border: 1px solid lightgray;">
            </div>
        </div>
    </div>
  
    <%-- Hidden Fields --%>
    <asp:HiddenField ID="hdfCheckFormLoadOrSearch" runat="server" />
    <asp:HiddenField ID="hdfOrderBy" runat="server" />
    <asp:HiddenField ID="hdfPolicyStatus" runat="server" />
    <asp:HiddenField ID="hdfChartType" runat="server" />
    <asp:HiddenField ID="hdfChartData" runat="server" />
    <asp:HiddenField ID="hdfFromDate" runat="server" />
    <asp:HiddenField ID="hdfToDate" runat="server" />
    <asp:HiddenField ID="hdfProduct" runat="server" />   
    <asp:HiddenField ID="hdfPolicyNumber" runat="server" />
    <asp:HiddenField ID="hdfTotalAmount" runat="server" />

</asp:Content>

