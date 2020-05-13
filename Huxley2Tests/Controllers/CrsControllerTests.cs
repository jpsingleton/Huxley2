// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class CrsControllerTests
    {
        [Fact]
        public void CrsControllerGetPassesRequestToService()
        {
            var service = A.Fake<ICrsService>();
            var controller = new CrsController(A.Fake<ILogger<CrsController>>(), service);

            controller.Get("test");

            A.CallTo(() => service.GetStations("test")).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void CrsControllerGetReturnsResponseFromService()
        {
            var response = new List<CrsRecord> { new CrsRecord { CrsCode = "TST", StationName = "Test Station" } };
            var service = A.Fake<ICrsService>();
            A.CallTo(() => service.GetStations("test")).Returns(response);
            var controller = new CrsController(A.Fake<ILogger<CrsController>>(), service);

            var stations = controller.Get("test");

            Assert.Equal(response, stations);
        }
    }
}
