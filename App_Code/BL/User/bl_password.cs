using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for bl_password
/// </summary>
public class bl_password
{
   // private string PwdTemplate = @"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){8,}";

    /// <summary>
    /// Password requires at least 1 lower case character, 1 upper case character, 1 number, 1 special character and must be at least 8 characters
    /// </summary>
   // private string PwdTemplate = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,})";
	public bl_password()
	{
		//
		// TODO: Add constructor logic here
		//
        //string para = ConfigurationManager.AppSettings["PASSWORD-POLICY"].ToString();
        string para = AppConfiguration.GetPasswordPolicy();
        string[] arr = para.Split(';');
        MinPasswordLength = Convert.ToInt32(arr[0].Split(':')[1].ToString());
        MinNumberic = Convert.ToInt32(arr[1].Split(':')[1].ToString());
        MinCharacter = Convert.ToInt32(arr[2].Split(':')[1].ToString());
        MinSpecialCharacter = Convert.ToInt32(arr[3].Split(':')[1].ToString());
        MinUpperCase = Convert.ToInt32(arr[4].Split(':')[1].ToString());
        PasswordAge = Convert.ToInt32(arr[5].Split(':')[1].ToString());
        MaxInvalidPasswordAttempts = Convert.ToInt32(arr[6].Split(':')[1].ToString());
        PasswordTemplate = arr[7].Split(':')[1].ToString();
	}
    public int MinPasswordLength{get;set;}
    public int MinNumberic{get;set;}
    public int MinCharacter{get;set;}
    public int MinSpecialCharacter{get;set;}
    public int MinUpperCase{get;set;}
    public int PasswordAge { get; set; }
    public int MaxInvalidPasswordAttempts { get; set; }
    public string PasswordTemplate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    public  bool PasswordIsValid(string Password, out string Message)
    {
        bool valid = false;
        
        Message = "";
        //if (Password.Length< MinPasswordLength)
        //{
        //    Message = "Password";
        //}
        if (Regex.IsMatch(Password, PasswordTemplate))
        {
            Message = "Matched";
            valid = true;
        }
        else
        {
           Message = "Password must contain lower case character, upper case character, number, special character and be at least " + MinPasswordLength + " characters";

           // Message = "Password not meet requirement";
            valid = false;
        }
        return valid;
    }




}