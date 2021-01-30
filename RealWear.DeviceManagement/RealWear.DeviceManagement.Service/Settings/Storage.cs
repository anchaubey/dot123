namespace RealWear.DeviceManagement.Service.Settings
{
    /// <summary>
    /// Defines the <see cref="Storage" />.
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// Gets or sets the MongoDBConnectionString.
        /// </summary>
        public string MongoDBConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the MongoDBDatabaseName.
        /// </summary>
        public string MongoDBDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the ServiceBusConnectionString.
        /// </summary>
        public string ServiceBusConnectionString { get; set; }
    }
}
