using System;
using Automatonymous;

namespace WebApplication
{
    public class ModelState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        
        public string CurrentState { get; set; }
    }
}