using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.managed;
using Quartz.objects.memory;

namespace Quartz.objects.ecs.world; 

public partial class World {
#region fields

	private const int _entityArrayInitialSize = 1 << 16;
	private readonly object _currentLock = new();
	
	private static readonly ManagedListPool<World> _worlds = new();
	public static readonly World general = Create();

	public WorldId worldId { get; protected set; }
	
	public bool isAlive { get; private set; } = true;
	public bool isActive { get; private set; } = true;
	public bool isVisible { get; private set; } = true;

	public int currentEntityCount => entities.elementCount;
	public int archetypeCount => archetypes.archetypes.Count;
	public int maxAliveEntityId => entities.count;
	public int worldCount => _worlds.elementCount;
	
	public NativeListPool<Entity> entities { get; } = new(_entityArrayInitialSize);
	public ArchetypeRoot archetypes { get; } = new();
	
#endregion fields

#region stats
	
	public int GetTotalComponentCount() => archetypes.archetypes.Sum(archetype => archetype.componentTypes.Length * archetype.components.entityCount);
	public int GetUniqueComponentCount() {
		HashSet<ComponentType> types = new();
		foreach (ComponentType type in archetypes.archetypes.SelectMany(archetype => archetype.componentTypes))
			types.Add(type);
		return types.Count;
	}

#endregion stats

#region world

	private World(WorldId worldId) => this.worldId = worldId;
	
	public void SetActive(bool v) => isActive = v;
	public void SetVisible(bool v) => isVisible = v;

	public void Destroy() {
		isAlive = false;
		isActive = false;
		_worlds.RemoveAt((int) worldId.id);
	}

	public static World Create() {
		World world = new(0);
		int id = _worlds.Add(world);
		world.worldId = (uint) id;
		return world;
	}

	public static void ForeachWorld(Action<World> a) {
		int c = _worlds.storage.Count;
		for (int i = 0; i < c; i++)
			if (!_worlds.emptyIndices.Contains(i)) 
				a(_worlds.storage[i]);
	}

#endregion world

#region archetypes

	public Archetype GetArchetype(params Type[] types) => archetypes.FindOrCreateArchetype(types);
	
#endregion archetypes

#region entities
	
	public EntityId CreateEntity() {
		int id = entities.Add(new());
		entities[id] = new((uint)id, worldId);
		return (uint)id;
	}

	public EntityId CreateEntity(Archetype archetype) {
		EntityId entity = CreateEntity();
		archetypes.AddEntity(entity, archetype);
		return entity;
	}

	public EntityId CreateEntity(params Type[] archetype) => CreateEntity(GetArchetype(archetype));

	public void CreateEntities(int count, Action<EntityId> onCreation) {
		entities.AddMultiple(count, i => onCreation( i));
	}
	
	public void CreateEntities(int count, Archetype archetype, Action<EntityId> onCreation) {
		archetype.PreAllocate(count);
		entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			onCreation(ent);
		});
	}

	public void DestroyEntity(EntityId id) {
		entities.RemoveAt((int) id.id);
		archetypes.RemoveEntity(id);
	}

#endregion entities

#region components
	
	public unsafe T* Comp<T>(EntityId id) where T : unmanaged, IComponent => archetypes.GetComponent<T>(id);

	public bool Remove<T>(EntityId id) where T : unmanaged, IComponent => archetypes.RemoveComponent<T>(id);

#endregion components

#region other

	public void Lock() => Monitor.Enter(_currentLock);
	public bool TryLock() => Monitor.TryEnter(_currentLock);
	public void Unlock() => Monitor.Exit(_currentLock);

#endregion other
}