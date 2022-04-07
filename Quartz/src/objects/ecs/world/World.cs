using System.Runtime.InteropServices;
using Quartz.core;
using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.managed;
using Quartz.objects.memory;

namespace Quartz.objects.ecs.world;

//TODO: prefabs
//TODO: better hierarchy iteration
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
	public static int worldCount => _worlds.elementCount;
	
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

	public Archetype? GetArchetype(params Type[] types) => archetypes.FindOrCreateArchetype(types);
	public Archetype? GetEntityArchetype(EntityId entity) => archetypes.TryGetArchetype(entity);
	
#endregion archetypes

#region entities

	public bool IsAlive(EntityId entity) => entity.id != uint.MaxValue && entity.id < entities.count && !entities.emptyIndices.Contains(entity);
	
	public EntityId CreateEntity() {
		int id = entities.Add(new());
		entities[id] = new((uint)id, worldId);
		return (uint)id;
	}

	public EntityId CreateEntity(Archetype archetype, InitMode initMode = InitMode.zeroed) {
		EntityId entity = CreateEntity();
		archetypes.AddEntity(entity, archetype);
		InitializeObject(archetype, entity, initMode);
		return entity;
	}

	public unsafe void InitializeObject(Archetype archetype, EntityId entity, InitMode initMode) {
		if (initMode == InitMode.zeroed)
			foreach (ComponentType type in archetype.componentTypes)
				QuartzNative.MemSet(Comp(entity, type), 0, (uint) Marshal.SizeOf(type.type));
		else if (initMode == InitMode.ctor)
			foreach (ComponentType type in archetype.componentTypes) {
				object component = Activator.CreateInstance(type.type)!;
				Marshal.StructureToPtr(component, (IntPtr) Comp(entity, type), false);
			}
	}

	public EntityId CreateEntity(params Type[] archetype) => CreateEntity(GetArchetype(archetype));

	public void CreateEntities(int count, Action<EntityId> onCreation, InitMode initMode = InitMode.ctor) {
		entities.AddMultiple(count, i => onCreation( i));
	}

	public void CreateEntities(int count, Archetype archetype, Action<EntityId> onCreation, InitMode initMode = InitMode.ctor) {
		switch (initMode) {
			case InitMode.uninitialized: CreateEntities_uninitialized(count, archetype, onCreation); break;
			case InitMode.zeroed:        CreateEntities_zeroed(count, archetype, onCreation); break;
			case InitMode.ctor:          CreateEntities_ctor(count, archetype, onCreation); break;
			default:                     throw new ArgumentOutOfRangeException(nameof(initMode), initMode, null);
		}
	}
	
	private void CreateEntities_uninitialized(int count, Archetype archetype, Action<EntityId> onCreation) {
		archetype.PreAllocate(count);
		entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			onCreation(ent);
		});
	}
	
	private void CreateEntities_zeroed(int count, Archetype archetype, Action<EntityId> onCreation) {
		archetype.PreAllocate(count);
		entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			InitializeObject(archetype, ent, InitMode.zeroed);
			onCreation(ent);
		});
	}
	
	private unsafe void CreateEntities_ctor(int count, Archetype archetype, Action<EntityId> onCreation) {
		archetype.PreAllocate(count);
		int compCount = archetype.componentTypes.Length;

		(IntPtr obj, IntPtr arr, int size)[] components = new (IntPtr, IntPtr, int)[compCount];
		for (int i = 0; i < compCount; i++) {
			object comp = Activator.CreateInstance(archetype.componentTypes[i].type)!;
			int size = Marshal.SizeOf(archetype.componentTypes[i].type);
			IntPtr allocation = (IntPtr) MemoryAllocator.Allocate(size);
			Marshal.StructureToPtr(comp, allocation, false);
			components[i] = (allocation, (IntPtr) archetype.GetComponent(i, 0), size);
		}
		
		entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);

			uint offset = archetype.GetComponentIdFromEntity(ent);
			foreach ((IntPtr obj, IntPtr arr, int size) in components)
				QuartzNative.MemCpy((byte*)arr + offset * size, (void*) obj, (uint) size);
			
			onCreation(ent);
		});
		
		for (int i = 0; i < compCount; i++) {
			MemoryAllocator.Free((void*) components[i].obj);
		}
	}

	public void DestroyEntity(EntityId id) {
		entities.RemoveAt((int) id.id);
		archetypes.RemoveEntity(id);
	}
	
	public void DestroyEntities(Archetype? archetype) {
		if (archetype == null) {
			DestroyEntitiesWhich(e => GetEntityArchetype(e) == null);
			return;
		}
		int c = archetype.components.entityCount;
		for (int i = 0; i < c; i++) {
			EntityId entity = archetype.components.entityComponentMap.GetVal((uint) i);
			entities.RemoveAt(entity);
		}
		
		archetype.Clear();
	}

	public void DestroyEntitiesWhich(Predicate<EntityId> predicate) {
		int pos = 0;
		while (pos < entities.count) {
			if (entities.emptyIndices.Contains(pos)) {
				pos++;
				continue;
			}
			EntityId entity = pos;
			if (predicate(entity)) DestroyEntity(pos);
			else pos++;
		}
	}

	public void Clear() {
		DestroyEntitiesWhich(_ => true);
	}

#endregion entities

#region components
	
	public unsafe T* Comp<T>(EntityId id) where T : unmanaged, IComponent => archetypes.GetComponent<T>(id);
	public unsafe T* TryComp<T>(EntityId id) where T : unmanaged, IComponent => archetypes.TryGetComponent<T>(id);
	
	public unsafe void* Comp(EntityId id, ComponentType t) => archetypes.GetComponent(id, t);

	public bool Remove<T>(EntityId id) where T : unmanaged, IComponent => archetypes.RemoveComponent<T>(id);

#endregion components

#region other

	public void Lock() => Monitor.Enter(_currentLock);
	public bool TryLock() => Monitor.TryEnter(_currentLock);
	public void Unlock() => Monitor.Exit(_currentLock);

#endregion other
}