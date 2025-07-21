using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_commission_report
/// </summary>
public class da_commission_report
{
	public da_commission_report()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static List<bl_commission_report> GetPolicyDetailForReport(DateTime from_date, DateTime to_date)
    {
        List<bl_commission_report> commission_list = new List<bl_commission_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = @"select distinct Receipt_No, * from (
						select * from (
						select Policy_Number,Receipt_No,Ct_Official_Receipt.Created_On,Policy_Type , Ct_Policy_Number.Policy_ID

						from Ct_Official_Receipt 
						inner join Ct_Official_Receipt_Prem_Pay on Ct_Official_Receipt_Prem_Pay.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
						inner join Ct_Policy_Number on Ct_Official_Receipt.Policy_ID=Ct_Policy_Number.Policy_ID

						union all

						select Policy_Number,Receipt_No,Ct_Official_Receipt.Created_On,Policy_Type,Ct_GTLI_Policy.GTLI_Policy_ID as Policy_ID

						from Ct_Official_Receipt 
						inner join Ct_Official_Receipt_Prem_Pay_GTLI on Ct_Official_Receipt_Prem_Pay_GTLI.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
						left join Ct_GTLI_Policy on Ct_Official_Receipt.Policy_ID=Ct_GTLI_Policy.GTLI_Policy_ID

						union all

						select Policy_Number,Receipt_No,Ct_Official_Receipt.Created_On,Policy_Type ,Ct_Policy_Number.Policy_ID
						from Ct_Official_Receipt 
						inner join Ct_Official_Receipt_Prem_Pay_Micro on Ct_Official_Receipt_Prem_Pay_Micro.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
						inner join Ct_Policy_Number on Ct_Official_Receipt.Policy_ID=Ct_Policy_Number.Policy_ID

						union all

						select Policy_Number,Receipt_No,Ct_Official_Receipt.Created_On,Policy_Type ,Ct_Policy_Number.Policy_ID
						from Ct_Official_Receipt 
						inner join Ct_Official_Receipt_Miscellaneous on Ct_Official_Receipt_Miscellaneous.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
						inner join Ct_Policy_Number on Ct_Official_Receipt.Policy_ID=Ct_Policy_Number.Policy_ID
                        
                        UNION ALL
						select Policy_Number,Receipt_No,Ct_Official_Receipt.Created_On,Policy_Type ,CT_CI_POLICY.Policy_ID
						from Ct_Official_Receipt 
						inner join Ct_Official_Receipt_Prem_Pay on Ct_Official_Receipt_Prem_Pay.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
						inner join CT_CI_POLICY on Ct_Official_Receipt.Policy_ID=CT_CI_POLICY.Policy_ID

						) #T 

		where (Created_On between @From_Date and @To_Date)   and Policy_Type not in (5)  
		 
		)#T
        ORDER BY Policy_Number, Created_On asc ";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_commission_report commission_report = new bl_commission_report();
                        commission_report.Policy_ID = myReader.GetString(myReader.GetOrdinal("Policy_ID"));
                        commission_report.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        commission_report.Receipt_No = myReader.GetString(myReader.GetOrdinal("Receipt_No"));
                        commission_report.Policy_Type = myReader.GetInt32(myReader.GetOrdinal("Policy_Type"));
                        commission_report.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));

                        commission_list.Add(commission_report);

                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicyDetailForReport] in class [da_commission_report]. Details: " + ex.Message);

        }

        return commission_list;
    }

    public static int CountNumPayment(string receipt_no, DateTime from_date, DateTime to_date)
    {
        int count_payment = 1;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select count(Ct_Official_Receipt_Prem_Pay.Official_Receipt_ID) from Ct_Official_Receipt_Prem_Pay 
                            inner join Ct_Official_Receipt  on Ct_Official_Receipt_Prem_Pay.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
                             where Receipt_No=@Receipt_No and  (Ct_Official_Receipt.Created_On between @From_Date and @To_Date) ";

                cmd.Parameters.AddWithValue("@Receipt_No", receipt_no);
                cmd.Parameters.AddWithValue("@From_Date", from_date);
                cmd.Parameters.AddWithValue("@To_Date", to_date);

                myConnection.Open();
                count_payment = int.Parse(cmd.ExecuteScalar().ToString());
                myConnection.Close();
            }
        }
        catch (Exception)
        {
            count_payment = 1;
        }

        return count_payment;
    }

    public static DataTable GetPolcyPremInfo(string policy_number, int policy_type)
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;

                if (policy_type == 1)
                {
                    cmd.CommandText = @"select case when  Premium is null then 0 else Premium end as Premium,
                             case when Discount_Amount is null then 0 else Discount_Amount end as Discount_Amount,
                             Issue_Date,En_Title,case when Pay_Year is null then 0 else Pay_Year end as Pay_Year,Pay_Mode,Original_Amount,Factor
                             from Ct_Policy_Premium 
                             inner join Ct_Policy on Ct_Policy.Policy_ID=Ct_Policy_Premium.Policy_ID
                             inner join Ct_Policy_Number on Ct_Policy.Policy_ID=Ct_Policy_Number.Policy_ID
                             inner join Ct_Policy_Pay_Mode on Ct_Policy.Policy_ID=Ct_Policy_Pay_Mode.Policy_ID
                             inner join Ct_Payment_Mode on Ct_Policy_Pay_Mode.Pay_Mode=Ct_Payment_Mode.Pay_Mode_ID
                             inner join Ct_Product on Ct_Policy.Product_ID=Ct_Product.Product_ID
                             where Policy_Number=@Policy_Number 

                             UNION ALL 

							 SELECT CASE WHEN Premium IS NULL THEN 0 ELSE Premium END AS PREMIUM,
							 CASE WHEN Discount_Amount IS NULL THEN 0 ELSE Discount_Amount END AS Discount_Amount,
							 Issued_Date AS Issue_Date, Product_Id AS En_Title, CASE WHEN PAY_YEAR IS NULL THEN 0 ELSE PAY_YEAR END AS Pay_Year, 
							 pay_mode, ORIGINAL_PREMIUM AS Original_Amount, Factor FROM ct_ci_policy
							 inner join ct_ci_policy_detail ON ct_ci_policy_detail.POLICY_ID = ct_ci_policy.POLICY_ID
							 inner join Ct_Policy_Pay_Mode ON Ct_Policy_Pay_Mode.Policy_ID = ct_ci_policy.policy_id
							 inner join Ct_Payment_Mode ON Ct_Policy_Pay_Mode.Pay_Mode=Ct_Payment_Mode.Pay_Mode_ID

                             WHERE Policy_Number=@Policy_Number ";
                }
                else if (policy_type == 3)
                {
                    cmd.CommandText = @"select case when  Premium is null then 0 else Premium end as Premium,
                         case when Discount_Amount is null then 0 else Discount_Amount end as Discount_Amount,
                         Issue_Date,En_Title, case when Pay_Year is null then 0 else Pay_Year end as Pay_Year from Ct_Policy_Micro inner join Ct_Policy_Micro_Premium 
                         on Ct_Policy_Micro.Policy_Micro_ID=Ct_Policy_Micro_Premium.Policy_Micro_ID 
                         inner join Ct_Policy_Number on Ct_Policy_Number.Policy_ID=Ct_Policy_Micro.Policy_Micro_ID
                         inner join Ct_Product on Ct_Policy_Micro.Product_ID=Ct_Product.Product_ID
                         where Policy_Number=@Policy_Number ";
                }
                else if (policy_type == 4)
                {
                    cmd.CommandText = @"select case when  Premium is null then 0 else Premium end as Premium,
                         case when Discount_Amount is null then 0 else Discount_Amount end as Discount_Amount,
                         Issue_Date,En_Title from Ct_Flexi_Term_Policy_Premium inner join 
                         Ct_Flexi_Term_Policy on Ct_Flexi_Term_Policy_Premium.Flexi_Term_Policy_ID=Ct_Flexi_Term_Policy.Flexi_Term_Policy_ID
                         inner join  Ct_Policy_Number on Ct_Policy_Number.Policy_ID=Ct_Flexi_Term_Policy.Flexi_Term_Policy_ID
                         inner join Ct_Product on Ct_Flexi_Term_Policy.Product_ID=Ct_Product.Product_ID
                        where Policy_Number=@Policy_Number ";
                }

                cmd.Parameters.AddWithValue("@Policy_Number", policy_number);

                myConnection.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                dap.Fill(dt);
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error: class [da_search_report], function [GetPolcyPremInfo]. Details: " + ex.Message);
            return dt;
        }
        
        return dt;
    }

    public static DataTable GetSIMode(string receipt_no, DateTime from_date, DateTime to_date, int policy_type)
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;

                if (policy_type == 1)
                {
                    cmd.CommandText = @"select Ct_Policy_Prem_Pay.Pay_Mode_ID,Sum_Insure,Ct_Policy_Prem_Pay.Amount,Ct_Policy_Prem_Pay.Prem_EM_Amount,Ct_Policy_Prem_Pay.Discount_Amount,Mode,Prem_Year,Prem_Lot,(Ct_Policy_Prem_Pay.Amount - Ct_Policy_Prem_Pay.Prem_EM_Amount) as Std_Prem from Ct_Policy_Prem_Pay
                                 inner join Ct_Official_Receipt_Prem_Pay on Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID=Ct_Official_Receipt_Prem_Pay.Policy_Prem_Pay_ID
                                 inner join Ct_Official_Receipt on Ct_Official_Receipt_Prem_Pay.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
                                 inner join Ct_Payment_Mode on Ct_Policy_Prem_Pay.Pay_Mode_ID=Ct_Payment_Mode.Pay_Mode_ID
                                 where Receipt_No=@Receipt_No and  (Ct_Official_Receipt.Created_On between @From_Date and @To_Date)";
                }
                else if (policy_type == 3)
                {
                    cmd.CommandText = @"select  Sum_Insured,Full_Name,   Case When Gender=0 then 'MS.' + Last_Name + ' ' +First_Name else
                    'MR.' + Last_Name + ' ' +First_Name end 'Customer',Ct_Policy_Micro_Prem_Pay.Sale_Agent_ID  from Ct_Policy_Micro_Prem_Pay
                    inner join Ct_Official_Receipt_Prem_Pay_Micro on Ct_Policy_Micro_Prem_Pay.Policy_Micro_Prem_Pay_ID=Ct_Official_Receipt_Prem_Pay_Micro.Policy_Micro_Prem_Pay_ID
                    inner join Ct_Official_Receipt on Ct_Official_Receipt_Prem_Pay_Micro.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
                    inner join Ct_Sale_Agent on Ct_Policy_Micro_Prem_Pay.Sale_Agent_ID=Ct_Sale_Agent.Sale_Agent_ID
                    inner join Ct_Policy_Micro on Ct_Policy_Micro.Policy_Micro_ID=Ct_Official_Receipt.Policy_ID
                    inner join Ct_Micro_Customer on Ct_Micro_Customer.Customer_Micro_ID=Ct_Policy_Micro.Customer_Micro_ID
  
                    where Receipt_No=@Receipt_No and  (Ct_Official_Receipt.Created_On between @From_Date and @To_Date)";
                }
                else if (policy_type == 4)
                {
                    cmd.CommandText = @"select  Sum_Insured,Full_Name,   Case When Gender=0 then 'MS.' + Last_Name + ' ' +First_Name else
                    'MR.' + Last_Name + ' ' +First_Name end 'Customer',Ct_Flexi_Term_Policy_Prem_Pay.Sale_Agent_ID  from Ct_Flexi_Term_Policy_Prem_Pay
                    inner join Ct_Official_Receipt_Prem_Pay_Micro on Ct_Flexi_Term_Policy_Prem_Pay.Flexi_Term_Policy_Prem_Pay_ID=Ct_Official_Receipt_Prem_Pay_Micro.Policy_Micro_Prem_Pay_ID
                    inner join Ct_Official_Receipt on Ct_Official_Receipt_Prem_Pay_Micro.Official_Receipt_ID=Ct_Official_Receipt.Official_Receipt_ID
                    inner join Ct_Sale_Agent on Ct_Flexi_Term_Policy_Prem_Pay.Sale_Agent_ID=Ct_Sale_Agent.Sale_Agent_ID
                    inner join Ct_Flexi_Term_Policy on Ct_Flexi_Term_Policy.Flexi_Term_Policy_ID=Ct_Official_Receipt.Policy_ID
                    inner join Ct_Customer_Flexi_Term on Ct_Customer_Flexi_Term.Customer_Flexi_Term_ID=Ct_Flexi_Term_Policy.Customer_Flexi_Term_ID

                    where Receipt_No=@Receipt_No and  (Ct_Official_Receipt.Created_On between @From_Date and @To_Date)
                    ";
                }

                cmd.Parameters.AddWithValue("@Receipt_No", receipt_no);
                cmd.Parameters.AddWithValue("@From_Date", from_date);
                cmd.Parameters.AddWithValue("@To_Date", to_date);

                myConnection.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                dap.Fill(dt);
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error: class [da_commission_report], function [GetSIMode]. Details: " + ex.Message);
            return dt;
        }

        return dt;
    }

    public static DataTable GetAppInfo(string policy_id)
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @" select App_Number,App_Date, case when Gender=0 then 'Ms.' + Last_Name + ' '+ First_Name
                             else 'Mr.' + Last_Name + ' '+ First_Name end as Customer from Ct_App_Info 
                             inner join Ct_App_Policy on Ct_App_Info.App_Register_ID=Ct_App_Policy.App_Register_ID
                             inner join Ct_App_Register on Ct_App_Info.App_Register_ID=Ct_App_Register.App_Register_ID
                             inner join Ct_Policy on Ct_Policy.Policy_ID=Ct_App_Policy.Policy_ID
                             inner join Ct_Customer on Ct_Policy.Customer_ID=Ct_Customer.Customer_ID
                             where Ct_Policy.Policy_ID=@Policy_ID 

                             UNION ALL 

							 select '' as App_Number,'' App_Date, case when Gender=0 then 'Ms.' + Last_Name + ' '+ First_Name
                             else 'Mr.' + Last_Name + ' '+ First_Name end as Customer from Ct_Ci_Policy 
                             inner join Ct_Customer on Ct_Ci_Policy.Customer_ID=Ct_Customer.Customer_ID
							 where Ct_Ci_Policy.Policy_ID=@Policy_ID";

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                myConnection.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                dap.Fill(dt);
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error: class [da_commission_report], function [GetAppInfo]. Details: " + ex.Message);
            return dt;
        }

        return dt;
    }
    public static DataTable GetSaleAgent(string policy_number, DateTime pay_date)
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select full_Name,Ct_Sale_Agent.Sale_Agent_ID from Ct_Sale_Agent
                            inner join ct_policy_prem_pay on Ct_Sale_Agent.sale_agent_id=ct_policy_prem_pay.sale_agent_id
							inner join Ct_Policy_Number on ct_policy_prem_pay.Policy_ID=Ct_Policy_Number.Policy_ID
                            where Policy_Number=@policy_number  

                            UNION ALL 

							select full_Name,Ct_Sale_Agent.Sale_Agent_ID from Ct_Sale_Agent
                            inner join ct_policy_prem_pay on Ct_Sale_Agent.sale_agent_id=ct_policy_prem_pay.sale_agent_id
							INNER JOIN CT_CI_POLICY ON CT_CI_POLICY.POLICY_ID= ct_policy_prem_pay.Policy_ID
                            where Policy_Number=@policy_number";

                cmd.Parameters.AddWithValue("@policy_number", policy_number);

                myConnection.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                dap.Fill(dt);
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error: class [da_commission_report], function [GetSaleAgent]. Details: " + ex.Message);
            return dt;
        }

        return dt;
    }

    public static string GetChannelNameByPolicyNum(string policy_number, int policy_type)
    {
        string channel_name = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;

                if (policy_type == 1)
                {
                    cmd.CommandText = @"select case when Ct_App_Register.Created_Note is null then ''
                                 else  Ct_App_Register.Created_Note end   from Ct_App_Register 
                                 inner join Ct_App_Policy on Ct_App_Register.App_Register_ID=Ct_App_Policy.App_Register_ID
                                 inner join Ct_Policy_Number on Ct_Policy_Number.Policy_ID=Ct_App_Policy.Policy_ID
                             where Policy_Number =@Policy_Number ";
                }
                else if (policy_type == 3)
                {
                    cmd.CommandText = @"select Channel_Name from Ct_Policy_Number 
                        inner join Ct_Policy_Micro on Ct_Policy_Micro.Policy_Micro_ID=Ct_Policy_Number.Policy_ID
						inner join Ct_Channel_Location on Ct_Policy_Micro.Channel_Location_ID=Ct_Channel_Location.Channel_Location_ID
                        inner join Ct_Channel_Item on Ct_Channel_Location.Channel_Item_ID=Ct_Channel_Item.Channel_Item_ID
						where Policy_Number=@Policy_Number ";
                }
                else if (policy_type == 4)
                {
                    cmd.CommandText = @"select Channel_Name from Ct_Policy_Number 
                        inner join Ct_Flexi_Term_Policy on Ct_Flexi_Term_Policy.Flexi_Term_Policy_ID=Ct_Policy_Number.Policy_ID
						inner join Ct_Channel_Location on Ct_Flexi_Term_Policy.Channel_Location_ID=Ct_Channel_Location.Channel_Location_ID
                        inner join Ct_Channel_Item on Ct_Channel_Location.Channel_Item_ID=Ct_Channel_Item.Channel_Item_ID
						where  Policy_Number=@Policy_Number ";
                }  
                
                cmd.Parameters.AddWithValue("@Policy_Number", policy_number);

                myConnection.Open();
                channel_name = cmd.ExecuteScalar().ToString();
                myConnection.Close();
            }
        }
        catch (Exception)
        {
            channel_name = "";
        }

        return channel_name;
    }
}