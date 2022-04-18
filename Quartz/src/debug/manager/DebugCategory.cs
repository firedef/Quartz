using System.Reflection;

namespace Quartz.debug.manager; 

public class DebugCategory {
	public string categoryName = "?";
	public List<(MethodInfo, DebugMemberAttribute)> methods = new();
	public List<(FieldInfo, DebugMemberAttribute)> fields = new();
	public List<(FieldInfo, DebugMemberFieldAttribute)> editableFields = new();
	public readonly List<DebugCategory> childs = new();

	public DebugCategory(string categoryName) => this.categoryName = categoryName;

	public DebugCategory GetChild(string name) {
		int c = childs.Count;
		for (int i = 0; i < c; i++) {
			if (childs[i].categoryName != name) continue;
			return childs[i];
		}
		
		childs.Add(new(name));
		return childs[^1];
	}
}