using System.Reflection;
using System.Runtime.InteropServices;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;
using YamlDotNet.RepresentationModel;

namespace Quartz.Ecs.ecs.prefabs.load; 

public class PrefabLoader {
	private readonly Stack<Node> _nodes;
	private readonly List<DeserializedEntityData> _entities = new();
	private DeserializedEntityData? _currentEntity = null;
	private DeserializedComponentData? _currentComponent = null;

	private PrefabLoader(Stack<Node> nodes) {
		_nodes = nodes;
	}

	private void ParseEntities(string yml) {
		YamlStream yaml = new();
		using StringReader sr = new(yml);
		yaml.Load(sr);

		YamlMappingNode map = (YamlMappingNode)yaml.Documents[0].RootNode;
		PushNodes(map, 0);
		ParseEntities();
	}

	private void PushNodes(YamlMappingNode map, int depth) {
		foreach ((YamlNode k, YamlNode v) in map.Children) _nodes.Push(new(k, v, depth));
	}

	private void ParseEntities(int startDepth = 0) {
		while (_nodes.Count > 0) if (ParseNode(startDepth)) break;
		
		if (_currentComponent != null) _currentEntity!.components.Add(_currentComponent);
		if (_currentEntity != null) _entities.Add(_currentEntity);
	}

	private bool ParseNode(int startDepth) {
		Node node = _nodes.Peek();

		switch (node.val.NodeType) {
			case YamlNodeType.Mapping:
				if (ParseNodeMap(node, startDepth)) return true;
				break;
			case YamlNodeType.Scalar:  
				if (ParseNodeScalar(node, startDepth)) return true;
				break;
			case YamlNodeType.Alias:    break;
			case YamlNodeType.Sequence: break;
			default:                    throw new ArgumentOutOfRangeException();
		}
		return false;
	}

	private bool ParseNodeMap(Node node, int startDepth) {
		YamlMappingNode map = (YamlMappingNode)node.val;

		// entities
		if (node.depth == startDepth) {
			_nodes.Pop();
			if (_currentEntity != null) {
				if (_currentComponent != null) {
					_currentEntity.components.Add(_currentComponent);
					_currentComponent = null;
				}
				_entities.Add(_currentEntity);
			}
			_currentEntity = new() {name = node.key.ToString()};
			PushNodes(map, node.depth + 1);
			return false;
		}
		
		// components / childs
		if (node.depth == startDepth + 1) {
			_nodes.Pop();
			if (node.key.ToString() == "childs") {
				PushNodes(map, node.depth + 1);
				
				PrefabLoader loader = new(_nodes);
				loader.ParseEntities(node.depth + 1);
				_currentEntity!.childs = loader._entities;
				return false;
			}
			if (_currentComponent != null) _currentEntity!.components.Add(_currentComponent);
			_currentComponent = new(node.key.ToString());
			PushNodes(map, node.depth + 1);
			return false;
		}

		return true;
	}
	
	private bool ParseNodeScalar(Node node, int startDepth) {
		// empty entity
		if (node.depth == startDepth) {
			_nodes.Pop();
			if (_currentEntity != null) {
				if (_currentComponent != null) {
					_currentEntity.components.Add(_currentComponent);
					_currentComponent = null;
				}
				_entities.Add(_currentEntity);
			}
			_currentEntity = new() {name = node.key.ToString()};
			return false;
		}
		
		// empty components
		if (node.depth == startDepth + 1) {
			_nodes.Pop();
			if (_currentComponent != null) _currentEntity!.components.Add(_currentComponent);
			_currentComponent = new(node.key.ToString());
			return false;
		}
		
		if (node.depth != startDepth + 2) return true;
		
		// component values
		_nodes.Pop();
		_currentComponent!.fields.Add(node.key.ToString(), node.val.ToString());
		return false;
	}

	public static void Load(string yml) {
		PrefabLoader loader = new(new());
		loader.ParseEntities(yml);

		foreach (DeserializedEntityData entityData in loader._entities) {
			CreateEntity(entityData);
		}
		
		// foreach (DeserializedEntityData entity in loader._entities) {
		// 	Console.WriteLine(entity.name);
		// 	foreach (DeserializedComponentData componentData in entity.components) {
		// 		Console.WriteLine($"  {componentData.name}");
		// 		foreach (KeyValuePair<string,string> field in componentData.fields) {
		// 			Console.WriteLine($"    {field.Key}:{field.Value}");
		// 		}
		// 	}
		// }
	}

	private static unsafe EntityId CreateEntity(DeserializedEntityData data) {
		List<(DeserializedComponentData, Type)> components = new();
		foreach (DeserializedComponentData componentData in data.components) {
			ComponentData? d = FindComponent(componentData.name);
			if (d == null) continue;
			components.Add((componentData, d.type));
		}
		
		EntityId entity = World.prefabs.AddEntity(components.Select(v => v.Item2).ToArray());

		foreach ((DeserializedComponentData d, Type t) in components) {
			entity.Set(t.Get(), d.fields);
		}
		
		// foreach ((DeserializedComponentData d, Type t) in components) {
		// 	object comp = Activator.CreateInstance(t)!;
		// 	SetFields(comp, t, d.fields);
		// 	void* ptr = World.prefabs.Comp(entity, t);
		// 	Marshal.StructureToPtr(comp, (IntPtr) ptr, false);
		// }

		entity.name = data.name;
		
		return entity;
	}

	private static ComponentData? FindComponent(string name) {
		ComponentData? comp = ComponentDataCache.TryFindComponent(name);
		if (comp == null) Console.WriteLine($"component '{name}' not found");
		return comp;
	}

	private static void SetFields(object obj, Type t, Dictionary<string, string> fields) {
		foreach ((string key, string val) in fields) {
			FieldInfo? field = t.GetField(key);
			if (field == null) {
				Console.WriteLine($"can't find '{key}' in {t.Name}");
				continue;
			}
			field.SetValue(obj, Convert.ChangeType(val, field.FieldType));
		}
	}
}