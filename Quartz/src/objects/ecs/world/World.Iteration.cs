using MathStuff;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;

namespace Quartz.objects.ecs.world; 

public partial class World {
	public void ForeachComponent<T>(Action<T> a) where T : unmanaged, IComponent {
		ComponentCollection<T> collection = GetComponentCollection<T>();
		foreach (T component in collection) a(component);
	}
	
	public void ForeachComponent<T>(Action<T, ComponentId> a) where T : unmanaged, IComponent {
		ComponentCollection<T> collection = GetComponentCollection<T>();
		int c = collection.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection.components.emptyIndices.Contains(i)) continue;
			a(collection.components[i], (uint) i);
		}
	}
	
	public unsafe void ForeachComponent<T1, T2>(Action<T1, T2, EntityId> a) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		int c = collection1.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection1.components.emptyIndices.Contains(i)) continue;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			if (!comp2.isValid) continue;

			a(*collection1[comp1], *collection2[comp2], entity);
		}
	}
	
	public unsafe void ForeachComponent<T1, T2, T3>(Action<T1, T2, T3, EntityId> a) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent where T3 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		ComponentCollection<T3> collection3 = GetComponentCollection<T3>();
		int c = collection1.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection1.components.emptyIndices.Contains(i)) continue;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			ComponentId comp3 = collection3.GetComponentFromEntity(entity);
			if (!comp2.isValid || !comp3.isValid) continue;

			a(*collection1[comp1], *collection2[comp2], *collection3[comp3], entity);
		}
	}
	
	public unsafe void ForeachComponent<T1, T2, T3, T4>(Action<T1, T2, T3, T4, EntityId> a) 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		ComponentCollection<T3> collection3 = GetComponentCollection<T3>();
		ComponentCollection<T4> collection4 = GetComponentCollection<T4>();
		int c = collection1.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection1.components.emptyIndices.Contains(i)) continue;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			ComponentId comp3 = collection3.GetComponentFromEntity(entity);
			ComponentId comp4 = collection4.GetComponentFromEntity(entity);
			if (!comp2.isValid || !comp3.isValid || !comp4.isValid) continue;

			a(*collection1[comp1], *collection2[comp2], *collection3[comp3], *collection4[comp4], entity);
		}
	}
	
	public void ParallelForeachComponent<T>(Action<T> a) where T : unmanaged, IComponent {
		ComponentCollection<T> collection = GetComponentCollection<T>();
		int c = collection.totalCount;

		Parallel.For(0, c, i => {
			if (collection.components.emptyIndices.Contains(i)) return;
			a(collection.components[i]);
		});
	}
	
	public void ParallelForeachComponent<T>(Action<T, ComponentId> a) where T : unmanaged, IComponent {
		ComponentCollection<T> collection = GetComponentCollection<T>();
		int c = collection.totalCount;

		Parallel.For(0, c, i => {
			if (collection.components.emptyIndices.Contains(i)) return;
			a(collection.components[i], (uint) i);
		});
	}
	
	public unsafe void ParallelForeachComponent<T1, T2>(Action<T1, T2, EntityId> a) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		int c = collection1.totalCount;

		Parallel.For(0, c, i => {
			if (collection1.components.emptyIndices.Contains(i)) return;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			if (!comp2.isValid) return;
			
			a(*collection1[comp1], *collection2[comp2], entity);
		});
	}
	
	public unsafe void ParallelForeachComponent<T1, T2, T3>(Action<T1, T2, T3, EntityId> a) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent where T3 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		ComponentCollection<T3> collection3 = GetComponentCollection<T3>();
		int c = collection1.totalCount;

		Parallel.For(0, c, i => {
			if (collection1.components.emptyIndices.Contains(i)) return;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			ComponentId comp3 = collection3.GetComponentFromEntity(entity);
			if (!comp2.isValid || !comp3.isValid) return;
			
			a(*collection1[comp1], *collection2[comp2], *collection3[comp3], entity);
		});
	}
	
	public unsafe void ParallelForeachComponent<T1, T2, T3, T4>(Action<T1, T2, T3, T4, EntityId> a) 
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		ComponentCollection<T3> collection3 = GetComponentCollection<T3>();
		ComponentCollection<T4> collection4 = GetComponentCollection<T4>();
		int c = collection1.totalCount;

		Parallel.For(0, c, i => {
			if (collection1.components.emptyIndices.Contains(i)) return;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			ComponentId comp3 = collection3.GetComponentFromEntity(entity);
			ComponentId comp4 = collection4.GetComponentFromEntity(entity);
			if (!comp2.isValid || !comp3.isValid || !comp4.isValid) return;
			
			a(*collection1[comp1], *collection2[comp2], *collection3[comp3], *collection4[comp4], entity);
		});
	}
	
	public unsafe void ParallelForeachComponent<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5, EntityId> a) 
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent{
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		ComponentCollection<T3> collection3 = GetComponentCollection<T3>();
		ComponentCollection<T4> collection4 = GetComponentCollection<T4>();
		ComponentCollection<T5> collection5 = GetComponentCollection<T5>();
		int c = collection1.totalCount;

		Parallel.For(0, c, i => {
			if (collection1.components.emptyIndices.Contains(i)) return;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			ComponentId comp3 = collection3.GetComponentFromEntity(entity);
			ComponentId comp4 = collection4.GetComponentFromEntity(entity);
			ComponentId comp5 = collection5.GetComponentFromEntity(entity);
			if (!comp2.isValid || !comp3.isValid || !comp4.isValid || !comp5.isValid) return;
			
			a(*collection1[comp1], *collection2[comp2], *collection3[comp3], *collection4[comp4], *collection5[comp5], entity);
		});
	}

	public unsafe delegate void ForeachComponentPtrDelegate<T1>(T1* c1, ComponentId componentId) where T1 : unmanaged, IComponent;
	public unsafe delegate void ForeachComponentPtrDelegate<T1, T2>(T1* c1, T2* c2, EntityId entityId) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent;
	public unsafe delegate void ForeachComponentPtrDelegate<T1, T2, T3>(T1* c1, T2* c2, T3* c3, EntityId entityId) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent where T3 : unmanaged, IComponent;
	
	public unsafe void ForeachComponentPtr<T1>(ForeachComponentPtrDelegate<T1> a) where T1 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		int c = collection1.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection1.components.emptyIndices.Contains(i)) continue;
			ComponentId comp1 = (uint)i;
			a(collection1[comp1], comp1);
		}
	}
	
	public unsafe void ForeachComponentPtr<T1, T2>(ForeachComponentPtrDelegate<T1, T2> a) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		int c = collection1.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection1.components.emptyIndices.Contains(i)) continue;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			if (!comp2.isValid) return;
			a(collection1[comp1], collection2[comp2], entity);
		}
	}
	
	public unsafe void ForeachComponentPtr<T1, T2, T3>(ForeachComponentPtrDelegate<T1, T2, T3> a) where T1 : unmanaged, IComponent where T2 : unmanaged, IComponent where T3 : unmanaged, IComponent {
		ComponentCollection<T1> collection1 = GetComponentCollection<T1>();
		ComponentCollection<T2> collection2 = GetComponentCollection<T2>();
		ComponentCollection<T3> collection3 = GetComponentCollection<T3>();
		int c = collection1.totalCount;
		for (int i = 0; i < c; i++) {
			if (collection1.components.emptyIndices.Contains(i)) continue;
			ComponentId comp1 = (uint)i;
			EntityId entity = collection1.GetEntityFromComponent(comp1);
			ComponentId comp2 = collection2.GetComponentFromEntity(entity);
			ComponentId comp3 = collection3.GetComponentFromEntity(entity);
			if (!comp2.isValid || !comp3.isValid) return;
			a(collection1[comp1], collection2[comp2], collection3[comp3], entity);
		}
	}
}