<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_policy_insurance_summary_rp.aspx.cs" Inherits="Pages_Reports_micro_policy_insurance_summary_rp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">


    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    
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
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
   
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       <asp:UpdateProgress ID="upg" runat="server" AssociatedUpdatePanelID="UP">
        <ProgressTemplate>
            <div class="my_progress">
                <div class="tr"></div>
                <div class="main">
                    <div class="dhead">
                        <h2>PROCESSING</h2>
                    </div>
                    <p>
                        <img id="loader" src="../../App_Themes/images/loader.gif" alt="Progressing" />
                    </p>
                    <p>Please wait...</p>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
        <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Micro Policy Insurance Summary</h3>

        </div>
        <div class="panel-body">
            <table style="width:100%;">
                 <tr>
                    <td  style="padding-right:20px;">
                        <asp:Label ID="lblIssuedDateF" runat="server" Text="Issued Date From:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:TextBox ID="txtIssuedDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:Label ID="lblIssuedDateTo" runat="server" Text="To:"></asp:Label>
                    </td >
                    <td style="padding-right:20px;">
                        <asp:TextBox ID="txtIssuedDateTo" runat="server" placeholder="DD-MM-YYYY"  CssClass="datepicker"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                    <td style="padding-right:20px;">
                           <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                    </td>
                    <td style="padding-right:20px;">
                       <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" ></asp:DropDownList>
                    </td>
                     <td style="padding-right:20px;">
                           <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                     </td>
                      <td style="padding-right:20px;">
                            <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged" ></asp:DropDownList>
                      </td>
                </tr>
                  <tr>
                     <td style="padding-right:20px;">
                        <asp:Label ID="lblGroup" runat="server" Text="Group Branch:"></asp:Label>
                    </td>
                      <td  style="padding-right:20px;" colspan="7">
                         <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                          </td>
                </tr>
                 <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblPolStatus" runat="server" Text="Policy Status:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;" colspan="9">                             
                                <table style="width: 100%; border: 1px solid gray" id="tblpolicy" runat="server">
                                    <tr>
                                        <td style="padding:10px;">

                                            <asp:CheckBoxList ID="cblpolicystatus" runat="server" RepeatDirection="Horizontal" RepeatColumns="12" CssClass="CheckboxList" Width="100%" CellSpacing="5" AutoPostBack="true" OnSelectedIndexChanged="cblpolicystatus_SelectedIndexChanged">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>                      
                                                <asp:ListItem Text="Inforce" Value="IF"></asp:ListItem>
                                                <asp:ListItem Text="Terminate" Value="TER"></asp:ListItem>
                                                <asp:ListItem Text="Cancel" Value="CAN"></asp:ListItem>
                                                <asp:ListItem Text="Expire" Value="EXP"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                <tr>
                  

                     <td style="padding-right:20px;">
                        <asp:Label ID="lblChannelLocation" runat="server" Text="Branch Code:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;" colspan="7">
                       <%-- <asp:DropDownList ID="ddlChannelLocation" runat="server"></asp:DropDownList>--%>
                      <table style="width: 100%; border: 1px solid gray; " id="tblUser" runat="server">
                    <tr>
                        <td style="padding:5px;">
                           
                        <asp:CheckBoxList ID="cblChannelLocation" runat="server" RepeatDirection="Horizontal" RepeatColumns="12" CssClass="CheckboxList" Width="100%" CellSpacing="5" AutoPostBack="true" OnSelectedIndexChanged="cblChannelLocation_SelectedIndexChanged"></asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
                    </td>
                   
                   
                </tr>
            </table>
        </div>
        <div class="panel-heading">
            <h3 class="panel-title">Result [<asp:Label ID="lblRecords" runat="server"></asp:Label><asp:Label ID="lblTotalPremium" runat="server"></asp:Label>] <div style="float:right;"><asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" style="margin-left:30px;" /><asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" style="margin-left:30px;" /></div></h3>
             
        </div>
        <div style="overflow-x: scroll;height: 400px; width: 100%;">
            <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="50" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center"  HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="#21275b"
                 OnPageIndexChanging="gv_valid_PageIndexChanging" AllowSorting="true" OnSorting="gv_valid_Sorting" >
                <SelectedRowStyle BackColor="#EEFFAA"   />
                    <PagerStyle HorizontalAlign = "Left" CssClass = "GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="No" Visible="true" >
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                   <asp:TemplateField HeaderText="Channel" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblChannel" runat="server" Text='<%#Eval("details") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("channel_name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Branch Code"   Visible="true" SortExpression="office_code">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchCode" Width="50px" runat="server" Text='<%#Eval("office_code") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch"   Visible="false" >
                        <ItemTemplate>
                            <asp:Label ID="lblBranch" width="280px" runat="server" Text='<%#Eval("office_name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Policy Package1" SortExpression="Policy_Package1">
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyPackage1" Width="30px" runat="server" Text='<%# Eval("Policy_Package1").ToString()=="0"? "-": Eval("Policy_Package1") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Policy Package2" SortExpression="Policy_Package2">
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyPackage2" Width="30px" runat="server" Text='<%# Eval("Policy_Package2").ToString()=="0"? "-": Eval("Policy_Package2") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Policy" SortExpression="total_policy">
                        <ItemTemplate>
                            <asp:Label ID="lblPolicy" Width="30px" runat="server" Text='<%# Eval("total_policy").ToString()=="0"? "-": Eval("total_policy") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="SO (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblSO" Width="50px" runat="server" Text='<%# Eval("SO").ToString()=="0.00"? "-" : Eval("SO") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SO Discount (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblSODiscount" Width="50px" runat="server" Text='<%# Eval("SO_discount").ToString()=="0.00"? "-" : Eval("SO_discount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SO After Dis. (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblSOAfterDiscount" Width="50px" runat="server" Text='<%# Eval("SO_after_discount").ToString()=="0.00"? "-" : Eval("SO_after_discount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DHC (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblDHC" Width="50px" runat="server" Text='<%# Eval("DHC").ToString()=="0.00" ? "-": Eval("DHC") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="DHC Discount (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblDHCDiscount" Width="50px" runat="server" Text='<%# Eval("DHC_discount").ToString()=="0.00" ? "-": Eval("DHC_discount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DHC After Dis. (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblDHCAfterDiscount" Width="50px" runat="server" Text='<%# Eval("DHC_After_discount").ToString()=="0.00" ? "-": Eval("DHC_after_discount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Total Amount (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblTotatlAmount"  Width="50px" runat="server" Text='<%# Eval("total_amount").ToString()=="0.00" ? "-": Eval("total_amount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Discount (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalDiscount"  Width="50px" runat="server" Text='<%# Eval("Total_discount").ToString()=="0.00" ? "-": Eval("total_discount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Premium Package1 (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblPremiumPackage1"  Width="50px" runat="server" Text='<%# Eval("premium_package1").ToString()=="0.00" ? "-": Eval("premium_package1") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Premium Package2 (USD)">
                        <ItemTemplate>
                            <asp:Label ID="lblPremiumPackage2"  Width="50px" runat="server" Text='<%# Eval("premium_package2").ToString()=="0.00" ? "-": Eval("premium_package2") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Amount After Dis. (USD)" SortExpression="Total_Amount_After_Discount">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalAmount"  Width="100px" runat="server" Text='<%# Eval("Total_Amount_After_Discount").ToString()=="0.00"? "-" : Eval("Total_Amount_After_Discount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Policy Status"   Visible="true" >
                        <ItemTemplate>
                            <asp:Label ID="lblpolicystatus" width="80px" runat="server" Text='<%# Eval("POLICY_STATUS").ToString()=="IF" ? "Inforce":Eval("POLICY_STATUS").ToString()=="TER" ? "Terminate":Eval("POLICY_STATUS").ToString()=="CAN" ? "Cancel":Eval("POLICY_STATUS").ToString()=="EXP" ? "Expire":"" %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Remarks" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblreamrks" runat="server" Width="50px" Text='<%# Eval("POLICY_STATUS_REMARKS") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                       </asp:TemplateField>
                   
                </Columns>

            </asp:GridView>
        </div>
    

    </div>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
 
    </asp:Content>
