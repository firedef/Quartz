using YamlDotNet.RepresentationModel;

namespace Quartz.Ecs.ecs.prefabs.load; 

public class Node {
	public readonly YamlNode key;
	public readonly YamlNode val;
	public readonly int depth;
	public Node(YamlNode key, YamlNode val, int depth) {
		this.key = key;
		this.val = val;
		this.depth = depth;
	}
}