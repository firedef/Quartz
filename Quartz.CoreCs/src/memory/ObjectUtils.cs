using Quartz.CoreCs.other;

namespace Quartz.CoreCs.memory; 

public abstract class ObjectUtils {
	private static readonly Dictionary<Type, ObjectUtils> _instances = new();

	protected abstract int itemSizeof { get; }
	protected abstract unsafe void CreateAndWrite(void* dest);

	private static ObjectUtils GetInstance(Type t) {
		if (_instances.TryGetValue(t, out ObjectUtils? obj)) return obj;
		obj = GenericTypes.Create<ObjectUtils>(typeof(ObjectUtils<>), t);
		_instances.Add(t, obj);
		return obj;
	}
	
	public static int GetObjectSize(Type t) => GetInstance(t).itemSizeof;
	public static unsafe void CreateObject(Type t, void* dest) => GetInstance(t).CreateAndWrite(dest);
}

public class ObjectUtils<T> : ObjectUtils where T : unmanaged {
	protected override unsafe int itemSizeof => sizeof(T);
	protected override unsafe void CreateAndWrite(void* dest) => *(T*)dest = new();
} 