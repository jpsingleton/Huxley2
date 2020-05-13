// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Services
{
    public class StationBoardServiceTests
    {
        [Fact]
        public async Task StationBoardServiceGetDepartureBoardCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetDepartureBoardRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetDepartureBoardRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepartureBoardAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetDepartureBoardWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetDepBoardWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetDepBoardWithDetailsRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalBoardCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetArrivalBoardRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrivalBoardRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalBoardAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalBoardWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetArrBoardWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrBoardWithDetailsRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalDepartureBoardCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetArrivalDepartureBoardRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrivalDepartureBoardRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalDepartureBoardAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalDepartureBoardWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetArrDepBoardWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetArrDepBoardWithDetailsRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetNextDeparturesCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetNextDeparturesRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetNextDeparturesRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetNextDeparturesWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetNextDeparturesWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetNextDeparturesWithDetailsRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetFastestDeparturesCallsClient()
        {
            var restRequest = new StationBoardRequest();
            var soapRequest = new GetFastestDeparturesRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetFastestDeparturesRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetFastestDeparturesWithDetailsCallsClient()
        {
            var restRequest = new StationBoardRequest { Expand = true };
            var soapRequest = new GetFastestDeparturesWithDetailsRequest();
            var mapper = A.Fake<IMapperService>();
            A.CallTo(() => mapper.MapGetFastestDeparturesWithDetailsRequest(restRequest)).Returns(soapRequest);
            var client = A.Fake<LDBServiceSoap>();
            var service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }


    }
}
