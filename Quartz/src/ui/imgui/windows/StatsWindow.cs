using ImGuiNET;
using Quartz.objects.memory;
using Quartz.other;
using Quartz.other.time;

namespace Quartz.ui.imgui.windows; 

public class StatsWindow : EditorWindow {
	public static StatsWindow? global;
	
	public StatsWindow() => Open();
	
	public override string GetWindowName() => "stats";
	
	protected override void OnOpen() {
		
	}
	protected override void OnClose() {
		
	}
	protected override void Layout() {
		ImGui.LabelText($"{Time.frameDeltaTime*1000:0.00}ms", "frame delta time: ");
		ImGui.LabelText($"{Time.fixedDeltaTime*1000:0.00}ms", "fixed delta time: ");
		ImGui.LabelText($"{(1f / Time.frameDeltaTime):0}", "fps: ");
		ImGui.LabelText($"{MemoryAllocator.currentAllocated.ToStringData()}", "current allocated bytes:");
		ImGui.LabelText($"{MemoryAllocator.totalAllocated.ToStringData()}", "total allocated bytes:");
		ImGui.LabelText($"{MemoryAllocator.allocatedSinceLastCleanup.ToStringData()}", "total allocated bytes since last cleanup:");
	}
	
	public static void OpenWindow() => global ??= new();
}