using System.Text;
using ImGuiNET;
using MathStuff;
using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.world;
using Quartz.ui.imgui.windows;

namespace Quartz.ui.imgui; 

public static class EcsSerialize {
	private static int _limEntitiesStart = 0;
	private static int _limEntitiesEnd = 2048;
	
	public static unsafe void SerializeComponent(ComponentType comp, void* ptr, bool isReadonly = false) {
		ImGuiElements.SerializeFields(comp.type, ptr, isReadonly);
	}

	public static unsafe void SerializeEntityComponents(Archetype arch, EntityId entityId, bool isReadonlyArchetype = false, bool isReadonly = false) {
		uint compIndex = arch.GetComponentIdFromEntity(entityId);
		int compCount = arch.componentTypes.Length;
		for (int i = 0; i < compCount; i++) {
			ImGui.TextColored(ImGuiColors.text2, $"{arch.componentTypes[i].type.Name}");
			ImGuiElements.Tooltip(arch.componentTypes[i].type.FullName!);
			
			if (!isReadonlyArchetype) {
				ImGui.SameLine(ImGui.GetWindowWidth() - 64);
				
				ImGui.PushID($"[{entityId},{i}].remove".GetHashCode());
				if (ImGui.Button("remove")) {
					entityId.world.Remove(entityId, arch.componentTypes[i]);
					ImGui.PopID();
				}
				ImGui.PopID();
			}
			
			SerializeComponent(arch.componentTypes[i], arch.GetComponent(i, compIndex), isReadonly);
			ImGui.Separator();
		}
	}

	public static void SerializeEntity(EntityId entityId, bool isReadonly = false, bool serializeComponents = true, bool fold = true) {
		World world = entityId.world;
		Archetype? archetype = world.GetEntityArchetype(entityId);
		
		StringBuilder info = new();
		info.AppendLine($"id: {entityId.id}");
		info.AppendLine($"version: {entityId.entity.version}");
		info.AppendLine($"world: {world.worldName}");
		if (archetype == null) info.AppendLine("archetype: null");
		else info.AppendLine($"archetype: {archetype.id}");
		
		if (fold) {
			bool foldResult = ImGui.TreeNode($"{entityId}_{entityId.entity.version}");
			ImGuiElements.Tooltip(info.ToString());
			if (!foldResult) return;
		}
		
		ImGui.TextColored(ImGuiColors.warn, info.ToString());
		
		if (serializeComponents) {
			if (archetype == null) ImGui.TextColored(ImGuiColors.error, "no components");
			else SerializeEntityComponents(archetype, entityId, isReadonly, isReadonly);
		}

		if (!isReadonly) {
			ImGui.PushID($"[{entityId}].remove".GetHashCode());
			if (ImGui.Button("destroy entity")) {
				entityId.Destroy();
				ImGui.PopID();
				return;
			}
			ImGui.PopID();
		}
		
		ImGui.Separator();
		if (fold) ImGui.TreePop();
	}

	public static void SerializeWorldEntitiesAsTree(World world, bool isReadonly = false) {
		ImGui.DragIntRange2("entities range", ref _limEntitiesStart, ref _limEntitiesEnd, 0.25f, 0, 500_000);
		int end = math.min(_limEntitiesEnd, world.currentEntityCount);

		int pos = 0;
		for (int i = 0; i < _limEntitiesStart; i++) {
			Entity entity = World.GetEntity(pos);
			pos++;
			if (entity.world != world.worldId) i--;
		}

		const int columnCount = 12;
		ImGui.BeginTable("entities", columnCount);
		for (int i = _limEntitiesStart; i < end; i++) {
			Entity entity = World.GetEntity(pos);
			pos++;
			if (entity.world != world.worldId) {
				i--;
				continue;
			}

			if ((i - _limEntitiesStart) % columnCount == 0) ImGui.TableNextRow();
			ImGui.TableNextColumn();
			
			Archetype? archetype = world.GetEntityArchetype(entity);
			StringBuilder info = new(64);
			info.AppendLine($"id: {entity.id.id}");
			info.AppendLine($"version: {entity.version}");
			info.AppendLine($"world: {world.worldName}");
			if (archetype == null) info.AppendLine("archetype: null");
			else info.AppendLine($"archetype: {archetype.id}");

			bool btn = ImGui.Button($"{entity}".PadRight(24));
			ImGuiElements.Tooltip(info.ToString());

			if (btn) EntityWindow.OpenNew(entity.id, isReadonly);
		}
		
		ImGui.EndTable();
	}

	public static void SerializeWorldArchetypes(World world, bool isReadonly = false) {
		int c = world.archetypeCount;
		for (int i = 0; i < c; i++) {
			Archetype archetype = world.GetArchetypeById(i);
			if (ImGui.TreeNode($"archetype {archetype.id}")) {
				int eCount = archetype.components.entityCount;
				int cCount = archetype.components.componentsPerEntity;
				ImGui.TextColored(ImGuiColors.warn, $"archetype: {archetype.id}, component count: {cCount}, entity count: {eCount}");
				ImGui.Separator();
				for (int comp = 0; comp < archetype.componentTypes.Length; comp++) {
					ComponentType componentType = archetype.componentTypes[comp];
					ImGui.TextColored(ImGuiColors.text2, $"{componentType.type}");
				}
				ImGui.Separator();
				ImGui.TreePop();
			}
		}
	}

	public static void SerializeWorld(World world, bool isReadonly = false) {
		ImGui.Text($"world {world.worldId.id}: '{world.worldName}'");
		ImGui.LabelText($"{world.currentEntityCount}", "entity count:");
		ImGui.LabelText($"{world.archetypeCount}", "archetype count:");
		ImGui.Checkbox("active", ref world.isActive);
		ImGui.Checkbox("visible", ref world.isVisible);

		if (world.archetypeCount == 0) {
			ImGui.TextColored(ImGuiColors.error, "no entity archetypes");
			return;
		}
		if (ImGui.TreeNode("archetypes")) {
			SerializeWorldArchetypes(world, isReadonly);
			ImGui.TreePop();
		}
		
		if (world.currentEntityCount == 0) {
			ImGui.TextColored(ImGuiColors.error, "no entities");
			return;
		}

		if (ImGui.TreeNode("entities")) {
			SerializeWorldEntitiesAsTree(world, isReadonly);
			ImGui.TreePop();
		}
	}
}