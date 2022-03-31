namespace Quartz.objects.entities; 

public abstract class EntityBase {
	public long entityId;
	public EntityFlags flags;

	public bool isAlive { get => flags.HasFlagFast(EntityFlags.isAlive); protected set => flags.SetFlagFast(EntityFlags.isAlive, value); }
	public bool isActive { get => flags.HasFlagFast(EntityFlags.isActive); set => flags.SetFlagFast(EntityFlags.isActive, value); }
	public bool isRendererActive { get => flags.HasFlagFast(EntityFlags.isRendererActive); set => flags.SetFlagFast(EntityFlags.isRendererActive, value); }
	public bool isDestructionStarted { get => flags.HasFlagFast(EntityFlags.isDestructionStarted); protected set => flags.SetFlagFast(EntityFlags.isDestructionStarted, value); }

	/// <summary>called every frame (by default: 60/sec)</summary>
	public virtual void OnUpdate(float deltaTime) {}

	/// <summary>called every fixed frame (by default: 20/sec)</summary>
	public virtual void OnFixedUpdate(float deltaTime) {}

	/// <summary>called on instantiation, when active</summary>
	public virtual void OnStart() {}
	
	public abstract void Render();
}