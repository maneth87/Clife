using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

public class ChannelItemUserConfig
{
    #region  Properties
    public string Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string ChannelItemId { get; set; }
    public string ChannelName { get; set; }
    public string ChannelNameKh { get; set; }

    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedRemarks { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedRemarks { get; set; }

    public bool Transection { get { return _tranSection; } }
    public string Message { get { return _message; } }
    //Private variable
    private static string _message = "";
    private static bool _tranSection = false;
    #endregion
    public ChannelItemUserConfig() { }
    public ChannelItemUserConfig(string id, string userName, string channelItemId, string remarks, string createdby, DateTime createdOn, string createdRemarks, string updatedBy, DateTime updatedOn, string updatedRemarks)
    {
        Id = id; ChannelItemId = channelItemId; Remarks = remarks; CreatedBy = createdby; CreatedOn = createdOn; CreatedRemarks = createdRemarks; UpdatedBy = updatedBy; UpdatedOn = updatedOn; UpdatedRemarks = updatedRemarks;
    }

    public bool Save()
    {
        try
        {
            DB db = new DB();
            _tranSection = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_INSERT", new string[,] {
            {"@ID", Id}, {"@USER_NAME",UserName}, {"@CHANNEL_ITEM_ID", ChannelItemId}, {"@REMARKS", Remarks},{"@CREATED_BY", CreatedBy}, {"@CREATED_ON",CreatedOn+""}, {"@CREATED_REMARKS",CreatedRemarks}
            }, "ChannelItemUserConfig=>Save()");

          
            if (_tranSection)
            {
                _message = "Saved channel item user link successfully.";
            }
            else
            {
                _message = "Saved channel item user link is getting error..";
            }
            return _tranSection;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Save()] in class [ChannelItemUserConfig], detail: " + ex.Message);
            _message = "Save channel item user link is getting error.";
            _tranSection = false;
            return _tranSection;
        }
    }
    public bool Update()
    {
        try
        {
            DB db = new DB();
            _tranSection = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_UPDATE", new string[,] {
            {"@ID", Id}, {"@USER_NAME",UserName}, {"@CHANNEL_ITEM_ID", ChannelItemId}, {"@REMARKS", Remarks},{"@UPDATED_BY", UpdatedBy}, {"@UPDATED_ON",UpdatedOn+""}, {"@UPDATED_REMARKS",UpdatedRemarks}
            }, "ChannelItemUserConfig=>Update()");


            if (_tranSection)
            {
                _message = "Updated channel item user link successfully.";
            }
            else
            {
                _message = "Updated channel item user link is getting error..";
            }
            return _tranSection;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Update()] in class [ChannelItemUserConfig], detail: " + ex.Message);
            _message = "Updated channel item user link is getting error.";
            _tranSection = false;
            return _tranSection;
        }
    }

    /// <summary>
    /// Delete obj id
    /// </summary>
    /// <returns></returns>
    public bool Delete(string id)
    {
        try
        {
            DB db = new DB();
            _tranSection = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_DELETE", new string[,] {
            {"@ID", Id}
            }, "ChannelItemUserConfig=>Delete(string id)");


            if (_tranSection)
            {
                _message = "Deleted channel item user link successfully.";
            }
            else
            {
                _message = "Delete channel item user link is getting error..";
            }
            return _tranSection;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Delete(string id)] in class [ChannelItemUserConfig], detail: " + ex.Message);
            _message = "Updated channel item user link is getting error.";
            _tranSection = false;
            return _tranSection;
        }
    }
    public ChannelItemUserConfig GetByUserId(string userId)
    {
        ChannelItemUserConfig obj = new ChannelItemUserConfig();
        try
        {

            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_GET_BY_USER_ID", new string[,] {
            { "@user_id", userId},
            }, "ChannelItemUserConfig=>GetByUserId(string userId)");
            if (db.RowEffect >= 0)
            {
                try
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        obj = new ChannelItemUserConfig()
                        {
                            ChannelItemId = r["channel_item_id"].ToString(),
                            UserId = r["user_id"].ToString(),
                            UserName = r["User_name"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            ChannelNameKh = r["channel_name_kh"].ToString()
                        };
                        break;


                    }
                    _tranSection = true;
                }
                catch (Exception ex)
                {
                    _tranSection = false;
                    _message = ex.Message;
                    obj = new ChannelItemUserConfig();
                    Log.AddExceptionToLog("Error function [GetByUserId(string userId)] in class [ChannelItemUserConfig], detail: " + ex.Message);

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
            obj = new ChannelItemUserConfig();
            Log.AddExceptionToLog("Error function [GetByUserId(string userId)] in class [ChannelItemUserConfig], detail: " + ex.Message);

        }
        return obj;
    }
    /// <summary>
    /// Get Channel Item  user Config by channel item id.
    /// </summary>
    /// <param name="channelItemId"></param>
    /// <returns></returns>
    public ChannelItemUserConfig GetByChannelItemId(string channelItemId)
    {
        ChannelItemUserConfig obj = new ChannelItemUserConfig();
        try
        {

            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_GET_BY_CHANNEL_ITEM_ID", new string[,] {
            { "@CHANNEL_ITEM_ID", channelItemId},
            }, "ChannelItemUserConfig=>GetByChannelItemId(string channelItemId)");
            if (db.RowEffect >= 0)
            {
                try
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        obj = new ChannelItemUserConfig()
                        {
                            ChannelItemId = r["channel_item_id"].ToString(),
                            UserId = r["user_id"].ToString(),
                            UserName = r["User_name"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            ChannelNameKh = r["channel_name_kh"].ToString()
                        };
                        break;


                    }
                    _tranSection = true;
                }
                catch (Exception ex)
                {
                    _tranSection = false;
                    _message = ex.Message;
                    obj = new ChannelItemUserConfig();
                    Log.AddExceptionToLog("Error function [GetByChannelItemId(string channelItemId)] in class [ChannelItemUserConfig], detail: " + ex.Message);

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
            obj = new ChannelItemUserConfig();
            Log.AddExceptionToLog("Error function [GetByChannelItemId(string channelItemId)] in class [ChannelItemUserConfig], detail: " + ex.Message);

        }
        return obj;
    }
    public ChannelItemUserConfig GetByUserName(string userName)
    {
        ChannelItemUserConfig obj = new ChannelItemUserConfig();
        try
        {

            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_GET_BY_USER_NAME", new string[,] {
            { "@user_name", userName},
            }, "ChannelItemUserConfig=>GetByUserName(string userName)");
            if (db.RowEffect >= 0)
            {
                try
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        obj = new ChannelItemUserConfig()
                        {
                            ChannelItemId = r["channel_item_id"].ToString(),
                            UserId = r["user_id"].ToString(),
                            UserName = r["User_name"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            ChannelNameKh = r["channel_name_kh"].ToString(),
                            Id=r["id"].ToString(),
                            Remarks=r["remarks"].ToString(),
                            CreatedRemarks=r["created_remarks"].ToString(),
                            UpdatedRemarks=r["updated_remarks"].ToString()
                        };
                        break;


                    }
                    _tranSection = true;
                }
                catch (Exception ex)
                {
                    _tranSection = false;
                    _message = ex.Message;
                    obj = new ChannelItemUserConfig();
                    Log.AddExceptionToLog("Error function [GetByUserName(string userName)] in class [ChannelItemUserConfig], detail: " + ex.Message);

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
            obj = new ChannelItemUserConfig();
            Log.AddExceptionToLog("Error function [GetByUserName(string userName)] in class [ChannelItemUserConfig], detail: " + ex.Message);

        }
        return obj;
    }
    public List<ChannelItemUserConfig> GetList()
    {
        List<ChannelItemUserConfig> obj = new List<ChannelItemUserConfig>();
        try
        {

            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG_GET_LIST", new string[,] {
          
            }, "ChannelItemUserConfig=>GetList()");
            if (db.RowEffect >= 0)
            {
                try
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        obj.Add(new ChannelItemUserConfig()
                        {
                            Id = r["id"].ToString(),
                            ChannelItemId = r["channel_item_id"].ToString(),
                            UserId = r["user_id"].ToString(),
                            UserName = r["User_name"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            ChannelNameKh = r["channel_name_kh"].ToString(),
                            Remarks = r["remarks"].ToString(),
                            CreatedRemarks = r["Created_Remarks"].ToString(),
                            UpdatedRemarks = r["updated_remarks"].ToString()
                        });

                    }
                    _tranSection = true;
                }
                catch (Exception ex)
                {
                    _tranSection = false;
                    _message = ex.Message;
                    obj = new List<ChannelItemUserConfig>();
                    Log.AddExceptionToLog("Error function [GetList()] in class [ChannelItemUserConfig], detail: " + ex.Message);

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
            obj = new List<ChannelItemUserConfig>();
            Log.AddExceptionToLog("Error function [GetList()] in class [ChannelItemUserConfig], detail: " + ex.Message);

        }
        return obj;
    }

    public string GenerateId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_CHANNEL_ITEM_LINK_USER_CONFIG" }, { "FIELD", "ID" } });
    }
}
