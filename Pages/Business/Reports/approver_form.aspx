<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approver_form.aspx.cs" Inherits="Pages_Business_Reports_approver_form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .tbl {
            width:500px;
            height:auto;
            border:2px solid #393991;
        }
        .colheader {
            background-color:#393991;
            padding: 5px 5px 5px 5px;
            width:250px;
            color:white;
        }
        .colsub {
            padding: 5px 5px 5px 5px;
            width:250px;
            
        }
        .colbtn {
             padding: 5px 5px 5px 5px;
             vertical-align:middle;
             text-align:center;
             border-top:1px solid #393991;
        }
        .btn {
            width:100px;
            padding: 5px 5px 5px 5px;
            border-radius:5px 5px;
        }
        .ddl {
             width:250px;
            padding: 5px 5px 5px 5px;

        }
        .message {
             padding: 5px 5px 5px 5px;
             color:red;
            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        
        <div id="approver" runat="server" >
        <asp:UpdatePanel ID="upApprover" runat="server">
            <ContentTemplate>
                <table class="tbl">
                <tr>
                    <td colspan="2" class="colheader">Approver</td>
                </tr>
                    <tr>
                        <td colspan="2" >
                            <div id="message" runat="server" class="message" ></div>
                        </td>
                    </tr>
                <tr>
                    <td class="colsub">
                        <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        Position
                    </td>
                    <td class="colsub">
                        <asp:DropDownList ID="ddlPosition" runat="server" AutoPostBack="true" CssClass="ddl" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  class="colsub">
                        <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        Full Name
                    </td>
                    <td  class="colsub">
                        <asp:DropDownList ID="ddlFullName" runat="server" AutoPostBack="false" CssClass="ddl"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="colbtn">
                        <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn" OnClick="btnClear_Click" />
                        <asp:Button ID="btnOk" Text="Ok" runat="server"  CssClass="btn" OnClick="btnOk_Click"/>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnClear" />
                 <asp:PostBackTrigger ControlID="btnOk" />
                 <asp:PostBackTrigger ControlID="ddlFullName" />
                 <asp:PostBackTrigger ControlID="ddlPosition" />
                
            </Triggers>
        

        </asp:UpdatePanel>
            
        </div>
    
    </div>
    </form>
</body>
</html>
