using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Dapr.Actors;
using Dapr.Actors.Client;
using Server.Actors;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly StorageState _state;

        private readonly IBookingService _bookingService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(StorageState state, IBookingService bookingService, ILogger<WeatherForecastController> logger)
        {
            _state = state;
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
                .ToArray();
        }

        [HttpPost("booking")]
        public async Task Booking([FromBody] BookRequest request)
        {
            await _bookingService.Book(_state, request, _logger);
        }

        [HttpPost]
        public async Task Post([FromBody] BookRequest request)
        {
            var actorId = new ActorId(request.ShowId.ToString());
            var proxy = ActorProxy.Create<IBookingMovieActor>(actorId, nameof(BookingMovieActor));
            await proxy.Book(request);
        }
    }
}
