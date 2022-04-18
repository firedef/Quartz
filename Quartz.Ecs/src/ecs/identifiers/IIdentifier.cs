namespace Quartz.Ecs.ecs.identifiers; 

public interface IIdentifier<TIndex> where TIndex : unmanaged {
}

public interface IIdentifierU16 : IIdentifier<ushort> {
	public const ushort nullIndex = ushort.MaxValue;
}

public interface IIdentifierU32 : IIdentifier<uint> {
	public const uint nullIndex = uint.MaxValue;
}