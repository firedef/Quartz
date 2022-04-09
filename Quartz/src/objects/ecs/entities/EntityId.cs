using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.entities; 

public readonly struct EntityId {
	public static readonly EntityId @null = new(uint.MaxValue);
	public readonly uint id;

	public bool isValid => id != uint.MaxValue;
	public bool isAlive => isValid && entity.isAlive;

	public Entity entity => World.GetEntity((int)id);
	public WorldId worldId => entity.world;
	public World world => worldId.world;
	
	public EntityId(uint id) => this.id = id;

	public static implicit operator uint(EntityId v) => v.id;
	public static implicit operator EntityId(uint v) => new(v);
	
	public static implicit operator int(EntityId v) => (int)v.id;
	public static implicit operator EntityId(int v) => new((uint)v);

	public static bool operator ==(EntityId a, EntityId b) => a.id == b.id; 
	public static bool operator !=(EntityId a, EntityId b) => a.id != b.id; 
	
	public override string ToString() => isValid ? id.ToString() : "null";
}