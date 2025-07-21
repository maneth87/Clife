
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Pages_Business_load_new_certificate : System.Web.UI.Page
{
    
  protected void Page_Load(object sender, EventArgs e)
  {
    try
    {
      string polId = string.Join(",", (IEnumerable<string>) this.Session["POL_ID_PRINT"]);
      if (this.Request.QueryString.Count == 2)
      {
        string printPolInsurance = this.Request.QueryString["printPolInsurance"].ToString();
        string polType = this.Request.QueryString["policyType"].ToString();
        using (HttpClient httpClient = new HttpClient())
        {
          httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          string requestUri = AppConfiguration.GetCertificateURL()+"?policyId="+polId+"&policyType="+polType+"&printPolInsurance="+printPolInsurance;
          HttpResponseMessage result = httpClient.GetAsync(requestUri).Result;
          result.EnsureSuccessStatusCode();
          Pages_Business_load_new_certificate.ResponeCertificate responeCertificate = JsonConvert.DeserializeObject<Pages_Business_load_new_certificate.ResponeCertificate>(result.Content.ReadAsStringAsync().Result);
          if (responeCertificate == null)
            throw new Exception("Failed to parse response.");
          responeCertificate.Status = result.StatusCode.ToString();
          responeCertificate.StatusCode = (int) result.StatusCode;
          if (responeCertificate.Certificate == null)
            return;
          this.Context.Response.Buffer = true;
          this.Context.Response.Clear();
          this.Context.Response.ContentType = "application/pdf";
          this.Context.Response.BinaryWrite(responeCertificate.Certificate);
          this.Context.Response.Flush();
          this.Context.Response.End();
        }
      }
      else
        this.Response.Write("Invalid URL.");
    }
    catch (Exception ex)
    {
      this.Response.Write("Load certificate error, detail:" + ex.Message);
    }
  }
    
  public class ResponeCertificate
  {
    public int StatusCode { get; set; }

    public string Status { get; set; }

    public byte[] Certificate { get; set; }

    public string Message { get; set; }
  }
}