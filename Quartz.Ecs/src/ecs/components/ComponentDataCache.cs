using System.Reflection;
using Quartz.Ecs.ecs.attributes;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.components; 

public static class ComponentDataCache {
	private static readonly Dictionary<Type, ComponentData> _cachedComponentsDictionary = new();
	private static readonly List<ComponentData> _cachedComponents = new();

	private static readonly Dictionary<Type, ComponentTypeArray> _requiredNormalComponents = new();
	private static readonly Dictionary<Type, ComponentTypeArray> _requiredSharedComponents = new();

	public static ComponentData Get(this ComponentType ident) => _cachedComponents[ident];
	public static ComponentType Get(this Type t) => _cachedComponentsDictionary.TryGetValue(t, out ComponentData? v) ? v.identifier : Cache(t);

	private static ComponentType AddData(ComponentData data) {
		_cachedComponentsDictionary.Add(data.type, data);
		_cachedComponents.Add(data);
		data.identifier = _cachedComponents.Count - 1;
		return data.identifier;
	}

	private static ComponentType Cache(Type t) {
		bool isShared = t.IsAssignableTo(typeof(ISharedComponent));
		ComponentData data = new(t, isShared ? ComponentKind.shared : ComponentKind.normal, ComponentType.@null);
		AddData(data);
		
		RequireAttribute? requireAttribute = t.GetCustomAttribute<RequireAttribute>();
		data.requiredTypes = requireAttribute?.values.Select(Get).ToArray() ?? Array.Empty<ComponentType>();

		ComponentAttribute? componentAttribute = t.GetCustomAttribute<ComponentAttribute>();
		if (componentAttribute != null) {
			data.name = componentAttribute.name;
		}

		return data.identifier;
	}

	public static ComponentTypeArray GetNormalComponents(this Type[] t) {
		int c = t.Length;
		ComponentTypeArray[] arr = new ComponentTypeArray[c];
		for (int i = 0; i < c; i++) arr[i] = GetRequiredNormalComponents(t[i]);
		return ComponentTypeArray.Merge(arr);
	}
	
	public static ComponentTypeArray GetSharedComponents(this Type[] t) {
		int c = t.Length;
		ComponentTypeArray[] arr = new ComponentTypeArray[c];
		for (int i = 0; i < c; i++) arr[i] = GetRequiredSharedComponents(t[i]);
		return ComponentTypeArray.Merge(arr);
	}

	public static ComponentTypeArray GetRequiredNormalComponents(this Type t) {
		if (_requiredNormalComponents.TryGetValue(t, out ComponentTypeArray? arr)) return arr;
		CacheRequiredComponents(t);
		return _requiredNormalComponents[t];
	}
	
	public static ComponentTypeArray GetRequiredSharedComponents(this Type t) {
		if (_requiredSharedComponents.TryGetValue(t, out ComponentTypeArray? arr)) return arr;
		CacheRequiredComponents(t);
		return _requiredSharedComponents[t];
	}

	private static void CacheRequiredComponents(Type t) {
		ComponentData cur = Get(t).data;
		HashSet<ComponentType> normal = new();
		HashSet<ComponentType> shared = new();
		
		CacheRequiredComponents(cur, normal, shared);
		
		_requiredNormalComponents.Add(t, new(normal.ToArray()));
		_requiredSharedComponents.Add(t, new(shared.ToArray()));
	}

	private static void CacheAllComponents(Assembly asm) {
		Type[] types = asm.GetTypes();
		foreach (Type type in types) {
			if (!type.IsAssignableTo(typeof(IEcsData))) continue;
			type.Get();
		}
	}

	private static void CacheAllComponents() {
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly asm in assemblies) CacheAllComponents(asm);
	}

	private static void CacheRequiredComponents(ComponentData cur, HashSet<ComponentType> normal, HashSet<ComponentType> shared) {
		if (cur.kind == ComponentKind.normal && !normal.Add(cur.identifier)) return;
		if (cur.kind == ComponentKind.shared && !shared.Add(cur.identifier)) return;

		foreach (ComponentType t in cur.requiredTypes) CacheRequiredComponents(t.data, normal, shared);
	}

	public static ComponentData? TryFindComponent(string name) {
		foreach (ComponentData comp in _cachedComponents) {
			if (comp.name == name || comp.type.Name == name || comp.type.FullName == name) return comp;
		}

		CacheAllComponents();
		foreach (ComponentData comp in _cachedComponents) {
			if (comp.name == name || comp.type.Name == name || comp.type.FullName == name) return comp;
		}

		return null;
	}
}