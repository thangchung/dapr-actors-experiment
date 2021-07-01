using System.Collections.Generic;
using Contracts;

namespace Server.Actors.Internals
{
    public class ActorStorageState : IStorageState
    {
        public List<MovieBooking> MovieBookings { get; set; } = new();
        public List<int> BookedSlots { get; set; } = new();
        public List<User> Users { get; } = new();
        public List<Movie> Movies { get; } = new();
        public List<Seat> Seats { get; } = new();
        public List<Show> Shows { get; } = new();
    }
}
