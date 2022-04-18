namespace Quartz.Ecs.ecs.jobs; 

public class JobState {
	public readonly int currentIteration;
	public readonly int maxIteration;
	private int _completedIterations;
	public int completedIterations => _completedIterations;

	public bool isLast => currentIteration == maxIteration;

	public JobState(int currentIteration, int maxIteration) {
		this.currentIteration = currentIteration;
		this.maxIteration = maxIteration;
		_completedIterations = completedIterations;
	}

	internal void IncrementCompletedIterations() => Interlocked.Increment(ref _completedIterations);
}