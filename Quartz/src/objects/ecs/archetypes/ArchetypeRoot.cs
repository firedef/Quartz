using Quartz.CoreCs.native.collections;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.archetypes; 

public partial class ArchetypeRoot {
#region fields

	private readonly List<Archetype> _archetypes = new();
	private readonly IntMap _entityArchetypeIdMap = new();
	public readonly World owner;

	public IReadOnlyList<Archetype> archetypes => _archetypes;

	public int archetypeCount => _archetypes.Count;

#endregion fields

#region archetypes

	public ArchetypeRoot(World owner) => this.owner = owner;

	private Archetype AddArchetype(ComponentType[] types) {
		Archetype arch = new(types, this, (uint)(_archetypes.Count), owner);
		_archetypes.Add(arch);
		return arch;
	}

	private Archetype GetArchetype(EntityId id) => _archetypes[(int) _entityArchetypeIdMap[id.id]];
	
	/// <summary>
	/// get archetype for existing entity
	/// </summary>
	/// <param name="id">entity id</param>
	/// <returns>archetype of entity <br/>null if entity does not have components</returns>
	public Archetype? TryGetArchetype(EntityId id) {
		uint arch = _entityArchetypeIdMap[id.id];
		return arch == uint.MaxValue ? null : _archetypes[(int) arch];
	}
	
	/// <summary>
	/// get archetype for entity with specified components
	/// </summary>
	/// <param name="types">components</param>
	/// <returns>archetype of entity <br/>null if types is empty</returns>
	public Archetype? FindArchetype(ComponentType[] types) {
		int c = _archetypes.Count;
		int compCount = types.Length;
		for (int i = 0; i < c; i++) {
			Archetype arch = _archetypes[i];
			if (arch.componentTypes.Length == compCount && arch.ContainsArchetype(types)) return _archetypes[i];
		}

		return null;
	}

	/// <summary>
	/// get archetype for entity with specified components, or create new if does not exist
	/// </summary>
	/// <param name="types">components</param>
	/// <returns>archetype of entity <br/>null if types is empty</returns>
	public Archetype? FindOrCreateArchetype(ComponentType[] types) => types.Length == 0 ? null : FindArchetype(types) ?? AddArchetype(types);
	
	/// <summary>
	/// get archetype for entity with specified components, or create new if does not exist
	/// </summary>
	/// <param name="types">components</param>
	/// <returns>archetype of entity <br/>null if types is empty</returns>
	public Archetype? FindOrCreateArchetype(Type[] types) => FindOrCreateArchetype(types.ToEcsRequiredComponents());

	public Archetype FindOrCreateArchetype<T0>() => FindOrCreateArchetype(new[]{typeof(T0)})!;
	public Archetype FindOrCreateArchetype<T0, T1>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1)})!;
	public Archetype FindOrCreateArchetype<T0, T1, T2>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2)})!;
	public Archetype FindOrCreateArchetype<T0, T1, T2, T3>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2), typeof(T3)})!;
	public Archetype FindOrCreateArchetype<T0, T1, T2, T3, T4>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4)})!;

#endregion archetypes

#region entities

	/// <summary>
	/// clone components from 'id' existing entity to 'cloneId' existing entity
	/// </summary>
	/// <param name="id">source entity</param>
	/// <param name="cloneId">destination entity</param>
	/// <param name="dest">components to copy</param>
	public void CopyEntity(EntityId id, EntityId cloneId, Archetype dest) {
		Archetype? current = TryGetArchetype(id);
		if (current == null) _entityArchetypeIdMap[cloneId.id] = (uint) dest.components.Add(id);
		else _entityArchetypeIdMap[cloneId.id] = (uint) dest.components.AddFrom(id, dest, current);
	}
	
	public static void CopyEntityComponents(EntityId src, EntityId dest, Archetype srcArch, Archetype destArch) => destArch.components.CopyFrom(dest, src, destArch, srcArch);
	
	/// <summary>
	/// clone components from 'id' existing entity to 'cloneId' existing entity
	/// </summary>
	/// <param name="id">source entity</param>
	/// <param name="cloneId">destination entity</param>
	/// <param name="dest">components to copy</param>
	public void CopyEntity(EntityId id, EntityId cloneId, Type[] dest) => CopyEntity(id, cloneId, FindOrCreateArchetype(dest)!);
	
	private uint MoveEntity(EntityId id, Archetype? dest) {
		Archetype? current = TryGetArchetype(id);
		if (current == null) {
			return AddEntity(id, dest!);
		}
		if (dest == null) {
			RemoveEntity(id);
			return uint.MaxValue;
		}
		uint newComponentId = (uint) dest.components.AddFrom(id, dest, current);
		_entityArchetypeIdMap.Set(id.id, dest.id);
		current.components.RemoveByEntityId(id);
		return newComponentId;
	}

	private uint MoveEntity(EntityId id, ComponentType[] dest) => MoveEntity(id, FindOrCreateArchetype(dest));
	
	/// <summary>
	/// remove all components from existing entity
	/// </summary>
	/// <param name="id">entity id</param>
	public void RemoveEntity(EntityId id) {
		Archetype? current = TryGetArchetype(id);
		current?.components.RemoveByEntityId(id);
		_entityArchetypeIdMap.Remove(id.id);
	}

	/// <summary>
	/// add components of archetype to existing empty entity
	/// </summary>
	/// <param name="id">entity id</param>
	/// <param name="dest">target archetype</param>
	public uint AddEntity(EntityId id, Archetype dest) {
		int c = dest.components.Add(id);
		_entityArchetypeIdMap[id.id] = dest.id;
		return (uint) c;
	}
	
	/// <summary>
	/// add components to existing empty entity
	/// </summary>
	/// <param name="id">entity id</param>
	/// <param name="dest">components</param>
	public void AddEntity(EntityId id, params Type[] dest) => AddEntity(id, FindOrCreateArchetype(dest)!);

#endregion entities
	
#region components

	/// <summary>
	/// add component to existing entity
	/// </summary>
	/// <param name="id">entity</param>
	/// <param name="type">component</param>
	/// <returns>added component pointer</returns>
	public unsafe void* AddComponent(EntityId id, Type type) {
		Archetype? current = TryGetArchetype(id);
		ComponentType[] newTypes = current?.componentTypes.AddRequiredType(type) ?? type.ToEcsRequiredComponents();
		uint newCompId = MoveEntity(id, newTypes);

		return GetArchetype(id).GetComponent(type.ToEcsComponent(),newCompId);
	}
	
	/// <summary>
	/// add component to existing entity
	/// </summary>
	/// <param name="id">entity</param>
	/// <returns>added component pointer</returns>
	public unsafe T* AddComponent<T>(EntityId id) where T : unmanaged, IComponent {
		Archetype? current = TryGetArchetype(id);
		ComponentType[] newTypes = current?.componentTypes.AddRequiredType(typeof(T)) ?? typeof(T).ToEcsRequiredComponents();
		uint newCompId = MoveEntity(id, newTypes);

		T* ptr = (T*) GetArchetype(id).GetComponent(typeof(T).ToEcsComponent(),newCompId);
		*ptr = new();
		return ptr;
	}

	/// <summary>
	/// try to remove component from entity
	/// </summary>
	/// <param name="id">entity</param>
	/// <param name="type">component</param>
	/// <returns>true if successfully removed</returns>
	public bool RemoveComponent(EntityId id, Type type) => RemoveComponent(id, type.ToEcsComponent());

	/// <summary>
	/// try to remove component from entity
	/// </summary>
	/// <param name="id">entity</param>
	/// <param name="type">component</param>
	/// <returns>true if successfully removed</returns>
	public bool RemoveComponent(EntityId id, ComponentType type) {
		Archetype? current = TryGetArchetype(id);
		if (current == null || !current.ContainsComponent(type)) return false;

		ComponentType[] newTypes = current.componentTypes.RemoveRequiredType(type);
		MoveEntity(id, newTypes);
		return true;
	}
	
	/// <summary>
	/// try to remove component from entity
	/// </summary>
	/// <param name="id">entity</param>
	/// <returns>true if successfully removed</returns>
	public bool RemoveComponent<T>(EntityId id) where T : unmanaged, IComponent => RemoveComponent(id, typeof(T)); 

	/// <summary>
	/// get existing component or create new
	/// </summary>
	/// <param name="id">entity</param>
	/// <param name="type">component</param>
	/// <returns>pointer to component</returns>
	public unsafe void* GetComponent(EntityId id, Type type) {
		Archetype? archetype = TryGetArchetype(id);
		if (archetype == null) return AddComponent(id, type);
		void* ptr = archetype.GetComponent(type.ToEcsComponent(), id);
		return ptr == null ? AddComponent(id, type) : ptr;
	}
	
	/// <summary>
	/// get existing component or create new
	/// </summary>
	/// <param name="id">entity</param>
	/// <param name="type">component</param>
	/// <returns>pointer to component</returns>
	public unsafe void* GetComponent(EntityId id, ComponentType type) {
		Archetype? archetype = TryGetArchetype(id);
		if (archetype == null) return AddComponent(id, type.type);
		void* ptr = archetype.GetComponent(type, id);
		return ptr == null ? AddComponent(id, type.type) : ptr;
	}
	
	public unsafe void* TryGetComponent(EntityId id, ComponentType type) {
		Archetype? archetype = TryGetArchetype(id);
		return archetype == null ? null : archetype.GetComponent(type, id);
	}

	/// <summary>
	/// get existing component or create new
	/// </summary>
	/// <param name="id">entity</param>
	/// <returns>pointer to component</returns>
	public unsafe T* GetComponent<T>(EntityId id) where T : unmanaged, IComponent {
		Archetype? archetype = TryGetArchetype(id);
		if (archetype == null) return AddComponent<T>(id);
		void* ptr = archetype.GetComponent(typeof(T).ToEcsComponent(), id);
		return (T*) (ptr == null ? AddComponent<T>(id) : ptr);
	}

	/// <summary>
	/// get existing component or return null
	/// </summary>
	/// <param name="id">entity</param>
	/// <returns>pointer to component <br/>null if entity does not have component</returns>
	public unsafe T* TryGetComponent<T>(EntityId id) where T : unmanaged, IComponent {
		Archetype? archetype = TryGetArchetype(id);
		if (archetype == null) return null;
		return (T*) archetype.GetComponent(typeof(T).ToEcsComponent(), id);
	}

	public bool ContainsComponent(EntityId id, Type type) => GetArchetype(id).ContainsComponent(type.ToEcsComponent());

#endregion components
	
}