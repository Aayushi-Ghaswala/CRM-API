using CRM_api.Services.IServices;
using CRM_api.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM_api.DataAccess.DataAccessDepedancy
{
    public static class ServicesDependancy
    {
        public static void InjectServiceDependecy(this IServiceCollection Services, IConfiguration config)
        {
            Services.InjectDataAccessDependecy(config);

            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            Services.AddScoped<IUserMasterService, UserMasterService>();
            Services.AddScoped<IRoleMasterService, RoleMasterService>();
            Services.AddScoped<IRegionService, RegionService>();
        }
    }
}
