using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Repositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.Repositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Repositories.HR_Module;
using CRM_api.DataAccess.Repositories.User_Module;
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

            //User Module
            Services.AddScoped<IUserMasterRepository, UserMasterRepository>();
            Services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
            Services.AddScoped<IRegionRepository, RegionRepository>();

            //Business_Module
            Services.AddScoped<ILoanMasterRepository, LoanMasterRepository>();
            Services.AddScoped<IMutualfundRepositry, MutualfundRepositery>();

            //HR Module
            Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Services.AddScoped<IDesignationRepository, DesignationRepository>();
        }
    }
}
