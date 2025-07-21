<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="view_company.aspx.cs" Inherits="Pages_GTLI_view_company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>

    <%--print--%>
    <script src="../../Scripts/jquery.print.js"></script>

    <ul class="toolbar">
        <li>
            <!-- Button to trigger modal new business form -->
            <input type="button" onclick="PrintCompanyList();"  style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
        <li>
            <!-- Button Search-->
            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>
    </ul>

    <style>
        @media print {
            @page {
                size: A4;
                margin: 5px;
                /*font-family: Arial;*/
                font-size: 8pt;
                width: 210mm;
                height: 297mm;
                
            }

            html {
                margin: 10px;
            }

            body {
                font-family: Arial;
                font-size: 8pt;
            }

            .hideelement {
                visibility: hidden;
            }

          
        }

          .ui-autocomplete {
                z-index: 5000;
            }
    
    </style>

    <script type="text/javascript">
        //Company Name autocomplete
        PageMethods.GetCompanyName(function (results) {

            $("#txtCompany").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(results, function (item) {
                        return matcher.test(item);
                    }));
                }
            });
        });

        //Print
        function PrintCompanyList() {

            $(".printable").print();
        }

        //Show Search Form
        function ShowSearchForm() {
            $("#txtCompanyName").val("");
            $("#Main_txtFrom").val("");
            $("#Main_txtTo").val("");

            $("#myModalSearchCompanyList").modal("show");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Companies</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <div class="printable" align="center">

                <table width="100%" style="font-family: Arial; font-size: 16px;">
                    <tr>
                        <td style="text-align: center">                           
                                <span style="font-weight: bold">
                                    <asp:Label ID="lblCompanyName" runat="server" Text="Cambodia Life Insurance PLC."></asp:Label>
                                </span><br />                        
                                <span style="font-weight: bold; margin-top: -7px">
                                    <asp:Label ID="lblTitle" runat="server" Text="Group Term Life: Company List"></asp:Label></span>
                        </td>
                    </tr>
                </table>
                <div align="left">
                <asp:Label ID="lblCount" runat="server" CssClass="hideelement"></asp:Label>
                </div>
                             
                <asp:GridView ID="gvCompany" runat="server" HeaderStyle-Font-Size="8pt" AutoGenerateColumns="False" Width="100%" DataKeyNames="GTLI_Company_ID" AllowPaging="True" PageSize="100" AllowSorting="True" OnPageIndexChanged="gvCompany_PageIndexChanged" OnPageIndexChanging="gvCompany_PageIndexChanging" OnSorting="gvCompany_Sorting">
                    <Columns>
                        <asp:BoundField DataField="Company_Name" HeaderText="Company Name" SortExpression="Company_Name">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="260px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Type_Of_Business" SortExpression="Type_Of_Business" HeaderText="Business Type">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="170px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Latest_Contact_Name" SortExpression="Latest_Contact_Name" HeaderText="Contact Person">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Latest_Contact_Phone" HeaderText="Phone Contact">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Latest_Contact_Email" HeaderText="Email">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Company_Address" HeaderText="Address">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="300px" />
                        </asp:BoundField>

                    </Columns>
                </asp:GridView>
            </div>
            <asp:SqlDataSource ID="BusinessDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>"
                ProviderName="<%$ ConnectionStrings:ApplicationDBContext.ProviderName %>"
                SelectCommand="SELECT GTLI_Business_ID, Business_Name FROM Ct_GTLI_Business ORDER BY Business_Name ASC"></asp:SqlDataSource>
            <asp:HiddenField ID="hdfCurrentSearchOption" runat="server" />
        </div>
    </div>
        <%--Modal Search Company List Form --%>
    <div id="myModalSearchCompanyList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchCompanyListHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Company List Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
          <table width="100%">
            <tr>
                <td>Business:
                </td>
                <td>
                    <asp:DropDownList ID="ddlBusiness" runat="server" AppendDataBoundItems="True" DataSourceID="BusinessDataSource" DataTextField="Business_Name" DataValueField="GTLI_Business_ID" Width="99%">
                        <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>                
                <td width="10%">Company:
                </td>
                <td width="80%">
                    <asp:TextBox ID="txtCompany" runat="server" Width="96%" ClientIDMode="Static"></asp:TextBox>
                </td>
                
            </tr>

        </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"    />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Company List  Search--%>
</asp:Content>

