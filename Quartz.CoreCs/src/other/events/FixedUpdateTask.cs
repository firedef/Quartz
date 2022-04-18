namespace Quartz.CoreCs.other.events; 

public class FixedUpdateTask {
	public readonly int tick;
	public bool isComplete { get; private set; }
	public readonly bool isLong;
	
	public FixedUpdateTask(int tick, bool isLong = false) {
		this.tick = tick;
		this.isLong = isLong;
	}

	internal void SetComplete() => isComplete = true;
}