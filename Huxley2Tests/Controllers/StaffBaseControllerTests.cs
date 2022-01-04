// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Interfaces;
using OpenLDBSVWS;

namespace Huxley2Tests.Controllers
{
    public class StaffBaseControllerTests : BaseControllerTests
    {
        internal new IStationBoardStaffService service;
        internal new BaseStationBoard response;

        public StaffBaseControllerTests()
        {
            service = A.Fake<IStationBoardStaffService>();
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
