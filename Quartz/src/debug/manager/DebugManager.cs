using System.Reflection;

namespace Quartz.debug.manager; 

public static class DebugManager {
	// public static readonly List<(MethodInfo, DebugMemberAttribute)> methods = Assembly
	//                                                                          .GetExecutingAssembly()
	//                                                                          .GetTypes()
	//                                                                          .SelectMany(v => v.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
	//                                                                          .Select(v => (v, v.GetCustomAttribute<DebugMemberAttribute>()))
	//                                                                          .Where(v => v.Item2 != null)
	//                                                                          .ToList()!;
	//
	//
	
	public static readonly List<DebugCategory> categories = new();
	private static readonly HashSet<Assembly> _processedAssemblies = new();

	static DebugManager() {
		ProcessAssembly(Assembly.GetExecutingAssembly());
	}

	public static void ProcessAssembly(Assembly asm) {
		if (!_processedAssemblies.Add(asm)) return;
		Type[] types = asm.GetTypes();

		foreach (Type type in types) 
			ProcessType(type);
	}

	private static void ProcessType(Type t) {
		foreach ((MethodInfo method, DebugMemberAttribute? attrib) in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Select(v => (v, v.GetCustomAttribute<DebugMemberAttribute>())).Where(v => v.Item2 != null)!) {
			AddMethod(attrib!.category, method, attrib);
		}
		
		foreach ((FieldInfo field, DebugMemberAttribute? attrib) in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Select(v => (v, v.GetCustomAttribute<DebugMemberAttribute>())).Where(v => v.Item2 != null)!) {
			AddField(attrib!.category, field, attrib);
		}
		
		foreach ((FieldInfo field, DebugMemberFieldAttribute? attrib) in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Select(v => (v, v.GetCustomAttribute<DebugMemberFieldAttribute>())).Where(v => v.Item2 != null)!) {
			AddEditableField(attrib!.category, field, attrib);
		}
	}

	public static void AddMethod(string name, MethodInfo m, DebugMemberAttribute attrib) {
		DebugCategory category = GetCategory(name);
		
		category.methods.Add((m, attrib));
	}
	
	public static void AddField(string name, FieldInfo f, DebugMemberAttribute attrib) {
		DebugCategory category = GetCategory(name);
		
		category.fields.Add((f, attrib));
	}
	
	public static void AddEditableField(string name, FieldInfo f, DebugMemberFieldAttribute attrib) {
		DebugCategory category = GetCategory(name);
		category.editableFields.Add((f, attrib));
	}

	public static DebugCategory GetCategory(string name) {
		string[] parts = name.Split('.');

		DebugCategory category = GetTopLevelCategory(parts[0]);
		for (int i = 1; i < parts.Length; i++)
			category = category.GetChild(parts[i]);

		return category;
	}

	private static DebugCategory GetTopLevelCategory(string name) {
		int c = categories.Count;
		for (int i = 0; i < c; i++) {
			if (categories[i].categoryName != name) continue;
			return categories[i];
		}
		
		categories.Add(new(name));
		return categories[^1];
	}
}