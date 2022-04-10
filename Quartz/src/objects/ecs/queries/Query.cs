using MathStuff;
using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.filters;
using Quartz.objects.ecs.world;
using Quartz.utils;

namespace Quartz.objects.ecs.queries; 

public record Query {
	
}

public record Query<T0> : Query where T0 : unmanaged, IComponent {
	protected Predicate<World>? worldWhere;
	
	protected Predicate<Archetype>? archetypeWhere;
	protected Predicate<Archetype>? archetypeWhile;
	protected Predicate<Archetype>? archetypeSkipWhile;

	/// <summary>not implemented!</summary>
	protected Predicate<EntityId>? entityWhere;
	
	/// <summary>not implemented!</summary>
	protected Predicate<EntityId>? entityWhile;
	
	/// <summary>not implemented!</summary>
	protected Predicate<EntityId>? entitySkipWhile;

	protected List<IEcsFilter> filters = new();

	/// <summary>not implemented!</summary>
	protected EcsDelegates.ComponentPredicate<T0>? componentWhere;
	
	/// <summary>not implemented!</summary>
	protected EcsDelegates.ComponentPredicate<T0>? componentWhile;
	
	/// <summary>not implemented!</summary>
	protected EcsDelegates.ComponentPredicate<T0>? componentSkipWhile;

	protected int skipCount = 0;
	protected int skipFromEndCount = 0;
	protected int takeCount = int.MaxValue;

	public Query<T0> Filter<TF>(TF val) where TF : IEcsFilter {
		filters.Add(val);
		return this;
	}

	public Query<T0> Filter<TF>() where TF : IEcsFilter, new() => Filter(new TF());

	public Query<T0> WhereWorld(Predicate<World> p) { worldWhere = (worldWhere, p).And(); return this; }
	
	public Query<T0> WhereArchetype(Predicate<Archetype> p) { archetypeWhere = (archetypeWhere, p).And(); return this; }
	public Query<T0> WhileArchetype(Predicate<Archetype> p) { archetypeWhile = (archetypeWhile, p).And(); return this; }
	public Query<T0> SkipWhileArchetype(Predicate<Archetype> p) { archetypeSkipWhile = (archetypeSkipWhile, p).And(); return this; }
	
	/// <summary>not implemented!</summary>
	public Query<T0> WhereEntity(Predicate<EntityId> p) { entityWhere = (entityWhere, p).And(); return this; }
	
	/// <summary>not implemented!</summary>
	public Query<T0> WhileEntity(Predicate<EntityId> p) { entityWhile = (entityWhile, p).And(); return this; }
	
	/// <summary>not implemented!</summary>
	public Query<T0> SkipWhileEntity(Predicate<EntityId> p) { entitySkipWhile = (entitySkipWhile, p).And(); return this; }
	
	/// <summary>not implemented!</summary>
	public Query<T0> WhereComponents(EcsDelegates.ComponentPredicate<T0> p) { componentWhere = (componentWhere, p).And(); return this; }
	
	/// <summary>not implemented!</summary>
	public Query<T0> WhileComponents(EcsDelegates.ComponentPredicate<T0> p) { componentWhile = (componentWhile, p).And(); return this; }
	
	/// <summary>not implemented!</summary>
	public Query<T0> SkipWhileComponents(EcsDelegates.ComponentPredicate<T0> p) { componentSkipWhile = (componentSkipWhile, p).And(); return this; }
	
	public Query<T0> Skip(int c) { skipCount = c; return this; }
	public Query<T0> SkipFromEnd(int c) { skipFromEndCount = c; return this; }
	public Query<T0> Take(int c) { takeCount = c; return this; }

	public void ForeachArchetype(Action<Archetype> a) {
		ComponentType[] types = typeof(T0).ToEcsRequiredComponents();
		
		World.ForeachWorld(world => {
			if (worldWhere != null && !worldWhere(world)) return;

			bool skip = archetypeSkipWhile != null;
			foreach (Archetype archetype in world.archetypes.archetypes) {
				if (skip && !archetypeSkipWhile!(archetype)) skip = false;
				if (archetypeWhile != null && !archetypeWhile(archetype)) return;
				if (skip) continue;
				if (archetypeWhere != null && !archetypeWhere(archetype)) continue;
				
				if (!archetype.ContainsArchetype(types)) continue;
				foreach (IEcsFilter filter in filters) 
					if (!filter.Filter(archetype)) goto SKIP;

				a(archetype);
			SKIP: ;
			}
		}, false);
	}

	public void Foreach(EcsDelegates.ComponentDelegate<T0> a) {
		ComponentType t0 = typeof(T0).ToEcsComponent();
		
		ForeachArchetype(archetype => {
			int i0 = archetype.IndexOfComponent(t0);
			archetype.components.Foreach(a, i0, skipCount, skipFromEndCount, takeCount);
		});
	}
	
	public unsafe void DestroyEntities(EcsDelegates.ComponentPredicate<T0> a) {
		ComponentType t0 = typeof(T0).ToEcsComponent();
		
		ForeachArchetype(archetype => {
			int i0 = archetype.IndexOfComponent(t0);
			
			int c = math.min(archetype.components.components[i0].count - skipFromEndCount, takeCount + skipCount);
			T0* ptr0 = (T0*) archetype.components.components[i0].rawData;

			for (int i = skipCount; i < c; i++) {
				if (!a(ptr0 + i)) continue;
				archetype.owner.DestroyEntity(archetype.components.entityComponentMap.GetKey((uint) i));
				i--;
				c--;
			}
		});
	}

	public void DestroyEntities() {
		ForeachArchetype(archetype => {
			archetype.owner.DestroyEntities(archetype);
		});
	}
}