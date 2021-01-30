namespace RealWear.DeviceManagement.Service.Events
{
    /// <summary>
    /// Defines the <see cref="IEvent" />.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the EventName.
        /// </summary>
        string EventName { get; }
    }
}
