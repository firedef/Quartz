using System.Reflection;
using ImGuiNET;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using Quartz.other.events;
using Quartz.ui.imgui.windows;

namespace Quartz.ui.windows; 

public abstract class ImGuiWindow : GameWindow {
	private ImGuiController _controller = null!;

	public ImGuiWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
		
	}
	
	protected override void OnLoad() {
		//typeof(GameWindow).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).First(v => v.Name == "_updateFrequency").SetValue(this, 5000);
		//VSync = VSyncMode.Off;
		base.OnLoad();
		_controller = new(Size.X, Size.Y);
	}

	protected override void OnResize(ResizeEventArgs e) {
		base.OnResize(e);
		_controller.WindowResized(Size.X, Size.Y);
	}

	protected override void OnRenderFrame(FrameEventArgs args) {
		base.OnRenderFrame(args);
		_controller.Update(this);
		
		Layout();
		_controller.Render();
	}

	private void Layout() {
		if (ImGui.Button("open stats window")) StatsWindow.OpenWindow();
		if (ImGui.Button("open profiler window")) ProfilerWindow.OpenWindow();
		if (ImGui.Button("open ecs window")) WorldWindow.OpenWindow();
		
		EventManager.OnImGui();
	}

	protected override void OnTextInput(TextInputEventArgs e) {
		base.OnTextInput(e);
		_controller.PressChar((char) e.Unicode);
	}

	protected override void OnMouseWheel(MouseWheelEventArgs e) {
		base.OnMouseWheel(e);
		_controller.OnMouseScroll(e.Offset);
	}
}