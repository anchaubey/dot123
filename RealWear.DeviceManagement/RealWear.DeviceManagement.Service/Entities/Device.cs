namespace RealWear.DeviceManagement.Service.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Defines the <see cref="Device" />.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the SerialNumber.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Workspace.
        /// </summary>
        [BsonIgnoreIfNull]
        public string Workspace { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [BsonIgnoreIfNull]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        [BsonIgnoreIfNull]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the OSVersion.
        /// </summary>
        [BsonIgnoreIfNull]
        public string OSVersion { get; set; }

        /// <summary>
        /// Gets or sets the DeviceModel.
        /// </summary>
        [BsonIgnoreIfNull]
        public string DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the Agent1Version.
        /// </summary>
        [BsonIgnoreIfNull]
        public string Agent1Version { get; set; }

        /// <summary>
        /// Gets or sets the Agent2Version.
        /// </summary>
        [BsonIgnoreIfNull]
        public string Agent2Version { get; set; }

        /// <summary>
        /// Gets or sets the FirmwareRelease.
        /// </summary>
        [BsonIgnoreIfNull]
        public string FirmwareRelease { get; set; }

        /// <summary>
        /// Gets or sets the Groups.
        /// </summary>
        [BsonIgnoreIfNull]
        public string[] Groups { get; set; }
    }
}
