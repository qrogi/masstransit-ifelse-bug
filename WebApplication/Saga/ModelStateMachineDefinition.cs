using GreenPipes;
using MassTransit;
using MassTransit.Definition;

namespace WebApplication
{
    public class ModelStateMachineDefinition : SagaDefinition<ModelState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<ModelState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}