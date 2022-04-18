using Quartz.Ecs.collections;

namespace Quartz.Ecs.ecs.components.data; 

public class NormalComponentArray : ComponentArrayBase {
	public readonly EcsList[] data;

	public NormalComponentArray(ComponentTypeArray types) : base(types) {
		int c = types.componentCount;
		data = new EcsList[c];
		for (int i = 0; i < c; i++)
			data[i] = EcsListTypes.CreateList(types[i].data.type);
	}
}