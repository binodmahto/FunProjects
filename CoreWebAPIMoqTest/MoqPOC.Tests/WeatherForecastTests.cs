using Moq;
using MoqPOC.Services;
using Newtonsoft.Json;

namespace MoqPOC.Tests
{
    public class WeatherForecastTests
    {
        private readonly TestMoqPOCApplication _testMoqPOCApplication;
        private readonly MockServices _mockServices;
        public readonly HttpClient _client;
        
        public WeatherForecastTests()
        {
            _mockServices = new MockServices();
            _testMoqPOCApplication = new TestMoqPOCApplication(_mockServices);
            _client = _testMoqPOCApplication.CreateClient();
        }

        [Fact]
        public async void GetWeatherForecastTest()
        {
            var expResult = new WeatherForecast[]
            {
                new WeatherForecast(DateTime.Now, 26, "Bengaluru")
            };
            _mockServices.WeatherForecastServiceMock.Setup(m => m.GetWeatherForecast()).ReturnsAsync(expResult);

            var response = await _client.GetAsync("/weatherforecast");
            string jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WeatherForecast[]>(jsonString);

            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.Equal(expResult, result);

        }
    }
}