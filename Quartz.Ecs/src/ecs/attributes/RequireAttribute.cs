namespace Quartz.Ecs.ecs.attributes; 

[AttributeUsage(AttributeTargets.Struct)]
public class RequireAttribute : Attribute {
	public readonly Type[] values;
	public RequireAttribute(params Type[] values) => this.values = values;
}