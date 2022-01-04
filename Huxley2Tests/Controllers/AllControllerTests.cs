// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class AllControllerTests : BaseControllerTests
    {
        private AllController controller;

        public AllControllerTests()
        {
            controller = new AllController(A.Fake<ILogger<AllController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task AllControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetArrivalDepartureBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AllControllerGetReturnsResponseFromService()
        {
            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task AllControllerSetsETag()
        {
            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
