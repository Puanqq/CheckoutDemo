using Checkout.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.UnitOfWork.Configurations
{
    public abstract class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        protected DatabaseFacade database;
        protected readonly TContext context;
        public UnitOfWork(TContext context)
        {
            this.context = context;           
            database = context.Database;
        }   
        public void Dispose()
        {
            context.Dispose();
        }

        public Task<int> SaveChangeAsync()
        {
            try
            {
                return context.SaveChangesAsync();
            }
            catch 
            {                
                return Task.FromResult(0);
            }
        }
        public DatabaseFacade Database { get => database; }
    }
}
