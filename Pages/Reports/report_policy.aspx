<%@ Page Title="Clife | Reports => Individual Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_policy.aspx.cs" Inherits="Pages_Reports_report_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

   <ul class="toolbar" style="text-align:left">
        <%--<li>
            <asp:ImageButton ID="ImagPrintPDF"  runat="server" ImageUrl="~/App_Themes/functions/download_pdf.png" OnClick="ImagPrintPDF_Click"  />
        </li>--%>
        <li>
            <asp:ImageButton ID="ImgPrint"  runat="server" ImageUrl="~/App_Themes/functions/download_excel.png" OnClick="ImgPrint_Click" />
        </li>
       <li>
            <%--<asp:ImageButton ID="ImgPrint_Auto"  runat="server" ImageUrl="~/App_Themes/functions/print.png"  OnClientClick="Print_default();"/>--%>
           <input type="button"  onclick="Print_default();" style="background: url('../../App_Themes/functions/print.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />      

       </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalReportPolicy" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
  </ul>

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


        function GetDataFromDiv() {
            var cookieValue = document.getElementById('AppendDivPolicy').getAttribute('value');
            alert(cookieValue);
        }

        function intOnly(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        function Print_default() {
            $(".PrintPD").print();
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

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>

   
     <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>


    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>

		<%--here is the block code--%>
        <!-- Modal Report Policy Form -->           
            <br />
            <br />            
            <br />

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Individual Policy Report</h3>
                    
                </div>
                <%--<div class="panel-heading">
                    <h3 class="panel-title">Policy Report

                         <asp:Label ID="Label1" runat="server" Visible="false" Text="From:" ForeColor="#3399ff"></asp:Label>&nbsp; <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                    &nbsp;<asp:Label ID="Label2" runat="server" Visible="false" Text="To:" ForeColor="#3399ff"></asp:Label>&nbsp;<asp:Label ID="lblto" runat="server"  ForeColor="Red" ></asp:Label>
                    </h3>
                </div>--%>
                <div class="panel-body">
                    <!-- Append data to Div -->
            <%--<div id="AppendDivPolicy" runat="server">
               
            </div>--%>

            <!-- End of Append Data to Div -->
                    
            <!-- Append data to Div -->

              <div class="PrintPD">        
                    <asp:Label ID="lblfrom" runat="server" ForeColor="Red" ></asp:Label>
                    <asp:Label ID="lblto" runat="server" ForeColor="Red" ></asp:Label>
              
                    <br />
                    <!-- Append data to Div -->
                    <div id="AppendDivPolicy" class="PrintPD" runat="server">
              
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
                            <td>From Date:</td>
                            <td>
                                <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker span2" TabIndex="1" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorFromDate" ControlToValidate="txtFrom_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                            <td>To Date:</td>
                            <td>
                                <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker span2" TabIndex="2" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" ControlToValidate="txtTo_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td> Product </td>
                            <td>
                                <asp:HiddenField ID="hdfProduct" runat="server"  Value='<%#Eval("ddlProduct.SelectedValue") %>'/>
                                 <asp:DropDownList ID="ddlProduct" width="95%" height="25px" runat="server" TabIndex="3" AppendDataBoundItems="True" DataSourceID="SqlDataSourceProduct" DataTextField="Product_ID" DataValueField="Product_ID">
                                 <asp:ListItem Value="-1">.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Payment Mode</td>
                            <td>
                                 <asp:HiddenField ID="hdfPaymentMode" runat="server"  Value='<%#Eval("ddlPaymentMod.SelectedValue") %>'/>
                                 <asp:DropDownList ID="ddlPaymentMod" width="95%" height="25px" runat="server" TabIndex="4" AppendDataBoundItems="True" DataSourceID="SqlDataSourcePaymentMode" DataTextField="Mode" DataValueField="Pay_Mode_ID">
                                 <asp:ListItem Value="-1">.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Policy Number</td>
                            <td>
                                <asp:HiddenField ID="hdfPolicyNum" runat="server"  Value='<%#Eval("txtPolicyNumber.text") %>'/>
                               <asp:TextBox ID="txtPolicyNumber"  runat="server"  width="86%" TabIndex="5" ></asp:TextBox> 
                                <%--onChange="intOnly(this);" onKeyUp="intOnly(this);" onKeyPress="intOnly(this);"--%>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="float:left; padding-left:25px; width:35%; margin-left:10%; box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:199px;">
                            <asp:GridView  ID="GvoPolicyStatus"  Width="39%" Height="5px" runat="server" DataSourceID="SqlDataSourcePolicyStatusType"  AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center" BorderColor="White" BorderWidth="0" BorderStyle="NotSet" EditRowStyle-BorderColor="White" EmptyDataRowStyle-BorderColor="White" CellPadding="2" HeaderStyle-BackColor="White" HeaderStyle-BorderColor="White" RowStyle-BorderColor="White" CellSpacing="200" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" PagerStyle-BorderColor="White" EditRowStyle-BorderStyle="NotSet" EmptyDataRowStyle-BorderStyle="NotSet" FooterStyle-BorderStyle="None" HeaderStyle-BorderStyle="NotSet" PagerStyle-BorderStyle="NotSet" RowStyle-BorderStyle="NotSet">
                                <Columns>
                                    <asp:TemplateField>
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckb1"  runat="server" TextAlign="Right" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Status_Type_ID" ) + "\");" %>' /> 
                                        <asp:HiddenField ID="hdfPolicy_Status_ID" runat="server" Value='<%#Eval("Policy_Status_Type_ID") %>' />
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
                                                <asp:ListItem Value="0" Selected="True"><label>&nbsp;&nbsp;Policy number</label></asp:ListItem>
                                                <asp:ListItem Value="1">&nbsp;&nbsp;Issue date</asp:ListItem>
                                                <asp:ListItem Value="2">&nbsp;&nbsp;Effective date</asp:ListItem>
                                                <asp:ListItem Value="3">&nbsp;&nbsp;Next due</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                     </div>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnOk" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="1" OnClick="btnOk_Click" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="ClearGrid();">Cancel</button>
                </div>
            </div>
            <!--End Modal Report Policy Form-->

                    <asp:Label ID="lblfrom1" Visible="true"  runat="server" Text="Label" ForeColor="White"></asp:Label>
                    <asp:Label ID="lblto1" Visible="true" runat="server" Text="Label" ForeColor="White"></asp:Label>

           <!--- Section Sqldatasource--->
                <asp:SqlDataSource ID="SqlDataSourcePolicyStatusType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_Status_Type_ID,Detail from Ct_Policy_Status_Type order by Detail asc "></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Pay_Mode_ID,Mode from Ct_Payment_Mode"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Product_ID from Ct_Product order by Product_ID asc"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourcePolicyNumber" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_ID,Policy_Number from Ct_Policy_Number  order by Policy_Number"></asp:SqlDataSource>
           <!-- End Section  -->


                </div>

            </div>

            

	    </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>