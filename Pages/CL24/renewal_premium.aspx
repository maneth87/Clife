<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/Content.master" CodeFile="renewal_premium.aspx.cs" Inherits="Pages_CL24_renewal_premium" %>

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
            <asp:ImageButton ID="ImgBtnSave" runat="server" Enabled="False"  Visible="true" ImageUrl="~/App_Themes/functions/save.png" OnClick="ImgBtnSave_Click" />
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

          function INSERTRemark(policy_number) {
              $("#Main_txtINSERTPolicyNumber").val("");
              $("#Main_hdfINSERTPolicyNumber").val("");
              $("#Main_txtINSERTRemarks").val("");

              $("#Main_txtINSERTPolicyNumber").val(policy_number);
              $("#Main_hdfINSERTPolicyNumber").val(policy_number);
              
              $("#ModalINSERTRemarkPolicy").modal("show");
          }

          function EDITRemark(policy_number) {
              
              $("#Main_txtEDITPolicyNumber").val("");
              $("#Main_hdfEDITPolicyNumber").val("");
              $("#Main_txtEDITRemarks").val("");

              $.ajax({
                  type: "POST",
                  url: "../../PolicyWebService.asmx/EDITRemark",
                  data: "{policy_number:'" + policy_number + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (data) {
                      var remark = data.d;
                      $("#Main_txtEDITPolicyNumber").val(policy_number);
                      $('#Main_txtEDITRemarks').val(remark);
                      $("#Main_hdfEDITPolicyNumber").val(policy_number);
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
      
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">CL24 : Renewal Premium</h3>
                </div>
                <div class="panel-body">
                    <table>
                        <tr>
                            <td style="width: 20%;"><b>Month</b>:</td>
                            <td>
                                <asp:DropDownList ID="ddlNextDueMonth" runat="server" Style="width: 178px;"></asp:DropDownList>
                            </td>
                            <td style="width: 12%;"><b>Year</b>:</td>
                            <td>
                                <asp:DropDownList ID="ddlNextDueYear" runat="server" Style="width: 170px;"></asp:DropDownList>
                            </td> 
                            <td>  </td> 
                            <td>
                                <asp:Button ID="btnGenerate" class="btn btn-success" runat="server" Style="height: 27px;" Text="Generate" onClick="btnGenerate_Click"  />
                            </td>
                        </tr>
                        <tr>
                            <td> </td>
                            <td> </td>
                            <td> </td>
                            <td>
                                <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                     
                    </table>
                    <br />

                </div>
            </div>      

            <%--Modal Add Remark Policy--%>
            <div id="ModalINSERTRemarkPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="ModalINSERTRemarkPolicy" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H1">Insert Remarks Of Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <div class="tab-content" style="height: 115px; overflow: hidden;">
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
                    <h3 id="H3">EDIT Remarks Of Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <div class="tab-content" style="height: 115px; overflow: hidden;">
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
                    <table>
                        <tr>
                            <td style="width: 20%;"><b>Report Date</b>:</td>
                            <td>
                                <asp:TextBox ID="txtReport_date" runat="server" PlaceHolder="DD/MM/YYYY" CssClass="datepicker" width="55%" TabIndex="4" ></asp:TextBox>
                                <span class="required">*</span>
                            </td>
                        </tr>
                    </table>
            
            <div id="dvReport" width="98%">        
                    <div class="panel panel-default" id="POLICY_REPORT" runat="server">
                <div class="panel-heading">
                    <h3 class="panel-title"> Policy Renewal Premium</h3>
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
                                    <asp:Button ID="btnEDITRemark" Visible="false" runat="server" Text="EDIT" class="btn btn-info" OnClientClick='<%# String.Format("EDITRemark(\"{0}\");return false;", Eval("Policy_Number")) %>'/> 
                                    <asp:Button ID="btnADDRemark" Visible="true" runat="server" Text="ADD" class="btn btn-primary" OnClientClick='<%# String.Format("INSERTRemark(\"{0}\");return false;", Eval("Policy_Number")) %>'/> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkbx" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Number" ) + "\", \"" + Eval("Payment_Mode" ) + "\", \"" + Eval("Gender" ) + "\");" %>' Visible='<%# (Convert.ToInt32(Eval("IsActive")) == 1)? true: false %>' />
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


