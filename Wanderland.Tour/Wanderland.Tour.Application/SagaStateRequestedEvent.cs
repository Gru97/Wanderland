namespace Wanderland.Tour.Application;

public class SagaStateRequestedEvent
{
    public Guid TourId { get; set; }
}
public class SagaStateResponse
{
    public int State { get; set; }
    public Guid TourId { get; set; }
}