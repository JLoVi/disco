// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public sealed class RemoveCueListFB : Table {
  public static RemoveCueListFB GetRootAsRemoveCueListFB(ByteBuffer _bb) { return GetRootAsRemoveCueListFB(_bb, new RemoveCueListFB()); }
  public static RemoveCueListFB GetRootAsRemoveCueListFB(ByteBuffer _bb, RemoveCueListFB obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public RemoveCueListFB __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public CueListFB CueList { get { return GetCueList(new CueListFB()); } }
  public CueListFB GetCueList(CueListFB obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }

  public static Offset<RemoveCueListFB> CreateRemoveCueListFB(FlatBufferBuilder builder,
      Offset<CueListFB> CueListOffset = default(Offset<CueListFB>)) {
    builder.StartObject(1);
    RemoveCueListFB.AddCueList(builder, CueListOffset);
    return RemoveCueListFB.EndRemoveCueListFB(builder);
  }

  public static void StartRemoveCueListFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddCueList(FlatBufferBuilder builder, Offset<CueListFB> CueListOffset) { builder.AddOffset(0, CueListOffset.Value, 0); }
  public static Offset<RemoveCueListFB> EndRemoveCueListFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<RemoveCueListFB>(o);
  }
};


}
