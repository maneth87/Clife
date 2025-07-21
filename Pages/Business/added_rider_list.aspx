<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="added_rider_list.aspx.cs" Inherits="Pages_Business_added_rider_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        .link {
           
             text-decoration-color:#0026ff;
        }
            .link:hover {
                font-style:italic;
            }

    </style>
    <asp:UpdatePanel ID="uppanel" runat="server">
        <ContentTemplate>
                    <div>
        <asp:GridView ID="gvApplication" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowCommand="gvApplication_RowCommand">
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

                <asp:TemplateField HeaderText="Signature date" SortExpression="App_Date">
                    <ItemTemplate>
                        <asp:Label ID="lblSignature" runat="server" Text='<%# Eval("App_Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Product" SortExpression="Product_ID">
                    <ItemTemplate>
                        <asp:Label ID="lblProduct_ID" runat="server" Text='<%# Eval("Product_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Age" SortExpression="Age_Insure">
                    <ItemTemplate>
                        <asp:Label ID="lblAge_Insure" runat="server" Text='<%# Eval("Age_Insure") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Pay Year" SortExpression="Pay_Year">
                    <ItemTemplate>
                        <asp:Label ID="lblPay_Year" runat="server" Text='<%# Eval("Pay_Year") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Coverage" SortExpression="Assure_Year">
                    <ItemTemplate>
                        <asp:Label ID="lblAssure_Year" runat="server" Text='<%# Eval("Assure_Year") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Coverage to age" SortExpression="Assure_Year">
                    <ItemTemplate>
                        <asp:Label ID="lblAssure_Up_To_Age" runat="server" Text='<%# Eval("Assure_Up_To_Age") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Pay Up To Age" SortExpression="Pay_Up_To_Age">
                    <ItemTemplate>
                        <asp:Label ID="lblPay_Up_To_Age" runat="server" Text='<%# Eval("Pay_Up_To_Age") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Sum Insure" SortExpression="System_Sum_Insure">
                    <ItemTemplate>
                        <asp:Label ID="lblSystem_Sum_Insure" runat="server" Text='<%# Eval("System_Sum_Insure", "{0:N0}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Annual Premium" SortExpression="Rounded_Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblRounded_Amount" runat="server" Text='<%# Eval("Rounded_Amount", "{0:N0}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Original Premium" SortExpression="Original_Amount" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOriginal_Amount" runat="server" Text='<%# Eval("Original_Amount") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="System Premium" SortExpression="System_Premium">
                    <ItemTemplate>
                        <asp:Label ID="lblSystem_Premium" runat="server" Text='<%# Eval("System_Premium", "{0:N0}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPay_Mode_Code" runat="server" Text='<%# Eval("Pay_Mode")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Surname" SortExpression="Last_Name">
                    <ItemTemplate>
                        <asp:Label ID="lblLast_Name" runat="server" Text='<%# Eval("Last_Name") %>'></asp:Label>
                    </ItemTemplate>
                   <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="First Name" SortExpression="First_Name">
                    <ItemTemplate>
                        <asp:Label ID="lblFirst_Name" runat="server" Text='<%# Eval("First_Name") %>'></asp:Label>
                    </ItemTemplate>
                  <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Gender">
                    <ItemTemplate>
                        <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Birth Date" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBirth_Date" runat="server" Text='<%# Eval("Birth_Date") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="Nationality" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lbl_country_id" runat="server" Text='<%# Eval("Country_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Rider Type" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lbl_rider_type" runat="server" Text='<%# Eval("Rider_type") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtunderwrite" runat="server" Text="Underwriting" CommandName="underwriting" CssClass="link" ForeColor="#0026ff"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>

        </ContentTemplate>

    </asp:UpdatePanel>
    
</asp:Content>

