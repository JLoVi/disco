// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct NodeFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static NodeFB GetRootAsNodeFB(ByteBuffer _bb) { return GetRootAsNodeFB(_bb, new NodeFB()); }
  public static NodeFB GetRootAsNodeFB(ByteBuffer _bb, NodeFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public NodeFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Id { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(4); }
  public string HostName { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetHostNameBytes() { return __p.__vector_as_arraysegment(6); }
  public string IpAddr { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetIpAddrBytes() { return __p.__vector_as_arraysegment(8); }
  public int Port { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public bool Voting { get { int o = __p.__offset(12); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }
  public bool VotedForMe { get { int o = __p.__offset(14); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }
  public NodeStateFB State { get { int o = __p.__offset(16); return o != 0 ? (NodeStateFB)__p.bb.Get(o + __p.bb_pos) : NodeStateFB.JoiningFB; } }
  public ulong NextIndex { get { int o = __p.__offset(18); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }
  public ulong MatchIndex { get { int o = __p.__offset(20); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }

  public static Offset<NodeFB> CreateNodeFB(FlatBufferBuilder builder,
      StringOffset IdOffset = default(StringOffset),
      StringOffset HostNameOffset = default(StringOffset),
      StringOffset IpAddrOffset = default(StringOffset),
      int Port = 0,
      bool Voting = false,
      bool VotedForMe = false,
      NodeStateFB State = NodeStateFB.JoiningFB,
      ulong NextIndex = 0,
      ulong MatchIndex = 0) {
    builder.StartObject(9);
    NodeFB.AddMatchIndex(builder, MatchIndex);
    NodeFB.AddNextIndex(builder, NextIndex);
    NodeFB.AddPort(builder, Port);
    NodeFB.AddIpAddr(builder, IpAddrOffset);
    NodeFB.AddHostName(builder, HostNameOffset);
    NodeFB.AddId(builder, IdOffset);
    NodeFB.AddState(builder, State);
    NodeFB.AddVotedForMe(builder, VotedForMe);
    NodeFB.AddVoting(builder, Voting);
    return NodeFB.EndNodeFB(builder);
  }

  public static void StartNodeFB(FlatBufferBuilder builder) { builder.StartObject(9); }
  public static void AddId(FlatBufferBuilder builder, StringOffset IdOffset) { builder.AddOffset(0, IdOffset.Value, 0); }
  public static void AddHostName(FlatBufferBuilder builder, StringOffset HostNameOffset) { builder.AddOffset(1, HostNameOffset.Value, 0); }
  public static void AddIpAddr(FlatBufferBuilder builder, StringOffset IpAddrOffset) { builder.AddOffset(2, IpAddrOffset.Value, 0); }
  public static void AddPort(FlatBufferBuilder builder, int Port) { builder.AddInt(3, Port, 0); }
  public static void AddVoting(FlatBufferBuilder builder, bool Voting) { builder.AddBool(4, Voting, false); }
  public static void AddVotedForMe(FlatBufferBuilder builder, bool VotedForMe) { builder.AddBool(5, VotedForMe, false); }
  public static void AddState(FlatBufferBuilder builder, NodeStateFB State) { builder.AddByte(6, (byte)State, 0); }
  public static void AddNextIndex(FlatBufferBuilder builder, ulong NextIndex) { builder.AddUlong(7, NextIndex, 0); }
  public static void AddMatchIndex(FlatBufferBuilder builder, ulong MatchIndex) { builder.AddUlong(8, MatchIndex, 0); }
  public static Offset<NodeFB> EndNodeFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<NodeFB>(o);
  }
};


}
