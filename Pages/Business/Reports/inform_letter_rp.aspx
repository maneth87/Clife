<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="inform_letter_rp.aspx.cs" Inherits="Pages_Business_Reports_inform_letter_rp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:Label runat="server" ID="lbl"></asp:Label>
    <div id="message" runat="server" style=" text-align: center; vertical-align: middle; color: #000; font-weight: bold; padding: 10px; position: absolute; width: 96%; border-radius: 10px; background-color: red;"></div>

</asp:Content>

