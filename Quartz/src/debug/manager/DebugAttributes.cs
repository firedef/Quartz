namespace Quartz.debug.manager;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
public class DebugMemberAttribute : Attribute {
	public string category;
	public string name;
	public string description;

	public DebugMemberAttribute(string category, string name, string description = "") {
		this.category = category;
		this.name = name;
		this.description = description;
	}
}

[AttributeUsage(AttributeTargets.Field)]
public class DebugMemberFieldAttribute : Attribute {
	public string category;
	public string name;
	public string description;

	public DebugMemberFieldAttribute(string category, string name, string description = "") {
		this.category = category;
		this.name = name;
		this.description = description;
	}
}