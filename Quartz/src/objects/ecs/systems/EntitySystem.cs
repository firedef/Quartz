using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.systems; 

public abstract class EntitySystem {
	protected readonly object _executionLock = new();
	
	public virtual void Execute() {
		Run();
	}

	public bool isRunning { get; protected set; }
	
	protected abstract void Run();
}