using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_master_list
/// </summary>
public class da_master_list
{
	public da_master_list()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static List<bl_master_list> GetMasterList(string masterCode)
    {
        List<bl_master_list> mList = new List<bl_master_list>();
        try
        {
            DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MASTER_LIST_GET_ACTIVE_BY_MASTER_CODE", new string[,] {
                    {"@master_list_code", masterCode}
                    }, "da_master=>da_master_relation=>GetMasterList(string masterCode)");
            foreach (DataRow dr in tbl.Rows)
            {
                mList.Add(new bl_master_list()
                {
                    Id = Convert.ToInt32(dr["id"].ToString()),
                    OrderNo = Convert.ToInt32(dr["order_no"].ToString()),
                    MasterListCode = dr["master_list_code"].ToString(),
                    Code = dr["code"].ToString(),
                    DescEn = dr["desc_en"].ToString(),
                    DescKh = dr["desc_kh"].ToString(),
                    Status = Convert.ToInt32(dr["status"].ToString())
                });
            }
        }
        catch (Exception ex)
        {
            mList = null;
            Log.AddExceptionToLog("Error function [GetMasterList(string masterCode)] in class [da_master_list], detail: " + ex.Message);
        }
        return mList;
    }

    public static class da_beneficiary_relation
    {
        public static List<bl_master_list.bl_relation> GetBeneficiaryRelationList()
        {
            List<bl_master_list.bl_relation> beneficiaryRelationList = new List<bl_master_list.bl_relation>();
            try
            {
                foreach (DataRow row in (InternalDataCollectionBase)new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_BENEFICIARY_RELATION_GET", new string[0, 0], "da_master=>da_beneficiary_relation=>GetBeneficiaryRelationList()").Rows)
                    beneficiaryRelationList.Add(new bl_master_list.bl_relation()
                    {
                        Id = row["id"].ToString(),
                        RelationEn = row["relation_en"].ToString(),
                        RelationKh = row["relation_kh"].ToString(),
                        GenderCode = Convert.ToInt32(row["gender_code"].ToString())
                    });
            }
            catch (Exception ex)
            {
                beneficiaryRelationList = (List<bl_master_list.bl_relation>)null;
                Log.AddExceptionToLog("Error function [GetBeneficiaryRelationList()] in class [da_master_list.da_beneficiary_relation], detail: " + ex.Message);
            }
            return beneficiaryRelationList;
        }

        public static List<bl_master_list.bl_relation> GetBeneficiaryRelationList(int gender)
        {
            try
            {
                return da_master_list.da_beneficiary_relation.GetBeneficiaryRelationList().Where<bl_master_list.bl_relation>((System.Func<bl_master_list.bl_relation, bool>)(_ => _.GenderCode == gender)).ToList<bl_master_list.bl_relation>();
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetBeneficiaryRelationList(int gender)] in class [da_master_list.da_beneficiary_relation], detail: " + ex.Message);
                return (List<bl_master_list.bl_relation>)null;
            }
        }
    }
}