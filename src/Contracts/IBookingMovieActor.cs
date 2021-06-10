using System;
using System.Threading.Tasks;
using Dapr.Actors;

namespace Contracts
{
    public interface IBookingMovieActor : IActor
    {
        Task Book(BookRequest request);
    }

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "user01";
    }

    public class Movie
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "Movie01";
        public string Description { get; set; } = "Movie01";
    }

    public enum SeatType
    {
        Normal,
        Special
    }

    public class Seat
    {
        public int Id { get; set; } = 1;
        public SeatType Type { get; set; } = SeatType.Normal;
    }

    public enum BookStatus
    {
        Available,
        Booked
    }

    public class Show
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class MovieBooking
    {
        public Guid UserId { get; set; }
        public Guid ShowId { get; set; }
        public int SeatId { get; set; }
        public BookStatus Status { get; set; } = BookStatus.Available;
    }

    [Serializable]
    public class BookRequest
    {
        public Guid UserId { get; set; }
        public Guid ShowId { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
    }
}
