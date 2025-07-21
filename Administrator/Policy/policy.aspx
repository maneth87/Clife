<%@ Page Title="Clife | Administrator => Policy" Language="C#" MasterPageFile="~/Administrator/Admin.master" AutoEventWireup="true" CodeFile="policy.aspx.cs" Inherits="Administrator_Policy_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <script src="../../Scripts/print.js"></script>

    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" OnClick="ImgBtnRefresh_Click" ImageUrl="~/App_Themes/functions/refresh.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImageBtnSearch" runat="server" data-toggle="modal" data-target="#mySearchPolicy" ImageUrl="~/App_Themes/functions/search.png" />


        </li>
        <li>
            
            <a data-toggle="dropdown">
                <input type="button" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 89px;" /></a>
            <ul class="dropdown-menu">
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditPersonalDetails();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Personal Details</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditAddressContact();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Address & Contact</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditPremiumDiscount();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Premium Discount</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditBeneficiaries();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Beneficiaries</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditInsurancePlan();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Insurance Plan</a>
                </li>

            </ul>

        </li>
        





    </ul>

    <script type="text/javascript">

        //Do operation when a checkbox is checked
        function SelectSingleCheckBox(ckb, policy_id, product_id, gender, customer_id) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");

                }

            }

            if (ckb.checked) {

                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#f5f5f5");

                //Bind value to Hidden Fields    
                $('#Main_hdfProductID').val(product_id);
                $('#Main_hdfPolicyID').val(policy_id);
                $('#Main_hdfCustomerID').val(customer_id);

             

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");
            }

        }

        //Check whether user make any selection       
        function CheckSelection() {

            var GridView = document.getElementById('<%= gvPolicy.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    $('#myEditPolicy').modal('show');
                    return;
                }
            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }

        //Personal detail section
        function ShowEditPersonalDetails() {
            if ($('#Main_hdfPolicyID').val() != "") {

                PopulateEditPersonalDetailsModal($('#Main_hdfPolicyID').val());
                $('#myEditPersonalDetailsModal').modal('show');

            } else {
                alert("No application to edit");
            }
        }

        //Get customer by policy id
        function PopulateEditPersonalDetailsModal(policy_id) {            
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetCustomerByPolicyID",
                data: "{policy_id:'" + policy_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_ddlIDTypeEdit').val(data.d.ID_Type);
                    $('#Main_txtIDNoEdit').val(data.d.ID_Card);
                    $('#Main_txtSurnameKhEdit').val(data.d.Khmer_Last_Name);
                    $('#Main_txtFirstNameKhEdit').val(data.d.Khmer_First_Name);
                    $('#Main_txtSurnameEdit').val(data.d.Last_Name);
                    $('#Main_txtFirstNameEdit').val(data.d.First_Name);
                    $('#Main_txtFatherSurnameEdit').val(data.d.Father_Last_Name);
                    $('#Main_txtFatherFirstNameEdit').val(data.d.Father_First_Name);
                    $('#Main_txtMotherSurnameEdit').val(data.d.Mother_Last_Name);
                    $('#Main_txtMotherFirstNameEdit').val(data.d.Mother_First_Name);
                    $('#Main_txtPreviousSurnameEdit').val(data.d.Prior_Last_Name);
                    $('#Main_txtPreviousFirstNameEdit').val(data.d.Prior_First_Name);
                    $('#Main_ddlNationalityEdit').val(data.d.Country_ID);                   

                }

            });
        }

        //Edit customer by customer id
        function EditPersonalDetails() {
            var customer_id = $('#Main_hdfCustomerID').val();
            var id_type = $('#Main_ddlIDTypeEdit option:selected').val();
            var id_no = $('#Main_txtIDNoEdit').val();
            var surname_kh = $('#Main_txtSurnameKhEdit').val();
            var first_name_kh = $('#Main_txtFirstNameKhEdit').val();
            var surname_en = $('#Main_txtSurnameEdit').val();
            var first_name_en = $('#Main_txtFirstNameEdit').val();
            var father_surname = $('#Main_txtFatherSurnameEdit').val();
            var father_first_name = $('#Main_txtFatherFirstNameEdit').val();
            var mother_surname = $('#Main_txtMotherSurnameEdit').val();
            var mother_first_name = $('#Main_txtMotherFirstNameEdit').val();
            var previous_surname = $('#Main_txtPreviousFirstNameEdit').val();
            var previous_first_name = $('#Main_txtPreviousFirstNameEdit').val();
            var nationality = $('#Main_ddlNationalityEdit option:selected').val();                       

            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/EditPersonalDetails",
                data: "{customer_id:'" + customer_id + "',id_type:'" + id_type + "',id_no:'" + id_no + "',surname_kh:'" + surname_kh + "',first_name_kh:'" + first_name_kh + "',surname_en:'" + surname_en + "',first_name_en:'" + first_name_en + "',father_surname:'" + father_surname + "',father_first_name:'" + father_first_name + "',mother_surname:'" + mother_surname + "',mother_first_name:'" + mother_first_name + "',previous_surname:'" + previous_surname + "',previous_first_name:'" + previous_first_name + "',nationality:'" + nationality + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d === "0") {
                        alert("Edit Personal Details failed. Please try again.");

                    } else {
                        alert("Edit Personal Details successful.");
                        
                        $('#myEditPersonalDetailsModal').modal('hide');

                        

                    }
                }

            });

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Policy</h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvPolicy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" DataSourceID="PolicyDataSource" Width="100%" HorizontalAlign="Center" OnRowDataBound="gvPolicy_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Customer_ID" ) + "\");" %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Policy_Number" HeaderText="Policy no." SortExpression="Policy_Number" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Customer_ID" HeaderText="Customer ID" SortExpression="Customer_ID" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="App_Number" HeaderText="Application no." SortExpression="App_Number" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Last_Name" HeaderText="Surname" SortExpression="Last_Name">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="First_Name" HeaderText="First name" SortExpression="First_Name">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Age_Insure" HeaderText="Age" SortExpression="Age_Insure" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                                <HeaderStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Product_ID" HeaderText="Product" SortExpression="Product_ID" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                 <HeaderStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sum_Insure" HeaderText="Sum Insure" SortExpression="Sum_Insure" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Rounded_Amount" HeaderText="Annual Premium" SortExpression="Rounded_Amount" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <%--Call function to get total premium = (system + extra premium) - discount--%>

                            <asp:TemplateField HeaderText="Extra Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# GetExtraPremium(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# GetDiscount(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# GetTotalPremium(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString(), DataBinder.Eval(Container.DataItem, "Premium").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="Premium" HeaderText="Premium" SortExpression="Premium" DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Right" >                             
                             <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                             </asp:BoundField>--%>

                            <asp:BoundField DataField="Issue_Date" HeaderText="Issue Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Issue_Date" ItemStyle-HorizontalAlign="Center">

                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Effective_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Maturity_Date" HeaderText="Maturity Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Maturity_Date" ItemStyle-HorizontalAlign="Center">

                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>


                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="PolicyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Cv_Basic_Policy WHERE Policy_Status_Type_ID = 'IF' ORDER BY Policy_Number DESC"></asp:SqlDataSource>
                </div>
            </div>



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
            <!--End Modal Search Application-->


            <!-- Modal Edit Policy -->
            <div id="myEditPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalEditPolicyHeader" aria-hidden="true">
                 <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Edit Policy</h3>
                </div>
                <div class="modal-body">
                    <br /><br />
                   
                </div>
                <div class="modal-footer">
                    <%--<input type="button" class="btn btn-primary" style="height: 27px;" data-dismiss="modal" value="Search"  />--%>
                    <asp:Button ID="btnSaveEdit" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Update" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Edit Policy-->


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


            <!-- Modal Edit Personal Details -->
            <div id="myEditPersonalDetailsModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditPersonalDetailsApplicationHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H5">Edit Customer Personal Details</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">I.D. Type:</td>
                            <td style="vertical-align: bottom">
                                <asp:DropDownList ID="ddlIDTypeEdit" runat="server" class="span2">
                                    <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                    <asp:ListItem Value="1">Passport</asp:ListItem>
                                    <asp:ListItem Value="2">Visa</asp:ListItem>
                                    <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;">I.D. No:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtIDNoEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">Surname in Khmer:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtSurnameKhEdit" Width="90%" Height="25px" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;">First Name in Khmer:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtFirstNameKhEdit" Width="90%" Height="25px" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">Surname:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;">First Name:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">Surname of Father:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtFatherSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;">First Name of Father:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtFatherFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">Surname of Mother:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtMotherSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;">First Name of Mother:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtMotherFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">Previous Surname:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtPreviousSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;">Previouse First Name:</td>
                            <td style="vertical-align: bottom">
                                <asp:TextBox ID="txtPreviousFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; width: 120px; text-align: right;">Nationality:</td>
                            <td style="vertical-align: bottom">
                                <asp:DropDownList ID="ddlNationalityEdit" class="span2" runat="server" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: middle; width: 130px; text-align: right;"></td>
                            <td style="vertical-align: bottom"></td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">
                    <%--<asp:Button ID="btnUpdatePersonalDetails" class="btn btn-primary" style="height: 27px;" runat="server" Text="Update" OnClick="btnUpdatePersonalDetails_Click" />--%>
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditPersonalDetails();" value="Update" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>

                </div>
            </div>
    <!--End Modal Edit Personal Details-->
            
            <%--Data source--%>
            <asp:SqlDataSource ID="SqlDataSourceNationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Nationality "></asp:SqlDataSource>

            <%--Hidden fields--%>
            <asp:HiddenField Id="hdfPolicyID" runat="server"></asp:HiddenField>
            <asp:HiddenField Id="hdfProductID" runat="server"></asp:HiddenField>
            <asp:HiddenField Id="hdfCustomerID" runat="server"></asp:HiddenField>
            

        </ContentTemplate>

         <Triggers>
           <asp:PostBackTrigger ControlID="btnSearchPolicy" />
        </Triggers>

        
        

    </asp:UpdatePanel>

</asp:Content>

