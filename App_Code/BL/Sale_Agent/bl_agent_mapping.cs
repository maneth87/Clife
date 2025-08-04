using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_agent_mapping
/// </summary>
public class bl_agent_mapping
{
	public bl_agent_mapping()
	{
		//
		// TODO: Add constructor logic here
		//
        
        
	}
    /// <summary>
    /// Constructor for Save or Update
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userName"></param>
    /// <param name="saleAgentId"></param>
    /// <param name="channelLocationId"></param>
    /// <param name="status"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdOn"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedOn"></param>
    /// <param name="remarks"></param>
    public bl_agent_mapping(string id , string userName, string saleAgentId, string channelLocationId, int status, string createdBy, DateTime createdOn, string updatedBy , DateTime updatedOn, string remarks)
    {
        Id = id; UserName = userName; SaleAgentId = saleAgentId; ChannelLocationId = channelLocationId; Status = status; CreatedBy = createdBy; CreatedOn = createdOn; UpdatedBy = updatedBy; UpdatedOn = updatedOn; Remarks = remarks;
    }
    public string Id { get; set; }
    public string UserName{get;set;}
    public string SaleAgentId{get;set;}
    public string ChannelLocationId { get; set; }
    public int Status { get; set; }
    /// <summary>
    /// Required for insert
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// Required for insert
    /// </summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>
    /// Required for update
    /// </summary>
    public string UpdatedBy { get; set; }
    /// <summary>
    /// Required for update
    /// </summary>
    public DateTime UpdatedOn { get; set; }
    public string Remarks { get; set; }

    /*Readonly Properties*/
    //public string SaleAgentName { get { return GetSaleAgentName(); } }
    //public string ChannelItemName { get { return GetChannelItemName(); } }
    //public string ChannelItemId { get { return _channelItemId; } }
   
    //public string OfficeCode { get { return _officeCode; } }
    //public string OfficeName { get { return _officeName; } }


    //private string _saleAgentName = "";
    //private string _officeName = "";
    //private string _officeCode = "";
    //private string _channelItemId = "";
    //private string _channelItemName = "";

    //private string GetSaleAgentName()
    //{
    //    if (SaleAgentId != null)
    //    {
    //        _saleAgentName = da_sale_agent.GetSaleAgentMicro(SaleAgentId).FullNameEn;
    //        return _saleAgentName;
    //    }
    //    else { return ""; }

    //}
    //private string GetChannelItemName()
    //{
    //    if (ChannelLocationId != null)
    //    {
    //        bl_channel_location obj = da_channel.GetChannelLocationByChannelLocationID(ChannelLocationId);
    //        _officeName = obj.Office_Name;
    //        _officeCode = obj.Office_Code;
    //        _channelItemId = obj.Channel_Item_ID;
    //        _channelItemName = da_channel.GetChannelItemNameByID(obj.Channel_Item_ID);
    //        return _channelItemName;
    //    }
    //    else { return ""; }
    //}
    /// <summary>
    /// Read property
    /// </summary>
    public string SaleAgentName { get; set; }

    /// <summary>
    /// Read property
    /// </summary>
    public string ChannelItemName { get; set; }
    /// <summary>
    /// Read property
    /// </summary>
    public string ChannelItemId { get; set; }
    /// <summary>
    /// Read property
    /// </summary>
    public string OfficeCode { get; set; }
    /// <summary>
    /// Read property
    /// </summary>
    public string OfficeName { get; set; }
    public string ProductId { get; set; }
}