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

namespace Huxley2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DelaysController : ControllerBase
    {
        private readonly ILogger<DelaysController> _logger;
        private readonly IDelaysService _delaysService;

        public DelaysController(
            ILogger<DelaysController> logger,
            IDelaysService delaysService
            )
        {
            _logger = logger;
            _delaysService = delaysService;
        }

        [HttpGet]
        [Route("{crs}")]
        [Route("{crs}/{numRows}")]
        [Route("{crs}/{filterType}/{filterCrs}")]
        [Route("{crs}/{filterType}/{filterCrs}/{numRows}")]
        [Route("{crs}/{filterType}/{filterCrs}/{numRows}/{std}")]
        [ProducesResponseType(typeof(DelaysResponse), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)] // 1 min
        public async Task<DelaysResponse> Get([FromRoute] StationBoardRequest request)
        {
            try
            {
                var clock = Stopwatch.StartNew();
                var board = await _delaysService.GetDelaysAsync(request);
                clock.Stop();
                _logger.LogInformation("Open LDB API time {ElapsedMilliseconds:#,#}ms",
                    clock.ElapsedMilliseconds);

                var checksum = _delaysService.GenerateChecksum(board);
                Response.Headers[HeaderNames.ETag] = checksum;
                _logger.LogInformation($"ETag: {checksum}");

                return board;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Open LDB ArrDepBoardWithDetails API call or delay calculation failed");
                throw;
            }
        }
    }
}
