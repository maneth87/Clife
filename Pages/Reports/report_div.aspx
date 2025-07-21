<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="report_div.aspx.cs" Inherits="Pages_Reports_report_div" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">


    <style>
        .lbl_format {
        font-family:'Khmer OS Content'; font-size:15px;
        }
        .lbl_surrender {
             font-family:'Khmer OS Content'; font-size:15px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
		<!-- here is the block code-->
            <div id="certification_policy" style="background-image:url('../../App_Themes/images/PolicySchedule.jpg');  margin-right:226px; width:1336px; height:1892px; border:solid; border-radius:25px; border-color:transparent">
                <!-- Block Header -->
                <table style="width:1127px; margin-top:95px;">
                    <tr>
                        <td style="height:18px; "><label id="lblPolicyNo" class="lbl_format" runat="server" style="text-align:right; margin-top:5px; margin-right:55px;"></label></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="height:18px; "><label id="lblCustomerID" class="lbl_format" runat="server" style="text-align:right; margin-top:12px; margin-right:55px;"></label></td>
                    </tr>
                   <tr>
                        <td></td>
                    </tr>
                </table> <!-- End of Header -->
               
                <br />

                <!-- Personal Info -->
                <div id="divLeft" style="float:left; margin-top:106px;">
                    <table style="width:560px; ">
                        <tr>
                            <td style="height:10px;">
                                <label id="lblInusredName" class="lbl_format" runat="server" style="text-align:left; margin-left:463px;"></label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:10px; ">
                                <label id="lblDOB" class="lbl_format" runat="server" style="text-align:left; margin-left:463px; "></label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:10px; ">
                                <label id="lblAge" class="lbl_format" runat="server" style="text-align:left; margin-left:463px;"></label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:10px; ">
                                <label id="lblPhone" class="lbl_format" runat="server" style="text-align:left; margin-left:463px;"></label>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divRight" style="float:right;margin-top:106px;">
                    <table style="width:560px;">
                        <tr>
                            <td style="height:10px;  margin-right:100px;">
                                <label id="lblPassport" class="lbl_format" runat="server" style="margin-left:245px;   width:140px;" ></label>
                            </td>
                       </tr>
                       <tr>
                            <td style="height:10px;" ></td>
                       </tr>
                       <tr>
                            <td style="height:10px; ">
                                <label id="lblSex" class="lbl_format" runat="server" style="margin-top:13px; margin-left:10px; "></label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:10px;"></td>
                       </tr>
                    </table>
                </div>

                <table style="width:1127px;">
                    <tr>
                        <td style="height:10px; ">
                            <label id="lblAddress" class="lbl_format" runat="server" style="margin-left:474px; margin-left:463px; "></label>
                        </td>
                    </tr>
                </table>
                <!-- End of Personal Info -->
              
                <!-- Block Insurance Plan -->
                  <div id="div_insurance_plan_left" style="float:left;  margin-top:41px;">
                        <table style="width:681px; ">
                            <tr>
                                <td style="height:10px;  margin-right:100px;">
                                    <label id="lblTypeInsurancePlan"  class="lbl_format" runat="server" style="text-align:left;  margin-left:463px;" ></label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:10px;  margin-right:100px;">
                                    <label id="lblSum_Insured"  class="lbl_format" runat="server" style="text-align:left; margin-left:463px; " ></label>
                                </td>
                           </tr>
                           <tr>
                                <td style="height:10px;  margin-right:100px;">
                                    <label id="lblCoverage_period"  class="lbl_format" runat="server" style="text-align:left; margin-left:463px; margin-top:2px; " ></label>
                                </td>
                           </tr>
                           <tr>
                                <td style="height:10px;  margin-right:100px;">
                                    <label id="lblPayment_period"  class="lbl_format" runat="server" style="text-align:left; margin-left:463px;  " ></label>
                                </td>
                           </tr>
                           <tr>
                               <td style="height:10px;  margin-right:100px;">
                                    <label id="lblEffective_date"  class="lbl_format" runat="server" style="text-align:left; margin-left:463px;  " ></label>
                               </td>
                           </tr>
                           <tr>
                               <td style="height:10px;  margin-right:100px;">
                                    <label id="lblExpriy_date"  class="lbl_format" runat="server" style="text-align:left; margin-left:463px;  " ></label>
                               </td>
                           </tr>
                           <tr>
                               <td style="height:10px;  margin-right:100px;">
                                    <label id="lblMaturity_date"  class="lbl_format" runat="server" style="text-align:left; margin-left:463px;  " ></label>
                               </td>
                           </tr>
                       </table>
                  </div>

                  <div id="div_insurance_plan_right" style="float:right; margin-top:40px; width:489px;">
                        <table style="width:489px;">
                            <tr>
                                <td><label id="lblUSD"  class="lbl_format" style="text-align:left; margin-left:150px; margin-top:2px; " >USD</label></td>
                                <td style="height:10px;  margin-right:100px;">
                                    <label id="lblStandardPrem"  class="lbl_format" runat="server" style="text-align:left; margin-left:150px; margin-top:2px; margin-left:-290px; " ></label>
                                </td>
                            </tr>
                            <tr>
                                <td><label id="lblUSD1"  class="lbl_format" style="text-align:left; margin-left:150px; margin-top:2px; " >USD</label></td>
                                <td style="height:10px;  margin-right:100px;">
                                   <label id="lblExtra_Prem"  class="lbl_format" runat="server" style="text-align:left;  margin-left:150px; margin-left:-272px; " ></label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:10px; color:white">Nothing</td>
                            </tr>
                            <tr>
                                <td><label id="Label1"  class="lbl_format" style="text-align:left; margin-left:150px; margin-top:2px; " >USD</label></td>
                                <td style="height:10px; ">
                                    <label id="lblTotalPremium" class="lbl_format" runat="server" style=" margin-left:150px; margin-left:-288px;"></label>
                                </td>
                            </tr>
                            <tr>
                                 <td style="height:10px;  margin-right:100px;">
                                   <label id="lblMode_payment"  class="lbl_format" runat="server" style="text-align:left;  margin-left:150px;  " ></label>
                                </td>
                            </tr>
                            <tr>
                                 <td style="height:10px;  margin-right:100px;">
                                   <label id="lblDue_date"  class="lbl_format" runat="server" style="text-align:left;  margin-left:150px;  " ></label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:10px; color:white">Nothing</td>
                            </tr>
                        </table>
                  </div>    <!-- End of Insurance Plan -->

                <!-- Benefit Payment Clauses -->
                    <br />
                <div id="Benefit" style="margin-top:250px; ">
                    <table style="width:1208px;">
                        <tr>
                            <td>
                                 <label id="lblBenefit_payment_clauses"  class="lbl_format" runat="server" style="text-align:left;  margin-left:150px; " >
                                    ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ <br />
                                     ប្រចាំឆ្នាំ
                                 </label>
                            </td>
                        </tr>
                    </table>
                </div><!-- End of Benefit Payment Cluases -->

                <!-- Beneficiary -->
                <div id="Beneficiary_name" runat="server" style="margin-left:127px; width:1080px; height:135px;  margin-top:130px;">

                </div> <!-- End of Beneficiary -->

                <!-- Surrender Value -->
                <div id="SurrenderValue" style="margin-left:127px; width:1080px; height:145px; margin-top:90px; border:solid 4px; border-color:blue;" >
                    <table style=" width:1080px; height:50px; border:solid 1px; border-color:orange; padding: 5px -5px -0px -0px;" >
                        <tr style="border:solid 1px;">
                            <td style="width:130px; text-align:center;"><label id="lblEndPolicy_year1"  class="lbl_surrender" runat="server"></label></td>
                            <td style="width:140px;"><label id="lblSurrenderValue1"  class="lbl_surrender" runat="server"></label></td>
                        
                            <!-- Col 3 -->
                            <td style="color:white;">nothing</td>

                            <td><label id="lblEndPolicy_year7"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue7"  class="lbl_surrender" runat="server"></label></td>

                            <!-- Col 6 -->
                            <td style="color:white;">nothing</td>

                            <td><label id="lblEndPolicy_year13"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue13"  class="lbl_surrender" runat="server"></label></td>

                            <!-- Col 9 -->
                            <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year19"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue19"  class="lbl_surrender" runat="server"></label></td>
                        </tr>

                        <tr>
                            <td><label id="lblEndPolicy_year2"  class="lbl_surrender" runat="server">2</label></td>
                            <td><label id="lblSurrenderValue2"  class="lbl_surrender" runat="server"></label></td>

                            <!-- Col 3 -->
                            <td style="color:white;">nothing</td>

                            <td><label id="lblEndPolicy_year8"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue8"  class="lbl_surrender" runat="server"></label></td>

                            <!-- Col 6 -->
                            <td style="color:white;">nothing</td>

                            <td><label id="lblEndPolicy_year14"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue14"  class="lbl_surrender" runat="server"></label></td>

                            <!-- Col 9 -->
                            <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year20"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue20"  class="lbl_surrender" runat="server"></label></td>
                       </tr>
                       <tr>  
                            <td><label id="lblEndPolicy_year3"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue3"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 3 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year9"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue9"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 6 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year15"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue15"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 9 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year21"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue21"  class="lbl_surrender" runat="server"></label></td>
                       </tr>
                       <tr>  
                            <td><label id="lblEndPolicy_year4"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue4"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 3 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year10"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue10"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 6 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year16"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue16"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 9 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year22"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue22"  class="lbl_surrender" runat="server"></label></td>
                       </tr>
                       <tr>  
                            <td><label id="lblEndPolicy_year5"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue5"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 3 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year11"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue11"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 6 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year17"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue17"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 9 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year23"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue23"  class="lbl_surrender" runat="server"></label></td>
                       </tr>
                       <tr>  
                            <td><label id="lblEndPolicy_year6"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue6"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 3 -->
                            <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year12"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue12"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 6 -->
                            <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year18"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue18"  class="lbl_surrender" runat="server"></label></td>

                           <!-- Col 9 -->
                           <td style="color:white;"></td>

                            <td><label id="lblEndPolicy_year24"  class="lbl_surrender" runat="server"></label></td>
                            <td><label id="lblSurrenderValue24"  class="lbl_surrender" runat="server"></label></td>
                       </tr>
                    </table>
                </div>  <!-- End of Surrender Value -->
              
            </div> <!-- End of Div Background Image -->
            
	</ContentTemplate>
    </asp:UpdatePanel>	
</asp:Content>

