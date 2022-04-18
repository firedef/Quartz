using System.Runtime.InteropServices;
using Quartz.CoreCs.memory;
using Quartz.CoreCs.native;
using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.hierarchy.ecs;
using Quartz.objects.memory;

namespace Quartz.objects.ecs.world;

//TODO: better hierarchy iteration
public partial class World {
#region fields

	private readonly object _currentLock = new();

	/// <summary>current 16-bit world id</summary>
	public WorldId worldId { get; protected set; }

	/// <summary>true, if world is alive (not null)</summary>
	public bool isAlive { get; private set; } = true;

	/// <summary>true, is world is participate in events</summary>
	public bool isActive = true;
	
	/// <summary>true, is world is renderable</summary>
	public bool isVisible = true;

	/// <summary>current alive entity count of this world</summary>
	public int currentEntityCount { get; private set; }

	/// <summary>current archetype count of this world</summary>
	public int archetypeCount => archetypes.archetypes.Count;

	public string worldName = "unnamed";

	public readonly ArchetypeRoot archetypes;
	
#endregion fields

#region stats

	/// <summary>count components on alive entities</summary>
	public int GetTotalComponentCount() => archetypes.archetypes.Sum(archetype => archetype.componentTypes.Length * archetype.components.entityCount);
	
	/// <summary>count unique components on all archetypes</summary>
	public int GetUniqueComponentCount() {
		HashSet<ComponentType> types = new();
		foreach (ComponentType type in archetypes.archetypes.SelectMany(archetype => archetype.componentTypes))
			types.Add(type);
		return types.Count;
	}

#endregion stats

#region world

	private World(WorldId worldId) {
		this.worldId = worldId;
		archetypes = new(this);
	}

	/// <summary>set world active state, to process events or not</summary>
	public void SetActive(bool v) => isActive = v;
	
	/// <summary>set world visible state, to render entities or not</summary>
	public void SetVisible(bool v) => isVisible = v;

	/// <summary>destroy this world with all entities</summary>
	public void Destroy() {
		isAlive = false;
		isActive = false;
		isVisible = false;
		DestroyEntitiesWhich(e => e.worldId.id == worldId.id);
		_worlds.RemoveAt(worldId.id);
	}

	/// <summary>create new world</summary>
	public static World Create(string name) {
		World world = new(0);
		int id = _worlds.Add(world);
		world.worldId = (ushort) id;
		world.worldName = name;
		return world;
	}

#endregion world

#region archetypes

	/// <summary>get archetype from types, or return null if types is empty</summary>
	public Archetype? GetArchetype(params Type[] types) => archetypes.FindOrCreateArchetype(types);
	
	/// <summary>get archetype from types, or return null if types is empty</summary>
	public Archetype? GetArchetype(params ComponentType[] types) => archetypes.FindOrCreateArchetype(types);

	/// <summary>get archetype of alive entity, or return null if entity does not have any components</summary>
	public Archetype? GetEntityArchetype(EntityId entity) => archetypes.TryGetArchetype(entity);
	
	public Archetype GetArchetypeById(int id) => archetypes.archetypes[id];
	
#endregion archetypes

#region entities

	/// <summary>check if entity is alive</summary>
	public static bool IsAlive(EntityId entity) => entity.id != uint.MaxValue && entity.id < _entities.count && _entities[entity].isAlive;
	
	private int CreateEmptyEntity() {
		lock (_globalLock) {
			int c = _entities.count;

			if (deadEntityCount == 0) {
				globalEntityCount++;
				currentEntityCount++;
				return _entities.Add(new(c, worldId, EntityFlags.isAlive));
			}
			
			globalEntityCount++;
			currentEntityCount++;

			for (int i = 0; i < c; i++) {
				if (_entities[i].isAlive) continue;
				_entities[i] = _entities[i] with {id = i, world = worldId, flags = EntityFlags.isAlive};
				return i;
			}

			throw new IndexOutOfRangeException();
		}
	}
	
	private void CreateEmptyEntities(int count, Action<EntityId> onCreation) {
		lock (_globalLock) {
			int c = _entities.count;

			for (int i = 0; i < c; i++) {
				if (deadEntityCount == 0) break;
				
				if (_entities[i].isAlive) continue;
				_entities[i] = new(i, worldId, EntityFlags.isAlive);
				onCreation(i);
				count--;
				globalEntityCount++;
				currentEntityCount++;
			}
			
			for (int i = 0; i < count; i++) {
				_entities.Add(new(c + i, worldId, EntityFlags.isAlive));
				onCreation(c + i);
				globalEntityCount++;
				currentEntityCount++;
			}
		}
	}

	/// <summary>create empty entity</summary>
	public EntityId CreateEntity() => CreateEmptyEntity();

	/// <summary>create entity with components</summary>
	public EntityId CreateEntity(Archetype archetype, InitMode initMode = InitMode.zeroed) {
		EntityId entity = CreateEntity();
		archetypes.AddEntity(entity, archetype);
		InitializeObject(archetype, entity, initMode);
		return entity;
	}

	/// <summary>initialize/reset components of entity</summary>
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

	/// <summary>create entity with components</summary>
	public EntityId CreateEntity(params Type[] archetype) => CreateEntity(GetArchetype(archetype)!);

	/// <summary>create multiple empty entities</summary>
	public void CreateEntities(int count, Action<EntityId> onCreation, InitMode initMode = InitMode.ctor) => CreateEmptyEntities(count, onCreation);

	/// <summary>create multiple entities with components</summary>
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
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			onCreation(i);
		});
	}
	
	private void CreateEntities_zeroed(int count, Archetype archetype, Action<EntityId> onCreation) {
		archetype.PreAllocate(count);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			InitializeObject(archetype, i, InitMode.zeroed);
			onCreation(i);
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
		
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);

			uint offset = archetype.GetComponentIdFromEntity(i);
			foreach ((IntPtr obj, IntPtr arr, int size) in components)
				QuartzNative.MemCpy((byte*)arr + offset * size, (void*) obj, (uint) size);
			
			onCreation(i);
		});
		
		for (int i = 0; i < compCount; i++) {
			MemoryAllocator.Free((void*) components[i].obj);
		}
	}

	private static void RemoveEmptyEntity(int index) {
		lock (_globalLock) {
			if (index == maxAliveEntityId) {
				globalEntityCount--;
				_entities[index].world.world.currentEntityCount--;
				index--;
				while (index >= 0 && !_entities[index].isAlive) index--;
				_entities.count = index + 1;
				return;
			}

			_entities[index].world.world.currentEntityCount--;
			_entities[index] = Entity.@null with { version = _entities[index].version + 1 };
			globalEntityCount--;
		}
	}

	/// <summary>destroy entity and components, if present</summary>
	public void DestroyEntity(EntityId id) {
		RemoveEmptyEntity(id);
		archetypes.RemoveEntity(id);
	}

	/// <summary>destroy all entities of specified archetype</summary>
	public void DestroyEntities(Archetype? archetype) {
		if (archetype == null) {
			DestroyEntitiesWhich(e => GetEntityArchetype(e) == null);
			return;
		}
		int c = archetype.components.entityCount;
		for (int i = 0; i < c; i++) {
			EntityId entity = archetype.components.entityComponentMap.GetKey((uint) i);
			DestroyEntity(entity);
		}
		
		archetype.Clear();
	}

	/// <summary>destroy all entities which match predicate</summary>
	public void DestroyEntitiesWhich(Predicate<EntityId> predicate) {
		int pos = 0;
		while (pos < _entities.count) {
			if (!_entities[pos].isAlive) {
				pos++;
				continue;
			}
			EntityId entity = pos;
			if (predicate(entity)) DestroyEntity(pos);
			else pos++;
		}
	}

	/// <summary>destroy all entities of this world</summary>
	public void Clear() {
		DestroyEntitiesWhich(e => e.worldId.id == worldId.id);
	}

	public unsafe EntityId Clone(EntityId src, bool recursive = true) {
		World srcWorld = src.world;
		Archetype? srcArchetype = srcWorld.GetEntityArchetype(src);

		if (srcArchetype == null) return CreateEntity();
		
		Archetype curArchetype = GetArchetype(srcArchetype.componentTypes)!;
		EntityId cur = CreateEntity(curArchetype);
		ArchetypeRoot.CopyEntityComponents(src, cur, srcArchetype, curArchetype);

		if (recursive && srcArchetype.ContainsComponent(typeof(HierarchyComponent))) {
			src.ForeachChild((hComp, entity) => {
				this.AddChild(cur, Clone(entity));
			});
		}
		
		return cur;
	}

#endregion entities

#region components

	/// <summary>get component, or create new</summary>
	public unsafe T* Comp<T>(EntityId id) where T : unmanaged, IComponent => archetypes.GetComponent<T>(id);
	
	/// <summary>get component, or create new</summary>
	public unsafe void* Comp(EntityId id, ComponentType t) => archetypes.GetComponent(id, t);

	/// <summary>get component, or return null if entity does not have it</summary>
	public unsafe T* TryComp<T>(EntityId id) where T : unmanaged, IComponent => archetypes.TryGetComponent<T>(id);
	
	/// <summary>get component, or return null if entity does not have it</summary>
	public unsafe void* TryComp(EntityId id, ComponentType t) => archetypes.TryGetComponent(id, t);

	/// <summary>try to remove component from entity</summary>
	public bool Remove<T>(EntityId id) where T : unmanaged, IComponent => archetypes.RemoveComponent<T>(id);
	
	/// <summary>try to remove component from entity</summary>
	public bool Remove(EntityId id, ComponentType t) => archetypes.RemoveComponent(id, t);

#endregion components

#region other

	/// <summary>lock current world</summary>
	public void Lock() => Monitor.Enter(_currentLock);
	
	/// <summary>try to lock current world</summary>
	public bool TryLock() => Monitor.TryEnter(_currentLock);

	/// <summary>unlock current world</summary>
	public void Unlock() => Monitor.Exit(_currentLock);

#endregion other

#region operators

	public static bool operator ==(World a, World b) => a.worldId.id == b.worldId.id;
	public static bool operator !=(World a, World b) => a.worldId.id != b.worldId.id;

#endregion operators
}