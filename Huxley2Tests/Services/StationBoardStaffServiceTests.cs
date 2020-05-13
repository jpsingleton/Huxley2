// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Services
{
    public class StationBoardStaffServiceTests
    {
        [Fact]
        public async Task StationBoardStaffServiceGetDepartureBoardCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetDepartureBoardByCRSRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetDepartureBoardStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepartureBoardByCRSAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetDepartureBoardWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetDepBoardWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetDepBoardWithDetailsStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalBoardCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetArrivalBoardByCRSRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrivalBoardStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalBoardByCRSAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalBoardWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetArrBoardWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrBoardWithDetailsStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalDepartureBoardCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetArrivalDepartureBoardByCRSRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrivalDepartureBoardStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalDepartureBoardByCRSAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalDepartureBoardWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetArrDepBoardWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrDepBoardWithDetailsStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetNextDeparturesCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetNextDeparturesRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetNextDeparturesStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetNextDeparturesWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetNextDeparturesWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetNextDeparturesWithDetailsStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetFastestDeparturesCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetFastestDeparturesRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetFastestDeparturesStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetFastestDeparturesWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetFastestDeparturesWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetFastestDeparturesWithDetailsStaffRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBSVServiceSoap>();
            var service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }


    }
}
