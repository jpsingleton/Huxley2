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
    public class ServiceController : ControllerBase
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly IServiceDetailsService _serviceDetailsService;

        public ServiceController(
            ILogger<ServiceController> logger,
            IServiceDetailsService serviceDetailsService
            )
        {
            _logger = logger;
            _serviceDetailsService = serviceDetailsService;
        }

        [HttpGet]
        [Route("")]
        [Route("{serviceId}")]
        [ProducesResponseType(typeof(OpenLDBWS.ServiceDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OpenLDBSVWS.ServiceDetails), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<object> Get([FromRoute] ServiceRequest routeRequest,
                                      [FromQuery] ServiceRequest queryRequest)
        {
            try
            {
                // There is no [FromUri] in ASP.NET Core so we need to get from route or query
                // If both are set then use the query string parameter in preference to route
                var request =
                    string.IsNullOrWhiteSpace(queryRequest.ServiceId) ?
                    string.IsNullOrWhiteSpace(routeRequest.ServiceId) ?
                    throw new Exception("No Service ID provided") :
                    routeRequest :
                    queryRequest;

                var clock = Stopwatch.StartNew();
                var service = await _serviceDetailsService.GetServiceDetailsAsync(request);
                clock.Stop();
                _logger.LogInformation("Open LDB API time {ElapsedMilliseconds:#,#}ms",
                    clock.ElapsedMilliseconds);

                var checksum = _serviceDetailsService.GenerateChecksum(service);
                Response.Headers[HeaderNames.ETag] = checksum;
                _logger.LogInformation($"ETag: {checksum}");

                return service;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Open LDB ServiceDetails API call failed");
                throw;
            }
        }
    }
}
