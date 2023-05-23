using CRM_api.Services.IServices.Business_Module.Loan_Module;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using CRM_api.Services.IServices.HR_Module;
using CRM_api.Services.IServices.User_Module;
using CRM_api.Services.Services.Business_Module.Loan_Module;
using CRM_api.Services.Services.Business_Module.MutualFunds_Module;
using CRM_api.Services.Services.Business_Module.Stocks_Module;
using CRM_api.Services.Services.HR_Module;
using CRM_api.Services.Services.User_Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CRM_api.DataAccess.DataAccessDepedancy
{
    public static class ServicesDependancy
    {
        public static void InjectServiceDependecy(this IServiceCollection Services, IConfiguration config)
        {
            Services.InjectDataAccessDependecy(config);

            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //User Module
            Services.AddScoped<IUserMasterService, UserMasterService>();
            Services.AddScoped<IRoleMasterService, RoleMasterService>();
            Services.AddScoped<IRegionService, RegionService>();

            //Business Module
            Services.AddScoped<ILoanMasterService, LoanMasterService>();
            Services.AddScoped<IStockService, StockService>();
            Services.AddScoped<IMutualfundService, MutualfundService>();

            //Hr Module
            Services.AddScoped<IEmployeeService, EmployeeService>();
            Services.AddScoped<IDepartmentService, DepartmentService>();
            Services.AddScoped<IDesignationService, DesignationService>();
            Services.AddScoped<ILeaveTypeService, LeaveTypeService>();

        }
    }
}
