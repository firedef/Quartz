using Quartz.Ecs.ecs.components;

namespace Quartz.Ecs.ecs.identifiers; 

public readonly struct ComponentType : IIdentifierU32 {
	public static readonly ComponentType @null = new(IIdentifierU32.nullIndex);
	public readonly uint typeId;
	public bool isValid => typeId != IIdentifierU32.nullIndex;
	public ComponentData data => ComponentDataCache.Get(this);

	public ComponentType(uint typeId) => this.typeId = typeId;
	
	public static bool operator ==(ComponentType a, ComponentType b) => a.typeId == b.typeId;
	public static bool operator !=(ComponentType a, ComponentType b) => a.typeId != b.typeId;
	public static implicit operator uint(ComponentType v) => v.typeId;
	public static implicit operator ComponentType(uint v) => new(v);
	public static implicit operator int(ComponentType v) => (int) v.typeId;
	public static implicit operator ComponentType(int v) => new((uint)v);

	public bool Equals(ComponentType other) => typeId == other.typeId;
	public override bool Equals(object? obj) => obj is ComponentType other && Equals(other);
	public override int GetHashCode() => (int)typeId;
}