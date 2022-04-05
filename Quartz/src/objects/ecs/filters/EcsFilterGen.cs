using Quartz.objects.ecs.components;
using Quartz.objects.ecs.archetypes;

namespace Quartz.objects.ecs.filters;

public readonly struct All<T1> : IEcsFilter
    where T1 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 1) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        return true;
    }
}

public readonly struct All<T1, T2> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 2) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 3) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        if (!archetype.ContainsComponent(typeof(T3))) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 4) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        if (!archetype.ContainsComponent(typeof(T3))) return false;
        if (!archetype.ContainsComponent(typeof(T4))) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 5) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        if (!archetype.ContainsComponent(typeof(T3))) return false;
        if (!archetype.ContainsComponent(typeof(T4))) return false;
        if (!archetype.ContainsComponent(typeof(T5))) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 6) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        if (!archetype.ContainsComponent(typeof(T3))) return false;
        if (!archetype.ContainsComponent(typeof(T4))) return false;
        if (!archetype.ContainsComponent(typeof(T5))) return false;
        if (!archetype.ContainsComponent(typeof(T6))) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent
    where T7 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 7) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        if (!archetype.ContainsComponent(typeof(T3))) return false;
        if (!archetype.ContainsComponent(typeof(T4))) return false;
        if (!archetype.ContainsComponent(typeof(T5))) return false;
        if (!archetype.ContainsComponent(typeof(T6))) return false;
        if (!archetype.ContainsComponent(typeof(T7))) return false;
        return true;
    }
}

public readonly struct All<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent
    where T7 : unmanaged, IComponent
    where T8 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 8) return false;
        if (!archetype.ContainsComponent(typeof(T1))) return false;
        if (!archetype.ContainsComponent(typeof(T2))) return false;
        if (!archetype.ContainsComponent(typeof(T3))) return false;
        if (!archetype.ContainsComponent(typeof(T4))) return false;
        if (!archetype.ContainsComponent(typeof(T5))) return false;
        if (!archetype.ContainsComponent(typeof(T6))) return false;
        if (!archetype.ContainsComponent(typeof(T7))) return false;
        if (!archetype.ContainsComponent(typeof(T8))) return false;
        return true;
    }
}

public readonly struct Any<T1> : IEcsFilter
    where T1 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        if (archetype.ContainsComponent(typeof(T3))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        if (archetype.ContainsComponent(typeof(T3))) return true;
        if (archetype.ContainsComponent(typeof(T4))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        if (archetype.ContainsComponent(typeof(T3))) return true;
        if (archetype.ContainsComponent(typeof(T4))) return true;
        if (archetype.ContainsComponent(typeof(T5))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        if (archetype.ContainsComponent(typeof(T3))) return true;
        if (archetype.ContainsComponent(typeof(T4))) return true;
        if (archetype.ContainsComponent(typeof(T5))) return true;
        if (archetype.ContainsComponent(typeof(T6))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent
    where T7 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        if (archetype.ContainsComponent(typeof(T3))) return true;
        if (archetype.ContainsComponent(typeof(T4))) return true;
        if (archetype.ContainsComponent(typeof(T5))) return true;
        if (archetype.ContainsComponent(typeof(T6))) return true;
        if (archetype.ContainsComponent(typeof(T7))) return true;
        return false;
    }
}

public readonly struct Any<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent
    where T7 : unmanaged, IComponent
    where T8 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)};

    public bool Filter(Archetype archetype) { 
        if (archetype.ContainsComponent(typeof(T1))) return true;
        if (archetype.ContainsComponent(typeof(T2))) return true;
        if (archetype.ContainsComponent(typeof(T3))) return true;
        if (archetype.ContainsComponent(typeof(T4))) return true;
        if (archetype.ContainsComponent(typeof(T5))) return true;
        if (archetype.ContainsComponent(typeof(T6))) return true;
        if (archetype.ContainsComponent(typeof(T7))) return true;
        if (archetype.ContainsComponent(typeof(T8))) return true;
        return false;
    }
}

public readonly struct None<T1> : IEcsFilter
    where T1 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 1) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        return true;
    }
}

public readonly struct None<T1, T2> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 2) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 3) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        if (archetype.ContainsComponent(typeof(T3))) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 4) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        if (archetype.ContainsComponent(typeof(T3))) return false;
        if (archetype.ContainsComponent(typeof(T4))) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 5) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        if (archetype.ContainsComponent(typeof(T3))) return false;
        if (archetype.ContainsComponent(typeof(T4))) return false;
        if (archetype.ContainsComponent(typeof(T5))) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5, T6> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 6) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        if (archetype.ContainsComponent(typeof(T3))) return false;
        if (archetype.ContainsComponent(typeof(T4))) return false;
        if (archetype.ContainsComponent(typeof(T5))) return false;
        if (archetype.ContainsComponent(typeof(T6))) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5, T6, T7> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent
    where T7 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 7) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        if (archetype.ContainsComponent(typeof(T3))) return false;
        if (archetype.ContainsComponent(typeof(T4))) return false;
        if (archetype.ContainsComponent(typeof(T5))) return false;
        if (archetype.ContainsComponent(typeof(T6))) return false;
        if (archetype.ContainsComponent(typeof(T7))) return false;
        return true;
    }
}

public readonly struct None<T1, T2, T3, T4, T5, T6, T7, T8> : IEcsFilter
    where T1 : unmanaged, IComponent
    where T2 : unmanaged, IComponent
    where T3 : unmanaged, IComponent
    where T4 : unmanaged, IComponent
    where T5 : unmanaged, IComponent
    where T6 : unmanaged, IComponent
    where T7 : unmanaged, IComponent
    where T8 : unmanaged, IComponent 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)};

    public bool Filter(Archetype archetype) { 
        if (archetype.componentTypes.Length < 8) return true;
        if (archetype.ContainsComponent(typeof(T1))) return false;
        if (archetype.ContainsComponent(typeof(T2))) return false;
        if (archetype.ContainsComponent(typeof(T3))) return false;
        if (archetype.ContainsComponent(typeof(T4))) return false;
        if (archetype.ContainsComponent(typeof(T5))) return false;
        if (archetype.ContainsComponent(typeof(T6))) return false;
        if (archetype.ContainsComponent(typeof(T7))) return false;
        if (archetype.ContainsComponent(typeof(T8))) return false;
        return true;
    }
}

public readonly struct And<T1, T2> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new() 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2)};

    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype);
    }
}

public readonly struct And<T1, T2, T3> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new() 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)};

    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) && new T2().Filter(archetype) && new T3().Filter(archetype) && new T4().Filter(archetype) && new T5().Filter(archetype) && new T6().Filter(archetype) && new T7().Filter(archetype) && new T8().Filter(archetype);
    }
}

public readonly struct Or<T1, T2> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new() 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2)};

    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype);
    }
}

public readonly struct Or<T1, T2, T3> : IEcsFilter
    where T1 : IEcsFilter, new()
    where T2 : IEcsFilter, new()
    where T3 : IEcsFilter, new() 
{
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)};

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
    public Type[] GetTypeArray() => new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)};

    public bool Filter(Archetype archetype) { 
        return new T1().Filter(archetype) || new T2().Filter(archetype) || new T3().Filter(archetype) || new T4().Filter(archetype) || new T5().Filter(archetype) || new T6().Filter(archetype) || new T7().Filter(archetype) || new T8().Filter(archetype);
    }
}

public readonly struct Not<T1> : IEcsFilter
    where T1 : IEcsFilter, new() 
{
    public Type[] GetTypeArray() => new[] {typeof(T1)};

    public bool Filter(Archetype archetype) { 
        return !new T1().Filter(archetype);
    }
}
