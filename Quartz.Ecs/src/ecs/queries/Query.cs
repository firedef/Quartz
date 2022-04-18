using Quartz.CoreCs.other;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.filters;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs.queries;

public class Query {
	protected readonly List<IEcsFilter> filters = new();
	protected readonly List<World>? worlds;
	
	protected State interactableWorlds = State.@on;
	protected State activeWorlds = State.@on;
	protected State visibleWorlds = State.@on;

	public Query(List<World>? worlds) => this.worlds = worlds;

	protected virtual bool FilterArchetype(Archetype archetype) {
		int c = filters.Count;
		for (int i = 0; i < c; i++)
			if (!filters[i].Filter(archetype))
				return false;
		return true;
	}
	
	public Query Filter<TFilter>(TFilter filter) where TFilter : IEcsFilter {
		filters.Add(filter);
		return this;
	}

	public Query Filter<TFilter>() where TFilter : IEcsFilter, new() => Filter(new TFilter());

	public Query Where(Predicate<Archetype> predicate) => Filter<PredicateFilter>(new(predicate));

	public Query Worlds(State interactable = State.@on, State active = State.@on, State visible = State.@on) {
		interactableWorlds = interactable;
		activeWorlds = active;
		visibleWorlds = visible;
		return this;
	}

	public QueryResult Result() {
		QueryResult result = new();
		FilterAll(result);
		return result;
	}

	private void FilterAll(QueryResult result) {
		if (worlds == null) {
			int c = World.totalWorldCount;
			for (int i = 0; i < c; i++) {
				World? world = World.GetWorldAt(i);
				if (world != null) FilterWorld(world, result);
			}
			return;
		}

		foreach (World w in worlds) FilterWorld(w, result);
	}

	private void FilterWorld(World world, QueryResult result) {
		if (world is not {isAlive: true} || 
		    !interactableWorlds.Check(world.isInteractable) || 
		    !activeWorlds.Check(world.isActive) || 
		    !visibleWorlds.Check(world.isVisible)) return;
		
		int c = world.archetypeCount;
		for (int i = 0; i < c; i++) {
			Archetype archetype = world.GetArchetypeAt(i);
			if (FilterArchetype(archetype)) result.archetypes.Add(archetype);
		}
	}
}