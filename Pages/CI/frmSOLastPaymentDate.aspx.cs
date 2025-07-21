using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
using System.Web.Security;

public partial class Pages_CI_frmSOLastPaymentDate : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        DataTable agent_list = da_sale_agent.GetSaleAgentList();
        //ddlAgent.DataSource = agent_list;
        //ddlAgent.DataTextField ="Sale_Agent_ID" + "-"+ "Full_Name";
        //ddlAgent.DataValueField = "Sale_Agent_ID";
        //ddlAgent.DataBind();
        foreach (DataRow row in agent_list.Rows)
        {
            ddlAgent.Items.Add(new ListItem(row["sale_agent_id"].ToString().Trim() + " - " + row["full_name"].ToString().Trim(), row["sale_agent_id"].ToString().Trim()));
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable tbl = da_ci.Policy.GetPolicyLastDue();
        string policy_number = "";
        string condi = "";
        if (txtPolicyNumber.Text.Trim() != "")
        {
            string[] pol = txtPolicyNumber.Text.Trim().Split(';');

            foreach (string str in pol)
            {
                if (str != "")
                {
                    policy_number += policy_number == "" ? str : "," + str;
                }
            }

        }
        try
        {
            string save_path = "~/Upload/";
            string file_name = "";
            string extension = "";

            string file_path = "";
            if ((PolicyNumberFile.PostedFile != null) && !string.IsNullOrEmpty(PolicyNumberFile.PostedFile.FileName))
            {
                file_name = Path.GetFileName(PolicyNumberFile.PostedFile.FileName);
                extension = Path.GetExtension(file_name);
                file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                file_path = save_path + file_name;
                int row_number = 0;

                PolicyNumberFile.SaveAs(Server.MapPath(file_path));//save file 

                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "SELECT * FROM [Upload$]";
                DataTable my_data = new DataTable();
                my_data = my_excel.GetData();

                row_number = my_data.Rows.Count;
                foreach (DataRow row in my_data.Rows)
                {
                    policy_number += policy_number == "" ? row["policy_number"].ToString().Trim() : "," + row["policy_number"].ToString().Trim();
                }
            }


            PolicyNumberFile.Dispose();
            File.Delete(file_path);
        }
        catch (Exception)
        {
        }

        condi += policy_number == "" ? "" : "Policy_number in ('" + policy_number.Replace(",", "','") + "')";
        condi += ddlAgent.SelectedValue == "" ? "" : condi == "" ? " agent_code='" + ddlAgent.SelectedValue + "'" : " and agent_code='" + ddlAgent.SelectedValue + "'";
        DataTable tbl_final = tbl.Clone();
        DataRow newRow;
        foreach (DataRow row in tbl.Select(condi))
        {
            newRow = tbl_final.NewRow();
            foreach (DataColumn col in tbl.Columns)
            {
                newRow[col.ColumnName] = row[col.ColumnName];
            }
            tbl_final.Rows.Add(newRow);
        }
        if (tbl_final.Rows.Count > 0)
        {

            #region //EXCEL
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

            Helper.excel.Sheet = sheet1;
            //design row header
            Helper.excel.HeaderText = new string[] { "No.", "Policy_Number", "Customer_ID", "Full_Name", "Full_Name_Kh", "ID_Card", "Gender", "DOB", "Mobile_Phone", 
                "Effective_Date", "Maturity_Date", "Expiry_Date", "Sum_Assured", "Product_ID", "Agent_Code", "Agent_Name", "Pay_Mode", "Policy_Status", "Due_Date", "Pay_Date", "Premium", "Pay_Year","Pay_Lot" };
            Helper.excel.generateHeader();
            int row_no = 0;
            foreach (DataRow row in tbl_final.Rows)
            {

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);//NO
                Cell1.SetCellValue(row_no);

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);//PolicyNo
                Cell2.SetCellValue(row["policy_number"].ToString());

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);//customerID
                Cell3.SetCellValue(row["customer_id"].ToString());

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);//fullname
                Cell4.SetCellValue(row["last_name"].ToString() + " " + row["first_name"].ToString());

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);//fullname kh
                Cell5.SetCellValue(row["khmer_last_name"].ToString() + " " + row["khmer_first_name"].ToString());

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);//ID card
                Cell6.SetCellValue(row["ID_Card"].ToString());

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);//Gender
                Cell7.SetCellValue(row["gender"].ToString() == "0" ? "F" : "M");

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);//BrithDate
                Cell8.SetCellValue(Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy"));
                //Cell6.SetCellValue(row["birth_date"].ToString());
                //Cell6.CellStyle = style;

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);//Mobile
                Cell9.SetCellValue(row["Mobile_phone1"].ToString());

                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);//Effective date
                Cell10.SetCellValue(Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"));


                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);//maturity date
                Cell11.SetCellValue(Convert.ToDateTime(row["maturity_date"].ToString()).ToString("dd/MM/yyyy"));
                // Cell9.CellStyle = style;

                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);//expirydate
                Cell12.SetCellValue(Convert.ToDateTime(row["expiry_date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"));

                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);//SA
                Cell13.SetCellValue(row["sum_assured"].ToString());

                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);//Product
                Cell14.SetCellValue(row["Product_ID"].ToString());

                HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);//AgentCOde
                Cell15.SetCellValue(row["agent_code"].ToString());

                HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);//AgentCOde
                Cell16.SetCellValue(row["agent_name"].ToString());

                HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);//PaymentMode
                Cell17.SetCellValue(row["pay_mode"].ToString());

                HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);//policy status
                Cell18.SetCellValue(row["policy_status"].ToString());

                HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);//due date
                Cell19.SetCellValue(Convert.ToDateTime(row["due_date"].ToString()).ToString("dd/MM/yyyy"));


                HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);//pay date
                Cell20.SetCellValue(Convert.ToDateTime(row["pay_date"].ToString()).ToString("dd/MM/yyyy"));

                HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);//premium
                Cell21.SetCellValue(row["premium"].ToString());

                HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);//prem_year
                Cell22.SetCellValue(row["prem_year"].ToString());
                HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);//prem_lot
                Cell23.SetCellValue(row["prem_lot"].ToString());

            }

            string logDes = txtPolicyNumber.Text.Trim() == "" ? "" : string.Concat("Policy Number:", txtPolicyNumber.Text.Trim());
            logDes += ddlAgent.SelectedIndex == 0 ? "" : logDes == "" ? string.Concat("Agent name:", ddlAgent.SelectedItem.Text) : string.Concat(", Agent name:", ddlAgent.SelectedItem.Text);

            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports policy last payment report in excel file with criteria [",logDes,"] [Total record(s):", tbl_final.Rows.Count, "]."));

            string filename = "policy-last-due-date" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            System.IO.MemoryStream file = new System.IO.MemoryStream();
            hssfworkbook.Write(file);


            Response.BinaryWrite(file.GetBuffer());

            Response.End();
            #endregion
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('No data found.');", true);

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {


    }
}