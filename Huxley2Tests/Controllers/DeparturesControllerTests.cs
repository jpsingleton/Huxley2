// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class DeparturesControllerTests : BaseControllerTests
    {
        private DeparturesController controller;

        public DeparturesControllerTests()
        {
            controller = new DeparturesController(A.Fake<ILogger<DeparturesController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task DeparturesControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetDepartureBoardAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeparturesControllerGetReturnsResponseFromService()
        {
            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task DeparturesControllerSetsETag()
        {
            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
