namespace RealWear.DeviceManagement.Service.Events
{
    /// <summary>
    /// Defines the <see cref="DeviceDeleted" />.
    /// </summary>
    public class DeviceDeleted : IEvent
    {
        /// <summary>
        /// Gets the EventName.
        /// </summary>
        public string EventName => "DeviceDeleted";

        /// <summary>
        /// Gets or sets the SerialNumber.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }
    }
}
