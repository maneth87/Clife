<%@ Page Title="Clife | Flexi Term => Upload Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="upload_policies.aspx.cs" Inherits="Pages_Flexi_Term_upload_policies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        
        <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server" ValidationGroup="2"  Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnUpload" runat="server" />
            </div>
            <asp:ImageButton ID="ImgBtnUpload" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/issue_policy.png" OnClick="ImgBtnUpload_Click" />

        </li>

    </ul>

    <script type="text/javascript">
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

            
        //Get Pay Year for (payment period)
        function GetPayYear(product_id) {

            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetPayYear",
                data: "{product_id:" + JSON.stringify(product_id) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtPaymentPeriod').val(data.d);
                    $('#Main_hdfPaymentPeriod').val(data.d);
                }

            });
        };

        //Get Assure Year for (Term of Insurance)
        function GetAssureYear(product_id, customer_age) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetAssureYear",
                data: "{product_id:'" + product_id + "',customer_age:'" + customer_age + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtTermInsurance').val(data.d);
                    $('#Main_hdfTermInsurance').val(data.d);
                }

            });
        };

     
        //Process Flexi Term Product Type
        function ProcessFlexiTermProductType(value) {
            if (value == "0") {         
                $('#Main_txtPaymentPeriod').val('');
                $('#Main_hdfPaymentPeriod').val('');
                $('#Main_txtTermInsurance').val('');
                $('#Main_hdfTermInsurance').val('');

            } else {
                

                var card = $('#Main_ddlCard option:selected').val();

                $.ajax({
                    type: "POST",
                    url: "../../ProductWebService.asmx/GetProductFlexiTerm",
                    data: "{card:'" + card + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        
                        GetPayYear(data.d.Product_ID);
                        GetAssureYear(data.d.Product_ID, 0);
                        $('#Main_hdfProduct').val(data.d.Product_ID);
                        
                    }

                });

               
            }
        }
               

        //function GetOffice(company_id) {

        //    $.ajax({
        //        type: "POST",
        //        url: "../../ChannelWebService.asmx/GetChannelLocation",
        //        data: "{channel_item_id:'" + company_id + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (data) {
        //            $('#Main_ddlOffice').setTemplate($("#jTemplateOffice").html());
        //            $('#Main_ddlOffice').processTemplate(data);

        //        }

        //    });
        //}

        //function GetOfficeID(office_id) {
        //    $('#Main_hdfChannelLocation').val(office_id);
        //}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>

    
   <%-- <script id="jTemplateOffice" type="text/html">
        <option selected="selected" value='0'>.</option>
        {#foreach $T.d as record}          
             <option value='{ $T.record.Channel_Location_ID }'>{ $T.record.Office_Name }</option>
        {#/for}
           
    </script>--%>
    <br />
    <br />
    <br />


    <%-- Upload Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Flexi Term: Upload Policies</h3>
        </div>
        <div class="panel-body">
            <%--Upload Content Here--%>
            <table>
                <tr>
                    <td style="text-align: right;">Product:</td>
                    <td style="vertical-align: bottom; width:350px; vertical-align:middle;">
                        <asp:DropDownList ID="ddlCard" runat="server" Width="95%" Height="35px" onchange="ProcessFlexiTermProductType(this.value);">
                            <asp:ListItem Value="0">.</asp:ListItem>
                            <asp:ListItem Value="1">Flexi Term Card</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCard" runat="server" ControlToValidate="ddlCard" InitialValue="0" Text="*" ForeColor="Red" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                   <tr>
                    <td style="text-align: right">Term of Insurance:</td>
                    <td>
                        <asp:TextBox ID="txtTermInsurance" runat="server" Width="91%" ReadOnly="True" onkeyup="ValidateNumber(this);" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorTermOfInsurance" runat="server" ErrorMessage="Require Term of Insurance" ControlToValidate="txtTermInsurance" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfTermInsurance" runat="server" />

                    </td>
                    <td>Years</td>
                </tr>
                <tr>
                    <td style="text-align: right">Payment Period:</td>
                    <td>
                        <asp:TextBox ID="txtPaymentPeriod" Width="91%" runat="server" ReadOnly="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPaymentPeriod" runat="server" ErrorMessage="Require Payment Period" ControlToValidate="txtPaymentPeriod" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfPaymentPeriod" runat="server" />

                    </td>
                    <td >
                        Years
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align:middle;">Bank Name:</td>
                    <td>
                        <asp:DropDownList ID="ddlBank" Width="95%" Height="35px" AppendDataBoundItems="true" runat="server" onchange="GetOffice(this.value);" DataSourceID="SqlDataSourceCompany" DataTextField="Channel_Name" DataValueField="Channel_Item_ID">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ControlToValidate="ddlBank" InitialValue="0" ErrorMessage="Require Bank Name" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
               <%-- <tr>
                    <td style="text-align: right; vertical-align:middle;">Branch:</td>
                    <td>
                        <asp:DropDownList ID="ddlOffice" Width="95%" Height="35px" runat="server" AppendDataBoundItems="true" onchange="GetOfficeID(this.value);">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorOffice" runat="server" ControlToValidate="ddlOffice" ErrorMessage="Require Branch" ForeColor="Red" Text="*" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>--%>
                <tr>
                    <td style="text-align: right">Upload:</td>
                    <td>
                        <asp:FileUpload ID="FileUploadFlexiTermPolicy" runat="server" Width="91%" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUploadPolicy" ControlToValidate="FileUploadFlexiTermPolicy" Text="*" ForeColor="Red" runat="server" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--End Upload Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Upload Result</h3>
        </div>
        <div class="panel-body">
            
            <asp:Table ID="tblResult" style='width:100%;' border='1' runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell HorizontalAlign="Center">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell HorizontalAlign="Center">Back Account No.</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">First Name</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Last Name</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Gender</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Date of Birth</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">ID No.</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">ID Type</asp:TableHeaderCell>
                     <asp:TableHeaderCell HorizontalAlign="Center">Result</asp:TableHeaderCell>
                    <asp:TableHeaderCell HorizontalAlign="Center">Reason</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>

  
    <asp:SqlDataSource ID="SqlDataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select DISTINCT Ct_Channel_Item.Channel_Item_ID, Ct_Channel_Item.Channel_Name from Ct_Channel_Item 
    INNER JOIN Ct_Channel_Location on Ct_Channel_Item.Channel_Item_ID = Ct_Channel_Location.Channel_Item_ID
    INNER JOIN Ct_External_User on Ct_External_User.Channel_Location_id = Ct_Channel_Location.Channel_Location_ID
    ORDER BY Ct_Channel_Item.Channel_Name ASC"></asp:SqlDataSource>

    <%--Hidden fields of User--%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />


    <%--Hidden fields of Channel--%>
    <asp:HiddenField ID="hdfChannelChannelItem" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelLocation" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelItem" runat="server" Value="0" />

    <asp:HiddenField ID="hdfProduct" runat="server" />
</asp:Content>

