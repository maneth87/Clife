<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="help.aspx.cs" Inherits="Pages_Help_help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <div>
       <div id="div_message" runat="server" style="text-align: center; vertical-align: middle; margin-top: 10px; margin-bottom: 10px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;"></div>

        <asp:Literal ID="ltFile" runat="server"  ></asp:Literal>
    </div>
</asp:Content>

