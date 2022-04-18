namespace Quartz.Ecs.ecs.attributes; 

[AttributeUsage(AttributeTargets.Struct)]
public class ComponentAttribute : Attribute {
	public readonly string? name;
	public ComponentAttribute(string? name = null) => this.name = name;
}