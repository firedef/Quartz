using System.Reflection;

namespace Quartz.CoreCs.other.events;

public static partial class EventManager {
	private static readonly HashSet<Assembly> _processedAssemblies = new();

	public static void ProcessCurrentAssembly() {
		ProcessAssembly(Assembly.GetExecutingAssembly());
		ProcessAssembly(Assembly.GetCallingAssembly());
	}

	public static void ProcessAssembly(Assembly asm) {
		if (_processedAssemblies.Contains(asm)) return;
		_processedAssemblies.Add(asm);

		Type[] types = asm.GetTypes();
		MethodInfo[] methods = types.SelectMany(v => v.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).ToArray();
		ProcessCallAttributes(methods);
		ProcessRepeatCallAttributes(methods);
		ExecuteOnceAttribute.ProcessAttributes(methods);
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