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
				if (world.isAlive && (world.isActive || auto.invokeWhileInactive)) sys.Run(world);
			}, () => world.isAlive && auto.continueInvoke, auto.types, auto.lifetime);
			return;
		}

		Dispatcher.global.PushMultiple(() => {
			if (world.isAlive) sys.Run(world);
		}, auto.types, auto.lifetime);
	}
	
	public static void Register<T>(this T sys, World world) where T : EntitySystem, IAutoEntitySystem {
		if (sys.repeating) {
			Dispatcher.global.PushRepeating(() => sys.Run(world), () => sys.continueInvoke, sys.types, sys.lifetime);
			return;
		}

		Dispatcher.global.PushMultiple(() => sys.Run(world), sys.types, sys.lifetime);
	}
}