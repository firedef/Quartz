using ImGuiNET;
using Quartz.collections;
using Quartz.CoreCs.collections;
using Quartz.CoreCs.other.events;
using Quartz.CoreCs.other.time;

namespace Quartz.ui.imgui.windows; 

public class ProfilerWindow : EditorWindow {
	public static ProfilerWindow? global;

	private static RingBuffer<float> _frameTimeHistory = new(1024);
	private static RingBuffer<float> _fixedTimeHistory = new(1024);
	private static float[] _pipelineEvents = new float[FixedUpdatePipeline.bufferLength]; 
	//private static RingBuffer<int> _eventManagerEvents = new(1024);

	public ProfilerWindow() => Open();
	
	public override string GetWindowName() => "profiler";
	
	protected override void OnOpen() {
		
	}
	protected override void OnClose() {
		global = null;
	}
	protected override unsafe void Layout() {
		float min, max, avg;

		min = _frameTimeHistory.buffer.Min();
		max = _frameTimeHistory.buffer.Max();
		avg = _frameTimeHistory.buffer.Average();
		ImGui.Text($"frame time (min: {min:0.00}ms, max: {max:0.00}ms, avg: {avg:0.00})");
		fixed(float* ptr = _frameTimeHistory.buffer)
			ImGui.PlotHistogram("frame time", ref *ptr, _frameTimeHistory.capacity, _frameTimeHistory.pos, "", min, max, new(ImGui.GetWindowWidth(), 64));
		_frameTimeHistory.Add(Time.frameTime * 1000);
		
		min = _fixedTimeHistory.buffer.Min();
		max = _fixedTimeHistory.buffer.Max();
		avg = _fixedTimeHistory.buffer.Average();
		ImGui.Text($"fixed time (min: {min:0.00}ms, max: {max:0.00}ms, avg: {avg:0.00})");
		fixed(float* ptr = _fixedTimeHistory.buffer)
			ImGui.PlotHistogram("fixed time", ref *ptr, _fixedTimeHistory.capacity, _fixedTimeHistory.pos, "", min, max, new(ImGui.GetWindowWidth(), 64));
		_fixedTimeHistory.Add(Time.fixedTime * 1000);

		RingBuffer<FixedUpdatePipeline.Events> fixedPipelineBuffer = FixedUpdatePipeline.GetRingBuffer();
		min = fixedPipelineBuffer.buffer.Min(v => v.events.Count);
		max = fixedPipelineBuffer.buffer.Max(v => v.events.Count);
		avg = (float)fixedPipelineBuffer.buffer.Average(v => v.events.Count);
		//ImGui.Text($"fixed time (min: {min:0.00}ms, max: {max:0.00}ms, avg: {avg:0.00})");

		for (int i = 0; i < _pipelineEvents.Length; i++) {
			_pipelineEvents[i] = fixedPipelineBuffer[-i].events.Count;
		}
		
		fixed(float* ptr = _pipelineEvents)
			ImGui.PlotHistogram("fixed time", ref *ptr, _pipelineEvents.Length, 0, "", min, max, new(ImGui.GetWindowWidth(), 64));

		for (int i = 0; i < FixedUpdatePipeline.bufferLength; i++) {
			foreach (FixedUpdatePipeline.Event ev in fixedPipelineBuffer[-i].events) {
				ImGui.Text($"[+{i}] {ev.name}");
			}
		}
		//_fixedTimeHistory.Add(Time.fixedTime * 1000);
	}
	
	public static void OpenWindow() => global ??= new();
}