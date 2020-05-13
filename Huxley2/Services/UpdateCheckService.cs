// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Huxley2.Services
{
    public class UpdateCheckService : IUpdateCheckService
    {
        private readonly ILogger<UpdateCheckService> _logger;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public UpdateCheckService(
            ILogger<UpdateCheckService> logger,
            IConfiguration config,
            HttpClient httpClient
            )
        {
            _logger = logger;
            _config = config;
            _httpClient = httpClient;
        }

        public bool UpdateAvailable { get; private set; }

        public string CurrentVersion { get; } = Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? string.Empty;

        public string AvailableVersion { get; private set; } = string.Empty;

        public Uri? UpdateUrl { get; private set; }

        public async Task CheckForUpdates()
        {
            _logger.LogInformation("Loading update settings");
            var updateCheckUrl = _config["UpdateCheckUrl"];
            if (!bool.TryParse(_config["EnableUpdateCheck"], out var enableUpdateCheck) ||
                !bool.TryParse(_config["UpdateCheckStableOnly"], out var stableOnly) ||
                !enableUpdateCheck || string.IsNullOrWhiteSpace(updateCheckUrl))
            {
                _logger.LogInformation("Update check disabled");
                return;
            }

            try
            {
                var response = await _httpClient.GetAsync(new Uri(updateCheckUrl));
                response.EnsureSuccessStatusCode();
                var versionUpdates = await response.Content.ReadAsAsync<IEnumerable<VersionUpdate>>();

                var versionUpdate = versionUpdates
                    .Where(v => !stableOnly || !v.Version.Contains('-', StringComparison.OrdinalIgnoreCase))
                    .LastOrDefault();

                if (versionUpdate != null)
                {
                    AvailableVersion = versionUpdate.Version;
                    UpdateUrl = new Uri(versionUpdate.Link);
                }

                _logger.LogInformation($"Current version {CurrentVersion}, available version {AvailableVersion}");

                UpdateAvailable = !string.IsNullOrWhiteSpace(AvailableVersion) && AvailableVersion != CurrentVersion;

                if (UpdateAvailable) _logger.LogInformation("Update is available");
            }
            catch (HttpRequestException hre)
            {
                _logger.LogWarning(hre, "Update check request failed");
            }
            catch (Exception e) when (
                e is NullReferenceException ||
                e is ArgumentNullException ||
                e is UriFormatException
                )
            {
                _logger.LogError(e, "Update check failed due to bad data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update check failed");
                throw new UpdateCheckServiceException("The update check failed.", ex);
            }
        }
    }
}
