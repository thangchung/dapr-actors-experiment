using System.Collections.Generic;
using Contracts;

namespace Server.Actors
{
    public interface IStorageState
    {
        public List<MovieBooking> MovieBookings { get; set; }
        public List<int> BookedSlots { get; set; }
        public List<User> Users { get; }
        public List<Movie> Movies { get; }
        public List<Seat> Seats { get; }
        public List<Show> Shows { get; }
    }
}
