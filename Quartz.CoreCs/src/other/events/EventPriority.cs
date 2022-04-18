namespace Quartz.CoreCs.other.events; 

public enum EventPriority : byte {
	minimal = 0,
	belowNormal = 50,
	normal = 100,
	aboveNormal = 150,
	important = 200,
	critical = 240,
	maximum = 255,
}