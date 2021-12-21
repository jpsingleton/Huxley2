﻿// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Collections.Generic;
using System.Linq;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using OpenLDBWS;

namespace Huxley2.Services
{
    public class MapperService : IMapperService
    {
        // P for train Services, B for bus services, S for ship services.
        private static string STAFF_SERVICES_CODES = "PBS";

        private readonly ILogger<MapperService> _logger;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ICrsService _crsService;
        private readonly IDateTimeService _dateTimeService;

        public MapperService(
            ILogger<MapperService> logger,
            IAccessTokenService accessTokenService,
            ICrsService crsService,
            IDateTimeService dateTimeService
            )
        {
            _logger = logger;
            _accessTokenService = accessTokenService;
            _crsService = crsService;
            _dateTimeService = dateTimeService;
        }

        public OpenLDBWS.GetArrBoardWithDetailsRequest MapGetArrBoardWithDetailsRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetArrBoardWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterCrs = MakeFilterCrs(request.FilterCrs),
                filterType = request.FilterType,
                numRows = request.NumRows,
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetArrBoardWithDetailsRequest MapGetArrBoardWithDetailsStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetArrBoardWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filtercrs = MakeFilterCrs(request.FilterCrs),
                filterType = (OpenLDBSVWS.FilterType)request.FilterType,
                numRows = request.NumRows,
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public OpenLDBWS.GetArrDepBoardWithDetailsRequest MapGetArrDepBoardWithDetailsRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetArrDepBoardWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterCrs = MakeFilterCrs(request.FilterCrs),
                filterType = request.FilterType,
                numRows = request.NumRows,
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetArrDepBoardWithDetailsRequest MapGetArrDepBoardWithDetailsStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetArrDepBoardWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filtercrs = MakeFilterCrs(request.FilterCrs),
                filterType = (OpenLDBSVWS.FilterType)request.FilterType,
                numRows = request.NumRows,
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public GetArrivalBoardRequest MapGetArrivalBoardRequest(StationBoardRequest request)
        {
            return new GetArrivalBoardRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterCrs = MakeFilterCrs(request.FilterCrs),
                filterType = request.FilterType,
                numRows = request.NumRows,
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public GetArrivalBoardByCRSRequest MapGetArrivalBoardStaffRequest(StationBoardRequest request)
        {
            return new GetArrivalBoardByCRSRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filtercrs = MakeFilterCrs(request.FilterCrs),
                filterType = (OpenLDBSVWS.FilterType)request.FilterType,
                numRows = request.NumRows,
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public GetArrivalDepartureBoardRequest MapGetArrivalDepartureBoardRequest(StationBoardRequest request)
        {
            return new GetArrivalDepartureBoardRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterCrs = MakeFilterCrs(request.FilterCrs),
                filterType = request.FilterType,
                numRows = request.NumRows,
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public GetArrivalDepartureBoardByCRSRequest MapGetArrivalDepartureBoardStaffRequest(StationBoardRequest request)
        {
            return new GetArrivalDepartureBoardByCRSRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filtercrs = MakeFilterCrs(request.FilterCrs),
                filterType = (OpenLDBSVWS.FilterType)request.FilterType,
                numRows = request.NumRows,
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public GetDepartureBoardRequest MapGetDepartureBoardRequest(StationBoardRequest request)
        {
            return new GetDepartureBoardRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterCrs = MakeFilterCrs(request.FilterCrs),
                filterType = request.FilterType,
                numRows = request.NumRows,
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public GetDepartureBoardByCRSRequest MapGetDepartureBoardStaffRequest(StationBoardRequest request)
        {
            return new GetDepartureBoardByCRSRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filtercrs = MakeFilterCrs(request.FilterCrs),
                filterType = (OpenLDBSVWS.FilterType)request.FilterType,
                numRows = request.NumRows,
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public OpenLDBWS.GetDepBoardWithDetailsRequest MapGetDepBoardWithDetailsRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetDepBoardWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterCrs = MakeFilterCrs(request.FilterCrs),
                filterType = request.FilterType,
                numRows = request.NumRows,
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetDepBoardWithDetailsRequest MapGetDepBoardWithDetailsStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetDepBoardWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filtercrs = MakeFilterCrs(request.FilterCrs),
                filterType = (OpenLDBSVWS.FilterType)request.FilterType,
                numRows = request.NumRows,
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public OpenLDBWS.GetFastestDeparturesRequest MapGetFastestDeparturesRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetFastestDeparturesRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 15),
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetFastestDeparturesRequest MapGetFastestDeparturesStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetFastestDeparturesRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 15),
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public OpenLDBWS.GetFastestDeparturesWithDetailsRequest MapGetFastestDeparturesWithDetailsRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetFastestDeparturesWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 10),
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetFastestDeparturesWithDetailsRequest MapGetFastestDeparturesWithDetailsStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetFastestDeparturesWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 10),
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public OpenLDBWS.GetNextDeparturesRequest MapGetNextDeparturesRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetNextDeparturesRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 25),
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetNextDeparturesRequest MapGetNextDeparturesStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetNextDeparturesRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 25),
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        public OpenLDBWS.GetNextDeparturesWithDetailsRequest MapGetNextDeparturesWithDetailsRequest(StationBoardRequest request)
        {
            return new OpenLDBWS.GetNextDeparturesWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 10),
                timeOffset = request.TimeOffset,
                timeWindow = request.TimeWindow,
            };
        }

        public OpenLDBSVWS.GetNextDeparturesWithDetailsRequest MapGetNextDeparturesWithDetailsStaffRequest(StationBoardRequest request)
        {
            return new OpenLDBSVWS.GetNextDeparturesWithDetailsRequest
            {
                AccessToken = _accessTokenService.MakeStaffAccessToken(request),
                crs = _crsService.MakeCrsCode(request.Crs),
                filterList = MakeFilterList(request.FilterList, 10),
                time = _dateTimeService.LocalNow.AddMinutes(request.TimeOffset), // local - not UTC
                timeWindow = (ushort)request.TimeWindow, // max 1440mins (24hrs)
                services = STAFF_SERVICES_CODES,
            };
        }

        private string? MakeFilterCrs(string? filterCrs)
        {
            return string.IsNullOrWhiteSpace(filterCrs) ? null : _crsService.MakeCrsCode(filterCrs);
        }

        private string[] MakeFilterList(IEnumerable<string> filterList, int maxLength)
        {
            if (!filterList.Any())
            {
                throw new Exception("At least 1 filter CRS code required");
            }
            // If filterList exceeds max length then API call will fail
            if (filterList.Count() > maxLength)
            {
                _logger.LogWarning($"Filter list truncated to {maxLength}");
            }
            return filterList.Select(_crsService.MakeCrsCode).Take(maxLength).ToArray();
        }

    }
}
