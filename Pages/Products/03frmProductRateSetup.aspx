<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="03frmProductRateSetup.aspx.cs" Inherits="Pages_Products_03frmProductRateSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <div id="div_main" runat="server" class="panel panel-default">
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
        <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="panel-heading">
                    <h2 class="panel-title">SETUP PRODUCT RATE</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH PRODUCT RATE</h3>
                        </div>
                        <div class="panel-body">
                            <table>

                                <tr>

                                    <td>Product ID: 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductNameSearch" placeholder="ANY WORDS OF PRODUCT ID" runat="server" Width="300px"></asp:TextBox>
                                    </td>


                                    <td style="vertical-align: middle; float: right;">
                                        <asp:Button ID="btnSearch" Text="SEARCH" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click"  />
                                    </td>
                                </tr>


                            </table>

                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div style="float:right; position:static; padding-right:15px; margin-top:10px;">
                            <asp:LinkButton ID="ibtnAdd" runat="server" Text="ADD NEW" OnClick="ibtnAdd_Click"></asp:LinkButton>
                        </div>
                        <div class="panel-heading">
                            <h3 class="panel-title">PRODUCT RATE LIST
                            </h3>

                        </div>

                        <div class="panel-body">
                            <asp:GridView ID="gvParam" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvParam_RowEditing" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvParam_PageIndexChanging"  AlternatingRowStyle-BackColor = "#C2D69B">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" Width="80px" runat="server" Text='<%#Eval("PRODUCT_RATE_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PRODUCT ID" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductId" Width="80px" runat="server" Text='<%#Eval("PRODUCT_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GENDER" Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" Width="150px" runat="server" Text='<%#Eval("GENDER").ToString()=="1" ? "M":"F" %>'></asp:Label>
                                            <asp:HiddenField id="hdfGender" runat="server" Value='<%#Eval("GENDER") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PAY MODE" Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayMode"  Width="60px" runat="server" Text='<%#Eval("PAY_MODE_STRING") %>'></asp:Label>
                                             <asp:HiddenField ID="hdfPayMode" runat="server" Value='<%#Eval("PAY_MODE") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                  
                                     <asp:TemplateField HeaderText="AGE MIN">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeMin" Width="20px" runat="server" Text='<%# Eval("AGE_MIN")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="AGE MAX">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeMax" Width="20px" runat="server" Text='<%# Eval("AGE_MAX")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="SA MIN">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaMin" Width="20px" runat="server" Text='<%# Eval("SUM_ASSURE_START")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="SA MAX">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaMax" Width="20px" runat="server" Text='<%# Eval("SUM_ASSURE_END")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="RATE PER">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRatePer" Width="150px" runat="server" Text='<%# Eval("RATE_PER")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="RATE TYPE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRateType" Width="150px" runat="server" Text='<%# Eval("RATE_TYPE")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="RATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" Width="150px" runat="server" Text='<%# Eval("RATE")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="REMARKS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Width="150px" runat="server" Text='<%# Eval("Remarks")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/App_Themes/functions/edit.png" CommandName="edit" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                        </div>

                    </div>

                    <div class="panel panel-default" id="divAdd" runat="server">

                        <div class="panel-heading">
                            <h3 class="panel-title">ADD NEW / UPDATE </h3>

                        </div>

                        <div class="panel-body"  id="divAddContent">
                           
                                <table id="tblAddContent" >
                                    <tr>
                                        <td>Product ID: <span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                          <asp:DropDownList ID="ddlProduct" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>Gender:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGender" runat="server">
                                                <asp:ListItem Value="" Text="--- Select ---"></asp:ListItem>
                                                 <asp:ListItem Value="0" Text="F"></asp:ListItem>
                                                 <asp:ListItem Value="1" Text="M"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                         <td>Paymode:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlPaymode" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                   
                                   
                                    <tr>
                                        <td>Age Min:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeMin"  runat="server" ></asp:TextBox>
                                        </td>
                                         <td>Age Max:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtAgeMax"  runat="server" ></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                     <tr>
                                        <td>Sum Assure Min:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSaMin"  runat="server" ></asp:TextBox>
                                        </td>
                                         <td>Sum Assure Max:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtSaMax"  runat="server" ></asp:TextBox>
                                        </td>
                                          <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Rate Per:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRatePer"  runat="server" ></asp:TextBox>
                                        </td>
                                         <td>Rate Type:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                           <asp:DropDownList ID="ddlRateType" runat="server">
                                                <asp:ListItem Value="" Text="--- Select ---"></asp:ListItem>
                                                 <asp:ListItem Value="VALUE" Text="VALUE"></asp:ListItem>
                                                 <asp:ListItem Value="RATE" Text="RATE"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>Rate:<span style="color: red; font-weight: bold;">*</span></td>
                                        <td>  <asp:TextBox ID="txtRate"  runat="server" ></asp:TextBox></td>
                                    </tr>
                                   
                                    <tr>
                                        <td>Remarks:
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtRemarks"  runat="server" Width="97%"></asp:TextBox>
                                        </td>
                                       
                                    </tr>
                                    <tr>

                                        <td style="vertical-align: central; text-align: center;" colspan="6">
                                            <asp:Button ID="btnSave" Text="SAVE" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                             <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-primary" OnClick="btnDelete_Click" />
                                             <asp:Button ID="btnCancel" Text="CANCEL" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>


                                </table>
                            
                        </div>

                    </div>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>



