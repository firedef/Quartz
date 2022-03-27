using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.collections.shaders;
using Quartz.debug.log;

namespace Quartz.utils; 

public static class OpenGl {
	public static ShaderProgram boundShaderProgram = ShaderProgram.empty;
	
	public static void BindShader(ref ShaderProgram v) {
		if (!v.isGenerated) v.Compile();
		if (boundShaderProgram == v) return;
		boundShaderProgram = v;
		Log.Minimal($"bound shader {v.programHandle.Handle}", LogForm.rendererCore);
		GL.UseProgram(v.programHandle);
	}
	
	public static void UnbindShader() {
		if (boundShaderProgram == ShaderProgram.empty) return;
		Log.Minimal($"unbound shader {boundShaderProgram.programHandle.Handle}", LogForm.rendererCore);
		boundShaderProgram = ShaderProgram.empty;
		GL.UseProgram(ProgramHandle.Zero);
	}
	
	public static void CheckErrors() {
		while (true) {
			ErrorCode errorCode = GL.GetError();
			if (errorCode == ErrorCode.NoError) return;
			Log.Error($"OpenGl error {(int) errorCode}: {errorCode}");
		}
	}
	
	public static void CheckErrors(string partName) {
		while (true) {
			ErrorCode errorCode = GL.GetError();
			if (errorCode == ErrorCode.NoError) return;
			Log.Error($"OpenGl error {(int) errorCode}: {errorCode} ({partName})");
		}
	}
}