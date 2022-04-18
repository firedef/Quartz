using Quartz.CoreCs.collections;

namespace Quartz.objects.ecs.managed; 

public static class EcsManagedData<T> {
	public static readonly ManagedListPool<(T v, int refCount)> items = new();

	public static bool Contains(Ref<T> r) => items.Contains(r.id);

	public static Ref<T> Add(T v) {
		for (int i = 0; i < items.storage.Count; i++) {
			if (items.emptyIndices.Contains(i) || !items.storage[i].v!.Equals(v)) continue;
			Ref(i);
			return new(i);
		}
		return new(items.Add((v, 1)));
	}
	// public static void Set(Ref<T> r, T v) {
	// 	if (r.isValid) items.storage[r.id] = v;
	// }
	
	public static void Remove(int i) => items.RemoveAt(i);
	public static void Remove(Ref<T> r) { if (r.isValid) Unref(r.id); }

	public static void Ref(int i) {
		(T v, int refCount) item = items.storage[i];
		item.refCount++;
		items.storage[i] = item;
	}
	
	public static void Unref(int i) {
		(T v, int refCount) item = items.storage[i];
		item.refCount--;
		if (item.refCount <= 0) items.RemoveAt(i);
		else items.storage[i] = item;
	}
}