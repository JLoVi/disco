// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct HandWaiveFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static HandWaiveFB GetRootAsHandWaiveFB(ByteBuffer _bb) { return GetRootAsHandWaiveFB(_bb, new HandWaiveFB()); }
  public static HandWaiveFB GetRootAsHandWaiveFB(ByteBuffer _bb, HandWaiveFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public HandWaiveFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public NodeFB? Node { get { int o = __p.__offset(4); return o != 0 ? (NodeFB?)(new NodeFB()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }

  public static Offset<HandWaiveFB> CreateHandWaiveFB(FlatBufferBuilder builder,
      Offset<NodeFB> NodeOffset = default(Offset<NodeFB>)) {
    builder.StartObject(1);
    HandWaiveFB.AddNode(builder, NodeOffset);
    return HandWaiveFB.EndHandWaiveFB(builder);
  }

  public static void StartHandWaiveFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddNode(FlatBufferBuilder builder, Offset<NodeFB> NodeOffset) { builder.AddOffset(0, NodeOffset.Value, 0); }
  public static Offset<HandWaiveFB> EndHandWaiveFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<HandWaiveFB>(o);
  }
};


}
