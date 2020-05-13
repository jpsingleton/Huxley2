// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace OpenLDBWS
{
    public partial class BaseServiceItem
    {
        public string ServiceIdPercentEncoded => WebUtility.UrlEncode(serviceIDField);

        public Guid ServiceIdGuid => new Guid(Convert.FromBase64String(serviceIDField));

        [SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Not a URL")]
        public string ServiceIdUrlSafe => WebEncoders.Base64UrlEncode(Convert.FromBase64String(serviceIDField));
    }
}
