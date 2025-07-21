using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
public class Channel_Item_Config
{
    public string ID { get; set; }
    public string ProductId { get; set; }
    public string ChannelItemId { get; set; }
    public int MaxPolicyPerLife { get; set; }
    public int Status { get; set; }
    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedRemarks { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedRemarks { get; set; }
    /// <summary>
    /// Read only
    /// </summary>
    public string ChannelItemName { get { return _channelIdName; } }

    public static bool Transection { get { return _tranSection; } }
    public static string Message { get { return _message; } }

    //Private variable
    private static string _message = "";
    private static bool _tranSection=false;
    private string _channelIdName="";

    public Channel_Item_Config() { }
   
   /// <summary>
   /// Created by maneth @19 Oct 2023
   /// </summary>
   /// <param name="channelItemId"></param>
   /// <param name="status">[1: Active, 0: Inactive, -1: all]</param>
   /// <returns></returns>
    public List<Channel_Item_Config> GetChannelItemConfig(string channelItemId, int status)
    { 
        List<Channel_Item_Config> chList=new List<Channel_Item_Config> ();
        try
        {

            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_CHANNEL_ITEM_CONFIG_GET", new string[,] {
            { "@CHANNEL_ITEM_ID", channelItemId},
            { "@IS_ACTIVE",status+""}
            }, "Channel_Item_config=>GetChannelItemConfig(string channelItemId, int status)");
            if (db.RowEffect >= 0)
            {
                try
                {
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow r in tbl.Rows)
                        {
                            chList.Add(new Channel_Item_Config()
                            {
                                ChannelItemId = r["channel_item_id"].ToString(),
                                ProductId = r["product_id"].ToString(),
                                ID = r["ID"].ToString(),
                                MaxPolicyPerLife = Convert.ToInt32(r["max_policy_per_life"].ToString()),
                                Status = Convert.ToInt32(r["status"].ToString()),
                                Remarks = r["remarks"].ToString(),
                                CreatedBy = r["created_by"].ToString(),
                                CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                                CreatedRemarks = r["created_remarks"].ToString(),
                                UpdatedBy = r["updated_by"].ToString(),
                                UpdatedOn = Convert.ToDateTime(r["updated_on"].ToString()),
                                UpdatedRemarks = r["updated_remarks"].ToString(),
                                _channelIdName = r["channel_name"].ToString()
                            });
                        }
                        _tranSection = true;
                    }
                    else
                    {
                        _tranSection = true;
                        _message = "Policy channel item configuration is missing.";
                    }
                }
                catch (Exception ex) {
                    _tranSection = false;
                    _message = ex.Message;
                    chList=new List<Channel_Item_Config>();
                    Log.AddExceptionToLog("Error function [ GetChannelItemConfig(string channelItemId, int status)] in class [Channel_Item_Config], detail:" + ex.Message);
                }
            }
            else
            { 
                //get data error
                _tranSection=false;
                _message=db.Message;
            }
          
        }
        catch(Exception ex)
        {
            _message= ex.Message;
            _tranSection= false;
            chList=new List<Channel_Item_Config>();
            Log.AddExceptionToLog("Error function [ GetChannelItemConfig(string channelItemId, int status)] in class [Channel_Item_Config], detail:" + ex.Message);

        }
        return chList;
    }

    public Channel_Item_Config GetChannelItemConfig(string channelItemId, string productId, int status=1)
    {
        Channel_Item_Config ch = new Channel_Item_Config();
        try
        {

            List<Channel_Item_Config> chList = GetChannelItemConfig(channelItemId, status);
            foreach (Channel_Item_Config obj in chList.Where(_ => _.ProductId == productId))
            {
                ch = obj;
                break;
                _tranSection = true;
            }


        }
        catch (Exception ex)
        {
            _message = ex.Message;
            _tranSection = false;
            ch = null;
            Log.AddExceptionToLog("Error function [ GetChannelItemConfig(string channelItemId, string productId, int status=1)] in class [Channel_Item_Config], detail:" + ex.Message);

        }
        return ch;
    }

    /// <summary>
    /// Get all channel config 
    /// </summary>
    /// <returns></returns>
    public List<Channel_Item_Config> GetChannelItemConfig()
    {
        List<Channel_Item_Config> chList = new List<Channel_Item_Config>();
        try
        {

            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_CHANNEL_ITEM_CONFIG_GET_ALL", new string[,] {
 
            }, "Channel_Item_config=>GetChannelItemConfig()");
            if (db.RowEffect >= 0)
            {
                try
                {
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow r in tbl.Rows)
                        {
                            chList.Add(new Channel_Item_Config()
                            {
                                ChannelItemId = r["channel_item_id"].ToString(),
                                ProductId = r["product_id"].ToString(),
                                ID = r["ID"].ToString(),
                                MaxPolicyPerLife = Convert.ToInt32(r["max_policy_per_life"].ToString()),
                                Status = Convert.ToInt32(r["status"].ToString()),
                                Remarks = r["remarks"].ToString(),
                                CreatedBy = r["created_by"].ToString(),
                                CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                                CreatedRemarks = r["created_remarks"].ToString(),
                                UpdatedBy = r["updated_by"].ToString(),
                                UpdatedOn = Convert.ToDateTime(r["updated_on"].ToString()),
                                UpdatedRemarks = r["updated_remarks"].ToString(),
                                _channelIdName=r["channel_name"].ToString()
                            });
                        }
                        _tranSection = true;
                    }
                    else
                    {
                        _tranSection = true;
                        _message = "Policy channel item configuration is not found in system.";
                    }
                }
                catch (Exception ex)
                {
                    _tranSection = false;
                    _message = ex.Message;
                    chList = new List<Channel_Item_Config>();
                    Log.AddExceptionToLog("Error function [GetChannelItemConfig()] in class [Channel_Item_Config], detail:" + ex.Message);

                }
            }
            else
            {
                //get data error
                _tranSection = false;
                _message = db.Message;
            }

        }
        catch (Exception ex)
        {
            _message = ex.Message;
            _tranSection = false;
            chList = new List<Channel_Item_Config>();
            Log.AddExceptionToLog("Error function [GetChannelItemConfig()] in class [Channel_Item_Config], detail:" + ex.Message);

        }
        return chList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"> Channel item config object</param>
    /// <returns></returns>
    public bool Save(Channel_Item_Config obj)
    {
        bool save = false;
        try
        {
            DB db = new DB();
            save = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_CHANNEL_ITEM_CONFIG_INSERT", new string[,] { 
            {"@ID", obj.ID}, {"@PRODUCT_ID",obj.ProductId}, {"@CHANNEL_ITEM_ID",obj.ChannelItemId}, {"@MAX_POL_PER_LIFE", obj.MaxPolicyPerLife+""}, 
            {"@STATUAT", obj.Status+""}, {"@REMARKS", obj.Remarks}, {"@CREATED_BY", obj.CreatedBy},{"@CREATED_ON", obj.CreatedOn+""}, {"@CREATED_REMARKS", obj.CreatedRemarks}
            }, "Channel_Item_Config=>Save(Channel_Item_Config obj)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Save(Channel_Item_Config obj)] in class [Channel_Item_Config], detail:" + ex.Message);
            save = false;
        }
        return save;
    }
    public bool Update(Channel_Item_Config obj)
    {
        bool update = false;
        try
        {
            DB db = new DB();
            update = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_CHANNEL_ITEM_CONFIG_UPDATE", new string[,] { 
            {"@ID", obj.ID}, {"@PRODUCT_ID",obj.ProductId}, {"@CHANNEL_ITEM_ID",obj.ChannelItemId}, {"@MAX_POL_PER_LIFE", obj.MaxPolicyPerLife+""}, 
            {"@STATUAT", obj.Status+""}, {"@REMARKS", obj.Remarks}, {"@UPDATED_BY", obj.UpdatedBy},{"@UPDATED_ON", obj.UpdatedOn+""}, {"@UPDATED_REMARKS", obj.UpdatedRemarks}
            }, "Channel_Item_Config=>Update(Channel_Item_Config obj)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Update(Channel_Item_Config obj)] in class [Channel_Item_Config], detail:" + ex.Message);
            update = false;
        }
        return update;
    }
    public bool Delete(string id)
    {
        bool delete = false;
        try
        {
            DB db = new DB();
            delete = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_CHANNEL_ITEM_CONFIG_DELETE", new string[,] { 
            {"@ID", id}
            }, "Channel_Item_Config=>Delete(string id)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Delete(string id)] in class [Channel_Item_Config], detail:" + ex.Message);
            delete = false;
        }
        return delete;
    }
    public string GetNewId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_POLICY_CHANNEL_ITEM_CONFIG" }, { "FIELD", "ID" } });
    }
}
