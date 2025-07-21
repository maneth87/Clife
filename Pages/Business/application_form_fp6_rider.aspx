<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="application_form_fp6_rider.aspx.cs" Inherits="Pages_Business_application_form_fp6_rider" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upApplicationNew" runat="server">
        <ContentTemplate>
                <ul class="toolbar"  style="display: none;">
                   
                    <li>
                         <asp:ImageButton ID="ImgBtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png"  CausesValidation="false" OnClick="ImgBtnClear_Click" style="display: none;"/>
                    </li>
                    <li>
                        <div style="display: none;">
                            <asp:Button ID="btnSave" runat="server" />
                        </div>
                        
                    </li>
                </ul>
         <!-- Add new application modal -->
         
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
        .message {
            color:red;
            vertical-align:middle;
            padding-left:30px;
            padding-bottom:10px;
            text-align:center;
            font-weight:bold;
        }
        .gRow {
            
           vertical-align:middle;
           text-align:center;
           border-color:gray;
           border-width:1px;
           border-style:solid;
        }
        .gHeader {
           vertical-align:middle;
           text-align:center;
           font-weight:bold;
           background-color:#6A5ACD;
           border-color:#6A5ACD;
           color:white;
        }
        .gHeaderLeft {
           vertical-align:middle;
           text-align:center;
           font-weight:bold;
           background-color:#6A5ACD;
           border-color:#6A5ACD;
           color:white;
           border-top-left-radius:5px;
        }
        .gHeaderRight {
           vertical-align:middle;
           text-align:center;
           font-weight:bold;
           background-color:#6A5ACD;
           border-color:#6A5ACD;
           color:white;
           border-top-right-radius:5px;
        }
        .gRadio {
            vertical-align:middle;
            /*text-align:center;*/
            /*width:100px;*/
        }
    </style>

    <%--Javascript for bootstrap--%>
    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>



    <%-- Form Design Section--%>
    <h1>Application Form For Rider</h1>

    <table id="tblAppContent" class="table-layout">
        <tr>
            <td>
                <div class="container">
                    
                    <div>
                        <asp:Button runat="server" ID="btnGoBack" Text="<< Go Back" style="border:none; text-decoration:underline; color:blue; display:none;" OnClick="btnGoBack_Click" CausesValidation="false" />
                        <asp:ImageButton runat="server" ID="ibtnBack" ImageUrl="~/App_Themes/functions/back-icon.png" OnClick="ibtnBack_Click" CausesValidation="false" style="width:35px; height:35px; margin-left:10px; margin-top:10px;" ToolTip="Back" />
                        <div style="float:right; vertical-align:middle; padding-top:20px;">
                            <asp:ImageButton ID="imgSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="imgSave_Click" CausesValidation="false" />
                            <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/App_Themes/functions/delete.png" OnClick="imgDelete_Click" CausesValidation="false" />
                        </div>
                    </div>
                    
                    <div id="application_tab" class="tab-content">
                        <div class="message" style="padding-top:10px;">
                            <asp:Label ID="lblMessageApplication" Text="" runat="server"></asp:Label>
                        </div>
                        
                  </div>
                 
                </div>
                
            </td>
        </tr>

       <tr>
           <td style="padding-left:10px; padding-top:10px;">Policy Number:<asp:TextBox ID="txtPolicyNumber" Enabled="false" ReadOnly="true" runat="server"></asp:TextBox></td>
           
       </tr>

        <tr>
            <td colspan="2">
                <div class="container">

                    <div id="tab-content" style="padding-top: 10px;">
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active"><a href="#personalinformation" data-toggle="tab">Personal Information</a></li>
                            <li><a href="#insuranceplan" data-toggle="tab">Premium Detail</a></li>
                            <li><a href="#jobhistory" data-toggle="tab">Job History</a></li>
                            <li><a href="#health" data-toggle="tab">Miscellaneolus and Health Details</a></li>
                        </ul>
                        <div id="my-tab-content" class="tab-content">
                          


                            <%--life insured 2 --%>
                                <div class="tab-pane active" id="personalinformation">
                                    <asp:UpdatePanel ID="upPersonalInformation" runat="server">
                                        <ContentTemplate>
                                            <div class="message">
                                                <asp:Label ID="lblMessagePersonalInfo" runat="server"></asp:Label>
                                            </div>
                                            <table  style="width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                
                                                <tr>
                                                    <td style="width: 40%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                        <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Personal Information</h3>
                                                    </td>
                                                    <td style="width: 60%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                        <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Personal Information List</h3>

                                                    </td>
                                                </tr>
                                                 
                                                <tr>
                                                    <td style="width: 40%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px;">
                                                        <table style="width:100%;" >
                                                            <tr>
                                                                <td style="width: 40%; text-align: right">Rider:</td>
                                                                 <td style="width: 60%">
                                                                    <asp:DropDownList ID="ddlRiderType" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlRiderType_SelectedIndexChanged" >
                                                                        <asp:ListItem Value="">.</asp:ListItem>
                                                                        <asp:ListItem Value="2">Spouse</asp:ListItem>
                                                                        <asp:ListItem Value="3">Kid 1</asp:ListItem>
                                                                        <asp:ListItem Value="4">Kid 2</asp:ListItem>
                                                                        <asp:ListItem Value="5">Kid 3</asp:ListItem>
                                                                        <asp:ListItem Value="6">Kid 4</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                               <td style="width: 40%; text-align: right">SumInsured</td>
                                                                <td style="width: 60%">
                                                                    <asp:TextBox runat="server" ID="txtRiderSumInsured" ReadOnly="false" class="span2"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 40%; text-align: right">Marital Status:</td>
                                                                <td style="width: 60%">
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
                                                                <td style="width: 40%; text-align: right">I.D Type:</td>
                                                                <td style="width: 60%">
                                                                    <asp:DropDownList ID="ddlIDType" runat="server" class="span2">
                                                                        <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                                                        <asp:ListItem Value="2">Visa</asp:ListItem>
                                                                        <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right" class="auto-style3">I.D No.:</td>
                                                                <td style="width: 60%">
                                                                    <asp:TextBox ID="txtIDNumber" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                                                                </td>
                                                             </tr>
                           
                                                            <tr>
                                                                <td style="text-align: right">Surname in Khmer:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSurnameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                                </td>
                                                               
                                                            </tr>
                                                            <tr>
                                                                 <td style="text-align: right" class="auto-style3">First Name in Khmer:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFirstNameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Surname:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSurnameEng" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                                </td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right" class="auto-style3">First Name:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFirstNameEng" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Nationality:</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlNationality" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                                        <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="auto-style3"></td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right" class="auto-style2">Gender:</td>
                                                                <td class="auto-style2">
                                                                    <asp:DropDownList ID="ddlGender" class="span2" runat="server">
                                                                        <asp:ListItem Value="1">Male</asp:ListItem>
                                                                        <asp:ListItem Value="0">Female</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="auto-style5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Date of Birth:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDateBirth" runat="server" MaxLength="15" CssClass="span2 datepicker"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirth" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="auto-style3" style="padding-top: 2px;"></td>
                                                                <td></td>
                                                           </tr>
                                                           <tr>
                                                                <td style="text-align: right"></td>
                                                                <td style="color:red; vertical-align:top;">(DD/MM/YYYY)</td>
                                                                <td class="auto-style3" style="padding-top: 2px"></td>
                                                                <td></td>
                                                           </tr>
                                                             <tr>
                                                                <td style="text-align: right">Date of Entry:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDateOfEntry" runat="server" MaxLength="10" CssClass="span2 datepicker"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtEffectiveDate" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="auto-style3" style="padding-top: 2px"></td>
                                                                <td></td>
                                                           </tr>
                                                            <tr>
                                                                <td style="text-align: right">Effective Date:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEffectiveDate" runat="server" MaxLength="15" CssClass="span2 datepicker" Enabled="false" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtEffectiveDate" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="auto-style3" style="padding-top: 2px"></td>
                                                                <td></td>
                                                           </tr>
                                                            <tr>
                                                                <td style="text-align: right"></td>
                                                                <td style="color:red; vertical-align:top;">(DD/MM/YYYY)</td>
                                                                <td class="auto-style3" style="padding-top: 2px"></td>
                                                                <td></td>
                                                           </tr>
                                                           <tr>
                                                                <td style="width: 40%; text-align: right">Relationship to policy owner:</td>
                                                                 <td style="width: 60%">
                                                                    <asp:DropDownList ID="ddlRelationShip" runat="server" class="span2" >
                                                                        <asp:ListItem Value="">.</asp:ListItem>
                                                                        <asp:ListItem Value="WIFE">Wife</asp:ListItem>
                                                                        <asp:ListItem Value="HUSBAND">Husband</asp:ListItem>
                                                                        <asp:ListItem Value="SON">Son</asp:ListItem>
                                                                        <asp:ListItem Value="DAUGHTER">Daughter</asp:ListItem>
                                                                    </asp:DropDownList>
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
                                                    <td style="width: 60%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-right:5px; padding-left:5px; padding-top:10px;">
                                                        <table style="width:100%; margin-bottom: 10px;" >
                                                            <tr>
                                                               <td>
                                                                       <asp:GridView ID="gvPersonalInfo" runat="server" AutoGenerateColumns="false"  OnSelectedIndexChanging="gvPersonalInfo_SelectedIndexChanging" OnRowDeleting="gvPersonalInfo_RowDeleting" OnRowDataBound="GvQA_RowDataBound">
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
                                                                                    <asp:TemplateField HeaderText="Level" ControlStyle-Width="50px" Visible="false">
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
                                                                                    <asp:TemplateField HeaderText="Sur Name" ControlStyle-Width="200px">
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
                                                                                    <asp:TemplateField HeaderText="First Name" ControlStyle-Width="200px">
                                                                                        <HeaderStyle CssClass="gHeader" />
                                                                                        <ItemStyle CssClass="gRow" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFirstNameEn" runat="server" Text='<%# Eval("firstEnName") %>'></asp:Label>
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
                                                                  
                                                        
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-top:10px;">
                                                                    <asp:Button ID="btnUpdatePerson" runat="server" CausesValidation="false" Text="Save" CssClass="btn btn-primary" OnClick="btnUpdatePerson_Click" OnClientClick="return confirm('Confirm update!!!');" style="display:none;" />
                                                                </td>
                                                            </tr>
                                               
                                                        </table>
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
                                                <td style="width: 30%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Job History Detail</h3>
                                                </td>
                                                <td style="width: 70%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Job History List</h3></td>
                                            </tr>
                                            <tr>
                                                 <td style="width: 30%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px;">
                                                     <table width="100%">
                                                         <tr>
                                                             <td style="width: 50%; text-align: right">Policy Owner Type:</td>
                                                             <td style="width: 50%">
                                                                 <asp:DropDownList ID="ddlJobPolicyOwner" runat="server" Width="100%" >
                                                                 
                                                                 </asp:DropDownList>
                                                             </td>
                                                         </tr>
                                                        <tr>
                                                            <td style="width: 35%; text-align: right">Name of Employer:</td>
                                                            <td style="width: 65%">
                                                                <asp:TextBox ID="txtNameEmployerLife1" Width="90%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Nature of Business:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtNatureBusinessLife1" Width="90%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Role and Responsibility:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtRoleAndResponsibilityLife1" Width="90%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Current Position:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtCurrentPositionLife1" Width="90%" runat="server" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Anual Income (USD):</td>
                                                            <td>
                                                                <asp:TextBox ID="txtAnualIncomeLife1" Width="90%" runat="server" onkeyup="ValidateNumber(this);" MaxLength="12"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right">Address:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress" Width="90%" runat="server"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align:center;">
                                                                <div style="padding-top:10px; padding-bottom:10px;">
                                                                    <asp:Button ID="btnAddJobHistory" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddJobHistory_Click" CausesValidation="false" />
                                                                    <asp:Button ID="btnClearJobHistory" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClearJobHistory_Click" CausesValidation="false" />
                                                                </div>
                                                                
                                                            </td>
                                                          
                                                        </tr>

                                                    </table>
                                                 </td>
                                                
                                                <td style="width: 70%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-top:10px;">
                                                    <asp:GridView ID="gvJobHistory" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanging="gvJobHistory_SelectedIndexChanging" OnRowDeleting="gvJobHistory_RowDeleting" OnRowDataBound="GvQA_RowDataBound">
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
                                                            <asp:TemplateField HeaderText="Level" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Job ID" ControlStyle-Width="200px" Visible="false">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblJobID" runat="server" Text='<%# Eval("job_id") %>'></asp:Label>
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
                                                            <asp:TemplateField HeaderText="Employer Name" ControlStyle-Width="150px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmployerName" runat="server" Text='<%# Eval("employer_name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Nature of Business" ControlStyle-Width="200px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNatureOfBusiness" runat="server" Text='<%# Eval("nature_of_business") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="current_position" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCurrentPosition" runat="server" Text='<%# Eval("current_position") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Job Role" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblJobRole" runat="server" Text='<%# Eval("job_role") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Anual Income" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeaderRight" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAnualIncome" runat="server" Text='<%# Eval("anual_income") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Address" ControlStyle-Width="200px" Visible="false">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView> 
                                                    <div style="padding-top:10px;">
                                                        <asp:Button ID="btnSaveJob" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveJob_Click"  style="display:none;" />
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
                                        <table style="text-align:center; margin-left:10px; width:98%;">
                                            <tr>
                                                <td style="text-align:center;">
                                                    <asp:GridView ID="gvPremiumDetail" runat="server" AutoGenerateColumns="false" OnRowDataBound="GvQA_RowDataBound" >
                                                        <SelectedRowStyle BackColor="#00BFFF" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Level" ControlStyle-Width="30px" Visible="true">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" ControlStyle-Width="30px" Visible="false">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("person_id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                    
                                                       
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Application ID" ControlStyle-Width="200px" Visible="false">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicationID" runat="server" Text='<%# Eval("app_register_id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                      
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Full Name" ControlStyle-Width="200px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("full_name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Gender" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Birth Date" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("birth_date") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Date Of Entry" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Eval("effective_date") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Age" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAge" runat="server" Text='<%# Eval("age_insure") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Assure Year" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssureYear" runat="server" Text='<%# Eval("assure_year") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Assure Up To Age" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssureUpToAge" runat="server" Text='<%# Eval("assure_up_to_age") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Pay Year" ControlStyle-Width="60px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPayYear" runat="server" Text='<%# Eval("pay_year") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Pay Up To Age" ControlStyle-Width="100px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPayUpToYear" runat="server" Text='<%# Eval("pay_up_to_age") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Pay Mode" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPayMode" runat="server" Text='<%# Eval("pay_mode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Product ID" ControlStyle-Width="80px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                      
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sum Insured" ControlStyle-Width="50px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSumInsured" runat="server" Text='<%# Eval("sum_insure") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                      
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Premium" ControlStyle-Width="60px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPremium" runat="server" Text='<%# Eval("premium") %>' style="font-weight:bold;"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Original Premium" ControlStyle-Width="60px" Visible="true">
                                                                <HeaderStyle CssClass="gHeaderRight" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOriginalPremium" runat="server" Text='<%# Eval("original_premium") %>' style="font-weight:bold;"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                                      
                                                    </asp:GridView>
                                                             
                                                </td>
                                            </tr>
                                            <tr style="height:20px;">
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;">
                                                                Total Premium:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox id="txtTotalPremium" runat="server" ReadOnly="true" style="text-align:right; font-weight:bold;"></asp:TextBox>USD
                                                            </td>
                                                        </tr>
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
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Health Detail</h3>
                                                </td>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                                    <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Health Detail List</h3></td>
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
                                                        <tr style="height:10px;">
                                                            <td colspan="2"></td>
                                                        </tr>    
                                                    </table>
                                             
                                                </td>
                                                <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6; padding-left:10px; padding-top:10px;">
                                                    <asp:GridView ID="gvBody" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanging="gvBody_SelectedIndexChanging" OnRowDeleting="gvBody_RowDeleting" OnRowDataBound="GvQA_RowDataBound">
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
                                                        <div>
                                                            <br />
                                                             <asp:Button ID="btnBodySave" runat="server" Text="Save" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnBodySave_Click" OnClientClick="return confirm('Confirm update!!');"  style="display:none;" />
                                                        </div>
                                                </td>
                                                
                                            </tr>
                                            
                                        </table>
                                        
                                        <table style="width:100%; margin-top:10px;">
                                            <tr>
                                                <td width="2%"></td>
                                                <td colspan="3">
                                                    <ul id="answer_tabs" class="nav nav-tabs" data-tabs="tabs">
                                                        <li class="active"><a href="#spouse" data-toggle="tab">Spouse</a></li>
                                                         <li><a href="#kid1" data-toggle="tab">Kid 1</a></li>
                                                         <li><a href="#kid2" data-toggle="tab">Kid 2</a></li>
                                                         <li><a href="#kid3" data-toggle="tab">Kid 3</a></li>
                                                         <li><a href="#kid4" data-toggle="tab">Kid 4</a></li>
                                                    </ul>
                                                    <div id="answer-tab-content" class="tab-content">
                                                        <%--spouse--%>
                                                        <div class="tab-pane active" id="spouse">
                                                         <%--Gridview Question--%>
                                                    <asp:GridView ID="GvQA" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                         
                                                        <%--<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                                       <%-- <AlternatingRowStyle BackColor="#ffffff"/>--%>
                                                        <Columns>
                                                            
                                                            <asp:TemplateField HeaderText="Question" ControlStyle-Width="70%" Visible="true" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow"/>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuestion" runat="server"  Text='<%# Eval("Question") %>' Style="text-align:left; width:100%; padding-left:10px; position:relative; vertical-align:middle; " ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Spouse" ControlStyle-Width="150px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife1" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife1" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife1" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                     </div>
                                                         <%--End Spouse--%>

                                                         <%--kid1--%>
                                                        <div class="tab-pane " id="kid1">
                                                         <%--Gridview Question--%>
                                                    <asp:GridView ID="gvkid1" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                         
                                                        <%--<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                                       <%-- <AlternatingRowStyle BackColor="#ffffff"/>--%>
                                                        <Columns>
                                                            
                                                            <asp:TemplateField HeaderText="Question" ControlStyle-Width="70%" Visible="true" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuestion" runat="server"  Text='<%# Eval("Question") %>' Style="text-align:left; width:100%; padding-left:10px; position:relative; vertical-align:middle; " ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Kid 1" ControlStyle-Width="150px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                               <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife2" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife2" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife2" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>
                                                     </div>
                                                         <%--End kid1--%>

                                                        <%--kid2--%>
                                                        <div class="tab-pane " id="kid2">
                                                         <%--Gridview Question--%>
                                                    <asp:GridView ID="gvkid2" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                         
                                                        <%--<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                                       <%-- <AlternatingRowStyle BackColor="#ffffff"/>--%>
                                                        <Columns>
                                                            
                                                            <asp:TemplateField HeaderText="Question" ControlStyle-Width="70%" Visible="true" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuestion" runat="server"  Text='<%# Eval("Question") %>' Style="text-align:left; width:100%; padding-left:10px; position:relative; vertical-align:middle; " ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Kid 2" ControlStyle-Width="150px" Visible="true">
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                               <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife3" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife3" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife3" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>
                                                     </div>
                                                        <%--kid3--%>
                                                        <div class="tab-pane " id="kid3">
                                                         <%--Gridview Question--%>
                                                    <asp:GridView ID="gvkid3" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                         
                                                        <%--<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                                       <%-- <AlternatingRowStyle BackColor="#ffffff"/>--%>
                                                        <Columns>
                                                            
                                                            <asp:TemplateField HeaderText="Question" ControlStyle-Width="70%" Visible="true" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuestion" runat="server"  Text='<%# Eval("Question") %>' Style="text-align:left; width:100%; padding-left:10px; position:relative; vertical-align:middle; " ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            
                                                            <asp:TemplateField HeaderText="Kid 3" ControlStyle-Width="150px" Visible="true">
                                                                
                                                                <HeaderStyle CssClass="gHeader" />
                                                                <ItemStyle CssClass="gRow" />
                                                               <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife4" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife4" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife4" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>
                                                     </div>
                                                        <%--kid4--%>
                                                        <div class="tab-pane " id="kid4">
                                                         <%--Gridview Question--%>
                                                    <asp:GridView ID="gvkid4" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                         
                                                        <%--<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                                       <%-- <AlternatingRowStyle BackColor="#ffffff"/>--%>
                                                        <Columns>
                                                            
                                                            <asp:TemplateField HeaderText="Question" ControlStyle-Width="70%" Visible="true" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="gHeaderLeft" />
                                                                <ItemStyle CssClass="gRow" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuestion" runat="server"  Text='<%# Eval("Question") %>' Style="text-align:left; width:100%; padding-left:10px; position:relative; vertical-align:middle; " ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Kid 4" ControlStyle-Width="150px" Visible="true">
                                                                <HeaderStyle CssClass="gHeaderRight" />
                                                                <ItemStyle CssClass="gRow" VerticalAlign="Middle" HorizontalAlign="Center" />
                                                               <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rbtnlAnswerLife5" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Y" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="N" Value="2"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:Label ID="lblSeqNumberLife5" runat="server" Text='<%# Eval("Seq_Number") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblQuestionIDLife5" runat="server" Text='<%# Eval("Question_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>


                                                    </asp:GridView>
                                                     </div>
                                                    </div>
                                                     
                                                    
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
    <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Question_FP6_Child ORDER BY Seq_Number ASC "></asp:SqlDataSource>
  
    <asp:SqlDataSource ID="SqlDataSourceChannel" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Channel where Status = 1 order by Created_On ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePlan" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Product_ID, convert(varchar,Pay_Year) +','+ convert(varchar,Assure_Year) as 'year' from Ct_Product_Life where Product_ID Like 'NFP%'"></asp:SqlDataSource>
      <!--- End Section Sqldatasource--->
</asp:Content>

