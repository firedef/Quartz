using Quartz.CoreCs.other.events;

namespace Quartz.Ecs.ecs.jobs; 

public static partial class JobScheduler {
	private const int _minChunkSize = 8192;
	private const int _sliceCount = 8;

	public static void Schedule(this EntitiesJobBase job, JobScheduleSettings settings) {
		FixedUpdatePipeline.EnqueueWithDelay(job.Run, null, settings.tickDelay, settings.loadBalancingTicks, !settings.multithreaded, waitForComplete: settings.waitForComplete);
	}
}