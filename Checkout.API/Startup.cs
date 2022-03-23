using AutoMapper;
using Checkout.API.Backgrounds;
using Checkout.API.Backgrounds.Interfaces;
using Checkout.API.Filters;
using Checkout.API.Manager;
using Checkout.API.Manager.Interfaces;
using Checkout.API.Mappings;
using Checkout.API.Middlewares;
using Checkout.Entities.Models;
using Checkout.UnitOfWork.Configurations;
using Checkout.UnitOfWork.IRepositories;
using Checkout.UnitOfWork.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Checkout.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); 

            services.AddDbContext<CheckoutDemoContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout.API", Version = "v1" });
            });
            
            services.AddTransient<IOrdersManager, OrdersManager>();
            services.AddScoped<ICheckoutUnitOfWork, CheckoutUnitOfWork>();
            services.AddScoped<ValidationFilterAttribute>();

            services.AddSingleton<IWorker, Worker>();            
            
            //Config automapper            
            services.AddAutoMapper(typeof(DomainToResponseProfile));            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
