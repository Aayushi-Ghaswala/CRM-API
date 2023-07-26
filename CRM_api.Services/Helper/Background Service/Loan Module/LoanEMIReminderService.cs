using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.Services.Helper.Reminder_Helper.Loan_Module;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CRM_api.Services.Helper.Background_Service.Loan_Module
{
    public class LoanEMIReminderService : IHostedService, IDisposable
    {
        private readonly ILogger<LoanEMIReminderService> _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _config;
        private readonly FirebaseMessaging _firebaseMessaging;

        public LoanEMIReminderService(ILogger<LoanEMIReminderService> logger, IConfiguration config, IServiceScopeFactory serviceScopeFactory, FirebaseMessaging firebaseMessaging)
        {
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
            _firebaseMessaging = firebaseMessaging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ILoanMasterRepository>();

                LoanEMIReminderHelper.LoanEMIReminder(_config, service, _firebaseMessaging);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}