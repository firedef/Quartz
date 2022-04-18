using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.components;

public interface IEcsData {
	public void Parse(Dictionary<string, string> fields);
	public void Write(Dictionary<string, string> fields);
}
public interface IComponent : IEcsData {}
public interface ISharedComponent : IEcsData {}

public interface ICloneableComponent {
	public unsafe void OnClone(void* src);
}