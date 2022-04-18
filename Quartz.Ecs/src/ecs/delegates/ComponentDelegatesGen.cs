using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.delegates;

public unsafe delegate void ComponentDelegateS1<T0>(ushort* t0)
	where T0 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0>(EntityId entity, ushort* t0)
	where T0 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0>(ushort* t0)
	where T0 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1>(ushort* t0, ushort* t1)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1>(EntityId entity, ushort* t0, ushort* t1)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1>(ushort* t0, ushort* t1)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2>(ushort* t0, ushort* t1, ushort* t2)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2>(EntityId entity, ushort* t0, ushort* t1, ushort* t2)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2>(ushort* t0, ushort* t1, ushort* t2)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3>(ushort* t0, ushort* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3>(EntityId entity, ushort* t0, ushort* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3>(ushort* t0, ushort* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0>(T0* t0)
	where T0 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0>(EntityId entity, T0* t0)
	where T0 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0>(T0* t0)
	where T0 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1>(T0* t0, ushort* t1)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1>(EntityId entity, T0* t0, ushort* t1)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1>(T0* t0, ushort* t1)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2>(T0* t0, ushort* t1, ushort* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2>(EntityId entity, T0* t0, ushort* t1, ushort* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2>(T0* t0, ushort* t1, ushort* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3>(T0* t0, ushort* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3>(EntityId entity, T0* t0, ushort* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3>(T0* t0, ushort* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4>(T0* t0, ushort* t1, ushort* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4>(EntityId entity, T0* t0, ushort* t1, ushort* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4>(T0* t0, ushort* t1, ushort* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1>(T0* t0, T1* t1)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1>(EntityId entity, T0* t0, T1* t1)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1>(T0* t0, T1* t1)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2>(T0* t0, T1* t1, ushort* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2>(EntityId entity, T0* t0, T1* t1, ushort* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2>(T0* t0, T1* t1, ushort* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3>(T0* t0, T1* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3>(EntityId entity, T0* t0, T1* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3>(T0* t0, T1* t1, ushort* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4>(T0* t0, T1* t1, ushort* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4>(EntityId entity, T0* t0, T1* t1, ushort* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4>(T0* t0, T1* t1, ushort* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, ushort* t2, ushort* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5>(EntityId entity, T0* t0, T1* t1, ushort* t2, ushort* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, ushort* t2, ushort* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1, T2>(T0* t0, T1* t1, T2* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1, T2>(EntityId entity, T0* t0, T1* t1, T2* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1, T2>(T0* t0, T1* t1, T2* t2)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2, T3>(T0* t0, T1* t1, T2* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2, T3>(EntityId entity, T0* t0, T1* t1, T2* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2, T3>(T0* t0, T1* t1, T2* t2, ushort* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3, T4>(T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3, T4>(EntityId entity, T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3, T4>(T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4, T5>(EntityId entity, T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5, T6>(EntityId entity, T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, ushort* t3, ushort* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1, T2, T3>(T0* t0, T1* t1, T2* t2, T3* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1, T2, T3>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1, T2, T3>(T0* t0, T1* t1, T2* t2, T3* t3)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2, T3, T4>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2, T3, T4>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2, T3, T4>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3, T4, T5>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4, T5, T6>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, ushort* t4, ushort* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1, T2, T3, T4>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1, T2, T3, T4>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1, T2, T3, T4>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2, T3, T4, T5>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3, T4, T5, T6>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, ushort* t5, ushort* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1, T2, T3, T4, T5>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1, T2, T3, T4, T5>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2, T3, T4, T5, T6>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, ushort* t6, ushort* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1, T2, T3, T4, T5, T6>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1, T2, T3, T4, T5, T6>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2, T3, T4, T5, T6, T7>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7, T8>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8, ushort* t9, ushort* t10)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8, ushort* t9, ushort* t10)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, ushort* t7, ushort* t8, ushort* t9, ushort* t10)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegate<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent;

public unsafe delegate void ComponentEntityDelegate<T0, T1, T2, T3, T4, T5, T6, T7>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent;

public unsafe delegate bool ComponentPredicate<T0, T1, T2, T3, T4, T5, T6, T7>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent;

public unsafe delegate void ComponentDelegateS1<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS1<T0, T1, T2, T3, T4, T5, T6, T7, T8>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS1<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS2<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9, ushort* t10)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9, ushort* t10)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9, ushort* t10)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9, ushort* t10, ushort* t11)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent
	where T11 : unmanaged, ISharedComponent;

public unsafe delegate void ComponentEntityDelegateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(EntityId entity, T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9, ushort* t10, ushort* t11)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent
	where T11 : unmanaged, ISharedComponent;

public unsafe delegate bool ComponentPredicateS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0* t0, T1* t1, T2* t2, T3* t3, T4* t4, T5* t5, T6* t6, T7* t7, ushort* t8, ushort* t9, ushort* t10, ushort* t11)
	where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent
	where T11 : unmanaged, ISharedComponent;