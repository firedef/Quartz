using Quartz.CoreCs.other;

namespace Quartz.Ecs.ecs.jobs; 

public record JobScheduleSettings {
	public static readonly JobScheduleSettings @default = new();
	public static readonly JobScheduleSettings @singleThreaded = new(multithreaded: false);
	public static readonly JobScheduleSettings @singleThreadedImmediate = new(multithreaded: false, loadBalancingTicks:0);
	public static readonly JobScheduleSettings @immediateNextUpdate = new(loadBalancingTicks:0);
	public static readonly JobScheduleSettings @immediateNow = new(executeNow:true);
	public static readonly JobScheduleSettings @background = new(waitForComplete:false, loadBalancingTicks:5);
	public static readonly JobScheduleSettings @anyWorld = new(interactableWorlds:State.any, activeWorlds:State.any, visibleWorlds:State.any);
	
	public bool executeNow = false;
	public bool multithreaded = true;
	public bool waitForComplete = true;
	public int tickDelay = 0;
	public int loadBalancingTicks = 2;
	public int minChunkSize = 65536;
	public int maxThreadCount = Environment.ProcessorCount >> 1;

	public State interactableWorlds = State.@on;
	public State activeWorlds = State.@on;
	public State visibleWorlds = State.@on;

	public JobScheduleSettings() { }

	public JobScheduleSettings(bool multithreaded = true, bool waitForComplete = true, int tickDelay = 0, int loadBalancingTicks = 2, int minChunkSize = 65536, int maxThreadCount = -1, State interactableWorlds = State.@on, State activeWorlds = State.@on, State visibleWorlds = State.@on, bool executeNow = false) {
		this.multithreaded = multithreaded;
		this.waitForComplete = waitForComplete;
		this.tickDelay = tickDelay;
		this.loadBalancingTicks = loadBalancingTicks;
		this.interactableWorlds = interactableWorlds;
		this.activeWorlds = activeWorlds;
		this.visibleWorlds = visibleWorlds;
		this.minChunkSize = minChunkSize;
		this.executeNow = executeNow;
		if (maxThreadCount != -1) this.maxThreadCount = maxThreadCount;
	}
}