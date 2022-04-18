namespace Quartz.CoreCs.other.events; 

[Flags]
public enum EventTypes : uint {
	none = 0,

	/// <summary>called every frame update, after render, by default - 60/sec</summary>
	frameUpdate = 1 << 0,

	/// <summary>called every fixed update, by default - 20/sec</summary>
	fixedUpdate = 1 << 1,

	/// <summary>called every rare update, by default - 1/sec</summary>
	rareUpdate = 1 << 2,

	/// <summary>called every frame, before frame update</summary>
	render = 1 << 3,

	/// <summary>called on assembly loading (app start)</summary>
	start = 1 << 4,

	/// <summary>called on assembly unloading (app close)</summary>
	quit = 1 << 5,

	/// <summary>called on low memory</summary>
	lowMemory = 1 << 6,
	
	imgui = 1 << 7,

	/// <summary>not implemented <br/><br/>called on scene loading</summary>
	sceneEnter = 1 << 8,

	/// <summary>not implemented <br/><br/>called on scene unloading or on application close</summary>
	sceneExit = 1 << 9,
	//reserved = 1 << 10,

	/// <summary>not implemented <br/><br/>called on game pause / resume</summary>
	gamePauseToggle = 1 << 11,
	//reserved = 1 << 12,
	//reserved = 1 << 13,
	//reserved = 1 << 14,

	/// <summary>not implemented</summary>
	custom = 1u << 31,
}