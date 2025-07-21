<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmupload_g_card.aspx.cs" Inherits="Pages_Card_frmupload_g_card" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    
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
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Upload Group Card</h3>

                </div>
                <div class="panel-body">
                    <table class="table-layout">

                        <tr style="vertical-align: middle;">

                            <td style="padding: 10px 10px;">
                                <div style="position: absolute; margin-left: 500px;">
                                    <a style="color: blue;" href="Group_certificate_upload_template.xlsx"><span>Download Template</span></a>
                                </div>
                                <span style="color: red;">*</span> Select a file :
                    <asp:FileUpload ID="fupload" runat="server" Enabled="true" />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" CssClass="btn btn-primary" />

                            </td>
                        </tr>


                        <tr style="vertical-align: middle;">
                            <td style="vertical-align: middle;">

                                <div id="div_record_found" runat="server" style="color: #e99400; border-bottom: 1px groove #d5d5d5; padding: 10px; display: block; font-family: 'trebuchet MS', verdana, sans-serif;"></div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="container">
                                    <div id="tab-content" style="padding-top: 10px;">
                                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                                            <li class="active"><a href="#div_valid" data-toggle="tab" style="color: green;">Valid Data</a></li>
                                            <li><a href="#div_invalid" data-toggle="tab" style="color: red;">Invalid Data</a></li>

                                        </ul>
                                        <div id="my-tab-content" class="tab-content">
                                            <div class="tab-pane active" id="div_valid">
                                                <asp:GridView ID="gv_valideList" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center">
                                                    <SelectedRowStyle BackColor="#EEFFAA" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="No" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer Name Kh">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerNameKh" runat="server" Text='<%# Eval("CustomerNameKh") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Customer Name En">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerNameEn" runat="server" Text='<%#Eval("CustomerNameEn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="DOB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDob" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Gender">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Certificate Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCertificateNumber" runat="server" Text='<%# Eval("CertificateNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Effective Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("EffectiveDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Expiry Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Maturity Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Convert.ToDateTime(Eval("MaturityDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString()=="0" ? "DELETE": Eval("Status").ToString()=="1" ? "ADD": "Invalid" %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                                <div style="text-align: center;">

                                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="return confirm('Do you want to process saving data?');" OnClick="btnSave_Click" Visible="false" />
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="div_invalid">


                                                <asp:GridView ID="gv_invalid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center">
                                                    <SelectedRowStyle BackColor="#EEFFAA" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="No" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer Name Kh">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerNameKh" runat="server" Text='<%# Eval("CustomerNameKh") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Customer Name En">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerNameEn" runat="server" Text='<%#Eval("CustomerNameEn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="DOB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDob" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Gender">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Certificate Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCertificateNumber" runat="server" Text='<%# Eval("CertificateNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Effective Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("EffectiveDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Expiry Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Maturity Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Convert.ToDateTime(Eval("MaturityDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString()=="0" ? "DELETE": Eval("Status").ToString()=="1" ? "ADD": "Invalid"  %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                                <div style="text-align: center;">

                                                    <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click" Visible="false" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div id="dv_Valide">
                                </div>

                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
             <asp:PostBackTrigger ControlID="export_excel" />
                <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>



</asp:Content>


