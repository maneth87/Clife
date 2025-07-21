using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;
using System.Web.Services;
/// <summary>
/// Summary description for da_Policy_FP_Plan
/// </summary>
public class da_Policy_FP_Plan
{
	public da_Policy_FP_Plan()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //save fp policy plan
    public static bool saveFfPolicyPlan(bl_Policy_FP_Plan plan)
    {

        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_INSERT_POLICY_FP_PLAN";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;
            var value = plan;

            para.AddWithValue("@Plan_ID", value.Fp_plan_id);
            para.AddWithValue("@Plan_Name", value.Fp_Plan_name);
            para.AddWithValue("@Sum_Insured1", value.Sum_insured1);
            para.AddWithValue("@Sum_Insured2", value.Sum_insured2);
            para.AddWithValue("@Sum_Insured3", value.Sum_insured3);
            para.AddWithValue("@Sum_Insured4", value.Sum_insured4);
            para.AddWithValue("@Coverage_Peroid", value.Coverage_peroid);
            para.AddWithValue("@Payment_Peroid", value.Payment_peroid);
            para.AddWithValue("@Created_By", value.Created_by);
            para.AddWithValue("@Created_On", value.Created_on);
         
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [saveFpPolicyPlan] in class [da_fp_plan], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    //Get plan list
    
    [WebMethod]
    public static List<bl_Policy_FP_Plan> getPlanListByPlanID(string plan_id)
    { 
        List<bl_Policy_FP_Plan> myPlanList= new List<bl_Policy_FP_Plan>();
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();

        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_GET_FP_PLAN_BY_ID";
            myCommand.Parameters.AddWithValue("@Plan_ID", plan_id);
            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    bl_Policy_FP_Plan myPlan = new bl_Policy_FP_Plan();
                    myPlan.Fp_plan_id = myReader.GetString(myReader.GetOrdinal("plan_id"));
                    myPlan.Fp_Plan_name = myReader.GetString(myReader.GetOrdinal("plan_name"));
                    myPlan.Sum_insured1 =Convert.ToDouble(myReader.GetDecimal(myReader.GetOrdinal("sum_insured1")));
                    myPlan.Sum_insured2 = Convert.ToDouble(myReader.GetDecimal(myReader.GetOrdinal("sum_insured2")));
                    myPlan.Sum_insured3 = Convert.ToDouble(myReader.GetDecimal(myReader.GetOrdinal("sum_insured3")));
                    myPlan.Sum_insured4 = Convert.ToDouble(myReader.GetDecimal(myReader.GetOrdinal("sum_insured4")));
                    myPlan.Coverage_peroid = myReader.GetInt32(myReader.GetOrdinal("coverage_peroid"));
                    myPlan.Payment_peroid =myReader.GetInt32(myReader.GetOrdinal("payment_peroid"));
                    myPlan.Created_by = myReader.GetString(myReader.GetOrdinal("created_by"));
                    myPlan.Created_on = myReader.GetDateTime(myReader.GetOrdinal("created_on"));
                    myPlanList.Add(myPlan);
                }

            }
            myReader.Close();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            myPlanList = null;
            Log.AddExceptionToLog("Error function [getPlanListByPlanID] in class [da_Policy_FP_Plan], Detail: " + ex.Message);
        }

        return myPlanList;
    }
}