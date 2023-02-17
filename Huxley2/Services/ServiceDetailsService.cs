// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Threading.Tasks;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using OpenLDBWS;

namespace Huxley2.Services
{
    public class ServiceDetailsService : IServiceDetailsService
    {
        private readonly ILogger<ServiceDetailsService> _logger;
        private readonly IAccessTokenService _accessTokenService;
        private readonly LDBServiceSoap _soapClient;
        private readonly LDBSVServiceSoap _staffSoapClient;

        public ServiceDetailsService(
            ILogger<ServiceDetailsService> logger,
            IAccessTokenService accessTokenService,
            LDBServiceSoap soapClient,
            LDBSVServiceSoap staffSoapClient
            )
        {
            _logger = logger;
            _accessTokenService = accessTokenService;
            _soapClient = soapClient;
            _staffSoapClient = staffSoapClient;
        }

        public async Task<object> GetServiceDetailsAsync(ServiceRequest request)
        {
            // Staff API
            // Formatted as a 15 digit base-10 long
            // This appears to be the date and the UID (with the first character in decimal representation)

            // use the staff API if configured
            if (request.ServiceId.Length == 15 && long.TryParse(request.ServiceId, out _)
                && _accessTokenService.TryMakeStaffAccessToken(out var staffToken))
            {
                _logger.LogInformation($"Calling staff service details SOAP endpoint for {request.ServiceId}");
                var staffService = await _staffSoapClient.GetServiceDetailsByRIDAsync(
                    new GetServiceDetailsByRIDRequest
                    {
                        AccessToken = staffToken,
                        rid = request.ServiceId,
                    });
                return staffService.GetServiceDetailsResult;
            }


            // Public API
            // 7 digit base-10 number and an up-to-8 character TIPLOC,
            // padded with underscore characters

            // We also accept the standard hexadecimal (base 16) GUID representation
            if (Guid.TryParse(request.ServiceId, out Guid sid))
            {
                // use new GUID parsing method
                request.ServiceId = OpenLDBWS.BaseServiceItem.FromGuid(sid);
            }

            // Support URL safe base 64 encoding as it's more suitable for this situation
            // https://en.wikipedia.org/wiki/Base64#The_URL_applications
            // https://tools.ietf.org/html/rfc4648#section-5
            // Encoder available as part of ASP.NET Core: Microsoft.Extensions.WebEncoders
            // For more info read ASP.NET Core 2 High Performance (https://unop.uk/book) :)
            if (request.ServiceId.Length == 20)
            {
                var sidBytes = WebEncoders.Base64UrlDecode(request.ServiceId);
                if (sidBytes.Length == 15)
                {
                    request.ServiceId = System.Text.Encoding.UTF8.GetString(sidBytes);
                }
            }

            _logger.LogInformation($"Calling service details SOAP endpoint for {request.ServiceId}");
            var service = await _soapClient.GetServiceDetailsAsync(new GetServiceDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                serviceID = request.ServiceId,
            });
            return service.GetServiceDetailsResult;
        }

        public string GenerateChecksum(object service) => ChecksumGenerator.GenerateChecksum(service);
    }
}
