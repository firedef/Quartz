namespace Quartz.objects.entities; 

[Flags]
public enum EntityFlags {
	isAlive = 1 << 0,
	isActive = 1 << 1,
	isRendererActive = 1 << 2,
	isDestructionStarted = 1 << 3,
}

public static class EntityFlagsExtensions {
	public static bool HasFlagFast(this EntityFlags value, EntityFlags flag) => (value & flag) != 0;
	public static EntityFlags SetFlagFast(this EntityFlags value, EntityFlags flag, bool v) {
		value &= ~flag;
		if (v) value |= flag;
		return flag;
	}
}