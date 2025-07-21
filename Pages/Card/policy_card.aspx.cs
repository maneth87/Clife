using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.Security;
public partial class Pages_Card_policy_card : System.Web.UI.Page
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        my_excel.EmployeeList.Clear();
        if (!Page.IsPostBack)
        {
            
            ddl_policy_type.Items.Add(new ListItem("Individual","IND"));
            ddl_policy_type.Items.Add(new ListItem("Group", "GROUP"));
            
        }
    }
    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        List<string> list_policy_id = new List<string>();
        List<string> list_customer_id = new List<string>();

        if (ckb_excel.Checked)
        {
            #region excel file
            //delete old record
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;
            INSERT_TEMP_DATA temp_data;
            temp_data = new INSERT_TEMP_DATA();
            temp_data.DELETE_TEMP_DATA(user_name);

            foreach (GridViewRow row in gv_excel.Rows)
            {
                CheckBox ckb = (CheckBox)row.FindControl("ckb");
                if (ckb.Checked)
                {

                    ckb.Checked = true;

                    Label lbl_policy_id = (Label)row.FindControl("lblPolicy_ID");

                    list_policy_id.Add(lbl_policy_id.Text.Trim());

                    Label lbl_kh_name = (Label)row.FindControl("lblKh_Name");
                    Label lbl_en_name = (Label)row.FindControl("lblEn_Name");
                    Label lbl_policy_number = (Label)row.FindControl("lblPolicy_Number");
                    Label lbl_customer_id = (Label)row.FindControl("lblCustomer_ID");
                    Label lbl_effective_date = (Label)row.FindControl("lblEffective_Date");
                    Label lbl_maturity_date = (Label)row.FindControl("lblMaturity_Date");

                     temp_data = new INSERT_TEMP_DATA();
                  
                    temp_data.Policy_Number = lbl_policy_number.Text  ;
                    temp_data.Customer_Number = lbl_customer_id.Text;
                    temp_data.KH_Name = lbl_kh_name.Text;
                    temp_data.EN_Name = lbl_en_name.Text;
                    temp_data.Effective_Date = Helper.FormatDateTime(lbl_effective_date.Text);
                    temp_data.Maturity_Date = Helper.FormatDateTime(lbl_maturity_date.Text);
                    temp_data.Created_By = user_name;

                    //create new format for policy number
                    if (ddl_policy_type.SelectedValue.Trim() == "IND")//individual
                    {
                        temp_data.Policy_Number = temp_data.Policy_Number;// +"-" + temp_data.Customer_Number;
                    }
                    else
                    {
                        temp_data.Policy_Number = temp_data.Policy_Number + "-" + temp_data.Customer_Number;
                    }

                    
                    temp_data.INSERT_DATA(temp_data);

                }

            }
            Session["SS_DATA_SOURCE_TYPE"] = "EXTERNAL";
            #endregion
        }
        else
        {
            #region in database
            foreach (GridViewRow row in gv_policy.Rows)
            {
                CheckBox ckb = (CheckBox)row.FindControl("ckb");
                if (ckb.Checked)
                {

                    ckb.Checked = true;

                    Label lbl_policy_id = (Label)row.FindControl("lblPolicy_ID");

                    list_policy_id.Add(lbl_policy_id.Text.Trim());


                    if (ddl_policy_type.SelectedValue.Trim().ToUpper() == "GROUP")
                    {
                        Label lblCustomer_ID = (Label)row.FindControl("lblCustomer_ID");

                        list_customer_id.Add(lblCustomer_ID.Text.Trim());
                    }

                }

            }

            Session["SS_DATA_SOURCE_TYPE"] = "INTERNAL";
            //Store policy id list for page print_policy_cards_rp.aspx
            Session["SS_POLICY_ID"] = list_policy_id;
           
            if (ddl_policy_type.SelectedValue.Trim().ToUpper() == "GROUP")
            {
                //Store customer id list for page print_policy_cards_rp.aspx
                Session["SS_CUSTOMER_ID"] = list_customer_id;
            }
            #endregion
        }

        if (list_policy_id.Count>0)
        {
            string url = "";
            url = "print_policy_cards_rp.aspx?policy_type=" + ddl_policy_type.SelectedValue.Trim();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
            
        }
        else
        { 
            //message alert;
        }
      
            
    }
   
    protected void ckbAll_CheckedChanged(object sender, EventArgs e)
    {

        CheckBox ckb_header = (CheckBox)gv_policy.HeaderRow.FindControl("ckbAll");

        foreach (GridViewRow row in gv_policy.Rows)
        {
            CheckBox ckb = (CheckBox)row.FindControl("ckb");
            if (ckb_header.Checked)
            {
                ckb.Checked = true;

            }
            else
            {
                ckb.Checked = false;

            }
        }
       
    }
    protected void ckbAll_CheckedChanged1(object sender, EventArgs e)
    {

        CheckBox ckb_header = (CheckBox)gv_excel.HeaderRow.FindControl("ckbAll");

        foreach (GridViewRow row in gv_excel.Rows)
        {
            CheckBox ckb = (CheckBox)row.FindControl("ckb");
            if (ckb_header.Checked)
            {
                ckb.Checked = true;

            }
            else
            {
                ckb.Checked = false;

            }
        }

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        DataTable tbl = new DataTable();
        string codi_policy_number = "";
        string f_date = "";
        string t_date = "";
      int upload_recorded = 0;
       
        #region NEW CODE
        if (txt_policy_number.Text.Trim() != "") //policy number
        {
            codi_policy_number = txt_policy_number.Text.Trim();

        }
        if (txt_effective_date_from.Text.Trim() != "" && txt_effective_date_to.Text.Trim() != "")
        {
            
            f_date =txt_effective_date_from.Text.Trim();
            t_date = txt_effective_date_to.Text.Trim();

        }
        if (ckb_excel.Checked == false)
        {
            if (ddl_policy_type.SelectedValue.Trim().ToUpper() == "IND")
            {
                tbl = DataSetGenerator.Get_Data_Soure("SP_PRINT_POLICY_CARDS", new string[,] { { "@Policy_Type", "IND" }, { "@Policy_ID", "" }, { "@Customer_ID", "" }, { "@Policy_Number", codi_policy_number }, { "@From", f_date }, { "@To", t_date } });
            }
            else if (ddl_policy_type.SelectedValue.Trim().ToUpper() == "GROUP")
            {
                tbl = DataSetGenerator.Get_Data_Soure("SP_PRINT_POLICY_CARDS", new string[,] { { "@Policy_Type", "GROUP" }, { "@Policy_ID", "" }, { "@Customer_ID", "" }, { "@Policy_Number", codi_policy_number }, { "@From", f_date }, { "@To", t_date } });
            }
        }
        else
        {
            #region Excel
          
            if (excel.HasFile)
            {
                //save path
                string save_path = "~/Upload/Policy_Cards/";
                string server_file_path = "";
                string extention = "";
                dynamic file = excel.PostedFile;
                string excel_provider = "";
                string file_name = System.IO.Path.GetFileName(excel.FileName);
                extention = System.IO.Path.GetExtension(file_name);
                string date = DateTime.Now.ToString("ddmmyyyyhhmmss");
                string[] file_name_without_ex ;
                file_name_without_ex = file_name.Split('.');
                server_file_path = Server.MapPath(save_path + file_name_without_ex[0].ToString() + date + extention);
                excel.SaveAs(server_file_path);

                if (extention.ToUpper() == ".XLSX")
                {
                    excel_provider = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + server_file_path + "';Extended Properties=Excel 12.0;";
                }
                else
                {
                    excel_provider = "provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + server_file_path + "';Extended Properties=Excel 8.0;";
                }

                System.Data.OleDb.OleDbConnection MyConnection = null;

                System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                MyConnection = new System.Data.OleDb.OleDbConnection(excel_provider);
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Data$]", MyConnection);
                MyCommand.Fill(tbl);
                MyConnection.Close();

                
                if (tbl.Rows.Count > 0)
                {
                    my_excel obj_excel = new my_excel();
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        try
                        {
                            upload_recorded = upload_recorded + 1;
                            var row = tbl.Rows[i];
                            obj_excel.Add_Employee(upload_recorded, "", row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), Convert.ToDateTime(row[4].ToString()), Convert.ToDateTime(row[5].ToString()));
                          
                        }
                        catch (Exception ex)
                        { 
                        
                        }
                        

                    }

                }

            }
            #endregion

        }
       
        #endregion
        //int records = tbl.Rows.Count;
        int records = upload_recorded;
        if (records > 0)
        {
            div_record_found.InnerHtml = records + " Record(s) Found.";
            div_record_found.Style.Add("display", "");
           
        }
        else
        {
            div_record_found.InnerHtml = "Record(s) Not Found.";
        }
        if (ckb_excel.Checked)
        {
          
            gv_excel.DataSource = my_excel.EmployeeList.ToArray();
            gv_excel.DataBind();
            gv_excel.Visible = true;
            gv_policy.Visible = false;

        }
        else
        {
            if (tbl.Rows.Count > 0)
            {
                div_record_found.InnerHtml = tbl.Rows.Count + " Record(s) Found.";
                div_record_found.Style.Add("display", "");
            }
            else
            {
                div_record_found.InnerHtml = "Record(s) Not Found.";
            }
            gv_policy.DataSource = tbl;
            gv_policy.DataBind();
            gv_excel.Visible = false;
            gv_policy.Visible = true;
        }
        
       

    }
    class my_excel
    {
        public static List<my_excel> EmployeeList = new List<my_excel>();

        public int No { get; set; }
        public string Policy_ID { get; set; }
        public string Policy_Number { get; set; }
        public string Customer_ID { get; set; }
        public string KH_Name { get; set; }
        public string EN_Name { get; set; }
        public DateTime Effective_Date { get; set; }
        public DateTime Maturity_Date { get; set; }
       
        private static List<my_excel> GetEmployeeList()
        {
            List<my_excel> employee_list = new List<my_excel>();
            my_excel emp = new my_excel();


            return employee_list;
        }
        public  bool Add_Employee(int no, string policy_id, string policy_number, string customer_number, string kh_name, string en_name, DateTime effective_date, DateTime maturity_date)
        {
            bool status = false;
            my_excel emp = new my_excel();
            emp.No = no;
            emp.Policy_ID = policy_id;
            emp.Policy_Number = policy_number;
            //emp.Customer_ID = string.Format("{0:00000}", Convert.ToDouble(customer_number));
            emp.Customer_ID = string.Format("{0:000000}", Convert.ToDouble(customer_number));
            emp.KH_Name = kh_name;
            emp.EN_Name = en_name;
            emp.Effective_Date = effective_date;
            emp.Maturity_Date = maturity_date;
            
            EmployeeList.Add(emp);
            return status;
        }
        
    }
    protected void ckb_excel_CheckedChanged(object sender, EventArgs e)
    {
        if (ckb_excel.Checked)
        {
            txt_effective_date_from.Enabled = false;
            txt_effective_date_to.Enabled = false;
            txt_policy_number.Enabled = false;
            excel.Enabled = true;
        }
        else
        {
            txt_effective_date_from.Enabled = true;
            txt_effective_date_to.Enabled = true;
            excel.Enabled = false;
            txt_policy_number.Enabled = true;
        }
    }
    class INSERT_TEMP_DATA
    {
        public string Policy_Number { get; set; }
        public string Customer_Number { get; set; }
        public string KH_Name { get; set; }
        public string EN_Name { get; set; }
        public DateTime Effective_Date { get; set; }
        public DateTime Maturity_Date { get; set; }
        public string Created_By { get; set; }

        public bool INSERT_DATA(string policy_number, string customer_number, string kh_name, string en_name, DateTime effective_date, DateTime  maturity_date, string created_by)
        {
            bool status = false;
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_TEMP_POLICY_CARD", new string[,] { { "@Policy_Number", policy_number } ,
                                                                                                                                    {"@Customer_Number", customer_number}, 
                                                                                                                                    {"@kh_Name", kh_name}, 
                                                                                                                                    {"@En_Name", en_name}, 
                                                                                                                                    {"@Effective_Date", effective_date+"" }, 
                                                                                                                                    {"@Expiry_Date", maturity_date+""},
                                                                                                                                    {"@Created_By", created_by}}, "policy_card => INSERT_DATA");

            return status;
        }
        public bool INSERT_DATA(INSERT_TEMP_DATA obj_temp_data)
        {
            bool status = false;
            status = INSERT_DATA(obj_temp_data.Policy_Number, obj_temp_data.Customer_Number, obj_temp_data.KH_Name, obj_temp_data.EN_Name, obj_temp_data.Effective_Date, obj_temp_data.Maturity_Date, obj_temp_data.Created_By);
            return status;
        }
        public void DELETE_TEMP_DATA(string created_by)
        {
            Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_TEMP_POLICY_CARD", new string[,] { { "@Created_By", created_by } }, "da_policy_card => DELETE_TEMP_DATA");
        }
    }
}