using System.Numerics;
using ImGuiNET;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.commands;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;
using Quartz.ui.windows;

namespace Quartz.ui.imgui.windows; 

public class WorldWindow : EditorWindow {
	public static WorldWindow? global;

	public bool onlyActive = false;
	public bool showDeadEntities = true;
	public bool showCustomNames = true;
	public static readonly HashSet<EntityId> selectedEntities = new();

	public WorldId selectedWorld = WorldId.@null;
	public Archetype? selectedArchetype = null;

	static WorldWindow() {
		World.getEntityOpacity += (e, v) => selectedEntities.Count > 0 ? selectedEntities.Contains(e) ? 1f : v * .5f : v;
	}
	
	public WorldWindow() => Open();
	
	public override string GetWindowName() => $"worlds";
	
	protected override void OnOpen() {
		
	}
	protected override void OnClose() {
		global = null;
	}
	protected override void Layout() {
		//ImGui.LabelText($"{World.worldCount}", "world count:");
		//ImGui.LabelText($"{World.totalEntityCount}", "total entity count:");
		//ImGui.LabelText($"{World.totalDeadEntityCount}", "dead entity count:");

		// if (ImGui.TreeNode("world settings")) {
		// 	ImGuiElements.SerializeBool("show only active", "worlds: show only active".GetHashCode(), ref onlyActive);
		// 	ImGuiElements.SerializeBool("show dead entities", "worlds: show dead".GetHashCode(), ref showDeadEntities);
		// 	ImGui.TreePop();
		// }

		if (ImGui.Button("settings")) selectedWorld = WorldId.@null;
		World.ForeachWorld(world => {
			ImGui.SameLine();
			
			Vector4 buttonColor = ImGuiColors.ok;
			if (!world.isActive) buttonColor = ImGuiColors.disabled;
			if (selectedWorld == world.worldId) buttonColor = ImGuiColors.warn;
			ImGui.PushStyleColor(ImGuiCol.Button, buttonColor);
			
			bool btn = false;
			if (selectedWorld == world.worldId) btn = ImGuiElements.ButtonYellow(world.name);
			else if (!world.isActive) btn = ImGuiElements.ButtonGray(world.name);
			else btn = ImGuiElements.ButtonPurple(world.name);
			
			ImGui.PopStyleColor();
			if (btn) selectedWorld = world.worldId;
		}, onlyActive);
		
		if (selectedWorld == WorldId.@null) {
			ImGuiElements.SerializeBool("show only active", "worlds: show only active".GetHashCode(), ref onlyActive);
			ImGuiElements.SerializeBool("show dead entities", "worlds: show dead".GetHashCode(), ref showDeadEntities);
			ImGuiElements.SerializeBool("show custom names", "worlds: show custom names".GetHashCode(), ref showCustomNames);
			ImGui.TreePop();
		}
		else {
			World? world = World.GetWorldAt(selectedWorld);
			if (world != null) EcsSerialize.SerializeWorld(world, ref selectedArchetype, e => {
				bool shift = ImGuiWindow.shiftPressed;
				if (shift) {
					if (!selectedEntities.Add(e.id)) selectedEntities.Remove(e.id);
					return;
				}
				EntityWindow.OpenNew(e.id);
				selectedEntities.Clear();
			}, showCustomNames, showDeadEntities);
		}

		if (selectedEntities.Count > 0) {
			if (ImGui.IsMouseClicked(ImGuiMouseButton.Right)) {
				ImGui.OpenPopup("selected entities");
			}
			
			if (ImGui.IsKeyPressed(ImGuiKey.Delete)) {
				foreach (EntityId id in selectedEntities) 
					World.AddCommand(new DestroyEntityEcsCommand(id.world, id));
				selectedEntities.Clear();
			}
			
			if (ImGui.BeginPopup("selected entities")) {
				ImGui.Text($"selected: {selectedEntities.Count}");
				if (ImGui.Button("destroy")) {
					foreach (EntityId id in selectedEntities) 
						World.AddCommand(new DestroyEntityEcsCommand(id.world, id));
					selectedEntities.Clear();
				}
			}
			ImGui.EndPopup();
		}
	}

	public static void OpenWindow() => global ??= new();
}