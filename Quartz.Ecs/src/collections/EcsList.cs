using MathStuff;
using Quartz.CoreCs.collections;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.collections; 

public static class EcsListTypes {
	private static readonly Dictionary<Type, Type> _types = new();

	private static Type MakeGeneric(Type t) {
		Type type = typeof(EcsList<>).MakeGenericType(t);
		_types.Add(t, type);
		return type;
	}

	public static Type GetEcsListType(Type t) => _types.TryGetValue(t, out Type? v) ? v : MakeGeneric(t);

	public static EcsList CreateList(Type t) => (EcsList)Activator.CreateInstance(GetEcsListType(t))!;
}

public abstract class EcsList {
	public abstract int count { get; }
	public abstract int capacity { get; }
	public abstract int sizeofElement { get; }
	public abstract unsafe void* rawData { get; }
	public abstract unsafe void* rawDataEnd { get; }

	public abstract unsafe void* GetElementPtr(int index);

	public abstract void Push();
	public abstract void PushMultiple(int c);
	public abstract unsafe void PushFrom(void* element);
	
	public abstract void Pop(bool dispose);
	public abstract void RemoveByReplaceLast(int index, bool dispose);
	
	public abstract void Swap(int a, int b);

	public abstract void Trim();
	public abstract void Clear();

	public abstract unsafe void CopyElementFrom(int index, void* src);
	public abstract unsafe void Initialize(int index);
	
	public abstract unsafe void DisposeAt(int index);

	public abstract unsafe void Parse(int index, Dictionary<string, string> fields);
}

public class EcsList<T> : EcsList where T : unmanaged {
	private readonly NativeList<T> _list = new(32);
	private readonly bool _isIDisposable = typeof(T).IsAssignableTo(typeof(IDisposable));
	private readonly bool _isICloneableComponent = typeof(T).IsAssignableTo(typeof(ICloneableComponent));

	public override int count => _list.Count;
	public override int capacity => _list.capacity;
	public override unsafe int sizeofElement => sizeof(T);
	
	public unsafe T* data => _list.ptr;
	public override unsafe void* rawData => _list.ptr;
	public override unsafe void* rawDataEnd => data + count;

	public void Push(T v) => _list.Add(v);
	public override void Push() => Push(new());
	
	public override void PushMultiple(int c) {
		_list.EnsureFreeSpace(c);
		for (int i = count; i < c + count; i++) _list[i] = new();
		_list.count += c;
	}
	
	public override unsafe void Initialize(int index) => _list.ptr[index] = new();

	public override unsafe void PushFrom(void* element) => Push(*(T*)element);

	public override unsafe void Pop(bool dispose) {
		_list.count--;
		if (dispose && _isIDisposable) ((IDisposable)_list.ptr[_list.count]).Dispose();
	}
	
	public override unsafe void RemoveByReplaceLast(int index, bool dispose) {
		if (dispose && _isIDisposable) ((IDisposable)_list.ptr[index]).Dispose();
		data[index] = data[count - 1];
		_list.count--;
	}

	public override unsafe void DisposeAt(int index) {
		if (_isIDisposable) ((IDisposable)_list.ptr[index]).Dispose();
	}
	public override unsafe void Swap(int a, int b) => (data[a], data[b]) = (data[b], data[a]);

	public override void Trim() {
		int c = (int)math.min(count * 1.2f + 8, _list.capacity);
		_list.Trim(c);
	}

	public override unsafe void Clear() {
		if (_isIDisposable) {
			int c = _list.count;
			for (int i = 0; i < c; i++) ((IDisposable)_list.ptr[i]).Dispose();
		}
		_list.count = 0;
	}

	public override unsafe void CopyElementFrom(int index, void* src) {
		DisposeAt(index);
		data[index] = *(T*)src;
		if (_isICloneableComponent) ((ICloneableComponent) data[index]).OnClone(src);
	}

	public override unsafe void* GetElementPtr(int index) => data + index;

	public override unsafe void Parse(int index, Dictionary<string, string> fields) {
		IEcsData v = (IEcsData) _list.ptr[index];
		v.Parse(fields);
		_list.ptr[index] = (T) v;
	}
} 