using Quartz.objects.ecs.components;
using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.entities; 

public static class EntityIdExtensions {
	public static unsafe EntityId Set<T>(this EntityId entity, T comp) where T : unmanaged, IComponent {
		*entity.world.Comp<T>(entity) = comp;
		return entity;
	}
	
	public static unsafe EntityId Set<T>(this EntityId entity) where T : unmanaged, IComponent {
		*entity.world.Comp<T>(entity) = new();
		return entity;
	}
	
	public static EntityId Remove<T>(this EntityId entity) where T : unmanaged, IComponent {
		entity.world.Remove<T>(entity);
		return entity;
	}
	
	public static void Destroy(this EntityId entity) => entity.world.DestroyEntity(entity);
	public static EntityId Clone(this EntityId entity) => entity.world.Clone(entity);
	public static EntityId Clone(this EntityId entity, World target) => target.Clone(entity);
}