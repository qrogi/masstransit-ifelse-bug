using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MassTransit.EntityFrameworkCoreIntegration;
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

namespace WebApplication
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApplication", Version = "v1"});
            });
            
            services.AddDbContext<ModelDbContext>(options =>
                options.UseNpgsql("Server=localhost;Port=5432;UserId=postgres;Password=252525;Database=postgres;"));
            
            services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<ModelStateMachine, ModelState>(
                        typeof(ModelStateMachineDefinition))
                    .EntityFrameworkRepository(
                        r =>
                        {
                            r.UsePostgres();
                            r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                            r.ExistingDbContext<ModelDbContext>();
                        });
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConcurrentMessageLimit = 10;
                    cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
                });
            });
            
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}