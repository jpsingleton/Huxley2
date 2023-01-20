// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace OpenLDBWS
{
    public partial class BaseServiceItem
    {
        // new service ID format: 0000000XXXXXXXX, where 0 is any numeric char
        // and X is any alpha char or an underscore. So far, all service IDs
        // appear to end in at least one underscore, but I am uncertain if this
        // is always the case

        // underscores are already URL-safe, so we do not need to encode them
        // (and in fact WebUtility.UrlEncode will not change this string)
        public string ServiceIdPercentEncoded => serviceIDField;

        public Guid ServiceIdGuid => ToGuid(serviceIDField);

        // note that while ServiceID is already URL-safe, the underscore char
        // is not valid base64, so any inputs that previously assumed b64 input
        // may break. As such, we will encode the string to be b64-safe as well
        // as URL-safe
        [SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Not a URL")]
        public string ServiceIdUrlSafe => WebEncoders.Base64UrlEncode(
            System.Text.Encoding.UTF8.GetBytes(serviceIDField)
        );

        public static Guid ToGuid(string serviceID)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(serviceID);

            string hexString = Convert.ToHexString(bytes);

            // Need 8 chars for first group of GUID, plus one extra for the "rest" of the GUID.
            // In reality, this should always be the case, but let's try to make this method as
            // robust as possible.
            if (hexString.Length < 9)
            {
                hexString = hexString.PadRight(9, '0');
            }

            // First group of GUID
            string guidStart = hexString[0..8];

            // Rest of the hex string to form the rest of the GUID, padding the left until it
            // makes up a full GUID string when combined with the first group, and ensuring
            // it's no longer than 24 chars in the unlikely case it is.
            string guidRest = hexString[8..].PadLeft(24, '0').Substring(0, 24);

            return new Guid(guidStart + guidRest);
        }

        public static string FromGuid(Guid serviceGuid)
        {
            // Opposite of the above

            string guidString = serviceGuid.ToString("N", CultureInfo.InvariantCulture);

            // Gets the raw byte array from the GUID, filtering out padded null bytes (0x00)
            byte[] bytes = Array.FindAll(Convert.FromHexString(guidString), b => b != 0x00);

            // Convert bytes back to string
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
