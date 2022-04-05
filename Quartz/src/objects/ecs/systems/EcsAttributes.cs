using System.Reflection;
using Quartz.other.events;

namespace Quartz.objects.ecs.systems;

public class ExecuteByEventPipelineAttribute : Attribute {
	public int repeatDelay;
	public ExecuteByEventPipelineAttribute(int repeatDelay) => this.repeatDelay = repeatDelay;

	public static void ProcessAttributes(IEnumerable<Type> types) {
		foreach ((Type t, ExecuteByEventPipelineAttribute? attribute) in types.Select(v => (v, v.GetCustomAttribute<ExecuteByEventPipelineAttribute>()))) {
			if (attribute == null || !t.IsAssignableTo(typeof(EntitySystem))) continue;
			EntitySystem? sys = (EntitySystem?)Activator.CreateInstance(t);
			if (sys == null) continue;
			EventsPipeline.Execute(sys.ExecuteForEveryWorld, attribute.repeatDelay);
		}
	}
}