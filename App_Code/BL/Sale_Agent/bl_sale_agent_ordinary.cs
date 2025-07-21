using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_sale_agent_ordinary
/// </summary>
public class bl_sale_agent_ordinary
{
	#region "Private Variable"
        
        private string _Sale_Agent_ID;
        private string _ID_Card;
        private int _ID_Type;
        private string _First_Name;
        private string _Last_Name;
        private int _Gender;
        private DateTime _Birth_Date;
        private string _Country_ID;

    #endregion


    #region "Constructor"
        public bl_sale_agent_ordinary()
        {

        }
        #endregion

    #region "Public Property"

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
    }

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }

    public int ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }
    }

    public string First_Name
    {
        get { return _First_Name; }
        set { _First_Name = value; }
    }

    public string Last_Name
    {
        get { return _Last_Name; }
        set { _Last_Name = value; }
    }
    #region <by: maneth date: 11052017 description: create new property for khmer full name>
    public string Khmer_First_Name { get; set; }
    public string Khmer_Last_Name { get; set; }
    #endregion
    public int Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public DateTime Birth_Date
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
    }

    public string Country_ID
    {
        get { return _Country_ID; }
        set { _Country_ID = value; }
    }

    #endregion
}