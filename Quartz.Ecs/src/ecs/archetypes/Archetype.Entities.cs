using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.archetypes;

public partial class Archetype {
	public bool ContainsEntityId(EntityId id) => components.ContainsEntityId(id);
	public ComponentId AddEntity(EntityId id) => components.Add(id);
	public void AddEntities(EntityId[] entities) => components.AddMultiple(entities);
	public bool RemoveEntity(EntityId id, bool dispose = true) {
		owner.entityArchetypeMap.Remove(id);
		return components.Remove(id, dispose);
	}

	public ComponentId GetComponent(EntityId id) => components.GetComponent(id);
	public EntityId GetEntity(ComponentId id) => components.GetEntity(id);
	
	public unsafe void* GetComponentArrayPtr(ComponentType t) => components.GetComponents(t);
	public unsafe void* GetElement(ComponentType t, ComponentId index) => components.GetElement(t, index);
	public unsafe void* GetComponentPtr(EntityId entity, ComponentType t) => components.GetElement(t, components.GetComponent(entity));
	

	public void CopyFrom(EntityId src, EntityId trgt, Archetype srcArchetype) {
		components.CopyFrom(GetComponent(trgt), srcArchetype.GetComponent(src), srcArchetype.components);
	}
	
	public void CopyFromAndDisposeOld(EntityId src, EntityId trgt, Archetype srcArchetype) {
		components.CopyFromAndDisposeOld(GetComponent(trgt), srcArchetype.GetComponent(src), srcArchetype.components);
	}
}