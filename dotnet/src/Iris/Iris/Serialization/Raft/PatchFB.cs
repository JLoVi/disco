// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public sealed class PatchFB : Table {
  public static PatchFB GetRootAsPatchFB(ByteBuffer _bb) { return GetRootAsPatchFB(_bb, new PatchFB()); }
  public static PatchFB GetRootAsPatchFB(ByteBuffer _bb, PatchFB obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public PatchFB __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Id { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __vector_as_arraysegment(4); }
  public string Name { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __vector_as_arraysegment(6); }

  public static Offset<PatchFB> CreatePatchFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset NameOffset = default(StringOffset)) {
    builder.StartObject(2);
    PatchFB.AddName(builder, NameOffset);
    PatchFB.AddId(builder, IdOffset);
    return PatchFB.EndPatchFB(builder);
  }

  public static void StartPatchFB(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset NameOffset) { builder.AddOffset(1, NameOffset.Value, 0); }
  public static Offset<PatchFB> EndPatchFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<PatchFB>(o);
  }
};


}
