using System;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Events;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public WeatherForecastController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _publishEndpoint.Publish<CreateModelRequestedEvent>(new()
            {
                CorrelationId = Guid.NewGuid(),
                WithError = true
            });

            return Ok();
        }
    }
}