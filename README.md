# dapr-actors-experiment
This is an experiment that use Dapr actors on booking movie scenario

```
$ tye run
```

# Run it at terminal

curl -X POST http://127.0.0.1:<actor port>/v1.0/actors/BookingMovieActor/adeaaf18-80da-49ae-bf16-83a4ef4783ff/method/Book -d "{ \"userId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\", \"showId\": \"adeaaf18-80da-49ae-bf16-83a4ef4783ff\", \"seatNumber\": 1, \"price\": 100 }"

<actor port>: the middle column in tye dashboard

## PerTestClient project

If you want to run with NBommer, try to un-comment the code at 

```csharp
var response = await httpClient.PostAsJsonAsync(
  "http://127.0.0.1:<actor port>/v1.0/actors/BookingMovieActor/adeaaf18-80da-49ae-bf16-83a4ef4783ff/method/Book",
  request, context.CancellationToken);
```


