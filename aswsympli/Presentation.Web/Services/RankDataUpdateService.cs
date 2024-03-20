using Application.Features.SEORank.Commands.UpdateSEORank;

namespace Presentation.Web.Services
{
    public class RankDataUpdateService
    {
        private readonly System.Timers.Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public RankDataUpdateService(IServiceProvider serviceProvider)
        {
            _timer = new System.Timers.Timer(new TimeSpan(1, 0, 0));
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _serviceProvider = serviceProvider;
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var s = _serviceProvider.GetService<IUpdateSEORankDataHandler>();
            s.HandleAsync().Wait();
        }
    }
}
