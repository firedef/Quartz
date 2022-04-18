using Quartz.Ecs.ecs.entities;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs.identifiers; 

public readonly struct EntityId : IIdentifierU32 {
	public static readonly EntityId @null = new(IIdentifierU32.nullIndex);
	public readonly uint position;
	public bool isValid => position != IIdentifierU32.nullIndex;
	public bool isAlive => isValid && entity.isAlive;

	public string? name {
		get => World.TryGetEntityName(this);
		set => World.SetEntityName(this, value);
	}

	public Entity entity => World.GetEntity(this);
	public WorldId worldId => entity.worldId;
	public World world => World.GetWorld(this);
	
	public EntityId(uint position) => this.position = position;
	
	public static bool operator ==(EntityId a, EntityId b) => a.position == b.position;
	public static bool operator !=(EntityId a, EntityId b) => a.position != b.position;
	public static implicit operator uint(EntityId v) => v.position;
	public static implicit operator EntityId(uint v) => new(v);
	public static implicit operator int(EntityId v) => (int) v.position;
	public static implicit operator EntityId(int v) => new((uint)v);
	
	public override string ToString() => name ?? position.ToString();
	
	public bool Equals(EntityId other) => position == other.position;
	public override bool Equals(object? obj) => obj is EntityId other && Equals(other);
	public override int GetHashCode() => (int)position;
}