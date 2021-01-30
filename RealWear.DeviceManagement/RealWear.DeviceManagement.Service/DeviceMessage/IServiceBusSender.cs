namespace RealWear.DeviceManagement.Service.DeviceMessage
{
    using RealWear.DeviceManagement.Service.Events;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IServiceBusSender" />.
    /// </summary>
    public interface IServiceBusSender
    {
        /// <summary>
        /// The SendRegisteredMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="DeviceRegistered"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendRegisteredMessageAsync(DeviceRegistered deviceEvent, CancellationToken cancellationToken);

        /// <summary>
        /// The SendUpdatedMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="DeviceUpdated"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendUpdatedMessageAsync(DeviceUpdated deviceEvent, CancellationToken cancellationToken);

        /// <summary>
        /// The SendDeletedMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="DeviceDeleted"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendDeletedMessageAsync(DeviceDeleted deviceEvent, CancellationToken cancellationToken);
    }
}
