using Quartz.Ecs.ecs.worlds;

namespace Quartz.objects.scenes; 

public class Scene {
	public string name;
	public readonly World world;

	public Scene(string name) {
		this.name = name;
		world = World.Create(name);
	}

	public void Destroy() {
		world.DestroyWorld();
	}
}