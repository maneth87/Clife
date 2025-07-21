using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_benefit_payment_clause
/// </summary>
public class bl_benefit_payment_clause
{
	#region "Private Variable"
    private string _Benefit_Payment_Clause_ID;
    private string _Product_ID;
    private string _Benefit_Clasue;

    #endregion
   
    #region "Constructor"
    public bl_benefit_payment_clause()
        {

        }
    #endregion

    #region "Public Property"


    public string Benefit_Payment_Clause_ID
    {
        get { return _Benefit_Payment_Clause_ID; }
        set { _Benefit_Payment_Clause_ID = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public string Benefit_Clasue
    {
        get { return _Benefit_Clasue; }
        set { _Benefit_Clasue = value; }
    }
    #endregion
}