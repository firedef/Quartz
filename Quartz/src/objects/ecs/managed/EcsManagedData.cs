namespace Quartz.objects.ecs.managed; 

public static class EcsManagedData<T> {
	public static readonly ManagedListPool<T> items = new();

	public static bool Contains(Ref<T> r) => items.Contains(r.id);

	public static Ref<T> Add(T v) {
		return new(items.Add(v));
	}
	public static void Set(Ref<T> r, T v) {
		if (r.isValid) items.storage[r.id] = v;
	}
	
	public static void Remove(int i) => items.RemoveAt(i);
	public static void Remove(Ref<T> r) { if (r.isValid) items.RemoveAt(r.id); }
}