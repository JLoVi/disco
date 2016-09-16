// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct CueFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static CueFB GetRootAsCueFB(ByteBuffer _bb) { return GetRootAsCueFB(_bb, new CueFB()); }
  public static CueFB GetRootAsCueFB(ByteBuffer _bb, CueFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public CueFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Id { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(4); }
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
  public IOBoxFB? IOBoxes(int j) { int o = __p.__offset(8); return o != 0 ? (IOBoxFB?)(new IOBoxFB()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int IOBoxesLength { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<CueFB> CreateCueFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset NameOffset = default(StringOffset),
      VectorOffset IOBoxesOffset = default(VectorOffset)) {
    builder.StartObject(3);
    CueFB.AddIOBoxes(builder, IOBoxesOffset);
    CueFB.AddName(builder, NameOffset);
    CueFB.AddId(builder, IdOffset);
    return CueFB.EndCueFB(builder);
  }

  public static void StartCueFB(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset NameOffset) { builder.AddOffset(1, NameOffset.Value, 0); }
  public static void AddIOBoxes(FlatBufferBuilder builder, VectorOffset IOBoxesOffset) { builder.AddOffset(2, IOBoxesOffset.Value, 0); }
  public static VectorOffset CreateIOBoxesVector(FlatBufferBuilder builder, Offset<IOBoxFB>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartIOBoxesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<CueFB> EndCueFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<CueFB>(o);
  }
  public static void FinishCueFBBuffer(FlatBufferBuilder builder, Offset<CueFB> offset) { builder.Finish(offset.Value); }
};


}
