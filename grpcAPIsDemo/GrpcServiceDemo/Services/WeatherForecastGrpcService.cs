using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace GrpcServiceDemo.Services
{
    public sealed class WeatherForecastGrpcService : WeatherForcast.WeatherForcastBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastService> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastGrpcService(ILogger<WeatherForecastService> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        public override Task<WeatherForecastReply> GetWeatherForecast(Empty request, ServerCallContext context)
        {
            return _weatherForecastService.GetWeatherForecast(context);
        }

        public override Task<WeatherForecastReply> GetWeatherForecastForDate(Timestamp date, ServerCallContext context)
        {
            return _weatherForecastService.GetWeatherForecastForDate(date, context);
        }

        public override async Task GetWeatherForecastStream(Empty request, IServerStreamWriter<WeatherForecast> responseStream, ServerCallContext context)
        {
            var i = 0;
            while(!context.CancellationToken.IsCancellationRequested && i <50)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(_weatherForecastService.GetWeatherForecast(i));
                i++;
            }
        }

        public override async Task<WeatherForecastReply> GetWeatherForecastClientStream(IAsyncStreamReader<StreamMessage> requestStream, ServerCallContext context)
        {
            var response = new WeatherForecastReply();
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var i = requestStream.Current.Index;
                response.Result.Add(_weatherForecastService.GetWeatherForecast(i));
            }
            return await Task.FromResult<WeatherForecastReply>(response);
        }

        public override async Task GetWeatherForecastDuplexStream(IAsyncStreamReader<StreamMessage> requestStream, IServerStreamWriter<WeatherForecast> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var i = requestStream.Current.Index;
                await Task.Delay(1000);
                await responseStream.WriteAsync(_weatherForecastService.GetWeatherForecast(i));
            }
        }
    }
}
