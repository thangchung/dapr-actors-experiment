using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Dapr.Actors;
using Dapr.Actors.Client;
using Server.Actors;
using Server.Actors.Internals;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IStorageState _state;
        private readonly IBookingService _bookingService;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILogger<BookingController> _logger;

        public BookingController(NormalStorageState state, IBookingService bookingService, ILogger<BookingController> logger)
        {
            _state = state;
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task Booking([FromBody] BookRequest request)
        {
            _logger.LogInformation($"[Normal-Host]: {Dns.GetHostName()}");
            _logger.LogInformation(
                $"[Normal-IP Address]: {Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Aggregate(" ", (a, b) => $"{a} {b}").Split(' ').LastOrDefault()}");

            //try
            {
                //await _semaphore.WaitAsync();
                await _bookingService.Book(_state, request, false, _logger);
            }
            //finally
            {
                //_semaphore.Release();
            }
        }

        [HttpPost("actor")]
        public async Task BookingActor([FromBody] BookRequest request)
        {
            _logger.LogInformation($"[Actor-Host]: {Dns.GetHostName()}");
            _logger.LogInformation(
                $"[Actor-IP Address]: {Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Aggregate(" ", (a, b) => $"{a} {b}").Split(' ').LastOrDefault()}");

            var actorId = new ActorId(request.ShowId.ToString());
            var proxy = ActorProxy.Create<IBookingMovieActor>(actorId, nameof(BookingMovieActor));
            await proxy.Book(request);
        }
    }
}
