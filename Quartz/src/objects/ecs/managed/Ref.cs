namespace Quartz.objects.ecs.managed; 

public readonly struct Ref<T> : IDisposable {
	public readonly int id;
	public bool isValid => id != -1 && EcsManagedData<T>.Contains(this);
	
	public T? v {
		get => isValid ? EcsManagedData<T>.items.storage[id].v : default;
		//set => EcsManagedData<T>.Set(this, value!);
	}

	public Ref(int id) => this.id = id;
	public Ref(T v) => id = EcsManagedData<T>.Add(v).id;

	public void Dispose() => EcsManagedData<T>.Remove(this);

	public static implicit operator T(Ref<T> v) => v.v!;
	public static implicit operator Ref<T>(T v) => new(v);

	public override string ToString() => v?.ToString() ?? "null";
}