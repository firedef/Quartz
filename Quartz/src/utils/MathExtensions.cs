using MathStuff;
using MathStuff.vectors;
using OpenTK.Mathematics;

namespace Quartz.utils; 

public static class MathExtensions {
	public static float3 Position(this Matrix4 mat) => new(mat.Row3.X, mat.Row3.Y, mat.Row3.Z);
	public static float3 Scale(this Matrix4 mat) => new(mat.Row0.Xyz.Length, mat.Row1.Xyz.Length, mat.Row2.Xyz.Length);

	public static Matrix4 SetPosition(this Matrix4 mat, float3 v) {
		mat.Row3.X = v.x;
		mat.Row3.Y = v.y;
		mat.Row3.Z = v.z;
		return mat;
	}
	
	public static Matrix4 SetScale(this Matrix4 mat, float3 v) {
		mat.Row0.Xyz = mat.Row0.Xyz.Normalized() * new Vector3(v.x);
		mat.Row1.Xyz = mat.Row1.Xyz.Normalized() * new Vector3(v.y);
		mat.Row2.Xyz = mat.Row2.Xyz.Normalized() * new Vector3(v.z);
		return mat;
	}
	
	public static Matrix4 SetRotationZ(this Matrix4 mat, float v) {
		float sin = MathF.Sin(v);
		float cos = MathF.Cos(v);
		mat.Row0.X = mat.Row0.Xyz.Length * sin;
		mat.Row0.Y = cos;
		mat.Row1.X = -cos;
		mat.Row1.Y = mat.Row1.Xyz.Length * sin;
		return mat;
	}
}