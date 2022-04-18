using System.Reflection;
using ImGuiNET;
using MathStuff;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quartz.CoreCs.other.events;
using Quartz.ui.imgui;
using Quartz.ui.imgui.windows;

namespace Quartz.ui.windows; 

public abstract class ImGuiWindow : GameWindow {
	public static bool shiftPressed = false;
	public static bool ctrlPressed = false;
	public static bool altPressed = false;
	public static bool superPressed = false;
	
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
		
		shiftPressed = KeyboardState.IsKeyDown(Keys.LeftShift) || KeyboardState.IsKeyDown(Keys.RightShift);
		ctrlPressed = KeyboardState.IsKeyDown(Keys.LeftControl) || KeyboardState.IsKeyDown(Keys.RightControl);
		altPressed = KeyboardState.IsKeyDown(Keys.LeftAlt) || KeyboardState.IsKeyDown(Keys.RightAlt);
		superPressed = KeyboardState.IsKeyDown(Keys.LeftSuper) || KeyboardState.IsKeyDown(Keys.RightSuper);
		
		_controller.Update(this);
		
		Layout();
		_controller.Render();
	}

	private void Layout() {
		ImGui.ShowDemoWindow();
		ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4);
		ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 8);
		
		ImGui.PushStyleColor(ImGuiCol.WindowBg, ((color) "#1c202bfa").ToVec4());
		ImGui.PushStyleColor(ImGuiCol.Border, ((color) "#394259").ToVec4());
		
		ImGui.PushStyleColor(ImGuiCol.TitleBg, ((color) "#1c202b").ToVec4());
		ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, ((color) "#1c202bfa").ToVec4());
		ImGui.PushStyleColor(ImGuiCol.TitleBgActive, ((color) "#394259").ToVec4());
		
		ImGui.PushStyleColor(ImGuiCol.Separator, ((color) "#394259").ToVec4());
		
		ImGui.PushStyleColor(ImGuiCol.Text, ((color) "#b0c6ff").ToVec4());
		
		if (ImGui.Button("open stats window")) StatsWindow.OpenWindow();
		if (ImGui.Button("open profiler window")) ProfilerWindow.OpenWindow();
		if (ImGui.Button("open reflection window")) ReflectionWindow.OpenWindow();
		if (ImGui.Button("open debug window")) DebugWindow.OpenWindow();
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