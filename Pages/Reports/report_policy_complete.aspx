<%@ Page Title="Clife | Report => Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_policy_complete.aspx.cs" Inherits="Pages_Reports_report_policy_complete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

    <ul class="toolbar" style="text-align: left">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>
        <li>
            <asp:ImageButton ID="ImgPrint" runat="server" ImageUrl="~/App_Themes/functions/download_excel.png" OnClick="ImgPrint_Click" />
        </li>
        <li>
            <input type="button" onclick="Print_default();" style="background: url('../../App_Themes/functions/print.png') 
                             no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
         <li>
            <asp:ImageButton ID="ImgCreateChart" data-toggle="modal" data-target="#dvChartModal" runat="server" ImageUrl="~/App_Themes/functions/create_chart.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalReportPolicy" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
       
        
    </ul>

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>

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
            $('.PrintPD').attr('href', '../../print_single.css');
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
                display :block ;
                font-size: 8pt;
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
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--Operation here--%>
            <br />
            <br />
            <br />
            
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Policy Report</h3>
                    
                </div>
                <div class="panel-body">

                    <div class="PrintPD">
                        <asp:Label ID="lblfrom" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblto" runat="server" ForeColor="Red"></asp:Label>

                        <br />
                        <!-- Append data to Div -->
                        <div id="AppendDivPolicy" class="PrintPD" runat="server">
                        </div>

                        <asp:Label ID="lblNumberOfPolicy" runat="server" Text="" ForeColor="#3399ff" Font-Bold="true"></asp:Label>

                        <asp:GridView ID="gvPolicy" Font-Size="8" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" DataSourceID="PolicyDataSource" Width="100%" HorizontalAlign="Center" OnRowDataBound="gvPolicy_RowDataBound" PageSize="100">
                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                     <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Policy_Number" HeaderText="Policy no." SortExpression="Policy_Number" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Sale_Agent" HeaderText="Agent Name" SortExpression="Sale_Agent" ItemStyle-HorizontalAlign="Left">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                                     <HeaderStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Customer_ID" HeaderText="Cust. ID" SortExpression="Customer_ID" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                </asp:BoundField>
                                
                                <%-- <asp:BoundField DataField="App_Number" HeaderText="Application no." SortExpression="App_Number" ItemStyle-HorizontalAlign="Center" >
                             <ItemStyle HorizontalAlign="Center" />
                             </asp:BoundField>--%>


                                <asp:TemplateField HeaderText="Cust. Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerFullName" runat="server" Text='<%# GetDisplayName(DataBinder.Eval(Container.DataItem, "First_Name").ToString(), DataBinder.Eval(Container.DataItem, "Last_Name").ToString(), DataBinder.Eval(Container.DataItem, "Khmer_First_Name").ToString(), DataBinder.Eval(Container.DataItem, "Khmer_Last_Name").ToString(), DataBinder.Eval(Container.DataItem, "Product_ID").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="Last_Name" HeaderText="Surname" SortExpression="Last_Name">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="First_Name" HeaderText="Firstname" SortExpression="First_Name">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <%--<asp:BoundField DataField="Khmer_Last_Name" HeaderText="Surname" SortExpression="Khmer_Last_Name">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                                    
                                </asp:BoundField>
                                <asp:BoundField DataField="Khmer_First_Name" HeaderText="Firstname" SortExpression="Khmer_First_Name">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Age_Insure" HeaderText="Age" SortExpression="Age_Insure" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Gen.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="Agreement_Date" HeaderText="Signature date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Agreement_Date" ItemStyle-HorizontalAlign="Center" >
                             <ItemStyle HorizontalAlign="Center" />
                             </asp:BoundField>--%>
                                <asp:BoundField DataField="Product_ID" HeaderText="Product" SortExpression="Product_ID" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Assure_Year" HeaderText="Cov. Period" SortExpression="Assure_Year" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Pay_Year" HeaderText="Pmt Period" SortExpression="Pay_Year" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Sum_Insure" HeaderText="SI" SortExpression="Sum_Insure" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Original_Amount" HeaderText="Annual Premium" SortExpression="Original_Amount" DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Right" >                             
                             <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                             </asp:BoundField>

                                <asp:BoundField DataField="Premium" HeaderText="System Premium" SortExpression="Premium" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Discount_Amount" HeaderText="Disc." SortExpression="Discount_Amount" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:BoundField DataField="EM_Amount" HeaderText="EM" SortExpression="EM_Amount" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Premium_Paid" HeaderText="Total Premium" SortExpression="Premium_Paid" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Pmt Mode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymode" runat="server" Text='<%# GetPayMode(DataBinder.Eval(Container.DataItem, "Pay_Mode").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="No_of_Payment" HeaderText="No. of Pmt" SortExpression="No_of_Payment" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                </asp:BoundField>

                                 <asp:TemplateField HeaderText="NPI">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNPI" runat="server" Text='<%# CalculateNPI(DataBinder.Eval(Container.DataItem, "Premium").ToString(), DataBinder.Eval(Container.DataItem, "No_of_Payment").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="API">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAPI" runat="server" Text='<%# CalculateAPI(DataBinder.Eval(Container.DataItem, "Premium").ToString(), DataBinder.Eval(Container.DataItem, "Pay_Mode").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="No. of Pmt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNumberOfPayment" runat="server" Text='<%# GetNumberOfPayment(DataBinder.Eval(Container.DataItem, "Policy_ID").ToString(), Convert.ToInt32( DataBinder.Eval(Container.DataItem, "Policy_Type_ID")), Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "Due_Date")))%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Due_Date" ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center"/>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Issue_Date" HeaderText="Issue    Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Issue_Date" ItemStyle-HorizontalAlign="Center">

                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Effective_Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Maturity_Date" HeaderText="Maturity Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Maturity_Date" ItemStyle-HorizontalAlign="Center">


                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                

                            </Columns>
                            <PagerSettings Position="Top" />
                            <PagerStyle HorizontalAlign="Center" />
                        </asp:GridView>
                        <br />
                    </div>
                    <asp:SqlDataSource ID="PolicyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT TOP (100) * FROM V_Policy_Complete ORDER BY Policy_Number DESC"></asp:SqlDataSource>


                
            </div>

                <div id="myModalReportPolicy" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalReportPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Policy Report</h3>
                </div>

                <div class="modal-body">
                    <!---Modal Body--->
                    <table style="text-align: left; padding-left: 20px;">
                        <tr>
                            <td width="80px">&nbsp;&nbsp;&nbsp;From Date:</td>
                            <td>
                                <asp:HiddenField ID="hdfFromDate" runat="server" />
                                <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker span2" onkeypress="return false;" Width="150" TabIndex="1" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorFromDate" ControlToValidate="txtFrom_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                            <td width="80px">&nbsp;&nbsp;To Date:</td>
                            <td>
                                <asp:HiddenField ID="hdfToDate" runat="server" />
                                <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker span2" onkeypress="return false;"  Width="150" TabIndex="2" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorToDate" ControlToValidate="txtTo_date" runat="server" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="80px">&nbsp;&nbsp;&nbsp;Product </td>
                            <td>
                                <asp:HiddenField ID="hdfProduct" runat="server"  Value='<%#Eval("ddlProduct.SelectedValue") %>'/>
                                 <asp:DropDownList ID="ddlProduct" width="95%" height="25px" runat="server" TabIndex="3" AppendDataBoundItems="True" DataSourceID="SqlDataSourceProduct" DataTextField="Product_ID" DataValueField="Product_ID">
                                 <asp:ListItem Value="-1">.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="80px">&nbsp;&nbsp;Policy No.</td>
                            <td>
                                <asp:HiddenField ID="hdfPolicyNum" runat="server" Value='<%#Eval("txtPolicyNumber.text") %>'/>
                               <asp:TextBox ID="txtPolicyNumber"  runat="server"  Width="150" TabIndex="4" ></asp:TextBox>                                
                            </td>
                        </tr>
                       
                    </table>
                    <br />
                    <div style="float:left; margin-left:43px; padding-left:10px; padding-top:10px; padding-right:10px; width:36%;  box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:240px;">
                        <asp:GridView ID="GvoPolicyStatus" CssClass="Gov_policy" Width="100%" Height="5px" runat="server" DataSourceID="SqlDataSourcePolicyStatusType"  AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center" BorderColor="White" BorderWidth="0" BorderStyle="NotSet" EditRowStyle-BorderColor="White" EmptyDataRowStyle-BorderColor="White" CellPadding="2" HeaderStyle-BackColor="White" HeaderStyle-BorderColor="White" RowStyle-BorderColor="White" CellSpacing="200" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" PagerStyle-BorderColor="White" EditRowStyle-BorderStyle="NotSet" EmptyDataRowStyle-BorderStyle="NotSet" FooterStyle-BorderStyle="None" HeaderStyle-BorderStyle="NotSet" PagerStyle-BorderStyle="NotSet" RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="0">
                            <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="indent_bottom" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckb1"  runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Status_Type_ID" ) + "\");" %>' /> 
                                        <asp:HiddenField ID="hdfPolicy_Status_ID" runat="server" Value='<%#Eval("Policy_Status_Type_ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:BoundField DataField="Policy_Status_Type_ID"  HeaderText="" Visible="false" />
                                  <asp:BoundField DataField="Detail" HeaderText="Status Code" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                            <RowStyle HorizontalAlign="Left" />
                        </asp:GridView>
                    </div>
               
                    <div style="float:right; width:35%; padding-left:20px; margin-right:10%; box-shadow: 2px 2px 2.5px rgba(182, 255, 0, 0.64); border:solid 1px ; border-color:lightgray; border-radius:5px; height:250px;">
                            <table>
                                <tr>
                                    <td style="padding:20px 20px 20px 20px;">Order by:
                                        <asp:Panel ID="Panel1" runat="server"  CssClass="panel_border" Height="90px" Width="150px">
                                            <asp:RadioButtonList ID="RdbOrderBy" runat="server" CssClass="radio_style" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" width="100%"  >
                                                <asp:ListItem Value="Policy_Number" Selected="True"><label>&nbsp;&nbsp;Policy number</label></asp:ListItem>
                                                <asp:ListItem Value="Issue_Date">&nbsp;&nbsp;Issue date</asp:ListItem>
                                                <asp:ListItem Value="Effective_Date">&nbsp;&nbsp;Effective date</asp:ListItem>
                                                
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
                    
                </div>
            </div>

             <!-- Modal Result -->
            <div id="ResultModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalResult" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnCancelNote" runat="server" Text="Close" class="btn" data-dismiss="modal" aria-hidden="true" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>
            <!--End Modal Result -->

            <!-- Modal Search Policy -->
            <div id="mySearchPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SAppNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Policy No</a></li>
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Customer Name</a></li>                       
                    </ul>

                    <div class="tab-content" style="height: 60px; overflow: hidden;">
                        <div class="tab-pane active" id="SAppNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Policy No:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumberSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>
                            </table>

                        </div>
                        <div class="tab-pane" style="height: 70px;" id="SCustomerName">                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Surname:</td>
                                    <td>
                                        <asp:TextBox ID="txtSurnameSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                   
                                </tr>
                                 <tr>
                                    <td width="70px;">&nbsp;&nbsp;Firstname:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstnameSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>
                            </table>

                        </div>
                        

                    </div>
                   
                </div>
                <div class="modal-footer">
                    <%--<input type="button" class="btn btn-primary" style="height: 27px;" data-dismiss="modal" value="Search"  />--%>
                    <asp:Button ID="btnSearchPolicy" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Search" OnClick="btnSearchPolicy_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Search Policy-->

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
                                    <asp:ListItem Value="2">&nbsp;Policy</asp:ListItem>
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
            <asp:SqlDataSource ID="SqlDataSourcePolicyStatusType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_Status_Type_ID,Detail from Ct_Policy_Status_Type order by Detail asc "></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Pay_Mode_ID,Mode from Ct_Payment_Mode"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Product_ID from Ct_Product order by Product_ID asc"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourcePolicyNumber" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_ID,Policy_Number from Ct_Policy_Number  order by Policy_Number"></asp:SqlDataSource>
            <!-- End Section  -->

            <%--Hidden Fields--%>
            <asp:HiddenField ID="hdfPolicyID" runat="server" />
            <asp:HiddenField ID="hdfAge" runat="server" />
            <asp:HiddenField ID="hdfSumInsured" runat="server" />
            <asp:HiddenField ID="hdfAssureYear" runat="server" />
            <asp:HiddenField ID="hdfPolicy_Status_ID" runat="server" />

            <%-- Hidden Fields for Public Property --%>
            <asp:HiddenField ID="hdfOrderBy" runat="server"/>
            <asp:HiddenField ID="hdfPolicyStatus" runat="server" />
            <asp:HiddenField ID="hdfCheckFormLoadOrSearch" runat="server" />
            <asp:HiddenField ID="hdfChartType" runat="server" />
            <asp:HiddenField ID="hdfChartData" runat="server" />

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearchPolicy" />
            <asp:PostBackTrigger ControlID="btnOk" />
            <asp:PostBackTrigger ControlID="btnCreateChart" />
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

