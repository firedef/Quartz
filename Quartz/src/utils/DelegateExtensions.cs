using Quartz.Ecs.ecs.components;

namespace Quartz.utils; 

// public static class DelegateExtensions {
// 	public static Predicate<T> And<T>(this (Predicate<T>? a, Predicate<T>? b) predicates) {
// 		if (predicates.a == null) return predicates.b!;
// 		if (predicates.b == null) return predicates.a!;
// 		return v => predicates.a(v) && predicates.b(v);
// 	}
// 	
// 	public static unsafe EcsDelegates.ComponentPredicate<T0> And<T0>(this (EcsDelegates.ComponentPredicate<T0>? a, EcsDelegates.ComponentPredicate<T0>? b) predicates) 
// 		where T0 : unmanaged, IComponent {
// 		if (predicates.a == null) return predicates.b!;
// 		if (predicates.b == null) return predicates.a!;
// 		return v => predicates.a(v) && predicates.b(v);
// 	}
// }