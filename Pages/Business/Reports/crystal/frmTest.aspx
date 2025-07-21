<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmTest.aspx.cs" Inherits="Pages_Business_Reports_crystal_frmTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:Label ID="lblNo" runat="server" Text="No"></asp:Label>
    <asp:TextBox runat="server" ID="txtNo"></asp:TextBox>

    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
    <asp:TextBox runat="server" ID="txtName"></asp:TextBox>

    <asp:Button runat="server" ID="insert" Text="Insert" OnClick="insert_Click" />
    <asp:Button runat="server" ID="Delete" Text="Delete" OnClick="Delete_Click" />
     <asp:Button runat="server" ID="Search" Text="Search" OnClick="Search_Click" />

    <div id="message"  runat="server"></div>
    
</asp:Content>

