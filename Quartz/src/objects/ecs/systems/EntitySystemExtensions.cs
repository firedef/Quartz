using System.Reflection;
using Quartz.CoreCs.other.events;
using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.systems; 

public static class EntitySystemExtensions {
	public static void RegisterSystemsFromAssembly(this Assembly ass) {
		Type sysType = typeof(EntitySystem);
		Type autoSysType = typeof(IAutoEntitySystem);
		foreach (Type type in ass.GetTypes().Where(v => v.IsAssignableTo(sysType) && v.IsAssignableTo(autoSysType))) {
			if (Activator.CreateInstance(type) is not EntitySystem sys) continue;
			sys.Register();
		}
	}
	
	public static void Register(this EntitySystem sys) {
		if (sys is not IAutoEntitySystem auto) return;

		if (auto.repeating) {
			Dispatcher.global.PushRepeating(sys.Execute, () => auto.continueInvoke, auto.eventTypes, auto.lifetime);
			return;
		}

		Dispatcher.global.PushMultiple(() => {
			sys.Execute();
		}, auto.eventTypes, auto.lifetime);
	}
	
	// public static void Register<T>(this T sys, World world) where T : EntitySystem, IAutoEntitySystem {
	// 	if (sys.repeating) {
	// 		Dispatcher.global.PushRepeating(() => sys.Execute(world, false), () => sys.continueInvoke, sys.eventTypes, sys.lifetime);
	// 		return;
	// 	}
	//
	// 	Dispatcher.global.PushMultiple(() => sys.Execute(world), sys.eventTypes, sys.lifetime);
	// }
	
	public static void DispatchExecution(this EntitySystem sys, int delayTicks, int maxTickOffset = 5, float weight = 25, bool mainThread = false, bool waitForComplete = true) {
		FixedUpdatePipeline.EnqueueWithDelay(sys.Execute, $"{sys.GetType().FullName}.Execute()", delayTicks, maxTickOffset, mainThread, weight, waitForComplete);
	}

	public static void ExecuteByDispatcher(this EntitySystem sys, EventTypes type) => Dispatcher.global.Push(sys.Execute, type);
}