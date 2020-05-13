// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Extensions;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Huxley2.Services
{
    public class DelaysService : IDelaysService
    {
        private readonly ILogger<DelaysService> _logger;
        private readonly IStationBoardService _stationBoardService;
        private readonly ICrsService _crsService;
        private readonly IConfiguration _config;
        private readonly IDateTimeService _dateTimeService;

        private readonly int _delayMinutesThreshold;
        public int DelayMinutesThreshold => _delayMinutesThreshold;

        public DelaysService(
            ILogger<DelaysService> logger,
            IStationBoardService stationBoardService,
            ICrsService crsService,
            IConfiguration config,
            IDateTimeService dateTimeService
            )
        {
            _logger = logger;
            _stationBoardService = stationBoardService;
            _crsService = crsService;
            _config = config;
            _dateTimeService = dateTimeService;

            _logger.LogInformation("Loading Delay Minutes Threshold from settings");
            if (!int.TryParse(_config["DelayMinutesThreshold"], out _delayMinutesThreshold))
            {
                _delayMinutesThreshold = 5; // default 5 minutes if can't read configuration
            }
            _logger.LogInformation($"Threshold: {_delayMinutesThreshold} minutes");
        }

        public async Task<DelaysResponse> GetDelaysAsync(StationBoardRequest request)
        {
            // Parse the list of comma separated STDs if provided (e.g. /btn/to/lon/50/0729,0744,0748)
            var stds = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.Std))
            {
                var potentialStds = request.Std.Split(',');
                var ukNow = TimeZoneInfo.ConvertTimeFromUtc(_dateTimeService.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                var dontRequest = 0;
                foreach (var potentialStd in potentialStds)
                {
                    // Parse the STD in 24-hour format (with no colon)
                    if (!DateTime.TryParseExact(potentialStd, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime requestStd))
                    {
                        continue;
                    }
                    stds.Add(potentialStd);
                    var diff = requestStd.Subtract(ukNow);
                    if (diff.TotalHours > 2 || diff.TotalHours < -1)
                    {
                        dontRequest++;
                    }
                }
                // Don't make a request if all trains are more than 2 hours in the future or more than 1 hour in the past
                if (stds.Count > 0 && stds.Count == dontRequest)
                {
                    return new DelaysResponse();
                }
            }

            var totalDelayMinutes = 0;
            var delayedTrains = new List<ServiceItem>();

            var filterOnLondonTerminals = request.FilterCrs.NotNullAndEquals("LON") ||
                                          request.FilterCrs.NotNullAndEquals("London");
            if (filterOnLondonTerminals)
            {
                request.FilterCrs = null;
            }

            request.Expand = true;
            if (!(await _stationBoardService.GetArrivalDepartureBoardAsync(request) is StationBoardWithDetails response))
            {
                _logger.LogError("Failed to get API response for delay calculation");
                return new DelaysResponse();
            }

            var trainServices = response.trainServices ?? Array.Empty<ServiceItemWithCallingPoints>();
            var railReplacement = !trainServices.Any() && (response.busServices?.Any() ?? false);
            var messagesPresent = response.nrccMessages?.Any() ?? false;

            if (filterOnLondonTerminals)
            {
                response.filtercrs ??= "LON";
                response.filterLocationName ??= "London"; // only overwrite if null
                response.filterType = request.FilterType; // request filter is null so response type will be 0
                var lTerms = _crsService.GetLondonTerminals();
                trainServices = request.FilterType switch
                {
                    FilterType.to => trainServices
                        .Where(ts => ts.subsequentCallingPoints != null && ts.subsequentCallingPoints.Any(
                            scp => scp.callingPoint != null && scp.callingPoint.Any(
                                d => lTerms.Any(
                                    lt => lt.CrsCode == d.crs.ToUpperInvariant())))).ToArray(),
                    FilterType.from => trainServices
                        .Where(ts => ts.previousCallingPoints != null && ts.previousCallingPoints.Any(
                            pcp => pcp.callingPoint != null && pcp.callingPoint.Any(
                                d => lTerms.Any(
                                    lt => lt.CrsCode == d.crs.ToUpperInvariant())))).ToArray(),
                    _ => throw new ArgumentOutOfRangeException(nameof(request), "Invalid FilterType"),
                };
            }

            // If STDs are provided then select only the train(s) matching them
            if (stds.Count > 0)
            {
                trainServices = trainServices.Where(ts => stds.Contains(ts.std.RemoveString(":"))).ToArray();
            }

            // Parse the response from the web service
            foreach (var si in trainServices.Where(si => !si.etd.NotNullAndEquals("On time") &&
                                                         !si.eta.NotNullAndEquals("On time")))
            {
                var et = si.etd ?? si.eta ?? string.Empty;
                var st = si.std ?? si.sta ?? string.Empty;

                if (si.isCancelled ||
                    si.filterLocationCancelled ||
                    et.NotNullAndEquals("Delayed") ||
                    et.NotNullAndEquals("Canceled"))
                {
                    delayedTrains.Add(si);
                }
                else
                {
                    // Could be "Starts Here", "No Report" or contain a * (report overdue)
                    if (DateTime.TryParse(et.RemoveString("*"), out DateTime etdt))
                    {
                        if (DateTime.TryParse(st, out DateTime stdt))
                        {
                            var late = etdt.Subtract(stdt);
                            totalDelayMinutes += (int)late.TotalMinutes;
                            if (late.TotalMinutes > _delayMinutesThreshold)
                            {
                                delayedTrains.Add(si);
                            }
                        }
                    }
                }
            }

            return new DelaysResponse
            {
                GeneratedAt = response.generatedAt,
                Crs = response.crs,
                LocationName = response.locationName,
                Filtercrs = response.filtercrs,
                FilterLocationName = response.filterLocationName,
                FilterType = response.filterType,
                Delays = delayedTrains.Count > 0 || railReplacement || messagesPresent,
                TotalTrainsDelayed = delayedTrains.Count,
                TotalDelayMinutes = totalDelayMinutes,
                TotalTrains = trainServices.Length,
                DelayedTrains = delayedTrains,
            };
        }
    }
}
