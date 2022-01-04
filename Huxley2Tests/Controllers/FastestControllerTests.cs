// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class FastestControllerTests : BaseControllerTests
    {
        private FastestController controller;

        public FastestControllerTests()
        {
            controller = new FastestController(A.Fake<ILogger<FastestController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task FastestControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetFastestDeparturesAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task FastestControllerGetReturnsResponseFromService()
        {
            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task FastestControllerSetsETag()
        {
            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
