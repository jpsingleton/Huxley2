// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System.Threading.Tasks;

namespace Huxley2.Services
{
    public class StationBoardService : IStationBoardService
    {
        private readonly ILogger<StationBoardService> _logger;
        private readonly IMapperService _mapperService;
        private readonly LDBServiceSoap _soapClient;

        public StationBoardService(
            ILogger<StationBoardService> logger,
            IMapperService mapperService,
            LDBServiceSoap soapClient
        )
        {
            _logger = logger;
            _mapperService = mapperService;
            _soapClient = soapClient;
        }

        public async Task<BaseStationBoard> GetDepartureBoardAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling departure board SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetDepBoardWithDetailsAsync(
                    _mapperService.MapGetDepBoardWithDetailsRequest(request));
                return boardWithDetails.GetStationBoardResult;
            }

            var board = await _soapClient.GetDepartureBoardAsync(
                _mapperService.MapGetDepartureBoardRequest(request));
            return board.GetStationBoardResult;
        }

        public async Task<BaseStationBoard> GetArrivalBoardAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling arrival board SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetArrBoardWithDetailsAsync(
                    _mapperService.MapGetArrBoardWithDetailsRequest(request));
                return boardWithDetails.GetStationBoardResult;
            }

            var board = await _soapClient.GetArrivalBoardAsync(
                _mapperService.MapGetArrivalBoardRequest(request));
            return board.GetStationBoardResult;
        }

        public async Task<BaseStationBoard> GetArrivalDepartureBoardAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling arrival departure board SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetArrDepBoardWithDetailsAsync(
                    _mapperService.MapGetArrDepBoardWithDetailsRequest(request));
                return boardWithDetails.GetStationBoardResult;
            }

            var board = await _soapClient.GetArrivalDepartureBoardAsync(
                _mapperService.MapGetArrivalDepartureBoardRequest(request));
            return board.GetStationBoardResult;
        }

        public async Task<BaseStationBoard> GetNextDeparturesAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling next departures SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetNextDeparturesWithDetailsAsync(
                    _mapperService.MapGetNextDeparturesWithDetailsRequest(request));
                return boardWithDetails.DeparturesBoard;
            }

            var board = await _soapClient.GetNextDeparturesAsync(
                _mapperService.MapGetNextDeparturesRequest(request));
            return board.DeparturesBoard;
        }

        public async Task<BaseStationBoard> GetFastestDeparturesAsync(StationBoardRequest request)
        {
            _logger.LogInformation($"Calling fastest departures SOAP endpoint for {request.Crs}");
            if (request.Expand)
            {
                var boardWithDetails = await _soapClient.GetFastestDeparturesWithDetailsAsync(
                    _mapperService.MapGetFastestDeparturesWithDetailsRequest(request));
                return boardWithDetails.DeparturesBoard;
            }

            var board = await _soapClient.GetFastestDeparturesAsync(
                _mapperService.MapGetFastestDeparturesRequest(request));
            return board.DeparturesBoard;
        }
    }
}
