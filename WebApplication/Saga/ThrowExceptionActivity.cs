using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;
using WebApplication.Events;

namespace WebApplication
{
    public class ThrowExceptionActivity : Activity<ModelState, CreateModelRequestedEvent>
    {
        public void Probe(ProbeContext context)
        {
            context.CreateScope("create-model");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<ModelState, CreateModelRequestedEvent> context, Behavior<ModelState, CreateModelRequestedEvent> next)
        {
            throw new Exception();
            
            await next.Execute(context);
        }

        public async Task Faulted<TException>(BehaviorExceptionContext<ModelState, CreateModelRequestedEvent, TException> context, Behavior<ModelState, CreateModelRequestedEvent> next) where TException : Exception
        {
            await next.Faulted(context);
        }
    }
}