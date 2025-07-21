using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Web.Security;
public partial class Pages_CI_Terminate_so_upload : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    string user_name = "";
    string product_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (Request.QueryString.Count > 0)
        {
            product_id = Request.QueryString["pro_type"];
        }
        else
        {
            product_id = "SO,CI";
        }
        if (!Page.IsPostBack)
        {
            btnTerminate.Enabled = false;

        }
        user_name = Membership.GetUser().UserName;
    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        gv_policy.DataSource = null;
        gv_policy.DataBind();

        try
        {
            if ((TerminateFile.PostedFile != null) && !string.IsNullOrEmpty(TerminateFile.PostedFile.FileName))
            {

                string save_path = "~/Upload/";
                string file_name = Path.GetFileName(TerminateFile.PostedFile.FileName);
                string extension = Path.GetExtension(file_name);
                file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                string file_path = save_path + file_name;
                int row_number = 0;

                TerminateFile.SaveAs(Server.MapPath(file_path));//save file 

                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "SELECT * FROM [Upload$]";
                DataTable my_data = new DataTable();
                my_data = my_excel.GetData();

                row_number = my_data.Rows.Count;

                #region //All IF SO
                string filter = "";
                //get all S.O policies
                DataTable tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", "", product_id);

                filter += filter.Trim() == "" ? " policy_status in ('IF','LAP')" : " and policy_status in ('IF','LAP')";//get only IF status
                DataTable tbl_filter = tbl.Clone();//clone propertity from table tbl such columns name
                //Add new column Remarks
                tbl_filter.Columns.Add("Remarks");

                DataRow row_filter;

                foreach (DataRow row_upload in my_data.Rows)
                {
                    foreach (DataRow row in tbl.Select(filter))//filter data in table tbl
                    {
                        if (row["policy_number"].ToString().Trim() == row_upload["policy_number"].ToString().Trim())
                        {
                            row_filter = tbl_filter.NewRow();
                            //copy data from tbl to tbl_filter base on codition
                            for (int col = 0; col < tbl_filter.Columns.Count-1; col++)
                            {
                                row_filter[col] = row[col].ToString();
                            }

                            row_filter["Remarks"] = row_upload["Remarks"];

                            tbl_filter.Rows.Add(row_filter);
                            break;
                        }
                    }

                }

                if (tbl_filter.Rows.Count > 0)
                {
                    gv_policy.DataSource = tbl_filter;
                    gv_policy.DataBind();


                    btnTerminate.Enabled = true;
                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries policy for terminate by uploading excel file [Total record(s):", tbl_filter.Rows.Count, "]."));
                }
                #endregion //ALL IF SO

                //delete file
                try
                {
                    File.Delete(Server.MapPath(file_path));
                }
                catch (Exception ex)
                {
                    Log.AddExceptionToLog("Delete file [" + file_path + "] error, Detail:" + ex.Message);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Please upload policies for terminating.');", true);

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Terminate policy [UPLOAD DATA], detail:"+ex.Message, user_name);
        }
    }
    protected void btnTerminate_Click(object sender, EventArgs e)
    {
       
        int success = 0;
        int fail = 0;
        if (gv_policy.Rows.Count > 0)
        {
            
            for (int i = 0; i < gv_policy.Rows.Count; i++)
            {
                try
                {
                    Label lblPolicy = (Label)gv_policy.Rows[i].FindControl("lblPOlicyID");
                    Label lblRemarks = (Label)gv_policy.Rows[i].FindControl("lblRemarks");

                    if (da_ci.Policy.TerminatePolicy(lblPolicy.Text.Trim(), user_name, DateTime.Now, lblRemarks.Text.Trim()))
                    {
                        success += 1;
                    }
                    else
                    {
                        fail += 1;
                    }
                }
                catch (Exception ex)
                {
                    fail += 1;
                    Log.AddExceptionToLog("Terminate policy [TERMINATE], detail:" + ex.Message, user_name);
                }
            }
            if (success > 0 || fail > 0)
            {
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE, string.Concat("User updates policy status to terminate, [total success:", success, " total fail:", fail, "].")); 
            }

            if (success > 0 && fail == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Policy is terminated successfully.');", true);
                gv_policy.DataSource = null;
                gv_policy.DataBind();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Some policies cannot be terminated.');", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('There is no policy to terminate.');", true);

        }
       
    }
}