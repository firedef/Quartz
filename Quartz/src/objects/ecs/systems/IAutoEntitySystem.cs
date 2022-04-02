using Quartz.other.events;

namespace Quartz.objects.ecs.systems; 

public interface IAutoEntitySystem {
	public EventTypes types { get; }
	public bool repeating { get; }
	public bool continueInvoke { get; }
	public float lifetime { get; }
	public bool invokeWhileInactive { get; }
}