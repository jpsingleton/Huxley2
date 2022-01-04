// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Interfaces;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using Xunit;

namespace Huxley2Tests.Services
{
    public class DelaysServiceTests
    {
        private DelaysService service;
        private StationBoardRequest request;
        private IStationBoardService stationBoardService;
        private StationBoardWithDetails board;

        public DelaysServiceTests()
        {
            request = new StationBoardRequest
            {
                Crs = "ABC",
                FilterType = FilterType.to,
                FilterCrs = "XYZ",
                NumRows = 5,
            };
            stationBoardService = A.Fake<IStationBoardService>();
            board = new StationBoardWithDetails
            {
                generatedAt = DateTime.Now,
                crs = "ABC",
                locationName = "Aa Bb Cc",
                filtercrs = "XYZ",
                filterLocationName = "Xx Yy Zz",
                filterType = FilterType.to,
            };
            A.CallTo(() => stationBoardService.GetArrivalDepartureBoardAsync(request)).Returns(board);
            A.CallTo(() => stationBoardService.GetArrivalBoardAsync(request)).Returns(board);
            A.CallTo(() => stationBoardService.GetDepartureBoardAsync(request)).Returns(board);
            A.CallTo(() => stationBoardService.GetFastestDeparturesAsync(request)).Returns(board);
            A.CallTo(() => stationBoardService.GetNextDeparturesAsync(request)).Returns(board);
            service = new DelaysService(
                A.Fake<ILogger<DelaysService>>(),
                stationBoardService,
                A.Fake<ICrsService>(),
                A.Fake<IConfiguration>(),
                A.Fake<IDateTimeService>());
        }

        [Fact]
        public void DelaysServiceLoadsDelayMinutesThreshold()
        {
            var config = A.Fake<IConfiguration>();
            config["DelayMinutesThreshold"] = "7";
            var service = new DelaysService(
                A.Fake<ILogger<DelaysService>>(),
                stationBoardService,
                A.Fake<ICrsService>(),
                config,
                A.Fake<IDateTimeService>()
            );

            Assert.Equal(7, service.DelayMinutesThreshold);
        }

        [Fact]
        public void DelaysServiceLoadsDefaultDelayMinutesThreshold()
        {
            Assert.Equal(5, service.DelayMinutesThreshold);
        }

        [Fact]
        public async Task DelaysServiceReturnsDelaysResponse()
        {
            var response = await service.GetDelaysAsync(request);

            Assert.Equal(board.generatedAt, response.GeneratedAt);
            Assert.Equal(board.crs, response.Crs);
            Assert.Equal(board.locationName, response.LocationName);
            Assert.Equal(board.filtercrs, response.Filtercrs);
            Assert.Equal(board.filterLocationName, response.FilterLocationName);
            Assert.Equal(board.filterType, response.FilterType);
            Assert.False(response.Delays);
            Assert.Equal(0, response.TotalTrainsDelayed);
            Assert.Equal(0, response.TotalDelayMinutes);
            Assert.Equal(0, response.TotalTrains);
            Assert.IsType<List<ServiceItem>>(response.DelayedTrains);
            Assert.Empty(response.DelayedTrains);
        }

        [Fact]
        public async Task DelaysServiceReturnsDelaysResponseWithLateTrains()
        {
            board.trainServices = new ServiceItemWithCallingPoints[]{
                new ServiceItemWithCallingPoints{
                    std = "22:11",
                    etd = "22:22",
                },
                new ServiceItemWithCallingPoints{
                    std = "22:12",
                    etd = "*22:22",
                },
                new ServiceItemWithCallingPoints{
                    std = "22:13",
                    etd = "On time",
                },
            };

            var response = await service.GetDelaysAsync(request);

            Assert.Equal(board.generatedAt, response.GeneratedAt);
            Assert.Equal(board.crs, response.Crs);
            Assert.Equal(board.locationName, response.LocationName);
            Assert.Equal(board.filtercrs, response.Filtercrs);
            Assert.Equal(board.filterLocationName, response.FilterLocationName);
            Assert.Equal(board.filterType, response.FilterType);
            Assert.True(response.Delays);
            Assert.Equal(2, response.TotalTrainsDelayed);
            Assert.Equal(21, response.TotalDelayMinutes);
            Assert.Equal(3, response.TotalTrains);
            Assert.IsType<List<ServiceItem>>(response.DelayedTrains);
            Assert.NotEmpty(response.DelayedTrains);
        }

        [Fact]
        public async Task DelaysServiceReturnsDelaysResponseWithDelayedTrains()
        {
            board.trainServices = new ServiceItemWithCallingPoints[]{
                new ServiceItemWithCallingPoints{
                    eta = "Delayed",
                },
            };

            var response = await service.GetDelaysAsync(request);

            Assert.Equal(board.generatedAt, response.GeneratedAt);
            Assert.Equal(board.crs, response.Crs);
            Assert.Equal(board.locationName, response.LocationName);
            Assert.Equal(board.filtercrs, response.Filtercrs);
            Assert.Equal(board.filterLocationName, response.FilterLocationName);
            Assert.Equal(board.filterType, response.FilterType);
            Assert.True(response.Delays);
            Assert.Equal(1, response.TotalTrainsDelayed);
            Assert.Equal(0, response.TotalDelayMinutes);
            Assert.Equal(1, response.TotalTrains);
            Assert.IsType<List<ServiceItem>>(response.DelayedTrains);
            Assert.NotEmpty(response.DelayedTrains);
        }

        [Fact]
        public async Task DelaysServiceReturnsDelaysResponseWithCanceledTrains()
        {
            board.trainServices = new ServiceItemWithCallingPoints[]{
                new ServiceItemWithCallingPoints{
                    eta = "Canceled",
                },
                new ServiceItemWithCallingPoints{
                    isCancelled = true,
                },
                new ServiceItemWithCallingPoints{
                    filterLocationCancelled = true,
                },
            };

            var response = await service.GetDelaysAsync(request);

            Assert.Equal(board.generatedAt, response.GeneratedAt);
            Assert.Equal(board.crs, response.Crs);
            Assert.Equal(board.locationName, response.LocationName);
            Assert.Equal(board.filtercrs, response.Filtercrs);
            Assert.Equal(board.filterLocationName, response.FilterLocationName);
            Assert.Equal(board.filterType, response.FilterType);
            Assert.True(response.Delays);
            Assert.Equal(3, response.TotalTrainsDelayed);
            Assert.Equal(0, response.TotalDelayMinutes);
            Assert.Equal(3, response.TotalTrains);
            Assert.IsType<List<ServiceItem>>(response.DelayedTrains);
            Assert.NotEmpty(response.DelayedTrains);
        }

        [Fact]
        public void DelaysServiceGeneratesChecksum()
        {
            var result = new DelaysResponse();

            var checksum = service.GenerateChecksum(result);

            Assert.Equal("\"h1Quz4RRyCkArd8q0EaKIlYAgTONOMaxYGd0uLwxIJI\"", checksum);
        }

        [Fact]
        public void DelaysServiceGeneratesChecksumIgnoringGeneratedAt()
        {
            var result = new DelaysResponse { GeneratedAt = DateTime.Now };

            var checksum = service.GenerateChecksum(result);

            Assert.Equal("\"h1Quz4RRyCkArd8q0EaKIlYAgTONOMaxYGd0uLwxIJI\"", checksum);

            result.GeneratedAt = DateTime.Now.AddMinutes(5);

            checksum = service.GenerateChecksum(result);

            Assert.Equal("\"h1Quz4RRyCkArd8q0EaKIlYAgTONOMaxYGd0uLwxIJI\"", checksum);
        }

        [Fact]
        public void DelaysServiceGeneratesChecksumPreservingGeneratedAt()
        {
            var now = DateTime.Now;
            var result = new DelaysResponse { GeneratedAt = now };

            service.GenerateChecksum(result);

            Assert.Equal(now, result.GeneratedAt);
        }

    }
}
