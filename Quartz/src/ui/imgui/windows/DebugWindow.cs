using System.Reflection;
using ImGuiNET;
using Quartz.debug.manager;

namespace Quartz.ui.imgui.windows;

//TODO: 
public class DebugWindow : EditorWindow {
	public static DebugWindow? global;
	
	public DebugWindow() => Open();
	
	public override string GetWindowName() => $"debug members";
	protected override void OnOpen() {
		
	}
	
	protected override void OnClose() {
		global = null;
	}
	
	protected override void Layout() {
		// List<(MethodInfo, DebugMemberAttribute)> methods = DebugManager.methods;
		// foreach ((MethodInfo, DebugMemberAttribute) method in methods) {
		// 	if (ImGui.Button(method.Item2.name)) {
		// 		method.Item1.Invoke(null, null);
		// 	}
		// }

		foreach (DebugCategory category in DebugManager.categories) {
			LayoutCategory(category);
		}
	}

	private void LayoutCategory(DebugCategory category) {
		if (!ImGui.TreeNode(category.categoryName)) return;
		
		foreach ((FieldInfo, DebugMemberAttribute) field in category.fields) {
			ImGui.Text($"{field.Item2.name}: {field.Item1.GetValue(null)}");
		}
		
		foreach ((FieldInfo, DebugMemberFieldAttribute) field in category.editableFields) {
			ImGuiElements.SerializeField(field.Item1, 5453, null);
			//ImGui.Text($"{field.Item2.name}: {field.Item1.GetValue(null)}");
		}

		foreach ((MethodInfo, DebugMemberAttribute) method in category.methods) {
			bool btn = ImGui.Button(method.Item2.name);
			if (!string.IsNullOrWhiteSpace(method.Item2.description)) ImGuiElements.Tooltip(method.Item2.description);
			if (btn) method.Item1.Invoke(null, null);
		}

		foreach (DebugCategory c in category.childs) LayoutCategory(c);
		
		ImGui.TreePop();
	}
	
	public static void OpenWindow() => global ??= new();
	
	[DebugMember("Hello.World", "foo", "invoke aaaa")]
	public static void Aaaa() {
		Console.WriteLine($"cfskdf {bar2}");
	}

	[DebugMember("Hello.World", "bar", "bar field")]
	public static float bar = 65;
	
	[DebugMemberField("Hello.World", "bar2", "bar field editable")]
	public static float bar2 = 543;
}