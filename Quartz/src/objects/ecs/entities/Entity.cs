using System.Runtime.InteropServices;
using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.entities; 

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public struct Entity {
	public EntityId id;
	public WorldId world;

	public Entity(EntityId id, WorldId world) {
		this.id = id;
		this.world = world;
	}

	public static implicit operator EntityId(Entity v) => v.id;
	public static implicit operator uint(Entity v) => v.id.id;
}