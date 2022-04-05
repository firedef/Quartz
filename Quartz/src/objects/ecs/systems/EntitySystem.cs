using Quartz.objects.ecs.world;

namespace Quartz.objects.ecs.systems; 

public abstract class EntitySystem {
	protected readonly object _executionLock = new();
	
	public virtual void Execute(World world, bool allowParallel = true) {
		if (!allowParallel) Monitor.Enter(_executionLock);
		isRunning = true;
		Run(world);
		isRunning = false;
		if (!allowParallel) Monitor.Exit(_executionLock);
	}

	public bool isRunning { get; protected set; }
	
	protected abstract void Run(World world);
}

public abstract class AsyncEntitySystem : EntitySystem {
	public override void Execute(World world, bool allowParallel = true) {
		if (allowParallel) {
			RunAsync(world);
			return;
		}

		lock (_executionLock) {
			isRunning = true;
			RunAsync(world).Wait();
			isRunning = false;
		}
	}

	protected override void Run(World world) => RunAsync(world);
	protected abstract Task RunAsync(World world);
}