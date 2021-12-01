using Google.Protobuf.WellKnownTypes;
using Grpc.Core;


namespace GrpcServiceDemo.Services
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecastReply> GetWeatherForecast(ServerCallContext context);

        Task<WeatherForecastReply> GetWeatherForecastForDate(Timestamp date, ServerCallContext context);

         WeatherForecast GetWeatherForecast(int index);
    }
}
