using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Pages_Admin_generate_guid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            //DataTable tbl = DataSetGenerator.Get_Data_Soure("SELECT [NAME], [OBJECT_ID] FROM SYS.TABLES ORDER BY [NAME]");
            //ddlTable.Items.Clear();
            //ddlTable.Items.Add(new ListItem("---Select---", ""));
            //foreach (DataRow row in tbl.Rows)
            //{
            //    ddlTable.Items.Add(new ListItem(row["name"].ToString(), row["name"].ToString()));
            //}
        //}
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        string guid = Helper.GetNewGuid(new string[,] {{"TABLE", ddlTable.SelectedValue.Trim()},{"FIELD", ddlColumn.SelectedValue.Trim()}});
    }
    protected void btn_Click(object sender, EventArgs e)
    {

    }
}