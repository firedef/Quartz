using System.Reflection;
using ImGuiNET;

namespace Quartz.ui.imgui.windows; 

public class ReflectionWindow : EditorWindow {
	public static ReflectionWindow? global;
	public bool onlyPublic = true;
	public bool onlyStatic = false;
	public bool onlyNonAbstract = false;

	public ReflectionWindow() => Open();
	
	public override string GetWindowName() => "reflection window";
	
	protected override void OnOpen() {
		
	}
	protected override void OnClose() {
		global = null;
	}
	protected override unsafe void Layout() {
		ImGui.Checkbox("show only public", ref onlyPublic);
		ImGui.Checkbox("show only static", ref onlyStatic);
		ImGui.Checkbox("show only non-abstract", ref onlyNonAbstract);

		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		if (ImGui.TreeNode($"assemblies [{assemblies.Length}]")) {
			foreach (Assembly asm in assemblies) { 
				ImGui.PushStyleColor(ImGuiCol.Text, asm == Assembly.GetExecutingAssembly() ? ImGuiColors.warn : ImGuiColors.text1);
				if (ImGui.TreeNode(asm.FullName)) {
					ImGui.PopStyleColor();
				
					Type[] types = asm.GetTypes();
			
					foreach (Type type in types) {
						if (onlyPublic && type.IsNotPublic) continue;
						if (onlyNonAbstract && type.IsAbstract) continue;

						if (ImGui.TreeNode(type.Name)) {
							SerializeType(type);
							ImGui.TreePop();
						}
					}
			
					ImGui.TreePop();
				}
				else ImGui.PopStyleColor();
			}
			
			ImGui.TreePop();
		}
	}

	protected unsafe void SerializeType(Type t) {
		ImGui.TextColored(ImGuiColors.warn, t.FullName);

		MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

		if (ImGui.TreeNode($"fields [{fields.Length}]")) {
			foreach (FieldInfo field in fields) SerializeField(field);
			ImGui.TreePop();
		}
		
		if (ImGui.TreeNode($"methods [{methods.Length}]")) {
			foreach (MethodInfo method in methods) SerializeMethod(method);
			ImGui.TreePop();
		}

		ImGui.Separator();
	}

	protected unsafe void SerializeField(FieldInfo f) {
		if (f.IsStatic) {
			ImGui.TextColored(ImGuiColors.text2, $"static {$"{f.FieldType.Name} {f.Name}".PadRight(40)} = {f.GetValue(null)}");
		}
		else ImGui.TextColored(ImGuiColors.text2, $"{f.FieldType.Name} {f.Name}");
		ImGui.Separator();
	}
	
	protected unsafe void SerializeMethod(MethodInfo m) {
		if (m.IsStatic) {
			ImGui.TextColored(ImGuiColors.text2, $"static {m.ReturnType.Name} {m.Name}()");
		}
		else ImGui.TextColored(ImGuiColors.text2, $"{m.ReturnType.Name} {m.Name}()");
		ImGui.Separator();
	}
	
	public static void OpenWindow() => global ??= new();
}