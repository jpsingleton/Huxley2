// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Collections.Generic;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Huxley2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrsController : ControllerBase
    {
        private readonly ILogger<CrsController> _logger;
        private readonly ICrsService _crsService;

        public CrsController(
            ILogger<CrsController> logger,
            ICrsService crsService
            )
        {
            _logger = logger;
            _crsService = crsService;
        }

        [HttpGet]
        [Route("")]
        [Route("{query}")]
        [ProducesResponseType(typeof(IEnumerable<CrsRecord>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IEnumerable<CrsRecord> Get([FromRoute] string? query)
        {
            _logger.LogInformation($"Getting stations for query: {query}");
            return _crsService.GetStations(query);
        }
    }
}
