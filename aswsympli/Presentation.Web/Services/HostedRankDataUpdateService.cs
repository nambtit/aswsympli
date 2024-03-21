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

        public HostedRankDataUpdateService(IServiceProvider serviceProvider, IApplicationConfig applicationConfig)
        {
            _serviceProvider = serviceProvider;

            var interval = TimeSpan.FromMinutes(applicationConfig.SEORankDataRefreshFrequencyMinutes);
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

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
