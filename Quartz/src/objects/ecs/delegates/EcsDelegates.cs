using Quartz.objects.ecs.components;

namespace Quartz.objects.ecs.delegates; 

public static class EcsDelegates {
	public unsafe delegate void ForeachPtrDelegate<T0>(T0* c0) where T0 : unmanaged, IComponent;
	
	public unsafe delegate void ForeachPtrDelegate<T0, T1>(T0* c0, T1* c1) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent;
	
	public unsafe delegate void ForeachPtrDelegate<T0, T1, T2>(T0* c0, T1* c1, T2* c2) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent;
	
	public unsafe delegate void ForeachPtrDelegate<T0, T1, T2, T3>(T0* c0, T1* c1, T2* c2, T3* c3) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent;
	
	public unsafe delegate void ForeachPtrDelegate<T0, T1, T2, T3, T4>(T0* c0, T1* c1, T2* c2, T3* c3, T4* c4) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent;
	
	public unsafe delegate void ForeachPtrDelegate<T0, T1, T2, T3, T4, T5>(T0* c0, T1* c1, T2* c2, T3* c3, T4* c4, T5* c5) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent;
}