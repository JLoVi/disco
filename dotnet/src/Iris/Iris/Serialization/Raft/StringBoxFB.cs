// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct StringBoxFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static StringBoxFB GetRootAsStringBoxFB(ByteBuffer _bb) { return GetRootAsStringBoxFB(_bb, new StringBoxFB()); }
  public static StringBoxFB GetRootAsStringBoxFB(ByteBuffer _bb, StringBoxFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public StringBoxFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Id { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(4); }
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
  public string Patch { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetPatchBytes() { return __p.__vector_as_arraysegment(8); }
  public string Tags(int j) { int o = __p.__offset(10); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int TagsLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
  public StringTypeFB StringType { get { int o = __p.__offset(12); return o != 0 ? (StringTypeFB)__p.bb.GetUshort(o + __p.bb_pos) : StringTypeFB.SimpleFB; } }
  public string FileMask { get { int o = __p.__offset(14); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetFileMaskBytes() { return __p.__vector_as_arraysegment(14); }
  public int MaxChars { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public StringSliceFB? Slices(int j) { int o = __p.__offset(18); return o != 0 ? (StringSliceFB?)(new StringSliceFB()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int SlicesLength { get { int o = __p.__offset(18); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<StringBoxFB> CreateStringBoxFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset NameOffset = default(StringOffset),
      StringOffset PatchOffset = default(StringOffset),
      VectorOffset TagsOffset = default(VectorOffset),
      StringTypeFB StringType = StringTypeFB.SimpleFB,
      StringOffset FileMaskOffset = default(StringOffset),
      int MaxChars = 0,
      VectorOffset SlicesOffset = default(VectorOffset)) {
    builder.StartObject(8);
    StringBoxFB.AddSlices(builder, SlicesOffset);
    StringBoxFB.AddMaxChars(builder, MaxChars);
    StringBoxFB.AddFileMask(builder, FileMaskOffset);
    StringBoxFB.AddTags(builder, TagsOffset);
    StringBoxFB.AddPatch(builder, PatchOffset);
    StringBoxFB.AddName(builder, NameOffset);
    StringBoxFB.AddId(builder, IdOffset);
    StringBoxFB.AddStringType(builder, StringType);
    return StringBoxFB.EndStringBoxFB(builder);
  }

  public static void StartStringBoxFB(FlatBufferBuilder builder) { builder.StartObject(8); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset NameOffset) { builder.AddOffset(1, NameOffset.Value, 0); }
  public static void AddPatch(FlatBufferBuilder builder, StringOffset PatchOffset) { builder.AddOffset(2, PatchOffset.Value, 0); }
  public static void AddTags(FlatBufferBuilder builder, VectorOffset TagsOffset) { builder.AddOffset(3, TagsOffset.Value, 0); }
  public static VectorOffset CreateTagsVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartTagsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddStringType(FlatBufferBuilder builder, StringTypeFB StringType) { builder.AddUshort(4, (ushort)StringType, 0); }
  public static void AddFileMask(FlatBufferBuilder builder, StringOffset FileMaskOffset) { builder.AddOffset(5, FileMaskOffset.Value, 0); }
  public static void AddMaxChars(FlatBufferBuilder builder, int MaxChars) { builder.AddInt(6, MaxChars, 0); }
  public static void AddSlices(FlatBufferBuilder builder, VectorOffset SlicesOffset) { builder.AddOffset(7, SlicesOffset.Value, 0); }
  public static VectorOffset CreateSlicesVector(FlatBufferBuilder builder, Offset<StringSliceFB>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartSlicesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<StringBoxFB> EndStringBoxFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<StringBoxFB>(o);
  }
};


}
