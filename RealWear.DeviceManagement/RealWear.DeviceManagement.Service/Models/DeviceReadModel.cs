namespace RealWear.DeviceManagement.Service.Models
{
    /// <summary>
    /// Defines the <see cref="DeviceReadModel" />.
    /// </summary>
    public class DeviceReadModel : DeviceUpdateModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceReadModel"/> class.
        /// </summary>
        public DeviceReadModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceReadModel"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="description">The description<see cref="string"/>.</param>
        /// <param name="workspace">The workspace<see cref="string"/>.</param>
        /// <param name="oSVersion">The oSVersion<see cref="string"/>.</param>
        /// <param name="deviceModel">The deviceModel<see cref="string"/>.</param>
        /// <param name="agent1Version">The agent1Version<see cref="string"/>.</param>
        /// <param name="agent2Version">The agent2Version<see cref="string"/>.</param>
        /// <param name="firmwareRelease">The firmwareRelease<see cref="string"/>.</param>
        /// <param name="groups">The groups<see cref="string[]"/>.</param>
        /// <param name="serialNumber">The serialNumber<see cref="string"/>.</param>
        public DeviceReadModel(string id, string name, string email, string description, string workspace, string oSVersion, string deviceModel, string agent1Version, string agent2Version, string firmwareRelease, string[] groups, string serialNumber) : base(name, email, description)
        {
            Id = id;
            Workspace = workspace;
            OSVersion = oSVersion;
            DeviceModel = deviceModel;
            Agent1Version = agent1Version;
            Agent2Version = agent2Version;
            FirmwareRelease = firmwareRelease;
            Groups = groups;
            SerialNumber = serialNumber;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the SerialNumber.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Workspace.
        /// </summary>
        public string Workspace { get; set; }

        /// <summary>
        /// Gets or sets the OSVersion.
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// Gets or sets the DeviceModel.
        /// </summary>
        public string DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the Agent1Version.
        /// </summary>
        public string Agent1Version { get; set; }

        /// <summary>
        /// Gets or sets the Agent2Version.
        /// </summary>
        public string Agent2Version { get; set; }

        /// <summary>
        /// Gets or sets the FirmwareRelease.
        /// </summary>
        public string FirmwareRelease { get; set; }

        /// <summary>
        /// Gets or sets the Groups.
        /// </summary>
        public string[] Groups { get; set; }
    }
}
