using MathStuff;
using Quartz.Ecs.ecs.delegates;
using Quartz.Ecs.ecs.views;
using Quartz.Ecs.exceptions;
// ReSharper disable CognitiveComplexity

namespace Quartz.Ecs.ecs.components.data;

public partial class ArchetypeComponents {
    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0>(ComponentDelegateS1<T0> a, int i0, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            
            ushort* ptr0 = _shared.data[i0].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0>(ComponentPredicateS1<T0> a, int i0, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, ISharedComponent {
        lock (_lock) {
            
            ushort* ptr0 = _shared.data[i0].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0> GetViewS1<T0>() 
		where T0 : unmanaged, ISharedComponent 
        => new(_owner, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T0).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1>(ComponentDelegateS2<T0, T1> a, int i0, int i1, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, ISharedComponent
		where T1 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            
            ushort* ptr0 = _shared.data[i0].data;
			ushort* ptr1 = _shared.data[i1].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1>(ComponentPredicateS2<T0, T1> a, int i0, int i1, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, ISharedComponent
		where T1 : unmanaged, ISharedComponent {
        lock (_lock) {
            
            ushort* ptr0 = _shared.data[i0].data;
			ushort* ptr1 = _shared.data[i1].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1> GetViewS2<T0, T1>() 
		where T0 : unmanaged, ISharedComponent
		where T1 : unmanaged, ISharedComponent 
        => new(_owner, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T0).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T1).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void Foreach<T0>(ComponentDelegate<T0> a, int i0, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
            

            for (int i = skip; i < count; i++) 
                a(ptr0 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollection<T0>(ComponentPredicate<T0> a, int i0, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
            

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsView<T0> GetView<T0>() 
		where T0 : unmanaged, IComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0, T1>(ComponentDelegateS1<T0, T1> a, int i0, int i1, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
            ushort* ptr1 = _shared.data[i1].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0, T1>(ComponentPredicateS1<T0, T1> a, int i0, int i1, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
            ushort* ptr1 = _shared.data[i1].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0, T1> GetViewS1<T0, T1>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T1).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1, T2>(ComponentDelegateS2<T0, T1, T2> a, int i0, int i1, int i2, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent
		where T2 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
            ushort* ptr1 = _shared.data[i1].data;
			ushort* ptr2 = _shared.data[i2].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1, T2>(ComponentPredicateS2<T0, T1, T2> a, int i0, int i1, int i2, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent
		where T2 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
            ushort* ptr1 = _shared.data[i1].data;
			ushort* ptr2 = _shared.data[i2].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1, T2> GetViewS2<T0, T1, T2>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent
		where T2 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T1).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T2).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void Foreach<T0, T1>(ComponentDelegate<T0, T1> a, int i0, int i1, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
            

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollection<T0, T1>(ComponentPredicate<T0, T1> a, int i0, int i1, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
            

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsView<T0, T1> GetView<T0, T1>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0, T1, T2>(ComponentDelegateS1<T0, T1, T2> a, int i0, int i1, int i2, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
            ushort* ptr2 = _shared.data[i2].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0, T1, T2>(ComponentPredicateS1<T0, T1, T2> a, int i0, int i1, int i2, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
            ushort* ptr2 = _shared.data[i2].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0, T1, T2> GetViewS1<T0, T1, T2>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T2).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1, T2, T3>(ComponentDelegateS2<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent
		where T3 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
            ushort* ptr2 = _shared.data[i2].data;
			ushort* ptr3 = _shared.data[i3].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1, T2, T3>(ComponentPredicateS2<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent
		where T3 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
            ushort* ptr2 = _shared.data[i2].data;
			ushort* ptr3 = _shared.data[i3].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1, T2, T3> GetViewS2<T0, T1, T2, T3>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent
		where T3 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T2).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T3).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void Foreach<T0, T1, T2>(ComponentDelegate<T0, T1, T2> a, int i0, int i1, int i2, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
            

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollection<T0, T1, T2>(ComponentPredicate<T0, T1, T2> a, int i0, int i1, int i2, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
            

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsView<T0, T1, T2> GetView<T0, T1, T2>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0, T1, T2, T3>(ComponentDelegateS1<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
            ushort* ptr3 = _shared.data[i3].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0, T1, T2, T3>(ComponentPredicateS1<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
            ushort* ptr3 = _shared.data[i3].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0, T1, T2, T3> GetViewS1<T0, T1, T2, T3>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T3).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1, T2, T3, T4>(ComponentDelegateS2<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent
		where T4 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
            ushort* ptr3 = _shared.data[i3].data;
			ushort* ptr4 = _shared.data[i4].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1, T2, T3, T4>(ComponentPredicateS2<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent
		where T4 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
            ushort* ptr3 = _shared.data[i3].data;
			ushort* ptr4 = _shared.data[i4].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1, T2, T3, T4> GetViewS2<T0, T1, T2, T3, T4>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent
		where T4 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T3).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T4).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void Foreach<T0, T1, T2, T3>(ComponentDelegate<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
            

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollection<T0, T1, T2, T3>(ComponentPredicate<T0, T1, T2, T3> a, int i0, int i1, int i2, int i3, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
            

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsView<T0, T1, T2, T3> GetView<T0, T1, T2, T3>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0, T1, T2, T3, T4>(ComponentDelegateS1<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
            ushort* ptr4 = _shared.data[i4].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0, T1, T2, T3, T4>(ComponentPredicateS1<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
            ushort* ptr4 = _shared.data[i4].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0, T1, T2, T3, T4> GetViewS1<T0, T1, T2, T3, T4>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T4).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1, T2, T3, T4, T5>(ComponentDelegateS2<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent
		where T5 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
            ushort* ptr4 = _shared.data[i4].data;
			ushort* ptr5 = _shared.data[i5].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1, T2, T3, T4, T5>(ComponentPredicateS2<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent
		where T5 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
            ushort* ptr4 = _shared.data[i4].data;
			ushort* ptr5 = _shared.data[i5].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1, T2, T3, T4, T5> GetViewS2<T0, T1, T2, T3, T4, T5>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent
		where T5 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T4).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T5).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void Foreach<T0, T1, T2, T3, T4>(ComponentDelegate<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
            

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollection<T0, T1, T2, T3, T4>(ComponentPredicate<T0, T1, T2, T3, T4> a, int i0, int i1, int i2, int i3, int i4, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
            

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsView<T0, T1, T2, T3, T4> GetView<T0, T1, T2, T3, T4>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (T4*) _normal.data[IndexOfNormalComponent(typeof(T4).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0, T1, T2, T3, T4, T5>(ComponentDelegateS1<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
            ushort* ptr5 = _shared.data[i5].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0, T1, T2, T3, T4, T5>(ComponentPredicateS1<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
            ushort* ptr5 = _shared.data[i5].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0, T1, T2, T3, T4, T5> GetViewS1<T0, T1, T2, T3, T4, T5>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (T4*) _normal.data[IndexOfNormalComponent(typeof(T4).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T5).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1, T2, T3, T4, T5, T6>(ComponentDelegateS2<T0, T1, T2, T3, T4, T5, T6> a, int i0, int i1, int i2, int i3, int i4, int i5, int i6, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent
		where T6 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
            ushort* ptr5 = _shared.data[i5].data;
			ushort* ptr6 = _shared.data[i6].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1, T2, T3, T4, T5, T6>(ComponentPredicateS2<T0, T1, T2, T3, T4, T5, T6> a, int i0, int i1, int i2, int i3, int i4, int i5, int i6, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent
		where T6 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
            ushort* ptr5 = _shared.data[i5].data;
			ushort* ptr6 = _shared.data[i6].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6> GetViewS2<T0, T1, T2, T3, T4, T5, T6>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent
		where T6 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (T4*) _normal.data[IndexOfNormalComponent(typeof(T4).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T5).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T6).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void Foreach<T0, T1, T2, T3, T4, T5>(ComponentDelegate<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
			T5* ptr5 = (T5*) _normal.data[i5].rawData;
            

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollection<T0, T1, T2, T3, T4, T5>(ComponentPredicate<T0, T1, T2, T3, T4, T5> a, int i0, int i1, int i2, int i3, int i4, int i5, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
			T5* ptr5 = (T5*) _normal.data[i5].rawData;
            

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsView<T0, T1, T2, T3, T4, T5> GetView<T0, T1, T2, T3, T4, T5>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (T4*) _normal.data[IndexOfNormalComponent(typeof(T4).Get())].rawData, (T5*) _normal.data[IndexOfNormalComponent(typeof(T5).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS1<T0, T1, T2, T3, T4, T5, T6>(ComponentDelegateS1<T0, T1, T2, T3, T4, T5, T6> a, int i0, int i1, int i2, int i3, int i4, int i5, int i6, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
			T5* ptr5 = (T5*) _normal.data[i5].rawData;
            ushort* ptr6 = _shared.data[i6].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS1<T0, T1, T2, T3, T4, T5, T6>(ComponentPredicateS1<T0, T1, T2, T3, T4, T5, T6> a, int i0, int i1, int i2, int i3, int i4, int i5, int i6, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
			T5* ptr5 = (T5*) _normal.data[i5].rawData;
            ushort* ptr6 = _shared.data[i6].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS1<T0, T1, T2, T3, T4, T5, T6> GetViewS1<T0, T1, T2, T3, T4, T5, T6>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (T4*) _normal.data[IndexOfNormalComponent(typeof(T4).Get())].rawData, (T5*) _normal.data[IndexOfNormalComponent(typeof(T5).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T6).Get())].rawData, elementCount);

    /// <summary> invoke delegate for every component </summary>
    public unsafe void ForeachS2<T0, T1, T2, T3, T4, T5, T6, T7>(ComponentDelegateS2<T0, T1, T2, T3, T4, T5, T6, T7> a, int i0, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int skip = 0, int skipEnd = 0, int take = int.MaxValue) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent
		where T7 : unmanaged, ISharedComponent {
        lock (_lock) {
            int startCount = elementCount;
            int count = math.min(startCount - skipEnd, take + skip);
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
			T5* ptr5 = (T5*) _normal.data[i5].rawData;
            ushort* ptr6 = _shared.data[i6].data;
			ushort* ptr7 = _shared.data[i7].data;

            for (int i = skip; i < count; i++) 
                a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i, ptr7 + i);
            
            if (elementCount != startCount) throw new CollectionModifiedException();
        }
    }

    /// <summary> invoke delegate for every component with ability to modify collections (add/remove) <br/><br/>if predicate returns false, element will be removed from collection, otherwise continue execution </summary>
    public unsafe void ModifyCollectionS2<T0, T1, T2, T3, T4, T5, T6, T7>(ComponentPredicateS2<T0, T1, T2, T3, T4, T5, T6, T7> a, int i0, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int skip = 0, int skipEnd = 0, int take = int.MaxValue, bool invokeForNew = true) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent
		where T7 : unmanaged, ISharedComponent {
        lock (_lock) {
            T0* ptr0 = (T0*) _normal.data[i0].rawData;
			T1* ptr1 = (T1*) _normal.data[i1].rawData;
			T2* ptr2 = (T2*) _normal.data[i2].rawData;
			T3* ptr3 = (T3*) _normal.data[i3].rawData;
			T4* ptr4 = (T4*) _normal.data[i4].rawData;
			T5* ptr5 = (T5*) _normal.data[i5].rawData;
            ushort* ptr6 = _shared.data[i6].data;
			ushort* ptr7 = _shared.data[i7].data;

            if (invokeForNew)
                for (int i = skip; i < math.min(elementCount - skipEnd, take + skip);) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i, ptr7 + i)) i++;
                    else RemoveComponents(i);
            else {
                int c = math.min(elementCount - skipEnd, take + skip);
                for (int i = skip; i < c;) 
                    if (a(ptr0 + i, ptr1 + i, ptr2 + i, ptr3 + i, ptr4 + i, ptr5 + i, ptr6 + i, ptr7 + i)) i++;
                    else { RemoveComponents(i); c--; }
            }
        }
    }

    public unsafe ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6, T7> GetViewS2<T0, T1, T2, T3, T4, T5, T6, T7>() 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent
		where T7 : unmanaged, ISharedComponent 
        => new(_owner, (T0*) _normal.data[IndexOfNormalComponent(typeof(T0).Get())].rawData, (T1*) _normal.data[IndexOfNormalComponent(typeof(T1).Get())].rawData, (T2*) _normal.data[IndexOfNormalComponent(typeof(T2).Get())].rawData, (T3*) _normal.data[IndexOfNormalComponent(typeof(T3).Get())].rawData, (T4*) _normal.data[IndexOfNormalComponent(typeof(T4).Get())].rawData, (T5*) _normal.data[IndexOfNormalComponent(typeof(T5).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T6).Get())].rawData, (ushort*) _shared.data[IndexOfSharedComponent(typeof(T7).Get())].rawData, elementCount);

}