namespace RealWear.DeviceManagement.Service.Utilities
{
    using RealWear.DeviceManagement.Service.Constant;
    using System;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtensions
    {
        public static string GetRole(this IIdentity identity)
        {
            var id = identity as ClaimsIdentity;
            var claim = id.FindFirst(JwtClaimTypesConstant.Role);
            if (claim == null) throw new InvalidOperationException("role is missing");
            return claim.Value;
        }

        public static string GetWorkspaceName(this IIdentity identity)
        {
            var id = identity as ClaimsIdentity;
            var claim = id.FindFirst(JwtClaimTypesConstant.Workspace);
            if (claim == null) throw new InvalidOperationException("workspace is missing");
            return claim.Value;
        }
    }
}
