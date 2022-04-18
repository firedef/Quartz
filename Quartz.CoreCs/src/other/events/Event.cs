using Quartz.CoreCs.other.time;

namespace Quartz.CoreCs.other.events; 

public class Event {
	public Action action;
	public float addedTime = (float) Time.elapsedSecondsRealTime;
	public float lifetime;
	public EventPriority priority;

	public Event(Action action, float lifetime = float.MaxValue, EventPriority priority = EventPriority.normal) {
		this.action = action;
		this.lifetime = lifetime;
		this.priority = priority;
	}
}

public class RepeatingEvent {
	public Action action;
	public float addedTime = (float) Time.elapsedSecondsRealTime;
	public float lifetime;
	public EventPriority priority;

	public Func<bool> continueInvoke;

	public RepeatingEvent(Action action, Func<bool> continueInvoke, float lifetime = float.MaxValue, EventPriority priority = EventPriority.normal) {
		this.action = action;
		this.lifetime = lifetime;
		this.priority = priority;
		this.continueInvoke = continueInvoke;
	}
}