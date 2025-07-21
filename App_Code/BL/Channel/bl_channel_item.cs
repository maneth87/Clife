
using System;

public class bl_channel_item
{
  private string _Channel_Item_ID;
  private string _Channel_Name;
  private string _Channel_HQ_Address;
  private string _Created_By;
  private DateTime _Created_On;
  private string _Created_Note;
  private int _Status;
  private string _Channel_Sub_ID;

  public string Channel_Item_ID
  {
    get {return this._Channel_Item_ID;}
    set { this._Channel_Item_ID = value;}
  }

  public string Channel_Name
  {
    get {return this._Channel_Name;}
    set { this._Channel_Name = value;}
  }

  public string Channel_HQ_Address
  {
    get {return this._Channel_HQ_Address;}
    set { this._Channel_HQ_Address = value;}
  }

  public int Status
  {
    get {return this._Status;}
    set { this._Status = value;}
  }

  public string Channel_Sub_ID
  {
    get {return this._Channel_Sub_ID;}
    set { this._Channel_Sub_ID = value;}
  }

  public string Created_By
  {
    get {return this._Created_By;}
    set { this._Created_By = value;}
  }

  public DateTime Created_On
  {
    get {return this._Created_On;}
    set { this._Created_On = value;}
  }

  public string Channel_HQ_Address_KH { get; set; }

  public string Channel_Name_KH { get; set; }

  public class NAME
  {
    public static string Channel_Item_ID {get{return "Channel_Item_ID";}}

    public static string Channel_Name {get{return "Channel_Name";}}

    public static string Channel_HQ_Address {get{return "Channel_HQ_Address";}}
  }
}
