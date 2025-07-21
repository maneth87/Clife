using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;


/// <summary>
/// Summary description for SaleAgentWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SaleAgentWebService : System.Web.Services.WebService {

    public SaleAgentWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    
    static List<bl_sale_agent> sale_agent_list = new List<bl_sale_agent> { };

   
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<bl_sale_agent> GetSaleAgents(string sale_agent_name)
    {
        sale_agent_list = da_sale_agent.GetAgentList(sale_agent_name);
      
        return sale_agent_list;
    }

    /// <summary>
    /// Block Sale Agent
    /// </summary>
    #region

    [WebMethod]
    public string GetSaleAgents_Code(string Sale_Agent_ID, int Sale_Agent_Type, string ID_Card)
    {
        string check_duplicate = "";

        check_duplicate = da_sale_agent.GetAgentList_by_sale_Code(Sale_Agent_ID.ToLower(), Sale_Agent_Type, ID_Card);

        return check_duplicate;
    }

    [WebMethod]
    public bool GetSaleAgents_Code_Edit(string Sale_Agent_ID, string ID_Card)
    {
        bool check_duplicate = false;

        check_duplicate = da_sale_agent.Check_Duplicate_Edit(Sale_Agent_ID.ToLower(), ID_Card);

        return check_duplicate;
    }

    #endregion

    #region Delete Sale Agent
    [WebMethod]
    public bool DeleteSaleAgent(string sale_agent_id)
    {
        bool status = false;
        bl_sale_agent sale_agent = new bl_sale_agent();
        if (da_sale_agent.GetSaleAgent_IsUsed_By_Sale_ID(sale_agent_id) == false)
        {
            sale_agent.Sale_Agent_ID = sale_agent_id;
            if (da_sale_agent.DeleteSaleAgent_Record(sale_agent))
            {
                status = true;
            }
            else
            {
                status = false;
            }
        }
        else
        {
            status = false;
        }

        return status;
    }
    #endregion

    /// <summary>
    /// Block Employee
    /// </summary>
    #region
    [WebMethod]
    public string GetEmployee_Code(string Employee_ID, string ID_Card)
    {
        string check_duplicate = "";

        check_duplicate = da_employee.Check_Duplicate_ID_Card_ID(Employee_ID.ToLower(), ID_Card.ToLower(), 1);

        return check_duplicate;
    }

    [WebMethod]
    public string GetEmployee_Code_Edit(string Employee_ID, string ID_Card)
    {
        string check_duplicate = "";

        check_duplicate = da_employee.Check_Duplicate_Card_ID(Employee_ID.ToLower(), ID_Card.ToLower(), 3);

        return check_duplicate;
    }

    #endregion

    /// <summary>
    /// Block Office
    /// </summary>
    #region
    [WebMethod]
    public string GetOffice_Code(string Office_ID, string Detail)
    {
        string check_duplicate = "";

        check_duplicate = da_office.GetOffice_By_Office_ID(Office_ID.ToLower(), Detail.ToLower(), 1);

        return check_duplicate;
    }

    [WebMethod]
    public string GetOffice_Code_Edit(string Office_ID, string Detail)
    {
        string check_duplicate = "";

        check_duplicate = da_office.GetOffice_By_Office_Detail(Office_ID.ToLower(), Detail.ToLower(), 3);

        return check_duplicate;
    }

    #endregion


    /// <summary>
    /// Block Policy
    /// </summary>
    #region
    [WebMethod]
    public string GetPolicy_Status(string Policy_Status_Type_ID, string Policy_Status_Code)
    {
        string check_duplicate = "";

        check_duplicate = da_policy_status_type.GetPolicy_By_Policy_ID(Policy_Status_Type_ID.ToLower(), Policy_Status_Code.ToLower(), 1);

        return check_duplicate;
    }

    [WebMethod]
    public string GetPolicy_Status_Edit(string Policy_Status_Type_ID, string Policy_Status_Code)
    {
        string check_duplicate = "";

        check_duplicate = da_policy_status_type.GetPolicy_By_Policy_Code_Edit(Policy_Status_Type_ID.ToLower(), Policy_Status_Code.ToLower());

        return check_duplicate;
    }

    #endregion

    /// <summary>
    /// Block Relationship
    /// </summary>
    #region
    [WebMethod]
    public string GetRelationship(string Relationship, string Relationship_Khmer)
    {
        string check_duplicate = "";

        check_duplicate = da_relationship.GetRelationship_By_Relationship(Relationship, Relationship_Khmer);

        return check_duplicate;
    }

    [WebMethod]
    public string GetRelationship_Edit(string Relationship, string Relationship_Khmer)
    {
        string check_duplicate = "";

        //check_duplicate = da_relationship.GetRelationship_By_Relationship_Kh(Relationship, Relationship_Kh);

        return check_duplicate;
    }
    #endregion

    /// <summary>
    /// Block Underwriting
    /// </summary>
    #region
    [WebMethod]
    public string GetStatusUnderwriting_Code(string Status_Code, string Detail)
    {
        string check_duplicate = "";

        check_duplicate = da_underwrite_code.GetUnderwrite_By_Status_Code(Status_Code.ToLower(), Detail.ToLower(), 1);

        return check_duplicate;
    }

    [WebMethod]
    public string GetGetStatusUnderwriting_Code_Edit(string Status_Code, string Detail)
    {
        string check_duplicate = "";

        check_duplicate = da_underwrite_code.GetUnderwrite_By_Status_Code_Detail(Status_Code.ToLower(), Detail.ToLower(), 3);

        return check_duplicate;
    }
    #endregion




}
