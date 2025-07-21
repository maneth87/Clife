using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
class RidersList
{
    #region Properties
    public string App_Number { get; set; }
    public String App_ID { get; set; }
    public double ADB { get; set; }
    public double TPD { get; set; }
    public double Spouse { get; set; }
    public double Kid1 { get; set; }
    public double Kid2 { get; set; }
    public double Kid3 { get; set; }
    public double Kid4 { get; set; }
    public string Rider_Type { get; set; }

    #endregion
    public static List<RidersList> GetRidersList(string app_register_id)
    {
        List<RidersList> myList = new List<RidersList>();
        try
        {
            DataTable tblUW = da_application_fp6.GetDataTable("SP_Get_UW_Rider_Add_By_App", app_register_id);
            //foreach(DataRow row in tblUW.Rows)
            //{
                string rider_type = "";
                string app_id = "";
                string app_number = "";

                double adb = 0;
                double tpd = 0;
                double spouse = 0;
                double kid1 = 0;
                double kid2 = 0;
                double kid3 = 0;
                double kid4 = 0;
                tblUW = tblUW.AsEnumerable().GroupBy(r => new { Col1 = r["Col1"] })
                  .Select(g => g.OrderBy(r => r[""]).First()).CopyToDataTable();

                foreach (DataRow r in tblUW.Select("App_Register_ID='" + app_id + "' and rider_type='" + rider_type + "'"))
                {
                    rider_type = r["rider_type"].ToString().Trim().ToUpper();
                    app_id = r["App_Register_ID"].ToString().Trim().ToUpper();
                    app_number = r["app_number"].ToString().Trim().ToUpper();

                    if (rider_type == "ADB")
                    {
                        adb = Convert.ToDouble(r["system_premium"].ToString());
                    }
                    else if (rider_type == "TPD")
                    {
                       tpd = Convert.ToDouble(r["system_premium"].ToString());
                    }
                    else if (rider_type == "SPOUSE")
                    {
                       spouse = Convert.ToDouble(r["system_premium"].ToString());
                    }
                    else if (rider_type == "KID 1")
                    {
                        kid1 = Convert.ToDouble(r["system_premium"].ToString());
                    }
                    else if (rider_type == "KID 2")
                    {
                        kid2 = Convert.ToDouble(r["system_premium"].ToString());
                    }
                    else if (rider_type == "KID 3")
                    {
                        kid3 = Convert.ToDouble(r["system_premium"].ToString());
                    }
                    else if (rider_type == "KID 4")
                    {
                        kid4 = Convert.ToDouble(r["system_premium"].ToString());
                    }
                
                }
                
                RidersList rider = new RidersList();
                rider.App_ID = app_id;
                rider.App_Number = app_number;
                rider.Rider_Type = rider_type;
                rider.ADB = adb;
                rider.TPD = tpd;
                rider.Spouse = spouse;
                rider.Kid1 = kid1;
                rider.Kid2 = kid2;
                rider.Kid3 = kid3;
                rider.Kid4 = kid4;
                myList.Add(rider);
                     
            }
           
            
        //}
        catch (Exception ex)
        { 
            
        }
        return myList;
    }
}
public partial class Pages_Business_added_rider_list : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            DataTable tblUW = DataSetGenerator.Get_Data_Soure("SP_Get_UW_Rider_Add_All", new string[,] { });
           
               // GetDataTable("SP_Get_UW_Rider_Add_By_App","not null");
          //List<RidersList> list=  RidersList.GetRidersList("557D91C2-6621-4E44-8315-EB9E753381E7");
            gvApplication.DataSource = tblUW;
            gvApplication.DataBind();
        }
    }

    protected void gvApplication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "underwriting")
        {
            string app_id = "";
            GridViewRow grow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = grow.RowIndex;
            //Label lbl = (Label)gvApplication.Rows[index].FindControl("lblLevel");
           
                Label lbl = (Label)grow.FindControl("lblAppID");
                app_id = lbl.Text;
            Response.Redirect("underwriting_rider.aspx?rid=" + app_id + "&action=add");
        }
    }
}