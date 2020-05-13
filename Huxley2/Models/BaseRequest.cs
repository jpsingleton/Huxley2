// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.Mvc;

namespace Huxley2.Models
{
    public class BaseRequest
    {
        [FromQuery]
        public string AccessToken { get; set; } = string.Empty;
    }
}
