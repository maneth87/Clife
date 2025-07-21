<%@ Page Title="Clife | WING => Report" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_wing_account.aspx.cs" Inherits="Pages_Wing_report_wing_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">

    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>
        <li>
            <asp:ImageButton ID="ImgExportExcel" runat="server" ImageUrl="~/App_Themes/functions/download_excel.png" OnClick="ImgExportExcel_Click"  />
        </li>        
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearchWingAccount" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
       
    </ul>

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script>

        //Fucntion to check only one checkbox and highlight textbox
        function SelectSingleCheckBox(ckb, policy_wing_id) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");

                }
            }

            if (ckb.checked) {
                alert(policy_wing_id);

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");
            }
        }

        //Display date picker
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <br /><br /><br>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Wing Account for Policy Holders</h3>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gvWingAccount" runat="server" CssClass="grid-layout" AutoGenerateColumns="False" DataSourceID="WINGAccountDataSource" Width="100%" HorizontalAlign="Center" PageSize="100" OnRowDataBound="gvWingAccount_RowDataBound">
                <Columns>
                    <%--<asp:TemplateField>
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_WING_ID" ) + "\");" %>' />
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="Date_Request" HeaderText="Date Request" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Date_Request">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="WING_SK" HeaderText="SK" SortExpression="WING_SK">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Wing_Number" HeaderText="Wing #" SortExpression="Wing_Number">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Customer_Name" HeaderText="Name" ReadOnly="True" SortExpression="Customer_Name">
                        <ItemStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Gender" HeaderText="Gender" ReadOnly="True" SortExpression="Gender">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ID_Type" HeaderText="ID Type" ReadOnly="True" SortExpression="ID_Type">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField DataField="ID_Number" HeaderText="ID Number" SortExpression="ID_Number">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Birth_Date" HeaderText="Date of Birth" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Birth_Date">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Contact_Number" HeaderText="Contact No." SortExpression="Contact_Number">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number" SortExpression="Policy_Number">
                        <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>


            <asp:SqlDataSource ID="WINGAccountDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK ORDER BY Created_On DESC, Ct_Wing_Account.SK DESC"></asp:SqlDataSource>


            <!-- Modal Search WING Account -->
            <div id="myModalSearchWingAccount" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchWINGAccountHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search WING Account</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabWINGAccountSearch">
                        <li class="active"><a href="#SAppNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By Pol #</a></li>
                                            
                        <li><a href="#SKNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By WING SK</a></li>  
                        <li><a href="#WINGNO" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">WING #</a></li>   
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By Customer Name</a></li>  
                        <li><a href="#SDate" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">By Date</a></li>                    
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
                        <div class="tab-pane active" id="SKNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;WING SK:</td>
                                    <td>
                                        <asp:TextBox ID="txtWingSK" Width="95%" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>
                            </table>

                        </div>
                        <div class="tab-pane active" id="WINGNO">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;WING No.:</td>
                                    <td>
                                        <asp:TextBox ID="txtWingNo" Width="95%" runat="server"></asp:TextBox>
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
                        <div class="tab-pane" style="height: 90px;" id="SDate">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="40px;">&nbsp;&nbsp;From:&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtSearchFromDate" Width="90%" CssClass="datepicker" runat="server"></asp:TextBox>
                                    </td>      
                                    <td width="40px;">&nbsp;&nbsp;To:</td>
                                    <td>
                                        <asp:TextBox ID="txtSearchToDate" Width="90%" CssClass="datepicker" runat="server"></asp:TextBox>
                                    </td>                                
                                </tr>
                                
                            </table>

                        </div>          
                                       
                    </div>
                   <br />
                </div>
                <div class="modal-footer">                    
                    <asp:Button ID="btnSearchWING" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Search" OnClick="btnSearchWING_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Search Policy-->


        </div>

    </div>
    

</asp:Content>

