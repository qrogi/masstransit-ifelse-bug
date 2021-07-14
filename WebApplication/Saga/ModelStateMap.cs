using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication
{
    public class ModelStateMap : SagaClassMap<ModelState>
    {
        protected override void Configure(EntityTypeBuilder<ModelState> entity, ModelBuilder model)
        {
            entity.ToTable("model_state");

            entity.Property(x => x.CurrentState);
        }
    }
}