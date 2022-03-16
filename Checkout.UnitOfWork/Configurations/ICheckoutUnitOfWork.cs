using Checkout.Entities.Models;
using Checkout.UnitOfWork.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.UnitOfWork.Configurations
{
    public interface ICheckoutUnitOfWork : IUnitOfWork
    {
        IGenericRepository<Product> Product { get; init; }
        IGenericRepository<OrderItem> OrderItem { get; init; }
        IGenericRepository<OrderDetail> OrderDetail { get; init; }
    }
}
