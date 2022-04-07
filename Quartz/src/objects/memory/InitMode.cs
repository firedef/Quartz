namespace Quartz.objects.memory; 

public enum InitMode : byte {
	/// <summary>dont initialize object; fastest variant</summary>
	uninitialized,

	/// <summary>set memory to 0</summary>
	zeroed,

	/// <summary>call constructor and copy to memory; slowest variant</summary>
	ctor,
}