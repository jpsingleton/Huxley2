// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Huxley2Tests.Services
{
    public class MapperServiceTests
    {
        private readonly DateTime TestDateTime = DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture);

        [Fact]
        public void MapperServiceMapsGetArrBoardWithDetailsRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrBoardWithDetailsRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterCrs);
            Assert.Equal(restRequest.FilterType, soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrDepBoardWithDetailsRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrDepBoardWithDetailsRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterCrs);
            Assert.Equal(restRequest.FilterType, soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrivalBoardRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrivalBoardRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterCrs);
            Assert.Equal(restRequest.FilterType, soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrivalDepartureBoardRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrivalDepartureBoardRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterCrs);
            Assert.Equal(restRequest.FilterType, soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetDepartureBoardRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetDepartureBoardRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterCrs);
            Assert.Equal(restRequest.FilterType, soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetDepBoardWithDetailsRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetDepBoardWithDetailsRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterCrs);
            Assert.Equal(restRequest.FilterType, soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetFastestDeparturesRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetFastestDeparturesRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetFastestDeparturesWithDetailsRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetFastestDeparturesWithDetailsRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetNextDeparturesRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetNextDeparturesRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetNextDeparturesWithDetailsRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetNextDeparturesWithDetailsRequest(restRequest);

            Assert.Equal("test-out", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(restRequest.TimeOffset, soapRequest.timeOffset);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrBoardWithDetailsStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrBoardWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrDepBoardWithDetailsStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrDepBoardWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrivalBoardStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrivalBoardStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetArrivalDepartureBoardStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetArrivalDepartureBoardStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetDepartureBoardStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetDepartureBoardStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetDepBoardWithDetailsStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetDepBoardWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetFastestDeparturesStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetFastestDeparturesStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetFastestDeparturesWithDetailsStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetFastestDeparturesWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetNextDeparturesStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetNextDeparturesStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        [Fact]
        public void MapperServiceMapsGetNextDeparturesWithDetailsStaffRequest()
        {
            var (mapperService, restRequest) = Arrange();

            var soapRequest = mapperService.MapGetNextDeparturesWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(TestDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
        }

        private (MapperService, StationBoardRequest) Arrange()
        {
            var tokenService = A.Fake<IAccessTokenService>();
            var crsService = A.Fake<ICrsService>();
            var dateTimeService = A.Fake<IDateTimeService>();
            var mapperService = new MapperService(A.Fake<ILogger<MapperService>>(),
                tokenService,
                crsService,
                dateTimeService);

            var restRequest = new StationBoardRequest
            {
                AccessToken = "test-in",
                Crs = "CIN",
                FilterCrs = "FIN",
                FilterType = FilterType.from,
                NumRows = 5,
                TimeOffset = -30,
                TimeWindow = 60,
            };

            A.CallTo(() => tokenService.MakeAccessToken(restRequest))
                .Returns(new AccessToken { TokenValue = "test-out" });
            A.CallTo(() => tokenService.MakeStaffAccessToken(restRequest))
                .Returns(new OpenLDBSVWS.AccessToken { TokenValue = "test-out-staff" });
            A.CallTo(() => crsService.MakeCrsCode("CIN")).Returns("COT");
            A.CallTo(() => crsService.MakeCrsCode("FIN")).Returns("FOT");
            A.CallTo(() => dateTimeService.LocalNow).Returns(TestDateTime);

            return (mapperService, restRequest);
        }

    }
}
