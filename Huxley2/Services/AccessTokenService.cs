// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ServiceModel.Security;

namespace Huxley2.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ILogger<AccessTokenService> _logger;
        private readonly IConfiguration _config;

        private readonly string _darwinAccessToken;
        private readonly string _darwinStaffAccessToken;
        private readonly string _clientAccessToken;

        public AccessTokenService(
            ILogger<AccessTokenService> logger,
            IConfiguration config
            )
        {
            _logger = logger;
            _config = config;

            _logger.LogInformation("Loading access tokens from settings");
            _darwinAccessToken = _config["DarwinAccessToken"];
            _darwinStaffAccessToken = _config["DarwinStaffAccessToken"];
            _clientAccessToken = _config["ClientAccessToken"];
        }

        public OpenLDBWS.AccessToken MakeAccessToken(BaseRequest request)
        {
            return new OpenLDBWS.AccessToken
            {
                TokenValue = MakeToken(request, _darwinAccessToken)
            };
        }

        public OpenLDBSVWS.AccessToken MakeStaffAccessToken(BaseRequest request)
        {
            return new OpenLDBSVWS.AccessToken
            {
                TokenValue = MakeToken(request, _darwinStaffAccessToken)
            };
        }

        public bool TryMakeStaffAccessToken(out OpenLDBSVWS.AccessToken accessToken)
        {
            accessToken = new OpenLDBSVWS.AccessToken();
            if (ValidToken(_darwinStaffAccessToken))
            {
                accessToken.TokenValue = _darwinStaffAccessToken;
                return true;
            }
            return false;
        }

        private string MakeToken(BaseRequest request, string configuredToken)
        {
            var token = _clientAccessToken == request.AccessToken ?
                configuredToken : request.AccessToken;

            if (ValidToken(token))
            {
                return token;
            }
            _logger.LogError("Darwin access token not in settings, provided in URL or a valid GUID. ");
            throw new MessageSecurityException("Access token not configured or provided on request. ");
        }

        private static bool ValidToken(string token)
        {
            return !string.IsNullOrWhiteSpace(token) && Guid.TryParse(token, out _);
        }
    }
}
