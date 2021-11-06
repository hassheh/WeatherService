using NUnit.Framework;
using WeatherService.WeatherUpdaterService.Helper;

namespace WeatherService.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertKelvinToClesiusTest()
        {
            var clesiusTemp = WeatherHelper.ConvertKelvinToClesius(300);
            Assert.AreEqual(clesiusTemp, -26.85);
            Assert.Pass();
        }
    }
}