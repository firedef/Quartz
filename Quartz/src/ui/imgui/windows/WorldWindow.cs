using ImGuiNET;
using Quartz.objects.ecs.world;

namespace Quartz.ui.imgui.windows; 

public class WorldWindow : EditorWindow {
	public static WorldWindow? global;

	public bool onlyActive = false;
	
	public WorldWindow() => Open();
	
	public override string GetWindowName() => $"worlds";
	
	protected override void OnOpen() {
		
	}
	protected override void OnClose() {
		global = null;
	}
	protected override void Layout() {
		ImGui.LabelText($"{World.worldCount}", "world count:");
		ImGui.LabelText($"{World.totalGlobalEntityCount}", "total entity count:");
		ImGui.LabelText($"{World.deadEntityCount}", "dead entity count:");

		if (ImGui.TreeNode("world list")) {
			World.ForeachWorld(world => {
				ImGui.Separator();
				if (!ImGui.TreeNode($"world #{world.worldId.id} '{world.worldName}'")) return;
				EcsSerialize.SerializeWorld(world);
				
				ImGui.TreePop();
				ImGui.Separator();
			}, onlyActive);
			
			ImGui.TreePop();
		}
		
		if (ImGui.TreeNode("world settings")) {
			ImGuiElements.SerializeBool("show only active", "worlds: show only active".GetHashCode(), ref onlyActive);
			ImGui.TreePop();
		}
	}

	public static void OpenWindow() => global ??= new();
}