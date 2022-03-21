using Checkout.API.Backgrounds.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.API.Backgrounds
{
    public class TimedHostedService : IHostedService
    {
        private readonly IWorker worker;
        public TimedHostedService(IWorker worker)
        {
            this.worker = worker;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await worker.DoWork(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Background service is stop");
            return Task.CompletedTask;
        }
    }
}
