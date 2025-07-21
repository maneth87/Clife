<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/Content.master" AutoEventWireup="false" CodeFile="policy_detail1.aspx.vb" Inherits="Pages_Business_policy_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">

        <li>
            <input type="button" onclick="clear_form();" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>

        <li style="display: none;">
            <input type="button" data-toggle="modal" data-target="#modal_search_customer" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li style="display: none;">
            <input type="button" id="btnDelete" onclick="delete_customer();" style="background: url('../../App_Themes/functions/delete.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>

        <li>
            <input type="button" id="btnSave" onclick="save_all();" style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <input type="button" onclick="save_member();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <style>
        .modal.large {
            width: 60%; /* desired relative width */
            left: 20%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }

        .td10 {
            padding-left: 10px;
            padding-top: 0px;
            padding-bottom: 0px;
            padding-right: 10px;
            vertical-align: middle;
        }

        .tdpading10 {
            padding-top: 10px;
        }

        .txt100 {
            width: 96%;
        }

        .txt50 {
            width: 50%;
        }

        .td40 {
            width: 10%;
        }

        .td60 {
            width: 80%;
        }

        .td25 {
            width: 25%;
        }

        .ddl100 {
            width: 97.2%;
        }

        .required {
            color: red;
        }

        .select_row {
            background-color: #24f1b2;
        }
    </style>
    <script>
        var customer_search_id;
        var customer_search_name;
        var customer_search_option = 0;
        var customer_search_type_id;
        var customer_search_birth_date;
        var customer_search_gender;
        
        var selected_member_row=0;
        var member_row=0;
                
        var policy_id;
        var policy_number;
        var policy_customer;
        var policy_customer_id;
        var policy_customer_type;
        var policy_effective_date;
        var app_number;
        var app_origin_number;
        var app_date;
        var product;
        var policy_sum_insured;
        var policy_term_insurance;
        var policy_pay_period;
        var policy_pay_mode;
        var policy_premium;
        var policy_annaul_premium;
        var policy_annual_origin_premium;
        var policy_discount;
        var policy_extra_premium;
        var policy_extra_annual_premium;
        var policy_actual_premium;
        var policy_return_premium;
        var policy_payment_code;
        var policy_total_premium_after_discount;
        var policy_approved_date;

        var si;
        var premium;
        var discount;
        var extra_premium;
   
        var policy_owner_name;
        var policy_owner_birth_date;
        var policy_owner_gender;

        var member_id_delete='';
        var member_effective_date;
        var member_customer;
        var member_customer_id;
        var member_age;
        var member_si;
        var member_premium_by_mode;
        var member_annually_premium;
        var member_annual_origin_premium;
        var member_discount;
        var member_extra_premium;
        var member_extra_annual_premium;
        var member_total_premium_after_dis;
        var member_birth_date;
        var member_gender;
        var member_remarks;
        var member_percentage;
        var member_rate;

        var cover_delete_id=''; //reserve to delete from database
        var cover_type;
        var cover_effective_date;
        var cover_si;
        var cover_premium;
        var cover_annual_premium;
        var cover_annual_origin_premium;
        var cover_remarks;
        var cover_id;

        var rider_selected_row=0;
        var rider_row_no=0;
        var rider_delete_id=''; //reserve to delete to database
        var rider_kh_first_name;
        var rider_kh_last_name;
        var rider_en_first_name;
        var rider_en_last_name;
        var rider_gender;
        var rider_age;
        var rider_birth_date;
        var rider_id_type;
        var rider_id_card;
        var rider_nationality;
        var rider_type;
        var rider_relationship;
        var rider_effective_date;
        var rider_si;
        var rider_premium_by_mode;
        var rider_annual_premium;
        var rider_annual_origin_premium;
        var rider_extra_premium;
        var rider_extra_annual_premium;
        var rider_remarks;

        var ben_selected_row=0;
        var ben_row_no=0;
        var ben_delete_id=''; //reserve to delete from database
        var ben_kh_first_name;
        var ben_kh_last_name;
        var ben_en_first_name;
        var ben_en_last_name;
        var ben_gender;
        var ben_birth_date;
        var ben_id_type;
        var ben_id_card;
        var ben_nationality;
        var ben_relationship;
        var ben_percentage;
        var ben_remarks;

        var total_percentage =0;
        var selected_percentage=0;

        $(document).ready(function () {
            $('.datepicker').datepicker();

            $('.icon-search').mouseover(function () {
                $(this).css('cursor', 'pointer');

            });
           
            //policy controls
           
            policy_id=$('#<%=hdfPolicyID.ClientID%>');
            policy_number = $('#<%=txtPolicyNumber.ClientID%>');
            policy_customer = $('#<%=txtCustomerName.ClientID%>');
            policy_customer_id = $('#<%=hdfCustomerID.ClientID%>');
            policy_customer_type = $('#<%=ddlCustomerType.ClientID%>');
            policy_effective_date=$('#<%=txtEffectiveDate.ClientID%>');
            app_number=$('#<%=txtAppNumber.ClientID%>');
            app_date = $('#<%=txtAppDate.ClientID%>');
            product=$('#<%=ddlProduct.ClientID%>');
            policy_pay_period=$('#<%=txtPaymentPeriod.ClientID%>');
            policy_term_insurance=$('#<%=txtTermOfInsurance.ClientID%>');
            policy_sum_insured=$('#<%=txtPolicySI.ClientID%>');
            policy_pay_mode=$('#<%=ddlPayMode.ClientID%>');
            policy_premium = $('#<%=txtPolicyPremium.ClientID%>');
            policy_annaul_premium=$('#<%=txtPolicyAnnualPremium.ClientID%>');
            policy_annual_origin_premium=$('#<%=txtPolicyAnnualOriginPremium.ClientID%>');
            policy_discount=$('#<%=txtPolicyDis.ClientID%>');
            policy_extra_premium=$('#<%=txtExtraPremium.ClientID%>');
            policy_extra_annual_premium=$('#<%=txtExtraAnnualPremium.ClientID%>');
            policy_return_premium=$('#<%=txtPolicyReturnPremium.ClientID%>');
            policy_actual_premium=$('#<%=txtPolicyActualPremium.ClientID%>');
            policy_payment_code=$('#<%=txtPolicyPaymentCode.ClientID%>');
            policy_total_premium_after_discount=$('#<% =txtPolicyTotalPremiumAfterDis.ClientID%>');
            policy_approved_date=$('#<%=txtApprovedDate.ClientID%>');

            //policy owner controls
            policy_owner_birth_date=$('#<%=txtPolicyOwnerBirthDate.ClientID%>');
            policy_owner_name=$('#<%=txtPolicyOwnerName.ClientID%>');
            policy_owner_gender=$('#<%=txtPolicyOwerGender.ClientID%>');

            //member controls
            member_effective_date = $('#<%=txtMemberEffectiveDate.ClientID%>');
            member_customer = $('#<%=txtMemberCustomer.ClientID%>');
            member_customer_id=$('#<%=hdfMemberCustomerID.ClientID%>');
            member_age = $('#<%=txtMemberAge.ClientID%>');
            member_si = $('#<%=txtMemberSI.ClientID%>');
            member_premium_by_mode = $('#<%=txtMemberPremiumByMode.ClientID%>');
            member_annually_premium = $('#<%=txtMemberAnnuallyPremium.ClientID%>');
            member_annual_origin_premium=$('#<%=txtMemberAnnuallyOriginPremium.ClientID%>');
            member_discount = $('#<%=txtMemberDis.ClientID%>');
            member_extra_premium = $('#<%=txtMemberExtraPremium.ClientID%>');
            member_extra_annual_premium = $('#<%=txtMemberExtraAnnualPremium.ClientID%>');
            member_total_premium_after_dis = $('#<%=txtMemberTotalPremiumAfterDis.ClientID%>');
            member_birth_date=$('#<%=txtMemberBirthDate.ClientID%>');
            member_gender=$('#<%=txtMemberGender.ClientID%>');
            member_remarks=$('#<%=txtMemberRemarks.ClientID%>');
            member_percentage=$('#<%=txtMemberExtraPercentage.ClientID%>');
            member_rate=$('#<%=txtMemberExtraRate.ClientID%>');

            //additional cover controls
            cover_effective_date=$('#<%=txtCoverEffectiveDate.ClientID%>');
            cover_si=$('#<%=txtCoverSI.ClientID%>');
            cover_type=$('#<%=ddlCoverType.ClientID%>');
            cover_id=$('#<%=hdfCoverId.ClientID%>');
            cover_premium=$('#<%=txtCoverPremiumByMode.ClientID%>');
            cover_annual_origin_premium=$('#<%=txtCoverAnnualOriginPremium.ClientID%>');
            cover_annual_premium=$('#<%=txtCoverAnnualPremium.ClientID%>');
            cover_remarks=$('#<%=txtCoverRemarks.ClientID%>');

            //rider controls
            rider_kh_first_name = $('#<%=txtRiderKhmerFirstName.ClientID%>');
            rider_kh_last_name = $('#<%=txtRiderKhmerLastName.ClientID%>');
            rider_en_first_name = $('#<%=txtRiderFirstName.ClientID%>');
            rider_en_last_name = $('#<%=txtRiderLastName.ClientID%>');
            rider_gender = $('#<%=ddlRiderGender.ClientID%>');
            rider_birth_date = $('#<%=txtRiderBirthDate.ClientID%>');
            rider_age = $('#<%=txtRiderAge.ClientID%>');
            rider_id_type = $('#<%=ddlRiderIDType.ClientID%>');
            rider_id_card = $('#<%=txtRiderIDCard.ClientID%>');
            rider_nationality = $('#<%=ddlRiderNationality.ClientID%>');
            rider_type = $('#<%=ddlRiderType.ClientID%>');
            rider_relationship = $('#<%=ddlRiderRelationship.ClientID%>');
            rider_effective_date = $('#<%=txtRiderEffectiveDate.ClientID%>');
            rider_si = $('#<%=txtRiderSI.ClientID%>');
            rider_premium_by_mode = $('#<%=txtRiderPremiumByMode.ClientID%>');
            rider_annual_origin_premium = $('#<%=txtRiderAnnualOriginPremium.ClientID%>');
            rider_annual_premium = $('#<%=txtRiderAnnualPremium.ClientID%>');
            rider_extra_premium = $('#<%=txtRiderExtraPremium.ClientID%>');
            rider_extra_annual_premium = $('#<%=txtRiderExtraAnnualPremium.ClientID%>');
            rider_remarks = $('#<%=txtRiderRemarks.ClientID%>');

            //beneficiary controls
            ben_kh_first_name = $('#<%=txtBenKhmerFirstName.ClientID%>');
            ben_kh_last_name = $('#<%=txtBenKhmerLastName.ClientID%>');
            ben_en_first_name = $('#<%=txtBenFirstName.ClientID%>');
            ben_en_last_name = $('#<%=txtBenLastName.ClientID%>');
            ben_gender = $('#<%=ddlBenGender.ClientID%>');
            ben_birth_date = $('#<%=txtBenBirthDate.ClientID%>');
            ben_id_type = $('#<%=ddlBenIDType.ClientID%>');
            ben_id_card = $('#<%=txtBenIDCard.ClientID%>');
            ben_nationality=$('#<%=ddlBenNationlity.ClientID%>');
            ben_relationship = $('#<%=ddlBenRelationship.ClientID%>');
            ben_percentage = $('#<%=txtBenPercentage.ClientID%>');
            ben_remarks = $('#<%=txtBenRemarks.ClientID%>');

            //Disabled some controls
            policy_customer_type.prop('disabled', true);
            policy_customer.prop('disabled', true);
            policy_total_premium_after_discount.prop('disabled',true);
            policy_approved_date.prop('disabled',true);

            member_customer.prop('disabled', true);
            member_total_premium_after_dis.prop('disabled', true);

            rider_nationality.val('KH');//default select
            ben_nationality.val('KH');//default select

            //policy_id.val('C997727D-BF21-4630-B1D5-E1DDBA985D6F');
            get_all(policy_id.val());
            

            //event
            product.change(function(){
                get_pay_assured_year($(this).val());
            });

            policy_effective_date.datepicker().on('changeDate',function(){
                paste_effective($(this).val());
            });

            //Policy sum insured 
            policy_sum_insured.change(function(){
                paste_sum_insured($(this).val());
            });
            policy_premium.change(function(){
                calc_policy_total_premium_after_discount();
            });
            policy_extra_premium.change(function(){
                calc_policy_total_premium_after_discount();
            });
            policy_discount.change(function(){
                calc_policy_total_premium_after_discount();
            });

            member_premium_by_mode.change(function(){
                calcMemberTotalPremiumAfterDist();
            });
            member_extra_premium.change(function(){
                calcMemberTotalPremiumAfterDist();
            });
            member_discount.change(function(){
                calcMemberTotalPremiumAfterDist();
            });
           
            
        });
    
        //validate policy
        function validate_policy()
        {
            if(policy_customer.val().trim()=='')
            {
                return 'Customer name is required.';
            }
            else if(app_date.val().trim()=='')
            {
                return 'Application date is required.';
            }
            else if (product.val().trim()=='')
            {
                return 'Product is required.';
            }
            else if(policy_customer_type.val().trim()=='')
            {
                return 'Customer type is required.';
            }
            else if(policy_term_insurance.val().trim()=='')
            {
                return 'Term of insurance is required';
            }
            else if(policy_pay_period.val().trim()=='')
            {
                return 'Payment period is required.';
            }
            else if(policy_sum_insured.val().trim()=='')
            {
                return 'Sum insured is required.';
            }
            else if(policy_pay_mode.val().trim()=='')
            {
                return 'Pay mode is required.';
            }
            else if(policy_premium.val().trim()=='')
            {
                return 'Premium is required.';
            }
            else if(policy_annual_origin_premium.val().trim()=='')
            {
                return 'Annual premium is required.';
            }
            else if(policy_annaul_premium.val().trim()=='')
            {
                return 'Annual premium rounded is required.';
            }
            else if(policy_effective_date.val().trim()=='')
            {
                return 'Effective date is required.';
            }
            else 
            {
                return '';
            }
        }
        

        function save_policy()
        {
            var str_valid=validate_policy();
            if(str_valid !='')
            {
                alert(str_valid);
                return false;
            }

            var policies = new Array();
            policies[0]=policy_customer_id.val();
            policies[1]=product.val();
            policies[2]=policy_term_insurance.val();
            policies[3]=policy_pay_period.val();
            policies[4]=policy_number.val();
            policies[5]=app_number.val();
            policies[6]=app_number.val();//app_original_number
            policies[7]=policy_payment_code.val();
            policies[8]=app_date.val();
            policies[9]=policy_effective_date.val();
            policies[10]=policy_sum_insured.val();
            policies[11]=policy_pay_mode.val();
            policies[12]=policy_premium.val();
            policies[13]=policy_annual_origin_premium.val();
            policies[14]=policy_annaul_premium.val();
            policies[15]=policy_actual_premium.val();
            policies[16]=policy_discount.val();
            policies[17]=policy_extra_annual_premium.val();
            policies[18]=policy_extra_premium.val();
            policies[19]=policy_return_premium.val();

            if(policy_id.val()=='')//save new record
            {
                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/InsertPolicyFlatRate",
                    data:JSON.stringify({policy:policies}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data){
                        if(data.d!='')
                        {
                            policy_id.val(data.d);// reserve for saving member, cover, riders and beneficiary
                            //alert('Saved successfully.');
                            return true;
                           
                           
                        }
                        else
                        {
                            return false;
                        }
                    },
                    error: function(err)
                    {
                        alert('Save policy error: ' + err);
                    }
               
                });
            }
            
        }
        //update policy
        function update_policy()
        {
            var policies = new Array();

            policies[0]=policy_id.val();
            policies[1]=policy_customer_id.val();
            policies[2]=product.val();
            policies[3]=policy_term_insurance.val();
            policies[4]=policy_pay_period.val();
            policies[5]=policy_number.val();
            policies[6]=app_number.val();
            policies[7]=app_number.val();//app_original_number
            policies[8]=policy_payment_code.val();
            policies[9]=app_date.val();
            policies[10]=policy_effective_date.val();
            policies[11]=policy_sum_insured.val();
            policies[12]=policy_pay_mode.val();
            policies[13]=policy_premium.val();
            policies[14]=policy_annual_origin_premium.val();
            policies[15]=policy_annaul_premium.val();
            policies[16]=policy_actual_premium.val();
            policies[17]=policy_discount.val();
            policies[18]=policy_extra_annual_premium.val();
            policies[19]=policy_extra_premium.val();
            policies[20]=policy_return_premium.val();

            if(policy_id.val()!='')
            {
                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/UpdatePolicyFlatRate",
                    data:JSON.stringify({policy:policies}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data){
                        if(data.d)
                        {
                          
                            //alert('Updated successfully.');
                            return true;
                           
                        }
                        else
                        {
                            // alert('Updated fail.');
                            return false;
                           
                        }
                    },
                    error: function(err)
                    {
                        alert('Update policy error: ' +err);
                    }
               
                });
            }
        }

        //get policy
        function get_policy(policy_id)
        {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetPolicyFlatRate",
                data:JSON.stringify({policy_id:policy_id}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data){
                    var policy = data.d;
                    policy_number.val(policy.PolicyNumber);
                    app_number.val(policy.ApplicationNumber);
                    app_date.val(formatJSONDate( policy.ApplicationDate));
                    product.val(policy.ProductID);
                    policy_customer.val(policy.CustomerName);
                    policy_customer_id.val(policy.CustomerID);
                    policy_customer_type.val(policy.CustomerTypeID);
                    policy_term_insurance.val(policy.AssuredYear);
                    policy_pay_period.val(policy.PayYear);
                    policy_sum_insured.val(policy.SumInsured);
                    policy_pay_mode.val(policy.PayModeID);
                    policy_premium.val(policy.PremiumByMode);
                    policy_annaul_premium.val(policy.AnnualPremium);
                    policy_annual_origin_premium.val(policy.AnnualOriginPremium);
                    policy_extra_premium.val(policy.ExtraPremiumByMode);
                    policy_extra_annual_premium.val(policy.ExtraAnnualPremium);
                    policy_discount.val(policy.Discount);
                    //policy_total_premium_after_discount.val(parseInt(policy.PremiumByMode) + parseInt(policy.ExtraPremiumByMode) - parseInt(policy.Discount));
                    calc_policy_total_premium_after_discount();

                    policy_effective_date.val(formatJSONDate(policy.EffectiveDate));
                    policy_actual_premium.val(policy.ActualPremium);
                    policy_return_premium.val(policy.ReturnPremium);
                    policy_payment_code.val(policy.PaymentCode);
                   
                    if(formatJSONDate( policy.ApprovedDate) =='01/01/1900')
                    {
                        policy_approved_date.val('');
                    }
                    else
                    {
                        policy_approved_date.val( formatJSONDate( policy.ApprovedDate));
                    }
                   
                    // Policy owner
                    policy_owner_name.val(policy.CustomerName);
                    policy_owner_gender.val(policy.Gender);
                    policy_owner_birth_date.val(formatJSONDate(policy.BirthDate));
                }
                   
            });
        }

        function calc_policy_total_premium_after_discount()
        {
            policy_total_premium_after_discount.val(parseInt(policy_premium.val()) + parseInt(policy_extra_premium.val()) - parseInt(policy_discount.val()));
        }

        function save_member()
        {
          
            if(member_row>0)
            {
                var mem_id, mem_cust_id, mem_policy_id, mem_age, mem_si, mem_annual_prem, mem_annual_origin_prem, mem_prem_by_mode, mem_extra_prem, mem_extra_annual_prem, mem_remarks, mem_percentage, mem_rate;
                var mem_effective, mem_discount;
                var members = new Array();
                var index=0;
                for(i=1;i<=member_row;i++)

                {
                    mem_id = $('#lbl_member_id' + i).text();
                    mem_cust_id=$('#lbl_member_customer_id'+i).text();
                    mem_age=$('#lbl_member_age'+i).text();
                    mem_si =$('#lbl_member_si'+i).text();
                    mem_annual_prem=$('#lbl_member_annually_premium'+i).text();
                    mem_annual_origin_prem =$('#lbl_member_annual_origin_premium'+i).text();
                    mem_prem_by_mode=$('#lbl_member_premium_by_mode'+i).text();
                    mem_extra_prem=$('#lbl_member_extra_premium'+i).text();
                    mem_extra_annual_prem=$('#lbl_member_extra_annual_premium'+i).text();
                    mem_effective = $('#lbl_member_effective_date'+i).text();
                    mem_discount=$('#lbl_member_discount'+i).text();
                    mem_remarks = $('#lbl_member_remarks' + i).text();
                    mem_percentage=$('#lbl_member_percentage' + i).text();
                    mem_rate= $('#lbl_member_rate' + i).text();
                   
                    //alert(mem_cust_id);

                    members[index]=mem_id + ';' + policy_id.val() + ';' + mem_cust_id + ';' + mem_age + ';' + policy_pay_period.val() + ';' + policy_term_insurance.val() + ';' +
                               mem_si + ';' + mem_annual_prem + ';' + mem_annual_origin_prem + ';' + mem_prem_by_mode + ';' + policy_pay_mode.val()+ ';' + mem_extra_prem + ';' + 
                               mem_extra_annual_prem +  ';' + mem_discount + ';'  +mem_remarks + ';' + mem_percentage + ';' +mem_rate + ';' + mem_effective + ';' ;
                    index +=1;
                }

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/SaveMember",
                    data:JSON.stringify({members:members}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data){
                        if(data.d)
                        {
                            alert('Saved successfully.');
                            // return true;
                        }
                        else
                        {
                            alert('Member saved fail.');
                            return false;
                        }
                    }
               
                });
                
            }
        }

        //get member from database
        function get_member(policy_id)
        {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetMember",
                data:JSON.stringify({policy_id:policy_id}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data){
                    $.each(data.d, function(index, item){
                        
                        // add data into tbl_member_list
                        var row ='';
            
                        member_row +=1;

                        row = "<tr id='member_row_id" + member_row +  "'>" + 
                            
                                "<td style='display:none;'><lable id='lbl_member_no" + member_row  +  "'>" + member_row + "</label> "+
                                   "<lable id='lbl_member_customer_id" + member_row +"'>" + item.CustomerID + "</lable>" +
                                    "<lable id='lbl_member_id" + member_row  +  "'>" + item.PolicyMemberID  +  "</label></td>" + 
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_customer" + member_row + "'>" + item.CustomerName +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_age" + member_row + "'>" + item.Age +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_gender" + member_row + "'>" + item.Gender +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_birth_date" + member_row + "'>" + formatJSONDate( item.BirthDate) +  "</label></td>" +
                                "<td style='display:none;'><lable id='lbl_member_effective_date" + member_row + "'>" + formatJSONDate(item.EffectiveDate) +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_si" + member_row + "'>" + item.SumInsured +  "</label></td>" +
                                "<td style='display:none;'><lable id='lbl_member_pay_mode" + member_row + "'>" + item.PaymentCodeID +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_premium_by_mode" + member_row + "'>" + item.PremiumByMode +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_annually_premium" + member_row + "'>" + item.AnnualPremium +  "</label></td>" +
                                "<td style='display:none;'><lable id='lbl_member_annual_origin_premium" + member_row + "'>" + item.AnnualOriginPremium +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_discount" + member_row + "'>" + item.Discount +  "</label></td>" +
                                "<td style='display:none;'><lable id='lbl_member_percentage" + member_row + "'>" + item.ExtraPercentage +  "</label></td>" +
                                "<td style='display:none;'><lable id='lbl_member_rate" + member_row + "'>" + item.ExtraRate +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_extra_premium" + member_row + "'>" + item.ExtraPremiumByMode +  "</label></td>" +
                                "<td style='display:none;'><lable id='lbl_member_extra_annual_premium" + member_row + "'>" + item.ExtraAnnualPremium +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_total_premium_after_dis" + member_row + "'>" + item.PremiumByMode +  "</label></td>" +
                                "<td style= 'vertical-align:middle;'><lable id='lbl_member_remarks" + member_row + "'>" + item.Remarks +  "</label></td>" +
                                "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                                "<button type='button' id='btn_member_delete" + member_row + "' value='Delete' onclick='remove_member(" + member_row + ");' ><span class='icon-remove'></span></button>" +
                                "<button type='button' id='btn_member_select" + member_row + "' value='Select' onclick='select_member(" + member_row + ");' ><span class='icon-edit'></span></button></td" +
                              "</tr>";
                        $('#tbl_member_list').append(row);

                        // alert(' Customer ID : ' + $('#lbl_member_customer_id' + member_row).text());
                        //alert('Member ID: ' + $('#lbl_member_id' + member_row).text());
                        
                    });
                   
                }
               
            });
        }

        //get pay and assure year by product id
        function  get_pay_assured_year(product_id)
        {
            if(product_id!='')
            {
                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/GetPay_AssuredYear",
                    data: "{product_id:'" + product_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.length > 0) {
                            policy_term_insurance.val(data.d[1]);
                            policy_pay_period.val(data.d[0]);
                        }
                        else {
                            alert('Pay and Assured year not found.');
                        }
                    },
                    error:function(err){
                        alert("Something got error :" +err);
                    }
                });
            }
        }

        //open search form
        function search_form(option) {
            customer_search_option = option;
            $('#modal_search_customer').modal('show');
        }

        //search customers
        function search_customer() {
            var btn = $('#btnSearchCustomer');
            if (btn.val() == 'Search') {//first search

                var full_name = $('#<%=txtSearchCustName.ClientID%>').val();
                var customer_type_id = $('#<%=ddlSearchCustType.ClientID%>').val();
                var id_card_search = $('#<%=txtSearchIDCard.ClientID%>').val();
                var customer_number_search = $('#<%=txtSearchCustNo.ClientID%>').val();

                if (full_name == '' && customer_type_id == '' && id_card_search == '' && customer_number_search == '') {
                    return false;
                }

                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/GetCustomerList",
                    data: "{customer_name:'" + full_name + "', customer_type_id:'" + customer_type_id + "', id_card:'" + id_card_search + "', customer_id:'', customer_number:'" + customer_number_search + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.length > 0) {
                            $('#dvCustomerSearchList').setTemplate($("#jTemplateCustomer").html());
                            $('#dvCustomerSearchList').processTemplate(data);
                        }
                        else {
                            $('#dvCustomerSearchList').html('');
                        }
                    }

                });

                
            }
            else { //paste some information of customer after search when you click button ok
                
                if (customer_search_option == 1)//search customer for policy details
                {
                    policy_customer.val(customer_search_name);
                    policy_customer_type.val(customer_search_type_id);
                    policy_customer_id.val(customer_search_id);
                    if(policy_customer_type.find('option:selected').text().trim()=='Individual')
                    {
                        //paste customer name, customer id into member tab
                        member_customer.val(customer_search_name);
                        member_customer_id.val(customer_search_id);
                        member_gender.val(customer_search_gender);
                        member_birth_date.val(customer_search_birth_date);

                        //paste customer name, customer id into policy owner
                        policy_owner_name.val(customer_search_name);
                        policy_owner_birth_date.val(customer_search_birth_date);
                        policy_owner_gender.val(customer_search_gender);
                    
                    }
                    else
                    {
                        //paste customer name, customer id into policy owner
                        policy_owner_name.val(customer_search_name);
                        policy_owner_birth_date.val('');
                        policy_owner_gender.val('');
                    }
                }
                else if (customer_search_option == 2)//search customer for members
                {
                    member_customer.val(customer_search_name);
                    member_customer_id.val(customer_search_id);
                    member_gender.val(customer_search_gender);
                    member_birth_date.val(customer_search_birth_date);
                }

                btn.val('Search');
                $('#modal_search_customer').modal('hide');
            }
        }
        //get some information of customer in search form
        function get_customer_info(customer_id, customer_name, customer_type_id, customer_birth_date, customer_gender, index, total_row) {
            if ($('#Ch' + index).is(':checked')) {
                customer_search_id = customer_id;
                customer_search_name = customer_name;
                customer_search_type_id = customer_type_id;
                customer_search_birth_date = formatJSONDate( customer_birth_date);
                if(customer_gender=='0')
                {
                    customer_search_gender = "Female";
                }
                else if(customer_gender=='1')
                {
                    customer_search_gender = "Male";
                }
                
                // alert(customer_search_id);
                $('#Ch' + index).attr('background-color', 'red');

                $('#btnSearchCustomer').val('Ok');
            } else {
                customer_search_id = "";
                $('#btnSearchCustomer').val('Search');
            }
            //Uncheck all other checkboxes
            for (var i = 0; i < total_row  ; i++) {
                if (i != index) {
                    $('#Ch' + i).prop('checked', false);
                }

            }
        }
        //Add Members
        function add_member()
        {
            if(validate_member()!='')
            {
                alert(validate_member());
                return false;
            }
            var row ='';
            if(selected_member_row_index>0)//update member
            {
                $('#lbl_member_customer_id' + selected_member_row_index).text(member_customer_id.val());
                $('#lbl_member_customer' + selected_member_row_index).text( member_customer.val());
                $('#lbl_member_effective_date' + selected_member_row_index).text(member_effective_date.val());
                $('#lbl_member_gender' + selected_member_row_index).text( member_gender.val());
                $('#lbl_member_birth_date' + selected_member_row_index).text(member_birth_date.val());
                $('#lbl_member_si' + selected_member_row_index).text(member_si.val());
                $('#lbl_member_age' + selected_member_row_index).text( member_age.val());
                $('#lbl_member_premium_by_mode' + selected_member_row_index).text(member_premium_by_mode.val());
                $('#lbl_member_annually_premium' + selected_member_row_index).text(member_annually_premium.val());
                $('#lbl_member_annual_origin_premium' + selected_member_row_index).text( member_annual_origin_premium.val());
                $('#lbl_member_percentage' + selected_member_row_index).text(member_percentage.val());
                $('#lbl_member_rate' + selected_member_row_index).text( member_rate.val());
                $('#lbl_member_extra_premium' + selected_member_row_index).text( member_extra_premium.val());
                $('#lbl_member_extra_annual_premium' + selected_member_row_index).text( member_extra_annual_premium.val());
                $('#lbl_member_remarks' + selected_member_row_index).text( member_remarks.val());
                $('#lbl_member_discount' + selected_member_row_index).text(member_discount.val());
                
               

                //clear member text boxes
                clear_member_text_box();
            }
            else //add new member
            {
                member_row +=1;

                row = "<tr id='member_row_id" + member_row +  "'>" + 
                        "<td style='display:none;'><lable id='lbl_member_no" + member_row  +  "'>" + member_row + "</label><lable id='lbl_member_customer_id" + member_row  +  "'>" + member_customer_id.val() + "</label><lable id='lbl_member_id" + member_row  +  "'></label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_customer" + member_row + "'>" + member_customer.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_age" + member_row + "'>" + member_age.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_gender" + member_row + "'>" + member_gender.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_birth_date" + member_row + "'>" + member_birth_date.val() +  "</label></td>" +
                        "<td style='display:none;'><lable id='lbl_member_effective_date" + member_row + "'>" + member_effective_date.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_si" + member_row + "'>" + member_si.val() +  "</label></td>" +
                        "<td style='display:none;'><lable id='lbl_member_pay_mode" + member_row + "'>" + policy_payment_code.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_premium_by_mode" + member_row + "'>" + member_premium_by_mode.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_annually_premium" + member_row + "'>" + member_annually_premium.val() +  "</label></td>" +
                        "<td style='display:none;'><lable id='lbl_member_annual_origin_premium" + member_row + "'>" + member_annual_origin_premium.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_discount" + member_row + "'>" + member_discount.val() +  "</label></td>" +
                        "<td style='display:none;'><lable id='lbl_member_percentage" + member_row + "'>" + member_percentage.val() +  "</label></td>" +
                        "<td style='display:none;'><lable id='lbl_member_rate" + member_row + "'>" + member_rate.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_extra_premium" + member_row + "'>" + member_extra_premium.val() +  "</label></td>" +
                        "<td style='display:none;'><lable id='lbl_member_extra_annual_premium" + member_row + "'>" + member_extra_annual_premium.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_total_premium_after_dis" + member_row + "'>" + member_total_premium_after_dis.val() +  "</label></td>" +
                        "<td style= 'vertical-align:middle;'><lable id='lbl_member_remarks" + member_row + "'>" + member_remarks.val() +  "</label></td>" +
                        "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                        "<button type='button' id='btn_member_delete" + member_row + "' value='Delete' onclick='remove_member(" + member_row + ");' ><span class='icon-remove'></span></button>" +
                        "<button type='button' id='btn_member_select" + member_row + "' value='Select' onclick='select_member(" + member_row + ");' ><span class='icon-edit'></span></button></td" +
                      "</tr>";
                $('#tbl_member_list').append(row);
            }
        }
        
        //Clear member controls
        function clear_member_text_box()
        {
            member_customer_id.val('');
            member_customer.val('');
            member_birth_date.val('');
            member_gender.val('');
            member_age.val('');
            member_effective_date.val('');
            member_si.val('');
            member_premium_by_mode.val('');
            member_annual_origin_premium.val('');
            member_annually_premium.val('');
            member_extra_annual_premium.val('');
            member_extra_premium.val('');
            member_rate.val('');
            member_percentage.val('');
            member_discount.val('');
            member_total_premium_after_dis.val('');
            member_remarks.val('');

            //clear selected row color
            $('#member_row_id'+selected_member_row_index).removeClass('select_row');
            selected_member_row_index=0;
        }
        //clear all member controls
        function clear_member_controls()
        {
            clear_member_text_box();
            $('#tbl_member_list tr').each(function(index, element){
                $('#member_row_id'+index).remove();
            });
        }

        //clear form
        function clear_form()
        {
          
            //clear member tab
            clear_member_controls();
            //clear cover tab
            clear_cover_controlls();
        }
        
        //validate member
        function validate_member()
        {
            if(member_customer.val().trim()=='')
            {
                return 'Member is required.';
            }
            else if(member_birth_date.val().trim()=='')
            {
                return 'Birth date is required.';
            }
            else if(member_gender.val().trim()=='')
            {
                return 'Gender is required.';
            }
            else if(member_age.val().trim()=='')
            {
                return 'Age is required.';
            }
            else if(member_effective_date.val().trim()=='')
            {
                return 'Effective date is required.';
            }
            else if(member_si.val().trim()=='')
            {
                return 'Sum insured is required.';
            }
            else if(member_premium_by_mode.val().trim()=='')
            {
                return 'Premium by mode is required.';
            }
            else if (member_annual_origin_premium.val().trim()=='')
            {
                return 'Annual premium is required.';
            }
            else if(member_annually_premium.val().trim()=='')
            {
                return 'Annual premium rounded is required';
            }
            else
            {
                return '';
            }
        }

        //delete member from list
        function remove_member(row_index)
        {
            if (row_index > 0) {
                member_id_delete += $('#lbl_member_no' + row_index).text() + ";";
                alert(member_id_delete);
                $('#member_row_id' + row_index).remove();

                //reset selected row index if user selectd record
                selected_member_row_index = 0;
                member_row -=1;
            }
        }
        //select contact row
        var selected_member_row_index = 0;
        
        function select_member(row_index) {
            selected_member_row_index = row_index;//use for updating contact
            $('#tbl_member_list tr').each(function (index, element) {
                
                $('#member_row_id' + index).removeClass('select_row');
            });
            //$('#row_contact_list' + row_index).css('background-color', '#9974');
            $('#member_row_id' + row_index).addClass('select_row');

            member_customer_id.val($('#lbl_member_customer_id' + row_index).text());
            member_customer.val($('#lbl_member_customer' + row_index).text());
            member_effective_date.val($('#lbl_member_effective_date' + row_index).text());
            member_gender.val($('#lbl_member_gender' + row_index).text());
            member_birth_date.val($('#lbl_member_birth_date' + row_index).text());
            member_si.val($('#lbl_member_si' + row_index).text());
            member_age.val($('#lbl_member_age' + row_index).text());
            member_premium_by_mode.val($('#lbl_member_premium_by_mode' + row_index).text());
            member_annually_premium.val($('#lbl_member_annually_premium' + row_index).text());
            member_annual_origin_premium.val($('#lbl_member_annual_origin_premium' + row_index).text());
            member_percentage.val($('#lbl_member_percentage' + row_index).text());
            member_rate.val($('#lbl_member_rate' + row_index).text());
            member_extra_premium.val($('#lbl_member_extra_premium' + row_index).text());
            member_extra_annual_premium.val($('#lbl_member_extra_annual_premium' + row_index).text());
            member_remarks.val($('#lbl_member_remarks' + row_index).text());
            member_discount.val($('#lbl_member_discount' + row_index).text());

            //total after discount
            //member_total_premium_after_dis.val ( parseInt( member_premium_by_mode.val()) + parseInt( member_extra_premium.val()) - parseInt( member_discount.val()));
            calcMemberTotalPremiumAfterDist();

        }

        //validate cover
        function validate_cover()
        {
            if(cover_type.val().trim()=='')
            {
                return 'Cover type is required.';
            }
            else if(cover_si.val().trim()=='')
            {
                return 'Sum insured is required.';
            }
            else if(cover_effective_date.val().trim()=='')
            {
                return 'Effecive date is required.';
            }
            else if(cover_premium.val().trim()=='')
            {
                return 'Premium is required.';
            }
            else if(cover_annual_origin_premium.val().trim()=='')
            {
                return 'Annual premium is required.';
            }
            else if(cover_annual_premium.val().trim()=='')
            {
                return 'Annual premium rounded is required.';
            }
            else
            {
                return '';
            }
        }
        //check exist cover
        var exist_cover=false;
        function check_exist_cover(cover_type_id)
        {
            
            $('#tbl_cover_list tr').each(function(index, element){
               
                if( $('#lbl_cover_type_id' + index).text().trim() == cover_type_id)
                {
                    //alert( 'Exist : ' + $('#lbl_cover_type_id' + index).text() + ' ----- ' + cover_type_id);
                    exist_cover =true;
                    return false; // break loop
                }
                else
                {
                    exist_cover =false;
                }
               
            });
        }

        //add cover
        var cover_row=0;
        function add_cover()
        {
            var str_valid= validate_cover();
            var cover_name = cover_type.find('option:selected').text();

            if(str_valid.trim()!='')
            {
                alert(str_valid);
                return false;
            }

            if(selected_cover_row>0)//update selected cover
            {
                //code here
                $('#lbl_cover_id' + selected_cover_row).text(cover_id.val());
                $('#lbl_cover_type_id' + selected_cover_row).text( cover_type.val());
                $('#lbl_cover_type' + selected_cover_row).text( cover_name);
                $('#lbl_cover_si' + selected_cover_row).text(cover_si.val());
                $('#lbl_cover_effective_date' + selected_cover_row).text(cover_effective_date.val());
                $('#lbl_cover_premium' + selected_cover_row).text(cover_premium.val());
                $('#lbl_cover_annual_origin_premium' + selected_cover_row).text(cover_annual_origin_premium.val());
                $('#lbl_cover_annual_premium' + selected_cover_row).text(cover_annual_premium.val());
                $('#lbl_cover_remarks' + selected_cover_row).text(cover_remarks.val());
            }
            else//add new cover
            {
                var row;
                //process exist cover function
                check_exist_cover(cover_type.val().trim());
            
                if(exist_cover)
                {
                    alert('already exist');
                    return false;
                
                }

                cover_row +=1;
                row = "<tr id='cover_row_id" + cover_row +  "'>" + 
                    "<td style='text-align:center;'><label id='lbl_cover_no" + cover_row + "'>" + cover_row + "</label></td>" + 
                    "<td style='display:none;'><label id='lbl_cover_id" + cover_row + "'></label></td>" + 
                    "<td style='text-align:center;'><label id='lbl_cover_type" + cover_row + "'>" + cover_name + "</label></td>" +
                    "<td style='display:none;'><label id='lbl_cover_type_id" + cover_row + "'>" +  cover_type.val() + "</label></td>" + 
                    "<td style='text-align:center;'><label id='lbl_cover_si" + cover_row + "'>" + cover_si.val() + "</label></td>" + 
                    "<td style='text-align:center;'><label id='lbl_cover_effective_date" + cover_row + "'>" + cover_effective_date.val() + "</label></td>" +
                    "<td style='text-align:center;'><label id='lbl_cover_premium" + cover_row + "'>" + cover_premium.val() + "</label></td>" +
                    "<td style='text-align:center;'><label id='lbl_cover_annual_origin_premium" + cover_row + "'>" + cover_annual_origin_premium.val() + "</label></td>" + 
                    "<td style='text-align:center;'><label id='lbl_cover_annual_premium" + cover_row + "'>" + cover_annual_premium.val() + "</label></td>" + 
                    "<td style='text-align:center;'><label id='lbl_cover_remarks" + cover_row + "'>" + cover_remarks.val() + "</label></td>" + 
                    "<td style='padding:3px; height:20px; width:20px; vertical-align:middle; text-align:center;'>" +
                    "<button type='button' id='btn_cover_delete" + cover_row + "' value='Delete' onclick='remove_cover(" + cover_row + ");' ><span class='icon-remove'></span></button>" +
                    "<button type='button' id='btn_cover_select" + cover_row + "' value='Select' onclick='select_cover(" + cover_row + ");' ><span class='icon-edit'></span></button></td" +
                    "</tr>";
                $('#tbl_cover_list').append(row);
            }
            // clear cover text boxes
            clear_cover_few_text_boxes();

        }

        //clear cover text boxes
        function clear_cover_text_boxes()
        {
            cover_id.val('');
            cover_type.val('');
            cover_si.val('');
            cover_premium.val('');
            cover_annual_origin_premium.val('');
            cover_annual_premium.val('');
            cover_effective_date.val('');
            cover_remarks.val('');

            alert(selected_cover_row);

            $('#cover_row_id'+selected_cover_row).removeClass('select_row');
            selected_cover_row=0;

        }
        //clear cover text boxes
        //keep sum insured and effective date
        function clear_cover_few_text_boxes()
        {
            cover_id.val('');
            cover_type.val('');
            cover_premium.val('');
            cover_annual_origin_premium.val('');
            cover_annual_premium.val('');
            cover_remarks.val('');
            
            $('#cover_row_id'+selected_cover_row).removeClass('select_row');
            selected_cover_row=0;

        }
        //clear all cover controlls
        function clear_cover_controlls()
        {
            clear_cover_text_boxes();
            //clear table cover list
            $('#tbl_cover_list tr').each(function(index, element){
                $('#cover_row_id'+index).remove();
            });

        }

        //selected cover row
        var selected_cover_row=0;
        function select_cover(row_index)
        {
            selected_cover_row = row_index;// purpose use for update selected record from cover list
            //clear previous selected
            $('#tbl_cover_list tr').each(function (index, element) {
                
                $('#cover_row_id' + index).removeClass('select_row');
            });
            //set back color to selected row
            $('#cover_row_id' + row_index).addClass('select_row');

            //paste data to controlls from selected cover
            cover_id.val($('#lbl_cover_id' + row_index).text());
            cover_type.val($('#lbl_cover_type_id' + row_index).text());
            cover_si.val($('#lbl_cover_si' + row_index).text());
            cover_effective_date.val($('#lbl_cover_effective_date' + row_index).text());
            cover_premium.val($('#lbl_cover_premium' + row_index).text());
            cover_annual_origin_premium.val($('#lbl_cover_annual_origin_premium' + row_index).text());
            cover_annual_premium.val($('#lbl_cover_annual_premium' + row_index).text());
            cover_remarks.val($('#lbl_cover_remarks' + row_index).text());

        }
        
        //delete cover row
        function remove_cover(row_index)
        {
            
            if(confirm('Are you sure to delete ' + $('#lbl_cover_type' + row_index).text() + '?'))
            {
                var cover_id =$('#lbl_cover_id' + row_index).text().trim();
                if(cover_id!='')
                {
                    cover_delete_id += cover_id + ';';
                }
                
                $('#cover_row_id' + row_index).remove();
                selected_cover_row=0;
                cover_row -=1;
            }
        }

        //save cover
        function save_cover()
        {
            var tbl =$('#tbl_cover_list tr:gt(0)');
           
            if(tbl.length>0)
            {
                var covers = new Array();
                tbl.each (function (index, element){
                    
                    var td = $(element).find('td');
                    var i = td.eq(0).text(); // index value
                    
                    var index=0;
                    
                    covers[index] = $('#lbl_cover_id' + i).text() + ';' +
                                policy_id.val() + ';' +
                                $('#lbl_member_customer_id1').text() + ';' + // from member tab
                                $('#lbl_cover_type_id' + i).text() + ';' +
                                $('#lbl_member_age1').text() + ';' + // from member tab
                                policy_pay_period.val() + ';' +
                                policy_term_insurance.val() + ';' +
                                $('#lbl_cover_si' + i).text() + ';' +
                                $('#lbl_cover_annual_premium' + i).text() + ';' +
                                $('#lbl_cover_annual_origin_premium' + i).text() + ';' +
                                $('#lbl_cover_premium' + i).text() + ';' +
                                policy_pay_mode.val() + ';' + //from policy detail
                                $('#lbl_cover_remarks' + i).text() + ';' +
                                $('#lbl_cover_effective_date' + i).text() ;
                    index +=1;

                   
                });

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/SaveCover",
                    data:JSON.stringify({covers:covers}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data){
                        if(data.d)
                        {
                            //delete cover
                            $.ajax({
                                type: "POST",
                                url: "../../PolicyWebService.asmx/DeleteCover",
                                data:JSON.stringify({covers:cover_delete_id}),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function(data){
                                    //code here
                                }
               
                            });
                            //alert('Saved successfully.');
                            return true;
                        }
                        else
                        {
                            //alert('Saved fail.');
                            return false;
                        }
                    },
                    error: function (err){
                        alert('Save cover error : ' + err);
                    }

               
                });
            }

        }
        //get cover list
        function get_cover(policy_id)
        {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetCover",
                data:JSON.stringify({policy_id:policy_id}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data){
                    $.each(data.d, function(index, item){
                        cover_row +=1;
                        var row='';
                        row = "";
                        row = "<tr id='cover_row_id" + cover_row +  "'>" + 
                                "<td style='text-align:center;'><label id='lbl_cover_no" + cover_row + "'>" + cover_row + "</label></td>" + 
                                "<td style='display:none;'><label id='lbl_cover_id" + cover_row + "'>" + item.PolicyCoverID + "</label></td>" + 
                                "<td style='text-align:center;'><label id='lbl_cover_type" + cover_row + "'>" +  item.CoverType + "</label></td>" + 
                                "<td style='display:none;'><label id='lbl_cover_type_id" + cover_row + "'>" +  item.CoverTypeID + "</label></td>" + 
                                "<td style='text-align:center;'><label id='lbl_cover_si" + cover_row + "'>" + item.SumInsured + "</label></td>" + 
                                "<td style='text-align:center;'><label id='lbl_cover_effective_date" + cover_row + "'>" + formatJSONDate( item.EffectiveDate) + "</label></td>" +
                                "<td style='text-align:center;'><label id='lbl_cover_premium" + cover_row + "'>" + item.PremiumByMode + "</label></td>" +
                                "<td style='text-align:center;'><label id='lbl_cover_annual_origin_premium" + cover_row + "'>" + item.AnnualOriginPremium + "</label></td>" + 
                                "<td style='text-align:center;'><label id='lbl_cover_annual_premium" + cover_row + "'>" + item.AnnualPremium + "</label></td>" + 
                                "<td style='text-align:center;'><label id='lbl_cover_remarks" + cover_row + "'>" + item.Remarks + "</label></td>" + 
                                "<td style='padding:3px; height:20px; width:20px; vertical-align:middle; text-align:center;'>" +
                                "<button type='button' id='btn_cover_delete" + cover_row + "' value='Delete' onclick='remove_cover(" + cover_row + ");' ><span class='icon-remove'></span></button>" +
                                "<button type='button' id='btn_cover_select" + cover_row + "' value='Select' onclick='select_cover(" + cover_row + ");' ><span class='icon-edit'></span></button></td" +
                                "</tr>";
                        $('#tbl_cover_list').append(row);
                        //alert ('get cover, row count: ' + cover_row);
                    });
                }
               
            });
            
        }

        //Format Date in Jtemplate
        function formatJSONDate(jsonDate) {
            var value = jsonDate;
            if (value.substring(0, 6) == "/Date(") {
                var dt = new Date(parseInt(value.substring(6, value.length - 2)));
                var day, month;

                if(dt.getDate()<=9)
                {
                    day = '0' + dt.getDate();

                }
                else 
                {
                    day=dt.getDate();
                }
                if((dt.getMonth()+1)<=9)
                {
                    month = '0' + (dt.getMonth()+1);
                }
                else
                {
                    month=dt.getMonth()+1;
                }
                //var dtString = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                var dtString = day + "/" + month + "/" + dt.getFullYear();
                
                value = dtString;
            }
            return value;
        }

        //paste effective date to member effecive date, riders effective date and additional effective date, if customer type = individual
        function paste_effective(effective_date)
        {
            //alert('['+policy_customer_type.find('option:selected').text()+']');
            var selected_text=policy_customer_type.find('option:selected').text();

            if(selected_text.trim() == 'Individual')
            {
                member_effective_date.val(effective_date);
                cover_effective_date.val(effective_date);
                rider_effective_date.val(effective_date);
                //alert(effective_date + ' ' + policy_customer_type.find('option:selected').text());
            }
           
        }
        //paste sum inured to member sum inured and additional sum inured, if customer type = individual
        function paste_sum_insured(sum_insured)
        {
            //alert('['+policy_customer_type.find('option:selected').text()+']');
            var selected_text=policy_customer_type.find('option:selected').text();

            if(selected_text.trim() == 'Individual')
            {
                member_si.val(sum_insured);
                cover_si.val(sum_insured);
               
            }
            else
            {
                alert('Customer Type Is Required.');
            }
           
        }

        //add riders
        function add_rider()
        {
            var str_message =validate_rider();

            if(str_message!='')
            {
                alert(str_message);
                return false;
            }
            var gender = '';
            if(rider_gender.val()==0)
            {
                gender='Female';
            }
            else
            {
                gender='Male';
            }
            var extra_annual_prem =0;
            var extra_prem=0;
            if(rider_extra_annual_premium.val()=='')
            {
                extra_annual_prem=0;
            }
            else
            {
                extra_annual_prem = rider_extra_annual_premium.val();
            }
            if(rider_extra_premium.val()=='')
            {
                extra_prem =0;
            }
            else
            {
                extra_prem =rider_extra_premium.val();
            }

            if(rider_selected_row==0)//add new
            {
                rider_row_no +=1;

                var row = "<tr id='rider_row" + rider_row_no + "'>"+ 
                    "<td style='display:none;'><label id='lblRiderNo" + rider_row_no + "'>"  + rider_row_no + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderID" + rider_row_no + "'></label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderKhName" + rider_row_no + "'>" + rider_kh_last_name.val() + " " + rider_kh_first_name.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblRiderEnName" + rider_row_no + "'>" + rider_en_last_name.val() + " " + rider_en_first_name.val() + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderFirstNameKh" + rider_row_no + "'>" + rider_kh_first_name.val() + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderLastNameKh" + rider_row_no + "'>" + rider_kh_last_name.val() + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderFirstNameEn" + rider_row_no + "'>" + rider_en_first_name.val() + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderLastNameEn" + rider_row_no + "'>" + rider_en_last_name.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderGender" + rider_row_no + "'>" + rider_gender.find('option:selected').text() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderBirthDate" + rider_row_no + "'>" + rider_birth_date.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderAge" + rider_row_no + "'>" + rider_age.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderIDType" + rider_row_no + "'>" + rider_id_type.find('option:selected').text() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderIDCard" + rider_row_no + "'>" + rider_id_card.val() + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderNationality" + rider_row_no + "'>" + rider_nationality.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderType" + rider_row_no + "'>" + rider_type.find('option:selected').text() + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderRelationship" + rider_row_no + "'>" + rider_relationship.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderEffectiveDate" + rider_row_no + "'>" + rider_effective_date.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderSI" + rider_row_no + "'>" + rider_si.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderPremiumByMode" + rider_row_no + "'>" + rider_premium_by_mode.val() + "</label></td>" +
                    "<td  style='display:none;'><label id='lblRiderAnnualOriginPremium" + rider_row_no + "'>" + rider_annual_origin_premium.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderAnnualPremium" + rider_row_no + "'>" + rider_annual_premium.val() + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderExtraPremium" + rider_row_no + "'>" + extra_prem + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderExtraAnnualPremium" + rider_row_no + "'>" + extra_annual_prem + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblRiderRemarks" + rider_row_no + "'>" + rider_remarks.val() + "</label></td>" +
                     "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                                "<button type='button' id='btn_rider_delete" + rider_row_no + "' value='Delete' onclick='remove_rider(" + rider_row_no + ");' ><span class='icon-remove'></span></button>" +
                                "<button type='button' id='btn_rider_select" + rider_row_no + "' value='Select' onclick='select_rider(" + rider_row_no + ");' ><span class='icon-edit'></span></button></td" +
                    "</tr>";
                $('#tbl_rider_list').append(row);
            }
            else // update
            {
                $('#lblRiderFirstNameKh' + rider_selected_row).text(rider_kh_first_name.val());
                $('#lblRiderLastNameKh' + rider_selected_row).text(rider_kh_last_name.val());
                $('#lblRiderFirstNameEn' + rider_selected_row).text(rider_en_first_name.val());
                $('#lblRiderLastNameEn' + rider_selected_row).text(rider_en_last_name.val());
                $('#lblRiderKhName' + rider_selected_row).text(rider_kh_last_name.val() + ' ' + rider_kh_first_name.val());
                $('#lblRiderEnName' + rider_selected_row).text(rider_en_last_name.val() + ' ' + rider_en_first_name.val());
                $('#lblRiderGender' + rider_selected_row).text(rider_gender.find('option:selected').text());
                $('#lblRiderBithDate' + rider_selected_row).text(rider_birth_date.val());
                $('#lblRiderAge' + rider_selected_row).text(rider_age.val());
                $('#lblRiderIDType' + rider_selected_row).text(rider_id_type.find('option:selected').text());
                $('#lblRiderIDCard' + rider_selected_row).text(rider_id_card.val());
                $('#lblRiderNationality' + rider_selected_row).text(rider_nationality.val());
                $('#lblRiderType' + rider_selected_row).text(rider_type.find('option:selected').text());
                $('#lblRiderRelationship' + rider_selected_row).text(rider_relationship.val());
                $('#lblEffectiveDate' + rider_selected_row).text(rider_effective_date.val());
                $('#lblRiderSI' + rider_selected_row).text(rider_si.val());
                $('#lblRiderPremiumByMode' + rider_selected_row).text(rider_premium_by_mode.val());
                $('#lblRiderAnnualPremium' + rider_selected_row).text(rider_annual_premium.val());
                $('#lblRiderAnnualOriginPremium' + rider_selected_row).text(rider_annual_origin_premium.val());
                $('#lblRiderExtraPremium' + rider_selected_row).text(extra_prem);
                $('#lblRiderExtraAnnualPremium' + rider_selected_row).text(extra_annual_prem);
                $('#lblRiderRemarks' + rider_selected_row).text(rider_remarks.val());

                // $('#rider_row' + rider_selected_row).removeClass('select_row');
                clear_rider_selected_row(rider_selected_row);
                rider_selected_row=0;
            }
        }

        function validate_rider()
        {

            if(rider_en_first_name.val()=='')
            {
                return 'Frist name is required.';
            }
            else if(rider_en_last_name.val()=='')
            {
                return 'Last name is required.';
            }
            else if(rider_gender.val()=='')
            {
                return 'Gender is required.';
            }
            else if(rider_birth_date.val()=='')
            {
                return 'Birth date is required.';
            }
            else if(rider_id_type.val()=='')
            {
                return 'ID type is required.';
            }
            else if(rider_id_card.val()=='')
            {
                return 'ID card is required.';
            }
            else if(rider_nationality.val()=='')
            {
                return 'Nationality is required.';
            }
            else if(rider_type.val()=='')
            {
                return 'Rider type is required.';
            }
            else if(rider_relationship.val()=='')
            {
                return 'Relationship is required.';
            }
            else if(rider_effective_date.val()=='')
            {
                return 'Effective date is required.';
            }
            else if(rider_si.val()=='')
            {
                return 'Sum insured is required.';
            }
            else if(rider_premium_by_mode.val()=='')
            {
                return 'Premium by mode is required.';
            }
            else if(rider_annual_origin_premium.val()=='')
            {
                return 'Annual premium is required.';
            }
            else if(rider_annual_premium.val()=='')
            {
                return 'Annual premium rounded is required.';
            }
            else
            {
                return '';
            }
        }

        function remove_rider(row_index)
        {
            if(confirm('Do you want to remove this rider?'))
            {
                if($('#lblRiderID' + row_index).text() .trim()!='')
                {
                    rider_delete_id += $('#lblRiderID' + row_index).text() + ';' ;
                }

                $('#rider_row' + row_index).remove();
                // rider_row_no -=1;
                rider_selected_row =0;
            }
        }

        function select_rider(row_index)
        {
            rider_selected_row = row_index;
            $('#tbl_rider_list tr').each(function (index, element) {
                
                $('#rider_row' + index).removeClass('select_row');
            });
            //set back color to selected row
            $('#rider_row' + row_index).addClass('select_row');

            rider_kh_first_name.val($('#lblRiderFirstNameKh' + row_index).text());
            rider_kh_last_name.val($('#lblRiderLastNameKh' + row_index).text());
            rider_en_first_name.val($('#lblRiderFirstNameEn' + row_index).text());
            rider_en_last_name.val($('#lblRiderLastNameEn' + row_index).text());

            if($('#lblRiderGender' + row_index).text().trim().toUpperCase()=='MALE')
            {
                rider_gender.val(1);
            }
            else 
            {
                rider_gender.val(0);
            }
            rider_birth_date.val($('#lblRiderBirthDate' + row_index).text());
            rider_age.val($('#lblRiderAge' + row_index).text());
            rider_id_card.val($('#lblRiderIDCard' + row_index).text());
            
            var id_type =$('#lblRiderIDType' + row_index).text().trim().toUpperCase();
            if(id_type=='I.D CARD')
            {
                rider_id_type.val(0);
            }
            else if(id_type=='PASSPORT')
            {
                rider_id_type.val(1);
            }

            else if(id_type=='VISA')
            {
                rider_id_type.val(2);
            }
            else if(id_type=='BIRTH CERTIFICATE')
            {
                rider_id_type.val(3);
            }
            rider_nationality.val($('#lblRiderNationality' + row_index).text());

            var str_rider_type=$('#lblRiderType' + row_index).text().trim().toUpperCase();
            if(str_rider_type=='SPOUSE')
            {
                rider_type.val(1);
            }
            else if(str_rider_type=='KID 1')
            {
                rider_type.val(2);
            }
            else if(str_rider_type=='KID 2')
            {
                rider_type.val(3);
            }
            else if(str_rider_type=='KID 3')
            {
                rider_type.val(4);
            }
            else if(str_rider_type=='KID 4')
            {
                rider_type.val(5);
            }

            rider_relationship.val($('#lblRiderRelationship' + row_index).text());
            rider_effective_date.val($('#lblRiderEffectiveDate' + row_index).text());
            rider_si.val($('#lblRiderSI' + row_index).text());
            rider_premium_by_mode.val($('#lblRiderPremiumByMode' + row_index).text());
            rider_annual_origin_premium.val($('#lblRiderAnnualOriginPremium' + row_index).text());
            rider_annual_premium.val($('#lblRiderAnnualPremium' + row_index).text());
            rider_extra_annual_premium.val($('#lblRiderExtraAnnualPremium' + row_index).text());
            rider_extra_premium.val($('#lblRiderExtraPremium' +row_index).text());
            rider_remarks.val($('#lblRiderRemarks' + row_index).text());

        }

        function clear_rider_boxes()
        {
            
            rider_kh_first_name.val('');
            rider_kh_last_name.val('');
            rider_en_first_name.val('');
            rider_en_last_name.val('');
            rider_gender.prop('selectedIndex',0);
            rider_birth_date.val('');
            rider_age.val('');
            rider_id_type.prop('selectedIndex',0);
            rider_id_card.val('');
            rider_nationality.val('KH');
            rider_type.prop('selectedIndex',0);
            rider_relationship.prop('selectedIndex',0);
            rider_effective_date.val('');
            rider_si.val('');
            rider_premium_by_mode.val('');
            rider_annual_origin_premium.val('');
            rider_annual_premium.val('');
            rider_extra_annual_premium.val('');
            rider_extra_premium.val('');
            
            clear_rider_selected_row(rider_selected_row);
            rider_selected_row=0;

        }

        function clear_rider_selected_row(index)
        {
            $('#rider_row' + index).removeClass('select_row');
        }

        function save_rider()
        {
           
            var tbl =$('#tbl_rider_list tr:gt(0)');
            //alert(tbl.length);
            if(tbl.length>0)
            {
                var riders = new Array();
                var index=0;
                tbl.each (function (index, element){
                    var gender=-1;
                    var id_type =-1;
                    var str_id_type='';
                    var rider_type=0;
                    var str_rider_type='';
                    var td = $(element).find('td');
                    var i = td.eq(0).text(); // index value

                    if($('#lblRiderGender' + i).text() == 'Female')
                    {
                        gender=0;
                    }
                    else
                    {
                        gender=1;
                    }
                    str_id_type =$('#lblRiderIDType' + i).text().trim().toLowerCase();
                    if(str_id_type =='i.d card')
                    {
                        id_type=0;
                    }
                    else if(str_id_type == 'passport')
                    {
                        id_type=1;
                    }
                    else if(str_id_type=='visa')
                    {
                        id_type=2;
                    }
                    else if(str_id_type =='birth certificate')
                    {
                        id_type=3;
                    }
                    str_rider_type =  $('#lblRiderType' +i).text().trim().toLowerCase();
                    if(str_rider_type=='spouse')
                    {
                        rider_type =1;
                    }
                    else if(str_rider_type =='kid 1')
                    {
                        rider_type=2;
                    }
                    else if(str_rider_type =='kid 2')
                    {
                        rider_type=3;
                    }
                    else if(str_rider_type =='kid 3')
                    {
                        rider_type=4;
                    }
                    else if(str_rider_type =='kid 4')
                    {
                        rider_type=5;
                    }

                    riders[index] = $('#lblRiderID' + i).text() + ';' +
                                    policy_id.val() + ';' +
                                    $('#lbl_member_customer_id1').text() + ';' + // from member tab
                                    $('#lblRiderFirstNameKh' + i).text() + ';' +
                                    $('#lblRiderLastNameKh' + i).text() + ';' +
                                    $('#lblRiderFirstNameEn' + i).text() + ';' +
                                    $('#lblRiderLastNameEn' + i).text() + ';' +
                                    gender + ';' +
                                    $('#lblRiderBirthDate' + i).text() + ';' +
                                    $('#lblRiderAge' + i).text() + ';' +
                                    id_type + ';' +
                                    $('#lblRiderIDCard' + i).text() + ';' +
                                    $('#lblRiderNationality' +i).text() + ';' +
                                    rider_type + ';' +
                                    $('#lblRiderRelationship' + i).text() + ';' +
                                    $('#lblRiderEffectiveDate' + i).text() + ';' +
                                    policy_term_insurance.val() + ';' +
                                    policy_pay_period.val() + ';' +
                                    $('#lblRiderSI' + i).text() + ';' +
                                    policy_pay_mode.val() + ';' + //from policy detail
                                    $('#lblRiderPremiumByMode' +i).text() + ';' +
                                    $('#lblRiderAnnualOriginPremium' +i).text() + ';' +
                                    $('#lblRiderAnnualPremium' +i).text() + ';' +
                                    $('#lblRiderExtraPremium' +i).text() + ';' +
                                    $('#lblRiderExtraAnnualPremium' +i).text() + ';' +
                                    $('#lblRiderRemarks' + i).text()
                   
                });

                //for(i=1;i<=rider_row_no;i++)
                //{
                //    var gender=-1;
                //    var id_type =-1;
                //    var str_id_type='';
                //    var rider_type=0;
                //    var str_rider_type='';

                //    if($('#lblRiderGender' + i).text() == 'Female')
                //    {
                //        gender=0;
                //    }
                //    else
                //    {
                //        gender=1;
                //    }
                //    str_id_type =$('#lblRiderIDType' + i).text().trim().toLowerCase();
                //    if(str_id_type =='i.d card')
                //    {
                //        id_type=0;
                //    }
                //    else if(str_id_type == 'passport')
                //    {
                //        id_type=1;
                //    }
                //    else if(str_id_type=='visa')
                //    {
                //        id_type=2;
                //    }
                //    else if(str_id_type =='birth certificate')
                //    {
                //        id_type=3;
                //    }
                //    str_rider_type =  $('#lblRiderType' +i).text().trim().toLowerCase();
                //    if(str_rider_type=='spouse')
                //    {
                //        rider_type =1;
                //    }
                //    else if(str_rider_type =='kid 1')
                //    {
                //        rider_type=2;
                //    }
                //    else if(str_rider_type =='kid 2')
                //    {
                //        rider_type=3;
                //    }
                //    else if(str_rider_type =='kid 3')
                //    {
                //        rider_type=4;
                //    }
                //    else if(str_rider_type =='kid 4')
                //    {
                //        rider_type=5;
                //    }

                //    riders[index] = $('#lblRiderID' + i).text() + ';' +
                //                    policy_id.val() + ';' +
                //                    $('#lbl_member_customer_id1').text() + ';' + // from member tab
                //                    $('#lblRiderFirstNameKh' + i).text() + ';' +
                //                    $('#lblRiderLastNameKh' + i).text() + ';' +
                //                    $('#lblRiderFirstNameEn' + i).text() + ';' +
                //                    $('#lblRiderLastNameEn' + i).text() + ';' +
                //                    gender + ';' +
                //                    $('#lblRiderBirthDate' + i).text() + ';' +
                //                    $('#lblRiderAge' + i).text() + ';' +
                //                    id_type + ';' +
                //                    $('#lblRiderIDCard' + i).text() + ';' +
                //                    $('#lblRiderNationality' +i).text() + ';' +
                //                    rider_type + ';' +
                //                    $('#lblRiderRelationship' + i).text() + ';' +
                //                    $('#lblRiderEffectiveDate' + i).text() + ';' +
                //                    policy_term_insurance.val() + ';' +
                //                    policy_pay_period.val() + ';' +
                //                    $('#lblRiderSI' + i).text() + ';' +
                //                    policy_pay_mode.val() + ';' + //from policy detail
                //                    $('#lblRiderPremiumByMode' +i).text() + ';' +
                //                    $('#lblRiderAnnualOriginPremium' +i).text() + ';' +
                //                    $('#lblRiderAnnualPremium' +i).text() + ';' +
                //                    $('#lblRiderExtraPremium' +i).text() + ';' +
                //                    $('#lblRiderExtraAnnualPremium' +i).text() + ';' +
                //                    $('#lblRiderRemarks' + i).text()
                                   
                //    index +=1;
                //}

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/SaveRiders",
                    data:JSON.stringify({riders:riders}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data){
                        if(data.d)
                        {
                            if(rider_delete_id!='')
                            {
                                $.ajax({
                                    type: "POST",
                                    url: "../../PolicyWebService.asmx/DeleteRiders",
                                    data:JSON.stringify({riders:rider_delete_id}),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function(data){
                                        
                                    }
                                });
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
               
                });
            }
        }

        function get_rider(policy_id)
        {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetRider",
                data:JSON.stringify({policy_id:policy_id}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data){
                    $.each(data.d, function(index, item){
                        
                        // add data into tbl_member_list
                        var row ='';
                        var gender='';
                        var id_type='';
                        var rider_type='';

                        if(item.Gender==0)
                        {
                            gender='Female';
                        }
                        else
                        {
                            gender='Male'
                        }
                       
                        if(item.IDTypeID==0)
                        {
                            id_type='I.D Card';
                        }
                        else if(item.IDTypeID==1)
                        {
                            id_type ='Passport';
                        }
                        else if(item.IDTypeID==2)
                        {
                            id_type='Visa';
                        }
                        else if(item.IDTypeID==3)
                        {
                            id_type='Birth Certificate';
                        }
                      
                        if(item.RiderType==1)
                        {
                            rider_type ='Spouse';
                        }
                        else if(item.RiderType==2)
                        {
                            rider_type='Kid 1';
                        }
                        else if(item.RiderType==3)
                        {
                            rider_type='Kid 2';
                        }
                        else if(item.RiderType==4)
                        {
                            rider_type='Kid 3';
                        }
                        else if(item.RiderType==5)
                        {
                            rider_type='Kid 4';
                        }
                        rider_row_no +=1;

                        var row = "<tr id='rider_row" + rider_row_no + "'>"+ 
                    "<td style='display:none;'><label id='lblRiderNo" + rider_row_no + "'>"  + rider_row_no + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderID" + rider_row_no + "'>" + item.PolicyRiderID + "</label></td>" +
                    "<td  style='text-align:center; vertical-align: middle;'><label id='lblRiderKhName" + rider_row_no + "'>" + item.LastNameKh + " " + item.FirstNameKh + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblRiderEnName" + rider_row_no + "'>" + item.FirstNameEn + " " + item.LastNameEn + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderFirstNameKh" + rider_row_no + "'>" + item.FirstNameKh + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderLastNameKh" + rider_row_no + "'>" + item.LastNameKh + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderFirstNameEn" + rider_row_no + "'>" + item.FirstNameEn + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderLastNameEn" + rider_row_no + "'>" + item.LastNameEn + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderGender" + rider_row_no + "'>" + gender + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderBirthDate" + rider_row_no + "'>" + formatJSONDate( item.BirthDate )+ "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderAge" + rider_row_no + "'>" + item.Age + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderIDType" + rider_row_no + "'>" + id_type + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderIDCard" + rider_row_no + "'>" + item.IDCard + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderNationality" + rider_row_no + "'>" + item.Nationality + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderType" + rider_row_no + "'>" + rider_type + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderRelationship" + rider_row_no + "'>" + item.Relationship + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderEffectiveDate" + rider_row_no + "'>" + formatJSONDate( item.EffectiveDate) + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderSI" + rider_row_no + "'>" + item.SumInsured + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderPremiumByMode" + rider_row_no + "'>" + item.PremiumByMode + "</label></td>" +
                    "<td style='display:none;'><label id='lblRiderAnnualOriginPremium" + rider_row_no + "'>" + item.AnnualOriginPremium + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderAnnualPremium" + rider_row_no + "'>" + item.AnnualPremium + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderExtraPremium" + rider_row_no + "'>" + item.ExtraPremiumByMode + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle;'><label id='lblRiderExtraAnnualPremium" + rider_row_no + "'>" + item.ExtraPremium + "</label></td>" +
                    "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblRiderRemarks" + rider_row_no + "'>" + item.Remarks + "</label></td>" +
                     "<td style='padding:3px; height:20px; width:20px; vertical-align:middle; text-align:center;'>" +
                                "<button type='button' id='btn_rider_delete" + rider_row_no + "' value='Delete' onclick='remove_rider(" + rider_row_no + ");' ><span class='icon-remove'></span></button>" +
                                "<button type='button' id='btn_rider_select" + rider_row_no + "' value='Select' onclick='select_rider(" + rider_row_no + ");' ><span class='icon-edit'></span></button></td" +
                    "</tr>";
                        $('#tbl_rider_list').append(row);
                        
                    });
                   
                }
               
            });
        }

        function validate_ben()
        {
            if(ben_nationality.val()=='')
            {
                return 'Nationality is required.';
            }
            else if(ben_birth_date.val()=='')
            {
                return 'Birth date is required.';
            }
            else if(ben_gender.val()=='')
            {
                return 'Gender is required.';
            }
            else if(ben_relationship.val()=='')
            {
                return 'Relationship is required.';
            }
            else if (ben_percentage.val()=='')
            {
                    return 'Percentage is required.';
            }
            else if(ben_percentage.val()==0)
            {
                return 'Percentage must be grather than zero.';
            }
            else if(ben_id_type.val()=='')
            {
                return 'ID type is required.';
            }
            else if(ben_id_card.val()=='')
            {
                return 'ID card is required.';
            }
            else
            {
                return '';
            }
        }

        function add_beneficiary()
        {
            var str_message = validate_ben();
            var tbl = $('#tbl_beneficiary_list');
            if(str_message!='')
            {
                alert (str_message);
                return false;
            }
            //check maximum percentage [max =100%]
            var max_percentate =(total_percentage + parseInt(ben_percentage.val())) - selected_percentage;
            
            //alert(max_percentate);

            if(max_percentate>100)
            {
                alert ('Percentage is not allowed over 100%.');
                               
            }
            else
            {
                total_percentage += parseInt(ben_percentage.val()) - selected_percentage;
                if(ben_selected_row==0)
                {
                    ben_row_no +=1;
                    var row ="<tr id='ben_row" + ben_row_no + "'>" + 
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenNo" + ben_row_no +"'>" + ben_row_no + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenID" + ben_row_no +"'></label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenFullName" + ben_row_no +"'>" + ben_kh_last_name.val() + ' ' + ben_kh_first_name.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenKhFirstName" + ben_row_no +"'>" + ben_kh_first_name.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenKhLastName" + ben_row_no +"'>" + ben_kh_last_name.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenEnFirstName" + ben_row_no +"'>" + ben_en_first_name.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenEnLastName" + ben_row_no +"'>" + ben_en_last_name.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenGender" + ben_row_no +"'>" + ben_gender.find('option:selected').text() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenBirthDate" + ben_row_no +"'>" + ben_birth_date.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenIDType" + ben_row_no +"'>" + ben_id_type.find('option:selected').text() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenIDCard" + ben_row_no +"'>" + ben_id_card.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenNationality" + ben_row_no +"'>" + ben_nationality.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenRelationship" + ben_row_no +"'>" + ben_relationship.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenPercentage" + ben_row_no +"'>" + ben_percentage.val() + "</label></td>" +
                              "<td style='text-align:center; vertical-align: middle;'><label id='lblBenRemarks" + ben_row_no +"'>" + ben_remarks.val() + "</label></td>" +
                              "<td style='padding:3px; height:20px; width:60px; vertical-align:middle; text-align:center;'>" +
                                "<button type='button' id='btn_rider_delete" + ben_row_no + "' value='Delete' onclick='remove_beneficiary(" + ben_row_no + ");' ><span class='icon-remove'></span></button>" +
                                "<button type='button' id='btn_rider_select" + ben_row_no + "' value='Select' onclick='select_beneficiary(" + ben_row_no + ");' ><span class='icon-edit'></span></button></td" +
                    "</tr>";

                    tbl.append(row);
                }
                else
                {
                    //code update record here.
                    
                    $('#lblBenKhFirstName' + ben_selected_row).text(ben_kh_first_name.val( ));
                    $('#lblBenKhLastName' + ben_selected_row).text(ben_kh_last_name.val());
                    $('#lblBenEnFirstName' + ben_selected_row).text(ben_en_first_name.val());
                    $('#lblBenEnLastName' + ben_selected_row).text(ben_en_last_name.val());
                    $('#lblBenGender' + ben_selected_row).text(ben_gender.find('option:selected').text());
                    $('#lblBenIDType' + ben_selected_row).text(ben_id_type.find('option:selected').text());
                    $('#lblBenBirthDate' + ben_selected_row).text(ben_birth_date.val());
                    $('#lblBenIDCard' + ben_selected_row).text(ben_id_card.val());

                    $('#lblBenNationality' + ben_selected_row).text(ben_nationality.val());
                    $('#lblBenRelationship' + ben_selected_row).text(ben_relationship.val());
                    $('#lblBenPercentage' + ben_selected_row).text(ben_percentage.val());
                    $('#lblBenRemarks' + ben_selected_row).text(ben_remarks.val());

                    clear_beneficiary_boxes();
                }
            }
        }

        function remove_beneficiary(row_index)
        {
            if(confirm('Do you want to remove the beneciary?'))
            {
                var ben_id=$('#lblBenID' + row_index).text().trim();
                if(ben_id!='')
                {
                    ben_delete_id +=ben_id + ';';
                }

                ben_selected_row =0;
                ben_row_no -=1;

                total_percentage -= parseInt($('#lblBenPercentage' + row_index).text());

                $('#ben_row' + row_index).remove();
            }
        }

        function select_beneficiary(row_index)
        {
            ben_selected_row = row_index;
            clear_beneficiary_selected_row();
            $('#ben_row' + row_index).addClass('select_row');

            var int_gender=-1;
            var int_id_type=-1;
            var str_id_type = $('#lblBenIDType' + row_index).text().trim();

            if($('#lblBenGender' + row_index).text()=='Female')
            {
                int_gender  =0;
            }
            else
            {
                int_gender=1;
            }
            if(str_id_type=='I.D Card')
            {
                int_id_type=0;
            }
            else if(str_id_type=='Passport')
            {
                int_id_type =1;
            }
            else if(str_id_type=='Visa')
            {
                int_id_type =2;
            }
            else if(str_id_type=='Birth certificate')
            {
                int_id_type =3;
            }
            selected_percentage = $('#lblBenPercentage' + row_index).text();
            ben_kh_first_name.val( $('#lblBenKhFirstName' + row_index).text());
            ben_kh_last_name.val($('#lblBenKhLastName' + row_index).text());
            ben_en_first_name.val($('#lblBenEnFirstName' + row_index).text());
            ben_en_last_name.val($('#lblBenEnLastName' + row_index).text());
            ben_gender.val(int_gender);
            ben_birth_date.val($('#lblBenBirthDate' + row_index).text());
            ben_id_type.val(int_id_type);
            ben_id_card.val($('#lblBenIDCard' + row_index).text());

            ben_nationality.val($('#lblBenNationality' + row_index).text());
            ben_relationship.val($('#lblBenRelationship' + row_index).text());
            ben_percentage.val(selected_percentage);
            ben_remarks.val($('#lblBenRemarks' + row_index).text());
        }

        function clear_beneficiary_selected_row()
        {
            $('#tbl_beneficiary_list tr').each(function(index, element){
                $('#ben_row' + index).removeClass('select_row');
            });
        }

        function clear_beneficiary_boxes()
        {
            ben_kh_first_name.val('');
            ben_kh_last_name.val('');
            ben_en_first_name.val('');
            ben_en_last_name.val('');
            ben_gender.prop('selectedIndex',0);
            ben_birth_date.val('');
            ben_id_type.prop('selectedIndex',0);
            ben_id_card.val('');
            ben_nationality.val('KH');
            ben_relationship.prop('selectedIndex',0);
            ben_percentage.val('');
            ben_remarks.val('');
            clear_beneficiary_selected_row();

            ben_selected_row=0;
            selected_percentage=0;

        }

        function obj()
        {
            this.A='Letter A';
            this.B='Letter B';
            this.Combine='A + B'
        }

        var ben_save_status =false;
        function save_beneficiary()
        {
            

            var result= false;
            var message='';
            var tbl =$('#tbl_beneficiary_list tr:gt(0)');
            //alert(tbl.length);
            if(tbl.length>0)
            {
                var beneficiary = new Array();
                var index=0;
                tbl.each (function (index, element){
                    var gender=-1;
                    var id_type =-1;
                    var str_id_type='';

                    var td = $(element).find('td');
                    var i = td.eq(0).text(); // index value

                    if($('#lblBenGender' + i).text() == 'Female')
                    {
                        gender=0;
                    }
                    else
                    {
                        gender=1;
                    }
                    str_id_type =$('#lblBenIDType' + i).text().trim().toLowerCase();
                    if(str_id_type =='i.d card')
                    {
                        id_type=0;
                    }
                    else if(str_id_type == 'passport')
                    {
                        id_type=1;
                    }
                    else if(str_id_type=='visa')
                    {
                        id_type=2;
                    }
                    else if(str_id_type =='birth certificate')
                    {
                        id_type=3;
                    }

                    beneficiary[index] = $('#lblBenID' + i).text() + ';' +
                                    policy_id.val() + ';' +
                                    $('#lbl_member_customer_id1').text() + ';' + // from member tab
                                    $('#lblBenKhFirstName' + i).text() + ';' +
                                    $('#lblBenKhLastName' + i).text() + ';' +
                                    $('#lblBenEnFirstName' + i).text() + ';' +
                                    $('#lblBenEnLastName' + i).text() + ';' +
                                    gender + ';' +
                                    $('#lblBenBirthDate' + i).text() + ';' +
                                    id_type + ';' +
                                    $('#lblBenIDCard' + i).text() + ';' +
                                    $('#lblBenNationality' +i).text() + ';' +
                                    $('#lblBenRelationship' + i).text() + ';' +
                                    $('#lblBenPercentage' + i).text() + ';' +
                                    $('#lblBenRemarks' + i).text()
                   
                });
                
                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/SaveBeneficiary",
                    data:JSON.stringify({beneficiary:beneficiary}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data){
                        if(data.d)
                        {
                            //delete some beneficiaries
                            if(ben_delete_id.trim()!='')
                            {
                                $.ajax({
                                    type: "POST",
                                    url: "../../PolicyWebService.asmx/DeleteBeneficiary",
                                    data:JSON.stringify({beneficiary:ben_delete_id}),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function(data){
                                    }
                                });
                            }
                            //return true;
                            // ben_save_status =true;
                            result=true;
                            message='Success';
                            this.Result=true;
                            this.Message='Success';
                        }
                        else
                        {
                            //return false;
                            //ben_save_status =false;
                            result =false;
                            message='Fail';
                            this.Result=false;
                            this.Message='Fail';
                        }
                    }
                });
            }
           
        }

        function get_beneficiary(policy_id)
        {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetBeneficiary",
                data:JSON.stringify({policy_id:policy_id}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data){
                    var gender ='';
                    var id_type ='';

                    $.each(data.d,function(index, item){
                        ben_row_no +=1;
                        if(item.Gender==0)
                        {
                            gender ='Female';
                        }
                        else 
                        {
                            gender ='Male';
                        }
                        if(item.IDTypeID==0)
                        {
                            id_type='I.D Card';
                        }
                        else if(item.IDTypeID==1)
                        {
                            id_type ='Passport';
                        }
                        else if(item.IDTypeID==2)
                        {
                            id_type='Visa';
                        }
                        else if(item.IDTypeID==3)
                        {
                            id_type='Birth Certificate';
                        }


                        total_percentage +=item.Percentage;

                        var tbl = $('#tbl_beneficiary_list');

                        var row ="<tr id='ben_row" + ben_row_no + "'>" + 
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenNo" + ben_row_no +"'>" + ben_row_no + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenID" + ben_row_no +"'> " + item.PolicyBenID +  " </label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenFullName" + ben_row_no +"'>" + item.FirstNameKh + ' ' + item.LastNameKh + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenKhFirstName" + ben_row_no +"'>" + item.FirstNameKh + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenKhLastName" + ben_row_no +"'>" + item.LastNameKh + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenEnFirstName" + ben_row_no +"'>" + item.FirstNameEn + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle; display:none;'><label id='lblBenEnLastName" + ben_row_no +"'>" + item.LastNameEn + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenGender" + ben_row_no +"'>" + gender + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenBirthDate" + ben_row_no +"'>" + formatJSONDate(item.BirthDate) + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenIDType" + ben_row_no +"'>" + id_type + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenIDCard" + ben_row_no +"'>" + item.IDCard + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenNationality" + ben_row_no +"'>" + item.Nationality + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenRelationship" + ben_row_no +"'>" + item.Relationship + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenPercentage" + ben_row_no +"'>" + item.Percentage + "</label></td>" +
                                  "<td style='text-align:center; vertical-align: middle;'><label id='lblBenRemarks" + ben_row_no +"'>" + item.Remarks + "</label></td>" +
                                  "<td style='padding:3px; height:20px; width:60px; vertical-align:middle; text-align:center;'>" +
                                    "<button type='button' id='btn_rider_delete" + ben_row_no + "' value='Delete' onclick='remove_beneficiary(" + ben_row_no + ");' ><span class='icon-remove'></span></button>" +
                                    "<button type='button' id='btn_rider_select" + ben_row_no + "' value='Select' onclick='select_beneficiary(" + ben_row_no + ");' ><span class='icon-edit'></span></button></td" +
                        "</tr>";

                        tbl.append(row);
                    });
                }
            });
        }

        //save policy, member, cover, rider and beneficiary
        function save_all()
        {
            if(confirm('Confirm to save policy.!'))
            {
               
                //save_member();
               
                //if(save_cover()==false)
                // {
                // alert('Saved additional cover fail.');
                // return false;
                //status_message='Saved additional cover fail.';
                //alert (status_message);
                //return false;
                //}
                //else
                //{
                //    alert ('Saved successfully.');
                //}
               // save_cover();
                // save_rider();

             // var a = new save_beneficiary();
                // alert(a.Result + '  message: ' + a.Message);

                var o= new obj();
                alert ( o.A + ',' + o.B + ': ' +o.Combine);
              
                //if( save_beneficiary())
                //{
                //    alert('Beneficiary saved successfully.');
                //}
                //else
                //{
                //    alert('Beneficiary saved fail.');
                //}

               
            }
        }
        //get all information
        function get_all(policy_id)
        {
            get_policy(policy_id);
            get_member(policy_id);
            get_cover(policy_id);
            get_rider(policy_id);
            get_beneficiary(policy_id);
        }
        function calcMemberTotalPremiumAfterDist()
        {
           
            if(member_premium_by_mode.val()=='')
            {
                member_premium_by_mode.val('0');
            }
            if (member_extra_premium.val()=='')
            {
                member_extra_premium.val('0');
            }
            if(member_discount.val()=='')
            {
                member_discount.val('0');
            }
            var total_premium = parseFloat(member_premium_by_mode.val()) + parseFloat(member_extra_premium.val());
            member_total_premium_after_dis.val(total_premium - member_discount.val());
        }
    </script>
    <script id="jTemplateCustomer" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Full Name</th>
                    <th style="border-width: thin; border-style: solid;">ID Card</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid;">Birth Date</th>
                    <th style="border-width: thin; border-style: solid; display: none;">Customer ID</th>
                    <th style="border-width: thin; border-style: solid; text-align: center; vertical-align: middle;">Select</th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td style="vertical-align: middle;">{ $T.row$index + 1 }</td>
                        <td style="vertical-align: middle;">{ $T.row.Last_Name + ' ' + $T.row.First_Name }</td>
                        <td style="vertical-align: middle;">{ $T.row.ID_Card}</td>
                        {#if $T.row.Gender==0}
                            <td style="vertical-align: middle;">Female</td>
                        {#elseif $T.row.Gender==1}
                            <td style="vertical-align: middle;">Male</td>
                        {#else} 
                            <td style="vertical-align: middle;">-</td>
                        {#/if}
                        <td style="vertical-align: middle;">{formatJSONDate($T.row.Birth_Date)}</td>
                        <td style="display: none;">{  $T.row.Customer_ID }</td>
                        <td style="text-align: center; vertical-align: middle;">
                            <input id="Ch{ $T.row$index }" type="checkbox" onclick="get_customer_info('{ $T.row.Customer_ID }','{ $T.row.Last_Name + ' ' + $T.row.First_Name }', '{ $T.row.Customer_Type }', '{ $T.row.Birth_Date }', '{ $T.row.Gender }', '{ $T.row$index }', '{ $T.row$total }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
    <h1>Policy Details</h1>
    <table id="tblContent" class="table-layout" style="background-color: #f6f6f6; padding: 40px;">
        <tr>
            <td class="td10 tdpading10">
                <asp:Label ID="lblPolicyNumber" Text="Policy Number" runat="server"></asp:Label>
            </td>
            <td class="td10 tdpading10">
                <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hdfPolicyID" runat="server" Value="" />
                <span class="required">*</span>
            </td>

            <td class="td10 tdpading10">
                <asp:Label ID="lblCustomerName" Text="Customer Name" runat="server"></asp:Label>
            </td>
            <td class="td10 tdpading10">
                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hdfCustomerID" runat="server" />

                <span class="required">*</span>
                <span class="icon-search" onclick="search_form(1);"></span>

            </td>
        </tr>
        <tr>
            <td class="td10">
                <asp:Label ID="lblAppNumber" Text="Application Number" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtAppNumber" runat="server"></asp:TextBox>
            </td>
            <td class="td10">
                <asp:Label ID="lblAppDate" Text="Application Date" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtAppDate" runat="server" CssClass="datepicker"></asp:TextBox>
                <span class="icon-calendar"></span>
                <span class="required">*</span>
            </td>
        </tr>
        <tr>
            <td class="td10">
                <asp:Label ID="lblProduct" Text="Product" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:DropDownList ID="ddlProduct" runat="server" AppendDataBoundItems="true" DataSourceID="ds_product" DataTextField="en_title" DataValueField="product_id">
                    <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                </asp:DropDownList>
                <span class="required">*</span>
            </td>
            <td class="td10">
                <asp:Label ID="lblCustomerType" Text="Customer Type" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:DropDownList ID="ddlCustomerType" runat="server" DataSourceID="ds_customer_type" DataValueField="customer_type_id" DataTextField="customer_type" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hdfCustomerType" runat="server" />
                <span class="required">*</span>
            </td>
        </tr>
        <tr>
            <td class="td10">
                <asp:Label ID="lblTermOfInsurance" runat="server" Text="Term Of Insurance"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtTermOfInsurance" runat="server"></asp:TextBox>
                <span class="required">*</span>
            </td>
            <td class="td10">
                <asp:Label ID="lblPaymentPeriod" runat="server" Text="Payment Period"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPaymentPeriod" runat="server"></asp:TextBox>
                <span class="required">*</span>
            </td>
        </tr>
        <tr>
            <td class="td10">
                <asp:Label ID="lblPolicySI" runat="server" Text="Sum Insured"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicySI" runat="server" onkeyup="ValidateTextDecimal(this)"></asp:TextBox>
                USD
                <span class="required">*</span>
            </td>
            <td class="td10">
                <asp:Label ID="lblPayMode" runat="server" Text="Pay Mode"></asp:Label>
            </td>
            <td class="td10">
                <asp:DropDownList ID="ddlPayMode" runat="server" DataSourceID="ds_paymode" DataTextField="mode" DataValueField="Pay_Mode_ID" AppendDataBoundItems="true">
                    <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                </asp:DropDownList>
                <span class="required">*</span>
            </td>
        </tr>
        <tr>
            <td class="td10">
                <asp:Label ID="lblPolicyPremium" runat="server" Text="Premium"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyPremium" runat="server"></asp:TextBox>
                USD
                <span class="required">*</span>
            </td>
            <td class="td10">
                <asp:Label ID="lblPolicyAnnualOriginPremium" runat="server" Text="Annual Premium"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyAnnualOriginPremium" runat="server"></asp:TextBox>
                USD
                <span class="required">*</span>
            </td>

        </tr>
        <tr>

            <td class="td10">
                <asp:Label ID="lblPolicyAnnualPremium" runat="server" Text="Annual Premium Rounded"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyAnnualPremium" runat="server"></asp:TextBox>
                USD
                <span class="required">*</span>
            </td>
            <td class="td10">
                <asp:Label ID="lblExtraPremium" runat="server" Text="Extra Prmium"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtExtraPremium" runat="server"></asp:TextBox>
                USD
            </td>
        </tr>
        <tr>

            <td class="td10">
                <asp:Label ID="lblExtraAnnualPremium" runat="server" Text="Extra Annual Prmium"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtExtraAnnualPremium" runat="server"></asp:TextBox>
                USD
            </td>
            <td class="td10">
                <asp:Label ID="lblPolicyDis" runat="server" Text="Discount"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyDis" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>

            <td class="td10">
                <asp:Label ID="lblPolicyTotalPremiumAfterDis" runat="server" Text="Total Premium After Discount"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyTotalPremiumAfterDis" runat="server"></asp:TextBox>
            </td>
            <td class="td10">
                <asp:Label ID="lblEffectiveDate" Text="Effective Date" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="datepicker"></asp:TextBox>
                <span class="icon-calendar"></span>
                <span class="required">*</span>
            </td>
        </tr>
        <tr>

            <td class="td10">
                <asp:Label ID="lblPolicyActualPremium" runat="server" Text="Actual Prmium"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyActualPremium" runat="server"></asp:TextBox>
                USD
            </td>
            <td class="td10">
                <asp:Label ID="lblPolicyReturnPremium" Text="Return Premium" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyReturnPremium" runat="server"></asp:TextBox>
                USD
            </td>
        </tr>
        <tr>

            <td class="td10">
                <asp:Label ID="lblPolicyPaymentCode" Text="Payment Code" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtPolicyPaymentCode" runat="server"></asp:TextBox>
            </td>



            <td class="td10">
                <asp:Label ID="lblApprovedDate" Text="Approved Date" runat="server"></asp:Label>
            </td>
            <td class="td10">
                <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="datepicker"></asp:TextBox>
                <span class="icon-calendar"></span>
            </td>
        </tr>

        <tr>
            <td colspan="4">
                <%--Tab--%>
                <div>
                    <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                        <li class="active"><a href="#policy_owner" data-toggle="tab">Policy Owner Info.</a></li>
                        <li id="tab_contact"><a href="#member" data-toggle="tab">Members</a></li>
                        <li><a href="#cover" data-toggle="tab">Additional Cover</a></li>
                        <li><a href="#riders" data-toggle="tab">Riders</a></li>
                        <li><a href="#beneficiary" data-toggle="tab">Beneficiary</a></li>
                    </ul>
                    <div class="container">
                        <div class="tab-content">
                            <div id="policy_owner" class="tab-pane active" style="padding-left: 10px;">
                                <table>
                                    <tr>
                                        <td class="td10">
                                            <asp:Label ID="lblPolicyOwnerName" runat="server" Text="Policy Owner"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolicyOwnerName" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td class="td10">
                                            <asp:Label ID="lblPolicyOwnerBirthDate" runat="server" Text="Birth Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolicyOwnerBirthDate" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td class="td10">
                                            <asp:Label ID="lblPolicyOwerGender" runat="server" Text="Gender"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPolicyOwerGender" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%-- Tab member --%>
                            <div id="member" class="tab-pane" style="padding-left: 10px; padding-right: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMemberCustomerName" runat="server" Text="Member"></asp:Label>
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtMemberCustomer" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdfMemberCustomerID" runat="server" />
                                            <span class="icon-search" onclick="search_form(2);"></span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberBirthDate" runat="server" Text="Birth Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberBirthDate" runat="server" Enabled="false" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberGender" runat="server" Text="Gender"></asp:Label>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberGender" runat="server" Enabled="false" ReadOnly="true"></asp:TextBox>
                                        </td>

                                        <td rowspan="6" style="width: 2%; vertical-align: bottom; text-align: center; padding-right: 5px;">
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="Button3" class="btn btn-small btn-primary" onclick="clear_member_text_box();">Clear</button><br />
                                            <br />
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="btn_add" class="btn btn-small btn-primary" onclick="add_member();"><span class="icon-plus"></span></button>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMemberAge" runat="server" Text="Age"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberAge" runat="server"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberEffectiveDate" runat="server" Text="Effective Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberEffectiveDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                            <span class="icon-calendar"></span>
                                            <span class="required">*</span>
                                        </td>

                                        <td>
                                            <asp:Label ID="lblMemberSI" runat="server" Text="Sum Insured"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberSI" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMemberPremiumByMode" runat="server" Text="Premium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberPremiumByMode" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberAnnuallyOriginPremium" runat="server" Text="Annual Premium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberAnnuallyOriginPremium" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberAnnuallyPremium" runat="server" Text="Annual Premium Rounded"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberAnnuallyPremium" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMemberExtraPercentage" runat="server" Text="Extra Percentage"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberExtraPercentage" runat="server"></asp:TextBox>
                                            %
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberExtraRate" runat="server" Text="Extra Rate"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberExtraRate" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberExtraPremium" runat="server" Text="Extra Premium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberExtraPremium" runat="server"></asp:TextBox>
                                            USD
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblMemberExtraAnnualPremium" runat="server" Text="Annual Ex.Premium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberExtraAnnualPremium" runat="server"></asp:TextBox>
                                            USD
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberDis" runat="server" Text="Discount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberDis" runat="server"></asp:TextBox>

                                        </td>
                                        <td>
                                            <asp:Label ID="lblMemberTotalPremiumAfterDis" runat="server" Text="Total Premium After Dis."></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMemberTotalPremiumAfterDis" runat="server"></asp:TextBox>
                                            USD
                                        </td>
                                    </tr>
                                    <tr>


                                        <td>
                                            <asp:Label ID="lblMemberRemarks" runat="server" Text="Remarks"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtMemberRemarks" runat="server" Width="94.5%"></asp:TextBox>
                                        </td>

                                    </tr>
                                </table>
                                <div id="div_member_list">
                                    <table id="tbl_member_list" class="table table-bordered">
                                        <tr style="background-color: lightgray;">
                                            <td style="width: 10px; vertical-align: middle; display: none;">No.</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle;">Customer</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle;">Age</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle;">Gender</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle;">Birth Date</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; display: none;">Effective Date</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle;">Sum Insured</td>
                                            <td style="padding: 5px; height: 30px; width: 40px; vertical-align: middle; display: none;">Pay Mode</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle;">Premium By Mode</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle;">Annual Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; display: none;">Annual Original Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle;">Discount</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; display: none;">Extra Percentage</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; display: none;">Extra Rate</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle;">Extra Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; display: none;">Extra Annual Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle;">Total Premium After Dis.</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle;">Remarks</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle;"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- Tab cover --%>
                            <div id="cover" class="tab-pane" style="padding-left: 10px; padding-right: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCoverType" runat="server" Text="Cover Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCoverType" runat="server" DataSourceID="ds_cover" DataValueField="cover_id" DataTextField="cover" AppendDataBoundItems="true">
                                                <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                            <asp:HiddenField ID="hdfCoverId" runat="server" Value="" />
                                        </td>

                                        <td>
                                            <asp:Label ID="lblCoverSI" runat="server" Text="Sum Insured"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverSI" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCoverEffectiveDate" runat="server" Text="Effective Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverEffectiveDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                            <span class="icon-calendar"></span>
                                            <span class="required">*</span>
                                        </td>

                                        <td rowspan="3" style="width: 2%; vertical-align: bottom; text-align: center; padding-right: 5px;">
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="btnClearCover" class="btn btn-small btn-primary" onclick="clear_cover_text_boxes();">Clear</button><br />
                                            <br />
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="btnAddCover" class="btn btn-small btn-primary" onclick="add_cover();"><span class="icon-plus"></span></button>

                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblCoverPremiumByMode" runat="server" Text="Premium By Pay Mode"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverPremiumByMode" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCoverAnnualOriginPremium" runat="server" Text="Annual Premium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverAnnualOriginPremium" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCoverAnnualPremium" runat="server" Text="Annual Premium Rounded"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverAnnualPremium" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>


                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCoverRemarks" runat="server" Text="Remarks"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtCoverRemarks" runat="server" Width="94.5%"></asp:TextBox>
                                        </td>


                                    </tr>

                                </table>
                                <div id="cover_list">
                                    <table id="tbl_cover_list" class="table table-bordered">
                                        <tr style="background-color: lightgray;">
                                            <td style="width: 10px; vertical-align: middle;">No.</td>
                                            <td style="width: 10px; vertical-align: middle; display: none;">Cover ID</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle;">Cover Type</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">Cover Type ID</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle;">Sum Insured</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle;">Effective Date</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle;">Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle;">Annual Original Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle;">Annual Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle;">Remarks</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle;"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- Tab rider --%>
                            <div id="riders" class="tab-pane" style="padding-left: 10px; padding-right: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRiderKhmerFirstName" runat="server" Text="Khmer First Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderKhmerFirstName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderKhmerLastName" runat="server" Text="Khmer Last Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderKhmerLastName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderSI" Text="Sum Insured" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderSI" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                        <td rowspan="7" style="width: 2%; vertical-align: bottom; text-align: center; padding-right: 5px;">
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="Button1" class="btn btn-small btn-primary" onclick="clear_rider_boxes();">Clear</button><br />
                                            <br />
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="Button4" class="btn btn-small btn-primary" onclick="add_rider();"><span class="icon-plus"></span></button>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRiderFirstName" runat="server" Text="First Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderFirstName" runat="server"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderLastName" runat="server" Text="Last Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderLastName" runat="server"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderPremiumByMode" Text="Premium By Pay Mode" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderPremiumByMode" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRiderGender" Text="Gender" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRiderGender" runat="server">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Female" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderBirthDate" Text="Birth Date" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderBirthDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderAnnualOriginPremium" Text="Annual Premium" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderAnnualOriginPremium" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRiderAge" Text="Age" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderAge" runat="server"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderIDType" Text="ID Type" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRiderIDType" runat="server">
                                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                                <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                                <asp:ListItem Value="2">Visa</asp:ListItem>
                                                <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderAnnualPremium" runat="server" Text="Annual Premium Rounded"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderAnnualPremium" runat="server"></asp:TextBox>
                                            USD
                                            <span class="required">*</span>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblRiderIDCard" Text="ID Card" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderIDCard" runat="server"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderNationality" Text="Nationality" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRiderNationality" runat="server" DataSourceID="ds_country" DataTextField="nationality" DataValueField="country_id" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>

                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderExtraPremium" Text="Extra Premium" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderExtraPremium" runat="server"></asp:TextBox>
                                            USD
                                            
                                        </td>

                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblRiderType" Text="Rider Type" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRiderType" runat="server">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Spouse" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Kid 1" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Kid 2" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Kid 3" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Kid 4" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderRelationship" Text="Relationship" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRiderRelationship" runat="server">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="DAUGHTER" Value="DAUGHTER"></asp:ListItem>
                                                <asp:ListItem Text="HUSBAND" Value="HUSBAND"></asp:ListItem>
                                                <asp:ListItem Text="SON" Value="SON"></asp:ListItem>
                                                <asp:ListItem Text="WIFE" Value="WIFE"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderExtraAnnualPremium" runat="server" Text="Extra Annual Premium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderExtraAnnualPremium" runat="server"></asp:TextBox>
                                            USD
                                           
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblRiderEffectiveDate" Text="Effective Date" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRiderEffectiveDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                            <span class="icon-calendar"></span>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRiderRemarks" Text="Remarks" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRiderRemarks" runat="server" Width="89%"></asp:TextBox>
                                        </td>

                                    </tr>

                                </table>
                                <div id="rider_list">
                                    <table id="tbl_rider_list" class="table table-bordered">
                                        <tr style="background-color: lightgray;">
                                            <td style="width: 10px; vertical-align: middle; display:none;">No.</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">Rider ID</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center;">Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">English Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">Kh First Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">Kh Last Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">En First Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">En Last Name</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align:center;">Gender</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align:center;">Birth Date</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align:center;">Age</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align:center;">ID Type</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center;">ID Card</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">Nationality</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align:center;">Rider</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display:none;">Relationship</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align:center;">Effective Date</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align:center;">Sum Insured</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align:center;">Premium By Mode</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; display:none;">Annual Origin Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align:center;">Annual Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align:center;">Ex.Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align:center;">Ex.Annual Premium</td>
                                            <td style="padding: 5px; height: 30px; width: 20px; vertical-align: middle; text-align:center; display:none;">Remarks</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle;"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- Tab beneficiary --%>
                            <div id="beneficiary" class="tab-pane" style="padding-left: 10px; padding-right: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBenKhmerFirstName" runat="server" Text="Khmer First Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenKhmerFirstName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenKhmerLastName" runat="server" Text="Khmer Last Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenKhmerLastName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenNationlity" Text="Nationality" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBenNationlity" runat="server" DataSourceID="ds_country" DataTextField="nationality" DataValueField="country_id" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>

                                        <td rowspan="5" style="width: 2%; vertical-align: bottom; text-align: center; padding-right: 5px;">
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="Button2" class="btn btn-small btn-primary" onclick="clear_beneficiary_boxes();">Clear</button><br />
                                            <br />
                                            <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="Button5" class="btn btn-small btn-primary" onclick="add_beneficiary();"><span class="icon-plus"></span></button>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBenFirstName" runat="server" Text="First Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenFirstName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenLastName" runat="server" Text="Last Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenLastName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenRelationship" Text="Relationship" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBenRelationship" runat="server" DataSourceID="ds_relationship" DataTextField="relationship" DataValueField="relationship" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBenGender" Text="Gender" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBenGender" runat="server">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Female" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenBirthDate" Text="Birth Date" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenBirthDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                            <span class="icon-calendar"></span>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenPercentage" Text="Percentage" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenPercentage" runat="server"></asp:TextBox>
                                            <span class="required">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBenIDType" Text="ID Type" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBenIDType" runat="server">
                                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                                <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                                <asp:ListItem Value="2">Visa</asp:ListItem>
                                                <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="required">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBenIDCard" Text="ID Card" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBenIDCard" runat="server"></asp:TextBox>
                                            <span class="required">*</span>

                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBenRemarks" Text="Remarks" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtBenRemarks" runat="server" Width="94%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div id="beneficiary_list">
                                    <table id="tbl_beneficiary_list" class="table table-bordered">
                                        <tr style="background-color: lightgray;">
                                            <td style="width: 50px; vertical-align: middle; text-align:center;">No.</td>
                                            <td style="width: 50px; vertical-align: middle; display:none;">Beneficiary ID</td>
                                            <td style="padding: 5px; height: 30px; width: 200px; vertical-align: middle; text-align:center;">Full Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center; display:none;">Khmer First Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center; display:none;">Khmer Last Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center; display:none;">English First Name</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center; display:none;">English Last Name</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align:center;">Gender</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align:center;">Birth Date</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align:center;">ID Type</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center;">ID Card</td>
                                            <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align:center;">Nationality</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center;">Relationship</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center;">Percentage</td>
                                            <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align:center;">Remarks</td>
                                            <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align:center;"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <%-- End Tab --%>
            </td>
        </tr>
    </table>

    <%--Modal Search Customer--%>
    <div id="modal_search_customer" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="SearchCustomer" aria-hidden="true" data-keyboard="false" data-backdrop="static">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H2">Search Customer</h3>
        </div>
        <div class="modal-body ">
            <ul id="Ul1" class="nav nav-tabs" data-tabs="tabs">
                <li class="active"><a href="#search_customer_by_customer_number" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Customer Number</a></li>
                <li><a href="#search_customer_by_customer_name" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Customer Info.</a></li>
            </ul>
            <div id="search_customer_tabcontent" class="tab-content" style="height: 70px; overflow: hidden;">

                <div id="search_customer_by_customer_number" class="tab-pane active">
                    <table style="width: 70%;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblSearchCustNo" Text="Customer Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchCustNo"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="search_customer_by_customer_name" class="tab-pane">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblSearchCustType" Text="Customer Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSearchCustType" DataSourceID="ds_customer_type" DataTextField="customer_type" DataValueField="customer_type_id" AppendDataBoundItems="true">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSearchCustName" Text="Customer Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchCustName"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblSearchIDCard" Text="ID Card"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchIDCard"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="dvCustomerSearchList"></div>
        </div>
        <div class="modal-footer">
            <input type="button" id="btnSearchCustomer" value="Search" onclick="search_customer();" class="btn btn-primary" />
        </div>
    </div>
    <%--End Modal Search Customer--%>
    <asp:SqlDataSource ID="ds_Customer_Type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Customer_Type_ID, Customer_Type  FROM Ct_Customer_Type ORDER BY Customer_Type"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_cover" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT COVER_ID, COVER FROM TBL_COVER ORDER BY COVER ASC;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_paymode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Pay_Mode_ID, Mode FROM Ct_Payment_Mode ORDER BY MODE;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_product" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_ID, En_Title FROM Ct_Product WHERE En_Title <>'' ORDER BY En_Title;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_country" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM Ct_Country ORDER BY Nationality;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_relationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select relationship from ct_relationship  order by Relationship;"></asp:SqlDataSource>
</asp:Content>

