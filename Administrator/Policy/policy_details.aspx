<%@ Page Title="Clife | Administrator => Policy Details" Language="C#" MasterPageFile="~/Administrator/Admin.master" AutoEventWireup="true" CodeFile="policy_details.aspx.cs" Inherits="Administrator_Policy_policy_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" OnClick="ImgBtnRefresh_Click" ImageUrl="~/App_Themes/functions/refresh.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImageBtnSearch" runat="server" data-toggle="modal" data-target="#mySearchPolicy" ImageUrl="~/App_Themes/functions/search.png" />


        </li>


    </ul>



</asp:Content>

<%-- <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Policy</h3>
                </div>
                <div class="panel-body">                    
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Cv_Basic_Policy WHERE Policy_Status_Type_ID = 'IF' ORDER BY Policy_Number DESC"></asp:SqlDataSource>
                </div>
            </div>--%>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Policy Details</h3>
                </div>
                <div class="panel-body">

                    <asp:GridView ID="gvPolicy" runat="server" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowDataBound="gvPolicy_RowDataBound" DataSourceID="dsPolicy" DataKeyNames="Policy_ID" AllowPaging="True" PageSize="50">
                        <Columns>
                            <asp:CommandField ShowEditButton="True" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                            <asp:BoundField DataField="Policy_Number" HeaderText="Policy #" SortExpression="Policy_Number"  ReadOnly="true">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Customer_ID" HeaderText="Customer ID" SortExpression="Customer_ID"  ReadOnly="true">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Product_ID" HeaderText="Product ID" SortExpression="Product_ID" ReadOnly="true" >
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Agreement_Date" HeaderText="Agreement Date" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Agreement_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Issue_Date" HeaderText="Issue Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Issue_Date" ItemStyle-HorizontalAlign="Center">

                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Effective_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Maturity_Date" HeaderText="Maturity Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Maturity_Date" ItemStyle-HorizontalAlign="Center">

                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                           <%-- <asp:BoundField DataField="Age_Insure" HeaderText="Age" SortExpression="Age_Insure" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Pay_Year" HeaderText="Pay Year" SortExpression="Pay_Year" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Pay_Up_To_Age" HeaderText="Pay Up To Age" SortExpression="Pay_Up_To_Age" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Assure_Year" HeaderText="Assure Year" SortExpression="Assure_Year" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Assure_Up_To_Age" HeaderText="Assure Up To Age" SortExpression="Assure_Up_To_Age">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Created_On" HeaderText="Created On" SortExpression="Created_On" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Created_By" HeaderText="Created By" SortExpression="Created_By" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>


                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="Pager" HorizontalAlign="Center" />
                    </asp:GridView>



                    <asp:SqlDataSource ID="dsPolicy" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Ct_Policy_Number.Policy_Number, Ct_Policy.* FROM Ct_Policy INNER JOIN Ct_Policy_Number ON Ct_Policy.Policy_ID = Ct_Policy_Number.Policy_ID ORDER BY Policy_Number DESC;" UpdateCommand="UPDATE Ct_Policy SET Agreement_Date = @Agreement_Date, Effective_Date = @Effective_Date, Maturity_Date = @Maturity_Date, Issue_Date = @Issue_Date
WHERE Policy_ID = @Policy_ID;">
                        <UpdateParameters>
                            <asp:Parameter Name="Agreement_Date" />
                            <asp:Parameter Name="Effective_Date" />
                            <asp:Parameter Name="Maturity_Date" />
                            <asp:Parameter Name="Issue_Date" />
                            <asp:Parameter Name="Policy_ID" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>
            </div>


            <!-- Modal Search Policy -->
            <div id="mySearchPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SAppNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Policy No</a></li>
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Customer Name</a></li>
                    </ul>

                    <div class="tab-content" style="height: 60px; overflow: hidden;">
                        <div class="tab-pane active" id="SAppNo">

                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Policy No:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumberSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </div>
                        <div class="tab-pane" style="height: 70px;" id="SCustomerName">

                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Surname:</td>
                                    <td>
                                        <asp:TextBox ID="txtSurnameSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Firstname:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstnameSearch" Width="95%" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                            </table>

                        </div>


                    </div>

                </div>
                <div class="modal-footer">
                    <%--<input type="button" class="btn btn-primary" style="height: 27px;" data-dismiss="modal" value="Search"  />--%>
                    <asp:Button ID="btnSearchPolicy" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Search" OnClick="btnSearchPolicy_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Search Application-->

           
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearchPolicy" />
        </Triggers>


    </asp:UpdatePanel>
</asp:Content>

