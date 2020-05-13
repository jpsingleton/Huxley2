// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Services
{
    public class DelaysServiceTests
    {
        [Fact]
        public void DelaysServiceLoadsDelayMinutesThreshold()
        {
            var config = A.Fake<IConfiguration>();
            config["DelayMinutesThreshold"] = "7";

            var service = new DelaysService(
                A.Fake<ILogger<DelaysService>>(),
                A.Fake<IStationBoardService>(),
                A.Fake<ICrsService>(),
                config,
                A.Fake<IDateTimeService>()
            );

            Assert.Equal(7, service.DelayMinutesThreshold);
        }

        [Fact]
        public void DelaysServiceLoadsDefaultDelayMinutesThreshold()
        {
            var service = new DelaysService(
                A.Fake<ILogger<DelaysService>>(),
                A.Fake<IStationBoardService>(),
                A.Fake<ICrsService>(),
                A.Fake<IConfiguration>(),
                A.Fake<IDateTimeService>());

            Assert.Equal(5, service.DelayMinutesThreshold);
        }

        [Fact]
        public async Task DelaysServiceReturnsDelaysResponse()
        {
            var service = new DelaysService(
                A.Fake<ILogger<DelaysService>>(),
                A.Fake<IStationBoardService>(),
                A.Fake<ICrsService>(),
                A.Fake<IConfiguration>(),
                A.Fake<IDateTimeService>());

            var response = await service.GetDelaysAsync(new StationBoardRequest());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task DelaysServiceReturnsDelaysResponseWithLateTrains()
        {
            var service = new DelaysService(
                A.Fake<ILogger<DelaysService>>(),
                A.Fake<IStationBoardService>(),
                A.Fake<ICrsService>(),
                A.Fake<IConfiguration>(),
                A.Fake<IDateTimeService>());

            var request = new StationBoardRequest
            {
                Crs = "ABC",
                FilterType = FilterType.to,
                FilterCrs = "XYZ",
                NumRows = 5,
            };

            var response = await service.GetDelaysAsync(request);

            Assert.NotNull(response);
        }
    }
}
