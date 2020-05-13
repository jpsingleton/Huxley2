// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class AllControllerTests
    {
        [Fact]
        public async Task AllControllerGetPassesRequestToService()
        {
            var request = new StationBoardRequest();
            var service = A.Fake<IStationBoardService>();
            var controller = new AllController(A.Fake<ILogger<AllController>>(), service);

            await controller.Get(request);

            A.CallTo(() => service.GetArrivalDepartureBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AllControllerGetReturnsResponseFromService()
        {
            var request = new StationBoardRequest();
            var response = new BaseStationBoard();
            var service = A.Fake<IStationBoardService>();
            A.CallTo(() => service.GetArrivalDepartureBoardAsync(request)).Returns(response);
            var controller = new AllController(A.Fake<ILogger<AllController>>(), service);

            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }
    }
}
