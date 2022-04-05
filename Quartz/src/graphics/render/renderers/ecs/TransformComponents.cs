using MathStuff.vectors;
using OpenTK.Mathematics;
using Quartz.objects.ecs.components;

namespace Quartz.graphics.render.renderers.ecs; 

public record struct MatrixComponent(Matrix4 value) : IComponent { public Matrix4 value = value; }
public record struct PositionComponent(float3 value) : IComponent { public float3 value = value; }
public record struct ScaleComponent(float3 value) : IComponent { public float3 value = value; }
public record struct Rotation2DComponent(float value) : IComponent { public float value = value; }
public record struct Rotation3DComponent(float3 value) : IComponent { public float3 value = value; }