namespace Quartz.objects.ecs.components.attributes; 

[AttributeUsage(AttributeTargets.Struct)]
public class RequireAttribute : Attribute {
	public Type[] types;
	public RequireAttribute(params Type[] types) => this.types = types;
}