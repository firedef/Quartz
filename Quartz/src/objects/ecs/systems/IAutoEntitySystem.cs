using Quartz.CoreCs.other.events;

namespace Quartz.objects.ecs.systems; 

public interface IAutoEntitySystem {
	public EventTypes eventTypes { get; }
	public bool repeating { get; }
	public virtual bool continueInvoke => true;
	public virtual float lifetime => float.MaxValue;
	public virtual bool invokeWhileInactive => false;
}