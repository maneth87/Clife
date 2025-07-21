<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_banca_chart.aspx.cs" Inherits="Pages_Reports_micro_banca_chart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    
     <style>
        .star {
            color:red;
        }

     
         .GridPager a, .GridPager span
    {
        display: block;
        height: 15px;
        width: 15px;
        font-weight: bold;
        text-align: center;
        text-decoration: none;
       
    }
    .GridPager a
    {
        background-color: #f5f5f5;
        color: #969696;
        border: 1px solid #969696;
    }
    .GridPager span
    {
        background-color:#21275b; /*#A1DCF2;*/ 
        color:white;/*#000;*/ 
        border: 1px solid #3AC0F2;
    }
     .CheckboxList input {
            float: left;
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <h1>Policy Insurance Chart</h1>
    <table>
        <tr>
            <td style="border:2px solid #21275b; background-color:#21275b; color:white; font-weight:bold; font-size:12px; text-align:center;">Chart Filter</td>
            <td style="border:2px solid #21275b; background-color:#21275b; color:white; font-weight:bold; font-size:12px; text-align:center;">Chart Result</td>
        </tr>
        <tr>
            <td style="border:2px solid #21275b; width:25%;">
                <table>
                 
                    <tr>
                        <td style="padding-right: 20px;">
                            <asp:Label ID="lblIssuedDateF" runat="server" Text="Issued Date From:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtIssuedDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                            <span class="star">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 20px;">
                            <asp:Label ID="lblIssuedDateTo" runat="server" Text="Issued Date To:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtIssuedDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                            <span class="star">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right:20px;">
                           <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                    </td>
                    <td style="padding-right:20px;">
                       <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"  ></asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                          <td style="padding-right:20px;">
                           <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                     </td>
                      <td style="padding-right:20px;">
                            <asp:DropDownList ID="ddlChannelItem" runat="server"  ></asp:DropDownList>
                      </td>
                    </tr>
                      <tr>
                        <td>Chart Value:</td>
                        <td>
                            <asp:DropDownList ID="ddlChartValue" runat="server" OnSelectedIndexChanged="ddlChartValue_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Amount" Value="0"></asp:ListItem>
                                 <asp:ListItem Text="Policy" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                    </tr>
                     <tr>
                        <td>Chart Type:</td>
                        <td>
                            <asp:DropDownList ID="ddlChartType" runat="server" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <asp:Button ID="btnShowChart" Text="Show Chart" CssClass="btn btn-primary" runat="server" OnClick="btnShowChart_Click" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border:2px solid #21275b; width:70%;">
                <div style="width:100%;">
                    <asp:Chart ID="Chart1" runat="server" Width="900px"  Palette="None">
                    <Series>
                        <asp:Series Name="Series1" YValuesPerPoint="1" IsValueShownAsLabel="true">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                            <AxisX LineDashStyle="Dot"></AxisX>
                        </asp:ChartArea>

                    </ChartAreas>
                </asp:Chart>
                </div>
                
            </td>
        </tr>
    </table>
    <asp:Label ID="lblError" runat="server"></asp:Label>

</asp:Content>

