namespace RealWear.DeviceManagement.Service.Models
{
    using RealWear.DeviceManagement.Service.Constant;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="DeviceCreateModel" />.
    /// </summary>
    public class DeviceCreateModel : DeviceBaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCreateModel"/> class.
        /// </summary>
        public DeviceCreateModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCreateModel"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="description">The description<see cref="string"/>.</param>
        /// <param name="serialNumber">The serialNumber<see cref="string"/>.</param>
        public DeviceCreateModel(string name, string email, string description, string serialNumber) : base(name, email, description)
        {
            SerialNumber = serialNumber;
        }

        /// <summary>
        /// Gets or sets the SerialNumber.
        /// </summary>
        [Required(ErrorMessage = "Serial Number is required")]
        [RegularExpression(ApiConstant.SerialNumberRegex, ErrorMessage = "Invalid Serial Number")]
        public string SerialNumber { get; set; }
    }
}
