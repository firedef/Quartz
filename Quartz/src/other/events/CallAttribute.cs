namespace Quartz.other.events; 

[AttributeUsage(AttributeTargets.Method)]
public class CallAttribute : Attribute {
	public EventTypes types;

	public CallAttribute(EventTypes types) => this.types = types;
}

[AttributeUsage(AttributeTargets.Method)]
public class CallRepeatingAttribute : Attribute {
	public EventTypes types;

	public CallRepeatingAttribute(EventTypes types) => this.types = types;
}