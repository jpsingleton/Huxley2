// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class DelaysControllerTests : BaseControllerTests
    {
        private new IDelaysService service;
        private DelaysController controller;

        public DelaysControllerTests()
        {
            service = A.Fake<IDelaysService>();
            controller = new DelaysController(A.Fake<ILogger<DelaysController>>(), service)
            {
                ControllerContext = controllerContext
            };
        }
        [Fact]
        public async Task DelaysControllerGetPassesRequestToService()
        {
            await controller.Get(request);

            A.CallTo(() => service.GetDelaysAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DelaysControllerGetReturnsResponseFromService()
        {
            var response = new DelaysResponse();
            A.CallTo(() => service.GetDelaysAsync(request)).Returns(response);

            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }

        [Fact]
        public async Task DelaysControllerSetsETag()
        {
            var response = new DelaysResponse();
            A.CallTo(() => service.GetDelaysAsync(request)).Returns(response);
            A.CallTo(() => service.GenerateChecksum(response)).Returns(etag);

            await controller.Get(request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
