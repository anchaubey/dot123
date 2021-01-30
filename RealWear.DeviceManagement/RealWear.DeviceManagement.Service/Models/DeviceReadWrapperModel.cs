using System.Collections.Generic;

namespace RealWear.DeviceManagement.Service.Models
{
    public class DeviceReadWrapperModel
    {
        public List<DeviceReadModel> Devices { get; set; }

        public int? NextCursor { get; set; }
    }
}
