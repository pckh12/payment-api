using System;
using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Payment.API.Domain;
using Payment.API.Infrastructure;
using Payment.API.Infrastructure.Filters;
using Payment.API.Infrastructure.Repositories;

namespace Payment.API
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
          
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            // add mediatr support
            services.AddMediatR(typeof(Startup));

            // add dependency mappings
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();


            // add swagger support
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment API",
                    Description = "Sample Payment API",
                    Version = "v1"
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, "Payment.API.xml");
                c.IncludeXmlComments(filePath);
            });

            ConfigureDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PaymentContext context)
        {
            if (env.IsDevelopment())
            {
                context.Database.EnsureCreated();

                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable swagger middleware
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API V1");
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            // add in-memory database
            services.AddDbContext<PaymentContext>(c =>
                c.UseInMemoryDatabase("Payment"));
        }
    }
}
