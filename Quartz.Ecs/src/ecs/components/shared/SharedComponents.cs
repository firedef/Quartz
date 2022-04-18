using Quartz.CoreCs.collections;
using Quartz.CoreCs.other;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.components.shared;

public class SharedComponents {
	private readonly List<INativeListPool> _components = new();
	private readonly DualIntMap _typeIndexMap = new();
	private readonly object _lock = new();

	public int GetTypeIndex(ComponentType t) {
		lock (_lock) {
			int i = (int)_typeIndexMap[t];
			if (i != -1) return i;
			_typeIndexMap.Set(t, (uint)_components.Count);
			_components.Add(GenericTypes.Create<INativeListPool>(typeof(NativeListPool<>), t.data.type));
			return _components.Count - 1;
		}
	}
	
	public INativeListPool GetList(int index) { lock (_lock) return _components[index]; }

	public INativeListPool GetList(ComponentType t) {
		lock (_lock) {
			int i = GetTypeIndex(t);
			return _components[i];
		}
	}
	
	public NativeListPool<T> GetList<T>() where T : unmanaged, ISharedComponent => (NativeListPool<T>) GetList(typeof(T).Get());

	public unsafe T* Get<T>(ushort index) where T : unmanaged, ISharedComponent => GetList<T>().ptr + index - 1;
	public void Remove<T>(ushort index) where T : unmanaged, ISharedComponent => GetList<T>().RemoveAt(index - 1);
	public void Clear<T>() where T : unmanaged, ISharedComponent => GetList<T>().Clear();
	
	public ushort Add<T>(T v) where T : unmanaged, ISharedComponent { lock (_lock) return (ushort)(GetList<T>().Add(v) + 1); }
}