using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for MySql
/// </summary>
public class MySqlDB
{
    private MySqlConnection my_connection;
    private MySqlCommand my_command;
    private MySqlDataAdapter my_da;
    private string _Exception;
    private bool _ExecuteStatus;
    private bool _GenerateDataStatus;
    private DataTable _Data;
    //private int _CountData;

    private int _CountData()
    {
        if (GenerateDataStatus == true)
        {
            return Data.Rows.Count;
        }
        else // generate data error return -1
        {
            return -1;
        }
    }

	public MySqlDB()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public MySqlDB(string ConnectionString)
    {
        //
        // TODO: Add constructor logic here
        //
        this.ConnectionString = ConnectionString;
    }
    #region Properties
    public string ConnectionString { get; set; }

    public string[,] Parameters { get; set; }
    /// <summary>
    /// Return Execption while executing get error
    /// </summary>
    public string Exception { get { return _Exception; } }
    /// <summary>
    /// Return Execute status
    /// </summary>
    public bool ExecuteStatus { get { return _ExecuteStatus; } }
    public string ProcedureName { get; set; }
    /// <summary>
    /// Return Status after generate data into datatable
    /// </summary>
    public bool GenerateDataStatus { get { return _GenerateDataStatus; } }
    /// <summary>
    /// Return data after Generate data
    /// </summary>
    public DataTable Data { get { return _Data; } }
    /// <summary>
    /// Return number of row
    /// </summary>
    public int DataCount { get { return _CountData(); } }

    #endregion Properties
    
    public void Execute()
    {
        try
        {
            my_connection = new MySqlConnection( ConnectionString);
            my_connection.Open();
            my_command = new MySqlCommand(ProcedureName, my_connection);
            my_command.CommandType = CommandType.StoredProcedure;
           
            for (int i = 0; i <= Parameters.GetUpperBound(0); i++)
            {
                my_command.Parameters.AddWithValue(Parameters[i, 0], Parameters[i, 1]);

            }
            my_command.ExecuteNonQuery();
            my_command.Parameters.Clear();
            my_command.Dispose();
            my_connection.Close();
            _ExecuteStatus = true;
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Execute] in class[MySqlDB], Detail: " + ex.StackTrace + " => " + ex.Message);
            _ExecuteStatus = false;
            _Exception = ex.Message;
        }
    }

    public void Execute(string connection_string, string procedure_name, string[,] parameters)
    {
       
        try
        {
            my_connection = new MySqlConnection(connection_string);
            my_connection.Open();
            my_command = new MySqlCommand(procedure_name, my_connection);
            my_command.CommandType = CommandType.StoredProcedure;
            //initialize parameters
            for (int i = 0; i <= parameters.GetUpperBound(0); i++)
            {
                my_command.Parameters.AddWithValue(parameters[i, 0], parameters[i, 1]);

            }
            my_command.ExecuteNonQuery();
            my_command.Parameters.Clear();
            my_command.Dispose();
            my_connection.Close();
            _ExecuteStatus = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Execute] in class[MySqlDB], Detail: " + ex.StackTrace + " => " + ex.Message);
            _ExecuteStatus = true;
            _Exception = ex.Message;
        }
       
    }
    public void GenerateData()
    {
        DataTable my_data = new DataTable();
        try
        {
            my_connection = new MySqlConnection(ConnectionString);
            my_connection.Open();
            my_command = new MySqlCommand(ProcedureName, my_connection);
            my_command.CommandType = CommandType.StoredProcedure;
          
            for (int i = 0; i <= Parameters.GetUpperBound(0); i++)
            {
                my_command.Parameters.AddWithValue(Parameters[i, 0], Parameters[i, 1]);

            }
            my_da = new MySqlDataAdapter(my_command);
            my_da.Fill(my_data);

            _Data = my_data;

            my_da.Dispose();
            my_command.Parameters.Clear();
            my_command.Dispose();
            my_connection.Close();
            
            _GenerateDataStatus = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GenerateData()] in class[MySqlDB], Detail: " + ex.StackTrace + " => " + ex.Message);
            _GenerateDataStatus = false;
            _Exception = ex.Message;
        }
    }
}