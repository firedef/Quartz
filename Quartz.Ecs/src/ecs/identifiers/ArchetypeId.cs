namespace Quartz.Ecs.ecs.identifiers;

//TODO: increase to U32, if 65535 archetypes per world is not enough
public readonly struct ArchetypeId : IIdentifierU16 {
	public static readonly ArchetypeId @null = new(IIdentifierU16.nullIndex);
	public readonly ushort position;
	public bool isValid => position != IIdentifierU16.nullIndex;
	
	public ArchetypeId(ushort position) => this.position = position;
	
	public static bool operator ==(ArchetypeId a, ArchetypeId b) => a.position == b.position;
	public static bool operator !=(ArchetypeId a, ArchetypeId b) => a.position != b.position;
	public static implicit operator ushort(ArchetypeId v) => v.position;
	public static implicit operator ArchetypeId(ushort v) => new(v);
	public static implicit operator int(ArchetypeId v) => v.position;
	public static implicit operator ArchetypeId(int v) => new((ushort)v);

	public override string ToString() => position.ToString();
	
	public bool Equals(ArchetypeId other) => position == other.position;
	public override bool Equals(object? obj) => obj is ArchetypeId other && Equals(other);
	public override int GetHashCode() => position.GetHashCode();
}