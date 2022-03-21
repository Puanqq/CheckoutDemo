using System.Threading;
using System.Threading.Tasks;

namespace Checkout.API.Backgrounds.Interfaces
{
    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}