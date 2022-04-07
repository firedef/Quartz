using System.Runtime.InteropServices;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;

namespace Quartz.objects.hierarchy.ecs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct HierarchyComponent: IComponent {
	public EntityId parent = EntityId.@null;
	public EntityId firstChild = EntityId.@null;
	public EntityId nextSibling = EntityId.@null;
	public byte hierarchyLevel;

	public override string ToString() => $"(parent: {parent} child: {firstChild} sibling: {nextSibling} level: {hierarchyLevel})";
}