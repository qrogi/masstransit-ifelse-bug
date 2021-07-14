using System;
using Automatonymous;
using WebApplication.Events;

namespace WebApplication
{
    public class ModelStateMachine : MassTransitStateMachine<ModelState>
    {
        public ModelStateMachine()
        {
            Event(() => CreateModelRequested, x =>
            {
                x.CorrelateById(x => x.Message.CorrelationId);
            });
            
            InstanceState(x => x.CurrentState);

            Initially(
                When(CreateModelRequested)
                    .Then(x => x.Instance.CorrelationId = x.Data.CorrelationId)
                    .IfElse(x => !x.Data.WithError, then => then
                            .TransitionTo(Completed),
                        @else => @else
                            .Activity(x => x.OfType<ThrowExceptionActivity>())
                            .TransitionTo(Completed)
                            .Catch<Exception>(ex => ex
                                .TransitionTo(Faulted))));
        }
        
        public State Completed { get; set; }
        
        public State Faulted { get; set; }

        public Event<CreateModelRequestedEvent> CreateModelRequested { get; private set; }
    }
}