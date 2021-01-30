namespace RealWear.DeviceManagement.Service.Models
{
    using RealWear.DeviceManagement.Service.Constant;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="DeviceBaseModel" />.
    /// </summary>
    public class DeviceBaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceBaseModel"/> class.
        /// </summary>
        public DeviceBaseModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceBaseModel"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="description">The description<see cref="string"/>.</param>
        public DeviceBaseModel(string name, string email, string description)
        {
            Name = name;
            Email = email;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [MaxLength(40, ErrorMessage = "Name cannot be more than 40 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        [MaxLength(250, ErrorMessage = "Description cannot be more than 250 characters")]
        public string Description { get; set; }
    }
}
