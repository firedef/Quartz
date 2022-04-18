namespace Quartz.CoreCs.other; 

public static class GenericTypes {
	private static readonly Dictionary<(Type basic, Type genericArg), Type> _types = new();

	private static Type MakeGeneric(Type basic, Type genericArg) {
		Type type = basic.MakeGenericType(genericArg);
		_types.Add((basic, genericArg), type);
		return type;
	}

	public static Type GetType(Type basic, Type t) => _types.TryGetValue((basic, t), out Type? v) ? v : MakeGeneric(basic, t);

	public static object Create(Type basic, Type genericArg) => Activator.CreateInstance(GetType(basic, genericArg))!;
	public static TBase Create<TBase>(Type basic, Type genericArg) => (TBase) Activator.CreateInstance(GetType(basic, genericArg))!;
}