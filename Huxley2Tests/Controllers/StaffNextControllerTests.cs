// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class StaffNextControllerTests : StaffBaseControllerTests
    {
        private StaffNextController controller;

        public StaffNextControllerTests()
        {
            controller = new StaffNextController(A.Fake<ILogger<StaffNextController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task StaffNextControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetNextDeparturesAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StaffNextControllerGetReturnsResponseFromService()
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
