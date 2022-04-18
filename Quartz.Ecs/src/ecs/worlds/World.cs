using MathStuff;
using Quartz.CoreCs.collections;
using Quartz.CoreCs.other;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.commands;
using Quartz.Ecs.ecs.components.shared;
using Quartz.Ecs.ecs.entities;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.worlds; 

public partial class World {
	public static event Action<EntityId> OnEntityCreate = _ => { };
	public static event Action<EntityId> OnEntityDestroy = _ => { };
	public static EventFunc<EntityId, color> getEntityColor = new();
	public static EventFunc<EntityId, float> getEntityOpacity = new();
	
	private static readonly object _lock = new();
	
	public string name;
	public WorldId worldId;

	public bool isAlive = true;
	public bool isInteractable = true;
	public bool isActive = true;
	public bool isVisible = true;

	private static readonly EcsCommandBuffer _commandBuffer = new();
	public static readonly NativeListPool<Entity> entities = new(65536);
	public static readonly Dictionary<EntityId, string> entityNames = new();
	public static readonly ManagedListPool<World> worlds = new();
	public static readonly SharedComponents sharedComponents = new();
	public readonly ArchetypeRoot archetypes = new();

	public int archetypeCount => archetypes.archetypeCount;
	public static int worldCount => worlds.elementCount;
	public static int totalWorldCount => worlds.storage.Count;
	
	public int entityCount;
	public static int maxAliveEntityId => entities.count - 1;
	public static int totalEntityCount;
	public static int totalDeadEntityCount => entities.count - totalEntityCount;
	public static EntityId maxAnyEntityId { get; private set; } = -1;
	
	public static readonly World general = CreateGeneralWorld();
	public static readonly World prefabs = CreatePrefabsWorld();

	static World() {
		EventManager.ProcessCurrentAssembly();

		getEntityColor += (e, v) => {
			color col;
			if (!e.isAlive) col = "#f54263";
			else if (e.world.GetEntityArchetype(e) == null) col = "#464a57";
			else col = e.world.GetEntityArchetype(e)!.archetypeColor;

			return col;
		};

		getEntityOpacity += (e, v) => {
			float opacity = !e.isAlive ? 1f : e.name != null ? 0.5f : 0.4f;
			return opacity;
		};
	}

	private World(string name, WorldId worldId) {
		this.name = name;
		this.worldId = worldId;
	}

	private static World CreateGeneralWorld() {
		World world = Create("general");
		return world;
	}
	
	private static World CreatePrefabsWorld() {
		World world = Create("prefabs");
		world.isInteractable = world.isActive = world.isVisible = false;
		return world;
	}

	public static World Create(string name) {
		lock (_lock) {
			int index = worlds.Add(null!);
			World world = new(name, index);
			worlds.storage[index] = world;
			return world;
		}
	}

	public void DestroyWorld() {
		lock (_lock) {
			isAlive = isActive = isVisible = isInteractable = false;
			worlds.RemoveAt(worldId);
		}
	}
	public static void ClearAll() {
		lock (_lock) {
			entities.Clear();
			worlds.storage.Clear();
			worlds.emptyIndices.Clear();
			totalEntityCount = 0;
		}
	}

	public static World? GetWorldAt(int index) {
		lock (_lock) return worlds.emptyIndices.Contains(index) ? null : worlds.storage[index];
	}

	public static void AddCommand(IEcsCommand cmd) {
		lock (_lock) _commandBuffer.Add(cmd);
	}

	[CallRepeating(EventTypes.fixedUpdate)]
	private static void OnFixedUpdate() {
		if (!_commandBuffer.isEmpty) FixedUpdatePipeline.WaitForEmptyAndExecute(_commandBuffer.Execute);
	}

	public static void ForeachWorld(Action<World> a, bool onlyActive = true) {
		lock (_lock) {
			int c = worldCount;
			for (int i = 0; i < c; i++) {
				World? world = GetWorldAt(i);
				if (world is {isAlive: true} && (world.isActive || !onlyActive)) a(world);
			}
		}
	}

	public static void Lock() => Monitor.Enter(_lock);
	public static void Unlock() => Monitor.Exit(_lock);

	public static color GetEntityColor(EntityId e) => getEntityColor.Invoke(color.white, e).WithAlpha((byte)(255 * getEntityOpacity.Invoke(1, e)));
}