using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for ChannelWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ChannelWebService : System.Web.Services.WebService {

    public ChannelWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<bl_channel_item> GetChannelItem(string channel_id)
    {
        return da_channel.GetChannelItemListByChannel(channel_id);
    }

    [WebMethod]
    public List<bl_channel_location> GetChannelLocation(string channel_item_id)
    {
        return da_channel.GetChannelLocationListByChannelItemID(channel_item_id);
    }

    [WebMethod]
    public List<bl_channel_location> GetChannelLocationID(string channel_sub_id)
    {
        return da_channel.GetChannelLocationIDByChannelSubID(channel_sub_id);
    }
    
}
