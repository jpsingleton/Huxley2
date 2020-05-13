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
    public class FastestControllerTests
    {
        [Fact]
        public async Task FastestControllerGetPassesRequestToService()
        {
            var request = new StationBoardRequest();
            var service = A.Fake<IStationBoardService>();
            var controller = new FastestController(A.Fake<ILogger<FastestController>>(), service);

            await controller.Get(request);

            A.CallTo(() => service.GetFastestDeparturesAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task FastestControllerGetReturnsResponseFromService()
        {
            var request = new StationBoardRequest();
            var response = new BaseStationBoard();
            var service = A.Fake<IStationBoardService>();
            A.CallTo(() => service.GetFastestDeparturesAsync(request)).Returns(response);
            var controller = new FastestController(A.Fake<ILogger<FastestController>>(), service);

            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }
    }
}
