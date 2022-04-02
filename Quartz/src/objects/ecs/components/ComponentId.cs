namespace Quartz.objects.ecs.components; 

public readonly struct ComponentId {
	public readonly uint id;

	public bool isValid => id != uint.MaxValue;

	public ComponentId(uint id) => this.id = id;

	public static implicit operator uint(ComponentId v) => v.id;
	public static implicit operator ComponentId(uint v) => new(v);

	public override string ToString() => id.ToString();
}