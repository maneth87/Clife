<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="banca_view_upload_document_form.aspx.cs" Inherits="Pages_Business_banca_view_upload_document_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <style>
        .CheckboxList input {
            float: left;
            clear: both;
        }

        .GridPager a, .GridPager span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background-color: #21275b; /*#A1DCF2;*/
            color: white; /*#000;*/
            border: 1px solid #3AC0F2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:scriptmanager id="ScriptManager1" runat="server"></asp:scriptmanager>
    <asp:updateprogress id="upg" runat="server" associatedupdatepanelid="UP">
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
    </asp:updateprogress>
    <asp:updatepanel id="up" runat="server">
        <ContentTemplate>
            <div id="dv_main" runat="server" class="panel panel-default">
                <div class="panel panel-default" id="dvSearch" runat="server">

                    <div class="panel-heading">
                        <h3 class="panel-title">Search Certificate Form</h3>
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

                                            <td>Certificate No :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcertNO" runat="server"></asp:TextBox>
                                            </td>

                                            <td>Date Type :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddltype" runat="server" >
                                                     <asp:ListItem Value="" Text="---Select---" />
                                                    <asp:ListItem Value="IS" Text="Issue Date" />
                                                     <asp:ListItem Value="EF" Text="Effective Date" />
                                                     <asp:ListItem Value="CR" Text="Upload Date" />

                                                </asp:DropDownList>                                                                                              
                                            </td>
                                   
                                             <td>From Date :
                                            </td>
                                            <td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                                </td>
                                            </td>
                                            <td>To Date  :                                      
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                            </td>                                                                      
                                        </tr>
                                        <tr>
                                 <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" CssClass="span2">
                                </asp:DropDownList>

                            </td>
                              <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged" CssClass="span2"></asp:DropDownList>
                            </td>
                                        </tr>
                                      <tr>


                               <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelLocation" runat="server" Text="Branch Code:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;" colspan="9">                             
                                <table style="width: 100%; border: 1px solid gray;" id="tblUser" runat="server">
                                    <tr>
                                        <td style="padding: 5px;">

                                            <asp:CheckBoxList ID="cblChannelLocation" runat="server" RepeatDirection="Horizontal" RepeatColumns="12" CssClass="CheckboxList" Width="100%" CellSpacing="5" AutoPostBack="true" OnSelectedIndexChanged="cblChannelLocation_SelectedIndexChanged"></asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                                          <td colspan="10"  style="float:right"></td>
                                            <td style="vertical-align: top">
                                                <asp:Button ID="btnSearch" CssClass=" btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                            </td> 
                                              <td style="vertical-align: top">
                                                <asp:Button ID="btnClear" CssClass="btn-primary" runat="server" Text="Clear" OnClick="btnClear_Click" />
                                            </td>      
                        </tr>

                                       
                                  </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
      
                <div class="panel panel-default" id="dv_grid" runat="server">
                    <div class="panel-heading">

                        <h3 class="panel-title">Show Information </h3>

                    </div>
                    <asp:GridView ID="gv_view" CssClass="grid-layout" AllowPaging="true" PageSize="30" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                     OnRowCommand="gv_view_RowCommand" OnPageIndexChanging="gv_view_PageIndexChanging">
                        <SelectedRowStyle BackColor="#EEFFAA" />

                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Doc ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldoc_id" runat="server" Text='<%#Eval("doc_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                             <asp:TemplateField HeaderText="Application ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblapplicationID" runat="server" Text='<%#Eval("APPLICATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application Number" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblappNumber" runat="server" Text='<%#Eval("APPLICATION_NUMBER") %>'></asp:Label>
                                  <%--  <asp:HiddenField ID="hdfAppID" runat="server"  Value='<%#Eval("APPLICATION_ID") %>' />--%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Policy No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblpolicyNo" runat="server" Text='<%#Eval("POLICY_NUMBER") %>'></asp:Label>                                  
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Customer Name Eng" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcustomernameeng" runat="server" Text='<%#Eval("CUSTOMER_NAME_ENG") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name Kh" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcustomernamekh" runat="server" Text='<%#Eval("CUSTOMER_NAME_KH") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" Visible="true">
                                    <ItemTemplate>                                     
                                        <asp:Label ID="lblreviewstatus" runat="server" Text='<%#Eval("STATUS")%>'></asp:Label>  
                                        <%--<asp:HiddenField ID="hdfReviewStatus" runat="server"  Value='<%#Eval("REVIEWED_STATUS") %>'/>   --%>                                                                 
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblPreview" Text="Preview" Style="color: blue; padding: 2px 2px 2px 2px; font-weight: 800" runat="server" CommandName="CMD_PREVIEW" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>                           
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>                           
                        </Columns>

                    </asp:GridView>
                  </div>
                    <div class="panel panel-default" id="dv_preview" runat="server">
                        <div class="panel-heading">
                    <h3 class="panel-title">RESULT-
                        <asp:Label ID="lblCount" runat="server"></asp:Label>
                         
                    </h3>
                </div>
                        <%--<div class="panel-heading">

                            <h3 class="panel-title">Document Preview </h3>
                     </div>--%>
                         <asp:GridView ID="gv_preview" CssClass="grid-layout" AllowPaging="true" PageSize="24" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                             OnRowCommand="gv_preview_RowCommand" OnRowDataBound="gv_preview_RowDataBound">
                                <SelectedRowStyle BackColor="#EEFFAA" />

                                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                <Columns>                               
                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Doc.ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldocID" runat="server" Text='<%#Eval("Doc_ID") %>'></asp:Label>                                                                                   
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="App.ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblappID" runat="server" Text='<%#Eval("Application_ID") %>'></asp:Label>                                                                                   
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>          
                                    <asp:TemplateField HeaderText="Doc. Code" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocCode" runat="server" Text='<%#Eval("Doc_code") %>'></asp:Label>                                                                                      
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>                                
                                    <asp:TemplateField HeaderText="Doc. Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocName" Text='<%#Eval("Doc_name") %>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdfDocPath" runat="server" Value='<%#Eval("Doc_path") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>                               
                                   <asp:TemplateField HeaderText="Action">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblViewAll" Text="ViewAll" Style="color: white; padding: 2px 2px 2px 2px;" runat="server" CommandName="CMD_VIEW_ALL" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                        </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblView" Text="view" Style="color: blue; padding: 2px 2px 2px 2px; font-weight: 800" runat="server" CommandName="CMD_VIEW" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>                           
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />                               
                            </asp:TemplateField>                          
                                       <asp:TemplateField HeaderText="Status">                                                                                                                                                                                                  
                                            <ItemTemplate>                                             
                                                <asp:DropDownList ID="ddlcomfirm" runat="server">
                                                    <asp:ListItem Text="-----Select------" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Confirm" Value="confirm"></asp:ListItem>
                                                    <asp:ListItem Text="Reject" Value="reject"></asp:ListItem>
                                              </asp:DropDownList>
                                                <asp:HiddenField ID="hdfoldStatus" runat="server" value='<%#Eval("REVIEWED_STATUS") %>'/>
                                            </ItemTemplate>                                                             
                                        <ItemStyle HorizontalAlign="Center" /> 
                                   </asp:TemplateField>  
                                     <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtremarks"  runat="server" Text='<%#Eval("REVIEWED_REMARK") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdfremarks" runat="server"/>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>   
                                     <asp:TemplateField HeaderText="Reviewed_On" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreviewOn" Text='<%#Eval("Reviewed_On") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField> 
                                     <asp:TemplateField HeaderText="Reviewed_By" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreviewBy" Text='<%#Eval("Reviewed_By") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>                                              
                                </Columns>
                            </asp:GridView>                   
                          <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" OnClick="btnUpdate_Click" Style="margin-left:30px;" Text="Update" />                                                                   
                   </div>   
                     
                                   
                              
                </div>
                </div>     

            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnPreview" />--%>
                  <asp:PostBackTrigger ControlID="btnUpdate" />
           </Triggers>
    </asp:updatepanel>
</asp:Content>

