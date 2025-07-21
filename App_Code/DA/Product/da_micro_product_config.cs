using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_product_config
/// </summary>
public class da_micro_product_config
{
    public da_micro_product_config()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class ProductConfig
    {
        /// <summary>
        /// Get New SO proudct Config list
        /// </summary>
        /// <returns></returns>
        public static List<bl_micro_product_config> GetProductMicroProductSO()
        {
            List<bl_micro_product_config> listPro = new List<bl_micro_product_config>();
            try
            {
                DataTable tb = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_CONFIG_GET", new string[,] { }, "da_micro_product_config=>ProductConfig=>GetProductMicroProductSO()");
                foreach (DataRow r in tb.Rows)
                {
                    listPro.Add(new bl_micro_product_config()
                    {
                        Id = r["id"].ToString(),
                        Product_ID = r["product_id"].ToString(),
                        En_Abbr = r["en_abbr"].ToString(),
                        En_Title = r["en_title"].ToString(),
                        Kh_Abbr = r["kh_abbr"].ToString(),
                        Kh_Title = r["kh_title"].ToString(),
                        Age_Min = Convert.ToInt32(r["age_min"].ToString()),
                        Age_Max = Convert.ToInt32(r["age_max"].ToString()),
                        Sum_Min = Convert.ToDouble(r["sum_min"].ToString()),
                        Sum_Max = Convert.ToDouble(r["sum_max"].ToString()),
                        Remarks = r["remarks"].ToString(),
                        Status = (r["status"].ToString() == "1" ? true : false),
                        RiderProductID = r["PRODUCT_RIDER_ID"].ToString(),
                        BasicSumAssuredRange = r["BASIC_SUM_ASSURED_RANGE"].ToString() == "" ? new double[] { 0 } : Array.ConvertAll(r["BASIC_SUM_ASSURED_RANGE"].ToString().Split(','), new Converter<string, double>(Double.Parse)),
                        RiderSumAssuredRange = r["RIDER_SUM_ASSURED_RANGE"].ToString() == "" ? new double[] { 0 } : Array.ConvertAll(r["RIDER_SUM_ASSURED_RANGE"].ToString().Split(','), new Converter<string, double>(Double.Parse)),
                        PayMode = Array.ConvertAll(r["Pay_Mode"].ToString().Split(','), new Converter<string, Int32>(Int32.Parse)),
                        BusinessType = r["business_type"].ToString(),
                        AllowRefer = r["allow_refer"].ToString() == "1" ? true : false,
                        IsRequiredRider = r["Is_Required_rider"].ToString() == "1" ? true : false,
                        IsValidateReferralId = r["is_validate_referral_id"].ToString() == "1" ? true : false,
                        ChannelItemId = r["channel_item_id"].ToString(),
                        MarketingName = r["marketing_name"].ToString(),
                        CreatedOn=Convert.ToDateTime(r["created_on"].ToString()),
                        PayPeriodType = r["pay_period_type"].ToString() == "" ? new string[] { } :r["pay_period_type"].ToString().Split(';').ToArray(),
                        CoverPeriodType = r["cover_period_type"].ToString() == "" ? new string[] { } : r["cover_period_type"].ToString().Split(';').ToArray()
                    });

                }
            }
            catch (Exception ex)
            {
                listPro = new List<bl_micro_product_config>();
                Log.AddExceptionToLog("Error function [GetProductMicroProductSO()] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return listPro;

        }
        /// <summary>
        /// Get Miro product config by product id
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<bl_micro_product_config> GetProductMicroProductSO(string productID)
        {
            List<bl_micro_product_config> listPro = new List<bl_micro_product_config>();
            try
            {
                DataTable tb = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_CONFIG_GET_LIST_BY_PRODUCT_ID", new string[,] {
            {"@PRODUCT_ID", productID}
            }, "da_micro_product_config=>ProductConfig=>GetProductMicroProductSO(string productID)");
                foreach (DataRow r in tb.Rows)
                {
                    listPro.Add(new bl_micro_product_config()
                    {

                        Id = r["id"].ToString(),
                        Product_ID = r["product_id"].ToString(),
                        En_Abbr = r["en_abbr"].ToString(),
                        En_Title = r["en_title"].ToString(),
                        Kh_Abbr = r["kh_abbr"].ToString(),
                        Kh_Title = r["kh_title"].ToString(),
                        Age_Min = Convert.ToInt32(r["age_min"].ToString()),
                        Age_Max = Convert.ToInt32(r["age_max"].ToString()),
                        Sum_Min = Convert.ToDouble(r["sum_min"].ToString()),
                        Sum_Max = Convert.ToDouble(r["sum_max"].ToString()),
                        Remarks = r["remarks"].ToString(),
                        Status = (r["status"].ToString() == "1" ? true : false),
                        RiderProductID = r["PRODUCT_RIDER_ID"].ToString(),
                        BasicSumAssuredRange = Array.ConvertAll(r["BASIC_SUM_ASSURED_RANGE"].ToString().Split(','), new Converter<string, double>(Double.Parse)),
                        RiderSumAssuredRange = Array.ConvertAll(r["RIDER_SUM_ASSURED_RANGE"].ToString().Split(','), new Converter<string, double>(Double.Parse)),
                        PayMode = Array.ConvertAll(r["Pay_Mode"].ToString().Split(','), new Converter<string, Int32>(Int32.Parse)),
                        BusinessType = r["business_type"].ToString(),
                        AllowRefer = r["allow_refer"].ToString() == "1" ? true : false,
                        IsRequiredRider = r["Is_Required_rider"].ToString() == "1" ? true : false,
                        IsValidateReferralId = r["is_validate_referral_id"].ToString() == "1" ? true : false,
                        ChannelItemId = r["channel_item_id"].ToString(),
                        MarketingName = r["marketing_name"].ToString(),
                        PayPeriodType = r["pay_period_type"].ToString() == "" ? new string[] { } : r["pay_period_type"].ToString().Split(';').ToArray(),
                        CoverPeriodType = r["cover_period_type"].ToString() == "" ? new string[] { } : r["cover_period_type"].ToString().Split(';').ToArray()
                    });

                }
            }
            catch (Exception ex)
            {
                listPro = new List<bl_micro_product_config>();
                Log.AddExceptionToLog("Error function [GetProductMicroProductSO(string productID)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return listPro;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID">Main product id, such as SO</param>
        /// <returns></returns>
        public static List<bl_micro_product_rider> GetProductMicroProductRider(string productID)
        {
            List<bl_micro_product_rider> listPro = new List<bl_micro_product_rider>();
            try
            {
                DataTable tb = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_CONFIG_GET_PRODUCT_RIDER", new string[,] {
            {"@PRODUCT_ID", productID}}, "da_micro_product_config=>ProductConfig=>GetProductMicroProductRider(string productID)");
                foreach (DataRow row in tb.Rows)
                {
                    listPro.Add(new bl_micro_product_rider()
                    {
                        PRODUCT_MICRO_RIDER_ID = row["PRODUCT_MICRO_RIDER_ID"].ToString(),
                        PRODUCT_ID = row["PRODUCT_ID"].ToString(),
                        EN_TITLE = row["EN_TITLE"].ToString(),
                        EN_ABBR = row["EN_ABBR"].ToString(),
                        KH_TITLE = row["KH_TITLE"].ToString(),
                        AGE_MIN = Convert.ToInt32(row["AGE_MIN"].ToString()),
                        AGE_MAX = Convert.ToInt32(row["AGE_MAX"].ToString()),
                        SUM_ASSURE_MIN = Convert.ToInt32(row["SUM_MIN"].ToString()),
                        SUM_ASSURE_MIX = Convert.ToInt32(row["SUM_MAX"].ToString()),
                        REMARKS = row["REMARKS"].ToString()

                    });
                }
            }
            catch (Exception ex)
            {
                listPro = new List<bl_micro_product_rider>();
                Log.AddExceptionToLog("Error function [GetProductMicroProductRider(string productID)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return listPro;

        }

        //public static bl_micro_product_discount_config GetProductRiderDiscount(string productID)
        //{
        //    bl_micro_product_discount_config pro = new bl_micro_product_discount_config();
        //    try
        //    {
        //        DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_RIDER_DISCOUNT_CONFIG_GET", new string[,] { 
        //       {"@PRODUCT_ID", productID}
        //        }, "da_micro_product_config=>GetProductRiderDiscount(string productID)");

        //        if (tbl.Rows.Count > 0)
        //        {
        //            var r = tbl.Rows[0];

        //            pro.ID = r["id"].ToString();
        //            pro.ProductID = r["product_id"].ToString();
        //            pro.DiscountAmount = Convert.ToDouble(r["discount_amount"].ToString());
        //            pro.EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString());
        //            pro.ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString());
        //            pro.Status = r["status"].ToString() == "1" ? true : false;
        //            pro.CreatedBy = r["created_by"].ToString();
        //            pro.CreatedOn = Convert.ToDateTime(r["created_on"].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        pro = new bl_micro_product_discount_config();
        //        Log.AddExceptionToLog("Error function [GetProductRiderDiscount(string productID)] in class [da_micro_product_config], detail:" + ex.Message + "=>" + ex.StackTrace);
        //    }

        //    return pro;
        //}

        public static bl_micro_product_config GetProductMicroProduct(string productID)
        {
            bl_micro_product_config pro = new bl_micro_product_config();

            try
            {
                List<bl_micro_product_config> listPro = new List<bl_micro_product_config>();
                listPro = GetProductMicroProductSO();

                foreach (bl_micro_product_config obj in listPro.Where(_ => _.Product_ID == productID))
                {
                    pro = obj;
                    break;

                }

            }
            catch (Exception ex)
            {
                pro = new bl_micro_product_config();
                Log.AddExceptionToLog("Error function [GetProductMicroProduct(string productID)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return pro;

        }

        public static List<bl_micro_product_config> GetMicroProductConfigListByChannelItemId(string channelItemId)
        {
            List<bl_micro_product_config> pro = new List<bl_micro_product_config>();

            try
            {
                List<bl_micro_product_config> listPro = new List<bl_micro_product_config>();
                listPro = GetProductMicroProductSO();

                foreach (bl_micro_product_config obj in listPro.Where(_ => _.ChannelItemId == channelItemId))
                {
                    pro.Add(obj);

                }

            }
            catch (Exception ex)
            {
                pro = new List<bl_micro_product_config>();
                Log.AddExceptionToLog("Error function [GetMicroProductConfigListByChannelItemId(string channelItemId)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return pro;

        }
        /// <summary>
        /// Get active product config by channel item id
        /// </summary>
        /// <param name="channelItemId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static List<bl_micro_product_config> GetMicroProductConfigListByChannelItemId(string channelItemId, bool isActive)
        {
            List<bl_micro_product_config> pro = new List<bl_micro_product_config>();

            try
            {
                List<bl_micro_product_config> listPro = new List<bl_micro_product_config>();
                listPro = GetProductMicroProductSO();

                foreach (bl_micro_product_config obj in listPro.Where(_ => _.ChannelItemId == channelItemId && _.Status == isActive))
                {
                    pro.Add(obj);

                }

            }
            catch (Exception ex)
            {
                pro = new List<bl_micro_product_config>();
                Log.AddExceptionToLog("Error function [GetMicroProductConfigListByChannelItemId(string channelItemId, bool isActive)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return pro;

        }
        public static bool Save(bl_micro_product_config obj)
        {
            bool result = false;
            try
            {
                DB db = new DB();
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_CONFIG_INSERT", new string[,] {
            {"@ID",obj.Id} 
            , {"@PRODUCT_ID", obj.Product_ID}
            , {"@PRODUCT_RIDER_ID", obj.RiderProductID}
            , {"@BASIC_SUM_ASSURED_RANGE", obj.BasicSumAssured}
            , {"@RIDER_SUM_ASSURED_RANGE", obj.RiderSumAssured}
            , {"@PAY_MODE", obj.PayModeString}
            , {"@BUSINESS_TYPE", obj.BusinessType}
            , {"@MARKETING_NAME", obj.MarketingName}
            , {"@ALLOW_REFER", obj.AllowRefer==true ? "1":"0"}
            , {"@STATUS", obj.Status== true ? "1":"0"}
            , {"@REMARKS", obj.Remarks}
            , {"@CREATED_BY", obj.CreatedBy}
            , {"@CREATED_ON", obj.CreatedOn+""}
            , {"@CHANNEL_ITEM_ID", obj.ChannelItemId}
             , {"@IS_REQUIRED_RIDER", obj.IsRequiredRider==true ? "1":"0"}
             , {"@IS_VALIDATE_REFERRAL_ID", obj.IsValidateReferralId==true ? "1":"0"},
             {"@COVER_PERIOD_TYPE", obj.CoverPeriodTypeString},
             {"@PAY_PERIOD_TYPE",obj.PayPeriodTypeString}
            }, "da_micro_product_config=>Save(bl_micro_product_config obj)");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [Save(bl_micro_product_config obj)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return result;
        }

        /// <summary>
        /// Update product config by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Update(bl_micro_product_config obj)
        {
            bool result = false;
            try
            {
                DB db = new DB();
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_CONFIG_UPDATE", new string[,] {
            {"@ID",obj.Id} 
            , {"@PRODUCT_ID", obj.Product_ID}
            , {"@PRODUCT_RIDER_ID", obj.RiderProductID}
            , {"@BASIC_SUM_ASSURED_RANGE", obj.BasicSumAssured}
            , {"@RIDER_SUM_ASSURED_RANGE", obj.RiderSumAssured}
            , {"@PAY_MODE", obj.PayModeString}
            , {"@BUSINESS_TYPE", obj.BusinessType}
            , {"@MARKETING_NAME", obj.MarketingName}
            , {"@ALLOW_REFER", obj.AllowRefer==true ? "1":"0"}
            , {"@STATUS", obj.Status== true ? "1":"0"}
            , {"@REMARKS", obj.Remarks}
            , {"@UPDATED_BY", obj.UpdatedBy}
            , {"@UPDATED_ON", obj.UpdatedOn+""}
             , {"@CHANNEL_ITEM_ID", obj.ChannelItemId}
              , {"@IS_REQUIRED_RIDER", obj.IsRequiredRider==true ? "1":"0"}
               , {"@IS_VALIDATE_REFERRAL_ID", obj.IsValidateReferralId==true ? "1":"0"},
                {"@COVER_PERIOD_TYPE", obj.CoverPeriodTypeString},
             {"@PAY_PERIOD_TYPE",obj.PayPeriodTypeString}
            }, "da_micro_product_config=>Update(bl_micro_product_config obj)");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [Update(bl_micro_product_config obj)] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return result;
        }
        /// <summary>
        /// Get all channel item from micro product config
        /// </summary>
        /// <returns></returns>
        public static List<bl_channel_item> GetChannelItemList()
        {
            List<bl_channel_item> chList = new List<bl_channel_item>();
            try
            {
                DataTable tbl = new DataTable();
                tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "CT_MICRO_PRODUCT_CONFIG_GET_CHANNEL_ITEM_LIST", new string[,] { }, "da_micro_product_config=>ProductConfig=>GetChannelItemList()");
                foreach (DataRow r in tbl.Rows)
                {
                    chList.Add(new bl_channel_item() { Channel_Item_ID = r["channel_item_id"].ToString(), Channel_Name = r["channel_name"].ToString() });
                }
            }
            catch (Exception ex)
            {
                chList = new List<bl_channel_item>();
                Log.AddExceptionToLog("Error function [GetChannelItemList()] in class [da_micro_product_config=>ProductConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return chList;
        }
    }

    public class DiscountConfig
    {
        public static bl_micro_product_discount_config GetProductDiscount(string productID, string productRiderID, double basicSumAssured, double riderSumAssured, string clientType)
        {
            bl_micro_product_discount_config pro = new bl_micro_product_discount_config();
            try
            {
                DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_GET", new string[,] { 
           {"@PRODUCT_ID", productID},{"@PRODUCT_RIDER_ID", productRiderID}, {"@basic_sum_assured", basicSumAssured+""},{"@rider_sum_assured", riderSumAssured+""},
           {"@client_type",clientType}
            }, "da_micro_product_config=>DiscountConfig=>GetProductDiscount(string productID)");

                if (tbl.Rows.Count > 0)
                {
                    var r = tbl.Rows[0];

                    pro.ID = r["id"].ToString();
                    pro.ProductID = r["product_id"].ToString();
                    pro.EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString());
                    pro.ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString());
                    pro.Status = r["status"].ToString() == "1" ? true : false;
                    pro.CreatedBy = r["created_by"].ToString();
                    pro.CreatedOn = Convert.ToDateTime(r["created_on"].ToString());
                    pro.ProductRiderID = r["Product_Rider_ID"].ToString();
                    pro.BasicDiscountAmount = Convert.ToDouble(r["Basic_discount_amount"].ToString());
                    pro.RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString());
                    pro.BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString());
                    pro.RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString());
                    pro.ClientType = r["client_type"].ToString();
                }
            }
            catch (Exception ex)
            {
                pro = new bl_micro_product_discount_config();
                Log.AddExceptionToLog("Error function [GetProductDiscount(string productID)] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }

            return pro;
        }
        /// <summary>
        /// Get Basic product discount 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
        public static bl_micro_product_discount_config GetProductBasicDiscount(string productID, double sumAssured, string clientType)
        {
            bl_micro_product_discount_config pro = new bl_micro_product_discount_config();
            try
            {
                DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_BASIC_GET", new string[,] { 
           {"@PRODUCT_ID", productID},{"@sum_assured", sumAssured+""}, {"@client_type",clientType}
            }, "da_micro_product_config=>DiscountConfig=>GetProductBasicDiscount(string productID,  double sumAssured)");

                if (tbl.Rows.Count > 0)
                {
                    var r = tbl.Rows[0];

                    pro.ID = r["id"].ToString();
                    pro.ProductID = r["product_id"].ToString();
                    pro.EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString());
                    pro.ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString());
                    pro.Status = r["status"].ToString() == "1" ? true : false;
                    pro.CreatedBy = r["created_by"].ToString();
                    pro.CreatedOn = Convert.ToDateTime(r["created_on"].ToString());
                    pro.BasicDiscountAmount = Convert.ToDouble(r["Basic_discount_amount"].ToString());
                    pro.BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString());
                    pro.ClientType = r["client_type"].ToString();
                }
            }
            catch (Exception ex)
            {
                pro = new bl_micro_product_discount_config();
                Log.AddExceptionToLog("Error function [GetProductBasicDiscount(string productID,  double sumAssured)] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }

            return pro;
        }
        /// <summary>
        /// Get Rider product discount
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
        public static bl_micro_product_discount_config GetProductRiderDiscount(string productID, double sumAssured , string clientType)
        {
            bl_micro_product_discount_config pro = new bl_micro_product_discount_config();
            try
            {
                DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_RIDER_GET", new string[,] { 
           {"@RIDER_PRODUCT_ID", productID},{"@sum_assured", sumAssured+""}, {"@client_type",clientType}
            }, "da_micro_product_config=>DiscountConfig=>GetProductRiderDiscount(string productID, double sumAssured)");

                if (tbl.Rows.Count > 0)
                {
                    var r = tbl.Rows[0];

                    pro.ID = r["id"].ToString();

                    pro.EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString());
                    pro.ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString());
                    pro.Status = r["status"].ToString() == "1" ? true : false;
                    pro.CreatedBy = r["created_by"].ToString();
                    pro.CreatedOn = Convert.ToDateTime(r["created_on"].ToString());
                    pro.ProductRiderID = r["Product_Rider_ID"].ToString();
                    pro.RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString());
                    pro.RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString());
                    pro.ClientType = r["client_type"].ToString();
                }
            }
            catch (Exception ex)
            {
                pro = new bl_micro_product_discount_config();
                Log.AddExceptionToLog("Error function [GetProductRiderDiscount(string productID, double sumAssured)] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);
            }

            return pro;
        }
        public static List<bl_micro_product_discount_config> GetDiscountConfigList()
        {
            List<bl_micro_product_discount_config> disList = new List<bl_micro_product_discount_config>();
            try
            {
                DataTable tbl = new DataTable();
                tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_GET_LIST", new string[,] { }, "da_micro_product_config=>DiscountConfig=>GetDiscountConfigList()");
                foreach (DataRow r in tbl.Rows)
                {
                    disList.Add(new bl_micro_product_discount_config()
                    {
                        ID = r["id"].ToString(),
                        ProductID = r["product_id"].ToString(),
                        ProductRiderID = r["product_rider_id"].ToString(),
                        BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString()),
                        BasicDiscountAmount = Convert.ToDouble(r["basic_discount_amount"].ToString()),
                        RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString()),
                        RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                        Status = r["status"].ToString() == "1" ? true : false,
                        Remarks = r["remarks"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ClientType = r["client_type"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                disList = new List<bl_micro_product_discount_config>();
                Log.AddExceptionToLog("Error function [GetDiscountConfigList()] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return disList;
        }
        /// <summary>
        /// Get product discount config by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<bl_micro_product_discount_config> GetDiscountConfigList(string productId)
        {
            List<bl_micro_product_discount_config> disList = new List<bl_micro_product_discount_config>();
            try
            {
                DataTable tbl = new DataTable();
                tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_GET_LIST_BY_PRODUCT_ID", new string[,] { 
                {"@product_id",productId}
                }, "da_micro_product_config=>DiscountConfig=>GetDiscountConfigList(string productId)");
                foreach (DataRow r in tbl.Rows)
                {
                    disList.Add(new bl_micro_product_discount_config()
                    {
                        ID = r["id"].ToString(),
                        ProductID = r["product_id"].ToString(),
                        ProductRiderID = r["product_rider_id"].ToString(),
                        BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString()),
                        BasicDiscountAmount = Convert.ToDouble(r["basic_discount_amount"].ToString()),
                        RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString()),
                        RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                        Status = r["status"].ToString() == "1" ? true : false,
                        Remarks = r["remarks"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ClientType = r["client_type"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                disList = new List<bl_micro_product_discount_config>();
                Log.AddExceptionToLog("Error function [GetDiscountConfigList(string productId)] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return disList;
        }

        public static bool Save(bl_micro_product_discount_config obj)
        {
            bool result = false;
            try
            {
                result = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_INSERT", new string[,] {
                {"@ID", obj.ID}, {"@PRODUCT_ID", obj.ProductID}, {"@PRODUCT_RIDER_ID", obj.ProductRiderID}, {"@BASIC_DISCOUNT_AMOUNT", obj.BasicDiscountAmount+""}, 
                {"@RIDER_DISCOUNT_AMOUNT", obj.RiderDiscountAmount+""}, {"@BASIC_SUM_ASSURED", obj.BasicSumAssured+""}, {"@RIDER_SUM_ASSURED", obj.RiderSumAssured+""}, 
                {"@EFFECTIVE_DATE", obj.EffectiveDate+""}, {"@EXPIRY_DATE", obj.ExpiryDate+""},{"@STATUS", obj.Status==true ? "1":"0"}, 
                {"@CHANNEL_ITEM_ID", obj.ChannelItemId}, {"@REMARKS", obj.Remarks}, {"@CREATED_BY", obj.CreatedBy}, {"@CREATED_ON", obj.CreatedOn+""},
                {"@client_type", obj.ClientType}
                
                }, "da_micro_product_config=>DiscountConfig=>Save(bl_micro_product_discount_config obj)");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [Save(bl_micro_product_discount_config obj)] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return result;

        }
        /// <summary>
        /// update micro prouct discount config by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Update(bl_micro_product_discount_config obj)
        {
            bool result = false;
            try
            {
                result = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_DISCOUNT_CONFIG_UPDATE", new string[,] {
                {"@ID", obj.ID}, {"@PRODUCT_ID", obj.ProductID}, {"@PRODUCT_RIDER_ID", obj.ProductRiderID}, {"@BASIC_DISCOUNT_AMOUNT", obj.BasicDiscountAmount+""}, 
                {"@RIDER_DISCOUNT_AMOUNT", obj.RiderDiscountAmount+""}, {"@BASIC_SUM_ASSURED", obj.BasicSumAssured+""}, {"@RIDER_SUM_ASSURED", obj.RiderSumAssured+""}, 
                {"@EFFECTIVE_DATE", obj.EffectiveDate+""}, {"@EXPIRY_DATE", obj.ExpiryDate+""},{"@STATUS", obj.Status==true ? "1":"0"}, 
                {"@CHANNEL_ITEM_ID", obj.ChannelItemId}, {"@REMARKS", obj.Remarks}, {"@UPDATED_BY", obj.UpdatedBy}, {"@UPDATED_ON", obj.UpdatedOn+""},
                {"@client_type", obj.ClientType}
                }, "da_micro_product_config=>DiscountConfig=>Update(bl_micro_product_discount_config obj)");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [Update(bl_micro_product_discount_config obj)] in class [da_micro_product_config=>DiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return result;

        }
    }

    public class OverrideDiscountConfig
    {
        public static List<bl_override_discount_config> GetOverriderDiscountList()
        {
            List<bl_override_discount_config> disList = new List<bl_override_discount_config>();
            try
            {
                DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_OVERRIDE__DISCOUNT_CONFIG_GET_LIST", new string[,] { 
                }, "GetOverriderDiscountList()=>da_micro_product_config=>OverrideDiscountConfig");
                foreach (DataRow r in tbl.Rows)
                {
                    disList.Add(new bl_override_discount_config()
                    {
                        Id = r["id"].ToString(),
                        CustomerId = r["customer_id"].ToString(),
                        PolicyId = r["policy_id"].ToString(),
                        ProductId = r["product_id"].ToString(),
                        ProductRiderId = r["product_rider_id"].ToString(),
                        BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString()),
                        RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString()),
                        BasicDiscountAmount = Convert.ToDouble(r["basic_discount_amount"].ToString()),
                        RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                        Status = r["status"].ToString() == "1" ? true : false,
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        ChannelName = r["channel_name"].ToString(),
                        CustomerName = r["customer_name"].ToString(),
                        CustomerNumber = r["customer_number"].ToString(),
                        PolicyNumber = r["policy_number"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                disList = new List<bl_override_discount_config>();
                Log.AddExceptionToLog("Error function [GetOverriderDiscountList()] in class [da_micro_product_config=>OverrideDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return disList;
        }
        /// <summary>
        /// Get override discout list by condition type
        /// </summary>
        /// <param name="colName">Column name</param>
        /// <param name="val">value to search</param>
        /// <returns></returns>
        public static List<bl_override_discount_config> GetOverriderDiscountList(string colName, string val)
        {
            List<bl_override_discount_config> disList = new List<bl_override_discount_config>();
            try
            {
                DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_OVERRIDE__DISCOUNT_CONFIG_GET_LIST_BY_CODI", new string[,] { 
                    {"@COl_NAME",colName},{"@VAL",val}
                }, "GetOverriderDiscountList()=>da_micro_product_config=>OverrideDiscountConfig");
                foreach (DataRow r in tbl.Rows)
                {
                    disList.Add(new bl_override_discount_config()
                    {
                        Id = r["id"].ToString(),
                        CustomerId = r["customer_id"].ToString(),
                        PolicyId = r["policy_id"].ToString(),
                        ProductId = r["product_id"].ToString(),
                        ProductRiderId = r["product_rider_id"].ToString(),
                        BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString()),
                        RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString()),
                        BasicDiscountAmount = Convert.ToDouble(r["basic_discount_amount"].ToString()),
                        RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                        Status = r["status"].ToString() == "1" ? true : false,
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        ChannelName = r["channel_name"].ToString(),
                        CustomerName = r["customer_name"].ToString(),
                        CustomerNumber = r["customer_number"].ToString(),
                        PolicyNumber = r["policy_number"].ToString()

                    });
                }
            }
            catch (Exception ex)
            {
                disList = new List<bl_override_discount_config>();
                Log.AddExceptionToLog("Error function [GetOverriderDiscountList()] in class [da_micro_product_config=>OverrideDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return disList;
        }
        public static bl_override_discount_config GetOverriderDiscount(string customerId, string channelItemId, string productId)
        {
            bl_override_discount_config obj = new bl_override_discount_config();
            try
            {
                DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_OVERRIDE__DISCOUNT_CONFIG_GET_ACTIVE", new string[,] { 
                    {"@CUSTOMER_ID", customerId},{"@CHANNEL_ITEM_ID",channelItemId},{"@PRODUCT_ID",productId}
                }, "GetOverriderDiscount(string customerId, string channelItemId, string productId)=>da_micro_product_config=>OverrideDiscountConfig");

                if (tbl.Rows.Count > 0)
                {
                    var r = tbl.Rows[0];
                    obj = (new bl_override_discount_config()
                    {
                        Id = r["id"].ToString(),
                        CustomerId = r["customer_id"].ToString(),
                        PolicyId = r["policy_id"].ToString(),
                        ProductId = r["product_id"].ToString(),
                        ProductRiderId = r["product_rider_id"].ToString(),
                        BasicSumAssured = Convert.ToDouble(r["basic_sum_assured"].ToString()),
                        RiderSumAssured = Convert.ToDouble(r["rider_sum_assured"].ToString()),
                        BasicDiscountAmount = Convert.ToDouble(r["basic_discount_amount"].ToString()),
                        RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                        Status = r["status"].ToString() == "1" ? true : false,
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString())
                        
                    });
                }

            }
            catch (Exception ex)
            {
                obj = new bl_override_discount_config();
                Log.AddExceptionToLog("Error function [GetOverriderDiscount(string customerId, string channelItemId, string productId)] in class [da_micro_product_config=>OverrideDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return obj;
        }
        public static bool Save(bl_override_discount_config obj)
        {
            bool result = false;
            try
            {
                result = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_OVERRIDE__DISCOUNT_CONFIG_INSERT", new string[,] { 
                    {"@ID", obj.Id}, {"@CUSTOMER_ID", obj.CustomerId}, {"@POLICY_ID", obj.PolicyId}, {"@PRODUCT_ID", obj.ProductId}, {"@PRODUCT_RIDER_ID", obj.ProductRiderId}, 
                    {"@BASIC_SUM_ASSURED", obj.BasicSumAssured+""}, {"@RIDER_SUM_ASSURED", obj.RiderSumAssured+""}, {"@BASIC_DISCOUNT_AMOUNT", obj.BasicDiscountAmount+""}, 
                    {"@RIDER_DISCOUNT_AMOUNT", obj.RiderDiscountAmount+""}, {"@EFFECTIVE_DATE", obj.EffectiveDate+""}, {"@EXPIRY_DATE", obj.ExpiryDate+""}, {"@STATUS", obj.Status==true ? "1":"0"},
                    {"@CREATED_BY", obj.CreatedBy}, {"@CREATED_ON", obj.CreatedOn+""}, {"@CHANNEL_ITEM_ID", obj.ChannelItemId}, {"@REMARKS", obj.Remarks}
                }, "Save(bl_override_discount_config obj)=>da_micro_product_config=>OverrideDiscountConfig");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [Save(bl_override_discount_config obj)] in class [da_micro_product_config=>OverrideDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return result;
        }

        /// <summary>
        /// Update override discount config by id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Update(bl_override_discount_config obj)
        {
            bool result = false;
            try
            {
                result = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PRODUCT_OVERRIDE__DISCOUNT_CONFIG_UPDATE", new string[,] { 
                    {"@ID", obj.Id}, {"@CUSTOMER_ID", obj.CustomerId}, {"@POLICY_ID", obj.PolicyId}, {"@PRODUCT_ID", obj.ProductId}, {"@PRODUCT_RIDER_ID", obj.ProductRiderId}, 
                    {"@BASIC_SUM_ASSURED", obj.BasicSumAssured+""}, {"@RIDER_SUM_ASSURED", obj.RiderSumAssured+""}, {"@BASIC_DISCOUNT_AMOUNT", obj.BasicDiscountAmount+""}, 
                    {"@RIDER_DISCOUNT_AMOUNT", obj.RiderDiscountAmount+""}, {"@EFFECTIVE_DATE", obj.EffectiveDate+""}, {"@EXPIRY_DATE", obj.ExpiryDate+""}, {"@STATUS", obj.Status==true ? "1":"0"},
                    {"@UPDATED_BY", obj.UpdatedBy}, {"@UPDATED_ON", obj.UpdatedOn+""}, {"@CHANNEL_ITEM_ID", obj.ChannelItemId}, {"@REMARKS", obj.Remarks}
                }, "Update(bl_override_discount_config obj)=>da_micro_product_config=>OverrideDiscountConfig");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [Update(bl_override_discount_config obj)] in class [da_micro_product_config=>OverrideDiscountConfig], detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return result;
        }
    }
}