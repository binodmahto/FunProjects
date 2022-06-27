using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using MoqPOC;
using MoqPOC.Services;
using System.Reflection;

namespace MoqPOC.Tests
{
    internal class TestMoqPOCApplication : WebApplicationFactory<Program>
    {
        private readonly MockServices _mockServices;
        public TestMoqPOCApplication(MockServices mockServices)
        {
            _mockServices = mockServices;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                foreach ((var interfaceType, var serviceMock) in _mockServices.GetMocks())
                {
                    services.Remove(services.SingleOrDefault(d => d.ServiceType == interfaceType));
                    services.AddSingleton(typeof(IWeatherForecastService), serviceMock);
                }
            });
            return base.CreateHost(builder);
            
        }

    }
}
