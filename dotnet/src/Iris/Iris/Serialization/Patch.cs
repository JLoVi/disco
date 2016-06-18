// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization
{

using System;
using FlatBuffers;

public sealed class Patch : Table {
  public static Patch GetRootAsPatch(ByteBuffer _bb) { return GetRootAsPatch(_bb, new Patch()); }
  public static Patch GetRootAsPatch(ByteBuffer _bb, Patch obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Patch __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Id { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __vector_as_arraysegment(4); }
  public string Name { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __vector_as_arraysegment(6); }
  public Iris.Serialization.IOBox GetIOBoxes(int j) { return GetIOBoxes(new Iris.Serialization.IOBox(), j); }
  public Iris.Serialization.IOBox GetIOBoxes(Iris.Serialization.IOBox obj, int j) { int o = __offset(8); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int IOBoxesLength { get { int o = __offset(8); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<Patch> CreatePatch(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset NameOffset = default(StringOffset),
      VectorOffset IOBoxesOffset = default(VectorOffset)) {
    builder.StartObject(3);
    Patch.AddIOBoxes(builder, IOBoxesOffset);
    Patch.AddName(builder, NameOffset);
    Patch.AddId(builder, IdOffset);
    return Patch.EndPatch(builder);
  }

  public static void StartPatch(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset NameOffset) { builder.AddOffset(1, NameOffset.Value, 0); }
  public static void AddIOBoxes(FlatBufferBuilder builder, VectorOffset IOBoxesOffset) { builder.AddOffset(2, IOBoxesOffset.Value, 0); }
  public static VectorOffset CreateIOBoxesVector(FlatBufferBuilder builder, Offset<Iris.Serialization.IOBox>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartIOBoxesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<Patch> EndPatch(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // Id
    builder.Required(o, 6);  // Name
    builder.Required(o, 8);  // IOBoxes
    return new Offset<Patch>(o);
  }
  public static void FinishPatchBuffer(FlatBufferBuilder builder, Offset<Patch> offset) { builder.Finish(offset.Value); }
};


}