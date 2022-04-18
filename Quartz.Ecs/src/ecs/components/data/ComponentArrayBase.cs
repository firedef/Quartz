namespace Quartz.Ecs.ecs.components.data; 

public abstract class ComponentArrayBase {
	public readonly ComponentTypeArray types;

	protected ComponentArrayBase(ComponentTypeArray types) => this.types = types;
}