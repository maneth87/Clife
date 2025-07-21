using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_product_type
/// </summary>
public class bl_product_type
{
	 #region "Private Variable"
        private int _Product_Type_ID;
        private string _Product_Type;
                 
    #endregion


    #region "Constructor"
        public bl_product_type()
        {

        }
        #endregion

    #region "Public Property"

        public int Product_Type_ID
    {
        get { return _Product_Type_ID; }
        set { _Product_Type_ID = value; }
    }

        public string Product_Type
    {
        get { return _Product_Type; }
        set { _Product_Type = value; }
    }

    
    #endregion
}