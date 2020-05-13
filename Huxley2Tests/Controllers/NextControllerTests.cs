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
    public class NextControllerTests
    {
        [Fact]
        public async Task NextControllerGetPassesRequestToService()
        {
            var request = new StationBoardRequest();
            var service = A.Fake<IStationBoardService>();
            var controller = new NextController(A.Fake<ILogger<NextController>>(), service);

            await controller.Get(request);

            A.CallTo(() => service.GetNextDeparturesAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task NextControllerGetReturnsResponseFromService()
        {
            var request = new StationBoardRequest();
            var response = new BaseStationBoard();
            var service = A.Fake<IStationBoardService>();
            A.CallTo(() => service.GetNextDeparturesAsync(request)).Returns(response);
            var controller = new NextController(A.Fake<ILogger<NextController>>(), service);

            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }
    }
}
