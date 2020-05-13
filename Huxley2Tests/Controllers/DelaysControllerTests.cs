// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class DelaysControllerTests
    {
        [Fact]
        public async Task DelaysControllerGetPassesRequestToService()
        {
            var request = new StationBoardRequest();
            var service = A.Fake<IDelaysService>();
            var controller = new DelaysController(A.Fake<ILogger<DelaysController>>(), service);

            await controller.Get(request);

            A.CallTo(() => service.GetDelaysAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DelaysControllerGetReturnsResponseFromService()
        {
            var request = new StationBoardRequest();
            var response = new DelaysResponse();
            var service = A.Fake<IDelaysService>();
            A.CallTo(() => service.GetDelaysAsync(request)).Returns(response);
            var controller = new DelaysController(A.Fake<ILogger<DelaysController>>(), service);

            var board = await controller.Get(request);

            Assert.Equal(response, board);
        }
    }
}
