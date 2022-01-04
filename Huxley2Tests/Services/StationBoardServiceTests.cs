// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using Xunit;

namespace Huxley2Tests.Services
{
    public class StationBoardServiceTests
    {
        private StationBoardRequest restRequest;
        private IMapperService mapper;
        private LDBServiceSoap client;
        private StationBoardService service;

        public StationBoardServiceTests()
        {
            restRequest = new StationBoardRequest();
            mapper = A.Fake<IMapperService>();
            client = A.Fake<LDBServiceSoap>();
            service = new StationBoardService(A.Fake<ILogger<StationBoardService>>(), mapper, client);
        }

        [Fact]
        public async Task StationBoardServiceGetDepartureBoardCallsClient()
        {
            var soapRequest = new GetDepartureBoardRequest();
            A.CallTo(() => mapper.MapGetDepartureBoardRequest(restRequest)).Returns(soapRequest);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepartureBoardAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetDepartureBoardWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetDepBoardWithDetailsRequest();
            A.CallTo(() => mapper.MapGetDepBoardWithDetailsRequest(restRequest)).Returns(soapRequest);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalBoardCallsClient()
        {
            var soapRequest = new GetArrivalBoardRequest();
            A.CallTo(() => mapper.MapGetArrivalBoardRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalBoardAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalBoardWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetArrBoardWithDetailsRequest();
            A.CallTo(() => mapper.MapGetArrBoardWithDetailsRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalDepartureBoardCallsClient()
        {
            var soapRequest = new GetArrivalDepartureBoardRequest();
            A.CallTo(() => mapper.MapGetArrivalDepartureBoardRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalDepartureBoardAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetArrivalDepartureBoardWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetArrDepBoardWithDetailsRequest();
            A.CallTo(() => mapper.MapGetArrDepBoardWithDetailsRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetNextDeparturesCallsClient()
        {
            var soapRequest = new GetNextDeparturesRequest();
            A.CallTo(() => mapper.MapGetNextDeparturesRequest(restRequest)).Returns(soapRequest);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetNextDeparturesWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetNextDeparturesWithDetailsRequest();
            A.CallTo(() => mapper.MapGetNextDeparturesWithDetailsRequest(restRequest)).Returns(soapRequest);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetFastestDeparturesCallsClient()
        {
            var soapRequest = new GetFastestDeparturesRequest();
            A.CallTo(() => mapper.MapGetFastestDeparturesRequest(restRequest)).Returns(soapRequest);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardServiceGetFastestDeparturesWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetFastestDeparturesWithDetailsRequest();
            A.CallTo(() => mapper.MapGetFastestDeparturesWithDetailsRequest(restRequest)).Returns(soapRequest);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void StationBoardServiceGeneratesChecksum()
        {
            var board = new DeparturesBoard();

            var checksum = service.GenerateChecksum(board);

            // SHA256 hash of empty object Base64 URL encoded _with_ extra quotes
            Assert.Equal("\"vOQ83HdPDnNoRTQxqG8Ur0exYUAvQ0w3k4SSX6qgYxE\"", checksum);
        }

        [Fact]
        public void StationBoardServiceGeneratesChecksumIgnoringGeneratedAt()
        {
            var board = new DeparturesBoard { generatedAt = DateTime.Now };

            var checksum = service.GenerateChecksum(board);

            Assert.Equal("\"vOQ83HdPDnNoRTQxqG8Ur0exYUAvQ0w3k4SSX6qgYxE\"", checksum);

            board.generatedAt = DateTime.Now.AddMinutes(5);

            checksum = service.GenerateChecksum(board);

            Assert.Equal("\"vOQ83HdPDnNoRTQxqG8Ur0exYUAvQ0w3k4SSX6qgYxE\"", checksum);
        }

        [Fact]
        public void StationBoardServiceGeneratesChecksumPreservingGeneratedAt()
        {
            var now = DateTime.Now;
            var board = new DeparturesBoard { generatedAt = now };

            service.GenerateChecksum(board);

            Assert.Equal(now, board.generatedAt);
        }

    }
}
