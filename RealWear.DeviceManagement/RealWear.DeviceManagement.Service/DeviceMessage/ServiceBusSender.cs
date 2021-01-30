namespace RealWear.DeviceManagement.Service.DeviceMessage
{
    using Azure.Messaging.ServiceBus;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RealWear.DeviceManagement.Service.Events;
    using RealWear.DeviceManagement.Service.Settings;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ServiceBusSender" />.
    /// </summary>
    public class ServiceBusSender : IServiceBusSender
    {
        /// <summary>
        /// Defines the _serviceBusConectionString.
        /// </summary>
        private readonly string _serviceBusConectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusSender"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="IOptions{Storage}"/>.</param>
        public ServiceBusSender(IOptions<Storage> options)
        {
            _serviceBusConectionString = options.Value?.ServiceBusConnectionString;
        }

        /// <summary>
        /// The SendMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="Object"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <param name="topicName">The topicName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendMessageAsync(Object deviceEvent, CancellationToken cancellationToken, string topicName)
        {
            await using (ServiceBusClient client = new ServiceBusClient(_serviceBusConectionString))
            {
                Azure.Messaging.ServiceBus.ServiceBusSender sender = client.CreateSender(topicName);
                string data = JsonConvert.SerializeObject(deviceEvent);
                ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(data));
                await sender.SendMessageAsync(message, cancellationToken);
            }
        }

        /// <summary>
        /// The SendRegisteredMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="DeviceRegistered"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendRegisteredMessageAsync(DeviceRegistered deviceEvent, CancellationToken cancellationToken)
        {
            foreach (var device in deviceEvent.Devices)
            {
                await SendMessageAsync(device, cancellationToken, deviceEvent.EventName);
            }
        }

        /// <summary>
        /// The SendUpdatedMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="DeviceUpdated"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendUpdatedMessageAsync(DeviceUpdated deviceEvent, CancellationToken cancellationToken)
        {
            await SendMessageAsync(deviceEvent, cancellationToken, deviceEvent.EventName);
        }

        /// <summary>
        /// The SendDeletedMessageAsync.
        /// </summary>
        /// <param name="deviceEvent">The deviceEvent<see cref="DeviceDeleted"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendDeletedMessageAsync(DeviceDeleted deviceEvent, CancellationToken cancellationToken)
        {
            await SendMessageAsync(deviceEvent, cancellationToken, deviceEvent.EventName);
        }
    }
}
