using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.filters;


public readonly struct All<T1> : IEcsFilter
    where T1 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 1) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        return true;
    }
}

public readonly struct All<T1, T2> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 2) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 3) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        if (!archetype.ContainsComponent(_t3)) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 4) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        if (!archetype.ContainsComponent(_t3)) return false;
        if (!archetype.ContainsComponent(_t4)) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 5) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        if (!archetype.ContainsComponent(_t3)) return false;
        if (!archetype.ContainsComponent(_t4)) return false;
        if (!archetype.ContainsComponent(_t5)) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 6) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        if (!archetype.ContainsComponent(_t3)) return false;
        if (!archetype.ContainsComponent(_t4)) return false;
        if (!archetype.ContainsComponent(_t5)) return false;
        if (!archetype.ContainsComponent(_t6)) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData
    where T7 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();
	private static readonly ComponentType _t7 = typeof(T7).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 7) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        if (!archetype.ContainsComponent(_t3)) return false;
        if (!archetype.ContainsComponent(_t4)) return false;
        if (!archetype.ContainsComponent(_t5)) return false;
        if (!archetype.ContainsComponent(_t6)) return false;
        if (!archetype.ContainsComponent(_t7)) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData
    where T7 : unmanaged, IEcsData
    where T8 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();
	private static readonly ComponentType _t7 = typeof(T7).Get();
	private static readonly ComponentType _t8 = typeof(T8).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.componentCount < 8) return false;
        if (!archetype.ContainsComponent(_t1)) return false;
        if (!archetype.ContainsComponent(_t2)) return false;
        if (!archetype.ContainsComponent(_t3)) return false;
        if (!archetype.ContainsComponent(_t4)) return false;
        if (!archetype.ContainsComponent(_t5)) return false;
        if (!archetype.ContainsComponent(_t6)) return false;
        if (!archetype.ContainsComponent(_t7)) return false;
        if (!archetype.ContainsComponent(_t8)) return false;
        return true;
    }
}

public readonly struct Any<T1> : IEcsFilter
    where T1 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        if (archetype.ContainsComponent(_t3)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        if (archetype.ContainsComponent(_t3)) return true;
        if (archetype.ContainsComponent(_t4)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        if (archetype.ContainsComponent(_t3)) return true;
        if (archetype.ContainsComponent(_t4)) return true;
        if (archetype.ContainsComponent(_t5)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        if (archetype.ContainsComponent(_t3)) return true;
        if (archetype.ContainsComponent(_t4)) return true;
        if (archetype.ContainsComponent(_t5)) return true;
        if (archetype.ContainsComponent(_t6)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData
    where T7 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();
	private static readonly ComponentType _t7 = typeof(T7).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        if (archetype.ContainsComponent(_t3)) return true;
        if (archetype.ContainsComponent(_t4)) return true;
        if (archetype.ContainsComponent(_t5)) return true;
        if (archetype.ContainsComponent(_t6)) return true;
        if (archetype.ContainsComponent(_t7)) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData
    where T7 : unmanaged, IEcsData
    where T8 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();
	private static readonly ComponentType _t7 = typeof(T7).Get();
	private static readonly ComponentType _t8 = typeof(T8).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return true;
        if (archetype.ContainsComponent(_t2)) return true;
        if (archetype.ContainsComponent(_t3)) return true;
        if (archetype.ContainsComponent(_t4)) return true;
        if (archetype.ContainsComponent(_t5)) return true;
        if (archetype.ContainsComponent(_t6)) return true;
        if (archetype.ContainsComponent(_t7)) return true;
        if (archetype.ContainsComponent(_t8)) return true;
        return false;
    }
}

public readonly struct None<T1> : IEcsFilter
    where T1 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        return true;
    }
}

public readonly struct None<T1, T2> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        if (archetype.ContainsComponent(_t3)) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        if (archetype.ContainsComponent(_t3)) return false;
        if (archetype.ContainsComponent(_t4)) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        if (archetype.ContainsComponent(_t3)) return false;
        if (archetype.ContainsComponent(_t4)) return false;
        if (archetype.ContainsComponent(_t5)) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        if (archetype.ContainsComponent(_t3)) return false;
        if (archetype.ContainsComponent(_t4)) return false;
        if (archetype.ContainsComponent(_t5)) return false;
        if (archetype.ContainsComponent(_t6)) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData
    where T7 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();
	private static readonly ComponentType _t7 = typeof(T7).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        if (archetype.ContainsComponent(_t3)) return false;
        if (archetype.ContainsComponent(_t4)) return false;
        if (archetype.ContainsComponent(_t5)) return false;
        if (archetype.ContainsComponent(_t6)) return false;
        if (archetype.ContainsComponent(_t7)) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : unmanaged, IEcsData
    where T2 : unmanaged, IEcsData
    where T3 : unmanaged, IEcsData
    where T4 : unmanaged, IEcsData
    where T5 : unmanaged, IEcsData
    where T6 : unmanaged, IEcsData
    where T7 : unmanaged, IEcsData
    where T8 : unmanaged, IEcsData 
{
    private static readonly ComponentType _t1 = typeof(T1).Get();
	private static readonly ComponentType _t2 = typeof(T2).Get();
	private static readonly ComponentType _t3 = typeof(T3).Get();
	private static readonly ComponentType _t4 = typeof(T4).Get();
	private static readonly ComponentType _t5 = typeof(T5).Get();
	private static readonly ComponentType _t6 = typeof(T6).Get();
	private static readonly ComponentType _t7 = typeof(T7).Get();
	private static readonly ComponentType _t8 = typeof(T8).Get();

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(_t1)) return false;
        if (archetype.ContainsComponent(_t2)) return false;
        if (archetype.ContainsComponent(_t3)) return false;
        if (archetype.ContainsComponent(_t4)) return false;
        if (archetype.ContainsComponent(_t5)) return false;
        if (archetype.ContainsComponent(_t6)) return false;
        if (archetype.ContainsComponent(_t7)) return false;
        if (archetype.ContainsComponent(_t8)) return false;
        return true;
    }
}

public readonly struct And<T1, T2> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3, T4> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype) && new T4().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype) && new T4().Filter(archetype) && new T5().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new()
    where T6 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype) && new T4().Filter(archetype) && new T5().Filter(archetype) && new T6().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new()
    where T6 : IEcsFilter, new()
    where T7 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype) && new T4().Filter(archetype) && new T5().Filter(archetype) && new T6().Filter(archetype) && new T7().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new()
    where T6 : IEcsFilter, new()
    where T7 : IEcsFilter, new()
    where T8 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype) && new T4().Filter(archetype) && new T5().Filter(archetype) && new T6().Filter(archetype) && new T7().Filter(archetype) && new T8().Filter(archetype);
    }
}

public readonly struct Or<T1, T2> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3, T4> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype) || new T4().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype) || new T4().Filter(archetype) || new T5().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new()
    where T6 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype) || new T4().Filter(archetype) || new T5().Filter(archetype) || new T6().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new()
    where T6 : IEcsFilter, new()
    where T7 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype) || new T4().Filter(archetype) || new T5().Filter(archetype) || new T6().Filter(archetype) || new T7().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new()
    where T4 : IEcsFilter, new()
    where T5 : IEcsFilter, new()
    where T6 : IEcsFilter, new()
    where T7 : IEcsFilter, new()
    where T8 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype) || new T4().Filter(archetype) || new T5().Filter(archetype) || new T6().Filter(archetype) || new T7().Filter(archetype) || new T8().Filter(archetype);
    }
}

public readonly struct Not<T1> : IEcsFilter
    where T1 : IEcsFilter, new() 
{
    public bool Filter(Archetype archetype) { 
        return !new T1().Filter(archetype);
    }
}
