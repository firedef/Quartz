using System.Collections.Concurrent;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs.commands; 

public class EcsCommandBuffer {
	private readonly ConcurrentQueue<IEcsCommand> _commands = new();
	public bool isEmpty => _commands.IsEmpty;

	public void Add(IEcsCommand cmd) => _commands.Enqueue(cmd);
	
	public void Execute() {
		while (!_commands.IsEmpty)
			if (_commands.TryDequeue(out IEcsCommand? cmd))
				cmd.Execute();
	}
}

public interface IEcsCommand {
	public void Execute();
}

public class DestroyEntityEcsCommand : IEcsCommand {
	private readonly World _world;
	private readonly EntityId _entity;

	public DestroyEntityEcsCommand(World world, EntityId entity) {
		_world = world;
		_entity = entity;
	}

	public void Execute() => _world.DestroyEntity(_entity);	
}

public class CreateEntityEcsCommand : IEcsCommand {
	private readonly World _world;
	private readonly Archetype? _archetype;
	private readonly Action<EntityId> _onCreate;

	public CreateEntityEcsCommand(World world, Archetype? archetype, Action<EntityId> onCreate) {
		_world = world;
		_archetype = archetype;
		_onCreate = onCreate;
	}
	
	public void Execute() => _onCreate(_world.AddEntity(_archetype));	
}

public class CreateEntitiesEcsCommand : IEcsCommand {
	private readonly World _world;
	private readonly Archetype? _archetype;
	private readonly int _count;
	private readonly Action<EntityId> _onCreate;

	public CreateEntitiesEcsCommand(World world, Archetype? archetype, int count, Action<EntityId> onCreate) {
		_world = world;
		_archetype = archetype;
		_count = count;
		_onCreate = onCreate;
	}

	public void Execute() => _world.AddEntities(_count, _archetype, _onCreate);
}

public class AddComponentEcsCommand : IEcsCommand {
	private readonly World _world;
	private readonly EntityId _entity;
	private readonly ComponentType _type;

	public AddComponentEcsCommand(World world, EntityId entity, ComponentType type) {
		_world = world;
		_entity = entity;
		_type = type;
	}
	
	public AddComponentEcsCommand(World world, EntityId entity, Type type) {
		_world = world;
		_entity = entity;
		_type = type.Get();
	}

	public unsafe void Execute() => _world.Comp(_entity, _type);
}

public class RemoveComponentEcsCommand : IEcsCommand {
	private readonly World _world;
	private readonly EntityId _entity;
	private readonly ComponentType _type;

	public RemoveComponentEcsCommand(World world, EntityId entity, ComponentType type) {
		_world = world;
		_entity = entity;
		_type = type;
	}
	
	public RemoveComponentEcsCommand(World world, EntityId entity, Type type) {
		_world = world;
		_entity = entity;
		_type = type.Get();
	}

	public void Execute() => _world.RemoveComp(_entity, _type);
}