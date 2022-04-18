using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.filters;
using Quartz.Ecs.ecs.queries;

namespace Quartz.Ecs.ecs.worlds; 

public partial class World {
	public Query Select() => new(new() {this});
	public static Query SelectStatic() => new(null);

	public Query Select<T0>()
		where T0 : unmanaged, IEcsData
		=> Select().Filter<All<T0>>();
	
	public static Query SelectStatic<T0>()
		where T0 : unmanaged, IEcsData
		=> SelectStatic().Filter<All<T0>>();
	
	public Query Select<T0, T1>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		=> Select().Filter<All<T0, T1>>();
	
	public static Query SelectStatic<T0, T1>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		=> SelectStatic().Filter<All<T0, T1>>();
	
	public Query Select<T0, T1, T2>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		=> Select().Filter<All<T0, T1, T2>>();
	
	public static Query SelectStatic<T0, T1, T2>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		=> SelectStatic().Filter<All<T0, T1, T2>>();
	
	public Query Select<T0, T1, T2, T3>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		=> Select().Filter<All<T0, T1, T2, T3>>();
	
	public static Query SelectStatic<T0, T1, T2, T3>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		=> SelectStatic().Filter<All<T0, T1, T2, T3>>();
	
	public Query Select<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		=> Select().Filter<All<T0, T1, T2, T3, T4>>();
	
	public static Query SelectStatic<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		=> SelectStatic().Filter<All<T0, T1, T2, T3, T4>>();
	
	public Query Select<T0, T1, T2, T3, T4, T5>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		where T5 : unmanaged, IEcsData
		=> Select().Filter<All<T0, T1, T2, T3, T4, T5>>();
	
	public static Query SelectStatic<T0, T1, T2, T3, T4, T5>()
		where T0 : unmanaged, IEcsData
		where T1 : unmanaged, IEcsData
		where T2 : unmanaged, IEcsData
		where T3 : unmanaged, IEcsData
		where T4 : unmanaged, IEcsData
		where T5 : unmanaged, IEcsData
		=> SelectStatic().Filter<All<T0, T1, T2, T3, T4, T5>>();
}