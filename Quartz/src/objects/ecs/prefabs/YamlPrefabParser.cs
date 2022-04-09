using YamlDotNet.RepresentationModel;

namespace Quartz.objects.ecs.prefabs;

//TODO: rewrite and remove ugly spaghetti code
public static class YamlPrefabParser {
	public static List<EntityData> ParsePrefabs(string yml) {
		YamlStream yaml = new();
		using StringReader sr = new(yml);
		yaml.Load(sr);

		YamlMappingNode map = (YamlMappingNode)yaml.Documents[0].RootNode;

		Stack<(YamlNode k, YamlNode v, int depth)> nodes = new();
		
		// prefabs
		foreach ((YamlNode k, YamlNode v) in map.Children) nodes.Push((k, v, 0));

		return ParseEntities(nodes);
	}

	public static void ParseAndAddPrefabs(string yml) {
		List<EntityData> entities = ParsePrefabs(yml);
		foreach (EntityData entity in entities) {
			Console.WriteLine(entity);
		}
	}
	
	private static List<EntityData> ParseEntities(Stack<(YamlNode k, YamlNode v, int depth)> nodes, int startDepth = 0) {
		List<EntityData> entities = new();
		EntityData? curPrefab = null;
		ComponentData? curComponent = null;
		
		while (nodes.Count > 0)
			if (ProcessYamlNode(nodes, startDepth, entities, ref curPrefab, ref curComponent)) 
				break;

		if (curComponent != null) curPrefab!.components.Add(curComponent);
		if (curPrefab != null) entities.Add(curPrefab);

		return entities;
	}
	private static bool ProcessYamlNode(Stack<(YamlNode k, YamlNode v, int depth)> nodes, int startDepth, List<EntityData> entities, ref EntityData? curPrefab, ref ComponentData? curComponent) {
		(YamlNode key, YamlNode val, int depth) = nodes.Peek();

		switch (val.NodeType) {
			case YamlNodeType.Mapping:
				if (ProcessYamlNodeMap(nodes, startDepth, entities, ref curPrefab, ref curComponent, val, depth, key)) return true;
				break;
			case YamlNodeType.Scalar:
				if (ProcessYamlNodeScalar(nodes, startDepth, ref curPrefab, ref curComponent, entities, depth, key, val)) return true;
				break;
			case YamlNodeType.Alias:
			case YamlNodeType.Sequence:
			default: throw new ArgumentOutOfRangeException();
		}
		return false;
	}
	private static bool ProcessYamlNodeScalar(Stack<(YamlNode k, YamlNode v, int depth)> nodes, int startDepth, ref EntityData? curPrefab, ref ComponentData? curComponent, List<EntityData> entities, int depth, YamlNode key, YamlNode val) {
		if (depth == startDepth) { // empty entity
			nodes.Pop();
			if (curPrefab != null) {
				if (curComponent != null) {
					curPrefab.components.Add(curComponent);
					curComponent = null;
				}
				entities.Add(curPrefab);
			}
			curPrefab = new() {name = key.ToString()};
			return false;
		}
		if (depth == startDepth + 1) { // empty components
			nodes.Pop();
			if (curComponent != null) curPrefab!.components.Add(curComponent);
			curComponent = new() {name = key.ToString()};
			return false;
		}
		if (depth != startDepth + 2) return true; // component values
		nodes.Pop();
		curComponent!.fields.Add(key.ToString(), val.ToString());
		return false;
	}
	private static bool ProcessYamlNodeMap(Stack<(YamlNode k, YamlNode v, int depth)> nodes, int startDepth, List<EntityData> entities, ref EntityData? curPrefab, ref ComponentData? curComponent, YamlNode v, int depth, YamlNode k) {
		YamlMappingNode map = (YamlMappingNode)v;

		// prefabs
		if (depth == startDepth) {
			nodes.Pop();
			if (curPrefab != null) {
				if (curComponent != null) {
					curPrefab.components.Add(curComponent);
					curComponent = null;
				}
				entities.Add(curPrefab);
			}
			curPrefab = new() {name = k.ToString()};
		}

		// components
		else if (depth == startDepth + 1) {
			if (k.ToString() == "childs") {
				nodes.Pop();
				foreach ((YamlNode key, YamlNode value) in map.Children) nodes.Push((key, value, depth + 1));
				curPrefab!.childs = ParseEntities(nodes, startDepth + 2);
				return false;
			}
			nodes.Pop();
			if (curComponent != null) curPrefab!.components.Add(curComponent);
			curComponent = new() {name = k.ToString()};
		}
		else if (depth < startDepth) return true;

		foreach ((YamlNode key, YamlNode value) in map.Children) nodes.Push((key, value, depth + 1));
		return false;
	}

	// private static List<EntityData> ParseEntities(Stack<(YamlNode k, YamlNode v, int depth)> nodes, int startDepth = 0) {
	// 	List<EntityData> entities = new();
	// 	EntityData? curPrefab = null;
	// 	ComponentData? curComponent = null;
	// 	
	// 	while (nodes.Count > 0) {
	// 		(YamlNode k, YamlNode v, int depth) = nodes.Peek();
	//
	// 		switch (v.NodeType) {
	// 			case YamlNodeType.Mapping:{
	// 				YamlMappingNode map = (YamlMappingNode)v;
	// 				
	// 				// prefabs
	// 				if (depth == startDepth) {
	// 					nodes.Pop();
	// 					if (curPrefab != null) {
	// 						if (curComponent != null) {
	// 							curPrefab.components.Add(curComponent);
	// 							curComponent = null;
	// 						}
	// 						entities.Add(curPrefab);
	// 					}
	// 					curPrefab = new();
	// 					curPrefab.name = k.ToString();
	// 				}
	// 				
	// 				// components
	// 				else if (depth == startDepth + 1) {
	// 					if (k.ToString() == "childs") {
	// 						nodes.Pop();
	// 						foreach ((YamlNode key, YamlNode value) in map.Children) nodes.Push((key, value, depth + 1));
	// 						curPrefab!.childs = ParseEntities(nodes, startDepth + 2);
	// 						break;
	// 					}
	// 					else {
	// 						nodes.Pop();
	// 						if (curComponent != null) {
	// 							curPrefab!.components.Add(curComponent);
	// 							curComponent = null;
	// 						}
	// 					
	// 						curComponent = new();
	// 						curComponent.name = k.ToString();
	// 					}
	// 				}
	// 				
	// 				// // childs
	// 				// else if (depth == startDepth + 2) {
	// 				// 	nodes.Pop();
	// 				// 	curPrefab!.childs = ParseEntities(nodes, startDepth + 2);
	// 				// }
	// 				else if (depth < startDepth) {
	// 					goto END;
	// 				}
	// 				else nodes.Pop();
	//
	// 				foreach ((YamlNode key, YamlNode value) in map.Children) nodes.Push((key, value, depth + 1));
	// 				break;
	// 			}
	// 			case YamlNodeType.Scalar:{
	// 				// empty components
	// 				if (depth == startDepth + 1) {
	// 					nodes.Pop();
	// 					if (curComponent != null) {
	// 						curPrefab!.components.Add(curComponent);
	// 						curComponent = null;
	// 					}
	// 					
	// 					curComponent = new();
	// 					curComponent.name = k.ToString();
	// 				}
	// 				
	// 				// component values
	// 				else if (depth == startDepth + 2) {
	// 					nodes.Pop();
	// 					curComponent!.fields.Add(k.ToString(), v.ToString());
	// 				}
	// 				else if (depth < startDepth) {
	// 					//nodes.Pop();
	// 					goto END;
	// 				}
	// 				else nodes.Pop();
	// 				break;
	// 			}
	// 			default: throw new ArgumentOutOfRangeException();
	// 		}
	// 	}
	//
	// END: ;
	// 	if (curComponent != null) curPrefab!.components.Add(curComponent);
	// 	if (curPrefab != null) entities.Add(curPrefab);
	//
	// 	return entities;
	// }

	public class EntityData {
		public string name = "?";
		public List<ComponentData> components = new();
		public List<EntityData> childs = new();

		public override string ToString() => $"entity {name}: {string.Join("", components.Select(v => $"\n  {v}"))}\nchilds: {string.Join("", childs.Select(v => $"\n  {v}"))}";
	}

	public record class ComponentData {
		public string name = "?";
		public Dictionary<string, string> fields = new();

		public override string ToString() => $"comp {name}: {string.Join("", fields.Select(v => $"\n    {v}"))}";
	}
}