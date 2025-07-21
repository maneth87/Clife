<%@ Page Title="Clife | Core Data => Sales Agent" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="sale_agent.aspx.cs" Inherits="Pages_CoreData_sale_agent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    
    <ul class="toolbar">
        <%--<li>
            <asp:ImageButton ID="ImgBCancel" runat="server" data-toggle="modal" data-target="#myModalCancelSaleAgent" ImageUrl="~/App_Themes/functions/cancel.png" />
        </li>--%>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" runat="server" data-toggle="modal" data-target="#myModalNewSearchSaleAgentCode" ImageUrl="~/App_Themes/functions/search.png" />
           <%-- <div style="display: none;">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click"  />
            </div>
            <input id="Button" type="button" runat="server" onclick="onclick_search()" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />--%>
        </li>
        <li>
            <%--<asp:ImageButton ID="ImagBDelete" runat="server" data-toggle="modal" data-target="#myModalDeleteSaleAgent" ImageUrl="~/App_Themes/functions/delete.png" />--%>
             <input type="button" onclick="show_delete_form();"  style="background: url('../../App_Themes/functions/delete.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <%--<asp:ImageButton ID="ImgBtnEdit" runat="server"  data-toggle="modal" data-target="#myModalEditSaleAgentType" ImageUrl="~/App_Themes/functions/edit.png"/>--%>
            <%--<asp:ImageButton ID="ImgBtnEdit" runat="server" OnClientClick="show_edit_form();"  ImageUrl="~/App_Themes/functions/edit.png"/>--%>
            <input type="button" onclick="show_edit_form();"  style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
             <asp:ImageButton ID="ImgBtnAdd" data-toggle="modal" data-target="#myModalSaleAgentType" runat="server" ImageUrl="~/App_Themes/functions/add.png" OnClick="ImgBtnAdd_Click" />
        </li>
       
       
    </ul>
     
     <%--Javascript Section--%>
    <script type="text/javascript">

        function
            GetSaleAgent_Code() {
           
            var Sale_Agent_Type = $('#Main_ddlSaleAgentType').val();
            //var ID_Card = $('#Main_txtIDNo').val();
            //var Sale_Agent_ID = $('#Main_txtSaleAgentID').val();
            var ID_Card = '';// $('#Main_txtIDNo').val();
            var Sale_Agent_ID = ''// $('#Main_txtSaleAgentID').val();

            //check sale agent type
            if (Sale_Agent_Type == 0)// ordinary
            {
                ID_Card = $('#<%= txtIDNo.ClientID%>').val();
                Sale_Agent_ID = $('#<%=txtSaleAgentID.ClientID%>').val();
            }
            else if (Sale_Agent_Type == 1)// bank
            {
                ID_Card = '';
                Sale_Agent_ID = $('#<%= txtBankSaleID.ClientID%>').val();
            }
            else if (Sale_Agent_Type == 2)//broker
            {
                ID_Card = '';
                Sale_Agent_ID = $('#<%=txtBrokerSaleID.ClientID%>').val();
            }

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetSaleAgents_Code",
                data: "{Sale_Agent_ID:'" + Sale_Agent_ID + "',Sale_Agent_Type:'" + Sale_Agent_Type + "',ID_Card:'" + ID_Card + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != "") {
                        alert(data.d);
                    }
                    else {
                        onclick_ordinary();
                    }
                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function GetSaleAgentID_Card() {
            var ID_Card = $('#Main_txtEditIDNo').val();
            var Sale_Agent_ID = $('#Main_hdfEditSaleID').val();
            var Sale_Agent_Type = $('#<%=ddlEditSaleAgentType.ClientID%>').val();
            
            //if agent type is ordinary need to check id card 
            if (Sale_Agent_Type == 0) {
                $.ajax({
                    type: "POST",
                    url: "../../SaleAgentWebService.asmx/GetSaleAgents_Code_Edit",
                    data: "{Sale_Agent_ID:'" + Sale_Agent_ID + "',ID_Card:'" + ID_Card + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d == true) {
                            alert('ID Card: (' + ID_Card + ') has already exist, please check it again.');
                        }
                        else {

                            onclick_ordinary();
                        }
                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
            }
            else {
                onclick_ordinary();
            }
        }

        var record_is_selected = false;
        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, Sale_Agent_ID, Sale_Agent_Type, ID_Card, ID_Type, First_Name, Last_Name, Khmer_First_Name, Khmer_Last_Name, Country_ID, Gender, Birth_Date, Phone_no, Mobile_no, Fax_no, Email, Full_Name, Khmer_Full_Name, Created_Note) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                }
            }

            if (ckb.checked) {
                record_is_selected = true;
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e0e0e0");

                /// Set text to textboxes for edition
                if (Sale_Agent_Type == 0) {

                    $('#EditSaleBroker').hide();
                    $('#EditSaleAgentBank').hide();
                    $('#EditSaleAgentOrdinary').show();

                    $('#Main_btnEditBankAgent_first').hide();
                    $('#Main_btnEditBroker_first').hide();
                    $('#Main_btnEdit_first').show();

                    $('#Main_ddlEditSaleAgentType').val(Sale_Agent_Type);
                    $('#Main_hdfDDLEditSaleType').val(Sale_Agent_Type);

                    $('#Main_txtEditSaleAgentID').val(Sale_Agent_ID);
                    $('#Main_txtEditIDNo').val(ID_Card);
                    $('#Main_ddlEditIDType').val(ID_Type);
                    $('#Main_txtEditLastName').val(Last_Name);
                    $('#Main_txtEditFirstName').val(First_Name);
                    $('#Main_txtEditKhmerLastName').val(Khmer_Last_Name);
                    $('#Main_txtEditKhmerFirstName').val(Khmer_First_Name);
                    $('#Main_ddlEditNationality').val(Country_ID);
                    $('#Main_ddlEditSex').val(Gender);
                    $('#Main_txtEditBirth_Date').val(Birth_Date);
                    $('#Main_txtEditPhoneNo').val(Phone_no);
                    $('#Main_txtEditMobileNo').val(Mobile_no);
                    $('#Main_txtEditFaxNo').val(Fax_no);
                    $('#Main_txtEditEmail').val(Email);
                    $('#Main_txtEditNote').val(Created_Note);

                    $('#Main_hdfEditSaleID').val(Sale_Agent_ID);

                    /// Delete 
                    $('#Main_txtDeleteSaleType').val("Ordinary Agent");
                    $('#Main_hdfDeleteSaleAgent').val(Sale_Agent_Type);

                    $('#DeleteSaleAgentBroker').hide();
                    $('#DeleteSaleAgentBank').hide();
                    $('#DeleteSaleAgentOrdinary').show();

                    $('#Main_txtDeleteSaleID').val(Sale_Agent_ID + ' ?');
                    $('#Main_hdfDeleteSaleID').val(Sale_Agent_ID);

                    /// Cancel Sale Agent ID
                    $('#Main_txtCancelSaleType').val("Ordinary Agent");
                    $('#Main_hdfCancelSaleType').val(Sale_Agent_Type);

                    $('#Main_txtCancelSaleID').val(Sale_Agent_ID + ' ?');
                    $('#Main_hdfCancelSaleID').val(Sale_Agent_ID);

                    $('#Main_txtCancelNote').val("");
                }
                else if (Sale_Agent_Type == 1) {
                    $('#EditSaleBroker').hide();
                    $('#EditSaleAgentBank').show();
                    $('#EditSaleAgentOrdinary').hide();

                    $('#Main_btnEdit_first').hide();
                    $('#Main_btnEditBankAgent_first').show();
                    $('#Main_btnEditBroker_first').hide();

                    $('#Main_ddlEditSaleAgentType').val(Sale_Agent_Type);
                    $('#Main_hdfDDLEditSaleType').val(Sale_Agent_Type);

                    $('#Main_txtEditBankSaleID').val(Sale_Agent_ID);
                    $('#Main_txtEditBankFullName').val(Full_Name);
                    $('#<%=txtKhmerEditBankFullName.ClientID%>').val(Khmer_Full_Name);
                    $('#Main_txtEditBankPhone').val(Phone_no);
                    $('#Main_txtEditBankMobile').val(Mobile_no);
                    $('#Main_txtEditBankFax').val(Fax_no);
                    $('#Main_txtEditBankEmail').val(Email);
                    $('#Main_txtEditBankNote').val(Created_Note);

                    $('#Main_hdfEditBankSaleID').val(Sale_Agent_ID);

                    /// Delete 
                    $('#Main_txtDeleteSaleType').val("Bank Agent");
                    $('#Main_hdfDeleteSaleAgent').val(Sale_Agent_Type);

                    $('#DeleteSaleAgentBroker').hide();
                    $('#DeleteSaleAgentBank').show();
                    $('#DeleteSaleAgentOrdinary').hide();

                    $('#Main_txtDeleteSaleIDBank').val(Sale_Agent_ID + ' ?');
                    $('#Main_hdfDeleteSaleIDBank').val(Sale_Agent_ID);

                    /// Cancel
                    $('#Main_txtCancelSaleType').val("Bank Agent");
                    $('#Main_hdfCancelSaleType').val(Sale_Agent_Type);

                    $('#Main_txtCancelSaleID').val(Sale_Agent_ID + ' ?');
                    $('#Main_hdfCancelSaleID').val(Sale_Agent_ID);
                    $('#Main_txtCancelNote').val("");
                }
                else {

                    $('#EditSaleBroker').show();
                    $('#EditSaleAgentBank').hide();
                    $('#EditSaleAgentOrdinary').hide();

                    $('#Main_btnEdit_first').hide();
                    $('#Main_btnEditBankAgent_first').hide();
                    $('#Main_btnEditBroker_first').show();

                    $('#Main_ddlEditSaleAgentType').val(Sale_Agent_Type);
                    $('#Main_hdfDDLEditSaleType').val(Sale_Agent_Type);

                    $('#Main_txtEditBrokerSaleID').val(Sale_Agent_ID);
                    $('#Main_txtEditBrokerFullName').val(Full_Name);
                    $('#<%=txtKhmerEditBrokerFullName.ClientID%>').val(Khmer_Full_Name);
                    $('#Main_txtEditBrokerPhone').val(Phone_no);
                    $('#Main_txtEditBrokerMobile').val(Mobile_no);
                    $('#Main_txtEditBrokerFax').val(Fax_no);
                    $('#Main_txtEditBrokerEmail').val(Email);
                    $('#Main_txtEditBrokerNote').val(Created_Note);

                    $('#Main_hdfEditBrokerSaleID').val(Sale_Agent_ID);

                    /// Delete 
                    $('#Main_txtDeleteSaleType').val("Broker Agent");
                    $('#Main_hdfDeleteSaleAgent').val(Sale_Agent_Type);
                    $('#DeleteSaleAgentBroker').show();
                    $('#DeleteSaleAgentBank').hide();
                    $('#DeleteSaleAgentOrdinary').hide();

                    $('#Main_txtDeleteSaleIDBroker').val(Sale_Agent_ID + ' ?');
                    $('#Main_hdfDeleteSaleIDBroker').val(Sale_Agent_ID);

                    /// Cancel
                    $('#Main_txtCancelSaleType').val("Broker Agent");
                    $('#Main_hdfCancelSaleType').val(Sale_Agent_Type);

                    $('#Main_txtCancelSaleID').val(Sale_Agent_ID + ' ?');
                    $('#Main_hdfCancelSaleID').val(Sale_Agent_ID);
                    $('#Main_txtCancelNote').val("");
                }

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                //Remove text from textboxes in edit form
                $('#Main_ddlEditSaleAgentType').val(Sale_Agent_Type);

                $('#Main_txtEditSaleAgentID').val("");
                $('#Main_txtEditIDNo').val("");
                $('#Main_ddlEditIDType').val("");
                $('#Main_txtEditLastName').val("");
                $('#Main_txtEditFirstName').val("");
                $('#Main_ddlEditNationality').val("");
                $('#<%=txtEditKhmerFirstName.ClientID%>').val("");
                $('#<%=txtEditKhmerLastName.ClientID%>').val("");
                $('#Main_ddlEditSex').val("");
                $('#Main_txtEditBirth_Date').val("");
                $('#Main_txtEditPhoneNo').val("");
                $('#Main_txtEditMobileNo').val("");
                $('#Main_txtEditFaxNo').val("");
                $('#Main_txtEditEmail').val("");
                $('#Main_txtEditNote').val("");

                $('#EditSaleAgentOrdinary').hide();
                $('#EditSaleAgentBank').hide();
                $('#EditSaleBroker').hide();

                ClearText();

                $('#Main_btnEdit_first').hide();
                $('#Main_btnEditBankAgent_first').hide();
                $('#Main_btnEditBroker_first').hide();
                record_is_selected = false;
            }
        }

        ///On Dropdownlist change
        function ShowAgentForm() {

            var id_type = $('#Main_ddlSaleAgentType').val();
            if (id_type == 0) {
                $('#mySaleOdinaryAgent').show();
                $('#mySaleBankAgent').hide();
                $('#mySaleBrokerAgent').hide();

                $('#Main_btnOk_first').show();
                $('#Main_btnBankOk_first').hide();
                $('#Main_btnBrokerOk_first').hide();

            }
            else if (id_type == 1) {
                $('#mySaleOdinaryAgent').hide();
                $('#mySaleBankAgent').show();
                $('#mySaleBrokerAgent').hide();

                $('#Main_btnBankOk_first').show();
                $('#Main_btnOk_first').hide();
                $('#Main_btnBrokerOk_first').hide();

            }
            else {
                $('#mySaleOdinaryAgent').hide();
                $('#mySaleBankAgent').hide();
                $('#mySaleBrokerAgent').show();

                $('#Main_btnBankOk_first').hide();
                $('#Main_btnOk_first').hide();
                $('#Main_btnBrokerOk_first').show();

            }
        }

        function show_edit_form()
        {
            if (record_is_selected) {
                $('#myModalEditSaleAgentType').modal('show');
            }
            else {
                alert('Please select record first.');
                return;
            }
        }
        function show_delete_form()
        {
            if (record_is_selected) {
                $('#myModalDeleteSaleAgent').modal('show');
            }
            else {
                alert('Please select record first.');
                return;
            }
        }

        function delete_sale_agent()
        {
            var sale_agent_id = '';

            if ($('#<%=hdfDeleteSaleAgent.ClientID%>').val()=='0')
            {
                sale_agent_id = $('#<%=hdfDeleteSaleID.ClientID%>').val();
            }
            else if ($('#<%=hdfDeleteSaleAgent.ClientID%>').val()=='1')
            {
                sale_agent_id = $('#<%=hdfDeleteSaleIDBank.ClientID%>').val();
            }
            else if ($('#<%=hdfDeleteSaleAgent.ClientID%>').val()=='2')
            {
                sale_agent_id = $('#<%=hdfDeleteSaleIDBroker.ClientID%>').val();
            }

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/DeleteSaleAgent",
                data: "{sale_agent_id:'" + sale_agent_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d == true) {
                        alert('Deleted Successfully.');
                        
                    }
                    else {

                        alert('Deleted Fail.');
                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }
        function ClearText() {

            $('#mySaleOdinaryAgent').show();
            $('#mySaleBankAgent').hide();
            $('#mySaleBrokerAgent').hide();

            $('#Main_ddlSaleAgentType').val(0);

            $('#Main_txtEditSaleAgentID').val("");
            $('#Main_txtEditIDNo').val("");
            $('#Main_ddlEditIDType').val("");
            $('#Main_txtEditLastName').val("");
            $('#Main_txtEditFirstName').val("");
            $('#<%=txtEditKhmerFirstName.ClientID%>').val("");
            $('#<%=txtEditKhmerLastName.ClientID%>').val("");
            $('#Main_ddlEditNationality').val("");
            $('#Main_ddlEditSex').val("");
            $('#Main_txtEditBirth_Date').val("");
            $('#Main_txtEditPhoneNo').val("");
            $('#Main_txtEditMobileNo').val("");
            $('#Main_txtEditFaxNo').val("");
            $('#Main_txtEditEmail').val("");
            $('#Main_txtEditNote').val("");

            $('#Main_btnOk_first').show();
            $('#Main_btnBankOk_frist').hide();
            $('#Main_btnBrokerOk_first').hide();

            $('#Main_btnEdit_first').hide();
            $('#Main_btnEditBankAgent_first').hide();
            $('#Main_btnEditBroker_first').hide();

            //
            $('#Main_txtSaleAgentID').val("");
            $('#Main_txtLastName').val("");
            $('#Main_txtFirstName').val("");
            $('#<%=txtKhmerFirstName.ClientID%>').val("");
            $('#<%=txtKhmerLastName.ClientID%>').val("");
            $('#Main_txtNote').val("");
            $('#Main_txtMobileNo').val("");
            $('#Main_txtPhoneNo').val("");
            $('#Main_txtFaxNo').val("");
            $('#Main_txtEmail').val("");
            $('#Main_txtIDNo').val("");
            $('#Main_txtBirth_Date').val("");
            $('#Main_txtBankSaleID').val("");
            $('#Main_txtBankFullName').val("");
            $('#<%=txtKhmerBankFullName.ClientID%>').val("");
            $('#Main_txtBankNote').val("");
            $('#Main_txtBankMobile').val("");
            $('#Main_txtBankPhone').val("");
            $('#Main_txtBankFax').val("");
            $('#Main_txtBankEmail').val("");
            $('#Main_txtBrokerSaleID').val("");
            $('#Main_txtBrokerFullName').val("");
            $('#<%=txtKhmerBrokerFullName.ClientID%>').val("");
            $('#Main_txtBrokerNote').val("");
            $('#Main_txtBrokerMobile').val("");
            $('#Main_txtBrokerPhone').val("");
            $('#Main_txtBrokerFax').val("");
            $('#Main_txtBrokerEmail').val("");


        }

        function HideButton() {
            //ClearText();
            $('#Main_btnOk_first').show();
            $('#Main_btnBankOk_first').hide();
            $('#Main_btnBrokerOk_first').hide();

            $('#Main_btnEdit_first').hide();
            $('#Main_btnEditBankAgent_first').hide();
            $('#Main_btnEditBroker_first').hide();
        }

        /// Check Enable and Disable text Sale Agent ID
        function Enable_Text_SaleID() {
            alter("test");
            $('#Main_txtSaleAgentID').prop("disabled", false);
        }

        //Validation Section
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        function ValidateTextDecimal(j) {

            var msg = j.value;
            var w;
            var nokta = msg.indexOf(".");
            var ind;

            for (w = 0; w < msg.length; w++) {

                ind = msg.substring(w, w + 1);
                if (ind < "0" || ind > "9") {

                    if (nokta > 0)
                        if (w == nokta) continue;

                    msg = msg.substring(0, w);
                    j.value = msg;
                    break;
                }
            }
        }
        //End Validation

        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        function onclick_ordinary() {
            var btnOk = document.getElementById('<%= btnOk.ClientID %>'); //dynamically click button

            btnOk.click();
        }

        function onclick_edit() {
            var btnEdit = document.getElementById('<%= btnEdit.ClientID%>');

            btnEdit_click();
        }

        /// New search
        function onclick_search() {
            var btnSearch = document.getElementById('<%= btnSearch.ClientID %>');

            btnSearch.click();
        }

        function onclick_search_by_name() {
            var btnSearch_Name = document.getElementById('<%= btnSearch_Name.ClientID %>');
            btnSearch_Name.click();
        }

        function clear_sale_code() {
            $('#Main_txtSearchSaleAgentCode').val("");
        }

        function clear_sale_name() {
            $('#Main_txtFirstNameSearch').val("");
            $('#Main_txtLastNameSearch').val("");
            $('#<% =txtKhmerFirstName.ClientID%>').val("");
            $('#<%=txtKhmerLastName.ClientID%>').val("");
        }


 </script>

    <style>
        .grid {
         padding:10px 15px 5px 5px;
        }
   </style>
 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
     <script src="../../Scripts/jquery-jtemplates.js"></script>

    <%-- Form Design Section--%>
    <br />
    <br />
    <br />

    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--all operation here--%>

            <!-- Modal Add New Sale Agent Type Form -->
            <div id="myModalSaleAgentType" class="modal hide fade" tabindex="-1" role="dialog" onshow="HideButton();" aria-labelledby="myModalSaleAgentTypeHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalSaleAgentTypeHeader">Sale Agent</h3>
                </div>
                <div  class="modal-body" >
                    <!---Sale Agent--->
                    <table id="SaleOrdinaryAgent"  width="100%">
                        <tr>
                            <td width="2%"> </td>
                            <td width="20">Sale Agent Type</td>
                            <td width="76%">
                                <asp:DropDownList ID="ddlSaleAgentType" width="100%"   runat="server" onchange="ShowAgentForm();" >
                                <asp:ListItem Selected="True" Value="0" Text="Ordinary Agent" >Ordinary Agent</asp:ListItem>
                                <asp:ListItem Value="1" Text="Bank Agent">Bank Agent</asp:ListItem>
                                 <asp:ListItem Value="2" Text="Broker Agent">Broker Agent</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2"><hr style="border-style:double; border-bottom-color:gray" /> </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2">
                                <!---Sale Ordinary Agent--->
                                <table id="mySaleOdinaryAgent" width="100%" >
                                  <tr>
                                        <td>Sale Agent Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtSaleAgentID" runat="server" MaxLength="254" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorSaleAgentID" runat="server" ControlToValidate="txtSaleAgentID" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfSaleAgentID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>ID. No:</td>
                                        <td>
                                            <asp:TextBox ID="txtIDNo" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorIDNo" runat="server" ControlToValidate="txtIDNo" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>ID. Type:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlIDType" class="span2" runat="server" TabIndex="3" width="55%">
                                            <asp:ListItem Value="1">ID Card</asp:ListItem>
                                            <asp:ListItem Value="2">Passport</asp:ListItem>
                                            <asp:ListItem Value="3">Visa</asp:ListItem>
                                            <asp:ListItem Value="4">Certificate Birth</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="width:200px;">Khmer Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtKhmerLastName" runat="server" MaxLength="254" TabIndex="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtKhmerLastName" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Khmer First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtKhmerFirstName" runat="server" MaxLength="254" TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtKhmerFirstName" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="254" TabIndex="6"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorLastName" runat="server" ControlToValidate="txtLastName" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorFirstName" runat="server" ControlToValidate="txtFirstName" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlNationality" width="55%" height="25px" runat="server" TabIndex="8" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                <asp:ListItem>.</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Sex:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlSex" class="span2" runat="server" TabIndex="9">
                                            <asp:ListItem Value="1">Male</asp:ListItem>
                                            <asp:ListItem Value="0">Female</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Birth of Date:</td>
                                        <td>
                                              <asp:TextBox ID="txtBirth_Date"  runat="server"  CssClass="datepicker span2" TabIndex="10" ></asp:TextBox>&nbsp;(DD-MM-YYYY)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Phone No:</td>
                                         <td><asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="254" TabIndex="11"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Mobile No:</td>
                                         <td><asp:TextBox ID="txtMobileNo" runat="server" MaxLength="254" TabIndex="12"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Fax No:</td>
                                         <td><asp:TextBox ID="txtFaxNo" runat="server" MaxLength="254" TabIndex="13"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Email:</td>
                                         <td>
                                             <asp:TextBox ID="txtEmail" runat="server" MaxLength="254" TabIndex="14"></asp:TextBox>
                                             <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" ValidationGroup="1"></asp:RegularExpressionValidator>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td>Note:</td>
                                         <td><asp:TextBox ID="txtNote" runat="server" MaxLength="254" TabIndex="15"></asp:TextBox></td>
                                    </tr>

                                </table>
                                <!---Sale Ordinary Agent--->

                                <!---Sale Bank Agent--->
                                <table id="mySaleBankAgent" width="100%" hidden="hidden" >
                                    <tr>
                                        <td>Sale Agent Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankSaleID" runat="server" MaxLength="254" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBankSaleID" runat="server" ControlToValidate="txtBankSaleID" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfBankSaleID" runat="server" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="width:200px;">Khmer Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtKhmerBankFullName" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtKhmerBankFullName" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankFullName" runat="server" MaxLength="254" TabIndex="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBankFullName" runat="server" ControlToValidate="txtBankFullName" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Phone no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankPhone" runat="server" MaxLength="254" TabIndex="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBankPhone" runat="server" ControlToValidate="txtBankPhone" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankMobile" runat="server" MaxLength="254" TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBankMobile" runat="server" ControlToValidate="txtBankMobile" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankFax" runat="server" MaxLength="254" TabIndex="6"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBankFax" runat="server" ControlToValidate="txtBankFax" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Email:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankEmail" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtBankEmail" runat="server" ControlToValidate="txtBankEmail" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" ValidationGroup="4"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Note:</td>
                                        <td>
                                            <asp:TextBox ID="txtBankNote" runat="server" MaxLength="254" TabIndex="8"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBankNote" runat="server" ControlToValidate="txtBankNote" ForeColor="Red" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <!---Sale Bank Agent--->

                                <!---Sale Broker Agent--->
                                <table id="mySaleBrokerAgent" width="100%" hidden="hidden">
                                    <tr>
                                        <td>Sale Agent Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerSaleID" runat="server" MaxLength="254" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBrokderID" runat="server" ControlToValidate="txtBrokerSaleID" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfBrokerSaleID" runat="server" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Khmer Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtKhmerBrokerFullName" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtKhmerBrokerFullName" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerFullName" runat="server" MaxLength="254" TabIndex="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBrokerFullName" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Phone no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerPhone" runat="server" MaxLength="254" TabIndex="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtBrokerPhone" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerMobile" runat="server" MaxLength="254" TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBrokerMobile" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerFax" runat="server" MaxLength="254" TabIndex="6"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBrokerFax" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Email:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerEmail" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorBrokerEmail" runat="server" ControlToValidate="txtBrokerEmail" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" ValidationGroup="5"></asp:RegularExpressionValidator>
                                            
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Note:</td>
                                        <td>
                                            <asp:TextBox ID="txtBrokerNote" runat="server" MaxLength="254" TabIndex="8"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtBrokerNote" ForeColor="Red" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <!---Sale Broker Agent--->
                            </td>
                            <td width="2%"> </td>
                        </tr>
                        
                    </table>
                </div>

                <div class="modal-footer" >

                    <div style="display: none;">
                       <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
                    </div> 

                    <input type="button" onclick="GetSaleAgent_Code()" id="btnOk_first" class="btn btn-primary" style="height: 27px; width:100px" runat="server" value="OK Ordinary" ValidationGroup="1"  />

                    <input type="button" onclick="GetSaleAgent_Code()" id="btnBankOk_first" class="btn btn-primary" hidden="hidden" style="height: 27px;" runat="server" value="OK Bank" ValidationGroup="4"  />

                    <input type="button" onclick="GetSaleAgent_Code()" id="btnBrokerOk_first" class="btn btn-primary" hidden="hidden" style="height: 27px;" runat="server" value="OK Broker" ValidationGroup="5"  />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="ClearText();" >Cancel</button>
                </div>
            </div>
            <!-- End Modal Sale Agent Type Form-->

            <!-- Modal Edit Sale Agent Type Form -->
            <div id="myModalEditSaleAgentType" class="modal hide fade" tabindex="-1" role="dialog"  aria-labelledby="myModalEditSaleAgentTypeHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalEditSaleAgentTypeHeader"> Update Sale Agent</h3>
                </div>
                <div  class="modal-body" >
                    <!---Sale Agent--->
                    <table id="EditSaleAgent"  width="100%">
                        <tr>
                            <td width="2%"> </td>
                            <td width="20">Agent Type</td>
                            <td width="76%">
                                <asp:DropDownList ID="ddlEditSaleAgentType" width="100%"   runat="server" Enabled="False"  >
                                <asp:ListItem Value="0"  Text="Ordinary Agent">Ordinary Agent</asp:ListItem>
                                <asp:ListItem Value="1" Text="Bank Agent">Bank Agent</asp:ListItem>
                                <asp:ListItem Value="2" Text="Bank Agent">Broker Agent</asp:ListItem>
                            </asp:DropDownList> <asp:HiddenField ID="hdfDDLEditSaleType" runat="server"/>
                            </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2"><hr style="border-style:double; border-bottom-color:gray" /> </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2">
                                <!---Sale Ordinary Agent--->
                                <table id="EditSaleAgentOrdinary" width="100%" >
                                    <tr>
                                        <td>Agent Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditSaleAgentID" runat="server" MaxLength="254" TabIndex="1" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEditSaleAgentID" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfEditSaleID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>ID. No:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditIDNo" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEditIDNo" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>ID. Type:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlEditIDType" class="span2" runat="server" TabIndex="3" width="55%">
                                            <asp:ListItem Value="1">ID Card</asp:ListItem>
                                            <asp:ListItem Value="2">Passport</asp:ListItem>
                                            <asp:ListItem Value="3">Visa</asp:ListItem>
                                            <asp:ListItem Value="4">Certificate Birth</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Khmer Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditKhmerLastName" runat="server" MaxLength="254" TabIndex="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtEditKhmerLastName" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditKhmerFirstName" runat="server" MaxLength="254" TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtEditKhmerFirstName" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditLastName" runat="server" MaxLength="254" TabIndex="6"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEditLastName" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditFirstName" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEditFirstName" ForeColor="Red" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlEditNationality" width="55%" height="25px" runat="server" TabIndex="8" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                <asp:ListItem>.</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Sex:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlEditSex" class="span2" runat="server" TabIndex="9">
                                            <asp:ListItem Value="1">Male</asp:ListItem>
                                            <asp:ListItem Value="0">Female</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Birth of Date:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBirth_Date" runat="server" CssClass="datepicker span2" TabIndex="10" ></asp:TextBox>&nbsp;(DD-MM-YYYY)
                                              
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Phone No:</td>
                                         <td><asp:TextBox ID="txtEditPhoneNo" runat="server" MaxLength="254" TabIndex="11"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Mobile No:</td>
                                         <td><asp:TextBox ID="txtEditMobileNo" runat="server" MaxLength="254" TabIndex="12"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Fax No:</td>
                                         <td><asp:TextBox ID="txtEditFaxNo" runat="server" MaxLength="254" TabIndex="13"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Email:</td>
                                         <td><asp:TextBox ID="txtEditEmail" runat="server" MaxLength="254" TabIndex="14"></asp:TextBox></td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEditEmail" ErrorMessage="Invalid email address."  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="2"></asp:RegularExpressionValidator>
                                    </tr>
                                    <tr>
                                        <td>Note:</td>
                                         <td><asp:TextBox ID="txtEditNote" runat="server" MaxLength="254" TabIndex="15"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <!---Sale Ordinary Agent--->

                                <!---Sale Bank Agent--->
                                <table id="EditSaleAgentBank" width="100%" hidden="hidden" >
                                    <tr>
                                        <td>Sale Agent Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankSaleID" runat="server" MaxLength="254" TabIndex="1" ReadOnly="True"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEditBankSaleID" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfEditBankSaleID" runat="server" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Khmer Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtKhmerEditBankFullName" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtKhmerEditBankFullName" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankFullName" runat="server" MaxLength="254" TabIndex="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtEditBankFullName" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Phone no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankPhone" runat="server" MaxLength="254" TabIndex="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtEditBankPhone" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankMobile" runat="server" MaxLength="254" TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtEditBankMobile" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankFax" runat="server" MaxLength="254" TabIndex="6"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtEditBankFax" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankEmail" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtEditBankEmail" runat="server" ControlToValidate="txtEditBankEmail" ErrorMessage="Invalid email address."  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="6"></asp:RegularExpressionValidator>
                                          
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Note:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBankNote" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtEditBankNote" ForeColor="Red" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <!---Sale Bank Agent--->

                                <!---Sale Broker Agent--->
                                <table id="EditSaleBroker" width="100%" hidden="hidden">
                                      <tr>
                                        <td>Sale Agent Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerSaleID" runat="server" MaxLength="254" TabIndex="1" ReadOnly="True"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtEditBrokerSaleID" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfEditBrokerSaleID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Khmer Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtKhmerEditBrokerFullName" runat="server" MaxLength="254" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtKhmerEditBrokerFullName" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Full Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerFullName" runat="server" MaxLength="254" TabIndex="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtEditBrokerFullName" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td>Phone no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerPhone" runat="server" MaxLength="254" TabIndex="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtEditBrokerPhone" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerMobile" runat="server" MaxLength="254" TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtEditBrokerMobile" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax no.:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerFax" runat="server" MaxLength="254" TabIndex="6"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtEditBrokerFax" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerEmail" runat="server" MaxLength="254" TabIndex="7"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtEditBrokerEmail" runat="server" ControlToValidate="txtEditBrokerEmail" ErrorMessage="Invalid email address."  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ValidationGroup="7"></asp:RegularExpressionValidator>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Note:</td>
                                        <td>
                                            <asp:TextBox ID="txtEditBrokerNote" runat="server" MaxLength="254" TabIndex="8"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtEditBrokerNote" ForeColor="Red" ValidationGroup="7">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <!---Sale Broker Agent--->
                            </td>
                            <td width="2%"> </td>
                        </tr>
                        
                    </table>
                </div>

                <div class="modal-footer">

                    <div style="display: none;">
                       <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
                    </div>   

                    <input type="button" onclick="GetSaleAgentID_Card()" id="btnEdit_first" class="btn btn-primary" hidden="hidden" style="height: 27px;" runat="server" value="Edit Ordinary" ValidationGroup="2" />

                    <input type="button" onclick="GetSaleAgentID_Card()" id="btnEditBankAgent_first" class="btn btn-primary" hidden="hidden" style="height: 27px;" runat="server" value="Edit bank" ValidationGroup="6"  />

                    <input type="button" onclick="GetSaleAgentID_Card()" id="btnEditBroker_first" class="btn btn-primary" hidden="hidden" style="height: 27px;" runat="server" value="Edit broker" ValidationGroup="7"  />

                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="HideButton();">Cancel</button>
                </div>
            </div>
            <!-- End Modal Edit Sale Agent Type Form-->

            <!-- Modal Delete Sale Agent Form -->
             <div id="myModalDeleteSaleAgent" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeleteSaleAgentHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalDeleteSaleAgentHeader">Delete Sale Agent Form</h3>
                </div>
                <div class="modal-body">
                    <!---Sale Agent--->
                    <table id="DeleteSaleAgent"  width="100%">
                        <tr>
                            <td width="2%"> </td>
                            <td width="20">Sale Agent Type:</td>
                            <td width="76%">
                                <asp:TextBox ID="txtDeleteSaleType" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                                <asp:HiddenField ID="hdfDeleteSaleAgent" runat="server"/>
                            </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2"><hr style="border-style:double; border-bottom-color:gray" /> </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2">
                                <!---Sale Ordinary Agent--->
                                <table id="DeleteSaleAgentOrdinary" width="100%" >
                                    <tr>
										<td>
											<asp:HiddenField ID="hdfDeleteSaleID" runat="server" />
											 Are you sure, you want to delete Sale Agent Code <asp:TextBox ID="txtDeleteSaleID" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
										</td>
                                    </tr>
                                </table>
                                <!---Sale Ordinary Agent--->

                                <!---Sale Bank Agent--->
                                <table id="DeleteSaleAgentBank" width="100%" hidden="hidden" >
                                   <tr>
										<td>
											<asp:HiddenField ID="hdfDeleteSaleIDBank" runat="server" />
											 Are you sure, you want to delete Sale Agent Code <asp:TextBox ID="txtDeleteSaleIDBank" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
										</td>
                                    </tr>
                                </table>
                                <!---Sale Bank Agent--->

                                <!---Sale Broker Agent--->
                                <table id="DeleteSaleAgentBroker" width="100%" hidden="hidden">
                                     <tr>
										<td>
											<asp:HiddenField ID="hdfDeleteSaleIDBroker" runat="server" />
											 Are you sure, you want to delete Sale Agent Code <asp:TextBox ID="txtDeleteSaleIDBroker" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
										</td>
                                    </tr>
                                </table>
                                <!---Sale Broker Agent--->
                            </td>
                            <td width="2%"> </td>
                        </tr>
                    </table>
                </div>

                <div class="modal-footer">
                    <%--<asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" OnClick="btnDelete_Click"    />--%>
                    <button class="btn btn-primary" value="OK" onclick="delete_sale_agent();" >OK</button>
                    <button class="btn" data-dismiss="modal" aria-hidden="true" >Cancel</button>
                </div>
            </div>
            <!--End Modal Delete Sale Agent Ordinary Form-->

             <!-- Modal Cancel Sale Agent Form -->
            <div id="myModalCancelSaleAgent" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalCancelSaleAgentHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalCancelSaleAgentHeader">Cancel Sale Agent Form</h3>
                </div>
                <div class="modal-body">
                    <!---Sale Agent--->
                    <table id="CancelSaleAgent"  width="100%">
                        <tr>
                            <td width="2%"> </td>
                            <td width="20">Sale Agent Type:</td>
                            <td width="76%">
                                <asp:TextBox ID="txtCancelSaleType" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                               <asp:HiddenField ID="hdfCancelSaleType" runat="server"/>
                            </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2"><hr style="border-style:double; border-bottom-color:gray" /> </td>
                            <td width="2%"> </td>
                        </tr>
                        <tr>
                            <td width="2%"> </td>
                            <td colspan="2">
                                <!---Sale Ordinary Agent--->
                                <table id="CancelSaleAgentOrdinary" width="100%" >
                                   <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfCancelSaleID" runat="server" />
                                             Are you sure, you want to cancel Sale Agent Code <asp:TextBox ID="txtCancelSaleID" runat="server" BorderStyle="None"  Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Note:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCancelNote" runat="server" TabIndex="2" Width="98%" ></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidatorCancelSaleID" runat="server" ControlToValidate="txtCancelNote" ForeColor="Red" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="2%"> </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCancelSaleAgent" class="btn btn-primary" Style="height: 27px;" runat="server" Text="OK" ValidationGroup="3" OnClick="btnCancelSaleAgent_Click"   />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Cancel Sale Agent Form-->

            <!-- Modal Search Sale Agent Form -->
            <div id="myModalNewSearchSaleAgentCode" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalNewSearchSaleAgentCode" aria-hidden="true" style="width:750px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy Form</h3>
                </div>
                <div class="modal-body" style="width:700px;">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li><a href="#SAgentID" data-toggle="tab" onclick="clear_sale_name();" style="text-decoration: none;" >Search By Sale Agent Code</a></li>
                        <li><a href="#SAgentName" data-toggle="tab" onclick="clear_sale_code();" style="text-decoration: none;">Search By Sale Agent Name</a></li>
                    </ul>

                    <div class="tab-content" style="height: 60px; overflow: hidden; width:700px;">
                        <div class="tab-pane" id="SAgentName">
                            <table style="width:700px; ">
                                <tr>
                                    <td style="width: 100px; vertical-align: middle">Last Name:</td>
                                    <td style="width: 100px; vertical-align: bottom">
                                        <asp:TextBox ID="txtLastNameSearch" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px; vertical-align: middle">First Name:</td>
                                    <td style="width: 100px; vertical-align: bottom">
                                        <asp:TextBox ID="txtFirstNameSearch" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div style="display: none;">
                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click"  />
                                        </div>
                                        <input type="button" class="btn" style="height: 27px;" onclick="onclick_search();" value="Search" />
                                    </td>
                                </tr>
                            </table>
                            <hr />
                        </div>

                        <div class="tab-pane active" id="SAgentID">
                            <table style="width:700px;">
                                <tr>
                                    <td style="width: 100px; vertical-align: middle">Sale Agent Code:</td>
                                    <td style="width: 100px; vertical-align: bottom">
                                        <asp:TextBox ID="txtSearchSaleAgentCode"  runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div style="display: none;">
                                            <asp:Button ID="btnSearch_Name" runat="server" OnClick="btnSearch_Click"  />
                                        </div>
                                        <input type="button" class="btn" style="height: 27px;" onclick="onclick_search_by_name();" value="Search" />
                                    </td>
                                </tr>
                            </table>
                            <hr />
                        </div>
                    </div>
                </div>
           </div>

      <!-- End of New Search -->
      
            <div class="panel panel-default">
                <div class="panel-heading">
                    
                <h3 class="panel-title">Sale Agent</h3>
                </div>
                <div class="panel-body">
                    <%--Grid View--%>
                     <%--<asp:GridView ID="GvSaleAgent" CssClass="grid-layout; grid" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Black" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanged="GvSaleAgent_PageIndexChanged" OnPageIndexChanging="GvSaleAgent_PageIndexChanging" HorizontalAlign="Center" PagerStyle-HorizontalAlign="Center" PageSize="20">--%>
                    <asp:GridView ID="GvSaleAgent" CssClass="grid-layout; grid" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Black"  
                                   AllowSorting="true" AllowPaging="False"
                                    BorderStyle="Solid"
                                   BorderWidth="1px" OnSorting="Sorting" OnPageIndexChanging="GvSaleAgent_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                   <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                                </HeaderTemplate>
                                <ItemTemplate> 
                                    <asp:CheckBox ID="ckb1" runat="server"  onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Sale_Agent_ID" ) + "\",\"" + Eval("Sale_Agent_Type" ) + "\",\"" + Eval("ID_Card" ) + "\",\"" + Eval("ID_Type" ) + "\",\"" + Eval("First_Name" ) + "\",\"" + Eval("Last_Name" ) + "\",\"" + Eval("Khmer_First_Name" ) + "\",\"" + Eval("Khmer_Last_Name" )+ "\",\"" + Eval("Country_ID" ) + "\",\"" + Eval("Gender" ) + "\",\"" + Eval("Birth_Date","{0:dd-MM-yyyy}" ) + "\",\"" + Eval("Home_Phone1" ) + "\",\"" + Eval("Mobile_Phone1" ) + "\",\"" + Eval("Fax1" ) + "\", \"" + Eval("EMail" ) + "\" ,\"" + Eval("Full_Name" ) + "\" ,\"" + Eval("Full_Name_Kh" ) + "\" ,\"" + Eval("Created_Note" ) + "\");" %>' />
                                    <asp:HiddenField ID="hdfOldSaleAgentID" runat="server" Value='<%# Bind("Sale_Agent_ID")%>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="No" HeaderText="No"></asp:BoundField>
                            <asp:BoundField DataField="Sale_Agent_ID" HeaderText="Sale Agent Code" SortExpression="Sale_Agent_ID" />
                            <asp:BoundField DataField="Sale_Agent_Type_Name" HeaderText="Sale Agent Type" />
                            <asp:BoundField DataField="ID_Card" HeaderText="ID Card" Visible="False" />
                            <asp:BoundField DataField="ID_Type" HeaderText="ID Type" Visible="False" />

                            <asp:BoundField DataField="First_Name" HeaderText="First_Name" Visible="False" />
                            <asp:BoundField DataField="Last_Name" HeaderText="Last_Name" Visible="False" />
                             <asp:BoundField DataField="Khmer_First_Name" HeaderText="Khmer_First_Name" Visible="False" />
                            <asp:BoundField DataField="Khmer_Last_Name" HeaderText="Khmer_Last_Name" Visible="False" />
                            <asp:BoundField DataField="Country_ID" HeaderText="Nationality" Visible="False" />
                            <asp:BoundField DataField="Gender" HeaderText="Gender" Visible="False" />
                            <asp:BoundField DataField="Birth_Date" HeaderText="Birthdate" Visible="False" />
                            <asp:BoundField DataField="Home_Phone1" HeaderText="Home Phone1" Visible="False" />
                            <asp:BoundField DataField="Mobile_Phone1" HeaderText="Mobile Phone1" Visible="False" />
                            <asp:BoundField DataField="Fax1" HeaderText="Fax" Visible="False" />
                            <asp:BoundField DataField="EMail" HeaderText="Email" Visible="False" />
                            <asp:BoundField DataField="Full_Name" HeaderText="Full Name" SortExpression="Full_Name"/>
                            <asp:BoundField DataField="Full_Name_KH" HeaderText="Khmer Full Name" Visible="false" />
                            <asp:BoundField DataField="Created_On" HeaderText="Updated on" DataFormatString="{0:dd-MM-yyyy}" />
                            <asp:BoundField DataField="Created_By" HeaderText="Update by" />
                            <asp:BoundField DataField="Created_Note" HeaderText="Note" />
                            <asp:BoundField DataField="Sale_Agent_Type" HeaderText="Sale_Agent_Type" Visible="False" />
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" />
                        <RowStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </div>
            </div>
 
             <%-- Section Hidenfields Initialize  --%>
             <asp:HiddenField ID="hdfuserid" runat="server" />
             <asp:HiddenField ID="hdfusername" runat="server" />
             <asp:HiddenField ID="hdftotalagentrow" runat="server" />
     
            <%-- End Section Hidenfields Initialize  --%>
   
            <!--- Section Sqldatasource--->
            <asp:SqlDataSource ID="SqlDataSourceSaleAgent" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Sale_Agent]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceNationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Nationality "></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceOffice" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT  Office_ID, Detail FROM  dbo.Ct_Office "></asp:SqlDataSource>
           <%-- End Section  --%>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnOk" />
          <%--  <asp:PostBackTrigger ControlID="btnBankOk"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnBrokerOk"></asp:PostBackTrigger>--%>

            <asp:PostBackTrigger ControlID="btnEdit"></asp:PostBackTrigger>
            <%-- <asp:PostBackTrigger ControlID="btnEditBankAgent"></asp:PostBackTrigger>
             <asp:PostBackTrigger ControlID="btnEditBroker"></asp:PostBackTrigger>--%>

            <%--<asp:PostBackTrigger ControlID="btnDelete"></asp:PostBackTrigger>--%>
            <asp:PostBackTrigger ControlID="btnCancelSaleAgent"></asp:PostBackTrigger>
             <asp:PostBackTrigger ControlID="btnSearch"></asp:PostBackTrigger >

        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

