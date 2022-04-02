using System.Reflection;

namespace Quartz.other.events;

public static partial class EventManager {
	private static HashSet<Assembly> processedAssemblies = new();
	
	public static void ProcessCurrentAssembly() => ProcessAssembly(Assembly.GetCallingAssembly());

	public static void ProcessAssembly(Assembly asm) {
		if (processedAssemblies.Contains(asm)) return;
		processedAssemblies.Add(asm);

		MethodInfo[] methods = asm.GetTypes().SelectMany(v => v.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).ToArray();
		ProcessCallAttributes(methods);
		ProcessRepeatCallAttributes(methods);
	}

	private static void ProcessCallAttributes(IEnumerable<MethodInfo> methods) {
		IEnumerable<(MethodInfo, IEnumerable<CallAttribute>)> callAttributes = methods.Select(v => (v, v.GetCustomAttributes<CallAttribute>()));

		foreach ((MethodInfo method, IEnumerable<CallAttribute> attributes) in callAttributes)
			foreach (CallAttribute callAttribute in attributes)
				Dispatcher.global.PushMultiple(() => method.Invoke(null, null), callAttribute.types);
	}
	
	private static void ProcessRepeatCallAttributes(IEnumerable<MethodInfo> methods) {
		IEnumerable<(MethodInfo, IEnumerable<CallRepeatingAttribute>)> callAttributes = methods.Select(v => (v, v.GetCustomAttributes<CallRepeatingAttribute>()));

		foreach ((MethodInfo method, IEnumerable<CallRepeatingAttribute> attributes) in callAttributes)
		foreach (CallRepeatingAttribute callAttribute in attributes)
			Dispatcher.global.PushMultipleRepeating(() => method.Invoke(null, null), callAttribute.types);
	}
}