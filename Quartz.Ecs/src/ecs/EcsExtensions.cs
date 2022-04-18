using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs; 

public static class EcsExtensions {
	public static unsafe EntityId Set<T>(this EntityId entity, T comp) where T : unmanaged, IComponent {
		*entity.world.Comp<T>(entity) = comp;
		return entity;
	}
	
	public static unsafe EntityId Set<T>(this EntityId entity) where T : unmanaged, IComponent {
		*entity.world.Comp<T>(entity) = new();
		return entity;
	}
	
	public static void Set(this EntityId entity, ComponentType t, Dictionary<string, string> fields) {
		entity.world.SetComponent(entity, t, fields);
	}
	
	public static unsafe EntityId TryAdd<T>(this EntityId entity) where T : unmanaged, IComponent {
		entity.world.Comp<T>(entity);
		return entity;
	}
	
	public static EntityId Remove<T>(this EntityId entity) where T : unmanaged, IComponent {
		entity.world.RemoveComp<T>(entity);
		return entity;
	}
	
	public static void Destroy(this EntityId entity) => entity.world.DestroyEntity(entity);

	//TODO: add ICloneable with Ref<T> support
	public static EntityId Clone(this EntityId entity) => entity.world.Clone(entity);
	public static EntityId Clone(this EntityId entity, World target) => target.Clone(entity);
	public static void Clone(this EntityId entity, World target, int count, Action<EntityId> onClone) => target.CloneMultiple(entity, count, onClone);
}