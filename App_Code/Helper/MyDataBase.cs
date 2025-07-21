using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for MyDataBase
/// </summary>
public class MyDataBase
{
    SqlConnection MyConnection;
    SqlCommand MyCommand;
    SqlDataAdapter MyDataAdapter;
   
	public MyDataBase()
	{
		//
		// TODO: Add constructor logic here
		//
        //initialize database connection
        MyConnection = new SqlConnection(ConnectionString);
        Paras = new string[,] { { } };
        Message = "";
    }

    #region Enum Standard Text
    enum TransactionType
    { 
        Insert,
        Delete,
        Update
    };
    enum MYSQLCommandType
    {
        Text,
        StoredProcedure
    };

    enum SQLResponseTest
    { 
        Successfuly,
        Updated,
        Deleted,
        Fail,
        Selected
    };

    #endregion

    #region Properties

    /// <summary>
    /// Give Database connectionstring
    /// </summary>
    public string ConnectionString { get; set; }
    /// <summary>
    /// Give Dabase type: SQL Server or MySql
    /// </summary>
    public string DataBaseType { get; set; }
    /// <summary>
    /// Set SqlStatement as Sql query if SQLCommandType=1 or as Store procedure's name if SQLCommandType=2
    /// </summary>
    public string SqlStatement { get; set; }
    /// <summary>
    /// Put parameters here must be matched to parameters in query or store procedure
    /// </summary>
    public string[,] Paras { get; set; }
    /// <summary>
    /// Choose Text or Procedure
    /// </summary>
    public CommandType SQLCommandType { get; set; }

    public string Message { get; set; }
    #endregion

    #region Public Function

    /// <summary>
    /// Get data from database:
    /// To get data from database please initialize Sql connection, sql command Type, sql statement, sql parameters (if have) in MyDatabase Properties.
    /// </summary>
    /// <returns></returns>
    public DataTable GetData()
    {
        //open database connection
        OpenConnection();

        DataTable myTbl = new DataTable();
        try
        {
            MyCommand = new SqlCommand(SqlStatement, MyConnection);
            //use text or store procedure
            GetSQLCommandType(MyCommand);
            //initialize paraters
            InitialParameter(MyCommand, Paras);

            MyDataAdapter = new SqlDataAdapter(MyCommand);

            MyDataAdapter.Fill(myTbl);
            MyDataAdapter.Dispose();
            MyCommand.Parameters.Clear();
            //close database connection
            MyConnection.Close();

            MessageGenerator(myTbl.Rows.Count +" record(s) " + SQLResponseTest.Selected.ToString());
        }
        catch (Exception ex)
        {
            MessageGenerator(ex.Message);
            Log.AddExceptionToLog("Error Function [GetData] in class [MyDataBase], Detail: " + ex.Message);
            //close database connection
            CloseConnction();
        }
        return myTbl;
    }
    /// <summary>
    /// Insert data into database:
    /// To insert data into database please initialize Sql connection, sql command Type, sql statement, sql parameters (if have) in MyDatabase Properties
    /// </summary>
    /// <returns></returns>
    public bool Insert()
    {
        bool status=Execute(TransactionType.Insert.ToString());
        if (status)
        {
            MessageGenerator(SQLResponseTest.Successfuly.ToString());
        }
        else
        {
            MessageGenerator(SQLResponseTest.Fail.ToString());
        }
        return status;
    }
         
    /// <summary>
    /// Update data into database:
    /// To update data into database please initialize Sql connection, sql command Type, sql statement, sql parameters (if have) in MyDatabase Properties
    /// </summary>
    /// <returns></returns>
    public bool Update()
    {
        bool status =Execute(TransactionType.Update.ToString());
        if (status)
        {
            MessageGenerator(SQLResponseTest.Updated.ToString());
        }
        else
        {
            MessageGenerator(SQLResponseTest.Fail.ToString());
        }
        return status;
    }
    /// <summary>
    /// Delete data from database:
    /// To delete data from database please initialize Sql connection, sql command Type, sql statement, sql parameters (if have) in MyDatabase Properties
    /// </summary>
    /// <returns></returns>
    public bool Delete()
    {
        bool status= Execute(TransactionType.Delete.ToString());
        if (status)
        {
            MessageGenerator(SQLResponseTest.Deleted.ToString());
        }
        else
        {
            MessageGenerator(SQLResponseTest.Fail.ToString());
        }
        return status;
    }
  
    #endregion

    #region Local Function

   /// <summary>
   /// Open Database Connection, if connection state is closed it will be opened automatically.
   /// </summary>
   private void OpenConnection()
   {
       if (MyConnection.State == ConnectionState.Closed)
       {
           MyConnection.ConnectionString = ConnectionString;
           MyConnection.Open();
       }

   }


   /// <summary>
   /// Close Database Connection, if connection state is opend it will be closed automatically.
   /// </summary>
   private void CloseConnction()
   {
       if (MyConnection.State == ConnectionState.Open)
       {
           MyConnection.Close();
       }

   }

   /// <summary>
   /// Run sql statement base transaction type
   /// </summary>
   /// <param name="trensaction">Insert, Update and Delete</param>
   /// <returns></returns>
   private bool Execute(string trensaction)
   {
       bool status = false;
       //open database connection
       OpenConnection();
       try
       {
           MyCommand = new SqlCommand(SqlStatement, MyConnection);
           //user text or store procedure
           GetSQLCommandType(MyCommand);
           //initialize sql parameters
           InitialParameter(MyCommand, Paras);

           MyCommand.ExecuteNonQuery();
           MyCommand.Dispose();
           MyCommand.Parameters.Clear();

           //close database connection
           CloseConnction();

           status = true;

           //Clear Paras Property
           Paras = null;

       }
       catch (Exception ex)
       {
           Log.AddExceptionToLog("Error Function [Execute], transaction type [" + trensaction + "] in Class [MyDataBase], Detail: " + ex.Message);
           //close database connection
           MyConnection.Close();
           status = false;
       }
       return status;
   }
   /// <summary>
   /// Intialize sql parameters by para string array
   /// </summary>
   /// <param name="mySqlCommand">Sql Command</param>
   /// <param name="para">para[0,0]=Field, para[0,1]=Valus</param>
   private void InitialParameter(SqlCommand mySqlCommand, string[,] para)
   {
       if (para != null && para.Length > 0)
       {
           for (int i = 0; i <= para.GetUpperBound(0); i++)
           {
               mySqlCommand.Parameters.AddWithValue(para[i, 0].ToString().Trim(), para[i, 1].ToString().Trim());
           }
       }

   }
   /// <summary>
   /// Get Text or store procedure
   /// </summary>
   /// <param name="myCommand"></param>
   private void GetSQLCommandType(SqlCommand myCommand)
   {
       string text = MYSQLCommandType.Text.ToString();
       string store = MYSQLCommandType.StoredProcedure.ToString();

       if (SQLCommandType.ToString() == text)
       {
           myCommand.CommandType = CommandType.Text;

       }
       else if (SQLCommandType.ToString() == store)
       {
           myCommand.CommandType = CommandType.StoredProcedure;
       }
   }
   private void MessageGenerator(string message)
   {
       Message = message;
   }
    #endregion
}