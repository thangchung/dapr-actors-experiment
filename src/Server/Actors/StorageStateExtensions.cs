using System;
using Contracts;

namespace Server.Actors
{
    public static class StorageStateExtensions
    {
        public static IStorageState InitData(this IStorageState state)
        {
            state.Users.AddRange(new[]
            {
                new User {Id = new Guid("a000711d-e6b9-4c6c-b4d6-d0b726103847"), Name = "buyer01"},
                new User {Id = new Guid("09f718f2-8497-4703-972f-fc930197abbc"), Name = "buyer02"},
                new User {Id = new Guid("59729156-ee16-45be-a8ed-9f7717b7cee5"), Name = "buyer03"}
            });

            state.Movies.Add(new Movie
            {
                Id = new Guid("722ce95a-9b63-4e17-8a69-22f1c29cc2e9"),
                Title = "Four Comedians & A Bouncer"
            });

            state.Seats.AddRange(new[]
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

            state.Shows.Add(new Show
            {
                Id = new Guid("adeaaf18-80da-49ae-bf16-83a4ef4783ff"),
                Date = DateTime.Now,
                StartTime = DateTime.Now.AddMinutes(TimeSpan.FromMinutes(30).Minutes),
                EndTime = DateTime.Now.AddMinutes(TimeSpan.FromMinutes(90).Minutes),
                MovieId = new Guid("722ce95a-9b63-4e17-8a69-22f1c29cc2e9")
            });

            return state;
        }
    }
}
