using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;

namespace Server.Actors
{
    public class BookingMovieActor : Actor, IBookingMovieActor
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingMovieActor> _logger;
        private readonly StorageState _state;

        public BookingMovieActor(ActorHost host, IBookingService bookingService, ILogger<BookingMovieActor> logger) : base(host)
        {
            _bookingService = bookingService;
            _logger = logger;
            _state = new StorageState();
        }

        public async Task Book(BookRequest request)
        {
            _logger.LogInformation($"Trying to book SeatNumber={request.SeatNumber}");

            // if (_state.MovieBookings.Any(x => x.SeatId == request.SeatNumber))
            // {
            //     _logger.LogInformation($"SeatNumber={request.SeatNumber} has already booked.");
            // }
            // else
            // {
            //     _logger.LogInformation($"UserId={request.UserId} & SeatNumber={request.SeatNumber} will be booked.");

            //     _state.MovieBookings.Add(
            //         new MovieBooking
            //         {
            //             SeatId = request.SeatNumber,
            //             UserId = request.UserId,
            //             ShowId = request.ShowId,
            //             Status = BookStatus.Booked
            //         });
            // }

            // return Task.CompletedTask;

            await _bookingService.Book(_state, request, _logger);
        }
    }

    public interface IBookingService
    {
        Task Book(StorageState state, BookRequest request, ILogger logger);
    }

    public class BookingService : IBookingService
    {
        public Task Book(StorageState state, BookRequest request, ILogger logger)
        {
            if (state.MovieBookings.Any(x => x.SeatId == request.SeatNumber))
            {
                logger.LogInformation($"SeatNumber={request.SeatNumber} has already booked.");
            }
            else
            {
                logger.LogInformation($"UserId={request.UserId} & SeatNumber={request.SeatNumber} will be booked.");

                state.MovieBookings.Add(
                    new MovieBooking
                    {
                        SeatId = request.SeatNumber,
                        UserId = request.UserId,
                        ShowId = request.ShowId,
                        Status = BookStatus.Booked
                    });
            }

            return Task.CompletedTask;
        }
    }

    public class StorageState
    {
        public List<MovieBooking> MovieBookings { get; set; } = new();
        public List<User> Users { get; } = new();
        public List<Movie> Movies { get; } = new();
        public List<Seat> Seats { get; } = new();
        public List<Show> Shows { get; } = new();

        public void InitData()
        {
            Users.AddRange(new[]
            {
                new User {Id = new Guid("a000711d-e6b9-4c6c-b4d6-d0b726103847"), Name = "buyer01"},
                new User {Id = new Guid("09f718f2-8497-4703-972f-fc930197abbc"), Name = "buyer02"},
                new User {Id = new Guid("59729156-ee16-45be-a8ed-9f7717b7cee5"), Name = "buyer03"}
            });

            Movies.Add(new Movie
            {
                Id = new Guid("722ce95a-9b63-4e17-8a69-22f1c29cc2e9"),
                Title = "Four Comedians & A Bouncer"
            });

            Seats.AddRange(new[]
            {
                new Seat
                {
                    Id = 1,
                    Type = SeatType.Normal
                },
                new Seat
                {
                    Id = 2,
                    Type = SeatType.Special
                }
            });

            Shows.Add(new Show
            {
                Id = new Guid("adeaaf18-80da-49ae-bf16-83a4ef4783ff"),
                Date = DateTime.Now,
                StartTime = DateTime.Now.AddMinutes(TimeSpan.FromMinutes(30).Minutes),
                EndTime = DateTime.Now.AddMinutes(TimeSpan.FromMinutes(90).Minutes),
                MovieId = new Guid("722ce95a-9b63-4e17-8a69-22f1c29cc2e9")
            });
        }
    }
}
