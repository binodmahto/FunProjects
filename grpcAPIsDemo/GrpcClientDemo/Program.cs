
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceDemo;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7001");
//var client = new Greeter.GreeterClient(channel);
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GrpcClientDemo" });
//Console.WriteLine("Greeting: " + reply.Message);
//Console.WriteLine("Press any key to exit...");

var weatherClient = new WeatherForcast.WeatherForcastClient(channel);

var request = new Google.Protobuf.WellKnownTypes.Empty();
var weatherInfo = weatherClient.GetWeatherForecast(request);
Console.WriteLine("*****Weather forecast for 5 days*****");
Console.WriteLine(weatherInfo.Result);
Console.WriteLine();

weatherInfo = weatherClient.GetWeatherForecastForDate(Timestamp.FromDateTime(DateTime.UtcNow));
Console.WriteLine("*****Weather forecast for today*****");
Console.WriteLine(weatherInfo.Result);
Console.WriteLine();

request = new Google.Protobuf.WellKnownTypes.Empty();
weatherInfo = await weatherClient.GetWeatherForecastAsync(request);
Console.WriteLine("*****Weather forecast for 5 days*****");
Console.WriteLine(weatherInfo.Result);
Console.WriteLine();

weatherInfo = await weatherClient.GetWeatherForecastForDateAsync(Timestamp.FromDateTime(DateTime.UtcNow));
Console.WriteLine("*****Weather forecast for today*****");
Console.WriteLine(weatherInfo.Result);
Console.WriteLine();

Console.WriteLine("*******Server Streaming weather forecast*******");
try
{
var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
using var streamingCall = weatherClient.GetWeatherForecastStream(new Empty(), cancellationToken: cancellationToken.Token);

    await foreach (var weatherData in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cancellationToken.Token))
    {
        Console.WriteLine(weatherData);
    }
    Console.WriteLine("Stream completed.");
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}
Console.WriteLine();

Console.WriteLine("*******Client Streaming weather forecast*******");
try
{
    var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    using AsyncClientStreamingCall<StreamMessage, WeatherForecastReply> clientStreamingCall = weatherClient.GetWeatherForecastClientStream(cancellationToken: cancellationToken.Token);
    var i = 0;

    while (true)
    {
        if (i >= 10)
        {
            await clientStreamingCall.RequestStream.CompleteAsync();
            Console.WriteLine("Client Streaming completed.");
            break;
        }
        else
        {
            //write to stream
            await clientStreamingCall.RequestStream.WriteAsync(new StreamMessage { Index = i });
            i++;
        }
    }
    var response = await clientStreamingCall;
    Console.WriteLine(response.Result);
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}
Console.WriteLine();

Console.WriteLine("*******Server & Client Duplex Streaming weather forecast*******");
try
{
    var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    using AsyncDuplexStreamingCall<StreamMessage, WeatherForecast> duplexStreamingCall = weatherClient.GetWeatherForecastDuplexStream(cancellationToken: cancellationToken.Token);
    var i = 0;
    Task task = Task.WhenAll(new[]
    {
        Task.Run(async () =>{
            while (true)
            {
                if (i >= 10)
                {
                    await duplexStreamingCall.RequestStream.CompleteAsync();
                    Console.WriteLine("Client Streaming completed.");
                    break;
                }
                else
                {
                    //write to stream
                    await duplexStreamingCall.RequestStream.WriteAsync(new StreamMessage { Index = i });
                    i++;
                }
            }
        }),
        Task.Run(async () =>{
            //read from stream
             while (!cancellationToken.IsCancellationRequested && await duplexStreamingCall.ResponseStream.MoveNext())
             {
                 Console.WriteLine(duplexStreamingCall.ResponseStream.Current);
             }
        })
    });
    try
    {
        task.Wait(cancellationToken.Token);
    }
    catch (OperationCanceledException e)
    {
        await duplexStreamingCall.RequestStream.CompleteAsync();
        Thread.Sleep(6000);
    }
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}
Console.WriteLine();

Console.ReadKey();

