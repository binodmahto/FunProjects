using Moq;
using MoqPOC.Services;
using System.Reflection;

namespace MoqPOC.Tests
{
    internal class MockServices
    {
        public Mock<IWeatherForecastService> WeatherForecastServiceMock { get; init; } 

        public MockServices()
        {
            WeatherForecastServiceMock = new Mock<IWeatherForecastService>();
        }

        public IEnumerable<(Type, object)> GetMocks()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x =>
                {
                    var interfaceType = x.PropertyType.GetGenericArguments()[0];
                    var value = x.GetValue(this) as Mock;

                    return (interfaceType, value.Object);
                })
                .ToArray();
        }
    }
}
