using System;
using System.Net.Http;
using System.Net.Http.Json;
using Contracts;
using NBomber.Contracts;
using NBomber.CSharp;

namespace PerTestClient
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            SimpleHttpTest.Run();
        }
    }

    public static class SimpleHttpTest
    {
        public static void Run()
        {
            using var httpClient = new HttpClient();

            var random = new Random();

            var step1 = Step.Create("post_booking_dapr", async context =>
            {
                var request = new BookRequest
                {
                    UserId = new Guid("a000711d-e6b9-4c6c-b4d6-d0b726103847"),
                    ShowId = new Guid("adeaaf18-80da-49ae-bf16-83a4ef4783ff"),
                    SeatNumber = random.Next(1, 3),
                    Price = 100
                };

                var response = await httpClient.PostAsJsonAsync("http://localhost:5000/booking/actor", request, context.CancellationToken);

                // HACK!
                // var response = await httpClient.PostAsJsonAsync(
                //     "http://127.0.0.1:56736/v1.0/actors/BookingMovieActor/adeaaf18-80da-49ae-bf16-83a4ef4783ff/method/Book",
                //     request, context.CancellationToken);

                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            });

            var step = Step.Create("post_booking", async context =>
            {
                var request = new BookRequest
                {
                    UserId = new Guid("a000711d-e6b9-4c6c-b4d6-d0b726103847"),
                    ShowId = new Guid("adeaaf18-80da-49ae-bf16-83a4ef4783ff"),
                    SeatNumber = random.Next(1, 3),
                    Price = 100
                };

                var response = await httpClient.PostAsJsonAsync("http://localhost:5000/booking", request, context.CancellationToken);

                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            });

            var scenario = ScenarioBuilder
                .CreateScenario("simple_http", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromSeconds(5))
                );

            var scenario1 = ScenarioBuilder
                .CreateScenario("simple_http_dapr", step1)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromSeconds(5))
                );

            NBomberRunner
                .RegisterScenarios(scenario, scenario1)
                .Run();
        }
    }
}
