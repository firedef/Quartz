using Quartz.Ecs.ecs.archetypes;

namespace Quartz.Ecs.ecs.filters; 

public interface IEcsFilter {
	public bool Filter(Archetype archetype);
}

public readonly struct PredicateFilter : IEcsFilter {
	private readonly Predicate<Archetype> _predicate;
	public PredicateFilter(Predicate<Archetype> predicate) => _predicate = predicate;
	public bool Filter(Archetype archetype) => _predicate(archetype);
}