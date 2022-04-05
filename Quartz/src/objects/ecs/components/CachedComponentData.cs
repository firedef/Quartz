using System.Reflection;
using Quartz.objects.ecs.components.attributes;

namespace Quartz.objects.ecs.components; 

public class CachedComponentData {
	public Type type;
	public ComponentType typeId;
	public ComponentType[] required;
	public ComponentType remove;
}

public struct ComponentType {
	public static readonly ComponentType invalid = new(-1);
	public int id;

	public Type type => data.type;
	public CachedComponentData data => CachedComponents.Get(id);
	public bool isValid => id != -1;

	
	public ComponentType(int id) => this.id = id;

	public static implicit operator int(ComponentType t) => t.id;
	public static implicit operator ComponentType(int t) => new(t);

	public static bool operator ==(ComponentType a, ComponentType b) => a.id == b.id;
	public static bool operator !=(ComponentType a, ComponentType b) => a.id != b.id;
}

public static class CachedComponents {
	private static readonly Dictionary<Type, CachedComponentData> _cachedComponentDictionary = new();
	private static readonly List<CachedComponentData> _cachedComponentList = new();

	private static readonly Dictionary<Type, ComponentType[]> _cachedConvertedTypes = new();
	private static readonly Dictionary<Type[], ComponentType[]> _cachedConvertedTypesMultiple = new();

	private static void Add(CachedComponentData data) {
		_cachedComponentDictionary.Add(data.type, data);
		_cachedComponentList.Add(data);
	}

	public static bool IsCached(Type t) => _cachedComponentDictionary.ContainsKey(t);

	public static CachedComponentData Get(Type t) => _cachedComponentDictionary.TryGetValue(t, out CachedComponentData? v) ? v : Cache(t);
	public static CachedComponentData Get(int i) => _cachedComponentList[i];

	private static CachedComponentData Cache(Type t) {
		int typeId = _cachedComponentList.Count;
		CachedComponentData data = new();
		data.type = t;
		data.typeId = typeId;
		Add(data);
		
		RequireAttribute? requireAttribute = t.GetCustomAttribute<RequireAttribute>();
		data.required = requireAttribute?.types.Select(v => Get(v).typeId).ToArray() ?? Array.Empty<ComponentType>();

		return data;
	}

	private static void ConvertRequiredType(CachedComponentData current, HashSet<ComponentType> dest) {
		if (!dest.Add(current.typeId)) return;

		foreach (ComponentType i in current.required) {
			if (dest.Contains(i)) continue;
			if (_cachedConvertedTypes.TryGetValue(i.type, out ComponentType[]? v)) {
				foreach (ComponentType type in v) dest.Add(type);
				continue;
			}
			ConvertRequiredType(Get(i), dest);
		}
	}

	public static ComponentType[] ConvertRequiredType(Type t) {
		if (_cachedConvertedTypes.TryGetValue(t, out ComponentType[]? v)) return v;
		
		HashSet<ComponentType> converted = new(1);
		ConvertRequiredType(Get(t), converted);
		
		ComponentType[] convertedArr = converted.ToArray();
		_cachedConvertedTypes.Add(t, convertedArr);
		return convertedArr;
	}

	public static ComponentType[] ConvertRequiredTypes(Type[] types) {
		if (_cachedConvertedTypesMultiple.TryGetValue(types, out ComponentType[]? v)) return v;

		int oldTypeCount = types.Length;
		HashSet<ComponentType> converted = new(oldTypeCount);

		for (int i = 0; i < oldTypeCount; i++) ConvertRequiredType(Get(types[i]), converted);

		ComponentType[] convertedArr = converted.ToArray();
		_cachedConvertedTypesMultiple.Add(types, convertedArr);
		return convertedArr;
	}

	public static ComponentType[] ToEcsRequiredComponents(this Type[] types) => ConvertRequiredTypes(types);
	public static ComponentType[] ToEcsRequiredComponents(this Type type) => ConvertRequiredType(type);
	public static ComponentType ToEcsComponent(this Type type) => Get(type).typeId;

	public static ComponentType[] AddRequiredType(this IEnumerable<ComponentType> arr, Type other) {
		HashSet<ComponentType> hashset = arr.ToHashSet();
		ComponentType[] otherArr = other.ToEcsRequiredComponents();

		foreach (ComponentType type in otherArr) hashset.Add(type);

		return hashset.ToArray();
	}
	
	public static ComponentType[] RemoveRequiredType(this IEnumerable<ComponentType> arr, Type other) {
		HashSet<ComponentType> hashset = arr.ToHashSet();
		hashset.Remove(other.ToEcsComponent());
		return hashset.ToArray();
	}
}