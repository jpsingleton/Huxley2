// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class StaffDeparturesControllerTests
    {
        [Fact]
        public async Task StaffDeparturesControllerGetPassesRequestToService()
        {
            var request = new StationBoardRequest();
            var service = A.Fake<IStationBoardStaffService>();
            var controller = new StaffDeparturesController(A.Fake<ILogger<StaffDeparturesController>>(), service);

            await controller.Get(request);

            A.CallTo(() => service.GetDepartureBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StaffDeparturesControllerGetReturnsResponseFromService()
        {
            var request = new StationBoardRequest();
            var response = new BaseStationBoard();
            var service = A.Fake<IStationBoardStaffService>();
            A.CallTo(() => service.GetDepartureBoardAsync(request)).Returns(response);
            var controller = new StaffDeparturesController(A.Fake<ILogger<StaffDeparturesController>>(), service);

            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }
    }
}
