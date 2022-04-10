using System.Buffers;
using ImGuiNET;
using Quartz.collections;
using Quartz.objects.memory;
using Quartz.other;
using Quartz.other.time;

namespace Quartz.ui.imgui.windows; 

public class StatsWindow : EditorWindow {
	public static StatsWindow? global;

	private static float _frameDeltaTime;
	private static float _fixedDeltaTime;
	private static float _frameTime;
	private static float _fixedTime;
	private static int _frame;

	private static RingBuffer<float> _frameDeltaTimeHistory = new(256);

	public StatsWindow() => Open();
	
	public override string GetWindowName() => "stats";
	
	protected override void OnOpen() {
		
	}
	protected override void OnClose() {
		global = null;
	}
	protected override unsafe void Layout() {
		if (_frame % 16 == 0) {
			_frameDeltaTime = Time.frameDeltaTime * 1000;
			_fixedDeltaTime = Time.fixedDeltaTime * 1000;
			_frameTime = Time.frameTime * 1000;
			_fixedTime = Time.fixedTime * 1000;
		}
		_frame++;
		ImGui.LabelText($"{_frameDeltaTime:0.00}ms | +{_frameTime:0.00}ms", "frame delta time: ");
		
		fixed(float* ptr = _frameDeltaTimeHistory.buffer)
			ImGui.PlotLines("frame", ref *ptr, _frameDeltaTimeHistory.capacity, _frameDeltaTimeHistory.pos, "", _frameDeltaTimeHistory.buffer.Min(), _frameDeltaTimeHistory.buffer.Max(), new(ImGui.GetWindowWidth(), 32));
		_frameDeltaTimeHistory.Add(Time.frameDeltaTime * 1000);
		
		ImGui.LabelText($"{_fixedDeltaTime:0.00}ms | +{_fixedTime:0.00}ms", "fixed delta time: ");
		ImGui.LabelText($"{(1000f / _frameDeltaTime):0}", "fps: ");
		ImGui.LabelText($"{MemoryAllocator.currentAllocated.ToStringData()}", "current allocated bytes:");
		ImGui.LabelText($"{MemoryAllocator.totalAllocated.ToStringData()}", "total allocated bytes:");
		ImGui.LabelText($"{MemoryAllocator.allocatedSinceLastCleanup.ToStringData()}", "total allocated bytes since last cleanup:");
	}
	
	public static void OpenWindow() => global ??= new();
}