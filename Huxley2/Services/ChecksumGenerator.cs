// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using Huxley2.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Huxley2.Services
{
    public static class ChecksumGenerator
    {
        public static string GenerateChecksum(OpenLDBWS.BaseStationBoard board) => GenerateChecksumImpl(board);

        public static string GenerateChecksum(OpenLDBSVWS.BaseStationBoard board) => GenerateChecksumImpl(board);

        public static string GenerateChecksum(object service) => GenerateChecksumImpl(service);

        public static string GenerateChecksum(DelaysResponse response)
        {
            // Different due to casing
            var generatedAt = response.GeneratedAt;
            response.GeneratedAt = DateTime.MinValue;
            var checksum = GenerateChecksumObj(response);
            response.GeneratedAt = generatedAt;
            return checksum;
        }

        // Generates a checksum hash of an object with a generatedAt property excluding that property
        private static string GenerateChecksumImpl(dynamic response)
        {
            var generatedAt = DateTime.MinValue;
            if (response is OpenLDBWS.BaseStationBoard
                         or OpenLDBSVWS.BaseStationBoard
                         or OpenLDBWS.BaseServiceDetails
                         or OpenLDBSVWS.BaseServiceDetails)
            {
                // Use arbitrary timestamp as the value is always the time of request and will alter the hash
                generatedAt = response.generatedAt;
                response.generatedAt = DateTime.MinValue;
            }
            var checksum = GenerateChecksumObj(response);
            if (generatedAt != DateTime.MinValue)
            {
                // Restore original timestamp so reference to response still returns it as it was
                response.generatedAt = generatedAt;
            }
            return checksum;
        }

        private static string GenerateChecksumObj(object obj)
        {
            using (var ms = new MemoryStream())
            {
                // Specify type so base class is not used and hash changes with services
                JsonSerializer.Serialize(ms, obj, obj.GetType());
                ms.Position = 0;
                using (var algo = SHA256.Create())
                {
                    byte[] bytes = algo.ComputeHash(ms);
                    var checksum = $"\"{WebEncoders.Base64UrlEncode(bytes)}\"";
                    return checksum;
                }
            }
        }
    }
}
