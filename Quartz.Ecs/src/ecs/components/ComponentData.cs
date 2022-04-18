using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.components; 

public class ComponentData {
	public Type type;
	public string? name;
	public ComponentKind kind;
	public ComponentType identifier;
	public ComponentType[] requiredTypes = Array.Empty<ComponentType>();

	public ComponentData(Type type, ComponentKind kind, ComponentType identifier) {
		this.type = type;
		this.kind = kind;
		this.identifier = identifier;
	}
}