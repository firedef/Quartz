using Quartz.CoreCs.native.collections;
using Quartz.Ecs.collections;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.archetypes; 

public class ArchetypeRoot {
	public static event Action<Archetype> OnArchetypeCreate = _ => { };
	public static event Action<EntityId, Archetype?, Archetype?> OnEntityMove = (_,_,_) => { };
	
	public readonly List<Archetype> archetypes = new();
	public readonly Dictionary<uint, uint> entityArchetypeMap = new();
	//public readonly IntMap entityArchetypeMap = new();
	
	public int archetypeCount => archetypes.Count;

	private Archetype? CreateArchetype(ComponentTypeArray normal, ComponentTypeArray shared) {
		if (normal.componentCount == 0 && shared.componentCount == 0) return null;
		Archetype archetype = new(normal, shared, this, archetypeCount);
		archetypes.Add(archetype);
		OnArchetypeCreate(archetype);
		return archetype;
	}

	public Archetype? FindArchetype(ComponentTypeArray normal, ComponentTypeArray shared) {
		int c = archetypeCount;
		int normalCount = normal.componentCount;
		int sharedCount = shared.componentCount;
		for (int i = 0; i < c; i++)
			if (archetypes[i].normalComponentCount == normalCount && archetypes[i].sharedComponentCount == sharedCount && archetypes[i].ContainsArchetype(normal, shared))
				return archetypes[i];
		return null;
	}

	public Archetype? TryGetArchetype(ComponentTypeArray normal, ComponentTypeArray shared) {
		if (normal.componentCount == 0 && shared.componentCount == 0) return null;
		return FindArchetype(normal, shared) ?? CreateArchetype(normal, shared);
	}
	
	public Archetype? TryGetArchetype(ComponentType t) {
		Archetype? arch = t.data.kind == ComponentKind.normal ? FindArchetype(new(t), ComponentTypeArray.empty) : FindArchetype(ComponentTypeArray.empty, new(t));
		return arch ?? CreateArchetype(t.data.type.GetRequiredNormalComponents(), t.data.type.GetRequiredSharedComponents());
	}

	public Archetype? TryGetArchetype(params Type[] types) {
		(ComponentTypeArray normal, ComponentTypeArray shared) = GetComponentTypes(types);
		return TryGetArchetype(normal, shared);
	}

	public Archetype? TryGetArchetypeOfEntity(EntityId entityId) {
		return entityArchetypeMap.TryGetValue(entityId, out uint id) ? archetypes[(int)id] : null;
	}

	private static (ComponentTypeArray normal, ComponentTypeArray shared) GetComponentTypes(Type[] types) => (types.GetNormalComponents(), types.GetSharedComponents());

	public void MoveEntity(EntityId entity, Archetype? trgt) => MoveEntity(entity, TryGetArchetypeOfEntity(entity), trgt);

	public void InitializeEntities(EntityId[] entities, Archetype trgt) {
		trgt.AddEntities(entities);
		foreach (EntityId e in entities) {
			entityArchetypeMap[e] = trgt.id.position;
			OnEntityMove(e, null, trgt);
		}
	}
	
	public void MoveEntity(EntityId entity, Archetype? src, Archetype? trgt) {
		if (src == trgt) return;
		if (trgt == null) {
			if (src == null) return;
			src.RemoveEntity(entity);
			entityArchetypeMap.Remove(entity);
			OnEntityMove(entity, src, trgt);
			return;
		}
		if (src == null) {
			trgt.AddEntity(entity);
			entityArchetypeMap[entity] = trgt.id.position;
			OnEntityMove(entity, src, trgt);
			return;
		}

		trgt.AddEntity(entity);
		trgt.CopyFromAndDisposeOld(entity, entity, src);
		src.RemoveEntity(entity, false);
		entityArchetypeMap[entity] = trgt.id.position;
		OnEntityMove(entity, src, trgt);
	}

	public unsafe void* GetComponent(EntityId entityId, ComponentType type) {
		Archetype? cur = TryGetArchetypeOfEntity(entityId);
		return cur == null ? null : cur.GetElement(type, cur.GetComponent(entityId));
	}
	
	public unsafe void* TryAddComponent(EntityId entityId, ComponentType type) {
		Archetype? cur = TryGetArchetypeOfEntity(entityId);
		if (cur == null) {
			MoveEntity(entityId, null, TryGetArchetype(type));
			return GetComponent(entityId, type);
		}
		if (cur.ContainsComponent(type)) return null;

		Archetype trgt = TryGetArchetype(
			ComponentTypeArray.Merge(cur.components._normal.types, type.data.type.GetRequiredNormalComponents()), 
			ComponentTypeArray.Merge(cur.components._shared.types, type.data.type.GetRequiredSharedComponents()))!;
		
		MoveEntity(entityId, cur, trgt);
		return GetComponent(entityId, type);
	}
	
	public unsafe void* GetOrAddComponent(EntityId entityId, ComponentType type) {
		Archetype? cur = TryGetArchetypeOfEntity(entityId);
		if (cur == null) {
			MoveEntity(entityId, null, TryGetArchetype(type));
			return GetComponent(entityId, type);
		}
		if (cur.ContainsComponent(type)) return GetComponent(entityId, type);

		Archetype trgt = TryGetArchetype(
			ComponentTypeArray.Merge(cur.components._normal.types, type.data.type.GetRequiredNormalComponents()), 
			ComponentTypeArray.Merge(cur.components._shared.types, type.data.type.GetRequiredSharedComponents()))!;
		
		MoveEntity(entityId, cur, trgt);
		return GetComponent(entityId, type);
	}

	public bool RemoveComponent(EntityId entityId, ComponentType type) {
		Archetype? cur = TryGetArchetypeOfEntity(entityId);
		if (cur == null || !cur.ContainsComponent(type)) return false;

		if ((type.data.kind == ComponentKind.normal && cur.normalComponentCount == 1 && cur.sharedComponentCount == 0) || (type.data.kind == ComponentKind.shared && cur.normalComponentCount == 0 && cur.sharedComponentCount == 1)) {
			MoveEntity(entityId, cur, null);
			return true;
		}
		
		Archetype trgt = TryGetArchetype(
			ComponentTypeArray.Remove(cur.components._normal.types, type.data.type.Get(), ComponentKind.normal), 
			ComponentTypeArray.Remove(cur.components._shared.types, type.data.type.Get(), ComponentKind.shared))!;
		
		MoveEntity(entityId, cur, trgt);
		
		return true;
	}

	public unsafe void SetComponent(EntityId entity, ComponentType type, Dictionary<string, string> fields) {
		GetOrAddComponent(entity, type);
		Archetype archetype = TryGetArchetypeOfEntity(entity)!;
		if (type.data.kind == ComponentKind.shared) {
			if (fields.TryGetValue("id", out string? v)) *(ushort*)GetComponent(entity, type) = ushort.Parse(v);
			return;
		}

		int index = archetype.GetComponent(entity);
		EcsList ecsList = archetype.components._normal.data[archetype.components.IndexOfNormalComponent(type)];
		ecsList.Parse(index, fields);
	}
}