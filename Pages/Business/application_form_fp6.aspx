<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="application_form_fp6.aspx.cs" Inherits="Pages_Business_application_form_fp6" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy2" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="upApplicationNew" runat="server" UpdateMode="Conditional">
        
        <ContentTemplate>
            <ul class="toolbar">
                <li>
                        <!-- Button to trigger modal new application form -->
                    <asp:ImageButton ID="ImgBtnAddNewApplication" runat="server" ImageUrl="~/App_Themes/functions/add.png"  CausesValidation="false" />
                </li>
                <li>
                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" CausesValidation="false"  />
                </li>
                    
                <li>
                    <asp:ImageButton ID="imgCancel" runat="server"  ImageUrl="~/App_Themes/functions/cancel.png"  CausesValidation="false" />
                </li>
                <li>
                    <asp:ImageButton ID="ImgBtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png"  CausesValidation="false" OnClick="ImgBtnClear_Click" />
                </li>
                <li>
                    <div style="display: none;">
                        <asp:Button ID="btnSave" runat="server" />
                    </div>
                    <asp:ImageButton ID="imgSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="imgSave_Click" CausesValidation="false" />
                </li>
            </ul>
         <!-- Add new application modal -->
            <div>
                <cc1:ModalPopupExtender ID="mApplicationNew" runat="server" PopupControlID="divApplicatonNew" TargetControlID="ImgBtnAddNewApplication" BackgroundCssClass="modalBackground" CancelControlID="xbtn">
                   
                </cc1:ModalPopupExtender>
                   
                <div id="divApplicatonNew" runat="server" style="display:none; background-color: #FFFFFF;
                                border-width: 3px;
                                border-style: solid;
                                border-color: black;
                                padding-top: 10px;
                                padding-left: 10px;
                                width: 400px;
                                height: 220px;
                                border-radius:10px;">
                   
                    <div style="padding-top:0px; padding-right:5px; padding-bottom:0px;">
                        <asp:Button ID ="xbtn" runat="server" Text="X" style="border:none; background:none; float:right; text-shadow: 0 1px 0 #ffffff; opacity: 0.2; filter: alpha(opacity=20);" />
                        <h3>Create New Application</h3>
                        <hr style="padding-bottom:0px;" />
                                    
                    </div>
                     <table style="width:100%;">
                         <tr>
                             <td colspan="2">
                                  
                             </td>
                         </tr>
                         <tr>
                             <td colspan="2">
                                 <div class="message">
                                    <asp:Label ID="lblMessageApplicationNew" runat="server"></asp:Label>
                                </div>
                             </td>
                         </tr>
                                <tr>
                                    <td>Application Number</td>
                                            <td>
                                                <asp:TextBox ID="txtApplicationNumberNew" runat="server"></asp:TextBox>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td></td>
                                             <td>
                                                
                                             </td>
                                        </tr>
                                  </table>
                    <div style="vertical-align:middle; text-align:center;">
                        <hr />
                        <asp:Button ID="btnOkApplicationNew" runat="server" Text="Ok" CssClass="btn btn-primary" OnClick="btnOkApplicationNew_Click" CausesValidation="false" />
                    </div>

                </div>
          </div>

            <%-- cancel application modal --%>
            <div>
                <cc1:ModalPopupExtender ID="mCancelApplication" runat="server" PopupControlID="divCancelApplicatoin" TargetControlID="imgCancel" BackgroundCssClass="modalBackground" CancelControlID="btnCancelApplicationClose">
                   
                </cc1:ModalPopupExtender>
                <div id="divCancelApplicatoin" runat="server" style="display:none; background-color: #FFFFFF;
                                border-width: 3px;
                                border-style: solid;
                                border-color: black;
                                padding-top: 10px;
                                padding-left: 10px;
                                width: 400px;
                                height: 230px;

                                border-radius:10px;">

                    <div  style="padding-top:0px; padding-right:5px; padding-bottom:0px;">
                        <asp:Button ID ="btnCancelApplicationClose" runat="server" Text="X" style="border:none; background:none; float:right; text-shadow: 0 1px 0 #ffffff; opacity: 0.2; filter: alpha(opacity=20);" />
                        <h3 id="H3">Cancel Application Form</h3>
                        <hr />
                    </div>

                    <table style="padding-top:0px;">
                        <tr>
                             <td colspan="2">
                                 <div class="message">
                                    <asp:Label ID="lblMessageCancelApplication" runat="server"></asp:Label>
                                </div>
                             </td>
                         </tr>
                        <tr>
                            <td>Applciation Number:</td>
                            <td><asp:TextBox ID="txtCancelApplicationNumber" runat="server" Enabled="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Cancel Note:</td>
                            <td><asp:TextBox ID="txtCancelNote" runat="server" ></asp:TextBox></td>
                        </tr>
                       

                    </table>
                     <div style="padding-top:0px; padding-right:5px; padding-bottom:0px; text-align:center;">
                         <hr />
                        <asp:Button ID="btnCancelApplication" runat="server" Text="Ok" CssClass="btn btn-primary"  OnClick="btnCancelApplication_Click" CausesValidation="false" OnClientClick="return confirm('To cancel this application please click [Ok]!');" />
                    </div>
                </div>
            </div>
             <%-- End cancel application modal --%>

           <%-- Application search --%>
                                                
            <div>
                                                   
                <cc1:ModalPopupExtender ID ="mApplicationSearch" runat="server" PopupControlID="pApplicationSearch"  TargetControlID="ImgBtnSearch"
                                                         BackgroundCssClass="modalBackground" CancelControlID="btnCloseApplication">
                                                       
                                                   
                </cc1:ModalPopupExtender>
                                                    <asp:Panel ID="pApplicationSearch" runat="server" style="display:none;" CssClass="modalPopup">
                                                        <table style="width:100%; padding-left:10px; padding-right:10px;">
                                                             <tr>
                                                                <td colspan="4" style="padding-right:10px;">
                                                                    <asp:Button ID="btnCloseApplication" runat="server" Text="X" style="border:none; background:none; float:right; text-shadow: 0 1px 0 #ffffff; opacity: 0.2; filter: alpha(opacity=20);"/>
                                                                    <h3>Search Application</h3>
                                                                    <hr />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Select Search Option</td>
                                                                <td><asp:DropDownList ID="ddlSearchOption" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOption_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Applicaton Number" Value="APPNUMBER"></asp:ListItem>
                                                                    <asp:ListItem Text="Customer Name" Value="CUSNAME"></asp:ListItem>
                                                                    <asp:ListItem Text="ID Card No" Value="IDNO"></asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblApplicationNumberSearch" runat="server" Text="Application Number:"></asp:Label> 

                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtApplicationNumberSearch1" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                
                                                            </tr>
                                                            <tr>
                                                                <td><asp:Label ID="lblCustomerFirstNameSearch" runat="server" Text="First Name:"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomerFirstNameSearch" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td style="width:100px; text-align:right;"><asp:Label ID="lblCustomerLastNameSearch" runat="server" Text="Last Name:"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomerLastNameSearch" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td><asp:Label ID="lblIDTypeSearch" runat="server" Text="ID Type:"></asp:Label></td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlIDTypeSearch1" runat="server">
                                                                        <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                                                        <asp:ListItem Value="2">Visa</asp:ListItem>
                                                                        <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width:60px; text-align:right;"><asp:Label ID="lblIDNoSearch" runat="server" Text="ID No:"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtIDNoSearch" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                           
                                                            <tr>
                                                                <td></td>
                                                                <td colspan="3">
                                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false"/>
                                                                    <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="btn" OnClick="btnOk_Click" CausesValidation="false"/>
                                                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" CausesValidation="false"/>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td colspan="4"> <hr /></td>
                                                            </tr>
                                                           
                                                            <tr>
                                                                <td colspan="4">
                                                                     <div id="divApplicationSearch" runat="server" style="margin:5px 5px 5px 5px; height:250px; overflow:scroll; overflow-style:scrollbar; width:780px;">
                                                         
                                                                        <asp:GridView ID="gvApplication" runat="server" AutoGenerateColumns="false" Width="750px" OnSelectedIndexChanging="gvApplication_SelectedIndexChanging" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                                             <SelectedRowStyle BackColor="#00BFFF" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Select">
                                                                                    <ItemTemplate>
                                                                           
                                                                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                               
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="Application ID" ControlStyle-Width="150px" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApplicationID" runat="server" Text='<%# Eval("App_Register_ID") %>'></asp:Label>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="App. Number" ControlStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApplicationNumber" runat="server" Text='<%# Eval("App_Number") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="ID Type" ControlStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIDType" runat="server" Text='<%# Eval("ID_Type") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="ID Card" ControlStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIDCard" runat="server" Text='<%# Eval("ID_Card") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="Last Name" ControlStyle-Width="100px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("Last_Name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="First Name" ControlStyle-Width="100px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("First_Name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="Gender" ControlStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                             <Columns>
                                                                                <asp:TemplateField HeaderText="Birth Date" ControlStyle-Width="100px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBirthDate" runat="server" Text='<%# Eval("Birth_Date" ,"{0:dd/MM/yyyy}") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Nationality" ControlStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNationality" runat="server" Text='<%# Eval("Nationality") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                       
                                                    </asp:Panel>
                                                </div>
                                                <%-- End Application search --%>
      </ContentTemplate>
       
</asp:UpdatePanel>
             

<!-- End add new application modal -->
                                                    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>

   
    <style>
        body .modal.large {
            width: 70%; /* desired relative width */
            left: 15%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }

        .auto-style1 {
            height: 42px;
        }

        .auto-style2 {
            height: 37px;
        }

        .auto-style3 {
            width: 25%;
        }

        .auto-style4 {
            height: 42px;
            width: 25%;
        }

        .auto-style5 {
            height: 37px;
            width: 25%;
        }

        .auto-style6 {
            width: 35%;
        }

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 800px;
            height: 500px;
            border-radius: 10px;
        }

        .span-required {
            color: red;
        }

        .message {
            color: red;
            vertical-align: middle;
            padding-left: 30px;
            padding-bottom: 10px;
        }

        .gRow {
            vertical-align: middle;
            text-align: center;
            border-color: gray;
            border-width: 1px;
            border-style: solid;
        }

        .gHeader {
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            background-color: #6A5ACD;
            border-color: #6A5ACD;
            color: white;
        }

        .gHeaderLeft {
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            background-color: #6A5ACD;
            border-color: #6A5ACD;
            color: white;
            border-top-left-radius: 5px;
        }

        .gHeaderRight {
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            background-color: #6A5ACD;
            border-color: #6A5ACD;
            color: white;
            border-top-right-radius: 5px;
        }
    </style>

    <%--Javascript for bootstrap--%>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

    </script>



    <%-- Form Design Section--%>
    <h1>Application Form</h1>

    <table id="tblAppContent" class="table-layout">
      
        <tr>
            <td style="vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Application Details</h3>

            </td>
        </tr>
        <tr>
            <td>
                <div class="container">
                
                    <div id="application_tab" class="tab-content">
                        <%-- message alert --%>
                        <div>
                           
                        </div>
                        <%-- end message alert --%>
                         <%-- Application New --%>
                                    
                        <div class="tab-pane active" id="application_new">
                            <asp:UpdatePanel ID="upApplictionNew" runat="server">
                                <ContentTemplate>

                                     <!-- Modal Message -->
                                        <div id="ModalMessage" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalMessage" aria-hidden="true">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                <h3 class="panel-title">System Message Alert!</h3>
                                            </div>
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtMessage" Width="88%" TextMode="MultiLine" Height="60" runat="server" style="color:red; font-weight:normal;"></asp:TextBox>&nbsp;&nbsp;<br />
                                            <br />
                                            <div class="modal-footer" style="text-align:center;">
                                                <button class="btn" data-dismiss="modal" aria-hidden="true" style="display:none;">Close</button>
                                                <asp:Button ID="btnOpenRiderForm" runat="server" Text="Yes" class="btn" style="margin-left:20px; margin-right:20px;" OnClick="btnOpenRiderForm_Click" CausesValidation ="false"/>
                                                <asp:Button ID="btnNo" runat="server" Text="No" class="btn" style="margin-left:20px; margin-right:20px;" OnClick="btnNo_Click"  CausesValidation ="false"/>
                                                

                                            </div>
                                        </div>
                                        <!--End Modal Message -->

                                    <div class="message">
                                        <asp:Label ID="lblMessageApplication" runat="server"></asp:Label>
                                    </div>
                                    <table id="tblAppRegister" width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                                            <tr>
                                                <td style="width: 30%; text-align: right">Application No.:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtApplicationNumber" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                    
                                                    <asp:Label ID="Label1" runat="server" Text="*" style="color:red;"></asp:Label>
                                                </td>
                                                <td style="width: 35%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Channel:</td>
                                                <td class="auto-style6">
                                                   <%-- <asp:DropDownList ID="ddlChannel" runat="server" DataSourceID="SqlDataSourceChannel" DataTextField="Type" DataValueField="Channel_ID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"  >
                                                        <asp:ListItem Value="0">.</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                     <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"  >
                                                        <asp:ListItem Value="0">.</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label2" runat="server" Text="*" style="color:red;"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                             </tr>
                                            <tr>
                                                <td style="text-align:right">Company:</td>
                                                <td class="auto-style6">
                                                    <asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList>
                                                    <asp:Label ID="Label3" runat="server" Text="*" style="color:red;"></asp:Label>
                                                </td>
                                                <td>
                                                </td>

                                            </tr>
                                             <tr>
                                                <td style="text-align: right">Payment Code:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtPaymentCode" runat="server" MaxLength="50"></asp:TextBox>
                                                    <asp:Label ID="Label4" runat="server" Text="*" style="color:red;"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Policy No.:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtPolicyNumber" runat="server" ReadOnly="True"></asp:TextBox>
                                                  
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Underwrite Status:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtUnderwritingStatus" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Date of Entry:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtDateEntry" runat="server" MaxLength="15" CssClass="datepicker" ></asp:TextBox>
                                                    <asp:Label ID="Label5" runat="server" Text="* (DD/MM/YYYY)" style="color:red;"></asp:Label>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Data Entry By:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtDataEntryBy" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:Label ID="Label6" runat="server" Text="*" style="color:red;"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Date of Signature:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtDateSignature" runat="server" CssClass="datepicker" MaxLength="15"></asp:TextBox>
                                                    <asp:Label ID="Label7" runat="server" Text="* (DD/MM/YYYY)" style="color:red;"></asp:Label>
                                                </td>
                                                <td style="text-align:left;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Marketing Code:</td>
                                                <td class="auto-style6" style="vertical-align:middle;">
                                                    <asp:TextBox ID="txtMarketingCode" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:Label ID="Label8" runat="server" Text="*" style="color:red;"></asp:Label>
                                                    <asp:ImageButton ID="imgMarketingSearch" runat="server" ImageUrl="~/App_Themes/functions/search_icon.png" CausesValidation="false" />
                                                </td>
                                                <td style="text-align:left;">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Name of Marketing:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtMarketingName" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:Label ID="Label9" runat="server" Text="*" style="color:red;"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Note:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtNote" TextMode="MultiLine" Height="40" runat="server" MaxLength="255"></asp:TextBox>
                                                    <span> </span>
                                                </td>
                                                <td>
                                                   
                                                </td>
                                            </tr>
                                        </table>

                                                
                                    <%-- Marketing search --%>
                                                
                                    <div>
                                                    
                                        <cc1:ModalPopupExtender ID ="mMarketingSearch" runat="server" PopupControlID="pMarketingSearch"  TargetControlID="imgMarketingSearch" CancelControlID="btnCloseMarketing"
                                                         BackgroundCssClass="modalBackground">
                                        </cc1:ModalPopupExtender>
                                        <asp:Panel ID="pMarketingSearch" runat ="server" CssClass="modalPopup" style="display:none;"  >
                                            <div style="float:right; margin-top:10px; padding-right:20px;">
                                                <asp:Button ID ="btnCloseMarketing" runat="server" Text="X" BorderStyle="None"  />
                                            </div>
                                            <table style="width: 100%; text-align: left; margin-top:20px;">
                                                <tr>
                                                    <td style="width: 25%; vertical-align: middle">Agent Code/Name:</td>
                                                    <td style="width: 57%; vertical-align: bottom">
                                                        <asp:TextBox ID="txtAgentName" Width="90%" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 18%; vertical-align: top">
                                                        <asp:Button ID="btnSearchMarketingName" runat="server" Text="Search" CssClass="btn" OnClick="btnSearchMarketingName_Click" CausesValidation="false"/>
                                                    </td>
                                                </tr>
                                            </table>
                                                       
                                            <div style="float:right; margin-top:300px; padding-right:20px;">
                                                <asp:Button ID="btnOkMarketing" runat="server" CssClass="btn btn-primary" Text="Ok" OnClick="btnOkMarketing_Click" CausesValidation="false" />
                                            </div>
                                            <div id="divMarketing" runat="server" style="margin:5px 5px 5px 5px; height:280px; overflow:scroll; overflow-style:scrollbar;">
                                                <asp:GridView ID="gvMarketing" runat="server" AutoGenerateColumns="false" OnRowCommand="gvMarketing_RowCommand" OnSelectedIndexChanging="gvMarketing_SelectedIndexChanging">
                                                    <SelectedRowStyle BackColor="#00BFFF" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                                 
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Agent Code" ControlStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("Sale_Agent_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                                 
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Full Name" ControlStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("Full_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                            
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                                
                                    <%-- End Marketing search --%>
                                 </ContentTemplate>
                                            <Triggers>
                                               <asp:PostBackTrigger ControlID="btnOpenRiderForm" />
                                               <asp:PostBackTrigger ControlID="btnNo" />
                                               
                                            </Triggers> 
                            </asp:UpdatePanel>
                        </div>
                 </div>
                 
                </div>
                
            </td>
        </tr>
        <tr>
            
            <td style="vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
            </td>
           
        </tr>
        <tr>
            <td colspan="2">
                <div class="container">

                    <div id="tab-content" style="padding-top: 10px;">
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active"><a href="#personalinformation" data-toggle="tab">Personal Information</a></li>
                            <li><a href="#mailling" data-toggle="tab">Mailling Address</a></li>
                            <li><a href="#jobhistory" data-toggle="tab">Job History</a></li>
                            <li><a href="#insuranceplan" data-toggle="tab">Insurance Plan</a></li>
                            <li><a href="#beneficiaries" data-toggle="tab">Beneficiaries</a></li>
                            <li><a href="#health" data-toggle="tab">Miscellaneolus and Health Details</a></li>
                        </ul>

                        <div style=" padding:5px 5px 5px 5px; color:#333991; font-weight:bold;">
                                                <table>
                                                    <tr>
                                                        <td style="vertical-align:text-bottom;">Policy Owner is the life insured</td>
                                                        <td style="vertical-align:top;"> <asp:CheckBox ID="ckbPolicyowner"  runat="server" TextAlign="Left" /></td>
                                                    </tr>
                                                </table>
                                                
                                            
                        </div>
                        <div id="my-tab-content" class="tab-content">
                            <div class="tab-pane " id="mailling">
                                <asp:UpdatePanel ID="upContact" runat="server">
                                    <ContentTemplate>
                                        <div class="message">
                                            <asp:Label ID="lblMessageConatact" runat="server"></asp:Label>
                                        </div>
                                         <%--Mailling Address--%>
                                        <table style= " width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <tr>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Policy owner</h3>
                                                </td>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Life Insured 1</h3></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 14.8%; text-align: right">Address:</td>
                                                            <td style="width: 85.2%">
                                                                <asp:TextBox ID="txtAddress1Life1" Width="96%" Height="30px" runat="server"></asp:TextBox>
                                                                 <asp:TextBox ID="txtAddress2Life1" Width="96%" Height="30px" runat="server"></asp:TextBox>
                                                            </td>
                                                           <%-- <td style="width: 85.2%">
                                                                <asp:TextBox ID="txtAddress1Life1" Width="96%"​​ runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtAddress2Life1" Width="96%" runat="server"></asp:TextBox>
                                                            </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Province/City:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtCityLife1" Width="96%" Height="30px" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Zip Code:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtZipCodeLife1" Width="96%" runat="server" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Country:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountryLife1" Width="97.3%" Height="25px" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceCountry" DataTextField="Country_Name" DataValueField="Country_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlCountryLife1_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">.</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Require Country" ControlToValidate="ddlCountryLife1" InitialValue="0" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Tel.:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtTelephoneLife1" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Mobile Phone:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtMobilePhoneLife1" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="Require Mobile Phone" ControlToValidate="txtMobilePhoneLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">E-mail:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmailLife1" Width="96%" runat="server" MaxLength="125"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Invalid E-mail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmailLife1"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <table width="100%" >
                                                        <tr>
                                                            <td style="width: 14.8%; text-align: right">Address:</td>
                                                            <td style="width: 85.2%">
                                                                <asp:TextBox ID="txtAddress1Life2" Width="96%" Height="30px" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtAddress2Life2" Width="96%" Height="30px" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Province/City:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtCityLife2" Width="96%" Height="30px" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Zip Code:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtZipCodeLife2" Width="96%" runat="server" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Country:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountryLife2" Width="97.3%" Height="25px" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceCountry" DataTextField="Country_Name" DataValueField="Country_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlCountryLife2_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">.</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="Require Country" ControlToValidate="ddlCountryLife2" InitialValue="0" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Tel.:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtTelephoneLife2" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Mobile Phone:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtMobilePhoneLife2" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="Require Mobile Phone" ControlToValidate="txtMobilePhoneLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">E-mail:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmailLife2" Width="96%" runat="server" MaxLength="125"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Invalid E-mail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmailLife2"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left:5px;"><asp:ImageButton ID="imgbtnPaste" runat="server" ImageUrl="~/App_Themes/functions/paste.png"  style="width:28px; height:28px;" OnClick="imgbtnPaste_Click" CausesValidation="false" ToolTip="Paste from policy owner" /></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="vertical-align:middle; text-align:center;">
                                                    <asp:Button ID="btnAddressUpdate" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnAddressUpdate_Click" />
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                               
                                
                            </div>

                            <%--life insured 2 --%>
                                <div class="tab-pane active" id="personalinformation">
                                    <asp:UpdatePanel ID="upPersonalInformation" runat="server">
                                        <ContentTemplate>
                                            <div class="message">
                                                <asp:Label ID="lblMessagePersonalInfo" runat="server"></asp:Label>
                                            </div>
                                            
                                            <table  style=" width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                <tr>
                                                    <td style="width: 40%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                        <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Personal Information</h3>
                                                    </td>
                                                    <td style="width: 60%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                        <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Personal Information List</h3></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 40%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px;">
                                                        <table style="width: 100%; " >
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblmessage" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 30%; text-align: right">Policy Owner:</td>
                                                                 <td style="width: 70%">
                                                                    <asp:DropDownList ID="ddlPolicyOwner" runat="server" class="span2" >
                                                                        <asp:ListItem Value="0">Owner</asp:ListItem>
                                                                        <asp:ListItem Value="1">Life Insured</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                             
                                                             <tr>
                                                                <td style="width: 30%; text-align: right">Marital Status:</td>
                                                                 <td style="width: 70%">
                                                                    <asp:DropDownList ID="ddlMaritalStatus" runat="server" class="span2">
                                                                        <asp:ListItem Value="">.</asp:ListItem>
                                                                        <asp:ListItem Value="SINGLE">Single</asp:ListItem>
                                                                        <asp:ListItem Value="MARRIED">Married</asp:ListItem>
                                                                        <asp:ListItem Value="DIVORCED">Divorced</asp:ListItem>
                                                                        <asp:ListItem Value="WIDOWED">Widowed</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td style="width: 30%; text-align: right">I.D Type:</td>
                                                                <td style="width: 70%">
                                                                    <asp:DropDownList ID="ddlIDTypeLife1" runat="server" class="span2">
                                                                        <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                                                        <asp:ListItem Value="2">Visa</asp:ListItem>
                                                                        <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                               
                                                             </tr>
                                                            <tr>
                                                                <td style="width:30%; text-align: right" class="auto-style3">I.D No.:</td>
                                                                <td style="width: 70%">
                                                                    <asp:TextBox ID="txtIDNumberLife1" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                                                                </td>
                                                            </tr>
                           
                                                            <tr>
                                                                <td style="text-align: right">Surname in Khmer:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSurnameKhLife1" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                                </td>
                                                               
                                                            </tr>
                                                            <tr>
                                                                 <td style="text-align: right" class="auto-style3">First Name in Khmer:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFirstNameKhLife1" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Surname:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSurnameEngLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase" ></asp:TextBox>
                                                                </td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right" class="auto-style3">First Name:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFirstNameEngLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                           
                                                            <tr>
                                                                <td style="text-align: right">Nationality:</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlNationalityLife1" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                                        <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="auto-style3"></td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right" class="auto-style2">Gender:</td>
                                                                <td class="auto-style2">
                                                                    <asp:DropDownList ID="ddlGenderLife1" class="span2" runat="server">
                                                                        <asp:ListItem Value="1">Male</asp:ListItem>
                                                                        <asp:ListItem Value="0">Female</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="auto-style5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Date of Birth:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDateBirthLife1" runat="server" MaxLength="15" CssClass="span2 datepicker"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirthLife1" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                        <span style="color:red;">(DD/MM/YYYY)</span>
                                                                   
                                                                </td>
                                                                <td class="auto-style3" style="padding-top: 2px"></td>
                                                                <td></td>
                                                           </tr>
                                                            <tr>
                                                                <td style="width: 30%; text-align: right">Relationship:</td>
                                                                 <td style="width: 70%">
                                                                    <asp:DropDownList ID="ddlRelationShip" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlRelationShip_SelectedIndexChanged" >
                                                                        <asp:ListItem Value="">.</asp:ListItem>
                                                                        <asp:ListItem Value="WIFE">Wife</asp:ListItem>
                                                                        <asp:ListItem Value="HUSBAND">Husband</asp:ListItem>
                                                                        <asp:ListItem Value="OTHERS">Others</asp:ListItem>
                                                                        
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtOthers" runat="server" placeholder="Relationship" CssClass="span2" Visible="false"></asp:TextBox>
                                                                </td>
                                                                
                                                            </tr>
                                                           
                                                            <tr>
                                                                <td></td>
                                                                <td style="text-align:right; float:right;" colspan="3">
                                                                    <table>
                                                                        <tr>
                                                                             <td> <asp:Button ID="btnPersonalAdd" runat="server" Text="Add" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnPersonalAdd_Click" /></td>
                                                                            <td> <asp:Button ID="btnClearPersonInfo" runat="server" Text="Clear" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnClearPersonInfo_Click"/></td>
                                                                           
                                                                        </tr>
                                                                    </table>
                                                                   
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 60%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-left:5px; padding-right:5px; padding-top:10px;">
                                                        <asp:GridView ID="gvPersonalInfo" runat="server" AutoGenerateColumns="false"  OnSelectedIndexChanging="gvPersonalInfo_SelectedIndexChanging" OnRowDeleting="gvPersonalInfo_RowDeleting" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                            <SelectedRowStyle BackColor="#00BFFF" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="gHeaderLeft" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete-icon.png" Width="20px" Height="20px"  CommandName="delete" CausesValidation="false" OnClientClick=" return confirm('Confirm Delete!!!');" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                           
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Level" ControlStyle-Width="200px" Visible="False">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID Type" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIdType" runat="server" Text='<%# Eval("idType") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                                
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID Number" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIdNumber" runat="server" Text='<%# Eval("idNumber") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                             
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sur Name KH" ControlStyle-Width="200px"  Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSurNameKh" runat="server" Text='<%# Eval("surKhName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sur Name En" ControlStyle-Width="200px">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSurNameEn" runat="server" Text='<%# Eval("surEnName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="First Name KH" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFirstNameKh" runat="server" Text='<%# Eval("firstKhName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="First Name EN" ControlStyle-Width="200px">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFirstNameEn" runat="server" Text='<%# Eval("firstEnName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                             
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Father Sur Name" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFatherSurName" runat="server" Text='<%# Eval("fatherSurName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Father First Name" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFatherFirstName" runat="server" Text='<%# Eval("fatherFirstName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                                
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Mother Sur Name" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMotherSurName" runat="server" Text='<%# Eval("motherSurName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                                
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Mother First Name" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMotherFirstName" runat="server" Text='<%# Eval("motherFirstName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                                
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Previous Sur Name" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPreviousSurName" runat="server" Text='<%# Eval("previousSurName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                               
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Previous First Name" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPreviousFirstName" runat="server" Text='<%# Eval("previousFirstName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Nationality" ControlStyle-Width="200px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNationality" runat="server" Text='<%# Eval("nationality") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                               
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Gender" ControlStyle-Width="100px" Visible="true">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGender" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                             
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Date of Birth" ControlStyle-Width="100px">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDob" runat="server" Text='<%# Eval("dob") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" ControlStyle-Width="100px" Visible="false">
                                                                    <HeaderStyle CssClass="gHeader" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                           
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Marital Status" ControlStyle-Width="100px" Visible="true">
                                                                    <HeaderStyle CssClass="gHeaderRight" />
                                                                    <ItemStyle CssClass="gRow" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaritalStatus" runat="server" Text='<%# Eval("marital_status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                  
                                                        </asp:GridView>
                                                        <div style="padding-top:10px;">
                                                            <asp:Button ID="btnUpdatePerson" runat="server" CausesValidation="false" Text="Save" CssClass="btn btn-primary" OnClick="btnUpdatePerson_Click" OnClientClick="return confirm('Confirm update!!!');" />
                                                        </div>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane" id="jobhistory">
                                <asp:UpdatePanel ID="upJobHistory" runat="server">
                                    <ContentTemplate>
                                        <div  class="message">
                                            <asp:Label ID="lblMessageJobHistory" runat="server"></asp:Label>
                                        </div>

                                        <%--Job History--%>
                                         <table style= " width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                             <tr>
                                                <td style="width: 40%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Job History Detail</h3>
                                                </td>
                                                <td style="width: 60%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Job History List</h3></td>
                                            </tr>
                                            <tr>
                                                 <td style="width: 40%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px;">
                                                     <table width="100%">
                                                         <tr>
                                                             <td style="width: 35%; text-align: right">Person:</td>
                                                             <td>
                                                                 <asp:DropDownList ID="ddlJobPolicyOwner" runat="server">
                                                                 
                                                                 </asp:DropDownList>
                                                             </td>
                                                         </tr>
                                                        <tr>
                                                            <td style="width: 35%; text-align: right">Name of Employer:</td>
                                                            <td style="width: 65%">
                                                                <asp:TextBox ID="txtNameEmployerLife1" Width="63%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Nature of Business:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtNatureBusinessLife1" Width="63%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Role and Responsibility:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtRoleAndResponsibilityLife1" Width="63%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Current Position:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtCurrentPositionLife1" Width="63%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Anual Income (USD):</td>
                                                            <td>
                                                                <asp:TextBox ID="txtAnualIncomeLife1" Width="63%" runat="server" onkeyup="ValidateNumber(this);" MaxLength="12"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align:center;">
                                                                <asp:Button ID="btnAddJobHistory" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddJobHistory_Click" CausesValidation="false" />
                                                                <asp:Button ID="btnClearJobHistory" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClearJobHistory_Click" CausesValidation="false" />
                                                            </td>
                                                          
                                                        </tr>

                                                    </table>
                                                 </td>
                                                
                                                <td style="width: 60%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px; padding-left:10px; margin-bottom:10px;">
                                                    <asp:GridView ID="gvJobHistory" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanging="gvJobHistory_SelectedIndexChanging" OnRowDeleting="gvJobHistory_RowDeleting" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                        <SelectedRowStyle BackColor="#00BFFF" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderStyle  CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete-icon.png" Width="20px" Height="20px"  CommandName="delete" CausesValidation="false" OnClientClick=" return confirm('Confirm Delete!!!');" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Job ID" ControlStyle-Width="200px" Visible="false">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblJobID" runat="server" Text='<%# Eval("job_id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Application ID" ControlStyle-Width="200px" Visible="false">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Eval("app_register_id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Employer Name" ControlStyle-Width="150px" Visible="true">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmployerName" runat="server" Text='<%# Eval("employer_name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Nnature of Business" ControlStyle-Width="200px" Visible="true">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNatureOfBusiness" runat="server" Text='<%# Eval("nature_of_business") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="current_position" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCurrentPosition" runat="server" Text='<%# Eval("current_position") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Job Role" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblJobRole" runat="server" Text='<%# Eval("job_role") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Anual Income" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle  CssClass="gHeaderRight" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAnualIncome" runat="server" Text='<%# Eval("anual_income") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                     
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Level" ControlStyle-Width="200px" Visible="false">
                                                                <HeaderStyle  CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div style="padding-top:10px;">
                                                       
                                                        <asp:Button ID="btnSaveJob" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveJob_Click" />
                                                    </div>
                                                 </td>
                                             </tr>
                                            
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                          
                            <div class="tab-pane" id="insuranceplan">
                                <asp:UpdatePanel ID="upInsurancePlan" runat="server">
                                    <ContentTemplate>
                                       
                                        <div class="message">
                                            <asp:Label ID="lblMessageInsurancePlan" runat="server"></asp:Label>
                                        </div>
                                        <%--Insurance Plan--%>
                                        <table style="width:100%; border-top:1pt solid #d5d5d5;">
                                            <tr>
                                                <td style=" width:100%; vertical-align: top; padding-top:10px;">
                                                    <table style="border-right: 1pt solid #d5d5d5;" border="0">
                                                        <tr>
                                                            <td style="text-align: right ; width:30%;">Effective Date:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                                                <asp:Label ID="lbl" runat="server" ForeColor="Red" Text="(DD/MM/YYYY)"></asp:Label> 
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right ; width:30%;">Type of Insurance Plan:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTypeInsurancePlan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeInsurancePlan_SelectedIndexChanged" ></asp:DropDownList>
                                                                <%--<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="sqlDataSourcePlan" DataTextField="Product_ID" DataValueField="year" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeInsurancePlan_SelectedIndexChanged" AppendDataBoundItems="true"></asp:DropDownList>--%>
                                                                <asp:Label runat="server" ID="lblProductTitle" Text="" ForeColor="blue"></asp:Label>
                                                            </td>
                                                             <td>
                                                                 
                                                             </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Term of Insurance:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtTermInsurance" runat="server" ReadOnly="True" MaxLength="3"></asp:TextBox>Year
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Payment Period:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtPaymentPeriod" runat="server" ReadOnly="True"></asp:TextBox>Year
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Insurance Amount Required:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtInsuranceAmountRequired" runat="server" MaxLength="12" AutoPostBack="true" OnTextChanged="txtInsuranceAmountRequired_TextChanged" ></asp:TextBox> USD
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Premium Mode:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPremiumMode" runat="server" DataSourceID="SqlDataSourcePaymentMode" DataTextField="Mode" DataValueField="Pay_Mode_ID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlPremiumMode_SelectedIndexChanged"  >
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td style="float:left;">
                                                                <asp:ImageButton ID="imgRecalculation" runat="server" ImageUrl="~/App_Themes/functions/calculator.png" ToolTip="Calculate Premium" CausesValidation="false" OnClick="imgRecalculation_Click" style="margin-bottom:5px;"  />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                         <tr>
                                                <td style="width: 20%; text-align: right">Assuree Age:</td>
                                                <td>
                                                    <asp:TextBox ID="txtAssureeAgeLife1" runat="server" ReadOnly="True"></asp:TextBox>Year
                                                </td>
                                                <td style="float:left;">
                                                    
                                                </td>
                                            </tr>
                                                                     
                                            <tr>
                                                <td style="text-align: right">Annual Premium (System):</td>
                                                <td>
                                                    <asp:TextBox ID="txtAnnualOriginalPremiumAmountSystemLife1" runat="server" ReadOnly="True"></asp:TextBox>USD
                                                </td>
                                                                    
                                            </tr>
                                                                     
                                            <tr>
                                                <td style="text-align: right">Premium (System):</td>
                                                <td>
                                                    <asp:TextBox ID="txtPremiumAmountSystemLife1" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:TextBox ID="txtPremiumOriginalAmountLife1" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                            </tr>
                                                                    
                                            <tr>
                                                <td style=" text-align: right;">Total Premium:</td>
                                                <td >
                                                    <asp:TextBox ID="txtTotalPremium" runat="server" ReadOnly="true" ></asp:TextBox>
                                                    <asp:TextBox ID="txtTotalOriginalPremium" runat="server" ReadOnly="true"   Visible="false"></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                            </tr>
                                                                     
                                            <tr>
                                                <td style=" text-align: right;">Discount Amount:</td>
                                                <td >
                                                    <asp:TextBox ID="txtDiscountAmount" runat="server" MaxLength="12" Text="0" AutoPostBack="true" OnTextChanged="txtDiscountAmount_TextChanged"></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                            </tr>
                                                        <tr>
                                                            <td colspan="3"><hr /></td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right; ">Rider:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlRider" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRider_SelectedIndexChanged">
                                                                    <asp:ListItem Text="." Value="" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="ADB" Value="ADB"></asp:ListItem>
                                                                    <asp:ListItem Text="TPD" Value="TPD"></asp:ListItem>
                                                                    <asp:ListItem Text="Spouse" Value="SPOUSE"></asp:ListItem>
                                                                    <asp:ListItem Text="Child" Value="CHILD"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:ImageButton ID="imgbtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh_icon.png" OnClick="imgbtnRefresh_Click" ToolTip="Refresh rider calculation" CausesValidation="false" style="width:28px; height:28px; float:right; margin-right:130px;" />
                                                            </td>
                                                            
                                                             <td rowspan="5">
                                                                 
                                                             </td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right; ">Rider Sum Insured:</td>
                                                            <td>
                                                               <asp:TextBox ID="txtRiderSumInsured" runat="server" ReadOnly="true" Enabled="false" OnTextChanged="txtRiderSumInsured_TextChanged" AutoPostBack="true" CausesValidation="false" ></asp:TextBox>USD
                                                            </td>
                                                           <td></td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; ">Apply To:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlRiderPerson" runat="server">

                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; ">Categories</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlClassRate" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlClassRate_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; ">Position</td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlPosition"  Enabled="false"  AutoPostBack="true" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged"></asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right; ">Rate:</td>
                                                            <td>
                                                               <asp:TextBox ID="txtADBRate" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                            </td>
                                                           <td></td>
                                                           
                                                        </tr>
                                                       
                                                         <tr>
                                                            <td style="text-align: right; ">Rider Premium:</td>
                                                            <td>
                                                               <asp:TextBox ID="txtRiderPremium" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>USD
                                                            </td>
                                                           <td></td>
                                                           
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right; ">Original Amount:</td>
                                                            <td>
                                                               <asp:TextBox ID="txtOriginalAmount" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>USD
                                                            </td>
                                                           <td></td>
                                                           
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right; ">Rounded Amount:</td>
                                                            <td>
                                                               <asp:TextBox ID="txtRoundedAmount" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>USD
                                                            </td>
                                                           <td></td>
                                                           
                                                        </tr>
                                                         <tr>
                                                           <td></td>
                                                            <td>
                                                               
                                                                <asp:Button ID="btnRiderAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnRiderAdd_Click"  CausesValidation="false" />
                                                                 <asp:Button ID="btnRiderClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnRiderClear_Click" CausesValidation="false"/>
                                                            </td>
                                                           <td></td>
                                                          
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3"><br /></td>
                                                        </tr>
                                                        <tr>
                                                            <td>

                                                            </td>
                                                            <td colspan="2">
                                                                <asp:GridView ID="gvRider" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanging="gvRider_SelectedIndexChanging" OnRowDeleting="gvRider_RowDeleting" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                                      <SelectedRowStyle BackColor="#00BFFF" />
                                                                     <Columns>
                                                                                    
                                                                         <asp:TemplateField>
                                                                             <HeaderStyle CssClass="gHeaderLeft" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                             <ItemTemplate>
                                                                                 <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                     </Columns>

                                                                      <Columns>
                                                                        <asp:TemplateField>
                                                                             <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete-icon.png" Width="20px" Height="20px"  CommandName="delete" CausesValidation="false" OnClientClick=" return confirm('Confirm Delete!!!');" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Level"  Visible="False">
                                                                             <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Rider ID"  Visible="false">
                                                                             <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRiderID" runat="server" Text='<%# Eval("rider_id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Product ID"  Visible="false">
                                                                             <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         
                                                                    </Columns>
                                                                     <Columns>
                                                                         <asp:TemplateField HeaderText="Application ID"  Visible="false">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblApplicationNumber" runat="server" Text='<%# Eval("app_register_id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                      <Columns>
                                                                         <asp:TemplateField HeaderText="Rider Type" Visible="true" ControlStyle-Width="100px">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRiderType" runat="server" Text='<%# Eval("rider_type") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                      <Columns>
                                                                         <asp:TemplateField HeaderText="Sum Insured" Visible="true" ControlStyle-Width="100px">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSumInsured" runat="server" Text='<%# Helper.FormatDec(Convert.ToDouble( Eval("sumInsured"))) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                     
                                                                    <Columns>
                                                                         <asp:TemplateField HeaderText="Annual Premium" ControlStyle-Width="50px" Visible="true">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOriginalAmount" runat="server" Text='<%# Eval("original_amount") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                    <Columns>
                                                                         <asp:TemplateField HeaderText="Annual Premium Round Up" ControlStyle-Width="50px" Visible="true">
                                                                              <HeaderStyle CssClass="gHeaderRight" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRoundedAmount" runat="server" Text='<%# Eval("rounded_amount") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                     <Columns>
                                                                         <asp:TemplateField HeaderText="Premium By Pay Mode" Visible="true" ControlStyle-Width="100px">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPremium" runat="server" Text='<%# Eval("premium") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                      <Columns>
                                                                         <asp:TemplateField HeaderText="Discount" ControlStyle-Width="50px" Visible="false">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("discount") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                    <Columns>
                                                                         <asp:TemplateField HeaderText="Rate ID" ControlStyle-Width="50px" Visible="false">
                                                                              <HeaderStyle CssClass="gHeader" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRateID" runat="server" Text='<%# Eval("rate_id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                     </Columns>
                                                                    
                                                                 </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            
                                                            <td colspan="3"><hr /></td>
                                                        </tr>
                                                        </tr>
                                            </tr>
                                              
                                            <tr>
                                                <td style=" text-align: right;">Rider Total Premium:</td>
                                                <td >
                                                    <asp:TextBox ID="txtRiderTotalPreium" runat="server" MaxLength="12" Text="0" ReadOnly="true" ></asp:TextBox>USD
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lbtnShowRiderDetial" Text="View Rider Detail" CausesValidation="false" OnClick="lbtnShowRiderDetial_Click" style="color:blue; text-decoration-style:double; text-decoration-color:ActiveBorder; "></asp:LinkButton> | 
                                                    <asp:LinkButton runat="server" ID="lbtnAddRider" Text="Add Riders" CausesValidation="false" Enabled="false" OnClick="LinkButton1_Click"  style="color:blue; text-decoration-style:double; text-decoration-color:ActiveBorder; " ></asp:LinkButton>
                                                </td>
                                            </tr>
                                                                    
                                            <tr>
                                                <td style=" text-align: right;">Rider Discount Amount:</td>
                                                <td style="float:left;" >
                                                    <asp:TextBox ID="txtRiderDiscountAmount" runat="server" MaxLength="12" Text="0" AutoPostBack="true" OnTextChanged="txtRiderDiscountAmount_TextChanged"></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                            </tr>
                                                                    
                                            <tr>
                                                <td style=" text-align: right;">Rider Total Premium After Discount:</td>
                                                <td >
                                                    <asp:TextBox ID="txtRiderTotalPremiumAfterDiscount" runat="server" MaxLength="12" Text="0" ReadOnly="true"></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                            </tr>
                                                                   
                                            <tr>
                                                <td colspan="3">
                                                    <hr />
                                                </td>
                                            </tr>
                                                                    
                                            <tr>
                                                <td style=" text-align: right;">Total Premium After Discount:</td>
                                                <td >
                                                    <asp:TextBox ID="txtTotalPremiumAfterDiscount" runat="server" ReadOnly="true" ></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                            </tr>
                                                                    
                                            <tr>
                                                <td style="text-align: right">Premium (User):</td>
                                                <td>
                                                    <asp:TextBox ID="txtPremiumAmount" runat="server" MaxLength="12"></asp:TextBox>USD
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>  
                                            <tr>
                                                <td style="vertical-align: top; text-align: right;">Note:</td>
                                                <td >
                                                    <asp:TextBox ID="txtInsurancePlanNote" runat="server"  MaxLength="254" TextMode="MultiLine" Height="50"></asp:TextBox>                        
                                                </td>
                                                <td></td>
                                            </tr>  
                                            <tr>
                                                <td colspan="3" style="text-align:center;">
                                                    <table style="width:100%; border-spacing:0px;">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSaveInsuredPlan" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveInsuredPlan_Click" OnClientClick="return confirm('Confirm update!!');" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        
                                        </table>
                                        </td>

                                        <td style=" width:0%; vertical-align: top; padding-top:10px; ">
                                            <div id="dvLoan" runat="server">
                                               <%--Loan Information--%>
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td style="width: 30%; text-align: right">Loan Type:</td>
                                                        <td style="width: 70%">
                                                            <asp:DropDownList ID="ddlLoanType" runat="server">
                                                                <asp:ListItem Value="0">Home</asp:ListItem>
                                                                <asp:ListItem Value="1">Business</asp:ListItem>
                                                                <asp:ListItem Value="2">Others</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">Interest:</td>
                                                        <td>
                                                            <asp:TextBox ID="txtInterest" runat="server" MaxLength="5" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">Term:</td>
                                                        <td>
                                                            <asp:TextBox ID="txtTermLoan" runat="server" MaxLength="3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">Loan Effective Date:</td>
                                                        <td>
                                                            <asp:TextBox ID="txtLoanEffectiveDate" CssClass="datepicker" runat="server" MaxLength="15"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">Outstanding Loan:</td>
                                                        <td>
                                                            <asp:TextBox ID="txtOutstandingLoanAmount" runat="server" MaxLength="12"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                        </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                           
                            <div class="tab-pane" id="beneficiaries">
                                <asp:UpdatePanel ID="upBeneficiaries" runat="server">
                                    <ContentTemplate>
                                        <%--Benefitciary--%>
                                        <div class="message">
                                            <asp:Label ID="lblMessageBenifit" runat="server"></asp:Label>
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table id="tblBenefitTable" class="table table-bordered">

                                                        <tr>
                                                            <td width="2%">&nbsp;</td>

                                                            <td colspan="6">
                                                                <div style="padding-bottom: 5px">Note</div>
                                                                <asp:TextBox ID="txtBenefitNote" runat="server" Width="98.7%" MaxLength="255"></asp:TextBox>
                                                            </td>
                                                            <td width="2%"></td>
                                                        </tr>
                                                        <tr>
                                                           <td width="2%">&nbsp;</td>
                                                             <td colspan="6">
                                                                <div style="padding-bottom: 5px">Total Shared(%)</div>
                                                                <asp:TextBox ID="txtTotalSharedPercentage" runat="server" Width="98.7%" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td width="2%"></td>
                                                        </tr>

                                                        <tr>
                                                            <td width="2%"></td>
                                                            <td  style="width:100px;">
                                                                <div style="padding-bottom: 5px">ID. Type</div>
                                                                <asp:DropDownList ID="ddlBenefitIDType" runat="server" Style="width: 100px; height: 40px">
                                                                    <asp:ListItem Value="0"> I.D Card</asp:ListItem>
                                                                    <asp:ListItem Value="1">Passport</asp:ListItem>
                                                                    <asp:ListItem Value="2">Visa</asp:ListItem>
                                                                    <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <div style="padding-bottom: 5px">ID. No<asp:Label ID="lblBenefitIDNoValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                                <asp:TextBox ID="txtBenefitIDNo" Style="width: 93%;" runat="server" MaxLength="30" Height="30"></asp:TextBox>
                                                            </td>

                                                            <td>
                                                                <div style="padding-bottom: 5px">Surname and Name<asp:Label ID="lblBenefitNameValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                                <asp:TextBox ID="txtBenefitName" Style="width: 93%" runat="server" MaxLength="100" Height="30"></asp:TextBox>
                                                            </td>

                                                            <td style="width:100px;">
                                                                <div style="padding-bottom: 5px">Relation</div>
                                                                <asp:DropDownList ID="ddlBenefitRelation" Style="width: 100px;" Height="40" runat="server" DataSourceID="SqlDataSourceRelationship" DataTextField="Relationship" DataValueField="Relationship" onchange="CheckRelation(this.value)"></asp:DropDownList>
                                                            </td>
                                                            <td style="width:100px;">
                                                                <div style="padding-bottom: 5px">Share (%)<asp:Label ID="lblBenefitSharePercentageValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                                <asp:TextBox ID="txtBenefitSharePercentage" Style="width: 100px;" MaxLength="5" runat="server" Height="30"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <div style="padding-bottom: 5px">Remarks<asp:Label ID="lblBenefitRemarksValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                                <asp:TextBox ID="txtBenefitRemarks" Style="width: 93%"  runat="server" Height="30"></asp:TextBox>
                                                            </td>
                                                            <td style="vertical-align: middle; padding-top: 10px; text-align: center">
                                                                <asp:ImageButton ID="btnAddBenfit" runat="server" ImageUrl="~/App_Themes/functions/add-icon.png" Width="20px" Height="20px" OnClick="btnAddBenfit_Click" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="2%"></td>
                                                            <td colspan="6" >
                                                                <div id="divBenfit" runat="server">
                                                                    <asp:GridView ID="gvBenifit" runat="server" AutoGenerateColumns="false" Width="100%" OnSelectedIndexChanging="gvBenifit_SelectedIndexChanging" OnRowDeleting="gvBenifit_RowDeleting" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                                        <SelectedRowStyle BackColor="#00BFFF" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                   <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                      <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete-icon.png" Width="20px" Height="20px"  CommandName="delete" CausesValidation="false" OnClientClick="return confirm('Confirm Delete!!!');" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                               
                                                                         <Columns>
                                                                            <asp:TemplateField HeaderText="ID Type" ControlStyle-Width="120px" Visible="false">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIDType" runat="server" Text='<%# Eval("id_Type") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        
                                                                         <Columns>
                                                                            <asp:TemplateField HeaderText="ID Number" ControlStyle-Width="120px">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIDCard" runat="server" Text='<%# Eval("id_Card") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                         <Columns>
                                                                            <asp:TemplateField HeaderText="Full Name" ControlStyle-Width="300px">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("full_Name") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                         <Columns>
                                                                            <asp:TemplateField HeaderText="Relation" ControlStyle-Width="120px">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRelation" runat="server" Text='<%# Eval("relationship") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                         <Columns>
                                                                            <asp:TemplateField HeaderText="Shared (%)" ControlStyle-Width="120px">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPercentage" runat="server" Text='<%# Eval("percentage") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                       <Columns>
                                                                            <asp:TemplateField HeaderText="Remarks" ControlStyle-Width="200px">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                         <Columns>
                                                                            <asp:TemplateField HeaderText="ID" ControlStyle-Width="200px" Visible="false">
                                                                                <HeaderStyle CssClass="gHeader" />
                                                                                <ItemStyle CssClass="gRow" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("app_benefit_item_id") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                                
                                                            </td>
                                                            <td width="2%"></td>
                                                        </tr>
                                                         <tr>
                                                            <td width="2%"></td>
                                                            <td colspan="6" style="vertical-align:middle; text-align:right;">
                                                                <asp:Button ID="btnUpdateBenefit" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnUpdateBenefit_Click" />
                                                            </td>
                                                            <td width="2%"></td>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                

                            </div>
                            <div class="tab-pane" id="health">
                                <asp:UpdatePanel ID="upHealth" runat="server">
                                    <ContentTemplate>
                                        <div class="message">
                                            <asp:Label ID="lblMessageHealth" runat="server"></asp:Label>
                                        </div>
                                        <%--Miscellaneous Health--%>
                                        
                                         <table style= " width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                             <tr>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Body Detail</h3>
                                                </td>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Body List</h3></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px;">
                                            
                                                     <table border="0">
                                                        <tr>
                                                            <td width="19%" style="text-align: right">Person</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBodyPerson" runat="server"></asp:DropDownList>
                                                            </td>
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td width="19%" style="text-align: right">Height:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtHeightLife1" Width="90%" runat="server" MaxLength="3"></asp:TextBox>
                                                            </td>
                                                            <td>cm.</td>
                                                        </tr>
                                                        <tr>
                                                            <td width="19%" style="text-align: right">Weight:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtWeightLife1" Width="90%" runat="server" MaxLength="3"></asp:TextBox>
                                                            </td>
                                                            <td>kg.</td>
                                                   
                                                        </tr>
                                                        <tr>
                                               
                                                            <td width="50%" style="text-align: right;">Weight change in pass 6 months?</td>
                                                            <td colspan="2">
                                                                <asp:RadioButtonList ID="rblWeightChangeLife1" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal" CellSpacing="-1" AutoPostBack="true" OnSelectedIndexChanged="rblWeightChangeLife1_SelectedIndexChanged" >
                                                                    <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Increase" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Descrease" Value="2"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="19%" style="text-align: right;">
                                                                <asp:Label ID="lblWeightChangeReasonLife1" Text="Reason:"  Visible="false" runat="server"></asp:Label></td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtWeightChangeReasonLife1"  Visible="false" runat="server"  Width="83.6%" Height="30px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td  style="text-align:center;">
                                                                <asp:Button ID="btnAddWeight" runat="server" Text="Add" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnAddWeight_Click" />
                                                                <asp:Button ID="btnClearWeight" runat="server" Text="Clear" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnClearWeight_Click"/>
                                                            </td>
                                                        </tr>          
                                                    </table>
                                             
                                                </td>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px; padding-left:10px;">
                                                    <asp:GridView ID="gvBody" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanging="gvBody_SelectedIndexChanging" OnRowDeleting="gvBody_RowDeleting" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                                    <SelectedRowStyle BackColor="#00BFFF" />
                                                                     <Columns>
                                                                                    
                                                                         <asp:TemplateField>
                                                                             <HeaderStyle CssClass="gHeaderLeft" />
                                                                             <ItemStyle CssClass="gRow" />
                                                                             <ItemTemplate>
                                                                                 <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/App_Themes/functions/tick-icon.png" Width="20px" Height="20px"  CommandName="select" CausesValidation="false" />
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                     </Columns>
                                                                            
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete-icon.png" Width="20px" Height="20px"  CommandName="delete" CausesValidation="false" OnClientClick=" return confirm('Confirm Delete!!!');" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Level" ControlStyle-Width="10px" Visible="true">
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Application ID" ControlStyle-Width="200px" Visible="false">
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAppID" runat="server" Text='<%# Eval("app_register_id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Weight" ControlStyle-Width="100px" Visible="true">
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("weight") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Height" ControlStyle-Width="100px" Visible="true">
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblHeight" runat="server" Text='<%# Eval("height") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Weight Is Changed" ControlStyle-Width="200px" Visible="false">
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWeightIsChanged" runat="server" Text='<%# Eval("is_weight_changed") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Reason" ControlStyle-Width="200px" Visible="true">
                                                                            <HeaderStyle CssClass="gHeaderRight" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblReason" runat="server" Text='<%# Eval("reason") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="ID" ControlStyle-Width="200px" Visible="false">
                                                                            <HeaderStyle CssClass="gHeader" />
                                                                            <ItemStyle CssClass="gRow" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                        <div style="padding-top:10px;">
                                                           
                                                             <asp:Button ID="btnBodySave" runat="server" Text="Save" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnBodySave_Click" OnClientClick="return confirm('Confirm update!!');" />
                                                        </div>
                                                </td>
                                                
                                            </tr>
                                        </table>
                                        <table style="width:100%;">
                                            <tr>
                                                <td width="2%"></td>
                                                <td colspan="3" style=" padding-top:10px;"">
                                                    <%--Gridview Question--%>
                                                    <asp:GridView ID="GvQA" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="gvPersonalInfo_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Question" ControlStyle-Width="70%" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle  CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>' style="text-align:left; width:100%; padding-left:10px; position:relative;"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                           <%-- <asp:BoundField DataField="Question" HeaderText="Question" ItemStyle-Width="84%" HeaderStyle-HorizontalAlign="Center" />--%>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblAnswerLife1" Text="Owner" runat="server" Width="80px"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife1" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife1" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife1" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderRight" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblAnswerLife2" Text="Life1" runat="server"  Width="80px"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife2" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife2" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife2" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>
                                                </td>
                                                <td width="2%"></td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <!--- Section Sqldatasource--->
    <asp:SqlDataSource ID="SqlDataSourceProductType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_Type_ID, CONVERT(varchar,Product_Type_ID + replicate(' ', 50 - len(Product_Type_ID))) + '. ' + Product_Type AS myproducttype FROM dbo.Ct_Product_Type where Product_Type_ID != 4 ORDER BY Product_Type_ID"></asp:SqlDataSource>
    <%--<asp:SqlDataSource ID="SqlDataSourceUnderwritingStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Status_Code, Detail FROM dbo.Ct_Underwrite_Table ORDER BY Status_Code"></asp:SqlDataSource>--%>
    <asp:SqlDataSource ID="SqlDataSourceCountry" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Country_Name FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceNationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Nationality "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Payment_Mode_List]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceRelationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Relationship_List]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Question ORDER BY Seq_Number ASC "></asp:SqlDataSource>
  
    <asp:SqlDataSource ID="SqlDataSourceChannel" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Channel where Status = 1 order by Created_On ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePlan" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Product_ID, convert(varchar,Pay_Year) +','+ convert(varchar,Assure_Year) as 'year' from Ct_Product_Life where Product_ID Like 'NFP%'"></asp:SqlDataSource>
      <!--- End Section Sqldatasource--->
</asp:Content>

