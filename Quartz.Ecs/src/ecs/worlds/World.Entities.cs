using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.entities;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.worlds;

public partial class World {
	public EntityId AddEntity() {
		lock (_lock) {
			EntityId id = entities.Claim();
			Entity entity = new() {
				id = id,
				version = entities[id].version + 1,
				worldId = worldId,
				flags = EntityFlags.isAlive,
			};
			if (id > maxAnyEntityId) {
				maxAnyEntityId = id;
				entity.version = 0;
			}
			entities[id] = entity;
			
			entityCount++;
			totalEntityCount++;
			archetypes.entityArchetypeMap.Remove(id);
			OnEntityCreate(id);
			return id;
		}
	}
	
	public void AddEntities(int count, Action<EntityId> onCreate) {
		lock (_lock) {
			entityCount += count;
			totalEntityCount += count;
			entities.ClaimMultiple(count, id => {
				Entity entity = new() {
					id = id,
					version = entities[id].version + 1,
					worldId = worldId,
					flags = EntityFlags.isAlive,
				};
				if (id > maxAnyEntityId) {
					maxAnyEntityId = id;
					entity.version = 0;
				}
				entities[id] = entity;
				archetypes.entityArchetypeMap.Remove((uint)id);
				onCreate(id);
				OnEntityCreate(id);
			});
		}
	}
	
	public EntityId AddEntity(Archetype? archetype) {
		lock (_lock) {
			EntityId id = AddEntity();
			if (archetype != null) SetArchetype(id, archetype);
			return id;
		}
	}
	
	public void AddEntities(int count, Archetype? archetype, Action<EntityId> onCreate) {
		lock (_lock) {
			if (archetype == null) {
				AddEntities(count, onCreate);
				return;
			}
			
			entityCount += count;
			totalEntityCount += count;
			EntityId[] createdEntities = new EntityId[count];

			int i = 0;
			entities.ClaimMultiple(count, id => {
				Entity entity = new() {
					id = id,
					version = entities[id].version + 1,
					worldId = worldId,
					flags = EntityFlags.isAlive,
				};
				if (id > maxAnyEntityId) {
					maxAnyEntityId = id;
					entity.version = 0;
				}
				entities[id] = entity;
				archetypes.entityArchetypeMap[(uint)id] = archetype.id.position;
				OnEntityCreate(id);
				createdEntities[i] = id;
				i++;
			});
			
			archetypes.InitializeEntities(createdEntities, archetype);
			foreach (EntityId id in createdEntities) onCreate(id);
		}
	}

	public EntityId AddEntity(ComponentTypeArray normal, ComponentTypeArray shared) => AddEntity(GetArchetype(normal, shared));
	public EntityId AddEntity(params Type[] types) => AddEntity(GetArchetype(types));

	public EntityId AddEntity<T0>() where T0 : unmanaged, IEcsData => AddEntity(typeof(T0));
	public EntityId AddEntity<T0, T1>() 
		where T0 : unmanaged, IEcsData 
		=> AddEntity(typeof(T0),typeof(T1));

	public void SetArchetype(EntityId e, Archetype? arch) {
		lock (_lock) archetypes.MoveEntity(e, arch);
	}

	public static Entity GetEntity(EntityId e) {
		lock (_lock) return entities[e];
	}
	public static World GetWorld(EntityId e) {
		lock (_lock) return worlds.storage[GetEntity(e).worldId.position];
	}

	private unsafe void RemoveEntity(EntityId e) {
		int index = e;
		
		totalEntityCount--;
		GetWorld(e).entityCount--;
		Entity entity = entities[index];
		entity.flags = EntityFlags.none;
		entities[index] = entity;
		RemoveEntityName(e);
		
		if (e.position == entities.count - 1) {
			index--;
			while (index >= 0 && !entities[index].isAlive) index--;
			Console.WriteLine($"{entities.count} -> {index + 1}");
			entities.count = index + 1;
			return;
		}
		entities.ptr[index].version++;
		entities.RemoveAt(index);
	}

	public void DestroyEntity(EntityId e) {
		lock (_lock) {
			OnEntityDestroy(e);
			GetEntityArchetype(e)?.RemoveEntity(e);
			RemoveEntity(e);
		}
	}
	
	public static void RemoveEntityName(EntityId e) {
		lock (_lock) entityNames.Remove(e);
	}

	public static void SetEntityName(EntityId e, string? name) {
		lock (_lock) {
			if (name == null) RemoveEntityName(e);
			else entityNames[e] = name;
		}
	}
	
	public static string? TryGetEntityName(EntityId e) {
		lock (_lock) {
			entityNames.TryGetValue(e, out string? v);
			return v;
		}
	}

	//TODO: add hierarchy support
	public EntityId Clone(EntityId src) {
		lock (_lock) {
			World srcWorld = src.world;
			Archetype? srcArchetype = srcWorld.GetEntityArchetype(src);
			Archetype? trgtArchetype = srcWorld.worldId == worldId || srcArchetype == null ? srcArchetype : GetArchetype(srcArchetype); 
			EntityId cloneId = AddEntity(trgtArchetype);
		
			if (src.name != null) SetEntityName(cloneId, src.name);
			trgtArchetype?.CopyFrom(src, cloneId, srcArchetype!);

			return cloneId;
		}
	}
	
	public void CloneMultiple(EntityId src, int count, Action<EntityId> onClone) {
		lock (_lock) {
			World srcWorld = src.world;
			Archetype? srcArchetype = srcWorld.GetEntityArchetype(src);
			Archetype? trgtArchetype = srcWorld.worldId == worldId || srcArchetype == null ? srcArchetype : GetArchetype(srcArchetype); 
		
			AddEntities(count, trgtArchetype, cloneId => {
				if (src.name != null) SetEntityName(cloneId, src.name);
				trgtArchetype?.CopyFrom(src, cloneId, srcArchetype!);
				onClone(cloneId);
			});
		}
	}
}