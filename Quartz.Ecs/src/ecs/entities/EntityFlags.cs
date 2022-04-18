namespace Quartz.Ecs.ecs.entities; 

[Flags]
public enum EntityFlags : byte {
	none = 0,
	isAlive = 1,
}