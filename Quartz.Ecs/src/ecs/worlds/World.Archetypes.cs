using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.worlds;

public partial class World {
	public Archetype GetArchetypeAt(int id) {
		lock (_lock) return archetypes.archetypes[id];
	}

	public Archetype? GetArchetype(ComponentTypeArray normal, ComponentTypeArray shared) {
		lock (_lock) return archetypes.TryGetArchetype(normal, shared);
	}

	public Archetype? GetArchetype(params Type[] types) {
		lock (_lock) return archetypes.TryGetArchetype(types);
	}

	public Archetype? GetArchetype(Archetype archetype) {
		lock (_lock) return archetypes.TryGetArchetype(archetype.components._normal.types, archetype.components._shared.types);
	}

	public Archetype? GetEntityArchetype(EntityId e) {
		lock (_lock) return archetypes.TryGetArchetypeOfEntity(e);
	}

	public Archetype GetArchetype<T0>()
		where T0 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0))!;
	
	public Archetype GetArchetype<T0, T1>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1))!;
	
	public Archetype GetArchetype<T0, T1, T2>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2))!;
	
	public Archetype GetArchetype<T0, T1, T2, T3>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3))!;
	
	public Archetype GetArchetype<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4))!;
	
	public Archetype GetArchetype<T0, T1, T2, T3, T4, T5>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		where T5 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))!;
	
	public Archetype GetArchetype<T0, T1, T2, T3, T4, T5, T6>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		where T5 : unmanaged, IEcsData
		where T6 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))!;
	
	public Archetype GetArchetype<T0, T1, T2, T3, T4, T5, T6, T7>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		where T5 : unmanaged, IEcsData
		where T6 : unmanaged, IEcsData
		where T7 : unmanaged, IEcsData
		=> GetArchetype(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7))!;
}