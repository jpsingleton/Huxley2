// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using CsvHelper;
using Huxley2.Extensions;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenLDBSVWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Huxley2.Services
{
    public class CrsService : ICrsService
    {
        private readonly ILogger<CrsService> _logger;
        private readonly IConfiguration _config;
        private readonly IAccessTokenService _accessTokenService;
        private readonly LDBSVRefServiceSoap _refClient;
        private readonly HttpClient _httpClient;

        private bool _crsRecordsLoaded = false;
        private Dictionary<string, string> _crsRecordsByCode = new Dictionary<string, string>();
        private Dictionary<string, string> _crsRecordsByName = new Dictionary<string, string>();
        private IEnumerable<CrsRecord> _londonTerminals = new List<CrsRecord>();

        public CrsService(
            ILogger<CrsService> logger,
            IConfiguration config,
            IAccessTokenService accessTokenService,
            LDBSVRefServiceSoap refClient,
            HttpClient httpClient
            )
        {
            _logger = logger;
            _config = config;
            _accessTokenService = accessTokenService;
            _refClient = refClient;
            _httpClient = httpClient;
        }

        public IEnumerable<CrsRecord> GetLondonTerminals()
        {
            return _londonTerminals;
        }

        public IEnumerable<CrsRecord> GetStations(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return _crsRecordsByCode.Select(c => new CrsRecord { CrsCode = c.Key, StationName = c.Value })
                    .OrderBy(c => c.StationName);
            }
            if (query.NotNullAndEquals("London Terminals"))
            {
                return _londonTerminals;
            }
            return _crsRecordsByCode.Where(c => c.Value.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0)
                .Select(c => new CrsRecord { CrsCode = c.Key, StationName = c.Value })
                .OrderBy(c => c.StationName);
        }

        public string MakeCrsCode(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return string.Empty;
            }
            if (!_crsRecordsLoaded)
            {
                _logger.LogWarning("CRS codes not preloaded - attempting to load now");
                // Don't block / await on this - run in background and return query
                _ = LoadCrsCodes();
                return query;
            }

            // If query is in the CRS code records return it
            if (_crsRecordsByCode.TryGetValue(query, out _))
            {
                return query;
            }
            // If query matches a single station name return the code
            if (_crsRecordsByName.TryGetValue(query, out var crs))
            {
                return crs;
            }

            // If query partially matches a single station name return the first code
            // Query should be upper case already but just in case (also catches for above)
            // If no match then return query as is
            return _crsRecordsByName.FirstOrDefault(c =>
                c.Key.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0).Value
                ?? query;
        }

        public async Task LoadCrsCodes()
        {
            if (_crsRecordsLoaded)
            {
                return;
            }

            // Prefer staff reference service as it's a better source for CRS codes
            // There are fewer codes in the CSV and some don't work (e.g. ASI, SPX)
            if (_accessTokenService.TryMakeStaffAccessToken(out var accessToken))
            {
                try
                {
                    await GetCrsCodesFromStaffApi(accessToken);
                }
                catch (CrsServiceException)
                {
                    // Swallow this exception as we try the CSV download next
                }
            }
            if (!_crsRecordsLoaded)
            {
                try
                {
                    await GetCrsCodesFromCsvDownload();
                }
                catch (CrsServiceException)
                {
                    // A failure here means we can't proceed
                    return;
                }
            }
            if (!_crsRecordsLoaded)
            {
                // If we couldn't load the records then there is no point in processing them
                return;
            }

            try
            {
                ProcessCrsCodes();
            }
            catch (CrsServiceException)
            {
                // This method shouldn't throw exceptions as it may run in the background
            }
        }

        private async Task GetCrsCodesFromStaffApi(AccessToken accessToken)
        {
            _logger.LogInformation("Loading station list from staff API");
            try
            {
                var request = new GetStationListRequest
                {
                    AccessToken = accessToken,
                };
                var response = await _refClient.GetStationListAsync(request);
                var stationRefData = response.GetStationListResult;
                // CRS should be upper case already but just in case
                _crsRecordsByCode = stationRefData?.StationList?.ToDictionary(
                        s => s.crs.ToUpperInvariant().Trim(),
                        s => s.Value)
                    ?? new Dictionary<string, string>();
                _crsRecordsLoaded = _crsRecordsByCode.Count > 0;
            }
            catch (Exception e) when (
                e is FaultException ||
                e is EndpointNotFoundException ||
                e is HttpRequestException ||
                e is SocketException
                )
            {
                _logger.LogWarning(e, "Failed to load station list from staff API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load station list from staff API");
                throw new CrsServiceException(
                    "The CRS service failed to load the station list from the staff API.", ex);
            }
        }

        private async Task GetCrsCodesFromCsvDownload()
        {
            _logger.LogInformation("Loading station list from CSV download");
            try
            {
                var stations = new Dictionary<string, string>();
                var csvUri = new Uri(_config["StationCodesCsvUrl"]);
                var stream = await _httpClient.GetStreamAsync(csvUri);
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var name = csv.GetField<string>(0);
                    var code = csv.GetField<string>(1);
                    stations.Add(code.ToUpperInvariant().Trim(), name.Trim());
                }
                _crsRecordsByCode = stations;
                _crsRecordsLoaded = _crsRecordsByCode.Count > 0;
            }
            catch (Exception e) when (
                e is HttpRequestException ||
                e is SocketException
                )
            {
                _logger.LogWarning(e, "Failed to load station list from CSV download");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load station list from CSV download");
                throw new CrsServiceException(
                    "The CRS service failed to load the station list from the CSV file download.", ex);
            }
        }

        private void ProcessCrsCodes()
        {
            try
            {
                _crsRecordsByName = _crsRecordsByCode.ToDictionary(
                        s => s.Value.ToUpperInvariant().Trim(),
                        s => s.Key)
                    ?? new Dictionary<string, string>();

                var count = Math.Min(_crsRecordsByCode.Count, _crsRecordsByName.Count);
                if (count != _crsRecordsByCode.Count || count != _crsRecordsByName.Count)
                {
                    _logger.LogWarning("CRS code counts do not match");
                }
                if (count < 1)
                {
                    _logger.LogWarning("CRS codes not loaded");
                }

                _logger.LogInformation($"{count} CRS codes loaded");

                // Set London Terminals from codes
                // https://www.nationalrail.co.uk/times_fares/ticket_types/46587.aspx#terminals
                var lTermCrs = new[] { "BFR", "CST", "CHX", "CTK", "EUS", "FST", "KGX", "LST",
                "LBG", "MYB", "MOG", "OLD", "PAD", "STP", "VXH", "VIC", "WAT", "WAE", };
                _londonTerminals = _crsRecordsByCode.Where(c => lTermCrs.Contains(c.Key))
                    .Select(c => new CrsRecord { CrsCode = c.Key, StationName = c.Value, })
                    .OrderBy(c => c.StationName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process station list");
                throw new CrsServiceException(
                    "The CRS service failed to process the station list.", ex);
            }
        }

    }
}
