using Quartz.objects.memory;

namespace Quartz.objects.ecs.archetypes;

public static class EcsListTypes {
	private static Dictionary<Type, Type> _types = new();

	private static Type MakeGeneric(Type t) {
		Type type = typeof(EcsList<>).MakeGenericType(t);
		_types.Add(t, type);
		return type;
	}

	public static Type GetEcsListType(Type t) => _types.TryGetValue(t, out Type? v) ? v : MakeGeneric(t);

	public static EcsList CreateList(Type t) => (EcsList)Activator.CreateInstance(GetEcsListType(t))!;
}

public abstract class EcsList {
	protected const int arrayInitialSize = 256;
	public abstract unsafe int count { get; set; }
	public abstract unsafe void* rawData { get; }
	public abstract void Add();
	public abstract unsafe void AddFrom(void* src);
	public abstract void ReplaceByLast(int index);
	public abstract int elementSize { get; }
	public abstract void PreAllocate(int elementCount);
	public abstract int freeSpace { get; }
}

public unsafe class EcsList<T> : EcsList where T : unmanaged {
	protected NativeList<T> collection = new(arrayInitialSize);

	public override int count { get => collection.count; set => collection.count = value; }
	public T* data => collection.ptr;
	public override void* rawData => data;
	public override int elementSize => sizeof(T);
	public override int freeSpace => collection.freeSpace;

	public override void Add() {
		collection.EnsureFreeSpace(1);
		count++;
	}

	public override void AddFrom(void* src) {
		collection.EnsureFreeSpace(1);
		data[count] = *(T*)src;
		count++;
	}

	public override void ReplaceByLast(int index) {
		collection.count--;
		if (index != collection.count)
			collection.ptr[index] = collection.ptr[collection.count];
	}

	public override void PreAllocate(int elementCount) => collection.EnsureFreeSpace(elementCount);
}