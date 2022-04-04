using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.filters;

namespace Quartz.objects.ecs.world; 

public partial class World {
	public void Foreach<T0>(EcsDelegates.ForeachPtrDelegate<T0> a)
		where T0 : unmanaged, IComponent 
		=> archetypes.Foreach(a);
	
	public void Foreach<T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a)
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		=> archetypes.Foreach(a);
	
	public void Foreach<T0, T1, T2>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2> a)
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		=> archetypes.Foreach(a);
	
	public void Foreach<T0, T1, T2, T3>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3> a)
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		=> archetypes.Foreach(a);
	
	public void Foreach<T0, T1, T2, T3, T4>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4> a)
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent 
		=> archetypes.Foreach(a);
	
	public void Foreach<T0, T1, T2, T3, T4, T5>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4, T5> a)
		where T0 : unmanaged, IComponent 
		where T1 : unmanaged, IComponent 
		where T2 : unmanaged, IComponent 
		where T3 : unmanaged, IComponent 
		where T4 : unmanaged, IComponent 
		where T5 : unmanaged, IComponent 
		=> archetypes.Foreach(a);
	
	public void Foreach<TFilter, T0>(EcsDelegates.ForeachPtrDelegate<T0> a)
		where TFilter : IEcsFilter, new()
		where T0 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T0>(a);
	
	public void Foreach<TFilter, T0, T1>(EcsDelegates.ForeachPtrDelegate<T0, T1> a)
		where TFilter : IEcsFilter, new()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T0, T1>(a);
	
	public void Foreach<TFilter, T0, T1, T2>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2> a)
		where TFilter : IEcsFilter, new()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T0, T1, T2>(a);
	
	public void Foreach<TFilter, T0, T1, T2, T3>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3> a)
		where TFilter : IEcsFilter, new()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T0, T1, T2, T3>(a);
	
	public void Foreach<TFilter, T0, T1, T2, T3, T4>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4> a)
		where TFilter : IEcsFilter, new()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T0, T1, T2, T3, T4>(a);
	
	public void Foreach<TFilter, T0, T1, T2, T3, T4, T5>(EcsDelegates.ForeachPtrDelegate<T0, T1, T2, T3, T4, T5> a)
		where TFilter : IEcsFilter, new()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T0, T1, T2, T3, T4, T5>(a);
}