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

            var step = Step.Create("post_booking", async context =>
            {
                var random = new Random();
                var request = new BookRequest
                {
                    UserId = new Guid("a000711d-e6b9-4c6c-b4d6-d0b726103847"),
                    ShowId = new Guid("adeaaf18-80da-49ae-bf16-83a4ef4783ff"),
                    SeatNumber = random.Next(1, 2),
                    Price = 100
                };

                //var response = await httpClient.PostAsJsonAsync("http://localhost:5000/WeatherForecast", request, context.CancellationToken);

                // var response = await httpClient.PostAsJsonAsync(
                //     "http://127.0.0.1:54124/v1.0/actors/BookingMovieActor/adeaaf18-80da-49ae-bf16-83a4ef4783ff/method/Book",
                //     request, context.CancellationToken);

                var response = await httpClient.PostAsJsonAsync("http://localhost:5000/WeatherForecast/booking", request, context.CancellationToken);

                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            });

            var scenario = ScenarioBuilder
                .CreateScenario("simple_http", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromSeconds(30))
                );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}
