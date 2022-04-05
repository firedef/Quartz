using System.Reflection;
using Quartz.objects.ecs.world;
using Quartz.other.events;

namespace Quartz.objects.ecs.systems; 

public static class EntitySystemExtensions {
	public static void RegisterSystemsFromAssembly(this World world, Assembly ass) {
		Type sysType = typeof(EntitySystem);
		Type autoSysType = typeof(IAutoEntitySystem);
		foreach (Type type in ass.GetTypes().Where(v => v.IsAssignableTo(sysType) && v.IsAssignableTo(autoSysType))) {
			if (Activator.CreateInstance(type) is not EntitySystem sys) continue;
			sys.Register(world);
		}
	}
	
	public static void Register(this EntitySystem sys, World world) {
		if (sys is not IAutoEntitySystem auto) return;

		if (auto.repeating) {
			Dispatcher.global.PushRepeating(() => {
				if (world.isAlive && (world.isActive || auto.invokeWhileInactive)) sys.Execute(world);
			}, () => world.isAlive && auto.continueInvoke, auto.eventTypes, auto.lifetime);
			return;
		}

		Dispatcher.global.PushMultiple(() => {
			if (world.isAlive) sys.Execute(world);
		}, auto.eventTypes, auto.lifetime);
	}
	
	public static void Register<T>(this T sys, World world) where T : EntitySystem, IAutoEntitySystem {
		if (sys.repeating) {
			Dispatcher.global.PushRepeating(() => sys.Execute(world, false), () => sys.continueInvoke, sys.eventTypes, sys.lifetime);
			return;
		}

		Dispatcher.global.PushMultiple(() => sys.Execute(world), sys.eventTypes, sys.lifetime);
	}

	public static void ExecuteForEveryWorld(this EntitySystem sys) {
		World.ForeachWorld(w => sys.Execute(w));
	}

	public static void ExecuteByDispatcher(this EntitySystem sys, World world, EventTypes type) => Dispatcher.global.Push(() => sys.Execute(world), type);
}