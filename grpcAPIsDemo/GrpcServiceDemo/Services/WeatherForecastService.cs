using Grpc.Core;
using GrpcServiceDemo;
using Google.Protobuf.WellKnownTypes;

namespace GrpcServiceDemo.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
        }

        public Task<WeatherForecastReply> GetWeatherForecast(ServerCallContext context)
        {
            return Task.FromResult<WeatherForecastReply>(GetWeather());
        }

        public Task<WeatherForecastReply> GetWeatherForecastForDate(Timestamp date, ServerCallContext context)
        {
            return Task.FromResult<WeatherForecastReply>(GetWeather(date));
        }

        public WeatherForecast GetWeatherForecast(int index)
        {
            return GetWeather(index);
        }

        private WeatherForecastReply GetWeather()
        {
            var result = new WeatherForecastReply();
            for (var index = 1; index <= 5; index++)
            {
                result.Result.Add(
                        GetWeather(index)
                    );
            }
            return result;
        }

        private static WeatherForecast GetWeather(int index)
        {
            return new WeatherForecast
            {
                Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                TemperatureF = (int)(32 + (Random.Shared.Next(-20, 55) / 0.5556))
            };
        }

        private WeatherForecastReply GetWeather(Timestamp date)
        {
            var result = new WeatherForecastReply();
            result.Result.Add(
                new WeatherForecast
                {
                    Date = date,
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    TemperatureF = (int)(32 + (Random.Shared.Next(-20, 55) / 0.5556))
                }
                );
            return result;
        }
    }
}
