<%@ Page Language="C#" AutoEventWireup="true" CodeFile="form.aspx.cs" Inherits="Pages_Business_form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

     
    <link href="../../App_Themes/print.css" rel="stylesheet" />

    <title></title>
   

    <script type="text/ecmascript">
        function PrintDetails() {
            //Open hidden div and print
            var printContent = document.getElementById('<%= printarea.ClientID %>');

            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, '');
            printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/print.css" type="text/css" />');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.getElementById('print-content').style.display = 'block';
            

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }


    </script>

   
   
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="button" onclick="PrintDetails();" style="background: url('../../App_Themes/functions/print_data_checklist.png') no-repeat; border: none; height: 40px; width: 130px;" />
        </div>

        <div id="printarea" runat="server">
            <div id="print-content"">
                <asp:Label ID="lblPolicyNumber" runat="server" Text="00000254" CssClass="policy_number"></asp:Label>
                <asp:Label ID="lblCustomerID" runat="server" Text="00000210" CssClass="customer_id"></asp:Label>
                
                <br />
                <br />
                <br />
                <br />
                <br />

                <asp:Label ID="lblInsuredName" runat="server" Text="Sok Vatanak" CssClass="insured_name"></asp:Label>
                <asp:Label ID="lblID" runat="server" Text="0083726" CssClass="id_card"></asp:Label>
                <asp:Label ID="lblDateOfBirth" runat="server" Text="05/05/1975" CssClass="dob"></asp:Label>
                <asp:Label ID="lblAge" runat="server" Text="38" CssClass="age"></asp:Label>
                <asp:Label ID="lblGender" runat="server" Text="Male" CssClass="gender"></asp:Label>
                <asp:Label ID="lblPhoneNumber" runat="server" Text="012345678" CssClass="phone"></asp:Label>
                <asp:Label ID="lblAddress" runat="server" Text="House 30, Street 430, Psar Deourm, Khan Chomkamorn Phnom Penh" CssClass="address"></asp:Label>


                 <asp:Label ID="lblInsurancePlan" runat="server" Text="Premium Payback 200" CssClass="insurance_plan"></asp:Label>
                 <asp:Label ID="lblStandardPremium" runat="server" Text="USD 76.00" CssClass="standard_premium"></asp:Label>
                 <asp:Label ID="lblSumInsured" runat="server" Text="USD 20,000" CssClass="sum_insured"></asp:Label>
                 <asp:Label ID="lblExtraPremium" runat="server" Text="USD  0.00" CssClass="extra_premium"></asp:Label>
                 <asp:Label ID="lblCoveragePeriod" runat="server" Text="15 Years" CssClass="coverage_period"></asp:Label>
                 <asp:Label ID="lblPaymentPeriod" runat="server" Text="10 Years" CssClass="payment_period"></asp:Label>
                 <asp:Label ID="lblTotalPremium" runat="server" Text="USD 76.00" CssClass="total_premium"></asp:Label>
                 <asp:Label ID="lblEffectiveDate" runat="server" Text="16/01/2014" CssClass="effective_date"></asp:Label>
                 <asp:Label ID="lblModeOfPayment" runat="server" Text="Monthly" CssClass="mode_of_payment"></asp:Label>
                 <asp:Label ID="lblExpiryDate" runat="server" Text="15/01/2029" CssClass="expiry_date"></asp:Label>
                 <asp:Label ID="lblDueDate" runat="server" Text="16 of every month" CssClass="due_date"></asp:Label>
                 <asp:Label ID="lblMaturityDate" runat="server" Text="16/01/2029" CssClass="maturity_date"></asp:Label>

                 <%--<span id="provision" class="payment_clause">
                     ២.១ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព  <br />
ប្រសិនបើអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាពក្នុងកំឡុងពេលធានា ក្រុមហ៊ុននឹងធ្វើការទូទាត់អត្ថប្រយោជន៍ទៅកាន់អ្នកទទួលផល ឬ ទៅកាន់អ្នកទទួលមរតកស្របច្បាប់របស់អ្នកត្រូវបានធានារ៉ាប់រង <br />ទៅតាមករណីណាមួយដែលអាចកើតឡើង ដោយស្ថិតក្រោមលក្ខខណ្ឌដូចខាងក្រោម៖<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;២.១.១	ប្រសិនបើមរណភាពបណ្តាលមកពីជម្ងឺ ឬ ជរាពាធ ក្រុមហ៊ុននឹងទូទាត់ ៖<br />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ក) ១០០% នៃចំនួនទឹកប្រាក់ធានារ៉ាបរងសរុប និង <br />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ខ) ១១៥% នៃបុព្វលាភរ៉ាប់រងសរុបក្នុងកំរិតស្តង់ដារដែលបានបង់រួច (ដោយមិនគិតបញ្ចូលការប្រាក់ និងបុព្វលាភរ៉ាប់រងបន្ថែមឡើយ)<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;២.១.២	ប្រសិនបើមរណភាពបណ្តាលពីគ្រោះថ្នាក់ចៃដន្យ ក្រុមហ៊ុននឹងទូទាត់ ៖<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ក) ២០០% នៃចំនួនទឹកប្រាក់ធានារ៉ាបរងសរុប និង<br />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ខ) ១១៥% នៃបុព្វលាភរ៉ាប់រងសរុបក្នុងកំរិតស្តង់ដារដែលបានបង់រួច (ដោយមិនគិតបញ្ចូលការប្រាក់ និង បុព្វលាភរ៉ាប់រងបន្ថែមឡើយ)<br />
 ២.២ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងមានជីវិតរស់នៅ   <br />
ប្រសិនបើអ្នកត្រូវបានធានារ៉ាប់រងនៅមានជីវិតរស់នៅរហូតដល់កាលបរិច្ឆេទផុតកំណត់ធានាក្រុមហ៊ុននឹងទូទាត់ ១១៥% នៃបុព្វលាភរ៉ាប់រងសរុបក្នុងកំរិតស្តង់ដារដែលបានបង់រួច<br /> (ដោយមិនគិតបញ្ជូលការប្រាក់ និង បុព្វលាភរ៉ាប់រងបន្ថែមឡើយ) ទៅកាន់អ្នកត្រូវបានធានារ៉ាប់រងនៅកាលបរិច្ឆេទផុតកំណត់ធានា។


                 </span>  --%>
                
                <div id="provision_mrta" class="payment_clause">
                        ៥.១ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព<br />  
ក្នុងករណីដែលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព ហើយក្រុមហ៊ុនទទួលបានភស្តុតាងនៃការទទួលមរណភាពនេះដូចដែលក្រុមហ៊ុនបានស្នើសុំ នោះក្រុមហ៊ុននឹងទូទាត់អត្ថប្រយោជន៍ទៅកាន់អ្នកទទួលផលចម្បង (Major Beneficiary) ស្ថិតក្រោមតារាងកាត់បន្ថយទឹកប្រាក់ធានារ៉ាប់រង (Reduced Sum Insured Table)ក្នុងខែដែលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព។ ចំនួនដែលត្រូវទូទាត់នឹងមិនច្រើនជាងបំណុលដែលនៅជំពាក់អ្នកទទួលផលចម្បង (Major Beneficiary) ឡើយ។ អត្ថប្រយោជន៍ដែលនៅសល់បន្ទាប់ពីទូទាត់បំណុលហើយ (ប្រសិនបើមាន) ក្រុមហ៊ុននឹងទូទាត់ទៅកាន់អ្នកទទួលផលបន្ទាប់បន្សំ (Minor Beneficiary) ។ 
	៥.២ អត្ថប្រយោជន៍ករណីពិការភាពទាំងស្រុង និងជាអចិន្ត្រៃយ៍<br />
ក្នុងករណី អ្នកត្រូវបានធានារ៉ាប់រងក្លាយទៅជាមនុស្សពិការទាំងស្រុងនិងជាអចិន្ត្រៃយ៍ ហើយពិការភាពនោះកើតឡើងជាបន្តបន្ទាប់ក្នុងរយៈពេលមិនតិចជាង ១៨០ថ្ងៃ  ឬ ក្នុងករណីដែលអ្នកត្រូវបានធានារ៉ាប់រងក្លាយទៅជាមនុស្សពិការទាំងស្រុង និងជាអចិន្ត្រៃយ៍ដោយមានភស្តុតាងជាក់លាក់ ឬ មានការបញ្ជាក់វេជ្ជសាស្ត្រច្បាស់លាស់ នោះក្រុមហ៊ុននឹងទូទាត់ផលប្រយោជន៍ ទៅកាន់អ្នកទទួលផលចម្បង (Major Beneficiary) ស្ថិតក្រោមតារាងកាត់បន្ថយទឹកប្រាក់ធានារ៉ាប់រង (Reduced Sum Insured Table) ក្នុងខែដែលអ្នកត្រូវបានធានារ៉ាប់រងក្លាយជាមនុស្សពិការទាំងស្រុងនិងជាអចិន្ត្រៃយ៍ ។ ចំនួនដែលត្រូវទូទាត់នឹងមិនច្រើនជាងបំណុលដែលនៅជំពាក់ អ្នកទទួលផលចម្បង (Major Beneficiary) ឡើយ។ ផលប្រយោជន៍ដែលនៅសល់បន្ទាប់ពីទូទាត់បំណុលហើយ (ប្រសិនបើមាន) ក្រុមហ៊ុននឹងទូទាត់ទៅកាន់អ្នកត្រូវបានធានារ៉ាប់រង។


                        </div>
                              

                 <asp:Label ID="lblBenefit1" runat="server" Text="benefit1" CssClass="benefit1"></asp:Label>
                 <asp:Label ID="lblBenefit2" runat="server" Text="benefit2" CssClass="benefit2"></asp:Label>
                 <asp:Label ID="lblBenefit3" runat="server" Text="benefit3" CssClass="benefit3"></asp:Label>
                 <asp:Label ID="lblBenefit4" runat="server" Text="benefit4" CssClass="benefit4"></asp:Label>
                 <asp:Label ID="lblBenefit5" runat="server" Text="benefit5" CssClass="benefit5"></asp:Label>

                 <asp:Label ID="lblRelationship1" runat="server" Text="relationship1" CssClass="relationship1"></asp:Label>
                 <asp:Label ID="lblRelationship2" runat="server" Text="relationship2" CssClass="relationship2"></asp:Label>
                 <asp:Label ID="lblRelationship3" runat="server" Text="relationship3" CssClass="relationship3"></asp:Label>
                 <asp:Label ID="lblRelationship4" runat="server" Text="relationship4" CssClass="relationship4"></asp:Label>
                 <asp:Label ID="lblRelationship5" runat="server" Text="relationship5" CssClass="relationship5"></asp:Label>

                 <asp:Label ID="lblPercentage1" runat="server" Text="percentage1" CssClass="percentage1"></asp:Label>
                 <asp:Label ID="lblPercentage2" runat="server" Text="percentage2" CssClass="percentage2"></asp:Label>
                 <asp:Label ID="lblPercentage3" runat="server" Text="percentage3" CssClass="percentage3"></asp:Label>
                 <asp:Label ID="lblPercentage4" runat="server" Text="percentage4" CssClass="percentage4"></asp:Label>
                 <asp:Label ID="lblPercentage5" runat="server" Text="percentage5" CssClass="percentage5"></asp:Label>

                 <%---------------------------------------------------------------------------------------------------------%>

                <asp:Label ID="lblPolicy1" runat="server" Text="1" CssClass="policy_year_1"></asp:Label>
                <asp:Label ID="lblPolicy2" runat="server" Text="2" CssClass="policy_year_2"></asp:Label>
                <asp:Label ID="lblPolicy3" runat="server" Text="3" CssClass="policy_year_3"></asp:Label>
                <asp:Label ID="lblPolicy4" runat="server" Text="4" CssClass="policy_year_4"></asp:Label>
                <asp:Label ID="lblPolicy5" runat="server" Text="5" CssClass="policy_year_5"></asp:Label>
                <asp:Label ID="lblPolicy6" runat="server" Text="6" CssClass="policy_year_6"></asp:Label>

                <asp:Label ID="lblSurrender1" runat="server" Text="31" CssClass="surrender_value_1"></asp:Label>
                <asp:Label ID="lblSurrender2" runat="server" Text="322" CssClass="surrender_value_2"></asp:Label>
                <asp:Label ID="lblSurrender3" runat="server" Text="792" CssClass="surrender_value_3"></asp:Label>
                <asp:Label ID="lblSurrender4" runat="server" Text="1,219" CssClass="surrender_value_4"></asp:Label>
                <asp:Label ID="lblSurrender5" runat="server" Text="1,738" CssClass="surrender_value_5"></asp:Label>
                <asp:Label ID="lblSurrender6" runat="server" Text="2,141" CssClass="surrender_value_6"></asp:Label>

                <%---------------------------------------------------------------------------------------------------------%>

                <asp:Label ID="lblPolicy7" runat="server" Text="7" CssClass="policy_year_7"></asp:Label>
                <asp:Label ID="lblPolicy8" runat="server" Text="8" CssClass="policy_year_8"></asp:Label>
                <asp:Label ID="lblPolicy9" runat="server" Text="9" CssClass="policy_year_9"></asp:Label>
                <asp:Label ID="lblPolicy10" runat="server" Text="10" CssClass="policy_year_10"></asp:Label>
                <asp:Label ID="lblPolicy11" runat="server" Text="11" CssClass="policy_year_11"></asp:Label>
                <asp:Label ID="lblPolicy12" runat="server" Text="12" CssClass="policy_year_12"></asp:Label>

                <asp:Label ID="lblSurrender7" runat="server" Text="2,565" CssClass="surrender_value_7"></asp:Label>
                <asp:Label ID="lblSurrender8" runat="server" Text="3,009" CssClass="surrender_value_8"></asp:Label>
                <asp:Label ID="lblSurrender9" runat="server" Text="3,475" CssClass="surrender_value_9"></asp:Label>
                <asp:Label ID="lblSurrender10" runat="server" Text="3,963" CssClass="surrender_value_10"></asp:Label>
                <asp:Label ID="lblSurrender11" runat="server" Text="4,130" CssClass="surrender_value_11"></asp:Label>
                <asp:Label ID="lblSurrender12" runat="server" Text="4,303" CssClass="surrender_value_12"></asp:Label>


                <%---------------------------------------------------------------------------------------------------------%>

                <asp:Label ID="lblPolicy13" runat="server" Text="13" CssClass="policy_year_13"></asp:Label>
                <asp:Label ID="lblPolicy14" runat="server" Text="14" CssClass="policy_year_14"></asp:Label>
                <asp:Label ID="lblPolicy15" runat="server" Text="15" CssClass="policy_year_15"></asp:Label>
                <asp:Label ID="lblPolicy16" runat="server" Text="16" CssClass="policy_year_16"></asp:Label>
                <asp:Label ID="lblPolicy17" runat="server" Text="17" CssClass="policy_year_17"></asp:Label>
                <asp:Label ID="lblPolicy18" runat="server" Text="18" CssClass="policy_year_18"></asp:Label>

                <asp:Label ID="lblSurrender13" runat="server" Text="4,481" CssClass="surrender_value_13"></asp:Label>
                <asp:Label ID="lblSurrender14" runat="server" Text="4,666" CssClass="surrender_value_14"></asp:Label>
                <asp:Label ID="lblSurrender15" runat="server" Text="4,856" CssClass="surrender_value_15"></asp:Label>
                <asp:Label ID="lblSurrender16" runat="server" Text="-" CssClass="surrender_value_16"></asp:Label>
                <asp:Label ID="lblSurrender17" runat="server" Text="-" CssClass="surrender_value_17"></asp:Label>
                <asp:Label ID="lblSurrender18" runat="server" Text="-" CssClass="surrender_value_18"></asp:Label>

                 <%---------------------------------------------------------------------------------------------------------%>

                <asp:Label ID="lblPolicy19" runat="server" Text="19" CssClass="policy_year_19"></asp:Label>
                <asp:Label ID="lblPolicy20" runat="server" Text="20" CssClass="policy_year_20"></asp:Label>
                <asp:Label ID="lblPolicy21" runat="server" Text="21" CssClass="policy_year_21"></asp:Label>
                <asp:Label ID="lblPolicy22" runat="server" Text="22" CssClass="policy_year_22"></asp:Label>
                <asp:Label ID="lblPolicy23" runat="server" Text="23" CssClass="policy_year_23"></asp:Label>
                <asp:Label ID="lblPolicy24" runat="server" Text="24" CssClass="policy_year_24"></asp:Label>

                <asp:Label ID="lblSurrender19" runat="server" Text="-" CssClass="surrender_value_19"></asp:Label>
                <asp:Label ID="lblSurrender20" runat="server" Text="-" CssClass="surrender_value_20"></asp:Label>
                <asp:Label ID="lblSurrender21" runat="server" Text="-" CssClass="surrender_value_21"></asp:Label>
                <asp:Label ID="lblSurrender22" runat="server" Text="-" CssClass="surrender_value_22"></asp:Label>
                <asp:Label ID="lblSurrender23" runat="server" Text="-" CssClass="surrender_value_23"></asp:Label>
                <asp:Label ID="lblSurrender24" runat="server" Text="-" CssClass="surrender_value_24"></asp:Label>

                <asp:Label ID="lblIssueDate" runat="server" Text="28/01/2014" CssClass="issue_date"></asp:Label>                

            </div>
        </div>


        
    </form>

    <div id="sp_provision_mrta" class="payment_clause">
                            ៥.១ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព  
                            <br />
                            ក្នុងករណីដែលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព ហើយក្រុមហ៊ុនទទួលបានភស្តុតាងនៃការទទួលមរណភាពនេះដូចដែលក្រុមហ៊ុនបានស្នើសុំ នោះក្រុមហ៊ុននឹងទូទាត់អត្ថប្រយោជន៍ទៅកាន់<br />
                            អ្នកទទួលផលចម្បង (Major Beneficiary) ស្ថិតក្រោមតារាងកាត់បន្ថយទឹកប្រាក់ធានារ៉ាប់រង (Reduced Sum Insured Table) ក្នុងខែដែលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព។ 
                            <br />
                            ចំនួនដែលត្រូវទូទាត់នឹងមិនច្រើនជាងបំណុលដែលនៅជំពាក់អ្នកទទួលផលចម្បង (Major Beneficiary) ឡើយ។ អត្ថប្រយោជន៍ដែលនៅសល់បន្ទាប់ពីទូទាត់បំណុលហើយ (ប្រសិនបើមាន)
                            <br />
                            ក្រុមហ៊ុននឹងទូទាត់ទៅកាន់អ្នកទទួលផលបន្ទាប់បន្សំ (Minor Beneficiary) ។
                            <br />
                            
        ៥.២ អត្ថប្រយោជន៍ករណីពិការភាពទាំងស្រុង និងជាអចិន្ត្រៃយ៍

       <br />
                            ក្នុងករណី អ្នកត្រូវបានធានារ៉ាប់រងក្លាយទៅជាមនុស្សពិការទាំងស្រុងនិងជាអចិន្ត្រៃយ៍ ហើយពិការភាពនោះកើតឡើងជាបន្តបន្ទាប់ក្នុងរយៈពេលមិនតិចជាង ១៨០ថ្ងៃ  ឬ ក្នុងករណីដែលអ្នកត្រូវបានធានារ៉ាប់រង<br />
                            ក្លាយទៅជាមនុស្សពិការទាំងស្រុង
                            និងជាអចិន្ត្រៃយ៍ដោយមានភស្តុតាងជាក់លាក់ ឬ មានការបញ្ជាក់វេជ្ជសាស្ត្រច្បាស់លាស់ នោះក្រុមហ៊ុននឹងទូទាត់ផលប្រយោជន៍ ទៅកាន់អ្នកទទួលផលចម្បង 
                            <br />
                            (Major Beneficiary) ស្ថិតក្រោមតារាង កាត់បន្ថយទឹកប្រាក់ធានារ៉ាប់រង (Reduced Sum Insured Table) ក្នុងខែដែលអ្នកត្រូវបានធានារ៉ាប់រងក្លាយជាមនុស្សពិការទាំងស្រុងនិងជាអចិន្ត្រៃយ៍ ។ 
                            <br />
                            ចំនួនដែលត្រូវទូទាត់នឹងមិនច្រើនជាងបំណុលដែលនៅជំពាក់
                            អ្នកទទួលផលចម្បង (Major Beneficiary) ឡើយ។ ផលប្រយោជន៍ដែលនៅសល់បន្ទាប់ពីទូទាត់បំណុលហើយ (ប្រសិនបើមាន) 
                            <br />
                            ក្រុមហ៊ុននឹងទូទាត់ទៅកាន់អ្នកត្រូវបានធានារ៉ាប់រង។
                    </div>      

</body>
</html>
