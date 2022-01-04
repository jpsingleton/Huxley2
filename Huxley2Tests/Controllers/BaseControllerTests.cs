// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenLDBWS;

namespace Huxley2Tests.Controllers
{
    public class BaseControllerTests
    {
        internal HttpContext httpContext;
        internal HttpResponse httpResponse;
        internal ControllerContext controllerContext;
        internal StationBoardRequest request;
        internal IStationBoardService service;
        internal BaseStationBoard response;
        internal string etag;

        public BaseControllerTests()
        {
            httpContext = A.Fake<HttpContext>();
            httpResponse = A.Fake<HttpResponse>();
            A.CallTo(() => httpContext.Response).Returns(httpResponse);
            controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            request = new StationBoardRequest();
            etag = "fake-etag-value";

            service = A.Fake<IStationBoardService>();
            response = new BaseStationBoard();
            A.CallTo(() => service.GetArrivalDepartureBoardAsync(request)).Returns(response);
            A.CallTo(() => service.GetArrivalBoardAsync(request)).Returns(response);
            A.CallTo(() => service.GetDepartureBoardAsync(request)).Returns(response);
            A.CallTo(() => service.GetFastestDeparturesAsync(request)).Returns(response);
            A.CallTo(() => service.GetNextDeparturesAsync(request)).Returns(response);
            A.CallTo(() => service.GenerateChecksum(response)).Returns(etag);
        }
    }
}
