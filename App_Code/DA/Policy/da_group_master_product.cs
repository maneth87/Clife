using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_group_master_product
/// </summary>
public class da_group_master_product
{
	public da_group_master_product()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="product_id">[Skip filter put '']</param>
    /// <param name="group_code">[Skip filter put '']</param>
    /// <returns></returns>
    public static List<bl_group_master_product> GetGroupMasterProductList(string product_id, string channel_item_id)
    {
        List<bl_group_master_product> obj_arr = new List<bl_group_master_product>();
        try
        {
            DataTable tbl = new DB().GetData( AppConfiguration.GetConnectionString(), "SP_GET_GROUP_MASTER_PRODUCT", new string[,] { 
            { "@PRODUCT_ID", product_id }, { "@CHANNEL_ITEM_ID", channel_item_id }
            }, "da_group_master_product=>GetGroupMasterProductList(string product_id, string channel_item_id)"
                );
            foreach(DataRow row in tbl.Rows)
            {
                obj_arr.Add(new bl_group_master_product(){ GroupMasterID= row["group_master_id"].ToString(), 
                                                            GroupCode = row["group_code"].ToString(), 
                                                            ProductID = row["product_id"].ToString(), 
                                                            Remarks=row["remarks"].ToString(),
                                                            CreatedBy = row["created_by"].ToString(), 
                                                            CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()), 
                                                            UpdatedBy= row["updated_by"].ToString(), 
                                                            UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString()),
                EffectiveDate=Convert.ToDateTime(row["effective_Date"].ToString())});
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetGroupMasterProductList] in class [da_group_master_product], Detail: " + ex.Message);
        }
        return obj_arr;
    }
    public static bl_group_master_product GetGroupMasterProductByChannelItem(string channel_item_id)
    {
        bl_group_master_product obj = new bl_group_master_product();
        try
        {
            DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MASTER_PRODUCT_GET_BY_CHANNEL_ITEM_ID", new string[,] { { "@CHANNEL_ITEM_ID", channel_item_id } }
                , "da_group_master_product=>GetGroupMasterProductByChannelItem(string channel_item_id)");
            foreach (DataRow row in tbl.Rows)
            {
                obj=new bl_group_master_product()
                {
                    GroupMasterID = row["group_master_id"].ToString(),
                    ChannelItemID=row["channel_item_id"].ToString(),
                    GroupCode = row["group_code"].ToString(),
                    ProductID = row["product_id"].ToString(),
                    Remarks = row["remarks"].ToString(),
                    CreatedBy = row["created_by"].ToString(),
                    CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                    UpdatedBy = row["updated_by"].ToString(),
                    UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["effective_Date"].ToString())
                };
            }
        }
        catch (Exception ex)
        {
            obj = new bl_group_master_product();
            Log.AddExceptionToLog("Error function [GetGroupMasterProductByChannelItem(string channel_item_id)] in class [da_group_master_product], Detail: " + ex.Message);
        }
        return obj;
    }
    public static bl_group_master_product GetGroupMasterProductByGroupCode(string groupCode)
    {
        bl_group_master_product obj = new bl_group_master_product();
        try
        {
            DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MASTER_PRODUCT_GET_BY_G_CODE", new string[,] { { "@group_code", groupCode } },
                "da_group_master_product=>GetGroupMasterProductByGroupCode(string groupCode)"
                );
            foreach (DataRow row in tbl.Rows)
            {
                obj = new bl_group_master_product()
                {
                    GroupMasterID = row["group_master_id"].ToString(),
                    ChannelItemID = row["channel_item_id"].ToString(),
                    GroupCode = row["group_code"].ToString(),
                    ProductID = row["product_id"].ToString(),
                    Remarks = row["remarks"].ToString(),
                    CreatedBy = row["created_by"].ToString(),
                    CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                    UpdatedBy = row["updated_by"].ToString(),
                    UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["effective_Date"].ToString())
                };
            }
        }
        catch (Exception ex)
        {
            obj = new bl_group_master_product();
            Log.AddExceptionToLog("Error function [GetGroupMasterProductByGroupCode(string groupCode)] in class [da_group_master_product], Detail: " + ex.Message);
        }
        return obj;
    }
}