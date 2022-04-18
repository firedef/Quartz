using System;
using System.Diagnostics;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.queries;
using Quartz.Ecs.ecs.views;

namespace Quartz.Tests.data.ecs;

public class TestEntitiesSystem : EntitiesJobBase {
	public JobScheduleSettings settings;
	public Query? query;
	public TestEntitiesSystem(JobScheduleSettings settings, Query? query) {
		this.settings = settings;
		this.query = query;
	}

	public override void Run() {
		JobScheduler.Schedule<TestNormalComponentA>(RunSystem, settings, query);
	}

	public static unsafe void RunSystem(ComponentsView<TestNormalComponentA> components, JobState state) {
		Stopwatch sw = Stopwatch.StartNew();
		Console.WriteLine($"TestEntitiesSystem job start: {state.currentIteration} of {state.maxIteration}; completed: {state.completedIterations}; size: {components.count}");
		for (int i = 0; i < components.count; i++) {
			components.component0[i].a = 4 * components.component0[i].b;
		}
		Console.WriteLine($"TestEntitiesSystem job end in {sw.ElapsedMilliseconds}ms: {state.currentIteration} of {state.maxIteration}; completed: {state.completedIterations}");
	}
}