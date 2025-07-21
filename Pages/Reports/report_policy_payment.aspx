<%@ Page Title="Clife | Reports => Payment History" Language="C#" MasterPageFile="~/Pages/Content.master"  ValidateRequest="false" AutoEventWireup="true" CodeFile="report_policy_payment.aspx.cs" Inherits="Pages_Reports_report_policy_payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

   <ul class="toolbar" style="text-align:left">
       <li>
           <input type="button" id="btnExport" onclick="Export_Excel();" style="background: url('../../App_Themes/functions/download_excel.png') 
			        no-repeat; border: none; height: 40px; width: 90px;" />	
       </li>
       <li>
           <input type="button"  onclick="Print_default();" style="background: url('../../App_Themes/functions/print.png') 
			        no-repeat; border: none; height: 40px; width: 90px;" />			
       </li>
       <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
       </li>
  </ul>

   <script type="text/javascript">

       function formatJSONDate(jsonDate) {
           var date = eval(jsonDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
           return dateFormat(date, "dd/mm/yyyy");
       }

       //Section date picker //  $(document).ready(function ()) means that everything in this function will start auto when Loading page

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

           btnSearch.click();
       }

       /// Start when click on button btnExport
       //$(document).ready(function () {
       //    $("#Main_btnExport").click(function () {
               
       //        $("#tblExport").btechco_excelexport({
       //            containerid: "tblExport"
       //           , datatype: $datatype.Table
       //        });
       //    });
       //});


       function Export_Excel() {

           $("#tblExport").btechco_excelexport({
               containerid: "tblExport"
              , datatype: $datatype.Table
           });
       }


    </script>

   <style>

         /* Default Print */
        /*print content*/
        @media print {
            #ContentPrint {
                display: block;
            }

            #ContentShow {
               display: none;
            }
            .PrintPD {
                display :block ;
            }
        }

        /*view content*/
        @media screen {
            #ContentPrint {
                display: none;
            }

            #ContentShow {
               display: block;
            }
        }
    /* End */

        .div_policy_payment {
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

        .th {
            text-align:center;
            font:bold;
            color:darkolivegreen
        }
    </style>


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
		<%--here is the block code--%>
    
            <br /><br /><br />
        <div class="panel panel-default">
            <div class="panel-heading">

                    <h3 class="panel-title">Policy Payment History</h3>
                </div>

            <div class="panel-body">

                <div class="PrintPD"> 
                  
                    <%--  <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>--%>
              
                  
                    <div id="dvPolicyPayment"  runat="server">       
               
                    </div>
                    <asp:HiddenField ID="hdnData" runat="server" />

                    <table id="tblExport" cellpadding="0" cellspacing="0" width="1262px;" border="1"  style="border-top-style:none; ">
	                <thead>
                        <tr style="border:none;">
                            <th colspan="16">
                               <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                                <br />
                            </th>
                        </tr>
		                <tr>
			                <th class="th" >No</th>
			                <th class="th">Pol.no</th>
			                <th class="th">Customer</th>
			                <th class="th">Gender</th>
			                <th class="th">Year/Times</th>
			                <th class="th">Mode</th>
			                <th class="th">Sum Insure</th>
			                <th class="th">Amount</th>
                            <th class="th">Interest</th>
                            <th class="th">Prem & Interest</th>
			                <th class="th">Due Date</th>
                            <th class="th">Pay Date</th>
			                <th class="th">Next Due</th>
                            <th class="th">Receipt No</th>
			                <th class="th">Product</th>
                            <th class="th">Status</th>
		                </tr>
                        <tr>
                            <td colspan="16">
                               <asp:Label ID="lblinterest" runat="server" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
	                </thead>
	                <tbody>
			                <%=getWhileLoopData()%>
	                </tbody>
                </table>
                </div>
            </div>
        </div>

        <!-- Modal Search Policy -->
        <div id="myModalSearch" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearch" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="H2">Search Policy Payment Form</h3>
            </div>

            <div class="modal-body">

                <!---Modal Body--->
                    
               <table style="width: 100px;">
                    <tr>
                        <td>From Date:</td>
                        <td>
                            <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker span2" TabIndex="1" ></asp:TextBox>
                        </td>
                        <td>To Date:</td>
                        <td>
                            <asp:TextBox ID="txtTo_date"  runat="server" CssClass="datepicker span2" TabIndex="2" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="">Last Name:</td>
                        <td style="">
                            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                        </td>
                        <td style="">First Name:</td>
                        <td style="">
                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:30px;">Policy Num:</td>
                        <td>
                            <asp:TextBox ID="txtPoliNumberSearch"  runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>

            <label style="color:red; font-style:italic;">Searched by Payment Date</label>

            <div class="modal-footer">

                <div style="display: none;">
                       <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                </div>

               <input type="button" onclick="onclick_search()" id="btnOk_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="Search" ValidationGroup="1" />
               
               <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button

            </div>
        </div>
        <!--End Modal Search -->

	</ContentTemplate>

         <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
         </Triggers>

    </asp:UpdatePanel>	
</asp:Content>