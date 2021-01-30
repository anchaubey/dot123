namespace RealWear.DeviceManagement.Service.Events
{
    using RealWear.DeviceManagement.Service.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="DeviceRegistered" />.
    /// </summary>
    public class DeviceRegistered : IEvent
    {
        /// <summary>
        /// Gets the Event Name.
        /// </summary>
        public string EventName => "DeviceRegistered";

        /// <summary>
        /// Gets or sets the Devices.
        /// </summary>
        public IEnumerable<DeviceCreateModel> Devices { get; set; }
    }
}
