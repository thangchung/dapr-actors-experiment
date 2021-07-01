using System.Threading.Tasks;
using Contracts;
using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;
using Server.Actors.Internals;

namespace Server.Actors
{
    public class BookingMovieActor : Actor, IBookingMovieActor
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingMovieActor> _logger;
        private readonly IStorageState _state;

        public BookingMovieActor(ActorHost host, IBookingService bookingService, ActorStorageState storageState,
            ILogger<BookingMovieActor> logger) : base(host)
        {
            _bookingService = bookingService;
            _logger = logger;
            _state = storageState;
        }

        public async Task Book(BookRequest request)
        {
            // _logger.LogInformation($"Trying to book SeatNumber={request.SeatNumber}");
            await _bookingService.Book(_state, request, true, _logger);
        }
    }
}
