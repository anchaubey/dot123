namespace RealWear.DeviceManagement.Service.Models
{
    /// <summary>
    /// Defines the <see cref="DeviceUpdateModel" />.
    /// </summary>
    public class DeviceUpdateModel : DeviceBaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceUpdateModel"/> class.
        /// </summary>
        public DeviceUpdateModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceUpdateModel"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="description">The description<see cref="string"/>.</param>
        public DeviceUpdateModel(string name, string email, string description) : base(name, email, description)
        {
        }
    }
}
