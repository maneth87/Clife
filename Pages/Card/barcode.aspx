<%@ Page Title="Clife System | Card => Barcode" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="barcode.aspx.cs" Inherits="barcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <script>
        //Validate integer
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        //Validate decimal number
        function ValidateTextDecimal(j) {

            var msg = j.value;
            var w;
            var nokta = msg.indexOf(".");
            var ind;

            for (w = 0; w < msg.length; w++) {

                ind = msg.substring(w, w + 1);
                if (ind < "0" || ind > "9") {

                    if (nokta > 0)
                        if (w == nokta) continue;

                    msg = msg.substring(0, w);
                    j.value = msg;
                    break;
                }

            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">       
    

    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Generate Barcode</h3>
        </div>
        <div class="panel-body">
            <table width="100%">
                <tr>
                    <td>Product:</td>
                    <td>
                        <asp:DropDownList ID="ddlProduct" runat="server" Width="96.3%">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                             <asp:ListItem  Value="T1011">Term One</asp:ListItem>
                             <asp:ListItem Value="FT013">Flexi Term</asp:ListItem>
                        </asp:DropDownList>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" Text="*" ControlToValidate="ddlProduct" InitialValue="0" ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>

                    </td>                 
                                              
                </tr>
                <tr>
                    <td style="width:100px">Quantity:</td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="6" onkeyup="ValidateNumber(this);" Width="95%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" Text="*" ControlToValidate="txtQuantity"  ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>

                    </td>
                   
                </tr>
                <tr>
                    <td>Sum Insured:</td>
                    <td>
                        <asp:TextBox ID="txtSumInsured" runat="server" MaxLength="11" onkeyup="ValidateNumber(this);" Width="95%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red" Text="*" ControlToValidate="txtSumInsured"  ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>

                    </td>
                  
                </tr>
                <tr>
                    <td>Premium:</td>
                    <td>
                        <asp:TextBox ID="txtPremium" runat="server" MaxLength="16" onkeyup="ValidateTextDecimal(this);" Width="95%"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" Text="*" ControlToValidate="txtPremium"  ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>

                    </td>
                                        
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnGenerate" Height="35px" Text="Generate Barcodes" runat="server" OnClick="btnGenerate_Click"></asp:Button></td>

                </tr>
               
            </table>
           </div>
        </div>
</asp:Content>

