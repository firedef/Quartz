namespace Quartz.Ecs.exceptions; 

public class CollectionModifiedException : Exception {
	public CollectionModifiedException() : base("collection size was changed due enumeration") {}
}