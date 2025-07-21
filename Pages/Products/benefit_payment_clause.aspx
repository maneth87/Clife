<%@ Page Title="Clife | Core Data => Benefit Payment Clause" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="benefit_payment_clause.aspx.cs" Inherits="Pages_Products_benefit_payment_clause" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <script src="../../Scripts/tinymce/tinymce.min.js"></script>
    <ul class="toolbar">
        <li><asp:ImageButton ID="ImgBtnSave" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/save.png" CausesValidation="False" OnClick="ImgBtnSave_Click" /></li>


        </ul>
    
     <script type="text/javascript">

         //Tiny MCE Text Editor Initialize
         tinymce.init({
             selector: "textarea.tinyMCE",
             theme: "modern",
             external_filemanager_path: '../../Scripts/tinymce/plugins/filemanager/',
             width: "100%",
             height: 300,
             subfolder: "",
             plugins: [
                 "advlist autolink link image lists charmap print preview hr anchor pagebreak",
                 "searchreplace wordcount visualblocks visualchars code insertdatetime media nonbreaking",
                 "table contextmenu directionality emoticons paste textcolor filemanager"
             ],
             image_advtab: true,
             toolbar: "undo redo | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | styleselect forecolor backcolor | link unlink anchor | image media | print preview code"
         });

              
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
      
            <br />
            <br />
            <br />
    <div class="panel panel-default">
         <div class="panel-heading">
                    
                <h3 class="panel-title">Benefit Payment Clause</h3>
                </div>
        <div class="panel-body">
            <table style="width:100%">
                <tr>
                    <td>
                        Prouduct:<asp:RequiredFieldValidator ID="RequiredFieldValidatorProduct" runat="server" ErrorMessage="Require Product" InitialValue="0" ControlToValidate="ddlProduct" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>                    
                    <td>
                        <asp:DropDownList ID="ddlProduct" runat="server" Width="100%" AppendDataBoundItems="True" AutoPostBack="true" DataSourceID="SqlDataSourceProduct" DataTextField="En_Title" DataValueField="Product_ID" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" >
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                       
                    </td>
                </tr>
                <tr>
                    <td>
                        Benefit Clause:<asp:RequiredFieldValidator ID="RequiredFieldValidatorClause" runat="server" ErrorMessage="Require Benefit Clause" ControlToValidate="txtClause" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>                  
                </tr>
                <tr>                    
                    <td>
                        <asp:TextBox ID="txtClause" runat="server" CssClass="tinyMCE" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <br />
                        
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hdfBenefitPaymentClauseID" runat="server" />
    <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_ID, En_Title FROM Ct_Product ORDER BY En_Title ASC "></asp:SqlDataSource>
</asp:Content>

