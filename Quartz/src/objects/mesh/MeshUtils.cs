using MathStuff;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.collections;
using Quartz.CoreCs.collections;

namespace Quartz.objects.mesh; 

public static class MeshUtils {
	public static void UpdateBuffer<T>(HashedList<T> buffer, BufferTargetARB target, BufferUsageARB usage, bool forceFullUpdate) where T : unmanaged {
		if (forceFullUpdate || buffer.GetCountChange()) {
			BufferData(buffer, target, usage, buffer.count);
			buffer.ResetChanges();
			return;
		}

		foreach (Range range in buffer.GetChanges())
			BufferSubData(buffer, target, range);
	}

	public static void UpdateBuffer<T>(NativeList<T> buffer, BufferTargetARB target, BufferUsageARB usage, int newCount, int oldCount, bool forceFullUpdate = false) where T : unmanaged {
		if (newCount != oldCount) BufferData(buffer, target, usage, newCount);
		else BufferSubData(buffer, target, ..buffer.count);
	}
	
	public static void UpdateBufferPart<T>(NativeList<T> buffer, BufferTargetARB target, BufferUsageARB usage, int newCount, int oldCount, int updParts, ref int updIndex, bool forceFullUpdate = false) where T : unmanaged {
		if (newCount != oldCount) {
			BufferData(buffer, target, usage, newCount); 
			return;
		}
		int updCount = buffer.count / updParts;
		int start = updCount * updIndex;
		int end = math.min(newCount, updCount * (updIndex + 1));
		BufferSubData(buffer, target, start..end);
		updIndex = (updIndex + 1) % updParts;
	}
	
	public static unsafe void BufferData<T>(NativeList<T> buffer, BufferTargetARB target, BufferUsageARB usage, int c) where T : unmanaged => GL.BufferData(target, c * sizeof(T), buffer.ptr, usage);

	public static unsafe void BufferSubData<T>(NativeList<T> buffer, BufferTargetARB target, Range range) where T : unmanaged {
		int start = range.Start.Value;
		int len = range.End.Value - start;
		GL.BufferSubData(target, (IntPtr)(start * sizeof(T)), len * sizeof(T), buffer.ptr + start);
	}

	public static void ProcessVertexAttributes<T>() where T : IMeshVertex, new() => new T().ProcessVertexAttributes();
}