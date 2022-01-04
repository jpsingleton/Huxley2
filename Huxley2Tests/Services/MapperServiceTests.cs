// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Globalization;
using System.Linq;
using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using Xunit;

namespace Huxley2Tests.Services
{
    public class MapperServiceTests
    {
        private DateTime testDateTime;
        private MapperService mapperService;
        private StationBoardRequest restRequest;
        private string staffServicesCodes;

        public MapperServiceTests()
        {
            testDateTime = DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture);

            // P for train Services, B for bus services, S for ship services.
            staffServicesCodes = "PBS";

            var tokenService = A.Fake<IAccessTokenService>();
            var crsService = A.Fake<ICrsService>();
            var dateTimeService = A.Fake<IDateTimeService>();
            mapperService = new MapperService(A.Fake<ILogger<MapperService>>(),
                tokenService,
                crsService,
                dateTimeService);

            restRequest = new StationBoardRequest
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
            A.CallTo(() => dateTimeService.LocalNow).Returns(testDateTime);
        }

        [Fact]
        public void MapperServiceMapsGetArrBoardWithDetailsRequest()
        {
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
            var soapRequest = mapperService.MapGetArrBoardWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetArrDepBoardWithDetailsStaffRequest()
        {
            var soapRequest = mapperService.MapGetArrDepBoardWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetArrivalBoardStaffRequest()
        {
            var soapRequest = mapperService.MapGetArrivalBoardStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetArrivalDepartureBoardStaffRequest()
        {
            var soapRequest = mapperService.MapGetArrivalDepartureBoardStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetDepartureBoardStaffRequest()
        {
            var soapRequest = mapperService.MapGetDepartureBoardStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetDepBoardWithDetailsStaffRequest()
        {
            var soapRequest = mapperService.MapGetDepBoardWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filtercrs);
            Assert.Equal(restRequest.FilterType, (FilterType)soapRequest.filterType);
            Assert.Equal(restRequest.NumRows, soapRequest.numRows);
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetFastestDeparturesStaffRequest()
        {
            var soapRequest = mapperService.MapGetFastestDeparturesStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetFastestDeparturesWithDetailsStaffRequest()
        {
            var soapRequest = mapperService.MapGetFastestDeparturesWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetNextDeparturesStaffRequest()
        {
            var soapRequest = mapperService.MapGetNextDeparturesStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

        [Fact]
        public void MapperServiceMapsGetNextDeparturesWithDetailsStaffRequest()
        {
            var soapRequest = mapperService.MapGetNextDeparturesWithDetailsStaffRequest(restRequest);

            Assert.Equal("test-out-staff", soapRequest.AccessToken.TokenValue);
            Assert.Equal("COT", soapRequest.crs);
            Assert.Equal("FOT", soapRequest.filterList.Single());
            Assert.Equal(testDateTime.AddMinutes(restRequest.TimeOffset), soapRequest.time);
            Assert.Equal(restRequest.TimeWindow, soapRequest.timeWindow);
            Assert.Equal(staffServicesCodes, soapRequest.services);
        }

    }
}
