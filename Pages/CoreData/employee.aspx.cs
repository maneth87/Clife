using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CoreData_employee : System.Web.UI.Page
{
    bl_employee employee = new bl_employee();
    int check_employee_id = 0, check_card_id = 0;
    string userid, user_name;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    void ClearText()
    {
        txtEmployeeCodeModal.Text = "";
        txtIDCardModal.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtDateBirth.Text = "";
        txtNote.Text = "";
    }

    /// <summary>
    /// Insert
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;


        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        employee.Employee_ID = txtEmployeeCodeModal.Text.ToUpper();
        employee.ID_Card = txtIDCardModal.Text.ToUpper();
        employee.ID_Type = int.Parse(ddlIDType.SelectedValue);
        employee.First_Name=txtFirstName.Text.ToUpper();
        employee.Last_Name=txtLastName.Text.ToUpper();
        employee.Gender =int.Parse(ddlGender.SelectedValue);
        employee.Birth_Date =DateTime.Parse(txtDateBirth.Text,dtfi);
        employee.Country_ID = ddlNationality.Text;
        employee.Office_ID = ddlOffice.Text;
        employee.Created_On = DateTime.Now;
        employee.Created_By = user_name;
        employee.Created_Note = txtNote.Text;

        //if (da_employee.Check_Duplicate_ID_Card_ID(employee,0) == true) /// 0, check Employee ID
        //{
        //    check_employee_id = 1;
        //}

        //if (da_employee.Check_Duplicate_ID_Card_ID (employee,1)== true) /// 1, check Card ID
        //{
        //    check_card_id = 1;
        //}

        //if (check_employee_id == 1 && check_card_id == 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Employee Code (" + employee.Employee_ID + ") has already existed. Please check it again.')", true);
        //}

        //if (check_employee_id == 0 && check_card_id == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Card ID (" + employee.ID_Card + ") has already existed. Please check it again.')", true);
        //}

        //if (check_employee_id == 1 && check_card_id == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Employee Code (" + employee.Employee_ID + ") and The Card ID (" + employee.ID_Card + ") have already existed, Please check them again.')", true);
        //}

        //if (check_employee_id == 0 && check_card_id == 0)
        //{
        //    da_employee.InsertEmployee(employee);
        //}

        da_employee.InsertEmployee(employee);

        ClearText();

        GvEmployee.DataBind();
    }

    /// <summary>
    /// Update
    /// </summary>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;


        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        employee.Old_Employee_ID = hdOldEmployeeCode.Value.ToUpper();
        employee.Employee_ID = hdOldEmployeeCode.Value.ToUpper(); //txtEditEmployeeCode.Text;
        employee.ID_Card = txtEditIDCard.Text.ToUpper();
        employee.ID_Type = int.Parse(ddlEditIDType.SelectedValue);
        employee.First_Name = txtEditFirstName.Text.ToUpper();
        employee.Last_Name = txtEditLastName.Text.ToUpper();
        employee.Gender = int.Parse(ddlEditGender.SelectedValue);
        employee.Birth_Date = DateTime.Parse(txtEditBirth_Date.Text,dtfi);
        employee.Country_ID = ddlEditNationality.Text;
        employee.Office_ID = ddlEditOfficeCode.Text;
        employee.Created_On = DateTime.Now;
        employee.Created_By = user_name;
        employee.Created_Note = txtEditNote.Text;

        //if (da_employee.Check_Duplicate_ID_Card_ID(employee, 2) == true) /// 1, check Card ID
        //{
        //    check_card_id = 1;
        //}

        //if (check_card_id == 0)
        //{
        //    da_employee.UpdateEmployee(employee);
        //}

        da_employee.UpdateEmployee(employee);

        GvEmployee.DataBind();
    }

    /// <summary>
    /// Delete Employee By Employee ID
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        employee.Office_ID = hdfDeleteEmployeeID.Value;

        da_employee.DeleteEmployee_by_Employee_ID(employee);

        GvEmployee.DataBind();
       
    }

    /// <summary>
    /// Cancel Employee by Employee ID
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        bl_employee_cancel employee_cancel = new bl_employee_cancel();
        employee_cancel.Employee_ID = hdfCancelEmployee.Value;
        employee_cancel.Created_Note = txtCancelNote.Text;
        employee_cancel.Created_By = user_name;
        employee_cancel.Created_On = DateTime.Now;

        da_employee_cancle.InsertEmployee(employee_cancel);

        GvEmployee.DataBind();
    }
}