using Quartz.core.collections;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.filters;

namespace Quartz.objects.ecs.archetypes; 

public class ArchetypeRoot {
	public readonly List<Archetype> archetypes = new();
	public readonly IntMap entityArchetypeId = new();
	
#region archetypes

	public Archetype AddArchetype(Type[] types) {
		Archetype arch = new(types, this, (uint)(archetypes.Count));
		archetypes.Add(arch);
		return arch;
	}

	public Archetype GetArchetype(EntityId id) => archetypes[(int) entityArchetypeId[id.id]];
	public Archetype? TryGetArchetype(EntityId id) {
		uint arch = entityArchetypeId[id.id];
		if (arch == uint.MaxValue) return null;
		return archetypes[(int) arch];
	}

	public Archetype? FindArchetype(Type[] types) {
		int c = archetypes.Count;
		int compCount = types.Length;
		for (int i = 0; i < c; i++) {
			Archetype arch = archetypes[i];
			if (arch.componentTypes.Length == compCount && arch.ContainsArchetype(types)) return archetypes[i];
		}

		return null;
	}

	public Archetype FindOrCreateArchetype(Type[] types) => FindArchetype(types) ?? AddArchetype(types);

	public Archetype FindOrCreateArchetype<T0>() => FindOrCreateArchetype(new[]{typeof(T0)});
	public Archetype FindOrCreateArchetype<T0, T1>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1)});
	public Archetype FindOrCreateArchetype<T0, T1, T2>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2)});
	public Archetype FindOrCreateArchetype<T0, T1, T2, T3>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2), typeof(T3)});
	public Archetype FindOrCreateArchetype<T0, T1, T2, T3, T4>() => FindOrCreateArchetype(new[]{typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4)});

#endregion archetypes

#region entities

	public void CopyEntity(EntityId id, EntityId cloneId, Archetype dest) {
		Archetype current = GetArchetype(id);
		entityArchetypeId[cloneId.id] = (uint) dest.components.AddFrom(id, dest, current);
	}

	public void CopyEntity(EntityId id, EntityId cloneId, Type[] dest) => CopyEntity(id, cloneId, FindOrCreateArchetype(dest));
	
	public uint MoveEntity(EntityId id, Archetype dest) {
		Archetype current = GetArchetype(id);
		uint newComponentId = (uint) dest.components.AddFrom(id, dest, current);
		entityArchetypeId[id.id] = dest.id;
		current.components.RemoveByEntityId(id);
		return newComponentId;
	}

	public uint MoveEntity(EntityId id, Type[] dest) => MoveEntity(id, FindOrCreateArchetype(dest));
	
	public void RemoveEntity(EntityId id) {
		Archetype current = GetArchetype(id);
		current.components.RemoveByEntityId(id);
		entityArchetypeId.Remove(id.id);
	}

	public void AddEntity(EntityId id, Archetype dest) {
		dest.components.Add(id);
		entityArchetypeId[id.id] = dest.id;
	}
	
	public void AddEntity(EntityId id, params Type[] dest) => AddEntity(id, FindOrCreateArchetype(dest));

#endregion entities
	
#region components

	public unsafe void* AddComponent(EntityId id, Type type) {
		Archetype current = GetArchetype(id);
		Type[] newTypes = current.componentTypes.Append(type).ToArray();
		uint newCompId = MoveEntity(id, newTypes);

		return GetArchetype(id).GetComponent(type,newCompId);
	}

	public bool RemoveComponent(EntityId id, Type type) {
		Archetype current = GetArchetype(id);
		if (!current.ContainsComponent(type)) return false;
		
		Type[] newTypes = current.componentTypes.Where(v => v != type).ToArray();
		MoveEntity(id, newTypes);
		return true;
	}

	public bool RemoveComponent<T>(EntityId id) where T : unmanaged, IComponent => RemoveComponent(id, typeof(T)); 

	public unsafe void* GetComponent(EntityId id, Type type) {
		void* ptr = GetArchetype(id).GetComponent(type, id.id);
		return ptr == null ? AddComponent(id, type) : ptr;
	}
	
	public unsafe T* GetComponent<T>(EntityId id) where T : unmanaged, IComponent {
		void* ptr = GetArchetype(id).GetComponent(typeof(T), id.id);
		return (T*) (ptr == null ? AddComponent(id, typeof(T)) : ptr);
	}

	public bool ContainsComponent(EntityId id, Type type) => GetArchetype(id).ContainsComponent(type);

#endregion components
	
#region iteration

	public void Foreach<T0>(EcsDelegates.ForeachPtrDelegate<T0> a) where T0 : unmanaged, IComponent {
		Type t0 = typeof(T0);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0));
		}
	}
	
	public void Foreach<T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0) && arch.ContainsComponent(t1)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1));
		}
	}
	
	public void Foreach<T0, T1, T2>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2> a) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2));
		}
	}
	
	public void Foreach<T0, T1, T2, T3>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3> a) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		Type t3 = typeof(T3);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2) && arch.ContainsComponent(t3)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2), arch.IndexOfComponent(t3));
		}
	}
	
	public void Foreach<T0, T1, T2, T3, T4>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4> a) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		Type t3 = typeof(T3);
		Type t4 = typeof(T4);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2) && arch.ContainsComponent(t3) && arch.ContainsComponent(t4)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2), arch.IndexOfComponent(t3), arch.IndexOfComponent(t4));
		}
	}
	
	public void Foreach<T0, T1, T2, T3, T4, T5>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4, T5> a) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		Type t3 = typeof(T3);
		Type t4 = typeof(T4);
		Type t5 = typeof(T5);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2) && arch.ContainsComponent(t3) && arch.ContainsComponent(t4) && arch.ContainsComponent(t5)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2), arch.IndexOfComponent(t3), arch.IndexOfComponent(t4), arch.IndexOfComponent(t5));
		}
	}

	public void Foreach<TFilter, T0>(EcsDelegates.ForeachPtrDelegate<T0> a)
		where TFilter : IEcsFilter, new() 
		where T0 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (EcsFilters.Filter<TFilter>(arch) && arch.ContainsComponent(t0)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0));
		}
	}
	
	public void Foreach<TFilter, T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a)
		where TFilter : IEcsFilter, new() 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (EcsFilters.Filter<TFilter>(arch) && arch.ContainsComponent(t0) && arch.ContainsComponent(t1)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1));
		}
	}
	
	public void Foreach<TFilter, T0, T1, T2>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2> a)
		where TFilter : IEcsFilter, new() 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (EcsFilters.Filter<TFilter>(arch) && arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2));
		}
	}
	
	public void Foreach<TFilter, T0, T1, T2, T3>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3> a)
		where TFilter : IEcsFilter, new() 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		Type t3 = typeof(T3);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (EcsFilters.Filter<TFilter>(arch) && arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2) && arch.ContainsComponent(t3)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2), arch.IndexOfComponent(t3));
		}
	}
	
	public void Foreach<TFilter, T0, T1, T2, T3, T4>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4> a)
		where TFilter : IEcsFilter, new() 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		Type t3 = typeof(T3);
		Type t4 = typeof(T4);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (EcsFilters.Filter<TFilter>(arch) && arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2) && arch.ContainsComponent(t3) && arch.ContainsComponent(t4)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2), arch.IndexOfComponent(t3), arch.IndexOfComponent(t4));
		}
	}
	
	public void Foreach<TFilter, T0, T1, T2, T3, T4, T5>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4, T5> a)
		where TFilter : IEcsFilter, new() 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent 
		where T5 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		Type t2 = typeof(T2);
		Type t3 = typeof(T3);
		Type t4 = typeof(T4);
		Type t5 = typeof(T5);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (EcsFilters.Filter<TFilter>(arch) && arch.ContainsComponent(t0) && arch.ContainsComponent(t1) && arch.ContainsComponent(t2) && arch.ContainsComponent(t3) && arch.ContainsComponent(t4) && arch.ContainsComponent(t5)) 
				arch.components.Foreach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1), arch.IndexOfComponent(t2), arch.IndexOfComponent(t3), arch.IndexOfComponent(t4), arch.IndexOfComponent(t5));
		}
	}
	
	public void ParallelForeach<T0>(EcsDelegates.ForeachPtrDelegate<T0> a) where T0 : unmanaged, IComponent {
		Type t0 = typeof(T0);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0)) 
				arch.components.ParallelForeach(a, arch.IndexOfComponent(t0));
		}
	}
	
	public void ParallelForeach<T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
	{
		Type t0 = typeof(T0);
		Type t1 = typeof(T1);
		
		int ch = archetypes.Count;
		for (int i = 0; i < ch; i++) {
			Archetype arch = archetypes[i];
			if (arch.ContainsComponent(t0) && arch.ContainsComponent(t1)) 
				arch.components.ParallelForeach(a, arch.IndexOfComponent(t0), arch.IndexOfComponent(t1));
		}
	}
	
#endregion iteration
}