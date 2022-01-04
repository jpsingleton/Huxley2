// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class StaffFastestControllerTests : StaffBaseControllerTests
    {
        private StaffFastestController controller;

        public StaffFastestControllerTests()
        {
            controller = new StaffFastestController(A.Fake<ILogger<StaffFastestController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task StaffFastestControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetFastestDeparturesAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StaffFastestControllerGetReturnsResponseFromService()
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
