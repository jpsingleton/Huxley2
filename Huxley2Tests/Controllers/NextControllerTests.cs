// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class NextControllerTests : BaseControllerTests
    {
        private NextController controller;

        public NextControllerTests()
        {
            controller = new NextController(A.Fake<ILogger<NextController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task NextControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetNextDeparturesAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task NextControllerGetReturnsResponseFromService()
        {
            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task NextControllerSetsETag()
        {
            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
