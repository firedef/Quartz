using System.Data;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs.prefabs; 

public class Prefab {
	private static readonly Dictionary<string, Prefab> _prefabs = new();
	private static World _world => World.prefabs;

	private EntityId _entityId;
	private string _prefabName;

	public string prefabName {
		get => _prefabName;
		set {
			_prefabName = value;
			World.SetEntityName(_entityId, value);
		}
	}
	private Action<Prefab, EntityId> _onSpawn = (_,_) => { };

	public Prefab(EntityId entityId, string name) {
		_entityId = entityId;
		prefabName = name;
	}
	
	public EntityId Spawn() => Spawn(World.general);
	public void Spawn(int count) => Spawn(World.general, count);
	
	public EntityId Spawn(World target) {
		EntityId clone = _entityId.Clone(target);
		_onSpawn(this, clone);
		return clone;
	}
	
	public void Spawn(World target, int count) {
		_entityId.Clone(target, count, e => _onSpawn(this, e));
	}

	public Prefab Set<T>(T v) where T : unmanaged, IComponent {
		_entityId.Set(v);
		return this;
	}
	
	public Prefab Set<T>() where T : unmanaged, IComponent {
		_entityId.Set<T>(new());
		return this;
	}

	public Prefab Set<T>(Func<EntityId, T> setFunc) where T : unmanaged, IComponent {
		_entityId.TryAdd<T>();
		_onSpawn += (_,e) => e.Set(setFunc(e));
		return this;
	}
	
	public Prefab Set<T>(Func<T> setFunc) where T : unmanaged, IComponent {
		_entityId.TryAdd<T>();
		_onSpawn += (_,e) => e.Set(setFunc());
		return this;
	}
	
	public Prefab OnSpawn(Action<EntityId> a) {
		_onSpawn += (_,e) => a(e);
		return this;
	}
	
	public Prefab OnSpawn(Action<Prefab, EntityId> a) {
		_onSpawn += a;
		return this;
	}

	//TODO: hierarchy support
	public Prefab AddChild(Action<EntityId> a) {
		//a(_entityId.AddChild());
		return this;
	}

	public Prefab ClonePrefab(string newName) {
		Prefab newPrefab = New(newName, _entityId.Clone());
		newPrefab._onSpawn = _onSpawn;
		return newPrefab;
	}
	
	public Prefab ClonePrefab() {
		string newName = prefabName + "_c";
		int attempt = 0;
		while (Exists(newName + attempt)) attempt++;
		newName = newName + attempt;

		return ClonePrefab(newName);
	}

	public void DestroyPrefab() {
		_entityId.Destroy();
		_prefabs.Remove(prefabName);
		_entityId = EntityId.@null;
	}

	public static bool Exists(string name) => _prefabs.ContainsKey(name);

	public static Prefab New(string name) {
		if (_prefabs.ContainsKey(name)) throw new DuplicateNameException($"prefab {name} already present");
		
		Prefab prefab = new(_world.AddEntity(), name);
		_prefabs.Add(name, prefab);
		return prefab;
	}

	private static Prefab New(string name, EntityId entity) {
		if (_prefabs.ContainsKey(name)) throw new DuplicateNameException($"prefab {name} already present");
		
		Prefab prefab = new(entity, name);
		_prefabs.Add(name, prefab);
		return prefab;
	}

	public static Prefab New() => New($"prefab_{Guid.NewGuid()}");
	
	public static Prefab FromName(string name) => _prefabs[name];

	public override string ToString() => $"{_entityId}:'{prefabName}'";
}