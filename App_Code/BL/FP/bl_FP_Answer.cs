using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_FP_Answer
/// </summary>
public class bl_FP_Answer
{
    #region //Local variable and properties
    private string answer_id;

    public string Answer_id
    {
        get { return answer_id; }
        set { answer_id = value; }
    }
    private string question_id;

    public string Question_id
    {
        get { return question_id; }
        set { question_id = value; }
    }
    private string app_id;

    public string App_id
    {
        get { return app_id; }
        set { app_id = value; }
    }
    private string answer;

    public string Answer
    {
        get { return answer; }
        set { answer = value; }
    }
    private string customer_id;

    public string Customer_id
    {
        get { return customer_id; }
        set { customer_id = value; }
    }
    private string remarks;

    public string Remarks
    {
        get { return remarks; }
        set { remarks = value; }
    }

    #endregion

    public bl_FP_Answer()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public bl_FP_Answer(string answer_id, string question_id, string app_id, string answer, string customer_id, string remarks){
        Answer_id = this.answer_id;
        Question_id = this.question_id;
        App_id = this.app_id;
        Answer = this.answer;
        Customer_id = this.customer_id;
        Remarks = this.remarks;
    }
}