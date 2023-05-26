using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.Services.Helper.Reminder_Helper.LI_GI_Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CRM_api.Services.Helper.Background_Service.LI_GI_Module
{
    public class InsPremiumReminderService : IHostedService, IDisposable
    {
        private readonly ILogger<InsPremiumReminderService> _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _config;

        public InsPremiumReminderService(ILogger<InsPremiumReminderService> logger, IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInsuranceClientRepository>();

                InsPremiumReminderHelper.InsPremimumReminder(_config, service);
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