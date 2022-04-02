using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.memory;
// ReSharper disable InconsistentlySynchronizedField

namespace Quartz.objects.ecs.world; 

public partial class World {
	protected const int entityArrayInitialSize = 1 << 20;

	public readonly WorldId worldId;

	public bool isAlive = true;
	public bool isActive = true;
	public bool isVisible = true;
	
	public Dictionary<Type, ComponentCollection> components = new();
	public NativeListPool<Entity> entities = new(entityArrayInitialSize);

	public int entityCount => entities.count - entities.emptyIndices.count;

	public World(WorldId worldId) => this.worldId = worldId;

	public ComponentCollection<T> GetComponentCollection<T>() where T : unmanaged, IComponent {
		if (components.TryGetValue(typeof(T), out ComponentCollection? v)) return (ComponentCollection<T>) v;
		ComponentCollection<T> coll = new();
		components.Add(typeof(T), coll);
		return coll;
	}

	public unsafe Entity* GetEntityPtr(EntityId id) => entities.ptr + id.id;

	public unsafe T* GetComponent<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().GetOrAdd(id);
	public ComponentId GetComponentId<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().GetComponentFromEntity(id);
	public void RemoveComponent<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().Remove(id);
	public bool ContainsComponent<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().ContainsEntity(id);

	public int CountEntityComponents(EntityId id) => components.Values.Count(collection => collection.ContainsEntity(id));

	public void RemoveEntity(EntityId id) {
		foreach (ComponentCollection collection in components.Values)
			collection.Remove(id);
	}

	public EntityId GetEntityFromComponent<T>(ComponentId id) where T : unmanaged, IComponent => GetComponentCollection<T>().GetEntityFromComponent(id);

	public Entity AddEntity() {
		int id = entities.Add(new());
		Entity entity = new((uint) id, worldId);
		entities[id] = entity;
		return entity;
	}
}