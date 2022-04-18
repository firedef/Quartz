namespace Quartz.CoreCs.other.events; 

public class FixedUpdateEventSettings {
	public Action action;
	public string? name;
	public int invokeTick;
	public int balanceTicks;
	public float weight;
	public bool executeByMainThread;
	public bool waitForComplete;
	public FixedUpdateTask? executeAfter;
}