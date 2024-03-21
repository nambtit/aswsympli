using Application.Abstraction;
using Application.Features.SEORank.Commands.UpdateSEORank;
using SysTimer = System.Timers;

namespace Presentation.Web.Services
{
    public class HostedRankDataUpdateService : IHostedService, IDisposable
    {
        private readonly SysTimer.Timer _timer;
        private bool _wip;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HostedRankDataUpdateService> _logger;

        public HostedRankDataUpdateService(IServiceProvider serviceProvider, IApplicationConfig applicationConfig, ILogger<HostedRankDataUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            var interval = TimeSpan.FromMinutes(applicationConfig.SEORankDataRefreshFrequencyMinutes);
            interval = TimeSpan.FromSeconds(10);
            _timer = new SysTimer.Timer(interval);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Elapsed += UpdateSEORank;
            _timer.Enabled = true;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Enabled = false;
            return Task.CompletedTask;
        }

        private void UpdateSEORank(object? sender, SysTimer.ElapsedEventArgs e)
        {
            try
            {
                if (_wip)
                {
                    return;
                }

                _wip = true;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetService<IUpdateSEORankDataHandler>();
                    handler.HandleAsync().Wait();
                    _wip = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update SEO rank data. Error: {ex}");
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
