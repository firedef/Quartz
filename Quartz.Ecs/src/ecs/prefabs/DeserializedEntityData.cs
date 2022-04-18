namespace Quartz.Ecs.ecs.prefabs; 

public class DeserializedEntityData {
	public string name = "?";
	public List<DeserializedComponentData> components = new();
	public List<DeserializedEntityData> childs = new();
}

public class DeserializedComponentData {
	public string name;
	public readonly Dictionary<string, string> fields = new();

	public DeserializedComponentData(string name) => this.name = name;
}