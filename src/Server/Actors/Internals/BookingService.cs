using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.Extensions.Logging;

namespace Server.Actors.Internals
{
    public class BookingService : IBookingService
    {
        public Task Book(IStorageState state, BookRequest request, bool fromActor, ILogger logger)
        {
            if (state.Seats.All(x => x.Id != request.SeatNumber))
            {
                throw new ValidationException($"Seat number={request.SeatNumber} is out of range!!!");
            }

            if (state.BookedSlots.Any(x => x == request.SeatNumber))
            {
                logger.LogInformation(
                    $"[{(fromActor ? "Actor" : "Normal")}] SeatNumber={request.SeatNumber} has already booked.");
            }
            else
            {
                logger.LogInformation(
                    $"[{(fromActor ? "Actor" : "Normal")}-BOOKED]: UserId={request.UserId} & SeatNumber={request.SeatNumber} will be booked.");

                state.BookedSlots.Add(request.SeatNumber);
            }

            return Task.CompletedTask;
        }
    }
}
