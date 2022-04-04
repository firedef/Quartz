using Quartz.objects.ecs.archetypes;

namespace Quartz.objects.ecs.filters;

public interface IEcsFilter {
	public bool Filter(Archetype archetype);
}

public static class EcsFilters {
	public static bool Filter<T1>(Archetype arch) where T1 : IEcsFilter, new() => new T1().Filter(arch);
	public static bool Filter<T1>(T1 filter, Archetype arch) where T1 : IEcsFilter, new() => filter.Filter(arch);
}