using YamlDotNet.Serialization;

namespace Quartz.objects.ecs.prefabs; 

public class PrefabSerializationData {
	public string name = "";
	[YamlMember()]
	public EntitySerializationData data;
}

public class EntitySerializationData {
	public EntitySerializationData[] childs = Array.Empty<EntitySerializationData>();
}
/*
test_prefab:
	position:
		x: 5
		y: -88.4
	rotation:
		z: 180
	matrix: new
	childs:
		
 */