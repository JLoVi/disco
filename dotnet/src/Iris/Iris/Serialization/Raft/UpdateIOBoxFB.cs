// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public sealed class UpdateIOBoxFB : Table {
  public static UpdateIOBoxFB GetRootAsUpdateIOBoxFB(ByteBuffer _bb) { return GetRootAsUpdateIOBoxFB(_bb, new UpdateIOBoxFB()); }
  public static UpdateIOBoxFB GetRootAsUpdateIOBoxFB(ByteBuffer _bb, UpdateIOBoxFB obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public UpdateIOBoxFB __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public IOBoxFB IOBox { get { return GetIOBox(new IOBoxFB()); } }
  public IOBoxFB GetIOBox(IOBoxFB obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }

  public static Offset<UpdateIOBoxFB> CreateUpdateIOBoxFB(FlatBufferBuilder builder,
      Offset<IOBoxFB> IOBoxOffset = default(Offset<IOBoxFB>)) {
    builder.StartObject(1);
    UpdateIOBoxFB.AddIOBox(builder, IOBoxOffset);
    return UpdateIOBoxFB.EndUpdateIOBoxFB(builder);
  }

  public static void StartUpdateIOBoxFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddIOBox(FlatBufferBuilder builder, Offset<IOBoxFB> IOBoxOffset) { builder.AddOffset(0, IOBoxOffset.Value, 0); }
  public static Offset<UpdateIOBoxFB> EndUpdateIOBoxFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<UpdateIOBoxFB>(o);
  }
};


}
