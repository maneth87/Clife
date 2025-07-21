using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_answer_item
/// </summary>
public class bl_app_answer_item
{
    #region "Private Variable"

    private string _App_Answer_Item_ID;
    private string _App_Register_ID;
    private string _Question_ID;
    private int _Seq_Number;
    private int _Answer;
           
    #endregion
    
    #region "Constructor"
    public bl_app_answer_item()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Answer_Item_ID
    {
        get { return _App_Answer_Item_ID; }
        set { _App_Answer_Item_ID = value; }
    }

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }
   
    public string Question_ID
    {
        get { return _Question_ID; }
        set { _Question_ID = value; }

    }

    public int Seq_Number
    {
        get { return _Seq_Number; }
        set { _Seq_Number = value; }
    }

    public int Answer
    {
        get { return _Answer; }
        set { _Answer = value; }
    }
          
    #endregion
}