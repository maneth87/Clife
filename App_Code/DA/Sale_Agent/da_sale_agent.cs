using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Summary description for da_sale_agent
/// </summary>

using System.Configuration;
using System.Data;
using System.Reflection;

public class da_sale_agent
{
    private static da_sale_agent mytitle = null;
    private static string MYNAME = "da_sale_agent";
    public da_sale_agent()
    {
        if (mytitle == null)
        {
            mytitle = new da_sale_agent();
        }

    }

    //Function to get agent list by agent_name (Like)
    public static List<bl_sale_agent> GetAgentList(string Agent_Name)
    #region "Public Functions"
    {
        int row_index = 0;
        List<bl_sale_agent> sale_agent_list = new List<bl_sale_agent>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_AgentList_By_Agent_Name", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@AgentName";
            paramName.Value = Agent_Name;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    row_index += 1;
                    bl_sale_agent sale_agent = new bl_sale_agent();
                    sale_agent.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));
                    sale_agent.Sale_Agent_Type = rdr.GetInt32(rdr.GetOrdinal("Sale_Agent_Type"));
                    sale_agent.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                    sale_agent.Khmer_Full_Name = rdr["full_name_kh"].ToString();
                    sale_agent.Status = rdr.GetInt32(rdr.GetOrdinal("Status"));
                    sale_agent.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                    sale_agent.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                    sale_agent.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                    sale_agent.row_index = row_index;
                    sale_agent_list.Add(sale_agent);
                }
            }
        }

        return sale_agent_list;
    }

    /// <summary>
    /// Insert into Ct_Sale_Agent, Ct_Sale_Agent_Contact, Ct_Sale_Agent_Ordinary
    /// </summary>
    public static bool InsertSale_Agent(bl_sale_agent sale_agent, bl_sale_agent_contact sale_agent_contact, bl_sale_agent_ordinary sale_agent_ordinary)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Sale_Agent";

            /// Insert into Ct_Sale_Agent
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Sale_Agent_Type", sale_agent.Sale_Agent_Type);
            cmd.Parameters.AddWithValue("@Full_Name", sale_agent.Full_Name);
            cmd.Parameters.AddWithValue("@Full_Name_Kh", sale_agent.Khmer_Full_Name);
            cmd.Parameters.AddWithValue("@Status", sale_agent.Status);
            cmd.Parameters.AddWithValue("@Created_On", sale_agent.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", sale_agent.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", sale_agent.Created_Note);

            /// Ct_Sale_Agent_Contact
            cmd.Parameters.AddWithValue("@Mobile_Phone1", sale_agent_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", sale_agent_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", sale_agent_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", sale_agent_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", sale_agent_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", sale_agent_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", sale_agent_contact.Fax1);
            cmd.Parameters.AddWithValue("@Fax2", sale_agent_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", sale_agent_contact.EMail);

            /// Ct_Sale_Agent_Ordinary
            /// if (DateTime.Parse(sale_agent_ordinary.Birth_Date.ToString()).ToString("dd/MM/yyyy") == DateTime.Parse(DateTime.Parse("01/01/0001").ToString()).ToString("dd/MM/yyyy") && next_loop == 0)
            if (sale_agent.Sale_Agent_Type == 0)
            {
                cmd.Parameters.AddWithValue("@ID_Card", sale_agent_ordinary.ID_Card);
                cmd.Parameters.AddWithValue("@ID_Type", sale_agent_ordinary.ID_Type);
                cmd.Parameters.AddWithValue("@First_Name", sale_agent_ordinary.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", sale_agent_ordinary.Last_Name);
                cmd.Parameters.AddWithValue("@Khmer_First_Name", sale_agent_ordinary.Khmer_First_Name);
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", sale_agent_ordinary.Khmer_Last_Name);
                cmd.Parameters.AddWithValue("@Gender", sale_agent_ordinary.Gender);
                cmd.Parameters.AddWithValue("@Birth_Date", sale_agent_ordinary.Birth_Date);
                cmd.Parameters.AddWithValue("@Country_ID", sale_agent_ordinary.Country_ID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ID_Card", "");
                cmd.Parameters.AddWithValue("@ID_Type", 0);
                cmd.Parameters.AddWithValue("@First_Name", "");
                cmd.Parameters.AddWithValue("@Last_Name", "");
                cmd.Parameters.AddWithValue("@Khmer_First_Name", "");
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", "");
                cmd.Parameters.AddWithValue("@Gender", 0);
                cmd.Parameters.AddWithValue("@Birth_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Country_ID", "");
            }

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertSale_Agent] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update in Ct_Sale_Agent, Ct_Sale_Agent_Contact, Ct_Sale_Agent_Ordinary
    /// </summary>
    public static bool UpdateSale_Agent(bl_sale_agent sale_agent, bl_sale_agent_contact sale_agent_contact, bl_sale_agent_ordinary sale_agent_ordinary)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Sale_Agent";

            /// Insert into Ct_Sale_Agent
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Sale_Agent_Type", sale_agent.Sale_Agent_Type);
            cmd.Parameters.AddWithValue("@Full_Name", sale_agent.Full_Name);
            cmd.Parameters.AddWithValue("@Khmer_Full_Name", sale_agent.Khmer_Full_Name);
            cmd.Parameters.AddWithValue("@Status", sale_agent.Status);
            cmd.Parameters.AddWithValue("@Created_On", sale_agent.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", sale_agent.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", sale_agent.Created_Note);

            /// Ct_Sale_Agent_Contact
            cmd.Parameters.AddWithValue("@Mobile_Phone1", sale_agent_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", sale_agent_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", sale_agent_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", sale_agent_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", sale_agent_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", sale_agent_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", sale_agent_contact.Fax1);
            cmd.Parameters.AddWithValue("@Fax2", sale_agent_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", sale_agent_contact.EMail);

            /// Ct_Sale_Agent_Ordinary
            if (sale_agent.Sale_Agent_Type == 0)
            {
                cmd.Parameters.AddWithValue("@ID_Card", sale_agent_ordinary.ID_Card);
                cmd.Parameters.AddWithValue("@ID_Type", sale_agent_ordinary.ID_Type);
                cmd.Parameters.AddWithValue("@First_Name", sale_agent_ordinary.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", sale_agent_ordinary.Last_Name);
                cmd.Parameters.AddWithValue("@Khmer_First_Name", sale_agent_ordinary.Khmer_First_Name);
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", sale_agent_ordinary.Khmer_Last_Name);
                cmd.Parameters.AddWithValue("@Gender", sale_agent_ordinary.Gender);
                cmd.Parameters.AddWithValue("@Birth_Date", sale_agent_ordinary.Birth_Date);
                cmd.Parameters.AddWithValue("@Country_ID", sale_agent_ordinary.Country_ID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ID_Card", "");
                cmd.Parameters.AddWithValue("@ID_Type", 0);
                cmd.Parameters.AddWithValue("@First_Name", "");
                cmd.Parameters.AddWithValue("@Last_Name", "");
                cmd.Parameters.AddWithValue("@Khmer_First_Name", "");
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", "");
                cmd.Parameters.AddWithValue("@Gender", 0);
                cmd.Parameters.AddWithValue("@Birth_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Country_ID", "");
            }

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertSale_Agent] in class [bl_sale_agent]. Details: " + ex.Message);
                if (sale_agent.Sale_Agent_Type == 0)
                {
                    MessageBox.Show("The process of Add New Sale Ordinary Agent is unsuccessful. Please check it again.");
                }
                else if (sale_agent.Sale_Agent_Type == 0)
                {
                    MessageBox.Show("The process of Add New Sale Bank Agent is unsuccessful. Please check it again.");
                }
                else
                {
                    MessageBox.Show("The process of Add New Sale Broker Agent is unsuccessful. Please check it again.");
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Check Duplicate from Ct_Sale_Agent
    /// </summary>
    public static bool Check_Duplicate(bl_sale_agent sale_agent, bl_sale_agent_ordinary sale_agent_ordinary, int check_duplicate)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_Sale_Agent";
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Sale_Agent_Type", sale_agent.Sale_Agent_Type);

            if (sale_agent.Sale_Agent_Type == 0)
            { cmd.Parameters.AddWithValue("@ID_Card", sale_agent_ordinary.ID_Card); }

            else { cmd.Parameters.AddWithValue("@ID_Card", ""); }

            cmd.Parameters.AddWithValue("@check_duplicate_type", check_duplicate);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Check Sale ID that is being used from Ct_Sale_Agent
    /// </summary>
    public static bool GetSaleAgent_IsUsed_By_Sale_ID(string sale_agent_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Sale_Agent_IsUsed_By_Sale_Agent_ID";
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();
            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetSaleAgent_IsUsed_By_Sale_ID] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Delete from Ct_Sale_Agent
    /// </summary>
    public static bool DeleteSaleAgent_Record(bl_sale_agent sale_agent)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Sale_Agent_By_Sale_ID";
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent.Sale_Agent_ID);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [DeleteSaleAgent_Record] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return result;
    }


    /// <summary>
    /// Searcha Sale Agent By Sale Agent ID
    /// </summary>
    public static DataTable GetSaleAgent_By_SaleAgentCode(string sale_agent_code)
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Sale_Agent_Search_By_ID";
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_code);
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            cmd.Connection = con;
            con.Open();
            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetSaleAgent_By_SaleAgentCode] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return dt;
    }

    /// <summary>
    /// Search by Name
    /// </summary>

    public static DataTable GetSaleAgent_By_Name(string first_name, string last_name)
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT ROW_NUMBER() over(order by SA.Sale_Agent_ID) 'No',SA.Sale_Agent_ID, CASE WHEN Sale_Agent_Type IN (0) THEN 'Ordinary Agent' WHEN Sale_Agent_Type IN (1) 
                         THEN 'Bank Agent' ELSE 'Broker Agent' END AS Sale_Agent_Type_Name, SAO.ID_Card, SAO.ID_Type, SAO.First_Name, SAO.Last_Name, SAO.Country_ID, 
                         SAO.Gender, SAO.Birth_Date, SAC.Home_Phone1, SAC.Mobile_Phone1, SAC.Fax1, SAC.EMail, SA.Full_Name, SA.Created_On, SA.Created_By, 
                         SA.Created_Note, SA.Sale_Agent_Type
                         FROM dbo.Ct_Sale_Agent AS SA LEFT OUTER JOIN
                              dbo.Ct_Sale_Agent_Ordinary AS SAO ON SA.Sale_Agent_ID = SAO.Sale_Agent_ID INNER JOIN
                              dbo.Ct_Sale_Agent_Contact AS SAC ON SAC.Sale_Agent_ID = SA.Sale_Agent_ID
                         WHERE (SA.Status = 0) AND (SA.Sale_Agent_ID NOT IN
                             (SELECT Sale_Agent_ID
                               FROM dbo.Ct_Sale_Agent_Cancel))
                                and SAO.Last_Name LIKE '%" + last_name + "%' AND SAO.First_Name LIKE '%" + first_name + "%'  order by  SA.Sale_Agent_ID";

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            cmd.Connection = con;
            con.Open();
            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetSaleAgent_By_SaleAgentCode] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return dt;
    }

    /// <summary>
    /// Get Sale Agent List
    /// </summary>

    public static DataTable GetSaleAgentList()
    {
        DataTable dt = new DataTable();
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Sale_Agent_List";
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            cmd.Connection = con;
            con.Open();
            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetSaleAgentList] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return dt;
    }

    public static List<bl_sale_agent_all> GetAllSaleAgent()
    {
        List<bl_sale_agent_all> MyList = new List<bl_sale_agent_all>();
        bl_sale_agent_all agent;
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_ALL_SALE_AGENT", new string[,] { });
            foreach (DataRow row in tbl.Rows)
            {
                agent = new bl_sale_agent_all();
                agent.Sale_Agent_ID = row["sale_agent_id"].ToString();
                agent.Sale_Agent_Type_Name = row["sale_agent_type_name"].ToString();
                agent.ID_Card = row["id_card"].ToString();
                agent.ID_Type = Convert.ToInt32(row["id_type"].ToString());
                agent.Gender = Convert.ToInt32(row["gender"].ToString());
                agent.Country_ID = row["country_id"].ToString();
                agent.Birth_Date = Convert.ToDateTime(row["birth_date"].ToString());
                agent.Mobile_Phone = row["mobile_phone1"].ToString();
                agent.Home_Phone = row["home_phone1"].ToString();
                agent.Email = row["email"].ToString();
                agent.Full_Name_EN = row["full_name_en"].ToString();
                agent.Full_Name_KH = row["full_name_kh"].ToString();
                agent.Sale_Agent_Type = Convert.ToInt32(row["sale_agent_type"].ToString());
                agent.Created_Note = row["created_note"].ToString();
                agent.Resigned = row["resigned"].ToString();

                MyList.Add(agent);

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetAllSaleAgent] in class [da_sale_agent], Detail: " + ex.Message);
        }
        return MyList;
    }

    public static string GetAgentList_by_sale_Code(string Sale_Agent_ID, int Sale_Agent_Type, string ID_Card)
    {
        string string_duplicate = "";

        List<bl_sale_agent> sale_agent_list = new List<bl_sale_agent>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_Duplicate_Sale_Agent", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Sale_Agent_ID", Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Sale_Agent_Type", 0);
            //cmd.Parameters.AddWithValue("@ID_Card", "");
            cmd.Parameters.AddWithValue("@ID_Card", ID_Card);
            cmd.Parameters.AddWithValue("@check_duplicate_type", 0);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            bl_sale_agent sale_agent = new bl_sale_agent();

            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    sale_agent.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));
                    sale_agent.Sale_Agent_Type = rdr.GetInt32(rdr.GetOrdinal("Sale_Agent_Type"));
                    sale_agent.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                    sale_agent.Status = rdr.GetInt32(rdr.GetOrdinal("Status"));
                    sale_agent.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                    sale_agent.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                    sale_agent.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                    sale_agent_list.Add(sale_agent);
                }
            }

            if (sale_agent_list.Count > 0)
            {
                if (sale_agent.Sale_Agent_ID.ToUpper() == Sale_Agent_ID.ToUpper())
                {
                    string_duplicate += " Sale Agent Code (" + Sale_Agent_ID + ")";
                }

                if (sale_agent.Sale_Agent_Type == Sale_Agent_Type)
                {
                    if (sale_agent.Sale_Agent_Type == 0)
                    {
                        string_duplicate += " Sale Agent Type (" + "Ordinary Agent" + ")";
                    }
                    else if (sale_agent.Sale_Agent_Type == 1)
                    {
                        string_duplicate += " Sale Agent Type (" + "Bank Agent" + ")";
                    }
                    else if (sale_agent.Sale_Agent_Type == 2)
                    {
                        string_duplicate += " Sale Agent Type (" + "Broker Agent" + ")";
                    }
                }
            }
            else
            {
                //check ID card
                if (Check_Duplicate_string(ID_Card, Sale_Agent_Type, Sale_Agent_ID) == true)
                {
                    string_duplicate += " ID Card (" + ID_Card + ")";
                }
            }



            if (string_duplicate != "")
            {
                string_duplicate += " have already existed";
            }
        }

        return string_duplicate;
    }

    public static bool Check_Duplicate_string(string card, int sale_agent_type, string sale_agent_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_Duplicate_Sale_Agent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Sale_Agent_ID", "");
            //cmd.Parameters.AddWithValue("@Sale_Agent_Type", 0);
            //cmd.Parameters.AddWithValue("@ID_Card", card);
            //cmd.Parameters.AddWithValue("@check_duplicate_type", 2);

            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
            cmd.Parameters.AddWithValue("@Sale_Agent_Type", sale_agent_type);
            cmd.Parameters.AddWithValue("@ID_Card", card);
            if (sale_agent_type == 0)//ordinary
            {
                cmd.Parameters.AddWithValue("@check_duplicate_type", 2);
            }
            else //bank and broker
            {
                cmd.Parameters.AddWithValue("@check_duplicate_type", 0);
            }


            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate_string] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static bool Check_Duplicate_Edit(string sale_id, string card)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_Duplicate_Sale_Agent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_id);
            cmd.Parameters.AddWithValue("@Sale_Agent_Type", 0);
            cmd.Parameters.AddWithValue("@ID_Card", card);
            cmd.Parameters.AddWithValue("@check_duplicate_type", 3);

            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate_Edit] in class [bl_sale_agent]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get sale agent name by ID
    public static string GetSaleAgentNameByID(string sale_agent_id)
    {
        //Declare object
        string sale_agent_name = null;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_Name_By_ID";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            sale_agent_name = myReader.GetString(myReader.GetOrdinal("Full_Name"));

                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetSaleAgentNameByID] in class [da_sale_agent]. Details: " + ex.Message);
        }
        return sale_agent_name;
    }

    //Function to check sale agent id
    public static bool CheckSaleAgentId(string id)
    {

        bool result = new bool();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                //Mysql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Check_Sale_Agent_ID";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        result = true;
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
            Log.AddExceptionToLog("Error in function [CheckSaleAgentId] in class [da_sale_agent]. Details: " + ex.Message);
        }
        return result;
    }

    //Get channel_location_id from object sale_agent_channel_location by sale_agent_id
    public static string GetChannelLocationIDBySaleAgentID(string sale_agent_id)
    {
        //Declare object
        string channel_location_id = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Channel_Location_ID_By_Sale_Agent_ID";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            channel_location_id = myReader.GetString(myReader.GetOrdinal("Channel_Location_ID"));

                        }
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
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetChannelLocaitonIDBySaleAgentID] in class [da_sale_agent]. Details: " + ex.Message);
        }
        return channel_location_id;
    }
    #endregion

    //Get Sale_Agent_Team_ID by user id
    public static string GetSaleAgentTeamID(string user_id)
    {
        string sale_agent_team_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_By_User_ID";
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        sale_agent_team_id = myReader.GetString(myReader.GetOrdinal("Sale_Agent_Team_ID"));

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
            Log.AddExceptionToLog("Error in function [GetSaleAgentTeamID] in class [da_sale_sale]. Details: " + ex.Message);

        }
        return sale_agent_team_id;
    }

    //Get Sale_Agent_ID by user id
    public static string GetSaleAgentIDByUserID(string user_id)
    {
        string sale_agent_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_ID_By_User_ID";
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        sale_agent_id = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));

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
            Log.AddExceptionToLog("Error in function [GetSaleAgentIDByUserID] in class [da_sale_agent Details: " + ex.Message);

        }
        return sale_agent_id;
    }

    //Get Channel_ID by user id
    public static string GetChannelID(string user_id)
    {
        string channel_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_By_User_ID";
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        channel_id = myReader.GetString(myReader.GetOrdinal("Channel_ID"));

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
            Log.AddExceptionToLog("Error in function [GetChannelID] in class [da_sale_agent]. Details: " + ex.Message);

        }
        return channel_id;
    }

    //Get Code
    public static ArrayList GetAllSaleAgentCodeBySupervisorCode(string supervisor_id)
    {
        ArrayList final_agent_id_list = new ArrayList();

        ArrayList agent_id_list = new ArrayList();

        agent_id_list = da_sale_agent.GetSaleAgentIDListBySupervisorID(supervisor_id);

        int loop = 0;
        for (int i = 0; i < agent_id_list.Count; i++)
        {

            string my_agent_id = agent_id_list[i].ToString();

            ArrayList agent_id_list2 = new ArrayList();

            if (loop <= 10000 && my_agent_id != supervisor_id)
            {
                loop += 1;
                agent_id_list2 = da_sale_agent.GetSaleAgentIDListBySupervisorID(my_agent_id);
                final_agent_id_list.Add(my_agent_id);
            }

            if (agent_id_list2.Count > 0)
            {
                //Loop sub child list 
                for (int j = 0; j < agent_id_list2.Count; j++)
                {

                    agent_id_list.Add(agent_id_list2[j]);

                }
            }
        }

        return final_agent_id_list;
    }

    //Get Sale Agent Name
    public static string GetSaleAgentName(string sale_agent_id)
    {
        string sale_agent_name = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_Name_By_ID";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        sale_agent_name = myReader.GetString(myReader.GetOrdinal("Full_Name"));

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
            Log.AddExceptionToLog("Error in function [GetSaleAgentName] in class [da_sale_agent]. Details: " + ex.Message);

        }
        return sale_agent_name;
    }

    //Get all active Sale Agent List by now
    public static List<bl_sale_agent> GetActiveSaleAgentList()
    {
        List<bl_sale_agent> Sale_Agent_List = new List<bl_sale_agent>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Active_Sale_Agent_List";

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_sale_agent sale_agent = new bl_sale_agent();
                        sale_agent.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                        sale_agent.Full_Name = myReader.GetString(myReader.GetOrdinal("Full_Name"));
                        sale_agent.Channel_ID = myReader.GetString(myReader.GetOrdinal("Channel_ID"));
                        Sale_Agent_List.Add(sale_agent);
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
            Log.AddExceptionToLog("Error in function [GetActiveSaleAgentList] in class [da_sale_sale]. Details: " + ex.Message);

        }
        return Sale_Agent_List;
    }

    //Get Sale_Agent_ID list by supervisor id
    public static ArrayList GetSaleAgentIDListBySupervisorID(string supervisor_id)
    {
        ArrayList sale_agent_id_list = new ArrayList();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_ID_List_By_Supervisor_ID";
                myCommand.Parameters.AddWithValue("@Supervisor_ID", supervisor_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        sale_agent_id_list.Add(myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID")));

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
            Log.AddExceptionToLog("Error in function [GetSaleAgentIDListBySupervisorID] in class [da_sale_agent] Details: " + ex.Message);

        }
        return sale_agent_id_list;
    }

    //Get sale agent supervisor id
    public static bl_sale_agent GetSaleAgentSupervisor(string sale_agent_id)
    {
        bl_sale_agent supervisor = new bl_sale_agent();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Supervisor_By_Sale_Agent_ID";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        supervisor.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_Supervisor_ID"));
                        supervisor.Full_Name = myReader.GetString(myReader.GetOrdinal("Full_Name"));
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
            Log.AddExceptionToLog("Error in function [GetSaleAgentSupervisor] in class [da_sale_agent]. Details: " + ex.Message);

        }
        return supervisor;
    }

    public static List<bl_sale_agent> GetSaleAgentBTL(string userName = "")
    {
        List<bl_sale_agent> aList = new List<bl_sale_agent>();

        try
        {
            DB db = new DB();

            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGETN_GET_BTL", new string[,] {
            
            }, MYNAME + "=>GetSaleAgentBTL(string userName)");

            if (db.RowEffect > 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    aList.Add(new bl_sale_agent()
                    {
                        Sale_Agent_ID = r["sale_agent_id"].ToString(),
                        Full_Name = r["full_name"].ToString(),
                        Khmer_Full_Name = r["full_name_kh"].ToString(),
                        Email = r["email"].ToString(),
                        Position = r["position"].ToString()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            aList = new List<bl_sale_agent>();
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return aList;
    }


    public static List<bl_sale_agent_micro> GetSaleAgentMicroList()
    {
        List<bl_sale_agent_micro> objList = new List<bl_sale_agent_micro>();
        try
        {
            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_GET", new string[,] { }, "da_sale_agent=>GetSaleAgentMicroList()");
            foreach (DataRow r in tbl.Rows)
            {
                objList.Add(new bl_sale_agent_micro()
                {
                    SaleAgentId = r["sale_agent_id"].ToString(),
                    FullNameEn = r["full_name"].ToString(),
                    FullNameKh = r["full_name_kh"].ToString(),
                    Position = r["position"].ToString(),
                    Email = r["email"].ToString(),
                    SaleAgentType = Convert.ToInt32(r["sale_agent_type"].ToString()),
                    Status = Convert.ToInt32(r["status"].ToString()),
                    CreatedBy = r["created_by"].ToString(),
                    CreatedNote = r["created_note"].ToString(),
                    CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                   ValidFrom=Helper.FormatDateTime(Convert.ToDateTime(r["valid_from"].ToString()).ToString("dd/MM/yyyy")),
                    ValidTo = Helper.FormatDateTime(Convert.ToDateTime(r["valid_to"].ToString()).ToString("dd/MM/yyyy"))
                });
            }
        }
        catch (Exception ex)
        {
            objList = new List<bl_sale_agent_micro>();
            Log.AddExceptionToLog("Error function [GetSaleAgentMicroList()] in class [da_sale_agent], detail:" + ex.Message);
        }
        return objList;
    }
    /// <summary>
    /// Get Sale agent micro by name or id
    /// </summary>
    /// <param name="value"> full name or sale agent id </param>
    /// <returns></returns>
    public static List<bl_sale_agent_micro> GetSaleAgentMicroList(string  value)
    {
        List<bl_sale_agent_micro> objList = new List<bl_sale_agent_micro>();
        try
        {
            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_GET_BY_INFO", new string[,] { 
             {"@value", value}
            }, "da_sale_agent=>GetSaleAgentMicroList(string  value)");
            foreach (DataRow r in tbl.Rows)
            {
                objList.Add(new bl_sale_agent_micro()
                {
                    SaleAgentId = r["sale_agent_id"].ToString(),
                    FullNameEn = r["full_name"].ToString(),
                    FullNameKh = r["full_name_kh"].ToString(),
                    Position = r["position"].ToString(),
                    Email = r["email"].ToString(),
                    SaleAgentType = Convert.ToInt32(r["sale_agent_type"].ToString()),
                    Status = Convert.ToInt32(r["status"].ToString()),
                    CreatedBy = r["created_by"].ToString(),
                    CreatedNote = r["created_note"].ToString(),
                    CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                    ValidFrom = Helper.FormatDateTime(Convert.ToDateTime(r["valid_from"].ToString()).ToString("dd/MM/yyyy")),
                    ValidTo = Helper.FormatDateTime(Convert.ToDateTime(r["valid_to"].ToString()).ToString("dd/MM/yyyy"))
                });
            }
        }
        catch (Exception ex)
        {
            objList = new List<bl_sale_agent_micro>();
            Log.AddExceptionToLog("Error function [GetSaleAgentMicroList(string  value)] in class [da_sale_agent], detail:" + ex.Message);
        }
        return objList;
    }
    public static bl_sale_agent_micro GetSaleAgentMicro(string saleAgentId)
    {
        bl_sale_agent_micro obj = new bl_sale_agent_micro();
        try
        {
            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_GET_BY_SALE_AGENT_ID", new string[,] { 
            {"@sale_agent_id", saleAgentId}
            }, "da_sale_agent=>GetSaleAgentMicro(string saleAgentId)");
            foreach (DataRow r in tbl.Rows)
            {
                obj=new bl_sale_agent_micro()
                {
                    SaleAgentId = r["sale_agent_id"].ToString(),
                    FullNameEn = r["full_name"].ToString(),
                    FullNameKh = r["full_name_kh"].ToString(),
                    Position = r["position"].ToString(),
                    Email = r["email"].ToString(),
                    SaleAgentType = Convert.ToInt32(r["sale_agent_type"].ToString()),
                    Status = Convert.ToInt32(r["status"].ToString()),
                    CreatedBy = r["created_by"].ToString(),
                    CreatedNote = r["created_note"].ToString(),
                    CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                    ValidFrom = Helper.FormatDateTime(Convert.ToDateTime(r["valid_from"].ToString()).ToString("dd/MM/yyyy")),
                    ValidTo = Helper.FormatDateTime(Convert.ToDateTime(r["valid_to"].ToString()).ToString("dd/MM/yyyy"))
                };
            }
        }
        catch (Exception ex)
        {
            obj = new bl_sale_agent_micro();
            Log.AddExceptionToLog("Error function [GetSaleAgentMicro(string saleAgentId)] in class [da_sale_agent], detail:" + ex.Message);
        }
        return obj;
    }
    public static bool AddSaleAgentMicro(bl_sale_agent_micro obj)
    {
        bool add = false;
        try
        {
            DB db = new DB();
            add = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_INSERT", new string[,] {
            {"@SALE_AGENT_ID", obj.SaleAgentId},
            {"@SALE_AGENT_TYPE", obj.SaleAgentType+""},
            {"@FULL_NAME", obj.FullNameEn},
            {"@FULL_NAME_KH", obj.FullNameKh},
            {"@POSITION", obj.Position},
            {"@EMAIL", obj.Email},
            {"@STATUS", obj.Status+""},
            {"@CREATED_BY", obj.CreatedBy},
            {"@CREATED_ON", obj.CreatedOn+""},
            {"@CREATED_NOTE", obj.CreatedNote},
            {"@Valid_From", obj.ValidFrom+""},
            {"@Valid_To", obj.ValidTo+""}
            }, "da_sale_agent=>AddSaleAgentMicro(bl_sale_agent_micro obj)");
            
        }
        catch (Exception ex)
        {
            add = false;
            Log.AddExceptionToLog("Error function [AddSaleAgentMicro(bl_sale_agent_micro obj)] in class [da_sale_agent], detail:" + ex.Message);
        }
        return add;
    }
    /// <summary>
    /// Update sale agent micro by sale agent id
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool UpdateSaleAgentMicro(bl_sale_agent_micro obj)
    {
        bool add = false;
        try
        {
            DB db = new DB();
            add = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_UPDATE", new string[,] {
            {"@SALE_AGENT_ID", obj.SaleAgentId},
            {"@SALE_AGENT_TYPE", obj.SaleAgentType+""},
            {"@FULL_NAME", obj.FullNameEn},
            {"@FULL_NAME_KH", obj.FullNameKh},
            {"@POSITION", obj.Position},
            {"@EMAIL", obj.Email},
            {"@STATUS", obj.Status+""},
            {"@Valid_From", obj.ValidFrom+""},
            {"@Valid_To", obj.ValidTo+""}
            }, "da_sale_agent=>UpdateSaleAgentMicro(bl_sale_agent_micro obj)");

        }
        catch (Exception ex)
        {
            add = false;
            Log.AddExceptionToLog("Error function [UpdateSaleAgentMicro(bl_sale_agent_micro obj)] in class [da_sale_agent], detail:" + ex.Message);
        }
        return add;
    }
}