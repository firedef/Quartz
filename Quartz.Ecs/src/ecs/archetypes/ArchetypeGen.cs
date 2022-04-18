using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.views;

namespace Quartz.Ecs.ecs.archetypes;

public partial class Archetype {
    /// <summary>
    /// get components view of 0 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">shared component #0</typeparam>
    public ComponentsViewS1<T0> ComponentsS1<T0>()
		where T0 : unmanaged, ISharedComponent
        => components.GetViewS1<T0>();

    /// <summary>
    /// get components view of 0 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">shared component #0</typeparam>
	/// <typeparam name="T1">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1> ComponentsS2<T0, T1>()
		where T0 : unmanaged, ISharedComponent
		where T1 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1>();

    /// <summary>
    /// get components view of 1 normal components and 0 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
    public ComponentsView<T0> Components<T0>()
		where T0 : unmanaged, IComponent
        => components.GetView<T0>();

    /// <summary>
    /// get components view of 1 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">shared component #0</typeparam>
    public ComponentsViewS1<T0, T1> ComponentsS1<T0, T1>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent
        => components.GetViewS1<T0, T1>();

    /// <summary>
    /// get components view of 1 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">shared component #0</typeparam>
	/// <typeparam name="T2">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1, T2> ComponentsS2<T0, T1, T2>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent
		where T2 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1, T2>();

    /// <summary>
    /// get components view of 2 normal components and 0 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
    public ComponentsView<T0, T1> Components<T0, T1>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
        => components.GetView<T0, T1>();

    /// <summary>
    /// get components view of 2 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">shared component #0</typeparam>
    public ComponentsViewS1<T0, T1, T2> ComponentsS1<T0, T1, T2>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent
        => components.GetViewS1<T0, T1, T2>();

    /// <summary>
    /// get components view of 2 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">shared component #0</typeparam>
	/// <typeparam name="T3">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1, T2, T3> ComponentsS2<T0, T1, T2, T3>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent
		where T3 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1, T2, T3>();

    /// <summary>
    /// get components view of 3 normal components and 0 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
    public ComponentsView<T0, T1, T2> Components<T0, T1, T2>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
        => components.GetView<T0, T1, T2>();

    /// <summary>
    /// get components view of 3 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">shared component #0</typeparam>
    public ComponentsViewS1<T0, T1, T2, T3> ComponentsS1<T0, T1, T2, T3>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent
        => components.GetViewS1<T0, T1, T2, T3>();

    /// <summary>
    /// get components view of 3 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">shared component #0</typeparam>
	/// <typeparam name="T4">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1, T2, T3, T4> ComponentsS2<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent
		where T4 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1, T2, T3, T4>();

    /// <summary>
    /// get components view of 4 normal components and 0 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
    public ComponentsView<T0, T1, T2, T3> Components<T0, T1, T2, T3>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
        => components.GetView<T0, T1, T2, T3>();

    /// <summary>
    /// get components view of 4 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">shared component #0</typeparam>
    public ComponentsViewS1<T0, T1, T2, T3, T4> ComponentsS1<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent
        => components.GetViewS1<T0, T1, T2, T3, T4>();

    /// <summary>
    /// get components view of 4 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">shared component #0</typeparam>
	/// <typeparam name="T5">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1, T2, T3, T4, T5> ComponentsS2<T0, T1, T2, T3, T4, T5>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent
		where T5 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1, T2, T3, T4, T5>();

    /// <summary>
    /// get components view of 5 normal components and 0 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">normal component #4</typeparam>
    public ComponentsView<T0, T1, T2, T3, T4> Components<T0, T1, T2, T3, T4>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
        => components.GetView<T0, T1, T2, T3, T4>();

    /// <summary>
    /// get components view of 5 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">normal component #4</typeparam>
	/// <typeparam name="T5">shared component #0</typeparam>
    public ComponentsViewS1<T0, T1, T2, T3, T4, T5> ComponentsS1<T0, T1, T2, T3, T4, T5>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent
        => components.GetViewS1<T0, T1, T2, T3, T4, T5>();

    /// <summary>
    /// get components view of 5 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">normal component #4</typeparam>
	/// <typeparam name="T5">shared component #0</typeparam>
	/// <typeparam name="T6">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6> ComponentsS2<T0, T1, T2, T3, T4, T5, T6>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, ISharedComponent
		where T6 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1, T2, T3, T4, T5, T6>();

    /// <summary>
    /// get components view of 6 normal components and 0 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">normal component #4</typeparam>
	/// <typeparam name="T5">normal component #5</typeparam>
    public ComponentsView<T0, T1, T2, T3, T4, T5> Components<T0, T1, T2, T3, T4, T5>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
        => components.GetView<T0, T1, T2, T3, T4, T5>();

    /// <summary>
    /// get components view of 6 normal components and 1 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">normal component #4</typeparam>
	/// <typeparam name="T5">normal component #5</typeparam>
	/// <typeparam name="T6">shared component #0</typeparam>
    public ComponentsViewS1<T0, T1, T2, T3, T4, T5, T6> ComponentsS1<T0, T1, T2, T3, T4, T5, T6>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent
        => components.GetViewS1<T0, T1, T2, T3, T4, T5, T6>();

    /// <summary>
    /// get components view of 6 normal components and 2 shared components
    /// </summary>
    /// <typeparam name="T0">normal component #0</typeparam>
	/// <typeparam name="T1">normal component #1</typeparam>
	/// <typeparam name="T2">normal component #2</typeparam>
	/// <typeparam name="T3">normal component #3</typeparam>
	/// <typeparam name="T4">normal component #4</typeparam>
	/// <typeparam name="T5">normal component #5</typeparam>
	/// <typeparam name="T6">shared component #0</typeparam>
	/// <typeparam name="T7">shared component #1</typeparam>
    public ComponentsViewS2<T0, T1, T2, T3, T4, T5, T6, T7> ComponentsS2<T0, T1, T2, T3, T4, T5, T6, T7>()
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, ISharedComponent
		where T7 : unmanaged, ISharedComponent
        => components.GetViewS2<T0, T1, T2, T3, T4, T5, T6, T7>();

}