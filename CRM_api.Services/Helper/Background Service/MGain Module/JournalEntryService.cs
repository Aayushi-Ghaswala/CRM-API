using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.Services.IServices.Account_Module;
using CRM_api.Services.IServices.Business_Module.MGain_Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CRM_api.Services.Helper.Background_Service.MGain_Module
{
    public class JournalEntryService : IHostedService, IDisposable
    {
        private readonly ILogger<JournalEntryService> _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _config;

        public JournalEntryService(ILogger<JournalEntryService> logger, IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var now = DateTime.Now.ToString("dd-MM-yyyy");
                string? endOfMonth = (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString() + "-" + DateTime.Now.Month.ToString("D2") + "-" + DateTime.Now.Year);

                if (now == endOfMonth)
                {
                    var iMgainService = scope.ServiceProvider.GetRequiredService<IMGainService>();
                    var iMgainRepository = scope.ServiceProvider.GetRequiredService<IMGainRepository>();
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountTransactionservice>();

                    Non_CumulativeEntryHelper.Non_CumulativeEntryHelper.EntryHelper(iMgainRepository, iMgainService, accountService);
                }
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
