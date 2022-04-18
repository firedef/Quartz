using Quartz.CoreCs.collections;

namespace Quartz.Ecs.ecs.managed; 

public static class EcsManagedData<T> {
	public static readonly ManagedListPool<(T v, int refCount)> items = new();

	public static bool Contains(Ref<T> r) => r.id != 0 && items.Contains(r.id - 1);

	public static Ref<T> Add(T v) {
		for (int i = 0; i < items.storage.Count; i++) {
			if (items.emptyIndices.Contains(i) || !items.storage[i].v!.Equals(v)) continue;
			Ref(i + 1);
			return new(i + 1);
		}
		return new(items.Add((v, 1)) + 1);
	}

	public static void Remove(int i) {
		if (i > 0) items.RemoveAt(i - 1);
	}
	public static void Remove(Ref<T> r) { if (r.isValid) Unref(r.id); }

	public static void Ref(int i) {
		if (i == 0) return;
		(T v, int refCount) item = items.storage[i - 1];
		item.refCount++;
		items.storage[i - 1] = item;
	}
	
	public static void Unref(int i) {
		if (i == 0) return;
		(T v, int refCount) item = items.storage[i - 1];
		item.refCount--;
		if (item.refCount <= 0) items.RemoveAt(i - 1);
		else items.storage[i - 1] = item;
	}
}