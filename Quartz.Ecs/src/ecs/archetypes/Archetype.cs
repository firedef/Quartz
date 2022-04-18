using MathStuff;
using Quartz.CoreCs.other;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.components.data;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.archetypes; 

public partial class Archetype {
	public color archetypeColor;
	public readonly ArchetypeComponents components;

	public readonly ArchetypeId id;
	public readonly ArchetypeRoot owner;

	public int entityCount => components.elementCount;
	public int allocatedEntityCount => components.allocatedElementCount;
	public int componentCount => normalComponentCount + sharedComponentCount;
	public int normalComponentCount => components.normalComponentCount;
	public int sharedComponentCount => components.sharedComponentCount;

	public Archetype(ComponentTypeArray normal, ComponentTypeArray shared, ArchetypeRoot root, int id) {
		components = new(this, normal, shared);
		owner = root;
		this.id = id;
		archetypeColor = color.FromHsl((Rand.val, 1f, .65f));
	}

	public bool ContainsComponentId(ComponentId id) => components.ContainsComponentId(id);
	public bool ContainsComponent(ComponentType t) => IndexOfComponent(t) != -1;
	public bool ContainsArchetype(Archetype arch) => components.ContainsArchetype(arch.components);
	public bool ContainsArchetype(ComponentTypeArray normal, ComponentTypeArray shared) => components.ContainsNormalComponents(normal) && components.ContainsSharedComponents(shared);

	public int IndexOfComponent(ComponentType t) => t.data.kind == ComponentKind.normal ? components.IndexOfNormalComponent(t) : components.IndexOfSharedComponent(t);
	
	public void Clear() => components.Clear();
	public void Cleanup() => components.Trim();
}