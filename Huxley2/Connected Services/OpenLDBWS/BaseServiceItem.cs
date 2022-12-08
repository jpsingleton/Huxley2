﻿// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace OpenLDBWS
{
    public partial class BaseServiceItem
    {
        // underscores are already URL-safe, so we do not need to encode them
        // (and in fact WebUtility.UrlEncode will not change this string)
        public string ServiceIdPercentEncoded => serviceIDField;

        public Guid ServiceIdGuid => ToGuid(serviceIDField);

        // as above
        [SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Not a URL")]
        public string ServiceIdUrlSafe => serviceIDField;

        public static Guid ToGuid(string serviceID)
        {
            // because we have 128 bits to use in the GUID (and only ~56 bits
            // from the service ID), we can afford to encode the 7 numeric
            // characters directly and the 7 alpha characters as ASCII instead
            // of finding a more efficient solution, and zero-pad the centre
            string num = serviceID.Substring(0, 7); // digits
            string str = serviceID.Substring(7, 7); // letters
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            string hexString = Convert.ToHexString(bytes);
            string guidString = num + "00000000000" + hexString.Substring(0, 14);
            return new Guid(guidString);
        }

        public static string FromGuid(Guid serviceGuid)
        {
            // reverse of ToGuid above
            string guidString = serviceGuid.ToString("N");
            string num = guidString.Substring(0, 7); // get digits
            string hexString = guidString.Substring(18, 14); // get letters
            byte[] bytes = Convert.FromHexString(hexString);
            string str = System.Text.Encoding.UTF8.GetString(bytes);
            return num + str + "_";
        }
    }
}
