// automatically generated by the FlatBuffers compiler, do not modify

namespace Iris.Serialization.Raft
{

using System;
using FlatBuffers;

public struct ErrorResponseFB : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static ErrorResponseFB GetRootAsErrorResponseFB(ByteBuffer _bb) { return GetRootAsErrorResponseFB(_bb, new ErrorResponseFB()); }
  public static ErrorResponseFB GetRootAsErrorResponseFB(ByteBuffer _bb, ErrorResponseFB obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public ErrorResponseFB __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public RaftErrorFB? Error { get { int o = __p.__offset(4); return o != 0 ? (RaftErrorFB?)(new RaftErrorFB()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }

  public static Offset<ErrorResponseFB> CreateErrorResponseFB(FlatBufferBuilder builder,
      Offset<RaftErrorFB> ErrorOffset = default(Offset<RaftErrorFB>)) {
    builder.StartObject(1);
    ErrorResponseFB.AddError(builder, ErrorOffset);
    return ErrorResponseFB.EndErrorResponseFB(builder);
  }

  public static void StartErrorResponseFB(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddError(FlatBufferBuilder builder, Offset<RaftErrorFB> ErrorOffset) { builder.AddOffset(0, ErrorOffset.Value, 0); }
  public static Offset<ErrorResponseFB> EndErrorResponseFB(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<ErrorResponseFB>(o);
  }
};


}
