using Checkout.API.Backgrounds.Interfaces;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.API.Backgrounds
{
    public class Worker : IWorker
    {
        private int number = 0;
        public Worker() { }
        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Interlocked.Increment(ref number);
                Log.Information($"Worker print number is {number}");
                await Task.Delay(1000 * 60);
            }
        }
    }
}
