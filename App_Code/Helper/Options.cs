using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
/// <summary>
/// Summary description for Options
/// </summary>
public class Options
{
	public Options()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Text { get; set; }
    public string Values { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dropdown">Your Dropdown List</param>
    /// <param name="source">Data source</param>
    /// <param name="field">Field name in data source to display </param>
    /// <param name="value">Field name in data source to use as value</param>
    /// <param name="selected_index">Set default selected index</param>
    /// <param name="initialText">Set default first text in dropdown list</param>
    public static void Bind(DropDownList dropdown, object source, string field, string value, int selected_index, string initialText = "")
    {
        dropdown.Items.Add(new ListItem(initialText == "" ? "---Select---" : initialText, ""));
        dropdown.AppendDataBoundItems=true;
        dropdown.DataSource = source;
        dropdown.DataValueField = value;
        dropdown.DataTextField = field;
        dropdown.DataBind();
        if (selected_index > 0)
        {
            dropdown.SelectedIndex = selected_index;
        }
        else
        {
            dropdown.SelectedIndex = 0;
        }
    }

    public static void Bind(object OBJ, object source, string field, string value, int selected_index)
    {
        if (OBJ is DropDownList)
        {
            DropDownList dropdown = (DropDownList)OBJ;
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem("---Select---", ""));
            dropdown.AppendDataBoundItems = true;
            dropdown.DataSource = source;
            dropdown.DataValueField = value;
            dropdown.DataTextField = field;
            dropdown.DataBind();
            if (selected_index > 0)
            {
                dropdown.SelectedIndex = selected_index;
            }
            else
            {
                dropdown.SelectedIndex = 0;
            }
        }
        else if (OBJ is ListBox)
        {
            ListBox ckl = (ListBox)OBJ;
            ckl.Items.Clear();
            ckl.Items.Add(new ListItem("All", ""));
            ckl.AppendDataBoundItems = true;
            ckl.DataSource = source;
            ckl.DataValueField = value;
            ckl.DataTextField = field;
            ckl.DataBind();
        }
        else if (OBJ is CheckBoxList)
        {
            CheckBoxList ckl = (CheckBoxList)OBJ;
            ckl.Items.Clear();
            ckl.Items.Add(new ListItem("All", ""));
            ckl.AppendDataBoundItems = true;
            ckl.DataSource = source;
            ckl.DataValueField = value;
            ckl.DataTextField = field;
            ckl.DataBind();
            ckl.SelectedIndex = -1;
        }
    }


    public class IDType
    {



        public static List<Options> GetIDTypeList()
        {
            List<Options> list = new List<Options>();
            list.Add(new Options() { Values = "0", Text = "ID Card" });
            list.Add(new Options() { Values = "1", Text = "Passport" });
            list.Add(new Options() { Values = "2", Text = "Visa" });
            list.Add(new Options() { Values = "3", Text = "Birth Certificate" });
            
            return list;
        }
    }
    public class Gender
    {
        public static List<Options> GetGender()
        {
            List<Options> list = new List<Options>();
            list.Add(new Options() { Values = "0", Text = "Female" });
            list.Add(new Options() { Values = "1", Text = "Male" });
         
            return list;
        }
    }
    public class Country
    {
        //public Country()
        //{
        //    GetCountryList();
        //}
        //public static List<Options> CountryList = new List<Options>();

        public static List<Options> GetCountryList()
        {
            List<Options> list = new List<Options>();
            try
            {
                
                string query = "SELECT Country_ID, Country_Name FROM Ct_Country ORDER BY Country_Name;";
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), query);
                foreach (DataRow row in tbl.Rows)
                {
                    list.Add(new Options() { Text = row["country_name"].ToString(), Values = row["Country_ID"].ToString() });
                }
              
            }
            catch (Exception ex)
            { 
            
            }
            return list;
        }

        public static List<Options> GetNationalityList()
        {
            List<Options> list = new List<Options>();
            try
            {

                string query = "SELECT Country_ID, Nationality FROM Ct_Country ORDER BY Country_Name;";
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), query);
                foreach (DataRow row in tbl.Rows)
                {
                    list.Add(new Options() { Text = row["Nationality"].ToString(), Values = row["Country_ID"].ToString() });
                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }
    }
}