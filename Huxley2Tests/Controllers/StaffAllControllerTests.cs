// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class StaffAllControllerTests : StaffBaseControllerTests
    {
        private StaffAllController controller;

        public StaffAllControllerTests()
        {
            controller = new StaffAllController(A.Fake<ILogger<StaffAllController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task StaffAllControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetArrivalDepartureBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StaffAllControllerGetReturnsResponseFromService()
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
