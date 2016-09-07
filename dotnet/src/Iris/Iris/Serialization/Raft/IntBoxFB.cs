// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public sealed class IntBoxFB : Table {
  public static IntBoxFB GetRootAsIntBoxFB(ByteBuffer _bb) { return GetRootAsIntBoxFB(_bb, new IntBoxFB()); }
  public static IntBoxFB GetRootAsIntBoxFB(ByteBuffer _bb, IntBoxFB obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public IntBoxFB __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Id { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __vector_as_arraysegment(4); }
  public string Name { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __vector_as_arraysegment(6); }
  public string Patch { get { int o = __offset(8); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPatchBytes() { return __vector_as_arraysegment(8); }
  public string GetTags(int j) { int o = __offset(10); return o != 0 ? __string(__vector(o) + j * 4) : null; }
  public int TagsLength { get { int o = __offset(10); return o != 0 ? __vector_len(o) : 0; } }
  public uint VecSize { get { int o = __offset(12); return o != 0 ? bb.GetUint(o + bb_pos) : (uint)0; } }
  public int Min { get { int o = __offset(14); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public int Max { get { int o = __offset(16); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string Unit { get { int o = __offset(18); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetUnitBytes() { return __vector_as_arraysegment(18); }
  public IntSliceFB GetSlices(int j) { return GetSlices(new IntSliceFB(), j); }
  public IntSliceFB GetSlices(IntSliceFB obj, int j) { int o = __offset(20); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int SlicesLength { get { int o = __offset(20); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<IntBoxFB> CreateIntBoxFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset NameOffset = default(StringOffset),
      StringOffset PatchOffset = default(StringOffset),
      VectorOffset TagsOffset = default(VectorOffset),
      uint VecSize = 0,
      int Min = 0,
      int Max = 0,
      StringOffset UnitOffset = default(StringOffset),
      VectorOffset SlicesOffset = default(VectorOffset)) {
    builder.StartObject(9);
    IntBoxFB.AddSlices(builder, SlicesOffset);
    IntBoxFB.AddUnit(builder, UnitOffset);
    IntBoxFB.AddMax(builder, Max);
    IntBoxFB.AddMin(builder, Min);
    IntBoxFB.AddVecSize(builder, VecSize);
    IntBoxFB.AddTags(builder, TagsOffset);
    IntBoxFB.AddPatch(builder, PatchOffset);
    IntBoxFB.AddName(builder, NameOffset);
    IntBoxFB.AddId(builder, IdOffset);
    return IntBoxFB.EndIntBoxFB(builder);
  }

  public static void StartIntBoxFB(FlatBufferBuilder builder) { builder.StartObject(9); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset NameOffset) { builder.AddOffset(1, NameOffset.Value, 0); }
  public static void AddPatch(FlatBufferBuilder builder, StringOffset PatchOffset) { builder.AddOffset(2, PatchOffset.Value, 0); }
  public static void AddTags(FlatBufferBuilder builder, VectorOffset TagsOffset) { builder.AddOffset(3, TagsOffset.Value, 0); }
  public static VectorOffset CreateTagsVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartTagsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddVecSize(FlatBufferBuilder builder, uint VecSize) { builder.AddUint(4, VecSize, 0); }
  public static void AddMin(FlatBufferBuilder builder, int Min) { builder.AddInt(5, Min, 0); }
  public static void AddMax(FlatBufferBuilder builder, int Max) { builder.AddInt(6, Max, 0); }
  public static void AddUnit(FlatBufferBuilder builder, StringOffset UnitOffset) { builder.AddOffset(7, UnitOffset.Value, 0); }
  public static void AddSlices(FlatBufferBuilder builder, VectorOffset SlicesOffset) { builder.AddOffset(8, SlicesOffset.Value, 0); }
  public static VectorOffset CreateSlicesVector(FlatBufferBuilder builder, Offset<IntSliceFB>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartSlicesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<IntBoxFB> EndIntBoxFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<IntBoxFB>(o);
  }
};


}
