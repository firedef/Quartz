using MathStuff;
using MathStuff.vectors;
using Quartz.collections;

namespace Quartz.geometry.mesh; 

public abstract class MeshBase : IDisposable {
	public HashedList<Vertex> vertices;
	public HashedList<ushort> indices;
	public rect bounds;

	public MeshBase(HashedList<Vertex> vertices, HashedList<ushort> indices) {
		this.vertices = vertices;
		this.indices = indices;
	}
	
	public unsafe MeshBase(Vertex* vertices, ushort* indices, int vCount, int iCount) {
		this.vertices = new(vertices, vCount);
		this.indices = new(indices, iCount);
	}
	
	public unsafe MeshBase(Vertex[] vertices, ushort[] indices) {
		fixed(Vertex* vPtr = vertices) this.vertices = new(vPtr, vertices.Length);
		fixed(ushort* iPtr = indices) this.indices = new(iPtr, indices.Length);
	}

#region construction
	
	public unsafe void SetVertices(Vertex* v, int s) => vertices.CopyFrom(v, s);
	public unsafe void SetVertices(Vertex[] v) { fixed(Vertex* vPtr = v) SetVertices(vPtr, v.Length); }

	public unsafe void SetIndices(ushort* v, int s) => indices.CopyFrom(v, s);
	public unsafe void SetIndices(ushort[] v) { fixed(ushort* vPtr = v) SetIndices(vPtr, v.Length); }

	public void AddVertex(Vertex v) => vertices.Add(v);
	public void AddIndex(int v) => indices.Add((ushort) v);
	
	/// <summary>add quadrilateral to mesh (vertices and indices) <br/><br/>using front normal and 0-1 uv coordinates</summary>
	public void AddRect(float3 p0, float3 p1, float3 p2, float3 p3, color c0) => AddRect(
		new(p0, c0, new(0,0)),
		new(p1, c0, new(0,1)),
		new(p2, c0, new(1,1)),
		new(p3, c0, new(1,0))
	);
	
	/// <summary>add quadrilateral to mesh (vertices and indices) <br/><br/>using front normal</summary>
	public void AddRect(float3 p0, float3 p1, float3 p2, float3 p3, color c0, rect uvs) => AddRect(
		new(p0, c0, uvs.leftBottom),
		new(p1, c0, uvs.leftTop),
		new(p2, c0, uvs.rightTop),
		new(p3, c0, uvs.rightBottom)
	);

	/// <summary>add quadrilateral to mesh (vertices and indices)</summary>
	public void AddRect(Vertex p0, Vertex p1, Vertex p2, Vertex p3) {
		vertices.EnsureCapacity(4);
		AddVertex(p0);
		AddVertex(p1);
		AddVertex(p2);
		AddVertex(p3);
		AddQuadIndices();
	}

	public void AddQuadIndices() {
		indices.EnsureCapacity(6);
		int vCount = vertices.count - 4;
		AddIndex(vCount + 0);
		AddIndex(vCount + 1);
		AddIndex(vCount + 2);
		AddIndex(vCount + 0);
		AddIndex(vCount + 2);
		AddIndex(vCount + 3);
	}

#endregion construction

#region calculations
	
	/// <summary>recalculate normals of mesh</summary>
	public unsafe void RecalculateNormals() {
		int c = indices.count;

		for (int i = 0; i < c; i += 3) {
			ushort i1 = indices[i];
			ushort i2 = indices[(i + 1) % c];
			ushort i3 = indices[(i + 2) % c];

			float3 p0 = vertices[i1].position - vertices[i2].position;
			float3 p1 = vertices[i1].position - vertices[i3].position;
			float3 normal = float3.Cross(p0, p1).normalized;

			vertices.dataPtr[i1].normal = normal;
			vertices.dataPtr[i2].normal = normal;
			vertices.dataPtr[i3].normal = normal;
		}
	}

	/// <summary>recalculate AABB of mesh <br/><br/>does not take into account object transform (rotation, translation and scale)</summary>
	public void RecalculateBounds() {
		int c = vertices.count;
		float2 min =  float2.maxValue;
		float2 max = -float2.maxValue;

		for (int i = 0; i < c; i++) {
			float2 p = vertices[i].position;
			if (p.x < min.x) min.x = p.x;
			if (p.y < min.y) min.y = p.y;
			if (p.x > max.x) max.x = p.x;
			if (p.y > max.y) max.y = p.y;
		}

		bounds = new(min.x, min.y, max.x - min.x, max.y - min.y);
	}

#endregion calculations
	
#region cleanup
	
	public virtual void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	public void Clear() {
		vertices.Clear();
		indices.Clear();
	}

	protected virtual void Dispose(bool disposing) {
		if (!disposing) return;
		vertices.Dispose();
		indices.Dispose();
	}
	
	~MeshBase() => Dispose(false);

#endregion cleanup
	
#region other
	
	//public bool IsVisible(rect camera) => Geometry.Intersects(bounds, camera, float2.zero);
	//public bool IsVisible(rect camera, Transform objTransform) => enabled && Geometry.Intersects(new(bounds.left, bounds.bottom, bounds.width * objTransform.scale.x, bounds.height * objTransform.scale.y), camera, objTransform.position);

	/// <summary>mark mesh as modified <br/><br/>
	/// it will recalculate bounds and (if GlMesh) update it on gpu</summary>
	public virtual void OnModified() => RecalculateBounds();

#endregion other
}