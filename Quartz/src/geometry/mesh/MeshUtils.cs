using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.collections;

namespace Quartz.geometry.mesh; 

public static class MeshUtils {
	public static void UpdateBuffer<T>(HashedList<T> buffer, BufferTargetARB target, BufferUsageARB usage, bool forceFullUpdate) where T : unmanaged {
		if (forceFullUpdate || buffer.GetCountChange()) {
			BufferData(buffer, target, usage);
			buffer.ResetChanges();
			return;
		}

		foreach (Range range in buffer.GetChanges())
			BufferSubData(buffer, target, range);
	}

	public static void UpdateBuffer<T>(NativeList<T> buffer, BufferTargetARB target, BufferUsageARB usage, int oldCount, bool forceFullUpdate = false) where T : unmanaged {
		if (buffer.count != oldCount) BufferData(buffer, target, usage);
		else BufferSubData(buffer, target, ..buffer.count);
	} 
	
	public static unsafe void BufferData<T>(NativeList<T> buffer, BufferTargetARB target, BufferUsageARB usage) where T : unmanaged => GL.BufferData(target, buffer.count * sizeof(T), buffer.dataPtr, usage);

	public static unsafe void BufferSubData<T>(NativeList<T> buffer, BufferTargetARB target, Range range) where T : unmanaged {
		int start = range.Start.Value;
		int len = range.End.Value - start;
		GL.BufferSubData(target, (IntPtr)(start * sizeof(T)), len * sizeof(T), buffer.dataPtr + start);
	}

	public static void ProcessVertexAttributes<T>() where T : IMeshVertex, new() => new T().ProcessVertexAttributes();
}