// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public sealed class UpdatePatchFB : Table {
  public static UpdatePatchFB GetRootAsUpdatePatchFB(ByteBuffer _bb) { return GetRootAsUpdatePatchFB(_bb, new UpdatePatchFB()); }
  public static UpdatePatchFB GetRootAsUpdatePatchFB(ByteBuffer _bb, UpdatePatchFB obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public UpdatePatchFB __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public PatchFB Patch { get { return GetPatch(new PatchFB()); } }
  public PatchFB GetPatch(PatchFB obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }

  public static Offset<UpdatePatchFB> CreateUpdatePatchFB(FlatBufferBuilder builder,
      Offset<PatchFB> PatchOffset = default(Offset<PatchFB>)) {
    builder.StartObject(1);
    UpdatePatchFB.AddPatch(builder, PatchOffset);
    return UpdatePatchFB.EndUpdatePatchFB(builder);
  }

  public static void StartUpdatePatchFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddPatch(FlatBufferBuilder builder, Offset<PatchFB> PatchOffset) { builder.AddOffset(0, PatchOffset.Value, 0); }
  public static Offset<UpdatePatchFB> EndUpdatePatchFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<UpdatePatchFB>(o);
  }
};


}