using System.Data;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.world;
using Quartz.objects.hierarchy.ecs;

namespace Quartz.objects.ecs.prefabs;

//TODO: load from YAML
//TODO: serialize to YAML
public class Prefab {
	public static readonly Dictionary<string, Prefab> prefabs = new();
	private static World _world => World.prefabs;
	
	private EntityId _entityId;
	private Action<Prefab, EntityId> onSpawn = (_,_) => { };
	public string name { get; private set; }

	public Prefab(EntityId entityId, string name) {
		_entityId = entityId;
		this.name = name;
	}

	public EntityId Spawn() => Spawn(World.general);
	public void Spawn(int count) => Spawn(World.general, count);
	
	public EntityId Spawn(World target) {
		EntityId clone = _entityId.Clone(target);
		onSpawn(this, clone);
		return clone;
	}
	
	public void Spawn(World target, int count) {
		for (int i = 0; i < count; i++) onSpawn(this, _entityId.Clone(target));
	}

	public Prefab Set<T>(T v) where T : unmanaged, IComponent {
		_entityId.Set<T>(v);
		return this;
	}
	
	public Prefab Set<T>() where T : unmanaged, IComponent {
		_entityId.Set<T>(new());
		return this;
	}

	public Prefab Set<T>(Func<EntityId, T> setFunc) where T : unmanaged, IComponent {
		onSpawn += (_,e) => e.Set(setFunc(e));
		return this;
	}
	
	public Prefab Set<T>(Func<T> setFunc) where T : unmanaged, IComponent {
		onSpawn += (_,e) => e.Set(setFunc());
		return this;
	}
	
	public Prefab OnSpawn(Action<EntityId> a) {
		onSpawn += (_,e) => a(e);
		return this;
	}
	
	public Prefab OnSpawn(Action<Prefab, EntityId> a) {
		onSpawn += a;
		return this;
	}

	public Prefab AddChild(Action<EntityId> a) {
		a(_entityId.AddChild());
		return this;
	}

	public Prefab ClonePrefab(string newName) {
		Prefab newPrefab = New(newName, _entityId.Clone());
		newPrefab.onSpawn = onSpawn;
		return newPrefab;
	}
	
	public Prefab ClonePrefab() {
		string newName = name + "_c";
		int attempt = 0;
		while (Exists(newName + attempt)) attempt++;
		newName = newName + attempt;

		return ClonePrefab(newName);
	}

	public void DestroyPrefab() {
		_entityId.Destroy();
		prefabs.Remove(name);
		_entityId = EntityId.@null;
	}

	public static bool Exists(string name) => prefabs.ContainsKey(name);

	public static Prefab New(string name) {
		if (prefabs.ContainsKey(name)) throw new DuplicateNameException($"prefab {name} already present");
		
		Prefab prefab = new(_world.CreateEntity(), name);
		prefabs.Add(name, prefab);
		return prefab;
	}

	private static Prefab New(string name, EntityId entity) {
		if (prefabs.ContainsKey(name)) throw new DuplicateNameException($"prefab {name} already present");
		
		Prefab prefab = new(entity, name);
		prefabs.Add(name, prefab);
		return prefab;
	}

	public static Prefab New() => New($"prefab_{Guid.NewGuid()}");
	
	public static Prefab FromName(string name) => prefabs[name];

	public override string ToString() => $"{_entityId}:'{name}'";
}