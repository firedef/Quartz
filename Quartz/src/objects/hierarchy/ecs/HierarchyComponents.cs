using System.Runtime.InteropServices;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.objects.hierarchy.ecs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct HierarchyComponent: IComponent {
	public EntityId parent = EntityId.@null;
	public EntityId firstChild = EntityId.@null;
	public EntityId nextSibling = EntityId.@null;
	public byte hierarchyLevel;

	public override string ToString() => $"(parent: {parent} child: {firstChild} sibling: {nextSibling} level: {hierarchyLevel})";
	
	public void Parse(Dictionary<string, string> fields) {
		string? v;
		if (fields.TryGetValue("parent", out v)) parent = int.Parse(v);
		if (fields.TryGetValue("firstChild", out v)) firstChild = int.Parse(v);
		if (fields.TryGetValue("nextSibling", out v)) nextSibling = int.Parse(v);
		if (fields.TryGetValue("hierarchyLevel", out v)) hierarchyLevel = byte.Parse(v);
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}