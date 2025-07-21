using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_sale_agent
/// </summary>
public class bl_sale_agent
{

    #region "Private Variable"
        
        private string _Sale_Agent_ID;
        private int _Sale_Agent_Type;
        private string _Full_Name;
        private int _Status;
        private DateTime _Created_On;
        private string _Created_By;
        private string _Created_Note;
        private string _Channel_ID;

        //Extra
        private int _row_index;
        private string _SortDir;
        private string _SortColum;
     
    #endregion


    #region "Constructor"
        public bl_sale_agent()
        {

        }
        #endregion

    #region "Public Property"

    public string SortDir
    {
        get { return _SortDir; }
        set { _SortDir = value; }
    }

    public string SortColum
    {
        get { return _SortColum; }
        set { _SortColum = value; }
    }

    //====================

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
    }

    public int Sale_Agent_Type
    {
        get { return _Sale_Agent_Type; }
        set { _Sale_Agent_Type = value; }
    }

    public string Full_Name
    {
        get { return _Full_Name; }
        set { _Full_Name = value; }
    }
    #region <by: maneth date: 11052017 description: create new property for khmer full name>
    public string Khmer_Full_Name { get; set; }
    #endregion
    public int Status
    {
        get { return _Status; }
        set { _Status = value; }
    }

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
    }

    //Extra
    public int row_index
    {
        get { return _row_index; }
        set { _row_index = value; }
    }

    public string Channel_ID
    {
        get { return _Channel_ID; }
        set { _Channel_ID = value; }
    }

    #region added by maneth: 31 jan 23
    public string Email { get; set; }
    public string Position { get; set; }
    #endregion added by maneth: 31 jan 23
    #endregion

}