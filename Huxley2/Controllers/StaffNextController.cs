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
using OpenLDBSVWS;

namespace Huxley2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffNextController : ControllerBase
    {
        private readonly ILogger<StaffNextController> _logger;
        private readonly IStationBoardStaffService _stationBoardService;

        public StaffNextController(
            ILogger<StaffNextController> logger,
            IStationBoardStaffService stationBoardService
            )
        {
            _logger = logger;
            _stationBoardService = stationBoardService;
        }

        [HttpGet]
        [Route("{crs}/{filterType}/{filterCrs}")]
        [ProducesResponseType(typeof(DeparturesBoard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeparturesBoardWithDetails), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<BaseStationBoard> Get([FromRoute] StationBoardRequest request)
        {
            try
            {
                var clock = Stopwatch.StartNew();
                var board = await _stationBoardService.GetNextDeparturesAsync(request);
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
                _logger.LogError(e, "Open LDB NextDepartures API call failed");
                throw;
            }
        }
    }
}
