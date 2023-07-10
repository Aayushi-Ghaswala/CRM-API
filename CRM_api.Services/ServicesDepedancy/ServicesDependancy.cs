using CRM_api.DataAccess.DataAccessDepedancy;
using CRM_api.Services.IServices.Business_Module.Dashboard;
using CRM_api.Services.IServices.Business_Module.Fasttrack_Module;
using CRM_api.Services.IServices.Business_Module.LI_GI_Module;
using CRM_api.Services.IServices.Business_Module.Loan_Module;
using CRM_api.Services.IServices.Business_Module.MGain_Module;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using CRM_api.Services.IServices.Business_Module.WBC_Module;
using CRM_api.Services.IServices.HR_Module;
using CRM_api.Services.IServices.Sales_Module;
using CRM_api.Services.IServices.User_Module;
using CRM_api.Services.Services.Business_Module.Dashboard;
using CRM_api.Services.Services.Business_Module.Fasttrack_Module;
using CRM_api.Services.Services.Business_Module.LI_GI_Module;
using CRM_api.Services.Services.Business_Module.Loan_Module;
using CRM_api.Services.Services.Business_Module.MGain_Module;
using CRM_api.Services.Services.Business_Module.MutualFunds_Module;
using CRM_api.Services.Services.Business_Module.Stocks_Module;
using CRM_api.Services.Services.Business_Module.WBC_Module;
using CRM_api.Services.Services.HR_Module;
using CRM_api.Services.Services.Sales_Module;
using CRM_api.Services.Services.User_Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CRM_api.Services.ServicesDepedancy
{
    public static class ServicesDependancy
    {
        public static void InjectServiceDependecy(this IServiceCollection services, IConfiguration config)
        {
            services.InjectDataAccessDependecy(config);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //Cache Service
            services.AddMemoryCache();

            ////Background Service
            //Services.AddHostedService<InsPremiumReminderService>();
            //Services.AddHostedService<InsDueReminderService>();

            //User Module
            services.AddScoped<IUserMasterService, UserMasterService>();
            services.AddScoped<IRoleMasterService, RoleMasterService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<ILoginService, LoginService>();

            //Business Module
            services.AddScoped<ILoanMasterService, LoanMasterService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IInsuranceClientService, InsuranceClientService>();
            services.AddScoped<IMutualfundService, MutualfundService>();
            services.AddScoped<IMGainService, MGainService>();
            services.AddScoped<IMGainSchemeService, MGainSchemeService>();
            services.AddScoped<IWBCService, WBCService>();
            services.AddScoped<IBusinessDashboardService, BusinessDashboardService>();
            services.AddScoped<IFasttrackService, FasttrackService>();

            //Hr Module
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<ILeaveTypeService, LeaveTypeService>();
            services.AddScoped<IPayCheckService, PayCheckService>();
            services.AddScoped<IUserLeaveService, UserLeaveService>();

            //Sales Module
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ISourceTypeService, SourceTypeService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<IMeetingParticipantService, MeetingParticipantService>();
            services.AddScoped<IMeetingAttachmentService, MeetingAttachmentService>();
            services.AddScoped<ILeadService, LeadService>();
        }
    }
}
