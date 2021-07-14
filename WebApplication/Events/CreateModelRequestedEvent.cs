using System;

namespace WebApplication.Events
{
    public record CreateModelRequestedEvent
    {
        public Guid CorrelationId { get; init; }
        
        public bool WithError { get; init; }
    }
}