// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Huxley2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using Microsoft.AspNetCore.Http;
using Huxley2.Interfaces;

namespace Huxley2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffFastestController : ControllerBase
    {
        private readonly ILogger<StaffFastestController> _logger;
        private readonly IStationBoardStaffService _stationBoardService;

        public StaffFastestController(
            ILogger<StaffFastestController> logger,
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
                var board = await _stationBoardService.GetFastestDeparturesAsync(request);
                clock.Stop();
                _logger.LogInformation("Open LDB API time {ElapsedMilliseconds:#,#}ms",
                    clock.ElapsedMilliseconds);

                return board;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Open LDB FastestDepartures API call failed");
                throw;
            }
        }
    }
}
