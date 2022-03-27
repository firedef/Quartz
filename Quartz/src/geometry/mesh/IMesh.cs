using OpenTK.Graphics.OpenGL.Compatibility;

namespace Quartz.geometry.mesh; 

public interface IMesh {
	public PrimitiveType getTopology { get; }

	public void Bind();
	public bool PrepareForRender();
}