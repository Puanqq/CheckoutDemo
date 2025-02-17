﻿using Checkout.Entities.Models;
using Checkout.UnitOfWork.IRepositories;
using Checkout.UnitOfWork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.UnitOfWork.Configurations
{
    public class CheckoutUnitOfWork : UnitOfWork<CheckoutDemoContext>, ICheckoutUnitOfWork
    {
        public CheckoutUnitOfWork(CheckoutDemoContext context) : base(context)
        {
            Product = new GenericRepository<Product, CheckoutDemoContext>(context);
            Order = new GenericRepository<Order, CheckoutDemoContext>(context);
            OrderDetail = new GenericRepository<OrderDetail, CheckoutDemoContext>(context);       
        }
        public IGenericRepository<Product> Product { get; init; }
        public IGenericRepository<Order> Order { get; init; }
        public IGenericRepository<OrderDetail> OrderDetail { get; init; }
    }
}
