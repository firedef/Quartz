namespace Quartz.Ecs.ecs.managed; 

public struct Ref<T> : IDisposable {
	public int id;
	public bool isValid => id != 0 && EcsManagedData<T>.Contains(this);
	
	public T? v {
		get => isValid ? EcsManagedData<T>.items.storage[id-1].v : default;
		set {
			if (v?.Equals(value) ?? value == null) return;
			Dispose();
			id = value == null ? 0 : EcsManagedData<T>.Add(value).id;
		}
	}

	public Ref(int id) => this.id = id;
	public Ref(T v) => id = EcsManagedData<T>.Add(v).id;

	public void Dispose() => EcsManagedData<T>.Remove(this);

	public static implicit operator T(Ref<T> v) => v.v!;
	public static implicit operator Ref<T>(T v) => new(v);

	public override string ToString() => v?.ToString() ?? "null";

	public Ref<T> Copy() {
		if (isValid) EcsManagedData<T>.Ref(id);
		return this;
	}
}