// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public sealed class AddNodeFB : Table {
  public static AddNodeFB GetRootAsAddNodeFB(ByteBuffer _bb) { return GetRootAsAddNodeFB(_bb, new AddNodeFB()); }
  public static AddNodeFB GetRootAsAddNodeFB(ByteBuffer _bb, AddNodeFB obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public AddNodeFB __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public NodeFB Node { get { return GetNode(new NodeFB()); } }
  public NodeFB GetNode(NodeFB obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }

  public static Offset<AddNodeFB> CreateAddNodeFB(FlatBufferBuilder builder,
      Offset<NodeFB> NodeOffset = default(Offset<NodeFB>)) {
    builder.StartObject(1);
    AddNodeFB.AddNode(builder, NodeOffset);
    return AddNodeFB.EndAddNodeFB(builder);
  }

  public static void StartAddNodeFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddNode(FlatBufferBuilder builder, Offset<NodeFB> NodeOffset) { builder.AddOffset(0, NodeOffset.Value, 0); }
  public static Offset<AddNodeFB> EndAddNodeFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<AddNodeFB>(o);
  }
};


}
