using System.Numerics;
using System.Text;
using ImGuiNET;
using MathStuff;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.commands;
using Quartz.Ecs.ecs.entities;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;
using Quartz.ui.imgui.windows;
using Quartz.ui.windows;

namespace Quartz.ui.imgui; 

public static class EcsSerialize {
	private static int _limEntitiesStart = 0;
	private static int _limEntitiesEnd = 2048;
	
	public static unsafe void SerializeComponent(ComponentType comp, void* ptr, bool isReadonly = false) {
		ImGuiElements.SerializeFields(comp.data.type, ptr, isReadonly);
	}

	public static unsafe void SerializeEntityComponents(Archetype arch, EntityId entityId, bool isReadonlyArchetype = false, bool isReadonly = false) {
		uint compIndex = arch.GetComponent(entityId);
		int compCount = arch.normalComponentCount;
		for (int i = 0; i < compCount; i++) {
			ImGui.TextColored(ImGuiColors.text2, $"{arch.components._normal.types[i].data.type.Name}");
			ImGuiElements.Tooltip(arch.components._normal.types[i].data.type.FullName!);
			
			if (!isReadonlyArchetype) {
				ImGui.SameLine(ImGui.GetWindowWidth() - 64);
				
				ImGui.PushID($"[{entityId},{i}].remove".GetHashCode());
				if (ImGui.Button("remove")) {
					World.AddCommand(new RemoveComponentEcsCommand(entityId.world, entityId, arch.components._normal.types[i]));
					ImGui.PopID();
				}
				ImGui.PopID();
			}
			
			SerializeComponent(arch.components._normal.types[i], arch.GetComponentPtr(entityId, arch.components._normal.types[i]), isReadonly);
			ImGui.Separator();
		}
	}

	public static void SerializeEntity(EntityId entityId, bool isReadonly = false, bool serializeComponents = true, bool fold = true) {
		World world = entityId.world;
		Archetype? archetype = world.GetEntityArchetype(entityId);

		if (fold) {
			StringBuilder info = new();
			info.AppendLine($"id: {entityId.entity.id.position}");
			info.AppendLine($"name: {entityId.entity.id.name}");
			info.AppendLine($"version: {entityId.entity.version}");
			info.AppendLine($"world: {world.name}");
			if (archetype == null) info.AppendLine("archetype: null");
			else info.AppendLine($"archetype: {archetype.id}");
			
			bool foldResult = ImGui.TreeNode($"{entityId}_{entityId.entity.version}");
			ImGuiElements.Tooltip(info.ToString());
			if (!foldResult) return;
		}

		ImGui.TextColored(ImGuiColors.warn, $"id: {entityId.entity.id.position}");
		ImGui.TextColored(ImGuiColors.warn, $"version: {entityId.entity.version}");
		ImGui.TextColored(ImGuiColors.warn, $"world: {world.name}");
		ImGui.TextColored(ImGuiColors.warn, $"archetype: {archetype?.id.ToString() ?? "null"}");

		string entityName = entityId.name ?? "";
		
		ImGui.Text("name: ");
		ImGui.SameLine();
		ImGui.PushID($"entity {entityId.position}");
		if (ImGui.InputText("", ref entityName, 24)) {
			ImGui.PopID();
			if (string.IsNullOrWhiteSpace(entityName)) entityId.name = null;
			else entityId.name = entityName;
		}
		else ImGui.PopID();
		
		if (serializeComponents) {
			if (archetype == null) ImGui.TextColored(ImGuiColors.error, "no components");
			else SerializeEntityComponents(archetype, entityId, isReadonly, isReadonly);
		}

		if (!isReadonly) {
			ImGui.PushID($"[{entityId}].remove".GetHashCode());
			if (ImGui.Button("destroy entity")) {
				World.AddCommand(new DestroyEntityEcsCommand(world, entityId));
				ImGui.PopID();
				return;
			}
			ImGui.PopID();
			
			ImGui.PushID($"[{entityId}].add".GetHashCode());
			if (ImGui.Button("add component")) {
				ImGui.PopID();
				ImGui.OpenPopup($"add component to {entityId}");
			}
			else ImGui.PopID();

			if (ImGui.BeginPopup($"add component to {entityId}")) {
				ImGui.TextColored(ImGuiColors.error, "component add in editor is not implemented yet");
				ImGui.Button("component a");
				ImGui.Button("component b");
				ImGui.Button("component c");
			}
			ImGui.EndPopup();
		}
		
		if (fold) ImGui.TreePop();
	}

	public static void SerializeWorldEntitiesAsTree(World world, Archetype? selectedArchetype, Action<Entity> onBtnPress, bool showCustomNames, bool showDead, bool isReadonly = false) {
		ImGui.DragIntRange2("entities range", ref _limEntitiesStart, ref _limEntitiesEnd, 0.25f, 0, 500_000);
		int end = math.min(_limEntitiesEnd, World.maxAliveEntityId + 1);

		int pos = 0;
		for (int i = 0; i < _limEntitiesStart; i++) {
			Entity entity = World.GetEntity(pos);
			pos++;
			if (entity.worldId != world.worldId || (!showDead && !entity.isAlive)) i--;
		}

		float maxWidth = ImGui.GetWindowContentRegionMax().X - 16;
		float curWidth = 0;
		float widthAdd = ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetStyle().ItemSpacing.X;
		
		for (int i = _limEntitiesStart; i < end; i++) {
			Entity entity = World.GetEntity(pos);
			pos++;
			if (pos > World.maxAliveEntityId + 1) break;
			if (entity.worldId != world.worldId || (!showDead && !entity.isAlive)) {
				i--;
				continue;
			}

			Archetype? archetype = world.GetEntityArchetype(entity.id);
			if (archetype == null && selectedArchetype != null) {
				i--;
				continue;
			}
			if (archetype != null && selectedArchetype != null && archetype != selectedArchetype) {
				i--;
				continue;
			}

			string btnLabel = showCustomNames ? entity.id.ToString() : entity.id.position.ToString();
			float w = ImGui.CalcTextSize(btnLabel).X + widthAdd;
			curWidth += w;
			if (i == _limEntitiesStart || curWidth >= maxWidth) curWidth = w;
			else ImGui.SameLine();

			ImGui.PushID($"{entity.id} {entity.id.position}");
			bool btn = ImGuiElements.ButtonColored(btnLabel, World.GetEntityColor(entity.id));
			ImGui.PopID();
			
			if (ImGuiElements.BeginTooltip()) {
				StringBuilder info = new(64);
				info.AppendLine($"id: {entity.id.position}");
				info.AppendLine($"version: {entity.version}");
				info.AppendLine($"world: {world.name}");
				if (archetype == null) info.AppendLine("archetype: null");
				else info.AppendLine($"archetype: {archetype.id}");
				ImGui.Text(info.ToString());
				ImGuiElements.EndTooltip();
			}

			if (btn) onBtnPress(entity);
		}
	}

	public static void SerializeWorldArchetypes(World world, ref Archetype? selectedArchetype, bool isReadonly = false) {
		int c = world.archetypeCount;
		for (int i = 0; i < c; i++) {
			Archetype archetype = world.GetArchetypeAt(i);
			if (ImGui.TreeNode($"archetype {archetype.id}")) {
				int eCount = archetype.components.elementCount;
				int cCount = archetype.components.normalComponentCount;
				ImGui.TextColored(ImGuiColors.warn, $"archetype: {archetype.id}, component count: {cCount}, entity count: {eCount}");

				Vector4 col = archetype.archetypeColor.ToVec4();
				if (ImGui.ColorEdit4("color", ref col)) {
					archetype.archetypeColor = col.ToColor();
				}
				
				ImGui.Separator();
				for (int comp = 0; comp < archetype.normalComponentCount; comp++) {
					ComponentType componentType = archetype.components._normal.types[comp];
					ImGui.TextColored(ImGuiColors.text2, $"{componentType.data.type}");
				}
				if (ImGui.Button($"select archetype #{archetype.id}")) selectedArchetype = selectedArchetype == archetype ? null : archetype;
				ImGui.Separator();
				ImGui.TreePop();
			}
		}
	}

	public static void SerializeWorld(World world, ref Archetype? selectedArchetype, Action<Entity> onBtnPress, bool showCustomNames, bool showDead = true, bool isReadonly = false) {
		ImGui.Text($"world {world.worldId.position}: '{world.name}'");
		ImGui.LabelText($"{world.entityCount}", "entity count:");
		ImGui.LabelText($"{world.archetypeCount}", "archetype count:");
		ImGui.Checkbox("active", ref world.isActive);
		ImGui.Checkbox("visible", ref world.isVisible);

		if (world.archetypeCount == 0) {
			ImGui.TextColored(ImGuiColors.error, "no entity archetypes");
			return;
		}
		if (ImGui.TreeNode("archetypes")) {
			SerializeWorldArchetypes(world, ref selectedArchetype, isReadonly);
			ImGui.TreePop();
		}
		
		if (world.entityCount == 0) {
			ImGui.TextColored(ImGuiColors.error, "no entities");
			return;
		}

		if (ImGui.TreeNode("entities")) {
			SerializeWorldEntitiesAsTree(world, selectedArchetype, onBtnPress, showCustomNames, showDead, isReadonly);
			ImGui.TreePop();
		}
	}
}