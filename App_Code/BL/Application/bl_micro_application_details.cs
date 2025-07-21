
using System;
using System.Collections.Generic;


public class bl_micro_application_details
{
  public string PolicyId { get; set; }

  public string PolicyNumber { get; set; }

  public string PolicyStatus { get; set; }

  public int Age { get; set; }

  public DateTime IssueDate { get; set; }

  public DateTime EffectiveDate { get; set; }

  public double CollectedPremium { get; set; }

  public string PaymentReferenceNo { get; set; }

  public string OfficeCode { get; set; }

  public string OfficeName { get; set; }

  public bl_micro_application_customer Customer { get; set; }

  public bl_micro_application Application { get; set; }

  public bl_micro_application_insurance Insurance { get; set; }

  public bl_micro_application_insurance_rider Rider { get; set; }

  public List<bl_micro_application_beneficiary> Beneficiaries { get; set; }

  public bl_micro_application_beneficiary.PrimaryBeneciary PrimaryBeneficiary { get; set; }

  public bl_micro_application_questionaire Questionaire { get; set; }

  public List<bl_micro_application_details.SubApplication> SubApplications { get; set; }

  public class SubApplication
  {
    public string ApplicationNumber { get; set; }

    public string ApplicationId { get; set; }

    public double BasicAmount { get; set; }

    public double RiderAmount { get; set; }

    public double TotalAmount { get; set; }

    public string ClientType { get; set; }

    public double SumAssure { get; set; }
  }
}
