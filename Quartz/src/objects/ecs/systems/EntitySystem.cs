using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.systems; 

public abstract class EntitySystem {
	public abstract void Run(World world);
}