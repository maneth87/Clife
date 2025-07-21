using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Business_Reports_crystal_frmTest : System.Web.UI.Page
{
    MyDataBase db = new MyDataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        db.ConnectionString = AppConfiguration.GetConnectionString();
        
       

        if (!Page.IsPostBack)
        {
           
           // db.SqlStatement = "select * from Ct_App_Info_Person";
           // db.OpenConnection();
           // string str = "";
           // foreach (DataRow row in db.GetData().Rows)
           // {
           //     str += row["First_Name"].ToString() + " "+ row["last_Name"].ToString() + "<br />"; 
           // }
           //// message.InnerText = str;

           // string str = "";
            ////insert by sql text
            //db.SQLCommandType = 1;//Text
            //db.SqlStatement = "insert into tbltest (no, name) values (@no, @name)";
            ////db.SqlStatement = "insert into tbltest (no, name) values ('6', 'Zoomer')";
            //db.Paras = new string[,] { { "@no", "7" }, { "@name", "KTM" } };
            //str +=db.Insert();

            ////insert by sql store procedure
            //db.SQLCommandType = 2;//Text
            //db.SqlStatement = "SP_Insert_TblTest";
            //db.Paras = new string[,] { { "@no", "8" }, { "@name", "Toto" } };
            //str += db.Insert();
           
            //db.CloseConnction();

            //get data

            //db.SQLCommandType = 2;
            //db.SqlStatement = "SP_get_TblTest";
            //DataTable tbl = db.GetData();
            //str += "<br/>";
            //foreach (DataRow row in tbl.Rows)
            //{
            //    str += row["no"].ToString() + " " + row["name"].ToString() + "<br/>";
            //}

            //message.InnerHtml = str;
            //update text
            //db.SQLCommandType = 1;
            ////db.SqlStatement = "Update tblTest set name='Colorado' where no=1;";
            //db.SqlStatement = "Update tblTest set name=@name where no=@no;";
            //db.Paras = new string[,] { {"@name","Maneth" },{"@no","1"} };
            //db.OpenConnection();
            //message.InnerHtml = db.Update().ToString();
            //db.CloseConnction();

            

            //update store

            //db.SQLCommandType = 2;
            //db.SqlStatement = "SP_update_TblTest";
            //db.Paras = new string[,] { { "@name", "Nokia" }, { "@no", "1" } };
            //db.OpenConnection();
            //message.InnerHtml = db.Update().ToString();
            //db.CloseConnction();


            //delete tex
            //db.SQLCommandType = 1;
            //db.SqlStatement = "Delete tbltest where no in (@no);";
            //db.Paras = new string[,] { { "@no", "4" } };

            //db.Delete();

            //db.SQLCommandType = 2;
            //db.SqlStatement = "SP_get_TblTest";
            //DataTable tbl = db.GetData();
            //str += "<br/>";
            //foreach (DataRow row in tbl.Rows)
            //{
            //    str += row["no"].ToString() + " " + row["name"].ToString() + "<br/>";
            //}
           // message.InnerHtml = str;
            //message.InnerHtml = str;
            //delete store
            //db.SQLCommandType = 2;
            //db.SqlStatement = "SP_delete_TblTest";
            //db.Paras = new string[,] { { "@no", "1" } };
            //db.OpenConnection();
            //message.InnerHtml = db.Delete().ToString();
            //db.CloseConnction();
            getData();
            message.InnerHtml += CommandType.StoredProcedure.ToString();          
        }
    }
    protected void insert_Click(object sender, EventArgs e)
    {
        //insert by sql store procedure
        db.SQLCommandType = CommandType.StoredProcedure;//Text
        db.SqlStatement = "SP_Insert_TblTest";
        db.Paras = new string[,] { { "@no", txtNo.Text.Trim() }, { "@name", txtName.Text.Trim() } };
       db.Insert();
       getData();
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        db.SQLCommandType = CommandType.Text;
        db.SqlStatement = "Delete tbltest where no in (@no);";
        db.Paras = new string[,] { { "@no", txtNo.Text.Trim() } };
        db.Delete();

        getData();
    }
    void getData()
    {
        //db.SQLCommandType =CommandType.StoredProcedure;
        //db.SqlStatement = "SP_get_TblTest";

        db.SQLCommandType = CommandType.Text;
        db.SqlStatement = "select * from tblTest where no=@no";
        db.Paras = new string[,] { {"@no",txtNo.Text.Trim() } };
        DataTable tbl = db.GetData();


        message.InnerHtml = "";
        string str = "<table border='1'><tr><td>No</td><td>Name</td></tr>";
        foreach (DataRow row in tbl.Rows)
        {
           str  +="<tr><td>" + row["no"].ToString() + "</td><td> " + row["name"].ToString() + "</td></tr>";
        }
        str += "</table>";
        message.InnerHtml = str + " " + db.Message;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        getData();
    }
}