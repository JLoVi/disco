// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct AddPatchFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static AddPatchFB GetRootAsAddPatchFB(ByteBuffer _bb) { return GetRootAsAddPatchFB(_bb, new AddPatchFB()); }
  public static AddPatchFB GetRootAsAddPatchFB(ByteBuffer _bb, AddPatchFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public AddPatchFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public PatchFB? Patch { get { int o = __p.__offset(4); return o != 0 ? (PatchFB?)(new PatchFB()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }

  public static Offset<AddPatchFB> CreateAddPatchFB(FlatBufferBuilder builder,
      Offset<PatchFB> PatchOffset = default(Offset<PatchFB>)) {
    builder.StartObject(1);
    AddPatchFB.AddPatch(builder, PatchOffset);
    return AddPatchFB.EndAddPatchFB(builder);
  }

  public static void StartAddPatchFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddPatch(FlatBufferBuilder builder, Offset<PatchFB> PatchOffset) { builder.AddOffset(0, PatchOffset.Value, 0); }
  public static Offset<AddPatchFB> EndAddPatchFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<AddPatchFB>(o);
  }
};


}
