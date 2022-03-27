namespace Quartz.collections.shaders; 

public struct GeometryShader : IShaderPart {
	public string? source { get; set; }
	public int handle { get; set; }
	
	public GeometryShader(string? src) : this() => source = src;
}