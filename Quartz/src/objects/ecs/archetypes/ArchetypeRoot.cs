using Quartz.core.collections;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;

namespace Quartz.objects.ecs.archetypes; 

public partial class ArchetypeRoot {
#region fields

	private readonly List<Archetype> _archetypes = new();
	private readonly IntMap _entityArchetypeIdMap = new();

	public IReadOnlyList<Archetype> archetypes => _archetypes;

	public int archetypeCount => _archetypes.Count;

#endregion fields

#region archetypes

	private Archetype AddArchetype(ComponentType[] types) {
		Archetype arch = new(types, this, (uint)(_archetypes.Count));
		_archetypes.Add(arch);
		return arch;
	}

	private Archetype GetArchetype(EntityId id) => _archetypes[(int) _entityArchetypeIdMap[id.id]];
	
	public Archetype? TryGetArchetype(EntityId id) {
		uint arch = _entityArchetypeIdMap[id.id];
		return arch == uint.MaxValue ? null : _archetypes[(int) arch];
	}

	public Archetype? FindArchetype(ComponentType[] types) {
		int c = _archetypes.Count;
		int compCount = types.Length;
		for (int i = 0; i < c; i++) {
			Archetype arch = _archetypes[i];
			if (arch.componentTypes.Length == compCount && arch.ContainsArchetype(types)) return _archetypes[i];
		}

		return null;
	}

	public Archetype FindOrCreateArchetype(ComponentType[] types) => FindArchetype(types) ?? AddArchetype(types);
	public Archetype FindOrCreateArchetype(Type[] types) => FindOrCreateArchetype(types.ToEcsRequiredComponents());

	public Archetype FindOrCreateArchetype<T0>() => FindOrCreateArchetype(new[]{typeof(T0)});
	public Archetype FindOrCreateArchetype<T0, T1>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1)});
	public Archetype FindOrCreateArchetype<T0, T1, T2>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2)});
	public Archetype FindOrCreateArchetype<T0, T1, T2, T3>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2), typeof(T3)});
	public Archetype FindOrCreateArchetype<T0, T1, T2, T3, T4>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4)});

#endregion archetypes

#region entities

	public void CopyEntity(EntityId id, EntityId cloneId, Archetype dest) {
		Archetype current = GetArchetype(id);
		_entityArchetypeIdMap[cloneId.id] = (uint) dest.components.AddFrom(id, dest, current);
	}

	public void CopyEntity(EntityId id, EntityId cloneId, Type[] dest) => CopyEntity(id, cloneId, FindOrCreateArchetype(dest));
	
	private uint MoveEntity(EntityId id, Archetype dest) {
		Archetype current = GetArchetype(id);
		uint newComponentId = (uint) dest.components.AddFrom(id, dest, current);
		_entityArchetypeIdMap[id.id] = dest.id;
		current.components.RemoveByEntityId(id);
		return newComponentId;
	}

	private uint MoveEntity(EntityId id, ComponentType[] dest) => MoveEntity(id, FindOrCreateArchetype(dest));
	
	public void RemoveEntity(EntityId id) {
		Archetype current = GetArchetype(id);
		current.components.RemoveByEntityId(id);
		_entityArchetypeIdMap.Remove(id.id);
	}

	public void AddEntity(EntityId id, Archetype dest) {
		dest.components.Add(id);
		_entityArchetypeIdMap[id.id] = dest.id;
	}
	
	public void AddEntity(EntityId id, params Type[] dest) => AddEntity(id, FindOrCreateArchetype(dest));

#endregion entities
	
#region components

	public unsafe void* AddComponent(EntityId id, Type type) {
		Archetype current = GetArchetype(id);
		ComponentType[] newTypes = current.componentTypes.AddRequiredType(type);
		uint newCompId = MoveEntity(id, newTypes);

		return GetArchetype(id).GetComponent(type.ToEcsComponent(),newCompId);
	}

	public bool RemoveComponent(EntityId id, Type type) {
		Archetype current = GetArchetype(id);
		if (!current.ContainsComponent(type.ToEcsComponent())) return false;

		ComponentType[] newTypes = current.componentTypes.RemoveRequiredType(type);
		MoveEntity(id, newTypes);
		return true;
	}

	public bool RemoveComponent<T>(EntityId id) where T : unmanaged, IComponent => RemoveComponent(id, typeof(T)); 

	public unsafe void* GetComponent(EntityId id, Type type) {
		void* ptr = GetArchetype(id).GetComponent(type.ToEcsComponent(), id.id);
		return ptr == null ? AddComponent(id, type) : ptr;
	}
	
	public unsafe T* GetComponent<T>(EntityId id) where T : unmanaged, IComponent {
		void* ptr = GetArchetype(id).GetComponent(typeof(T).ToEcsComponent(), id.id);
		return (T*) (ptr == null ? AddComponent(id, typeof(T)) : ptr);
	}

	public bool ContainsComponent(EntityId id, Type type) => GetArchetype(id).ContainsComponent(type.ToEcsComponent());

#endregion components
	
}