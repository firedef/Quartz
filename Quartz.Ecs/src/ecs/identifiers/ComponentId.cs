namespace Quartz.Ecs.ecs.identifiers; 

public readonly struct ComponentId : IIdentifierU32 {
	public static readonly ComponentId @null = new(IIdentifierU32.nullIndex);
	public readonly uint position;
	public bool isValid => position != IIdentifierU32.nullIndex;
	
	public ComponentId(uint position) => this.position = position;
	
	public static bool operator ==(ComponentId a, ComponentId b) => a.position == b.position;
	public static bool operator !=(ComponentId a, ComponentId b) => a.position != b.position;
	public static implicit operator uint(ComponentId v) => v.position;
	public static implicit operator ComponentId(uint v) => new(v);
	public static implicit operator int(ComponentId v) => (int) v.position;
	public static implicit operator ComponentId(int v) => new((uint)v);

	public override string ToString() => position.ToString();
	
	public bool Equals(ComponentId other) => position == other.position;
	public override bool Equals(object? obj) => obj is ComponentId other && Equals(other);
	public override int GetHashCode() => (int)position;
}