using System.Runtime.InteropServices;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.entities; 

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Entity {
	public EntityId id;
	public uint version;
	public WorldId worldId;
	public EntityFlags flags;

	public bool isAlive => (flags & EntityFlags.isAlive) != 0;

	public override string ToString() => $"{id}.{version}";
}