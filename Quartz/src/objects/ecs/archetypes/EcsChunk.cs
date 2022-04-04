using System.Diagnostics;
using Quartz.collections;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;

namespace Quartz.objects.ecs.archetypes; 

public class EcsChunk {
	public DualIntMap entityComponentMap = new();
	public EcsList[] components;
	public int componentsPerEntity => components.Length;
	public int entityCount => components.Length == 0 ? 0 : components[0].count;

	public EcsChunk(EcsList[] components) => this.components = components;

	public int Add(EntityId entity) {
		int c = components.Length;
		for (int i = 0; i < c; i++) components[i].Add();
		entityComponentMap[entity.id] = (uint) (entityCount - 1);
		
		return entityCount - 1;
	}
	
	public unsafe int AddFrom(EntityId entity, Archetype current, Archetype src) {
		int id = entityCount;
		int c = components.Length;

		int otherComponentId = (int) src.components.entityComponentMap[entity.id];

		for (int i = 0; i < c; i++) {
			int copyFromIndex = src.IndexOfComponent(current.componentTypes[i]);
			if (copyFromIndex == -1) {
				components[i].Add();
				continue;
			}

			int offset = otherComponentId * components[i].elementSize;
			components[i].AddFrom((byte*) src.components.components[copyFromIndex].rawData + offset);
		}
		
		entityComponentMap[entity.id] = (uint) id;
		return id;
	}

	public void RemoveByComponentId(int index) {
		int c = componentsPerEntity;
		int e = entityCount;

		entityComponentMap.RemoveByVal((uint) index);
		
		if (index == e - 1) {
			for (int i = 0; i < c; i++) components[i].count = e - 1;
			return;
		}
		
		uint lastEntityId = entityComponentMap.GetKey((uint)e - 1);
		for (int i = 0; i < c; i++) components[i].ReplaceByLast(index);
		entityComponentMap[lastEntityId] = (uint) index;
		//Console.WriteLine(lastEntityId + " " + index + " " + e);
	}
	
	// struct HierarchyComponent
	// 		EntityId parent
	// 		EntityId nextSibling
	// 		EntityId nextChild
	public void RemoveByEntityId(EntityId entity) {
		int index = (int) entityComponentMap[entity.id];
		//if (index == -1) Debugger.Break();
		RemoveByComponentId(index);
	}
	
#region iteration

	public unsafe void Foreach<T0>(EcsDelegates.ForeachPtrDelegate<T0> a, int i0) where T0 : unmanaged, IComponent {
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;

		for (int i = 0; i < c; i++) a(ptr0 + i);
	}
	
	public unsafe void Foreach<T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a, int i0, int i1) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
	{
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;
		T1* ptr1 = (T1*) components[i1].rawData;

		for (int i = 0; i < c; i++) a(ptr0 + i, ptr1 + i);
	}
	
	public unsafe void Foreach<T0, T1, T2>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2> a, int i0, int i1, int i2) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
	{
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;
		T1* ptr1 = (T1*) components[i1].rawData;
		T2* ptr2 = (T2*) components[i2].rawData;

		for (int i = 0; i < c; i++) a(ptr0 + i, ptr1 + i, ptr2 + i);
	}
	
	public unsafe void Foreach<T0, T1, T2, T3>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
	{
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;
		T1* ptr1 = (T1*) components[i1].rawData;
		T2* ptr2 = (T2*) components[i2].rawData;
		T3* ptr3 = (T3*) components[i3].rawData;

		for (int i = 0; i < c; i++) a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i);
	}
	
	public unsafe void Foreach<T0, T1, T2, T3, T4>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent 
	{
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;
		T1* ptr1 = (T1*) components[i1].rawData;
		T2* ptr2 = (T2*) components[i2].rawData;
		T3* ptr3 = (T3*) components[i3].rawData;
		T4* ptr4 = (T4*) components[i4].rawData;

		for (int i = 0; i < c; i++) a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i);
	}
	
	public unsafe void Foreach<T0, T1, T2, T3, T4, T5>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent 
		where T5 : unmanaged, IComponent 
	{
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;
		T1* ptr1 = (T1*) components[i1].rawData;
		T2* ptr2 = (T2*) components[i2].rawData;
		T3* ptr3 = (T3*) components[i3].rawData;
		T4* ptr4 = (T4*) components[i4].rawData;
		T5* ptr5 = (T5*) components[i5].rawData;

		for (int i = 0; i < c; i++) a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i);
	}

	public unsafe void ParallelForeach<T0>(EcsDelegates.ForeachPtrDelegate<T0> a, int i0) where T0 : unmanaged, IComponent {
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;

		Parallel.For(0, c, i => a(ptr0 + i));
	}
	
	public unsafe void ParallelForeach<T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a, int i0, int i1) 
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
	{
		int c = components[i0].count;
		T0* ptr0 = (T0*) components[i0].rawData;
		T1* ptr1 = (T1*) components[i1].rawData;

		Parallel.For(0, c, i => a(ptr0 + i, ptr1 + i));
	}
	
#endregion iteration
}