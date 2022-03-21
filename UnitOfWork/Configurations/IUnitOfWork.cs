using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.UnitOfWork.Configurations
{
    public interface IUnitOfWork : IDisposable
    {
        DatabaseFacade Database { get; }
        Task<int> SaveChangeAsync();
    }
}
