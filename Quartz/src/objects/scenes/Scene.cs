using Quartz.objects.ecs.world;

namespace Quartz.objects.scenes; 

public class Scene {
	public World ecsWorld = World.Create();

	public void Destroy() {
		ecsWorld.Destroy();
	}
}