// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using OpenLDBWS;
using System;
using System.Threading.Tasks;

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
            // The (non-staff) Darwin API requires service ID to be a standard base 64 string
            // As it's simply a GUID in base 64 there'll always be maximum padding (== suffix)
            // %2F will still be escaped if from the route and not from the query but nothing else will
            // https://docs.microsoft.com/en-us/aspnet/core/web-api/#binding-source-parameter-inference
            request.ServiceId = request.ServiceId.Replace("%2F", "/", StringComparison.OrdinalIgnoreCase);

            if (request.ServiceId.Length == 24)
            {
                if (Convert.TryFromBase64String(request.ServiceId, new byte[16], out var length) && length == 16)
                {
                    _logger.LogInformation($"Calling service details SOAP endpoint for {request.ServiceId}");
                    var s = await _soapClient.GetServiceDetailsAsync(new GetServiceDetailsRequest
                    {
                        AccessToken = _accessTokenService.MakeAccessToken(request),
                        serviceID = request.ServiceId,
                    });
                    return s.GetServiceDetailsResult;
                }
            }

            // If ID looks like a RID (15 decimal digit long base 10 integer) then use the staff API if configured
            // This appears to be the date and the UID (with the first character in decimal representation)
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

            // We also accept the standard hexadecimal (base 16) GUID representation
            if (Guid.TryParse(request.ServiceId, out Guid sid))
            {
                request.ServiceId = Convert.ToBase64String(sid.ToByteArray());
            }

            // Support URL safe base 64 encoding as it's more suitable for this situation
            // https://en.wikipedia.org/wiki/Base64#URL_applications
            // https://tools.ietf.org/html/rfc4648#section-5
            // Encoder available as part of ASP.NET Core: Microsoft.Extensions.WebEncoders
            // For more info read ASP.NET Core 2 High Performance (https://unop.uk/book) :)

            if (request.ServiceId.Length == 22)
            {
                var sidBytes = WebEncoders.Base64UrlDecode(request.ServiceId);
                if (sidBytes.Length == 16)
                {
                    request.ServiceId = Convert.ToBase64String(sidBytes);
                }
            }

            // If ID wasn't percent-encoded then it may be missing / + =
            // We try to fix it up if it isn't the correct length
            while (!request.ServiceId.EndsWith("==", StringComparison.OrdinalIgnoreCase))
            {
                request.ServiceId += "=";
            }
            while (request.ServiceId.Length < 24)
            {
                request.ServiceId = "/" + request.ServiceId;
            }

            _logger.LogInformation($"Calling service details SOAP endpoint for {request.ServiceId}");
            var service = await _soapClient.GetServiceDetailsAsync(new GetServiceDetailsRequest
            {
                AccessToken = _accessTokenService.MakeAccessToken(request),
                serviceID = request.ServiceId,
            });
            return service.GetServiceDetailsResult;
        }
    }
}
