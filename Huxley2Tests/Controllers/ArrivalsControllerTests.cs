// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class ArrivalsControllerTests : BaseControllerTests
    {
        private ArrivalsController controller;

        public ArrivalsControllerTests()
        {
            controller = new ArrivalsController(A.Fake<ILogger<ArrivalsController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task ArrivalsControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetArrivalBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ArrivalsControllerGetReturnsResponseFromService()
        {
            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task ArrivalsControllerSetsETag()
        {
            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
