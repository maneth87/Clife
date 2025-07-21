<%@ Page Title="Clife | Reports => Policy Status" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="update_policy_status.aspx.cs" Inherits="Pages_PolicyPayment_update_policy_status" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <ul class="toolbar">
         <li>
           <input type="button"  onclick="Print_default();" style="background: url('../../App_Themes/functions/print.png') 
			        no-repeat; border: none; height: 40px; width: 90px;" />			
        </li>
        <li> 
            <asp:ImageButton ID="ImgeBtnEdit"  runat="server" data-toggle="modal" data-target="#myModalPolicy"  ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
    </ul>

    <%--Javascript Section--%>
    <script type="text/javascript">
        //Fucntion to check only one checkbox

        var Policy_Status_Type_ID_Old;
        var due_date;

        function SelectSingleCheckBox(ckb, Policy_Status_Type_ID, Policy_ID, due) {

            Policy_Status_Type_ID_Old = Policy_Status_Type_ID;
            due_date = due;

            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                }
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

                $('#Main_ddlPolicyStatus').val(Policy_Status_Type_ID);
                $('#Main_hdfPolicy_ID').val(Policy_ID);
                
            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

            }
        }

        function Check_Status() {

            // If LAP, change to IF without payment
            var current_status = $('#Main_ddlPolicyStatus').val();
            //var due_date_test = due_date;

            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/Check_Status",
                data: "{due_date:'" + due_date + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d == "") {

                        onclick_ok_edit();
                    }
                    else {
                        alert("Please do the process payment first");
                    }
                },
                error: function (msg) {

                    alert(msg);
                }
            });
   
            // End of process
       
        }
    
        function onclick_ok_edit() {

            var btnEdit = document.getElementById('<%= btnEdit.ClientID %>'); //dynamically click button

            btnEdit.click();
        }

        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

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

    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.print.js"></script>

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<%-- here is the block code--%>

            <!-- Modal Update Policy Status Form -->
            <div id="myModalPolicy"  class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalPolicyHeader" aria-hidden="true" >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 id="myModalPolicyHeader">Update Policy Status Form</h3>
            </div>
            <div class="modal-body">
                <!---Modal Body--->
                <table  style="text-align:left; width:100px;">
                    <tr>
                        <td>Status:</td>
                        <td style="width:50px;">
                             <asp:DropDownList ID="ddlPolicyStatus" width="363px" height="25px" runat="server" TabIndex="6" AppendDataBoundItems="True" DataSourceID="SqlDataSourcePolicyStatus" DataTextField="Policy_Status_Code" DataValueField="Policy_Status_Type_ID">
                                <asp:ListItem>.</asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Note:</td>
                        <td>
                            <asp:TextBox ID="txtNote"  runat="server" MaxLength="49" TabIndex="2" Width="340px"  Height="30px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtNote" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtNote" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-footer">   
              <div style="display: none;">
                  <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click"  ValidationGroup="1" />
              </div>
                     
              <input type="button" onclick="Check_Status()" id="btnEdit_first" class="btn btn-primary" style="height:27px; width:72px;" runat="server" value="Update" validationgroup="1" />

              <button class="btn"  data-dismiss="modal" aria-hidden="true">Cancel</button>
            </div>
            </div>
            <!--End Modal Update Policy Status Form--> 

            <!-- Modal Search -->
            <div id="myModalSearch" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Policy List</h3>
                </div>

                <div class="modal-body">
                    <!---Modal Body--->
                    <table  text-align: left;">
                        <tr>
                            <td>From Date:</td>
                            <td>
                                <asp:TextBox ID="txtFrom_date"  runat="server" CssClass="datepicker span2" width="136px" TabIndex="1" ></asp:TextBox>
                           </td>
                            <td>To Date:</td>
                            <td>
                                <asp:TextBox ID="txtTo_date"  runat="server"  CssClass="datepicker span2" width="136px" TabIndex="2" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Last Name:</td>
                            <td>
                                  <asp:TextBox ID="txtLastName_Search"  runat="server" width="136px" TabIndex="3" ></asp:TextBox>
                            </td>
                                <td>First Name:</td>
                            <td>
                               <asp:TextBox ID="txtFirstName_Search"  runat="server"  width="136px" TabIndex="4" ></asp:TextBox> 
                            </td>
                        </tr>
                        <tr>
                           <td> Policy Num:</td>
                            <td>
                                 <asp:TextBox ID="txtPolicyNum"  runat="server" width="136px" TabIndex="5" ></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnSearch" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnSearch_Click"  />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" >Cancel</button>
                </div>
            </div>
            <!-- End of Modal Search -->

            <br /><br /><br />

             <div class="panel panel-default" >
                <div class="panel-heading">
                    <h3 class="panel-title">Policy Status</h3>
                </div>

                 <asp:HiddenField ID="hdfPolicy_ID" runat="server" />

                <div class="panel-body">
                    <div class="PrintPD"> 
                    <%--Grid View--%>             
                    <%--DataSourceID="SqlDataSourcePolicy--%>    
                       
                    <asp:GridView ID="GvOPolicy" CssClass="grid-layout" Width="100%" runat="server" AutoGenerateColumns="False" >
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_Status_Type_ID" ) + "\", \"" + Eval("Policy_ID" ) + "\", \"" + Eval("Due_Date" ) + "\");" %>' />
                                   <%-- <asp:HiddenField ID="hdfPolicy_ID" runat="server" Value='<%# Bind("Policy_ID")%>' />--%>
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="No" HeaderText="No" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Policy_Number" HeaderText="Policy No" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Last_Name" HeaderText="Last Name"  >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" Width="100px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="First_Name" HeaderText="First Name" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" Width="100px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Gender" HeaderText="Gender" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"   Width="50px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Mode" HeaderText="Mode" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="55px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sum_Insure" HeaderText="Sum Insure" DataFormatString="${0:C2}" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="${0:C2}" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" Width="90px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Due_Date" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Due Date" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="75px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Next_Due" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Next Due" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="75px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Policy_ID" HeaderText="policy id" Visible="false"/>
                            <asp:BoundField DataField="En_Abbr" HeaderText="Product"  >
                            <ItemStyle HorizontalAlign="Left" Width="150px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Policy_Status_Type_ID" HeaderText="Status" >
                            <ItemStyle HorizontalAlign="Center" Width="50px"/>
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" />
                        <RowStyle HorizontalAlign="Left" />
                    </asp:GridView>
                         </div>
                </div>
            </div>
         
             <asp:Label ID="Label1" runat="server" Text="Total Policy:" Font-Bold="true"></asp:Label> <asp:Label ID="lbltotal" runat="server" Text="Label" Font-Bold="true" ForeColor="Red"></asp:Label>
            <%--<asp:SqlDataSource ID="SqlDataSourcePolicy" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Policy_List]" ></asp:SqlDataSource>--%>
            <asp:SqlDataSource ID="SqlDataSourcePolicyStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Policy_Status_Code]" ></asp:SqlDataSource>
            
	</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEdit" />
             <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>

    </asp:UpdatePanel>	
</asp:Content>
