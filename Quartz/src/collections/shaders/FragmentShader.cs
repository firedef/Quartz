namespace Quartz.collections.shaders; 

public struct FragmentShader : IShaderPart {
	public string? source { get; set; }
	public int handle { get; set; }
	
	public FragmentShader(string? src) : this() => source = src;
}