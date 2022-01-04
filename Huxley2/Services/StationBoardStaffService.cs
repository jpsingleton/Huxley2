// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;

namespace Huxley2.Services
{
    public class StationBoardStaffService : IStationBoardStaffService
    {
        private readonly ILogger<StationBoardStaffService> _logger;
        private readonly IMapperService _mapperService;
        private readonly LDBSVServiceSoap _soapClient;

        public StationBoardStaffService(
            ILogger<StationBoardStaffService> logger,
            IMapperService mapperService,
            LDBSVServiceSoap soapClient
            )
        {
            _logger = logger;
            _mapperService = mapperService;
            _soapClient = soapClient;
        }

        public async Task<BaseStationBoard> GetDepartureBoardAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling staff departure board SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetDepBoardWithDetailsAsync(
                    _mapperService.MapGetDepBoardWithDetailsStaffRequest(request));
                return boardWithDetails.GetBoardWithDetailsResult;
            }
            var board = await _soapClient.GetDepartureBoardByCRSAsync(
                _mapperService.MapGetDepartureBoardStaffRequest(request));
            return board.GetBoardResult;
        }

        public async Task<BaseStationBoard> GetArrivalBoardAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling staff arrival board SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetArrBoardWithDetailsAsync(
                    _mapperService.MapGetArrBoardWithDetailsStaffRequest(request));
                return boardWithDetails.GetBoardWithDetailsResult;
            }
            var board = await _soapClient.GetArrivalBoardByCRSAsync(
                _mapperService.MapGetArrivalBoardStaffRequest(request));
            return board.GetBoardResult;
        }

        public async Task<BaseStationBoard> GetArrivalDepartureBoardAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling staff arrival departure board SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetArrDepBoardWithDetailsAsync(
                    _mapperService.MapGetArrDepBoardWithDetailsStaffRequest(request));
                return boardWithDetails.GetBoardWithDetailsResult;
            }
            var board = await _soapClient.GetArrivalDepartureBoardByCRSAsync(
                _mapperService.MapGetArrivalDepartureBoardStaffRequest(request));
            return board.GetBoardResult;
        }

        public async Task<BaseStationBoard> GetNextDeparturesAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling staff next departures SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetNextDeparturesWithDetailsAsync(
                    _mapperService.MapGetNextDeparturesWithDetailsStaffRequest(request));
                return boardWithDetails.DeparturesBoard;
            }
            var board = await _soapClient.GetNextDeparturesAsync(
                _mapperService.MapGetNextDeparturesStaffRequest(request));
            return board.DeparturesBoard;
        }

        public async Task<BaseStationBoard> GetFastestDeparturesAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling staff fastest departures SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetFastestDeparturesWithDetailsAsync(
                    _mapperService.MapGetFastestDeparturesWithDetailsStaffRequest(request));
                return boardWithDetails.DeparturesBoard;
            }
            var board = await _soapClient.GetFastestDeparturesAsync(
                _mapperService.MapGetFastestDeparturesStaffRequest(request));
            return board.DeparturesBoard;
        }

        public string GenerateChecksum(BaseStationBoard board) => ChecksumGenerator.GenerateChecksum(board);
    }
}
