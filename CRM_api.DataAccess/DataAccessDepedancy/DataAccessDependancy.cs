using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Repositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.Repositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.Repositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.Repositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Repositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Repositories.Business_Module.WBC_Module;
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
        public static void InjectDataAccessDependecy(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CRMDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                    sql =>
                    {
                        sql.CommandTimeout(60);
                        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
            });

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //User Module
            services.AddScoped<IUserMasterRepository, UserMasterRepository>();
            services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();

            //Business_Module
            services.AddScoped<ILoanMasterRepository, LoanMasterRepository>();
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddScoped<IMutualfundRepositry, MutualfundRepositery>();
            services.AddScoped<IInsuranceClientRepository, InsuranceClientRepository>();
            services.AddScoped<IMGainSchemeRepository, MGainSchemeRepository>();
            services.AddScoped<IWBCRepository, WBCRepository>();

            //HR Module
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        }
    }
}
