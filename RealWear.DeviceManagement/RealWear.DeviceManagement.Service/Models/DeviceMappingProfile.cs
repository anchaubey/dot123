namespace RealWear.DeviceManagement.Service.Models
{
    using AutoMapper;
    using RealWear.DeviceManagement.Service.Entities;

    /// <summary>
    /// Defines the <see cref="DeviceMappingProfile" />.
    /// </summary>
    public class DeviceMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMappingProfile"/> class.
        /// </summary>
        public DeviceMappingProfile()
        {
            CreateMap<string, string>()
                .ConvertUsing(str => str != null ? str.Trim() : str);

            CreateMap<Device, DeviceReadModel>();

            CreateMap<DeviceCreateModel, Device>()
                .ForMember(deviceModel => deviceModel.Agent1Version, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.Agent2Version, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.DeviceModel, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.FirmwareRelease, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.Groups, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.OSVersion, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.Workspace, memberConfigurationExpression => memberConfigurationExpression.Ignore())
                .ForMember(deviceModel => deviceModel.Id, memberConfigurationExpression => memberConfigurationExpression.Ignore());

            CreateMap<DeviceUpdateModel, Device>()
            .ForMember(deviceModel => deviceModel.Agent1Version, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.Agent2Version, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.DeviceModel, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.FirmwareRelease, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.Groups, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.OSVersion, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.Workspace, memberConfigurationExpression => memberConfigurationExpression.Ignore())
            .ForMember(deviceModel => deviceModel.SerialNumber, memberConfigurationExpression => memberConfigurationExpression.Ignore());
        }
    }
}
