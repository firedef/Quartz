namespace Quartz.Ecs.ecs.identifiers; 

public readonly struct WorldId : IIdentifierU16 {
	public static readonly WorldId @null = new(IIdentifierU16.nullIndex);
	public readonly ushort position;
	public bool isValid => position != IIdentifierU16.nullIndex;
	
	public WorldId(ushort position) => this.position = position;
	
	public static bool operator ==(WorldId a, WorldId b) => a.position == b.position;
	public static bool operator !=(WorldId a, WorldId b) => a.position != b.position;
	public static implicit operator ushort(WorldId v) => v.position;
	public static implicit operator WorldId(ushort v) => new(v);
	public static implicit operator int(WorldId v) => v.position;
	public static implicit operator WorldId(int v) => new((ushort)v);

	public override string ToString() => position.ToString();
	
	public bool Equals(WorldId other) => position == other.position;
	public override bool Equals(object? obj) => obj is WorldId other && Equals(other);
	public override int GetHashCode() => position.GetHashCode();
}