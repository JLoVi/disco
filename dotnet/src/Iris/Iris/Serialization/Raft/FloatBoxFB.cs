// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct FloatBoxFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static FloatBoxFB GetRootAsFloatBoxFB(ByteBuffer _bb) { return GetRootAsFloatBoxFB(_bb, new FloatBoxFB()); }
  public static FloatBoxFB GetRootAsFloatBoxFB(ByteBuffer _bb, FloatBoxFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FloatBoxFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Id { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(4); }
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
  public string Patch { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetPatchBytes() { return __p.__vector_as_arraysegment(8); }
  public string Tags(int j) { int o = __p.__offset(10); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int TagsLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
  public uint VecSize { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public int Min { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Max { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Unit { get { int o = __p.__offset(18); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetUnitBytes() { return __p.__vector_as_arraysegment(18); }
  public uint Precision { get { int o = __p.__offset(20); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public FloatSliceFB? Slices(int j) { int o = __p.__offset(22); return o != 0 ? (FloatSliceFB?)(new FloatSliceFB()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int SlicesLength { get { int o = __p.__offset(22); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<FloatBoxFB> CreateFloatBoxFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset NameOffset = default(StringOffset),
      StringOffset PatchOffset = default(StringOffset),
      VectorOffset TagsOffset = default(VectorOffset),
      uint VecSize = 0,
      int Min = 0,
      int Max = 0,
      StringOffset UnitOffset = default(StringOffset),
      uint Precision = 0,
      VectorOffset SlicesOffset = default(VectorOffset)) {
    builder.StartObject(10);
    FloatBoxFB.AddSlices(builder, SlicesOffset);
    FloatBoxFB.AddPrecision(builder, Precision);
    FloatBoxFB.AddUnit(builder, UnitOffset);
    FloatBoxFB.AddMax(builder, Max);
    FloatBoxFB.AddMin(builder, Min);
    FloatBoxFB.AddVecSize(builder, VecSize);
    FloatBoxFB.AddTags(builder, TagsOffset);
    FloatBoxFB.AddPatch(builder, PatchOffset);
    FloatBoxFB.AddName(builder, NameOffset);
    FloatBoxFB.AddId(builder, IdOffset);
    return FloatBoxFB.EndFloatBoxFB(builder);
  }

  public static void StartFloatBoxFB(FlatBufferBuilder builder) { builder.StartObject(10); }
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
  public static void AddPrecision(FlatBufferBuilder builder, uint Precision) { builder.AddUint(8, Precision, 0); }
  public static void AddSlices(FlatBufferBuilder builder, VectorOffset SlicesOffset) { builder.AddOffset(9, SlicesOffset.Value, 0); }
  public static VectorOffset CreateSlicesVector(FlatBufferBuilder builder, Offset<FloatSliceFB>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartSlicesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<FloatBoxFB> EndFloatBoxFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FloatBoxFB>(o);
  }
};


}
