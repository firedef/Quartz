using System;
using System.Collections.Generic;
using Quartz.Ecs.ecs.attributes;
using Quartz.Ecs.ecs.components;

namespace Quartz.Tests.data.ecs;

public struct TestNormalComponentA : IComponent {
	public float a = -4.5f;
	public int b = 9;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

public struct TestNormalComponentB : IComponent {
	public bool a = true;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

[Require(typeof(TestNormalComponentA))]
public struct TestNormalComponentC : IComponent {
	public int a = -333;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

[Require(typeof(TestNormalComponentA))]
public struct TestSharedComponentD : ISharedComponent {
	public int a = -333;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

[Require(typeof(TestSharedComponentD))]
public struct TestNormalComponentE : IComponent {
	public int a = -333;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

[Require(typeof(TestNormalComponentE), typeof(TestNormalComponentA))]
public struct TestNormalComponentF : IComponent {
	public int a = -333;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

[Require(typeof(TestNormalComponentH))]
public struct TestNormalComponentG : IComponent {
	public int a = -333;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

[Require(typeof(TestNormalComponentG))]
public struct TestNormalComponentH : IComponent {
	public int a = -333;
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}

public struct TestDisposableComponent : IComponent, IDisposable {
	public int instances = 0;

	public TestDisposableComponent() => instances++;
	public void Dispose() {
		instances--;
		Console.WriteLine($"dispose: {instances}");
	}
	
	public void Parse(Dictionary<string, string> fields) { 
	
	}
	public void Write(Dictionary<string, string> fields) { 
	
	}
}