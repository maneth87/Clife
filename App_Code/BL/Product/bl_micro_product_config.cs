
using System;
using System.Collections.Generic;
using System.Data;

[Serializable]
public class bl_micro_product_config : bl_product
{
    public bl_micro_product_config()
    {
        if (this.Plan_Code == null)
            this.Plan_Code = "";
    }

    public string Id { get; set; }

    public string RiderProductID { get; set; }

    public bool Status { get; set; }

    public double[] BasicSumAssuredRange { get; set; }

    public double[] RiderSumAssuredRange { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int[] PayMode { get; set; }

    public string BusinessType { get; set; }

    public bool AllowRefer { get; set; }

    public bool IsRequiredRider { get; set; }

    public string MarketingName { get; set; }

    public string ChannelItemId { get; set; }

    public bool IsValidateReferralId { get; set; }

    public string ChannelName { get { return da_channel.GetChannelItemNameByID(this.ChannelItemId); } }

    public string BasicSumAssured
    {
        get { return string.Join<double>(",", (IEnumerable<double>)this.BasicSumAssuredRange); }
    }

    public string RiderSumAssured
    {
        get { return string.Join<double>(",", (IEnumerable<double>)this.RiderSumAssuredRange); }
    }

    public string PayModeString { get { return string.Join<int>(",", (IEnumerable<int>)this.PayMode); } }

    public string[] PayPeriodType { get; set; }

    public string[] CoverPeriodType { get; set; }

    public string PayPeriodTypeString { get { return string.Join(";", this.PayPeriodType); } }

    public string CoverPeriodTypeString { get { return string.Join(";", this.CoverPeriodType); } }

    public string ProductType { get { return this._getProductType(); } }

    private string _getProductType()
    {
        if (this.Product_ID == null)
            return "";
        DataTable data = new DB().GetData(AppConfiguration.GetConnectionString(), "SELECT a.Product_Type_ID, Product_Type FROM Ct_Product_Type a inner join ct_product b on a.Product_Type_ID=b.Product_Type_ID WHERE Product_ID='" + this.Product_ID + "';");
        return data.Rows.Count > 0 ? data.Rows[0]["Product_Type"].ToString().ToUpper() : "";
    }

    public enum PERIOD_TYPE
    {
        Y,
        M,
        D,
        H,
    }

    public enum PRODUCT_TYPE
    {
        ORDINARY,
        MORTGAGE,
        SAVINGS,
        GTLI,
        MICRO,
        MICRO_LOAN,
    }

    public enum BusinussTypeOption
    {
        INDIVIDUAL,
        BANCA_REFERRAL,
        BUNDLE,
        BANCA_COOPERATE,
    }

    public new class NAME
    {
        public static string Product_ID { get { return "Product_ID"; } }

        public static string Plan_Block { get { return "Plan_Block"; } }

        public static string ChannelItemId { get { return "ChannelItemId"; } }

        public static string ChannelName { get { return "ChannelName"; } }

        public static string MarketingName { get { return "MarketingName"; } }

        public static string ProductRemarks { get { return "ProductIdRemarks"; } }
    }
}
