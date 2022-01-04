// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using OpenLDBWS;

namespace Huxley2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArrivalsController : ControllerBase
    {
        private readonly ILogger<ArrivalsController> _logger;
        private readonly IStationBoardService _stationBoardService;

        public ArrivalsController(
            ILogger<ArrivalsController> logger,
            IStationBoardService stationBoardService
            )
        {
            _logger = logger;
            _stationBoardService = stationBoardService;
        }

        [HttpGet]
        [Route("{crs}")]
        [Route("{crs}/{numRows}")]
        [Route("{crs}/{filterType}/{filterCrs}")]
        [Route("{crs}/{filterType}/{filterCrs}/{numRows}")]
        [ProducesResponseType(typeof(StationBoard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StationBoardWithDetails), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<BaseStationBoard> Get([FromRoute] StationBoardRequest request)
        {
            try
            {
                var clock = Stopwatch.StartNew();
                var board = await _stationBoardService.GetArrivalBoardAsync(request);
                clock.Stop();
                _logger.LogInformation("Open LDB API time {ElapsedMilliseconds:#,#}ms",
                    clock.ElapsedMilliseconds);

                var checksum = _stationBoardService.GenerateChecksum(board);
                Response.Headers[HeaderNames.ETag] = checksum;
                _logger.LogInformation($"ETag: {checksum}");

                return board;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Open LDB ArrivalBoard API call failed");
                throw;
            }
        }
    }
}
