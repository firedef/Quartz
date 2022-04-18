using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.components; 

public class ComponentTypeArray {
	public static readonly ComponentTypeArray empty = new();
	private readonly ComponentType[] _array;
	
	public int componentCount => _array.Length;
	
	public ComponentTypeArray(params ComponentType[] array) => _array = array;

	public int IndexOf(ComponentType t) {
		int c = componentCount;
		for (int i = 0; i < c; i++)
			if (_array[i] == t)
				return i;
		return -1;
	}

	public bool Contains(ComponentType t) => IndexOf(t) != -1;

	public bool Contains(ComponentTypeArray other) {
		int c0 = componentCount;
		int c1 = other.componentCount;
		
		if (c0 < c1) return false;

		for (int i = 0; i < c1; i++) {
			for (int j = 0; j < c0; j++) 
				if (_array[j] == other._array[i]) goto FOUND;
			return false;
		FOUND: ;
		}
		return true;
	}
	
	public ComponentType this[int i] => _array[i];

	public static ComponentTypeArray Merge(params ComponentTypeArray[] arr) {
		if (arr.Length == 1) return arr[0];
		if (arr.Length == 2 && arr[1].componentCount == 0) return arr[0];
		if (arr.Length == 2 && arr[0].componentCount == 0) return arr[1];
		
		int arrCount = arr.Length;
		int totalCount = 0;
		for (int i = 0; i < arrCount; i++) totalCount += arr[i].componentCount;

		HashSet<ComponentType> components = new(totalCount);
		for (int i = 0; i < arrCount; i++) {
			int c = arr[i].componentCount;
			for (int j = 0; j < c; j++) components.Add(arr[i]._array[j]);
		}

		return new(components.ToArray());
	}
	
	public static ComponentTypeArray Merge(ComponentTypeArray a, ComponentTypeArray b) {
		if (b.componentCount == 0) return a;
		if (a.componentCount == 0) return b;
		
		HashSet<ComponentType> components = new(a.componentCount + b.componentCount);
		
		int c = a.componentCount;
		for (int j = 0; j < c; j++) components.Add(a._array[j]);
		c = b.componentCount;
		for (int j = 0; j < c; j++) components.Add(b._array[j]);
		
		return new(components.ToArray());
	}
	
	public static ComponentTypeArray Remove(ComponentTypeArray a, ComponentType t, ComponentKind currentKind) {
		return currentKind != t.data.kind ? a : new(a._array.Where(v => v != t).ToArray());
	}
}