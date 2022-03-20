using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.debug.log;
using Quartz.utils;

namespace Quartz.collections.shaders; 

public struct Shader {
	public static readonly Shader empty = new();
	
	public string? vertexSrc;
	public string? fragmentSrc;

	public ShaderHandle vertexHandle;
	public ShaderHandle fragmentHandle;
	public ProgramHandle programHandle;

	public bool isGenerated => programHandle.Handle != 0;

	public Shader(string vertexSrc, string fragmentSrc) : this() {
		this.vertexSrc = vertexSrc;
		this.fragmentSrc = fragmentSrc;
	}

	public bool Compile() {
		if (isGenerated) return false;

		vertexHandle = GL.CreateShader(ShaderType.VertexShader);
		fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
		programHandle = GL.CreateProgram();

		GL.ShaderSource(vertexHandle, vertexSrc!);
		GL.CompileShader(vertexHandle);
		
		GL.ShaderSource(fragmentHandle, fragmentSrc!);
		GL.CompileShader(fragmentHandle);
		
		GL.AttachShader(programHandle, vertexHandle);
		GL.AttachShader(programHandle, fragmentHandle);
		GL.LinkProgram(programHandle);

		Log.Note($"compile shader {programHandle.Handle}");

		return true;
	}

	public void Bind() => OpenGl.BindShader(ref this);

	public int AttributeLocation(string name) {
		if (!isGenerated) Compile();
		return GL.GetAttribLocation(programHandle, name);
	}
	
	public void ProcessAttribute(string name, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
		if (!isGenerated) Compile();
		uint loc = (uint) GL.GetAttribLocation(programHandle, name);
		
		GL.VertexAttribPointer(loc, size, type, normalized, stride, offset);
		GL.EnableVertexAttribArray(loc);
	}
	
	public void Dispose() {
		if (!isGenerated) return;
		if (OpenGl.boundShader == this) OpenGl.UnbindShader();
		
		GL.DeleteProgram(programHandle);
		GL.DeleteShader(fragmentHandle);
		GL.DeleteShader(vertexHandle);
		
		Log.Note($"delete shader {programHandle}");
		
		vertexHandle = ShaderHandle.Zero;
		fragmentHandle = ShaderHandle.Zero;
		programHandle = ProgramHandle.Zero;
	}

	public static bool operator ==(Shader a, Shader b) => a.programHandle == b.programHandle;
	public static bool operator !=(Shader a, Shader b) => a.programHandle != b.programHandle;

	public bool Equals(Shader other) => programHandle.Equals(other.programHandle);
	public override bool Equals(object? obj) => obj is Shader other && Equals(other);
	public override int GetHashCode() => programHandle.GetHashCode();
}