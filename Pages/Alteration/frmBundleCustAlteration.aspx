<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmBundleCustAlteration.aspx.cs" Inherits="Pages_Alteration_frmBundleCustAlteration" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="dv_main" runat="server">
    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Search Bundle Customer</h3>
                    
                </div>
                <div class="panel-body">
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblCustomerNoSearch" runat="server" Text="Customer No"></asp:Label>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustomerNoSearch" runat="server"></asp:TextBox>
                                        </td>

                                        <td style="vertical-align: top">
                                            <asp:Button ID="btnSearch" CssClass=" btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click"  />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="panel-heading">
                    <h3 class="panel-title">Customer Information</h3>
                </div>
                <div>

                    <table id="tbl_search" style="margin: 5px;">
                        <tr>

                            <td>
                                <asp:HiddenField ID="hdfCustomerId" runat="server" />
                                <asp:HiddenField ID="hdfContactId" runat="server" />
                                <asp:HiddenField ID="hdfAddressId" runat="server" />
                                  <asp:HiddenField ID="hdfBenId" runat="server" />
                                <asp:Label ID="lblCustomerNo" runat="server" Text="Customer NO."></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomerNo" runat="server"></asp:TextBox>
                            </td>
                             <td style="padding-left:20px;">
                                <asp:Label ID="lblLastNameKh" runat="server" Text="Last Name KH"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameKh" runat="server"></asp:TextBox>
                            </td>
                             <td style="padding-left:20px;">
                                <asp:Label ID="Label31" runat="server" Text="First Name KH"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstNameKh" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblLastNameEn" runat="server" Text="Last Name En"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameEn" runat="server"></asp:TextBox>
                            </td>
                            <td  style="padding-left:20px;">
                                <asp:Label ID="lblCusname" runat="server" Text="First Name En"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstNameEn" runat="server"></asp:TextBox>
                            </td>
                             <td  style="padding-left:20px;">
                                <asp:Label ID="Label32" runat="server" Text="Gender"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDOb" runat="server" Text="DOB"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:HiddenField  ID="hdfDOB" runat="server" />
                                <asp:TextBox ID="txtDOB" runat="server" CssClass="datepicker"></asp:TextBox>
                            </td>
                             <td style="padding-left:20px;">
                                <asp:Label ID="lblIdType" runat="server" Text="ID Type"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIdType" runat="server"></asp:DropDownList>
                            </td>
                             <td>
                                <asp:Label ID="Label33" runat="server" Text="ID NO"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdNo" runat="server" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                <asp:Label ID="lblNationality" runat="server" Text="Nationality"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlNationality" runat="server"></asp:DropDownList>
     
                            </td>
                             <td  style="padding-left:20px;">
                                <asp:Label ID="lblOccupation" runat="server" Text="Occupation"></asp:Label>
                               
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlOccupation" runat="server"></asp:DropDownList>
                            </td>    
                             <td  style="padding-left:20px;">
                                <asp:Label ID="Label34" runat="server" Text="Marital Status"></asp:Label>
                               
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlMaritalStatus" runat="server"></asp:DropDownList>
                            </td>                        
                        </tr>
                     
                        <tr>
                          
                            <td colspan="6" style="text-align: right;">
                                  <asp:Button ID="btnEditCustomer" CssClass=" btn-primary" runat="server" Text="Edit" OnClick="btnEditCustomer_Click" />
                                <asp:Button ID="btnUpdateCust" CssClass=" btn-primary" runat="server" Text="Update" OnClick="btnUpdateCust_Click" />
                            </td>
                        </tr>
                    </table>

                </div>

                 <div class="panel-heading">
                    <h3 class="panel-title">Customer Contact</h3>
                </div>
                <div>

                    <table id="Table1" style="margin: 5px;">
                        <tr>

                            <td>
                               
                                <asp:Label ID="Label1" runat="server" Text="Phone Number"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                            </td>
                             <td style="padding-left:20px;">
                                <asp:Label ID="Label2" runat="server" Text="Phone Number 2"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNumber2" runat="server"></asp:TextBox>
                            </td>
                             <td style="padding-left:20px;">
                                <asp:Label ID="Label35" runat="server" Text="Phone Number 3"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNumber3" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Email"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            </td>
                            <td  style="padding-left:20px;">
                                <asp:Label ID="Label4" runat="server" Text="Email 2"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail2" runat="server"></asp:TextBox>
                            </td>
                             <td  style="padding-left:20px;">
                                <asp:Label ID="Label36" runat="server" Text="Email 3"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail3" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                          
                            <td colspan="6" style="text-align: right;">
                                 <asp:Button ID="btnEditContact" CssClass=" btn-primary" runat="server" Text="Edit" OnClick="btnEditContact_Click" />
                                <asp:Button ID="btnUpdateContact" CssClass=" btn-primary" runat="server" Text="Update" OnClick="btnUpdateContact_Click" />
                            </td>
                        </tr>
                    </table>

                </div>

                 <div class="panel-heading">
                    <h3 class="panel-title">Customer Address</h3>
                </div>
                <div>

                    <table id="Table2" style="margin: 5px; width:auto;">
                        <tr>

                            <td>
                                
                                <asp:Label ID="Label11" runat="server" Text="Address"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" Width="450px" ></asp:TextBox>
                            </td>
                            
                        </tr>
                       
                       <tr>
                            <td >
                                <asp:Label ID="Label12" runat="server" Text="Remarks"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarksAddress" runat="server"  Width="450px"></asp:TextBox>
                            </td>
                       </tr>
                        <tr>
                          
                            <td colspan="2" style="text-align: right;">
                                 <asp:Button ID="btnEditAddress" CssClass=" btn-primary" runat="server" Text="Edit" OnClick="btnEditAddress_Click" />
                                <asp:Button ID="btnUpdateAddress" CssClass=" btn-primary" runat="server" Text="Update" OnClick="btnUpdateAddress_Click" />
                            </td>
                        </tr>
                    </table>

                </div>

                 <div class="panel-heading">
                    <h3 class="panel-title">Beneficiary</h3>
                </div>
                <div>

                    <table id="Table3" style="margin: 5px;">
                        <tr>
                             <td>
                               
                                <asp:Label ID="Label7" runat="server" Text="Policy Number"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPolicyNumber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPolicyNumber_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>

                            <td>
                               
                                <asp:Label ID="Label21" runat="server" Text="Full Name"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtBenFullName" runat="server"></asp:TextBox>
                            </td>
                             <td style="padding-left:20px;">
                                <asp:Label ID="Label22" runat="server" Text="Gender"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBenGender" runat="server"></asp:DropDownList>
                            </td>
                             <td>
                               
                                <asp:Label ID="Label5" runat="server" Text="Age"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtBenAge" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label23" runat="server" Text="Relation"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBenRelation" runat="server"></asp:DropDownList>
                            </td>
                            <td  style="padding-left:20px;">
                                <asp:Label ID="Label24" runat="server" Text="Percentage"></asp:Label>
                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtPercentage" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label25" runat="server" Text="Address"></asp:Label>
                                
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtBenAddress" runat="server" ></asp:TextBox>
                            </td>
                            
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Remarks"></asp:Label>
                                
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtBenRemarks" runat="server" ></asp:TextBox>
                            </td>
                            
                        </tr>
                       
                        <tr>
                          
                            <td colspan="6" style="text-align: right;">
                                 <asp:Button ID="btnEditBen" CssClass=" btn-primary" runat="server" Text="Edit" OnClick="btnEditBen_Click" />
                                <asp:Button ID="btnUpdateBen" CssClass=" btn-primary" runat="server" Text="Update" OnClick="btnUpdateBen_Click" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
          <asp:UpdateProgress ID="upg" runat="server" AssociatedUpdatePanelID="UP">
        <ProgressTemplate>
            <div class="my_progress">
                <div class="tr"></div>
                <div class="main">
                    <div class="dhead">
                        <h2>PROCESSING</h2>
                    </div>
                    <p>
                        <img id="loader" src="../../App_Themes/images/loader.gif" alt="Progressing" />
                    </p>
                    <p>Please wait...</p>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
</asp:Content>




