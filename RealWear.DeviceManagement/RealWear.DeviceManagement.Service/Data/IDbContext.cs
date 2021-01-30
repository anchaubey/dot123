namespace RealWear.DeviceManagement.Service.Data
{
    using MongoDB.Driver;
    using RealWear.DeviceManagement.Service.Entities;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IDbContext" />.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Gets the Device.
        /// </summary>
        IMongoCollection<Device> Device { get; }

        /// <summary>
        /// Gets a value indicating whether SupportsTransaction.
        /// </summary>
        bool SupportsTransaction { get; }

        /// <summary>
        /// The StartSessionAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IClientSessionHandle}"/>.</returns>
        Task<IClientSessionHandle> StartSessionAsync();
    }
}
