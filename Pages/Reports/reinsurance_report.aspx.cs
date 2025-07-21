using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;

public partial class Pages_Reports_reinsurance_report : System.Web.UI.Page
{
    List<bl_reinsurance> ReinsuranceList;
    //List<bl_reinsurance> ReinsuranceListFilter= new List<bl_reinsurance>();
    
    
    HSSFWorkbook hssfworkbook = new HSSFWorkbook();

    protected void Page_Load(object sender, EventArgs e)
    {
      // ReinsuranceListFilter.Clear();
        if (!Page.IsPostBack)
        { 
           
            txtToDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Today);
            ddlProductType.Items.Add(new ListItem(".", ""));
            foreach(var pro in da_product.getAllProductType())
            {
                ddlProductType.Items.Add(new ListItem(pro.Product_Type, pro.Product_Type_ID+""));
            }
           
        }
    }
    protected void ImgSearch_Click(object sender, ImageClickEventArgs e)
    {
        generate();

       // ScriptManager.RegisterStartupScript(this, this.GetType(), "", "<script>$('#excel').show();</script>", false);
    }

    void generate()
    {
        DateTime date = DateTime.Today;
        //date = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", date.ToString()));
        date = Helper.FormatDateTime(txtToDate.Text);

        string []ProductType=new string[]{};// = new string[]{"Ordinary", "Savings"};
        //ProductType = ddlProductType.SelectedItem.Text;

        if (ddlProductType.SelectedItem.Text == "Ordinary" || ddlProductType.SelectedItem.Text == "Savings")
        {
            ProductType = new string [] { "Ordinary","Savings"};
        }
        else if (ddlProductType.SelectedItem.Text == "Mortgage")
        {
            ProductType = new string[] { ddlProductType.SelectedItem.Text };

        }
        else if(ddlProductType.SelectedItem.Text==".")
        {
            ProductType = new string[] { "Ordinary", "Savings", "Mortgage" };
        }

        ReinsuranceList = da_reinsurance.getReinsuranceRecords(date, 50000);
        string str = "";
        int no = 0;
        int recordNo = 0;

        
      
        //str += "<p style='color:red; font-family:Arial; font-size:12px;'>" + recordNo + " Record(s) found.</p>";

        #region table header
        //border=1 style='border:solid 2px #808080;'
        str += "<table class='table-bordered' ><tr><th rowspan='2'>No</th><th rowspan='2'>Customer ID</th><th rowspan='2'>Policy No.</th><th rowspan='2'>Insured Name</th><th rowspan='2'>Date of Birth</th><th colspan='2'>Age</th>" +
                "<th rowspan='2'>Gender</th><th rowspan='2'>Plan</th><th rowspan='2'>Plan Code</th><th rowspan='2'>Cov.Per.</th><th rowspan='2'>Pmt Per.</th><th rowspan='2'>Effective Date</th>" +
                "<th rowspan='2'>Policy Year</th><th rowspan='2'>%EM</th><th rowspan='2'>Status</th><th rowspan='2'>Original Sum Insured</th><th rowspan='2'>Retention</th><th colspan='2'>Sum Reinsured</th><th rowspan='2'>Remarks</th><th rowspan='2'>Others</th><th rowspan='2'>Product type</th></tr>" +
                "<tr><th>Entry</th><th>Current</th><th>Automatic</th><th>Faculative</th></tr>";

        #endregion

        //ReinsuranceListFilter = new List<bl_reinsurance>();
        filter.reFilter.Clear();
        foreach (bl_reinsurance re in ReinsuranceList.Where(re => ProductType.Contains(re.ProductType)))
        //foreach (bl_reinsurance re in ReinsuranceList)
                      
        {
          //  ReinsuranceListFilter.Add(re);
            filter.reinsure = re;
            filter.addFilter();
            no += 1;
            recordNo = recordNo + 1;
            string others = "";
          
            if (re.SumInsureVarian > 0)
            {
                others += "Increase sum insure " + re.SumInsureVarian + "";
               
            }
            else if (re.SumInsureVarian < 0)
            {
                others += "Decrease sum insure " + (-1) * re.SumInsureVarian + " ";
            }

            if (re.EMPercentVarian > 0)
            {
                others += "Increase EM " + re.EMPercentVarian + "% ";

            }
            else if (re.EMPercentVarian < 0)
            {
                others += "Decrease EM " + (-1) * re.EMPercentVarian + "% ";
            }

            re.Others = others;
           
            str += "<tr><td>" + no + "</td>" +
                         "<td>" + re.CustomerID + "</td>" +
                         "<td>" + re.PolicyNumber + "</td>" +
                         "<td>" + re.InsuredNameEN + "</td>" +
                         "<td>" + string.Format("{0:dd/MMM/yyyy}", re.BirthDate) + "</td>" +
                         "<td>" + re.AgeInsure + "</td>" +
                         "<td>" + re.CurrentAge + "</td>" +
                         "<td>" + re.Gender + "</td>" +
                         "<td>" + re.ProductName + "</td>" +
                         "<td>" + re.PlanCode + "</td>" +
                         "<td>" + re.CoveragePeriod + "</td>" +
                         "<td>" + re.PaymentPeriod + "</td>" +
                         "<td>" + string.Format("{0:dd/MMM/yyyy}", re.EffectiveDate) + "</td>" +
                         "<td>" + re.PolicyYear + "</td>" +
                         //"<td>" + re.EMPercent + "</td>" +
                          "<td>" + re.TotalEMPercent + "</td>" +
                         "<td>" + re.Status + "</td>" +
                         "<td >" + re.TotalSumInsure + "</td>" +
                         "<td>" + re.Retention + "</td>" +
                         "<td>" + re.AutomaticSumInsure + "</td>" +
                         "<td>"+ re.Faculative +"</td>" +
                         "<td>" + re.Remarks + "</td>" +
                         "<td>" + others + "</td>" +
                         "<td>" + re.ProductType + "</td>" +
                         "</tr>";

            //if (rowspan == 1)
            //{
            //    str += "<tr><td>" + no + "</td>" +
            //           "<td>" + re.CustomerID + "</td>" +
            //           "<td>" + re.PolicyNumber + "</td>" +
            //           "<td>" + re.InsuredNameEN + "</td>" +
            //           "<td>" + string.Format("{0:dd/MMM/yyyy}", re.BirthDate) + "</td>" +
            //           "<td>" + re.AgeInsure + "</td>" +
            //           "<td>" + re.CurrentAge + "</td>" +
            //           "<td>" + re.Gender + "</td>" +
            //           "<td>" + re.ProductName + "</td>" +
            //           "<td>" + re.PlanCode + "</td>" +
            //           "<td>" + re.CoveragePeriod + "</td>" +
            //           "<td>" + re.PaymentPeriod + "</td>" +
            //           "<td>" + string.Format("{0:dd/MMM/yyyy}", re.EffectiveDate) + "</td>" +
            //           "<td>" + re.PolicyYear + "</td>" +
            //           "<td>" + re.EMPercent + "</td>" +
            //           "<td>" + re.Status + "</td>" +
            //           "<td >" + re.TotalSumInsure + "</td>" +
            //           "<td>" + re.Retention + "</td>" +
            //           "<td>" + re.AutomaticSumInsure + "</td>" +
            //           "<td>0</td>" +
            //           "<td>" + re.Remarks + "</td>" +
            //           "<td>" + others + "</td>" +
            //           "</tr>";
            //}
            //else
            //{
            //    str += "<tr><td>" + no + "</td>" +
            //           "<td rowspan='" + rowspan + "'>" + re.CustomerID + "</td>" +
            //           "<td>" + re.PolicyNumber + "</td>" +
            //           "<td>" + re.InsuredNameEN + "</td>" +
            //           "<td>" + string.Format("{0:dd/MMM/yyyy}", re.BirthDate) + "</td>" +
            //           "<td>" + re.AgeInsure + "</td>" +
            //           "<td>" + re.CurrentAge + "</td>" +
            //           "<td>" + re.Gender + "</td>" +
            //           "<td>" + re.ProductName + "</td>" +
            //           "<td>" + re.PlanCode + "</td>" +
            //           "<td>" + re.CoveragePeriod + "</td>" +
            //           "<td>" + re.PaymentPeriod + "</td>" +
            //           "<td>" + string.Format("{0:dd/MMM/yyyy}", re.EffectiveDate) + "</td>" +
            //           "<td>" + re.PolicyYear + "</td>" +
            //           "<td>" + re.EMPercent + "</td>" +
            //           "<td>" + re.Status + "</td>" +
            //           "<td rowspan='" + rowspan + "'>" + re.TotalSumInsure + "</td>" +
            //           "<td>" + re.Retention + "</td>" +
            //           "<td>" + re.AutomaticSumInsure + "</td>" +
            //           "<td>0</td>" +
            //           "<td>" + re.Remarks + "</td>" +
            //           "<td>" + others + "</td>" +
            //           "</tr>";
            //}
        }
        str += "</table>";


       

        if (recordNo > 0)
        {
            message.InnerHtml = "<p style='color:red; font-family:Arial; font-size:12px;'>" + recordNo + " Record(s) found.</p>" + str;
        }
        else
        {
            message.InnerHtml = "<p style='color:red; font-family:Arial; font-size:12px;'>" + recordNo + " Record(s) found.</p>";
        }
    }

    void excel()
    {
        string filename = "Reinsurance_Records_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        Response.Clear();
        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
        #region Columns header
        //make a header row
        //HSSFRow row1 = (HSSFRow)sheet1.CreateRow(0);

        //HSSFCell cell1=   (HSSFCell)row1.CreateCell(0);
        //cell1.SetCellValue("No");

        //HSSFCell cell2 = (HSSFCell)row1.CreateCell(1);
        //cell2.SetCellValue("Customer ID");

        //HSSFCell cell3 = (HSSFCell)row1.CreateCell(2);
        //cell3.SetCellValue("Policy No.");

        //HSSFCell cell4 = (HSSFCell)row1.CreateCell(3);
        //cell4.SetCellValue("Insured Name");

        //HSSFCell cell5 = (HSSFCell)row1.CreateCell(4);
        //cell5.SetCellValue("Insured Name");

        
        Helper.excel.Sheet = sheet1;
        
        Helper.excel.HeaderText = new string[] {"No", "Customer ID","Policy No.", "Insured Name", "Date of Birth", "Ent Age", "Cur Age", "Gender", "Plan", "Plan Code",
                                                "Cov.Per.", "Pmt Per.", "Effective Date", "Policy Year", "%EM", "Status", "Original Sum Insured", "Retention", "Automatic", "Faculative", "Remarks" ,"Others"};

        Helper.excel.generateHeader();
        
       
        #endregion

        #region rows
        //loops through data
        int rowIndex = 1;

        foreach (bl_reinsurance re in filter.reFilter)
        {
            HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(rowIndex);

            HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
            Cell1.SetCellValue(rowIndex);

            HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
            Cell2.SetCellValue(re.CustomerID);

            HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
            Cell3.SetCellValue(re.PolicyNumber);

            HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
            Cell4.SetCellValue(re.InsuredNameEN);

            HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
            Cell5.SetCellValue(re.BirthDate);

            HSSFCellStyle style =(HSSFCellStyle) hssfworkbook.CreateCellStyle();
            style.DataFormat=  hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
            Cell5.CellStyle = style;

            HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
            Cell6.SetCellValue(re.AgeInsure);

            HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
            Cell7.SetCellValue(re.CurrentAge);

            HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
            Cell8.SetCellValue(re.Gender);

            HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
            Cell9.SetCellValue(re.ProductName);

            HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
            Cell10.SetCellValue(re.PlanCode);

            HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
            Cell11.SetCellValue(re.CoveragePeriod);

            HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
            Cell12.SetCellValue(re.PaymentPeriod);

            HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
            Cell13.SetCellValue(re.EffectiveDate);
            style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
            Cell13.CellStyle = style;

            HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
            Cell14.SetCellValue(re.PolicyYear);

            HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
            Cell15.SetCellValue(re.EMPercent);

            HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
            Cell16.SetCellValue(re.Status);

            HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
            Cell17.SetCellValue(re.TotalSumInsure);

            HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
            Cell18.SetCellValue(re.Retention);

            HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
            Cell19.SetCellValue(re.AutomaticSumInsure);

            HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
            Cell20.SetCellValue(re.Faculative);



            HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
            Cell21.SetCellValue(re.Remarks);

            HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);
            Cell22.SetCellValue(re.Others);

            //HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);
            //Cell23.SetCellValue(re.SumInsureVarian);
            //HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(23);
            //Cell24.SetCellValue(re.SumInsure);


            rowIndex += 1;
        }
      
        #endregion

        MemoryStream file = new MemoryStream();
        hssfworkbook.Write(file);

        Response.BinaryWrite(file.GetBuffer());

        Response.End();
       
    }
    bool save()
    {
        bool status = false;
        //int a = 0;
        foreach (bl_reinsurance re in filter.reFilter)
        {
            re.Created_On = DateTime.Now;
            re.Created_By = "Admin";

            //a += 1;
            //if (a == filter.reFilter.Count)
            //{
            //    re.CustomerID = null;
            //}

            status = da_reinsurance.Insert(re);

        }
       
        return status;
    }

    protected void ImgExcel_Click(object sender, ImageClickEventArgs e)
    {
        if(filter.reFilter.Count>0)
        {
            
            if(save())
                excel();
        }
        else
        {
        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none", "<script>alert('No record(s).');return false;</script>", false);
        }
        

        //if (!save())
        //{
        //    ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none", "<script>alert('Saved fail');</script>", false);
        //}
    }
}
class filter
{
    public static List<bl_reinsurance> reFilter = new List<bl_reinsurance>();
    public static bl_reinsurance reinsure { get; set; }

    public static void addFilter()
    {
        reFilter.Add(reinsure);
    }
}
class excelHeader
{
    public static string[] HeaderText { get; set; }
    public static HSSFSheet Sheet {get;set;}

    public excelHeader(string[] headerText, HSSFSheet sheet)
    {
        HeaderText = headerText;
        Sheet = sheet;
    }

    public static void generateHeader()
    {
        HSSFRow row1 = (HSSFRow)Sheet.CreateRow(0);

        for (int i = 0; i < HeaderText.Length;i++ )
        {
            HSSFCell cell= (HSSFCell)row1.CreateCell(i);
            cell.SetCellValue(HeaderText[i].ToString());

        }
    }
}