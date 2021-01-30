namespace RealWear.DeviceManagement.Service.Constant
{
    public static class ApiConstant
    {
        public const string ApiName = "Device Management Microservice";

        public const string ApiVersion = "v1";

        public const string SwaggerUrl = "/swagger/v1/swagger.json";

        public const string SerialNumberRegex = @"^[a-zA-Z0-9]*$";

        public const int DefaultPageSize = 25;

        public const int DefaultRecordMaxLimit = 500;

        public const int MaxDeviceRequestToAdd = 100;

        public const int SearchStringMinLength = 5;
    }
}
