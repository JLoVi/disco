// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct RaftMsgFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static RaftMsgFB GetRootAsRaftMsgFB(ByteBuffer _bb) { return GetRootAsRaftMsgFB(_bb, new RaftMsgFB()); }
  public static RaftMsgFB GetRootAsRaftMsgFB(ByteBuffer _bb, RaftMsgFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public RaftMsgFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public RaftMsgTypeFB MsgType { get { int o = __p.__offset(4); return o != 0 ? (RaftMsgTypeFB)__p.bb.Get(o + __p.bb_pos) : RaftMsgTypeFB.NONE; } }
  public TTable? Msg<TTable>() where TTable : struct, IFlatbufferObject { int o = __p.__offset(6); return o != 0 ? (TTable?)__p.__union<TTable>(o) : null; }

  public static Offset<RaftMsgFB> CreateRaftMsgFB(FlatBufferBuilder builder,
      RaftMsgTypeFB Msg_type = RaftMsgTypeFB.NONE,
      int MsgOffset = 0) {
    builder.StartObject(2);
    RaftMsgFB.AddMsg(builder, MsgOffset);
    RaftMsgFB.AddMsgType(builder, Msg_type);
    return RaftMsgFB.EndRaftMsgFB(builder);
  }

  public static void StartRaftMsgFB(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddMsgType(FlatBufferBuilder builder, RaftMsgTypeFB MsgType) { builder.AddByte(0, (byte)MsgType, 0); }
  public static void AddMsg(FlatBufferBuilder builder, int MsgOffset) { builder.AddOffset(1, MsgOffset, 0); }
  public static Offset<RaftMsgFB> EndRaftMsgFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<RaftMsgFB>(o);
  }
};


}
