using System.Reflection;

namespace Quartz.CoreCs.other.events;

[AttributeUsage(AttributeTargets.Method)]
public class ExecuteOnceAttribute : Attribute {
	public int startTick;
	public int maxTickDelay;
	public bool runInMainThread;
	public bool waitForComplete;
	public float weight;

	public ExecuteOnceAttribute(int startTick = 10, int maxTickDelay = 10, bool runInMainThread = true, bool waitForComplete = true, float weight = 25) {
		this.startTick = startTick;
		this.maxTickDelay = maxTickDelay;
		this.runInMainThread = runInMainThread;
		this.waitForComplete = waitForComplete;
		this.weight = weight;
	}

	public static void ProcessAttributes(IEnumerable<MethodInfo> methods) {
		foreach ((MethodInfo t, ExecuteOnceAttribute? attribute) in methods.Select(v => (v, v.GetCustomAttribute<ExecuteOnceAttribute>()))) {
			if (attribute == null) continue;
			Action @delegate = t.CreateDelegate<Action>();
			FixedUpdatePipeline.Enqueue(@delegate, $"ExecuteOnce:{t.DeclaringType!.FullName}.{t.Name}()", attribute.startTick, attribute.maxTickDelay, attribute.runInMainThread, attribute.weight, attribute.waitForComplete);
		}
	}
}