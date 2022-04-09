using ImGuiNET;
using Quartz.other.events;

namespace Quartz.ui.imgui.windows; 

public abstract class EditorWindow {
	protected static readonly List<EditorWindow> openedWindows = new();
	protected static readonly object _lock = new();

	public abstract string GetWindowName();
	protected abstract void OnOpen();
	protected abstract void OnClose();

	public void Open() {
		lock (_lock) {
			openedWindows.Add(this);
			OnOpen();
		}
	}

	public void Close() {
		lock (_lock) {
			openedWindows.Remove(this);
			OnClose();
		}
	}

	protected abstract void Layout();

	public void Draw() {
		bool b = true;
		if (ImGui.Begin(GetWindowName(), ref b)) Layout();
		if (!b) Close();
		ImGui.End();
	}
	
	[CallRepeating(EventTypes.imgui)]
	private static void OnDraw() {
		lock (_lock) {
			for (int i = 0; i < openedWindows.Count; i++) openedWindows[i].Draw();
		}
	}
}