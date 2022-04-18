using MathStuff;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.queries;
using Quartz.Ecs.ecs.views;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Ecs.ecs.jobs; 

public static partial class JobScheduler {
	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>0 normal components and 1 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS1<T0>(this Action<ComponentsViewS1<T0>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS1<T0> view = archetype.ComponentsS1<T0>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS1<T0> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			ushort* c0 = view.component0 + start;

			// create new view
			ComponentsViewS1<T0> slice = new(archetype, c0, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>0 normal components and 2 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS2<T0, T1>(this Action<ComponentsViewS2<T0, T1>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, ISharedComponent
		where T1 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS2<T0, T1> view = archetype.ComponentsS2<T0, T1>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS2<T0, T1> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			ushort* c0 = view.component0 + start;
			ushort* c1 = view.component1 + start;

			// create new view
			ComponentsViewS2<T0, T1> slice = new(archetype, c0, c1, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>1 normal components and 0 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void Schedule<T0>(this Action<ComponentsView<T0>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsView<T0> view = archetype.Components<T0>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsView<T0> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;

			// create new view
			ComponentsView<T0> slice = new(archetype, c0, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>1 normal components and 1 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS1<T0, T1>(this Action<ComponentsViewS1<T0, T1>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS1<T0, T1> view = archetype.ComponentsS1<T0, T1>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS1<T0, T1> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			ushort* c1 = view.component1 + start;

			// create new view
			ComponentsViewS1<T0, T1> slice = new(archetype, c0, c1, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>1 normal components and 2 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS2<T0, T1, T2>(this Action<ComponentsViewS2<T0, T1, T2>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, ISharedComponent
		where T2 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS2<T0, T1, T2> view = archetype.ComponentsS2<T0, T1, T2>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS2<T0, T1, T2> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			ushort* c1 = view.component1 + start;
			ushort* c2 = view.component2 + start;

			// create new view
			ComponentsViewS2<T0, T1, T2> slice = new(archetype, c0, c1, c2, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>2 normal components and 0 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void Schedule<T0, T1>(this Action<ComponentsView<T0, T1>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsView<T0, T1> view = archetype.Components<T0, T1>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsView<T0, T1> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;

			// create new view
			ComponentsView<T0, T1> slice = new(archetype, c0, c1, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>2 normal components and 1 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS1<T0, T1, T2>(this Action<ComponentsViewS1<T0, T1, T2>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS1<T0, T1, T2> view = archetype.ComponentsS1<T0, T1, T2>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS1<T0, T1, T2> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			ushort* c2 = view.component2 + start;

			// create new view
			ComponentsViewS1<T0, T1, T2> slice = new(archetype, c0, c1, c2, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>2 normal components and 2 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS2<T0, T1, T2, T3>(this Action<ComponentsViewS2<T0, T1, T2, T3>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, ISharedComponent
		where T3 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2, T3>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS2<T0, T1, T2, T3> view = archetype.ComponentsS2<T0, T1, T2, T3>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS2<T0, T1, T2, T3> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			ushort* c2 = view.component2 + start;
			ushort* c3 = view.component3 + start;

			// create new view
			ComponentsViewS2<T0, T1, T2, T3> slice = new(archetype, c0, c1, c2, c3, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>3 normal components and 0 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void Schedule<T0, T1, T2>(this Action<ComponentsView<T0, T1, T2>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsView<T0, T1, T2> view = archetype.Components<T0, T1, T2>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsView<T0, T1, T2> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			T2* c2 = view.component2 + start;

			// create new view
			ComponentsView<T0, T1, T2> slice = new(archetype, c0, c1, c2, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>3 normal components and 1 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS1<T0, T1, T2, T3>(this Action<ComponentsViewS1<T0, T1, T2, T3>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2, T3>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS1<T0, T1, T2, T3> view = archetype.ComponentsS1<T0, T1, T2, T3>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS1<T0, T1, T2, T3> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			T2* c2 = view.component2 + start;
			ushort* c3 = view.component3 + start;

			// create new view
			ComponentsViewS1<T0, T1, T2, T3> slice = new(archetype, c0, c1, c2, c3, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>3 normal components and 2 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS2<T0, T1, T2, T3, T4>(this Action<ComponentsViewS2<T0, T1, T2, T3, T4>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, ISharedComponent
		where T4 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2, T3, T4>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS2<T0, T1, T2, T3, T4> view = archetype.ComponentsS2<T0, T1, T2, T3, T4>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS2<T0, T1, T2, T3, T4> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			T2* c2 = view.component2 + start;
			ushort* c3 = view.component3 + start;
			ushort* c4 = view.component4 + start;

			// create new view
			ComponentsViewS2<T0, T1, T2, T3, T4> slice = new(archetype, c0, c1, c2, c3, c4, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>4 normal components and 0 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void Schedule<T0, T1, T2, T3>(this Action<ComponentsView<T0, T1, T2, T3>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2, T3>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsView<T0, T1, T2, T3> view = archetype.Components<T0, T1, T2, T3>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsView<T0, T1, T2, T3> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			T2* c2 = view.component2 + start;
			T3* c3 = view.component3 + start;

			// create new view
			ComponentsView<T0, T1, T2, T3> slice = new(archetype, c0, c1, c2, c3, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>4 normal components and 1 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS1<T0, T1, T2, T3, T4>(this Action<ComponentsViewS1<T0, T1, T2, T3, T4>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2, T3, T4>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS1<T0, T1, T2, T3, T4> view = archetype.ComponentsS1<T0, T1, T2, T3, T4>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS1<T0, T1, T2, T3, T4> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			T2* c2 = view.component2 + start;
			T3* c3 = view.component3 + start;
			ushort* c4 = view.component4 + start;

			// create new view
			ComponentsViewS1<T0, T1, T2, T3, T4> slice = new(archetype, c0, c1, c2, c3, c4, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

	/// <summary>
	/// execute job using specified dispatch settings <br/><br/>4 normal components and 2 shared
	/// </summary>
	/// <param name="a">action to execute</param>
	/// <param name="settings">dispatch settings</param>
	/// <param name="query">specified archetype query, or automatic from dispatch settings, if null</param>
	public static unsafe void ScheduleS2<T0, T1, T2, T3, T4, T5>(this Action<ComponentsViewS2<T0, T1, T2, T3, T4, T5>, JobState> a, JobScheduleSettings settings, Query? query = null) 
		where T0 : unmanaged, IComponent
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, ISharedComponent
		where T5 : unmanaged, ISharedComponent {
		// lock world
		World.Lock();

		// create query
		query ??= World.SelectStatic<T0, T1, T2, T3, T4, T5>().Worlds(settings.interactableWorlds, settings.activeWorlds, settings.visibleWorlds);

		// run query
		QueryResult result = query.Result();

		// create job states
		int jobsCount = settings.multithreaded 
			? result.archetypes.Sum(archetype => (int) MathF.Ceiling(math.min((float) archetype.entityCount / settings.minChunkSize, settings.maxThreadCount)))
			: result.archetypes.Count;
		JobState?[] states = new JobState?[jobsCount];

		// dispatch
		int jobIndex = 0;
		foreach (Archetype archetype in result.archetypes) {
			// get view
			ComponentsViewS2<T0, T1, T2, T3, T4, T5> view = archetype.ComponentsS2<T0, T1, T2, T3, T4, T5>();
			int sizeAdd = settings.multithreaded 
				? math.max(settings.minChunkSize, archetype.entityCount / settings.maxThreadCount)
				: int.MaxValue;

			// slice view
			for (int i = 0; i < view.count; i += sizeAdd) {
				// add job state
				states[jobIndex] = new(jobIndex, jobsCount - 1);
				jobIndex++;

				// cache variables for delegate
				int sliceStart = i;
				int currentJobIndex = jobIndex - 1;

				// execute by now
				if (settings.executeNow) {
					Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]);
					continue;
				}

				// dispatch for later execution
				FixedUpdatePipeline.EnqueueWithDelay(
					() => Run_(archetype, view, sliceStart, sizeAdd, states[currentJobIndex]), 
					null, 
					settings.tickDelay, 
					settings.loadBalancingTicks, 
					!settings.multithreaded, 
					waitForComplete: settings.waitForComplete);
			}
		}

		// unlock world
		World.Unlock();

		// execute job method
		void Run_(Archetype archetype, ComponentsViewS2<T0, T1, T2, T3, T4, T5> view, int sliceStart, int size, JobState jobState) {
			// update variables
			int entityCount = archetype.entityCount;
			int start = math.min(sliceStart, entityCount);
			int count = math.clamp(entityCount - sliceStart, 0, size);

			// get components with offsets
			T0* c0 = view.component0 + start;
			T1* c1 = view.component1 + start;
			T2* c2 = view.component2 + start;
			T3* c3 = view.component3 + start;
			ushort* c4 = view.component4 + start;
			ushort* c5 = view.component5 + start;

			// create new view
			ComponentsViewS2<T0, T1, T2, T3, T4, T5> slice = new(archetype, c0, c1, c2, c3, c4, c5, count);

			// run job
			a(slice, jobState);

			// mark job as done
			for (int i = 0; i < jobsCount; i++) states[i]?.IncrementCompletedIterations();
		}
	}

}