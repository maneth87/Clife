
using System;

public class bl_channel
{
  private string _Channel_ID;
  private string _Details;
  private string _Type;
  private string _Created_By;
  private DateTime _Created_On;
  private string _Created_Note;
  private int _Status;

  public string Channel_ID
  {
    get {return this._Channel_ID;}
    set { this._Channel_ID = value;}
  }

  public string Details
  {
    get {return this._Details;}
    set { this._Details = value;}
  }

  public string Type
  {
    get {return this._Type;}
    set { this._Type = value;}
  }

  public int Status
  {
    get {return this._Status;}
    set { this._Status = value;}
  }

  public string Created_By
  {
    get {return this._Created_By;}
    set { this._Created_By = value;}
  }

  public DateTime Created_On
  {
    get {return this._Created_On;}
      set { this._Created_On = value; }
  }

  public enum CHANNEL_NAME
  {
    INDIVIDUAL,
    CORPORATE,
  }
}
