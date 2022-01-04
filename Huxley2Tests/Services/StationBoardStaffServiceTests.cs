// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using Xunit;

namespace Huxley2Tests.Services
{
    public class StationBoardStaffServiceTests
    {
        private StationBoardRequest restRequest;
        private IMapperService mapper;
        private LDBSVServiceSoap client;
        private StationBoardStaffService service;

        public StationBoardStaffServiceTests()
        {
            restRequest = new StationBoardRequest();
            mapper = A.Fake<IMapperService>();
            client = A.Fake<LDBSVServiceSoap>();
            service = new StationBoardStaffService(A.Fake<ILogger<StationBoardStaffService>>(), mapper, client);
        }

        [Fact]
        public async Task StationBoardStaffServiceGetDepartureBoardCallsClient()
        {
            var soapRequest = new GetDepartureBoardByCRSRequest();
            A.CallTo(() => mapper.MapGetDepartureBoardStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepartureBoardByCRSAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetDepartureBoardWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetDepBoardWithDetailsRequest();
            A.CallTo(() => mapper.MapGetDepBoardWithDetailsStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalBoardCallsClient()
        {
            var soapRequest = new GetArrivalBoardByCRSRequest();
            A.CallTo(() => mapper.MapGetArrivalBoardStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalBoardByCRSAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalBoardWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetArrBoardWithDetailsRequest();
            A.CallTo(() => mapper.MapGetArrBoardWithDetailsStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalBoardAsync(restRequest);

            A.CallTo(() => client.GetArrBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalDepartureBoardCallsClient()
        {
            var soapRequest = new GetArrivalDepartureBoardByCRSRequest();
            A.CallTo(() => mapper.MapGetArrivalDepartureBoardStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrivalDepartureBoardByCRSAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetArrivalDepartureBoardWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetArrDepBoardWithDetailsRequest();
            A.CallTo(() => mapper.MapGetArrDepBoardWithDetailsStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetArrivalDepartureBoardAsync(restRequest);

            A.CallTo(() => client.GetArrDepBoardWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetNextDeparturesCallsClient()
        {
            var soapRequest = new GetNextDeparturesRequest();
            A.CallTo(() => mapper.MapGetNextDeparturesStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetNextDeparturesWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetNextDeparturesWithDetailsRequest();
            A.CallTo(() => mapper.MapGetNextDeparturesWithDetailsStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetNextDeparturesAsync(restRequest);

            A.CallTo(() => client.GetNextDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetFastestDeparturesCallsClient()
        {
            var soapRequest = new GetFastestDeparturesRequest();
            A.CallTo(() => mapper.MapGetFastestDeparturesStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task StationBoardStaffServiceGetFastestDeparturesWithDetailsCallsClient()
        {
            restRequest.Expand = true;
            var soapRequest = new GetFastestDeparturesWithDetailsRequest();
            A.CallTo(() => mapper.MapGetFastestDeparturesWithDetailsStaffRequest(restRequest)).Returns(soapRequest);

            await service.GetFastestDeparturesAsync(restRequest);

            A.CallTo(() => client.GetFastestDeparturesWithDetailsAsync(soapRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void StationBoardStaffServiceGeneratesChecksum()
        {
            var board = new DeparturesBoard();

            var checksum = service.GenerateChecksum(board);

            // SHA256 hash of empty object Base64 URL encoded _with_ extra quotes
            Assert.Equal("\"zgb3gZHGVbuh9frUHx5P0x2-pnUSIEF_TPoBIrhuwjE\"", checksum);
        }

        [Fact]
        public void StationBoardStaffServiceGeneratesChecksumIgnoringGeneratedAt()
        {
            var board = new DeparturesBoard { generatedAt = DateTime.Now };

            var checksum = service.GenerateChecksum(board);

            Assert.Equal("\"zgb3gZHGVbuh9frUHx5P0x2-pnUSIEF_TPoBIrhuwjE\"", checksum);

            board.generatedAt = DateTime.Now.AddMinutes(5);

            checksum = service.GenerateChecksum(board);

            Assert.Equal("\"zgb3gZHGVbuh9frUHx5P0x2-pnUSIEF_TPoBIrhuwjE\"", checksum);
        }

        [Fact]
        public void StationBoardStaffServiceGeneratesChecksumPreservingGeneratedAt()
        {
            var now = DateTime.Now;
            var board = new DeparturesBoard { generatedAt = now };

            service.GenerateChecksum(board);

            Assert.Equal(now, board.generatedAt);
        }

    }
}
