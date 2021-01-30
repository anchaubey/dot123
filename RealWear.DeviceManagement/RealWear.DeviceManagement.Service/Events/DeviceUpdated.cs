namespace RealWear.DeviceManagement.Service.Events
{
    using RealWear.DeviceManagement.Service.Models;

    /// <summary>
    /// Defines the <see cref="DeviceUpdated" />.
    /// </summary>
    public class DeviceUpdated : IEvent
    {
        /// <summary>
        /// Gets the Event Name.
        /// </summary>
        public string EventName => "DeviceUpdated";

        /// <summary>
        /// Gets or sets the Device.
        /// </summary>
        public DeviceUpdateModel Device { get; set; }
    }
}
