using Quartz.CoreCs.collections;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.managed;
using Quartz.objects.ecs.queries;
using Quartz.objects.memory;

namespace Quartz.objects.ecs.world;

public partial class World {
#region fields

	private const int _entityArrayInitialSize = 1 << 16;
	private static readonly object _globalLock = new();
	private static readonly ManagedListPool<World> _worlds = new();

	/// <summary>general-purpose world</summary>
	public static readonly World general = CreateGeneralWorld();

	/// <summary>prefab world</summary>
	public static readonly World prefabs = CreatePrefabWorld();
	
	public static int globalEntityCount { get; private set; }
	public static int totalGlobalEntityCount => _entities.count;
	public static int deadEntityCount => totalGlobalEntityCount - globalEntityCount;
	public static int maxAliveEntityId => _entities.count - 1;
	public static int worldCount => _worlds.elementCount;
	private static readonly NativeList<Entity> _entities = new(_entityArrayInitialSize);

#endregion fields
	
#region world

	/// <summary>get world from id</summary>
	public static World GetWorld(int id) => _worlds.storage[id];

	/// <summary>execute delegate for each world</summary>
	public static void ForeachWorld(Action<World> a, bool onlyActive = true) {
		int c = _worlds.storage.Count;
		for (int i = 0; i < c; i++)
			if (_worlds.storage[i].isAlive && (!onlyActive || _worlds.storage[i].isActive))
				a(_worlds.storage[i]);
	}

	private static World CreateGeneralWorld() => Create("general");

	private static World CreatePrefabWorld() {
		World world = Create("prefabs");
		world.SetActive(false);
		world.SetVisible(false);
		return world;
	}

	public static Query<T0> Select<T0>() where T0 : unmanaged, IComponent => new(); 

#endregion world

#region entities

	/// <summary>get entity from id</summary>
	public static Entity GetEntity(int id) => _entities[id];

#endregion entities
}