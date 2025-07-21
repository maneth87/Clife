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

public partial class Pages_Business_Reports_new_policy_in_month : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text.Trim() != "" && txtTodate.Text.Trim() != "")
        {
            DataSet ds = DataSetGenerator.GetDataSet("SP_GET_NEW_POLICY_IN_MONTH", new string[,] { 
        {"@FROM_DATE", Helper.FormatDateTime(txtFromDate.Text.Trim())+""},
        {"@TO_DATE",Helper.FormatDateTime(txtTodate.Text.Trim())+""}
        });
            #region //EXCEL
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Summary");

            Helper.excel.Sheet = sheet1;
            //title
            Helper.excel.Title = new string[] {"New policy In Month", txtFromDate.Text.Trim()+ " - " + txtTodate.Text.Trim()};
            //design row header
            Helper.excel.HeaderText = new string[] { "Product_ID","Policy_No."};
            Helper.excel.generateHeader();
            int row_no = Helper.excel.NewRowIndex;
            foreach (DataRow row in ds.Tables[0].Rows)
            {

               
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);//NO
                Cell1.SetCellValue(row["product_id"].ToString());

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);//PolicyNo
                Cell2.SetCellValue(row["policy_no"].ToString());
                row_no += 1;

            }

            ////detail
            //int row_detail_index = row_no + 1;
            //HSSFRow rowdetail0 = (HSSFRow)sheet1.CreateRow(row_detail_index);

            //HSSFCell Celldetail0 = (HSSFCell)rowdetail0.CreateCell(0);
            //Celldetail0.SetCellValue("Detail");

            ////header in detail
            //HSSFRow rowdetail_header = (HSSFRow)sheet1.CreateRow(row_detail_index + 1);
            //HSSFCell Celldetail_header = (HSSFCell)rowdetail_header.CreateCell(0);//PolicyNo
            //Celldetail_header.SetCellValue("No.");

            //foreach (DataRow row in ds.Tables[1].Rows)
            //{
            //    HSSFRow rowdetail = (HSSFRow)sheet1.CreateRow(row_detail_index + 1);
            //    HSSFCell Celldetail = (HSSFCell)rowdetail.CreateCell(0);//PolicyNo
            //    Celldetail.SetCellValue(row["policy_number"].ToString());
            //    row_detail_index += 1;
            //}

            HSSFSheet sheet2 = (HSSFSheet)hssfworkbook.CreateSheet("Detail");

            Helper.excel.Sheet = sheet2;
           
            //design row header
            Helper.excel.HeaderText = new string[] { "No.", "Policy_Number", "Customer_Name", "Gender", "DOB", "ID_Type","ID_Card", "Product_ID","Effective_Date","Issued_Date","Sum_Assured","Premium","PayMode" };
            Helper.excel.generateHeader();

            int row_detail_index = Helper.excel.NewRowIndex;
            int row_detail_no = 0;
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                row_detail_no += 1;

                HSSFRow rowCell = (HSSFRow)sheet2.CreateRow(row_detail_index);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);//NO
                Cell1.SetCellValue(row_detail_no);

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);//PolicyNo
                Cell2.SetCellValue(row["policy_number"].ToString());
                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);//name
                Cell3.SetCellValue(row["customer_name"].ToString());
                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);//Gender
                Cell4.SetCellValue(row["Gender"].ToString());
                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);//dob
                Cell5.SetCellValue(Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy"));
                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);//id type
                Cell6.SetCellValue(row["id_type"].ToString());
                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);//id card
                Cell7.SetCellValue(row["id_card"].ToString());
                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);//product id
                Cell8.SetCellValue(row["product_id"].ToString());
                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);//effective date
                Cell9.SetCellValue(Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd/MM/yyyy"));
                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);//issued date
                Cell10.SetCellValue(Convert.ToDateTime(row["issued_date"].ToString()).ToString("dd/MM/yyyy"));
                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);//sum assured
                Cell11.SetCellValue(row["sum_assured"].ToString());
                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);//premium
                Cell12.SetCellValue(row["premium"].ToString());
                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);//pay mode
                Cell13.SetCellValue(row["paymode"].ToString());
                row_detail_index += 1;
            }


            string filename = "new_policy_in_month" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "alert('');", false);
        }
    }
}