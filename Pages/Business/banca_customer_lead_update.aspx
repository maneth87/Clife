<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="banca_customer_lead_update.aspx.cs" Inherits="Pages_Business_banca_customer_lead_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <style>
       

        .my_progress {
            width: 100%;
            height: 100%;
            color: #FFFFFF;
            position: absolute;
            float: left;
            overflow: hidden;
            top: 0px;
            left: 0px;
        }

            .my_progress div.tr {
                background-color: black;
                -moz-opacity: 0.7;
                opacity: 0.7;
                filter: alpha(opacity=70);
                position: absolute;
                top: 0px;
                left: 0px;
                z-index: 998;
                width: 100%;
                height: 100%;
            }

            .my_progress div.main {
                position: relative;
                top: 30%;
                width: 400px;
                z-index: 999;
                margin: auto;
                border: 2px solid #2b0557;
                border-radius: 5px;
                -moz-border-radius: 5px;
                -webkit-border-radius: 5px;
                -khtml-border-radius: 5px;
                -moz-box-shadow: 0 0 50px #fff;
                -webkit-box-shadow: 0 0 50px #fff;
                text-align: center;
                background-color: White;
                color: Black;
            }

                .my_progress div.main div.dhead {
                    text-align: left;
                    background-image: url(../images/top_nav_bg.png);
                }
    </style>



    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
            <ul class="toolbar">

                <li>
                    <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtnSave_Click" OnClientClick="return confirm('Do you want to update?');"/>
                </li>
            </ul>

            <h1>Update Customer Lead</h1>
            <table id="tblAppContent" class="table-layout" style="border: 1px solid #21275b;">
                <tr>

                    <td style="width: 75%; vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Customer Information</h3>

                    </td>

                </tr>
                <tr>
                    <td>
                         <table style="margin-top: 15px; margin-bottom: 10px;">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblIDType" runat="server" Text="ID Type:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlIDType" runat="server" class="span3">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="0">ID Card</asp:ListItem>
                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                        <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span> </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblIDNo" runat="server" Text="ID No.:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIDNumber" runat="server" class="span3"></asp:TextBox>
                                    <span style="color: red;">*</span> </td>
                                  <td style="text-align: right">
                                    <asp:Label ID="lblClientNameKh" runat="server" Text="Client Name In Khmer:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClientNameKh" runat="server" class="span3"  MaxLength="30"></asp:TextBox>
                                    <span style="color: red;">*</span> </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblClientNameEn" runat="server" Style="margin-left: 20px;" Text="Client Name In English:"></asp:Label>
                                </td>
                                 <td>
                                    <asp:TextBox ID="txtClientNameEn" runat="server" class="span3" MaxLength="30"></asp:TextBox>
                                    <span style="color: red;">*</span> </td>
                            </tr>
                           
                            <tr>
                                 <td style="text-align: right">
                                    <asp:Label ID="lblGender" runat="server" Text="Gender:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGender" runat="server" class="span3">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Male</asp:ListItem>
                                        <asp:ListItem Value="0">Female</asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span> </td>
                                 <td style="text-align: right">
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text="Date of Birth (DD-MM-YYYY):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="span3 datepicker"></asp:TextBox>
                                    <span style="color: red;">*</span> </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblPhoneNumber" runat="server" Text="PhoneNumber:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhoneNumber" runat="server" class="span3"></asp:TextBox>
                                    <span style="color: red;">*</span> </td>
                            </tr>
                             <tr>
                                  <td style="text-align: right">
                                    <asp:Label ID="lblVillage" runat="server" Text="Village:"></asp:Label>
                                </td>
                                 <td>
                                     <asp:TextBox ID="txtVillageEn" runat="server" class="span3"  placeholder="Village"></asp:TextBox>
                                 </td>
                                  <td style="text-align: right">
                                    <asp:Label ID="lblCommune" runat="server" Text="Commune:"></asp:Label>
                                </td>
                                 <td>
                                       <asp:TextBox ID="txtCommuneEn" runat="server" class="span3"  placeholder="Commune"></asp:TextBox>
                                 </td>
                                  <td style="text-align: right">
                                    <asp:Label ID="lblDistrict" runat="server" Text="District:"></asp:Label>
                                </td>
                                 <td>
                                      <asp:TextBox ID="txtDistrictEn" runat="server" class="span3"  placeholder="Distict"></asp:TextBox>
                                 </td>
                                  <td style="text-align: right">
                                    <asp:Label ID="lblProvince" runat="server" Text="Province:"></asp:Label>
                                </td>
                                 <td>
                                       <asp:TextBox ID="txtProvinceEn" runat="server" class="span3" placeholder="Province"></asp:TextBox>
                                 </td>
                             </tr>
                        </table>
                    </td>
                </tr>
                 <tr>

                    <td style="width: 75%; vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Update Status</h3>

                    </td>

                </tr>
                <tr>
                    <td>
                        <table style="margin-top: 15px; margin-bottom: 10px;">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" class="span3" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                     
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span> </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks:"></asp:Label>
                                </td>
                                <td>
                                   <%-- <asp:TextBox ID="txtStatusRemarks" runat="server" class="span6"></asp:TextBox>--%>
                                     <asp:DropDownList ID="ddlStatusRemarks" runat="server" class="span6" >
                                     
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span> </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>



            <asp:Label ID="lblError" runat="server"></asp:Label>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnSave" />
        </Triggers>
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
</asp:Content>


