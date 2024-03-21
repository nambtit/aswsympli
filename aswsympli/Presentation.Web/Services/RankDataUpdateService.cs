using Application.Features.SEORank.Commands.UpdateSEORank;
using SysTimer = System.Timers;

namespace Presentation.Web.Services
{
    public class RankDataUpdateService : IHostedService, IDisposable
    {
        private SysTimer.Timer _timer;
        private bool _wip;
        private readonly IServiceProvider _serviceProvider;

        public RankDataUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new SysTimer.Timer(new TimeSpan(0, 0, 10));
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
