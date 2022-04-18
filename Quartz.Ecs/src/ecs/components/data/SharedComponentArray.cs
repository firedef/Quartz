using Quartz.Ecs.collections;

namespace Quartz.Ecs.ecs.components.data; 

public class SharedComponentArray : ComponentArrayBase {
	public readonly EcsList<ushort>[] data;

	public SharedComponentArray(ComponentTypeArray types) : base(types) {
		int c = types.componentCount;
		data = new EcsList<ushort>[c];
		for (int i = 0; i < c; i++) data[i] = new();
	}
}