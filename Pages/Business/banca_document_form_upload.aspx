<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="banca_document_form_upload.aspx.cs" Inherits="Pages_Business_banca_document_form_upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
              <div id="dv_main" runat="server" class="panel panel-default">
                <div class="panel panel-default" id="dvSearch" runat="server">
                  
                    <div class="panel-heading">
                        <h3 class="panel-title">Search Application</h3>
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

                                            <td>Application No
                                            <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApplicationNO" runat="server" placeholder="APPxxxxxxx"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>Customer Name (English)
                                            <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCusName" runat="server"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>

                                            <td>Policy No
                                            <span style="color: red;">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPolicyno" runat="server" placeholder="CERSOxxxxxxx"></asp:TextBox>
                                            </td>
                                            <td colspan="3"></td>
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
               
                      <div class="panel panel-default" id="dv_gird" runat="server">
                    <div class="panel-heading">

                        <h3 class="panel-title">Information </h3>

                    </div>
                
                   <%-- <div style="overflow-x: scroll; height: 400px; width: 100%;">--%>
                        <asp:GridView ID="gv_form_app" CssClass="grid-layout" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnRowCommand="gv_form_app_RowCommand">
                            <SelectedRowStyle BackColor="#EEFFAA" />

                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="No" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblapplicationID" runat="server" Text='<%#Eval("APPLICATION_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application Number" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApplicationNo" Text='<%#Eval("APPLICATION_NUMBER") %>' runat="server"></asp:Label>                        
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
                                <asp:TemplateField HeaderText="UPLOAD APPLICATION FORM">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="lblupload" Text="UPLOAD" Style="color: blue; padding: 2px 2px 2px 2px; font-weight:800" runat="server" CommandName="CMD_UPLORD" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>

                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                         </div>
                     <%-- block upload form --%>
                    <div class="panel panel-default" id="dvUpload" runat="server">
                        <div class="panel-heading">
                            <h3 class="panel-title">Upload Application Form</h3>
                        </div>
                        <div class="panel-body">
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>Policy Number
                                            <span style="color: red;">*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                                                     <asp:TextBox ID="txtApp_ID" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfappid" runat ="server" />
                                                    <asp:HiddenField ID="hdfappNumber" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfileapp" Text="Application Form" runat="server"></asp:Label>
                                                    <span style="color: red;">*</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:FileUpload ID="fUploadappform" runat="server" />
                                                    <asp:LinkButton ID="lbtApp" runat="server" OnClick="lbtApp_Click" ></asp:LinkButton>
                                                    <asp:HiddenField ID="hdfPathAPP" runat="server" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfilecert" Text="Certificate" runat="server"></asp:Label>
                                                    <span style="color: red;">*</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:FileUpload ID="fUploadcert" runat="server" />
                                                     <asp:LinkButton ID="lblCert" runat="server" OnClick="lblCert_Click" ></asp:LinkButton>
                                                    <asp:HiddenField ID="hdfPathCERT" runat="server" />
                                                     
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfileidcard" Text="ID Card" runat="server"></asp:Label>
                                                    <span style="color: red;">*</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:FileUpload ID="fUploadidcard" runat="server" />
                                                    <asp:LinkButton ID="lblIDCard" runat="server" OnClick="lblIDCard_Click" ></asp:LinkButton>
                                                    <asp:HiddenField ID="hdfPahtID" runat="server" />
                                                     
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfilepayslip" Text="PaySlip" runat="server"></asp:Label>
                                                    <span style="color: red;">*</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:FileUpload ID="fUploadpayslip" runat="server" />
                                                    <asp:LinkButton ID="lblPaySlip" runat="server" OnClick="lblPaySlip_Click" ></asp:LinkButton>
                                                    <asp:HiddenField ID="hdfPathPaySlip" runat="server" />
                                                     
                                                </td>

                                            </tr>
                                            <tr>
                                                <td colspan="3">

                                                    <p style="color: red; font-size: 12px; font-style: italic;">
                                                        * Please Select PDF File For All File.</p>
                                                </td>
                                                <tr>
                                                    <td colspan="4" style="text-align:right"><%--  <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />--%>
                                                        <asp:Button ID="btnPreview" runat="server" CssClass="btn btn-primary" OnClick="btnPreview_Click" Style="margin-left: 30px;" Text="Preview" />
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>   

                    <%-- Block Preview --%>  
                   <div class="panel panel-default" id="dv_preview" runat="server">
                    <div class="panel-heading">

                        <h3 class="panel-title">Document Preview </h3>

                    </div>
                         <asp:GridView ID="gv_preview" CssClass="grid-layout" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                             OnRowCommand="gv_preview_RowCommand"   >
                                <SelectedRowStyle BackColor="#EEFFAA" />

                                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc. Code" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocCode" runat="server" Text='<%#Eval("DocumentCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Original Doc. Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOriginalDocName" Text='<%#Eval("OriginalDocumentName") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc. Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocName" Text='<%#Eval("DocumentName") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Doc. Type" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocType" runat="server" Text='<%#Eval("DocumentType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc. Path" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocPath" runat="server" Text='<%#Eval("DocumentPath") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField HeaderText="View">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblViewAll" Text="View All" Style="color: white; padding: 2px 2px 2px 2px;" runat="server" CommandName="CMD_VIEW_ALL" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <asp:LinkButton ID="lblupload" Text="View" Style="color: blue; padding: 2px 2px 2px 2px;" runat="server" CommandName="CMD_VIEW" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                         
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                          <asp:Literal ID="fView" runat="server" Visible="false" />
                                        <table>
                                       
                                        <tr>
                                            <td colspan="4" style="text-align: right">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-primary" Style="margin-left: 30px;"/>
                                                <asp:Button ID="btnUpload" Text="Confirm" runat="server" OnClick="btnUpload_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                                            </td>
                                        </tr>
                                    </table>
                             
                         </div>
                  <%--show Result --%> 
                  <div class="panel panel-default" id="dv_show" runat="server">
                    <div class="panel-heading">

                        <h3 class="panel-title">Show Result </h3>

                    </div>
                
                   <%-- <div style="overflow-x: scroll; height: 400px; width: 100%;">--%>
                        <asp:GridView ID="gv_show_upload" CssClass="grid-layout" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                            <SelectedRowStyle BackColor="#EEFFAA" />

                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="No" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>  
                                  <asp:TemplateField HeaderText="Application ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblappID" Text='<%#Eval("APPLICATION_ID") %>' runat="server"></asp:Label>                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>      
                                <asp:TemplateField HeaderText="Document Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldoc_code" Text='<%#Eval("DOC_CODE") %>' runat="server"></asp:Label>                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>      
                                <asp:TemplateField HeaderText="Document Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldoc_name" Text='<%#Eval("DOC_NAME") %>' runat="server"></asp:Label>                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Document Type" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldoc_type" runat="server" Text='<%#Eval("DOC_TYPE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Path" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldoc_path" runat="server" Text='<%#Eval("DOC_PATH") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Created_On" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreatedon" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created_By" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%#Eval("CREATED_BY") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                              
                            </Columns>

                        </asp:GridView>
                         </div>
                  </div>            
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="btnPreview" />
                  <asp:PostBackTrigger ControlID="btnUpload" />
           </Triggers>

        </asp:UpdatePanel>

</asp:Content>

