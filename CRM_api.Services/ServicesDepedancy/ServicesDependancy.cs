using CRM_api.Services.IServices;
using CRM_api.Services.IServices.HR_Module;
using CRM_api.Services.Services;
using CRM_api.Services.Services.HR_Module;
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

            Services.AddScoped<IEmployeeService, EmployeeService>();
            Services.AddScoped<IDepartmentService, DepartmentService>();
            Services.AddScoped<IDesignationService, DesignationService>();
        }
    }
}
