using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.collections.shaders;
using Quartz.debug.log;

namespace Quartz.utils; 

public static class OpenGl {
	public static Shader boundShader = Shader.empty;
	
	public static void BindShader(ref Shader v) {
		if (!v.isGenerated) v.Compile();
		if (boundShader == v) return;
		boundShader = v;
		Log.Minimal($"bound shader {v.programHandle.Handle}", LogForm.rendererCore);
		GL.UseProgram(v.programHandle);
	}
	
	public static void UnbindShader() {
		if (boundShader == Shader.empty) return;
		Log.Minimal($"unbound shader {boundShader.programHandle.Handle}", LogForm.rendererCore);
		boundShader = Shader.empty;
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