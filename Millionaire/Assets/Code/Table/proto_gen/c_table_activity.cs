//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: c_table_activity.proto
// Note: requires additional types generated from: common_activity.proto
// Note: requires additional types generated from: common_game_res.proto
namespace Table
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ACTIVITY_MISSION")]
  public partial class ACTIVITY_MISSION : global::ProtoBuf.IExtensible
  {
    public ACTIVITY_MISSION() {}
    

    private Common.ActivityType? _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Common.ActivityType type
    {
      get { return _type?? Common.ActivityType.ACTIVITY_TYPE_ELITE_CHALLENGE; }
      set { _type = value; }
    }
    //Here has been replaced by XXMMLLDeleter
    [global::System.ComponentModel.Browsable(false)]
    public bool typeSpecified
    {
      get { return _type != null; }
      set { if (value == (_type== null)) _type = value ? type : (Common.ActivityType?)null; }
    }
    private bool ShouldSerializetype() { return typeSpecified; }
    private void Resettype() { typeSpecified = false; }
    

    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name?? ""; }
      set { _name = value; }
    }
    //Here has been replaced by XXMMLLDeleter
    [global::System.ComponentModel.Browsable(false)]
    public bool nameSpecified
    {
      get { return _name != null; }
      set { if (value == (_name== null)) _name = value ? name : (string)null; }
    }
    private bool ShouldSerializename() { return nameSpecified; }
    private void Resetname() { nameSpecified = false; }
    

    private string _description;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"description", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string description
    {
      get { return _description?? ""; }
      set { _description = value; }
    }
    //Here has been replaced by XXMMLLDeleter
    [global::System.ComponentModel.Browsable(false)]
    public bool descriptionSpecified
    {
      get { return _description != null; }
      set { if (value == (_description== null)) _description = value ? description : (string)null; }
    }
    private bool ShouldSerializedescription() { return descriptionSpecified; }
    private void Resetdescription() { descriptionSpecified = false; }
    

    private string _icon;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"icon", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string icon
    {
      get { return _icon?? ""; }
      set { _icon = value; }
    }
    //Here has been replaced by XXMMLLDeleter
    [global::System.ComponentModel.Browsable(false)]
    public bool iconSpecified
    {
      get { return _icon != null; }
      set { if (value == (_icon== null)) _icon = value ? icon : (string)null; }
    }
    private bool ShouldSerializeicon() { return iconSpecified; }
    private void Reseticon() { iconSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ACTIVITY_MISSION_ARRAY")]
  public partial class ACTIVITY_MISSION_ARRAY : global::ProtoBuf.IExtensible
  {
    public ACTIVITY_MISSION_ARRAY() {}
    
    private readonly global::System.Collections.Generic.List<Table.ACTIVITY_MISSION> _rows = new global::System.Collections.Generic.List<Table.ACTIVITY_MISSION>();
    [global::ProtoBuf.ProtoMember(1, Name=@"rows", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Table.ACTIVITY_MISSION> rows
    {
      get { return _rows; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ACTIVITY_AWARD")]
  public partial class ACTIVITY_AWARD : global::ProtoBuf.IExtensible
  {
    public ACTIVITY_AWARD() {}
    

    private int? _score;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int score
    {
      get { return _score?? default(int); }
      set { _score = value; }
    }
    //Here has been replaced by XXMMLLDeleter
    [global::System.ComponentModel.Browsable(false)]
    public bool scoreSpecified
    {
      get { return _score != null; }
      set { if (value == (_score== null)) _score = value ? score : (int?)null; }
    }
    private bool ShouldSerializescore() { return scoreSpecified; }
    private void Resetscore() { scoreSpecified = false; }
    
    private readonly global::System.Collections.Generic.List<Common.GameRes> _award_list = new global::System.Collections.Generic.List<Common.GameRes>();
    [global::ProtoBuf.ProtoMember(2, Name=@"award_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Common.GameRes> award_list
    {
      get { return _award_list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ACTIVITY_AWARD_ARRAY")]
  public partial class ACTIVITY_AWARD_ARRAY : global::ProtoBuf.IExtensible
  {
    public ACTIVITY_AWARD_ARRAY() {}
    
    private readonly global::System.Collections.Generic.List<Table.ACTIVITY_AWARD> _rows = new global::System.Collections.Generic.List<Table.ACTIVITY_AWARD>();
    [global::ProtoBuf.ProtoMember(1, Name=@"rows", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Table.ACTIVITY_AWARD> rows
    {
      get { return _rows; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}