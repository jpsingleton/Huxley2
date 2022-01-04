// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class StaffArrivalsControllerTests : StaffBaseControllerTests
    {
        private StaffArrivalsController controller;

        public StaffArrivalsControllerTests()
        {
            controller = new StaffArrivalsController(A.Fake<ILogger<StaffArrivalsController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task StaffArrivalsControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetArrivalBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StaffArrivalsControllerGetReturnsResponseFromService()
        {
            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task StaffAllControllerSetsETag()
        {
            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
