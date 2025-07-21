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
public partial class Pages_Business_load_new_application_form : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
  {
    string str = string.Join(",", (IEnumerable<string>) this.Session["APP_ID_PRINT"]);
    using (HttpClient httpClient = new HttpClient())
    {
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      string requestUri = AppConfiguration.GetApplicationFormURL()+"?applicationId="+str+"&applicationType=IND";
      HttpResponseMessage result = httpClient.GetAsync(requestUri).Result;
      result.EnsureSuccessStatusCode();
      Pages_Business_load_new_application_form.ResponeApplicationForm responeApplicationForm = JsonConvert.DeserializeObject<Pages_Business_load_new_application_form.ResponeApplicationForm>(result.Content.ReadAsStringAsync().Result);
      if (responeApplicationForm == null)
        throw new Exception("Failed to parse response.");
      responeApplicationForm.Status = result.StatusCode.ToString();
      responeApplicationForm.StatusCode = (int) result.StatusCode;
      if (responeApplicationForm.ApplicationForm == null)
        return;
      this.Context.Response.Buffer = true;
      this.Context.Response.Clear();
      this.Context.Response.ContentType = "application/pdf";
      this.Context.Response.BinaryWrite(responeApplicationForm.ApplicationForm);
      this.Context.Response.Flush();
      this.Context.Response.End();
    }
  }

  public class ResponeApplicationForm
  {
    public int StatusCode { get; set; }

    public string Status { get; set; }

    public byte[] ApplicationForm { get; set; }

    public string Message { get; set; }
  }
}