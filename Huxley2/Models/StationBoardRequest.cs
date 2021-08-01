// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.Mvc;
using OpenLDBWS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Huxley2.Models
{
    public class StationBoardRequest : BaseRequest
    {
        private string _crs = string.Empty;
        private string? _filterCrs = null;

        [Range(0, 150)]
        public ushort NumRows { get; set; } = 10;

        [Required]
        [MinLength(3)]
        public string Crs
        {
            get => _crs?.ToUpperInvariant()?.Trim() ?? string.Empty;
            set => _crs = value;
        }

        [MinLength(3)]
        public string? FilterCrs
        {
            get => FilterList?.FirstOrDefault();
            set => _filterCrs = value;
        }

        /// <summary>
        /// A list of CRS codes of the destinations location to filter.
        /// Must have at least 1 for Next and Fastest.
        /// </summary>
        public List<string> FilterList =>
            _filterCrs?.ToUpperInvariant()?.Split(',')?
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim()).Distinct().ToList() ??
            new List<string>();

        public FilterType FilterType { get; set; } = FilterType.to;

        [FromQuery]
        [Range(-120, 119)]
        public int TimeOffset { get; set; } = 0;

        [FromQuery]
        [Range(-120, 120)]
        public int TimeWindow { get; set; } = 120;

        [FromQuery]
        public bool Expand { get; set; } = false;

        public string Std { get; set; } = string.Empty;

        [FromQuery]
        public string Services { get; set; } = "PBS";
    }
}
