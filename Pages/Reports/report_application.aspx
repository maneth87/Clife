<%@ Page Title="Clife | Reports => Application" Language="C#"  MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_application.aspx.cs" Inherits="Pages_Reports_report_application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

     <ul class="toolbar" style="text-align:left">
        <%--<li>
            <asp:ImageButton ID="ImgPrintPDF"  runat="server" ImageUrl="~/App_Themes/functions/download_pdf.png" OnClick="ImgPrintPDF_Click"  />
        </li>--%>
        <li>
            <asp:ImageButton ID="ImgPrint_Auto"  runat="server" ImageUrl="~/App_Themes/functions/print.png"  OnClientClick="Print_default();"/>
        </li>
        <li>
            <asp:ImageButton ID="ImgPrint"  runat="server" ImageUrl="~/App_Themes/functions/download_excel.png" OnClick="ImgPrint_Click" />
        </li>
           <li>
           <asp:ImageButton ID="ImgCreateChart" data-toggle="modal" data-target="#dvChartModal" runat="server" ImageUrl="~/App_Themes/functions/create_chart.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalReportApplication" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
      
  </ul>

    <script type="text/javascript">

        //Section date picker //  $(document).ready(function ()) means that everything in this function will start auto when Loading page

        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        function SelectSingleCheckBox(ckb, Status_Code) {

            if (ckb.checked) {

                $('#Main_hdfApplicationCode').val(Status_Code);
            }
            else {
                $('#Main_hdfApplicationCode').val("");
                ckb.checked = false;
                ClearGrid();
            }
        }

        function ClearGrid() {

            $('#Main_GvoApplicationStatus').val("");
        }


        function GetDataFromDiv() {
            var cookieValue = document.getElementById('divExport').getAttribute('value');
            alert(cookieValue);
        }

        function Print_default() {
            $(".PrintPD").print();
        }

        //Open new blank chart tap
        function SetTarget() {

            document.forms[0].target = "_blank";
            $("#dvChartModal").modal('hide');
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
                display: block;
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

        .indent_bottom {
            padding-bottom: 6px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>
     
   
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%--here is the block code--%>
            <br /><br /><br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Application Report</h3>
                    
                </div>
                
             <!-- Modal Report Application Form -->
           
            <div class="panel-body">
            <!-- Append data to Div -->
         <%--   <div id="divExport" runat="server">  
       
            </div> --%>
            <!-- End of Append Data to Div -->

             
            <div class="PrintPD">        
                    <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                    <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>
              
                    <br />
                    <!-- Append data to Div -->
                    <div id="divExport" class="PrintPD" runat="server">
              
                    </div>
               </div>  


            <div id="myModalReportApplication"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalReportApplicationHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Application Report</h3>
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

                    <div style="float:left; margin-left:43px; padding-left:10px; padding-top:10px; padding-right:10px; width:36%;  box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:260px;">
                        <asp:GridView ID="GvoApplicationStatus" CssClass="Gov_policy" Width="100%" Height="5px" runat="server" DataSourceID="SqlDataSourceApplicationCode"  AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center" BorderColor="White" BorderWidth="0" BorderStyle="NotSet" EditRowStyle-BorderColor="White" EmptyDataRowStyle-BorderColor="White" CellPadding="2" HeaderStyle-BackColor="White" HeaderStyle-BorderColor="White" RowStyle-BorderColor="White" CellSpacing="200" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" PagerStyle-BorderColor="White" EditRowStyle-BorderStyle="NotSet" EmptyDataRowStyle-BorderStyle="NotSet" FooterStyle-BorderStyle="None" HeaderStyle-BorderStyle="NotSet" PagerStyle-BorderStyle="NotSet" RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="0">
                            <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="indent_bottom" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckb1"  runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Status_Code" ) + "\");" %>' /> 
                                        <asp:HiddenField ID="hdfApplicationCode" runat="server" Value='<%#Eval("Status_Code") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Status_Code"  HeaderText="" Visible="false" />
                                <asp:BoundField DataField="Detail" HeaderText="Status Code" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                            <RowStyle HorizontalAlign="Left" />
                        </asp:GridView>
                    </div>

                     <div style="float:right; width:36%; padding-left:20px; margin-right:10%; box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:270px;">
                         <table>
                            <tr>
                                <td style="padding:20px 20px 20px 20px;">Order by:
                                     <asp:Panel ID="Panel1" runat="server" CssClass="rdb_style" Width="150px" Height="25px" >
                                        <asp:RadioButtonList ID="RdbOrderBy" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" width="100%"  >
                                            <asp:ListItem Value="0" Selected="True"><label>&nbsp;App Number</label></asp:ListItem>
                                            <asp:ListItem Value="1">&nbsp;Register Date</asp:ListItem>
                                        </asp:RadioButtonList>
                                   </asp:Panel>
                               </td>
                            </tr>
                        </table>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnOk" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="1" OnClick="btnOk_Click"  />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="ClearGrid();">Cancel</button>
                </div>
            </div>
            <!--End Modal Report Policy Form-->

            <!--Chart Modal-->
            <div id="dvChartModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalChartHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Chart</h3>
                </div>

                <div class="modal-body">
                    <!---Modal Body--->
                    <table  style="text-align: left;">
                        <tr>
                            <td width="50px" style="vertical-align:middle">Type:</td>
                            <td >
                                <asp:DropDownList ID="ddlType" runat="server">
                                    <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                    <asp:ListItem Value="1">Pie Chart</asp:ListItem>
                                    <asp:ListItem Value="2">Combo Chart</asp:ListItem>
                                    <asp:ListItem Value="3">Line Chart</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" ControlToValidate="ddlType" runat="server" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </td>
                           
                        </tr>
                        <tr>
                            <td width="70px">Data:</td>
                            <td >
                                <asp:RadioButtonList ID="rbtnlData" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" width="100%">
                                    <asp:ListItem Selected="True" Value="0">&nbsp;Sum Insured</asp:ListItem>
                                    <asp:ListItem Value="1">&nbsp;Premium</asp:ListItem>
                                    <asp:ListItem Value="2">&nbsp;Application</asp:ListItem>
                                </asp:RadioButtonList>
                                
                            </td>
                        </tr>
                        
                    </table>                   
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnCreateChart" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Create Chart" ValidationGroup="2" OnClick="btnCreateChart_Click" OnClientClick="SetTarget()" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Chart Modal-->

            <!--- Section Sqldatasource--->
                <asp:SqlDataSource ID="SqlDataSourceApplicationCode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Status_Code,Detail from Ct_Underwrite_Table order by Detail asc"></asp:SqlDataSource>
            <!-- End Section  -->

                <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
             <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>

            </div>
                </div>

            <%-- Hidden Fields for Public Property --%>
            <asp:HiddenField ID="hdfOrderBy" runat="server"/>
            <asp:HiddenField ID="hdfPolicyStatus" runat="server" />
            <asp:HiddenField ID="hdfCheckFormLoadOrSearch" runat="server" />
            <asp:HiddenField ID="hdfChartType" runat="server" />
            <asp:HiddenField ID="hdfChartData" runat="server" />
	</ContentTemplate>

          <Triggers>
             <asp:PostBackTrigger ControlID="btnOk" />
             <asp:PostBackTrigger ControlID="btnCreateChart" />
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>
