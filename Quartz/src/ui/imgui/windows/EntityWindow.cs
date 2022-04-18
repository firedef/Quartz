using ImGuiNET;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.ui.imgui.windows; 

public class EntityWindow : EditorWindow {
	public EntityId selectedEntity = EntityId.@null;
	public bool isReadOnly = false;

	public EntityWindow() => Open();
	
	public override string GetWindowName() => $"entity {selectedEntity.position}";
	
	protected override void OnOpen() {
	}
	protected override void OnClose() {
	}
	
	protected override void Layout() {
		ImGui.SetWindowSize(new(256,600), ImGuiCond.Once);
		if (!selectedEntity.isAlive) {
			Close();
			return;
		}
		EcsSerialize.SerializeEntity(selectedEntity, isReadOnly, fold:false);
	}

	public void SelectEntity(EntityId id, bool isReadonly = false) {
		selectedEntity = id;
		isReadOnly = isReadonly;
	}
	
	public static EntityWindow? OpenNew(EntityId id, bool isReadonly = false) {
		lock (_lock) {
			EntityWindow? win = (EntityWindow?) openedWindows.FirstOrDefault(v => v is EntityWindow w && w.selectedEntity == id);
			if (win != null) {
				ImGui.SetWindowFocus(win.GetWindowName());
				return null;
			}
			win = new() {selectedEntity = id, isReadOnly = isReadonly};
			return win;
		}
	}
}