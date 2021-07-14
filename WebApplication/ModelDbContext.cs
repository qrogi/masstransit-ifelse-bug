using System.Collections.Generic;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;

namespace WebApplication
{
    public class ModelDbContext : SagaDbContext
    {
        public ModelDbContext(DbContextOptions<ModelDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new ModelStateMap();
            }
        }
    }
}