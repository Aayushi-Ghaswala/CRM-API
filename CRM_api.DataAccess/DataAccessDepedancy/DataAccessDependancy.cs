using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories;
using CRM_api.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CRM_api.DataAccess.DataAccessDepedancy
{
    public static class DataAccessDependancy
    {
        public static void InjectDataAccessDependecy(this IServiceCollection Services, IConfiguration config)
        {
            Services.AddDbContext<CRMDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                    sql => sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            Services.AddScoped<IUserMasterRepository, UserMasterRepository>();
            Services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
            Services.AddScoped<IRegionRepository, RegionRepository>();
        }
    }
}
