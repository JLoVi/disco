// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct ConfigurationFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static ConfigurationFB GetRootAsConfigurationFB(ByteBuffer _bb) { return GetRootAsConfigurationFB(_bb, new ConfigurationFB()); }
  public static ConfigurationFB GetRootAsConfigurationFB(ByteBuffer _bb, ConfigurationFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public ConfigurationFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Id { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(4); }
  public ulong Index { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }
  public ulong Term { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }
  public NodeFB? Nodes(int j) { int o = __p.__offset(10); return o != 0 ? (NodeFB?)(new NodeFB()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int NodesLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<ConfigurationFB> CreateConfigurationFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      ulong Index = 0,
      ulong Term = 0,
      VectorOffset NodesOffset = default(VectorOffset)) {
    builder.StartObject(4);
    ConfigurationFB.AddTerm(builder, Term);
    ConfigurationFB.AddIndex(builder, Index);
    ConfigurationFB.AddNodes(builder, NodesOffset);
    ConfigurationFB.AddId(builder, IdOffset);
    return ConfigurationFB.EndConfigurationFB(builder);
  }

  public static void StartConfigurationFB(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddIndex(FlatBufferBuilder builder, ulong Index) { builder.AddUlong(1, Index, 0); }
  public static void AddTerm(FlatBufferBuilder builder, ulong Term) { builder.AddUlong(2, Term, 0); }
  public static void AddNodes(FlatBufferBuilder builder, VectorOffset NodesOffset) { builder.AddOffset(3, NodesOffset.Value, 0); }
  public static VectorOffset CreateNodesVector(FlatBufferBuilder builder, Offset<NodeFB>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartNodesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<ConfigurationFB> EndConfigurationFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<ConfigurationFB>(o);
  }
};


}
