using Moq;
using CoreWebAPIMoqDemo.Services;
using System.Reflection;

namespace CoreWebAPIMoqDemo.Tests
{
    internal class MockServices
    {
        public Mock<IWeatherForecastService> WeatherForecastServiceMock { get; init; } 

        public MockServices()
        {
            WeatherForecastServiceMock = new Mock<IWeatherForecastService>();
        }

        /// <summary>
        /// This returns the collection of all mock service's interface type and the mock object, defined here i.e. <see cref="WeatherForecastServiceMock"/>.
        /// </summary>
        /// <returns></returns>
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
