<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/Content.master" CodeFile="terminate_policy.aspx.cs" Inherits="Pages_CL24_Terminate_Policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

    
    <ul class="toolbar" style="text-align:left">
       
      <%--  <li>          
            <input type="button" id="export" onclick="ExportExcel();" style="background: url('../../App_Themes/functions/download_excel.png') 
                             no-repeat; border: none; height: 40px; width: 90px; " />      
        </li>--%>
        <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary"  style="display: none;"/>
      
        <li>
            <asp:ImageButton ID="ImgBtnSave" runat="server" Enabled="False"  Visible="true" ImageUrl="~/App_Themes/functions/save.png" OnClick="ImgBtnTerminate_Click" />
        </li>

        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearchPolicy" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
           
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
              $("#Main_hdfPayYear").val("");
          });

          function showSuccessMessage(message) {
              swal("Done!", message, "success", {
                  button: "Ok",
              });
          }

          function showFailMessage(message) {
              swal("Failed!", message, "error", {
                  button: "Ok",
              });
          }

          function alertMesg(message) {
              alert(message);
          }

          //Function Export report to excel
          function ExportExcel() {

              $("#dvReport").btechco_excelexport({
                  containerid: "dvReport"
                 , datatype: $datatype.Table
              });
          }

          function INSERTStatusRemark(policy_number, policy_year, pay_year) {
              console.log(pay_year);
              $("#Main_txtINSERTPolicyNumber").val("");
              $("#Main_hdfINSERTPolicyNumber").val("");
              $("#Main_txtINSERTPolicyYear").val("");
              $("#Main_hdfPayYear").val("");
              $("#Main_txtINSERTRemarks").val("");

              $("#Main_txtINSERTPolicyNumber").val(policy_number);
              $("#Main_hdfINSERTPolicyNumber").val(policy_number);
              $("#Main_txtINSERTPolicyYear").val(policy_year);
              $("#Main_hdfPayYear").val(pay_year);

              if (pay_year <= 2) {
                  $("#Main_ddlINSERTStatus").val("PEN");
              } else {
                  $("#Main_ddlINSERTStatus").val("POF");
              }
              
              $("#ModalINSERTRemarkPolicy").modal("show");
          }

          function EDITStatusRemark(policy_number, policy_year, pay_year) {
              
              $("#Main_txtEDITPolicyNumber").val("");
              $("#Main_hdfEDITPolicyNumber").val("");
              $("#Main_txtEDITRemarks").val("");
              $("#Main_txtEDITPolicyYear").val("");
              $("#Main_hdfPayYear").val("");

              $.ajax({
                  type: "POST",
                  url: "../../PolicyWebService.asmx/EDITStatusRemark",
                  data: "{policy_number:'" + policy_number + "', pay_year: '" + pay_year + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (data) {
                      var remark = data.d;
                      var arr = remark.split(',');

                      $("#Main_txtEDITPolicyNumber").val(policy_number);
                      $("#Main_txtEDITPolicyYear").val(policy_year);
                      $("#Main_ddlEditStatus").val(arr[0]); // status
                      $('#Main_txtEDITRemarks').val(arr[1]);
                      $('#Main_txtEDITPaidOFFDate').val(formatJSONDate(arr[2]));
                      $("#Main_hdfEDITPolicyNumber").val(policy_number);
                      $("#Main_hdfPayYear").val(pay_year);
                      $('#ModalEDITRemarkPolicy').modal('show');

                  }
                  
              });

          }

          function SelectSingleCheckBox(ckb, policy_number, product_id, gender) {
              var GridView = ckb.parentNode.parentNode.parentNode;

              var ckbList = GridView.getElementsByTagName("input");

              for (i = 0; i < ckbList.length; i++) {
                  if (ckbList[i].type == "checkbox") {
                      document.getElementById("<%=ImgBtnSave.ClientID%>").disabled = true;
                  } 
              }

              document.getElementById("<%=ImgBtnSave.ClientID%>").disabled = false;
          }

          //Format Date
          function formatDate(Date) {

              var value = Date.substring(0, 10);
              var arr = value.split('/');

              var year = arr[2].substring(0, 4);

              var day = "";
              if (arr[1].length == 1) {
                  day = "0" + arr[1];
              }
              else {
                  day = arr[1];
              }

              var month = "";
              if (arr[0].length == 1) {
                  month = "0" + arr[0];
              }
              else {
                  month = arr[0];
              }
              var convert_date = day + "/" + month + "/" + year;
              return convert_date;
          }

          //Format Date in Jtemplate
          function formatJSONDate(jsonDate) {
              var value = jsonDate;
              if (value.substring(0, 6) == "/Date(") {
                  var dt = new Date(parseInt(value.substring(6, value.length - 2)));

                  if (dt.getFullYear() > 1) {

                      var dtDay;
                      if (dt.getDate() >= 10) {
                          dtDay = dt.getDate();
                      }
                      else {
                          dtDay = "0" + dt.getDate();
                      }
                      var dtMonth;
                      var intMonth = dt.getMonth() + 1;
                      if (intMonth < 10) {
                          dtMonth = "0" + intMonth;
                      }
                      else {
                          dtMonth = "" + intMonth;
                      }

                      var dtString = dtDay + "/" + dtMonth + "/" + dt.getFullYear();
                      value = dtString;
                  }
                  else {

                      value = "";
                  }
              }
              else {
                  value = formatDate(jsonDate);
              }

              return value;

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
        .required {
            color: red;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
      <%-- Form Design Section--%>
            <div style="height:50px"></div>        
            <div id="myModalSearchPolicy" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Search Policy</h3>
                </div>

                <div class="modal-body">
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SPolicy_Number" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Report Date</a></li>                       
                    </ul>
                    <div class="tab-content" style="height: 40px; overflow: hidden;">

                        <div class="tab-pane active" style="height: 70px;" id="SReport_Date">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td style="width: 20%;"><b>Report Date</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtReportDate" runat="server" CssClass="datepicker" placeholder="DD/MM/YYYY" Width="46%" TabIndex="4" onkeypress="return false"></asp:TextBox>
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
            <%--Modal Add Remark Policy--%>
            <div id="ModalINSERTRemarkPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="ModalINSERTRemarkPolicy" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H1">Terminate Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <div class="tab-content" style="height: 220px; overflow: hidden;">
                        <div class="tab-pane active" id="Div2">
                            <table>
                                <tr>
                                    <td style="width: 20%;"><b>Policy Number</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtINSERTPolicyNumber" Width="88%" runat="server" TabIndex="2" disabled="true" ></asp:TextBox>
                                        <asp:HiddenField ID="hdfINSERTPolicyNumber" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 20%;"><b>Policy Year</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtINSERTPolicyYear" Width="88%" runat="server" TabIndex="2" disabled="true" ></asp:TextBox>
                                        <asp:HiddenField ID="hdfPayYear" runat="server" />
                                        <asp:HiddenField ID="hdfINSERTPolicyYear" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 20%;"><b>Paid OFF Date</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtINSERTPaidOFFDate" runat="server" CssClass="datepicker" placeholder="DD-MM-YYYY" Width="88%" TabIndex="4" onkeypress="return false"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td style="width: 20%;"><b>Status</b>:</td>
                                    <td>
                                        <asp:DropdownList ID="ddlINSERTStatus" runat="server" style="width: 327px;" >
                                            <asp:listitem text="--" Value="--"/>
                                            <asp:listitem text="Paid Off" Value="POF"/>
                                            <asp:listitem text="Penalty" Value="PEN"/>
                                        </asp:DropdownList> 
                                        
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 20%;"><b>Remark</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtINSERTRemarks" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </div>

                    </div>
                   
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSave" class="btn btn-primary" Style="height: 27px;" runat="server" Text="ADD"  OnClick="btnSaveRemark_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>

            <%--End Modal Add Remark Policy--%>

            <%--Modal EDIT Remark Policy--%>
            <div id="ModalEDITRemarkPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="ModalEDITRemarkPolicy" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H3">Terminate Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <div class="tab-content" style="height: 220px; overflow: hidden;">
                        <div class="tab-pane active" id="Div3">
                            <table>
                                <tr>
                                    <td style="width: 20%;"><b>Policy Number</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtEDITPolicyNumber" Width="88%" runat="server" TabIndex="2" disabled="true" ></asp:TextBox>
                                        <asp:HiddenField ID="hdfEDITPolicyNumber" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 20%;"><b>Policy Year</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtEDITPolicyYear" Width="88%" runat="server" TabIndex="2" disabled="true" ></asp:TextBox>
                                        <asp:HiddenField ID="hdfEditPolicyYear" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 20%;"><b>Paid OFF Date</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtEDITPaidOFFDate" runat="server" CssClass="datepicker" placeholder="DD/MM/YYYY" Width="88%" TabIndex="4" onkeypress="return false"></asp:TextBox>
                                    </td>
                                        
                                </tr>
                                <tr>
                                    <td style="width: 20%;"><b>Status</b>:</td>
                                    <td>
                                        <asp:DropdownList ID="ddlEditStatus" runat="server" style="width: 327px;">
                                            <asp:listitem text="--" Value="--"/>
                                            <asp:listitem text="Paid Off" Value="POF"/>
                                            <asp:listitem text="Penalty" Value="PEN"/>
                                        </asp:DropdownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 20%;"><b>Remark</b>:</td>
                                    <td>
                                        <asp:TextBox ID="txtEDITRemarks" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </div>

                    </div>
                   
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnEdit" class="btn btn-primary" Style="height: 27px;" runat="server" Text="EDIT"  OnClick="btnEDITRemark_Click" /> 
                    <asp:Button ID="btnRemove" class="btn btn-danger" Style="height: 27px;" runat="server" Text="Remove"  OnClick="btnRemove_Click" /> 
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>

            <%--End Modal Add Remark Policy--%>

            <asp:HiddenField ID="hdfuserid" runat="server" />
            <asp:HiddenField ID="hdfusername" runat="server" />

            <b><asp:Label ID="lblDateRange" runat="server" ForeColor="Red"></asp:Label></b>
            
            <div id="dvReport" width="98%">        
                    <div class="panel panel-default" id="POLICY_REPORT" runat="server">
                <div class="panel-heading">
                    <h3 class="panel-title">Policy List</h3>
                    <h3><asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label></h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gv_report" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9">
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
                                    <asp:Label ID="lblPolNo" runat="server" Text='<%#Eval("Policy_Number") %>'></asp:Label>
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
                                    <asp:Label ID="lblInsPlan" runat="server" Text='<%#Eval("Product_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sum Insure">
                                <ItemTemplate>
                                    <asp:Label ID="lblSA" runat="server" Text='<%# Eval("Sum_Insure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("Payment_Mode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Basic Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremium" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalPrem" runat="server" Text='<%# Eval("Total_Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Effective Date" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#  Convert.ToDateTime(Eval("Effective_Date")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Effective_Date")).ToString("dd-MM-yyyy") : " - "  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Issue Date" Visible="true" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Issue_Date")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Issue_Date")).ToString("dd-MM-yyyy") : " - "%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Start Date" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" Text='<%#  Convert.ToDateTime(Eval("Start_Date")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Start_Date")).ToString("dd-MM-yyyy") : " - "  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="End Date" Visible="true" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Convert.ToDateTime(Eval("End_Date")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("End_Date")).ToString("dd-MM-yyyy") : " - "%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pay Year" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayYear" runat="server" Text='<%# Eval("Pay_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Policy Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyYear" runat="server" Text='<%# Eval("Policy_Year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Last Due Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDueDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Due_Date")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Due_Date")).ToString("dd-MM-yyyy") : " - "%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Next Due Date" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNextDueDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Next_Due")) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(Eval("Next_Due")).ToString("dd-MM-yyyy") : " - "%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Bank">
                                <ItemTemplate>
                                    <asp:Label ID="lbChannelName" runat="server" Text='<%# Eval("Channel_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate runat="server">
                                    <asp:Button ID="btnEDITRemark" Visible="false" runat="server" Text="EDIT" class="btn btn-info" onclick='<%# "EDITStatusRemark(\"" + Eval("Policy_Number" ) + "\", \"" + Eval("Policy_Year" ) + "\", \"" + Eval("Pay_Year" ) + "\"); return false;" %>'/> 
                                    <asp:Button ID="btnADDRemark" Visible="true" runat="server" Text="ADD" class="btn btn-primary" onclick='<%# "INSERTStatusRemark(\"" + Eval("Policy_Number" ) + "\", \"" + Eval("Policy_Year" ) + "\", \"" + Eval("Pay_Year" ) + "\"); return false;" %>'/> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkbx" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Number" ) + "\", \"" + Eval("Payment_Mode" ) + "\", \"" + Eval("Gender" ) + "\");" %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>

                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                        <%--<PagerSettings PageButtonCount="15" />--%>

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


