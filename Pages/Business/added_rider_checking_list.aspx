<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="added_rider_checking_list.aspx.cs" Inherits="Pages_Business_added_rider_checking_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        .link {
           
             text-decoration-color:#0026ff;
             position:relative;
        }
            .link:hover {
                font-style:italic;
            }
            
    </style>
   

        <asp:UpdatePanel ID="uppanel" runat="server">
        <ContentTemplate>
                    <div>
        <asp:GridView ID="gvApplication" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center" OnRowCommand="gvApplication_RowCommand">
            <SelectedRowStyle BackColor="#EEFFAA" />

            <Columns>
                <asp:TemplateField HeaderText="Level" Visible="false">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("level") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AppID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblAppID" runat="server" Text='<%#Eval("App_Register_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Application no." SortExpression="App_Number">
                    <ItemTemplate>
                        <asp:Label ID="lblAppNumber" runat="server" Text='<%# Eval("App_Number") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Product" SortExpression="Product_ID">
                    <ItemTemplate>
                        <asp:Label ID="lblProduct_ID" runat="server" Text='<%# Eval("Product_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Rider Type" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lbl_rider_type" runat="server" Text='<%# Eval("Rider_type") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtPrintDataCheckinglist" runat="server" Text="Data Checking List" CssClass="link" ForeColor="#0026ff" CommandName="checking_list" ></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

