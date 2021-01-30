namespace RealWear.DeviceManagement.Service.Data
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using RealWear.DeviceManagement.Service.Entities;
    using RealWear.DeviceManagement.Service.Settings;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DbContext" />.
    /// </summary>
    public class DbContext : IDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="IOptions{DatabaseOptions}"/>.</param>
        public DbContext(IOptions<Storage> options)
        {
            MongoClient = new MongoClient(options.Value?.MongoDBConnectionString);
            var db = MongoClient.GetDatabase(options.Value?.MongoDBDatabaseName);
            Device = db.GetCollection<Device>(nameof(Device));
        }

        /// <summary>
        /// Gets the Device.
        /// </summary>
        public IMongoCollection<Device> Device { get; }

        /// <summary>
        /// Gets the MongoClient.
        /// </summary>
        private MongoClient MongoClient { get; }

        /// <summary>
        /// Gets a value indicating whether SupportsTransaction.
        /// </summary>
        public bool SupportsTransaction
        {
            get
            {
                return !string.IsNullOrEmpty(MongoClient.Cluster.Settings.ReplicaSetName);
            }
        }

        /// <summary>
        /// The StartSessionAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IClientSessionHandle}"/>.</returns>
        public async Task<IClientSessionHandle> StartSessionAsync()
        {
            return await MongoClient.StartSessionAsync(new ClientSessionOptions());
        }
    }
}
