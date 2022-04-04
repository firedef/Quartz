using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.managed;
using Quartz.objects.memory;
// ReSharper disable InconsistentlySynchronizedField

namespace Quartz.objects.ecs.world; 

public partial class World {
	protected static readonly ManagedListPool<World> worlds = new();
	public static readonly World general = Create();
	
	protected const int entityArrayInitialSize = 1 << 20;
	
	public WorldId worldId { get; protected set; }
	
	public bool isAlive { get; protected set; } = true;
	public bool isActive { get; protected set; } = true;
	public bool isVisible { get; protected set; } = true;

	public readonly NativeListPool<Entity> entities = new(entityArrayInitialSize);
	public readonly ArchetypeRoot archetypes = new();
	
	protected World(WorldId worldId) => this.worldId = worldId;

	public Archetype GetArchetype(params Type[] types) => archetypes.FindOrCreateArchetype(types);
	
	public Archetype GetArchetype<T0>()
		where T0 : unmanaged, IComponent
		=> GetArchetype(typeof(T0));
	
	public Archetype GetArchetype<T0, T1>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		=> GetArchetype(typeof(T0), typeof(T1));
	
	public Archetype GetArchetype<T0, T1, T2>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2));
	
	public Archetype GetArchetype<T0, T1, T2, T3>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
	
	public Archetype GetArchetype<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));

	public EntityId CreateEntity() {
		int id = entities.Add(new());
		entities[id - 1] = new((uint)id, worldId);
		return (uint)id;
	}

	public EntityId CreateEntity(Archetype archetype) {
		EntityId entity = CreateEntity();
		archetypes.AddEntity(entity, archetype);
		return entity;
	}

	public EntityId CreateEntity(params Type[] archetype) => CreateEntity(GetArchetype(archetype));
	
	public EntityId CreateEntity<T0>() 
		where T0 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T0>());
	
	public EntityId CreateEntity<T0, T1>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T0, T1>());
	
	public EntityId CreateEntity<T0, T1, T2>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T0, T1, T2>());
	
	public EntityId CreateEntity<T0, T1, T2, T3>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T0, T1, T2, T3>());
	
	public EntityId CreateEntity<T0, T1, T2, T3, T4>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T0, T1, T2, T3, T4>());

	public void DestroyEntity(EntityId id) {
		entities.RemoveAt((int) id.id);
		archetypes.RemoveEntity(id);
	}

	public unsafe T* Comp<T>(EntityId id) where T : unmanaged, IComponent => archetypes.GetComponent<T>(id);

	public bool Remove<T>(EntityId id) where T : unmanaged, IComponent => archetypes.RemoveComponent<T>(id);
	
	//
	// public readonly Dictionary<Type, ComponentCollection> components = new();
	// public readonly NativeListPool<Entity> entities = new(entityArrayInitialSize);
	//
	// public int entityCount => entities.count - entities.emptyIndices.count;
	//
	// protected World(WorldId worldId) => this.worldId = worldId;
	//
	// public ComponentCollection<T> GetComponentCollection<T>() where T : unmanaged, IComponent {
	// 	if (components.TryGetValue(typeof(T), out ComponentCollection? v)) return (ComponentCollection<T>) v;
	// 	ComponentCollection<T> coll = new();
	// 	components.Add(typeof(T), coll);
	// 	return coll;
	// }
	//
	// public unsafe Entity* GetEntityPtr(EntityId id) => entities.ptr + id.id;
	//
	// public unsafe T* GetComponent<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().GetOrAdd(id);
	// public ComponentId GetComponentId<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().GetComponentFromEntity(id);
	// public void RemoveComponent<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().Remove(id);
	// public bool ContainsComponent<T>(EntityId id) where T : unmanaged, IComponent => GetComponentCollection<T>().ContainsEntity(id);
	//
	// public int CountEntityComponents(EntityId id) => components.Values.Count(collection => collection.ContainsEntity(id));
	//
	// public void RemoveEntity(EntityId id) {
	// 	foreach (ComponentCollection collection in components.Values)
	// 		collection.Remove(id);
	// 	entities.RemoveAt((int) id.id);
	// }
	//
	// public EntityId GetEntityFromComponent<T>(ComponentId id) where T : unmanaged, IComponent => GetComponentCollection<T>().GetEntityFromComponent(id);
	//
	// public Entity AddEntity() {
	// 	int id = entities.Add(new());
	// 	Entity entity = new((uint) id, worldId);
	// 	entities[id] = entity;
	// 	return entity;
	// }
	
	public void SetActive(bool v) => isActive = v;
	public void SetVisible(bool v) => isVisible = v;

	public void Destroy() {
		isAlive = false;
		isActive = false;
		worlds.RemoveAt((int) worldId.id);
	}

	public static World Create() {
		World world = new(0);
		int id = worlds.Add(world);
		world.worldId = (uint) id;
		return world;
	}

}