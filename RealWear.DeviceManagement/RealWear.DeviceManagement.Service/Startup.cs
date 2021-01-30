namespace RealWear.DeviceManagement.Service
{
    using AutoMapper;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using RealWear.DeviceManagement.Service.Constant;
    using RealWear.DeviceManagement.Service.Data;
    using RealWear.DeviceManagement.Service.DeviceMessage;
    using RealWear.DeviceManagement.Service.Utilities;
    using RealWear.DeviceManagement.Service.Settings;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object AcccountConstants { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.Configure<Storage>(Configuration.GetSection(nameof(Storage)));
            services.AddScoped<IDbContext, DbContext>();            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IServiceBusSender, ServiceBusSender>();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration["Accounts:Authority"];
                options.SupportedTokens = SupportedTokens.Jwt;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AccountConstants.SuperAdmin, policy => policy.RequireRole(AccountConstants.SuperAdmin));
                options.AddPolicy(AccountConstants.AllUsers, policy => policy.RequireAuthenticatedUser());
            });

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc(ApiConstant.ApiVersion, new OpenApiInfo { Title = ApiConstant.ApiName, Version = ApiConstant.ApiVersion });
                swaggerOptions.OperationFilter<AddWorkspaceIdHeaderParameter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(ApiConstant.SwaggerUrl, $"{ApiConstant.ApiName} {ApiConstant.ApiVersion}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
