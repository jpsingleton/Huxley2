// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using OpenLDBWS;
using System;
using System.Collections.Generic;

namespace Huxley2.Models
{
    public class DelaysResponse
    {
        public DateTime GeneratedAt { get; set; }

        public string LocationName { get; set; } = string.Empty;

        public string Crs { get; set; } = string.Empty;

        public string? FilterLocationName { get; set; } = string.Empty;

        // The c should be a capital but this matches the SOAP StationBoard
        public string? Filtercrs { get; set; } = string.Empty;

        public FilterType FilterType { get; set; }

        public bool Delays { get; set; }

        public int TotalTrainsDelayed { get; set; }

        public int TotalDelayMinutes { get; set; }

        public int TotalTrains { get; set; }

        public IEnumerable<ServiceItem> DelayedTrains { get; set; } = new List<ServiceItem>();
    }
}
