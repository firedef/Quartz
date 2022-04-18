using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs.views;

public unsafe class ComponentsViewS1<T0>
    where T0 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    
    public readonly ushort* component0;

    public T0* componentValue0 => World.GetSharedComponent<T0>(*component0);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, ushort* component0, int count) {
        this.archetype = archetype;
        this.component0 = component0;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1>
    where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    
    public readonly ushort* component0;
	public readonly ushort* component1;

    public T0* componentValue0 => World.GetSharedComponent<T0>(*component0);
	public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, ushort* component0, ushort* component1, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2>
    where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    
    public readonly ushort* component0;
	public readonly ushort* component1;
	public readonly ushort* component2;

    public T0* componentValue0 => World.GetSharedComponent<T0>(*component0);
	public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);
	public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, ushort* component0, ushort* component1, ushort* component2, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3>
    where T0 : unmanaged, ISharedComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    
    public readonly ushort* component0;
	public readonly ushort* component1;
	public readonly ushort* component2;
	public readonly ushort* component3;

    public T0* componentValue0 => World.GetSharedComponent<T0>(*component0);
	public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);
	public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);
	public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, ushort* component0, ushort* component1, ushort* component2, ushort* component3, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0>
    where T0 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, int count) {
        this.archetype = archetype;
        this.component0 = component0;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
    public readonly ushort* component1;

    public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, ushort* component1, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
    public readonly ushort* component1;
	public readonly ushort* component2;

    public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);
	public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, ushort* component1, ushort* component2, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
    public readonly ushort* component1;
	public readonly ushort* component2;
	public readonly ushort* component3;

    public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);
	public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);
	public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, ushort* component1, ushort* component2, ushort* component3, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, ISharedComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
    public readonly ushort* component1;
	public readonly ushort* component2;
	public readonly ushort* component3;
	public readonly ushort* component4;

    public T1* componentValue1 => World.GetSharedComponent<T1>(*component1);
	public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);
	public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);
	public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, ushort* component1, ushort* component2, ushort* component3, ushort* component4, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
    public readonly ushort* component2;

    public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, ushort* component2, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
    public readonly ushort* component2;
	public readonly ushort* component3;

    public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);
	public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, ushort* component2, ushort* component3, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
    public readonly ushort* component2;
	public readonly ushort* component3;
	public readonly ushort* component4;

    public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);
	public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);
	public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, ushort* component2, ushort* component3, ushort* component4, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, ISharedComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
    public readonly ushort* component2;
	public readonly ushort* component3;
	public readonly ushort* component4;
	public readonly ushort* component5;

    public T2* componentValue2 => World.GetSharedComponent<T2>(*component2);
	public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);
	public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);
	public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, ushort* component2, ushort* component3, ushort* component4, ushort* component5, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1, T2>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, T2* component2, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2, T3>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
    public readonly ushort* component3;

    public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, T2* component2, ushort* component3, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3, T4>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
    public readonly ushort* component3;
	public readonly ushort* component4;

    public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);
	public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, T2* component2, ushort* component3, ushort* component4, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4, T5>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
    public readonly ushort* component3;
	public readonly ushort* component4;
	public readonly ushort* component5;

    public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);
	public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);
	public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, T2* component2, ushort* component3, ushort* component4, ushort* component5, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5, T6>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, ISharedComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
    public readonly ushort* component3;
	public readonly ushort* component4;
	public readonly ushort* component5;
	public readonly ushort* component6;

    public T3* componentValue3 => World.GetSharedComponent<T3>(*component3);
	public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);
	public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);
	public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, T2* component2, ushort* component3, ushort* component4, ushort* component5, ushort* component6, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1, T2, T3>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2, T3, T4>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
    public readonly ushort* component4;

    public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, ushort* component4, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3, T4, T5>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
    public readonly ushort* component4;
	public readonly ushort* component5;

    public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);
	public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, ushort* component4, ushort* component5, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4, T5, T6>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
    public readonly ushort* component4;
	public readonly ushort* component5;
	public readonly ushort* component6;

    public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);
	public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);
	public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, ushort* component4, ushort* component5, ushort* component6, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, ISharedComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
    public readonly ushort* component4;
	public readonly ushort* component5;
	public readonly ushort* component6;
	public readonly ushort* component7;

    public T4* componentValue4 => World.GetSharedComponent<T4>(*component4);
	public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);
	public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);
	public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, ushort* component4, ushort* component5, ushort* component6, ushort* component7, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1, T2, T3, T4>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2, T3, T4, T5>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
    public readonly ushort* component5;

    public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, ushort* component5, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
    public readonly ushort* component5;
	public readonly ushort* component6;

    public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);
	public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, ushort* component5, ushort* component6, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
    public readonly ushort* component5;
	public readonly ushort* component6;
	public readonly ushort* component7;

    public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);
	public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);
	public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, ushort* component5, ushort* component6, ushort* component7, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, ISharedComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
    public readonly ushort* component5;
	public readonly ushort* component6;
	public readonly ushort* component7;
	public readonly ushort* component8;

    public T5* componentValue5 => World.GetSharedComponent<T5>(*component5);
	public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);
	public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);
	public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, ushort* component5, ushort* component6, ushort* component7, ushort* component8, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1, T2, T3, T4, T5>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2, T3, T4, T5, T6>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
    public readonly ushort* component6;

    public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, ushort* component6, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
    public readonly ushort* component6;
	public readonly ushort* component7;

    public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);
	public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, ushort* component6, ushort* component7, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
    public readonly ushort* component6;
	public readonly ushort* component7;
	public readonly ushort* component8;

    public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);
	public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);
	public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, ushort* component6, ushort* component7, ushort* component8, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, ISharedComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
    public readonly ushort* component6;
	public readonly ushort* component7;
	public readonly ushort* component8;
	public readonly ushort* component9;

    public T6* componentValue6 => World.GetSharedComponent<T6>(*component6);
	public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);
	public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);
	public T9* componentValue9 => World.GetSharedComponent<T9>(*component9);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, ushort* component6, ushort* component7, ushort* component8, ushort* component9, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
		this.component9 = component9;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1, T2, T3, T4, T5, T6>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
    public readonly ushort* component7;

    public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, ushort* component7, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
    public readonly ushort* component7;
	public readonly ushort* component8;

    public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);
	public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, ushort* component7, ushort* component8, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
    public readonly ushort* component7;
	public readonly ushort* component8;
	public readonly ushort* component9;

    public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);
	public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);
	public T9* componentValue9 => World.GetSharedComponent<T9>(*component9);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, ushort* component7, ushort* component8, ushort* component9, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
		this.component9 = component9;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, ISharedComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
    public readonly ushort* component7;
	public readonly ushort* component8;
	public readonly ushort* component9;
	public readonly ushort* component10;

    public T7* componentValue7 => World.GetSharedComponent<T7>(*component7);
	public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);
	public T9* componentValue9 => World.GetSharedComponent<T9>(*component9);
	public T10* componentValue10 => World.GetSharedComponent<T10>(*component10);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, ushort* component7, ushort* component8, ushort* component9, ushort* component10, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
		this.component9 = component9;
		this.component10 = component10;
        this.count = count;
    }
}

public unsafe class ComponentsView<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
	public readonly T7* component7;
    

    

    public readonly int count;

    public ComponentsView(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, T7* component7, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
        this.count = count;
    }
}

public unsafe class ComponentsViewS1<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
	public readonly T7* component7;
    public readonly ushort* component8;

    public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);

    public readonly int count;

    public ComponentsViewS1(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, T7* component7, ushort* component8, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
        this.count = count;
    }
}

public unsafe class ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
	public readonly T7* component7;
    public readonly ushort* component8;
	public readonly ushort* component9;

    public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);
	public T9* componentValue9 => World.GetSharedComponent<T9>(*component9);

    public readonly int count;

    public ComponentsViewS2(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, T7* component7, ushort* component8, ushort* component9, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
		this.component9 = component9;
        this.count = count;
    }
}

public unsafe class ComponentsViewS3<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
	public readonly T7* component7;
    public readonly ushort* component8;
	public readonly ushort* component9;
	public readonly ushort* component10;

    public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);
	public T9* componentValue9 => World.GetSharedComponent<T9>(*component9);
	public T10* componentValue10 => World.GetSharedComponent<T10>(*component10);

    public readonly int count;

    public ComponentsViewS3(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, T7* component7, ushort* component8, ushort* component9, ushort* component10, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
		this.component9 = component9;
		this.component10 = component10;
        this.count = count;
    }
}

public unsafe class ComponentsViewS4<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    where T0 : unmanaged, IComponent
	where T1 : unmanaged, IComponent
	where T2 : unmanaged, IComponent
	where T3 : unmanaged, IComponent
	where T4 : unmanaged, IComponent
	where T5 : unmanaged, IComponent
	where T6 : unmanaged, IComponent
	where T7 : unmanaged, IComponent
	where T8 : unmanaged, ISharedComponent
	where T9 : unmanaged, ISharedComponent
	where T10 : unmanaged, ISharedComponent
	where T11 : unmanaged, ISharedComponent {
    public readonly Archetype archetype;

    public readonly T0* component0;
	public readonly T1* component1;
	public readonly T2* component2;
	public readonly T3* component3;
	public readonly T4* component4;
	public readonly T5* component5;
	public readonly T6* component6;
	public readonly T7* component7;
    public readonly ushort* component8;
	public readonly ushort* component9;
	public readonly ushort* component10;
	public readonly ushort* component11;

    public T8* componentValue8 => World.GetSharedComponent<T8>(*component8);
	public T9* componentValue9 => World.GetSharedComponent<T9>(*component9);
	public T10* componentValue10 => World.GetSharedComponent<T10>(*component10);
	public T11* componentValue11 => World.GetSharedComponent<T11>(*component11);

    public readonly int count;

    public ComponentsViewS4(Archetype archetype, T0* component0, T1* component1, T2* component2, T3* component3, T4* component4, T5* component5, T6* component6, T7* component7, ushort* component8, ushort* component9, ushort* component10, ushort* component11, int count) {
        this.archetype = archetype;
        this.component0 = component0;
		this.component1 = component1;
		this.component2 = component2;
		this.component3 = component3;
		this.component4 = component4;
		this.component5 = component5;
		this.component6 = component6;
		this.component7 = component7;
		this.component8 = component8;
		this.component9 = component9;
		this.component10 = component10;
		this.component11 = component11;
        this.count = count;
    }
}

