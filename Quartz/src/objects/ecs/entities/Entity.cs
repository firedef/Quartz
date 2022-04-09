using System.Runtime.InteropServices;
using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.entities; 

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public struct Entity {
	public static readonly Entity @null = new(EntityId.@null, WorldId.@null, EntityFlags.none);
	public EntityId id;
	public WorldId world;
	public int version;
	public EntityFlags flags;

	public bool isAlive => id.id != uint.MaxValue && id.id <= World.maxAliveEntityId && (flags & EntityFlags.isAlive) != 0;

	public Entity(EntityId id, WorldId world, EntityFlags flags) {
		this.id = id;
		this.world = world;
		this.flags = flags;
		version = 0;
	}

	public static implicit operator EntityId(Entity v) => v.id;
	public static implicit operator uint(Entity v) => v.id.id;

	public override string ToString() => $"{id}:{version}";
}

[Flags]
public enum EntityFlags : ushort {
	none = 0,
	isAlive = 1 << 0,
}