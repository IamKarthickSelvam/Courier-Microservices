namespace TrackingService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
