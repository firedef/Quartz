namespace Quartz.graphics.shaders; 

public struct VertexShader : IShaderPart {
	public string? source { get; set; }
	public int handle { get; set; }

	public VertexShader(string? src) : this() => source = src;
}