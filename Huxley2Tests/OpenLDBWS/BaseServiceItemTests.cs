// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.WebUtilities;
using OpenLDBWS;
using System;
using System.Net;
using System.Text;
using Xunit;

namespace Huxley2Tests.OpenLDBWS
{
    public class BaseServiceItemTests
    {
        [Fact]
        public void ServiceItemIdIsPercentEncoded()
        {
            // because the new-format codes don't need percent encoding, this
            // operation should do nothing, for backwards compatibility
            var code = "4613377ABRYSTH_";
            var serviceItem = new BaseServiceItem
            {
                serviceID = code
            };
            var expected = WebUtility.UrlEncode(serviceItem.serviceID);
            Assert.Equal(code, serviceItem.ServiceIdPercentEncoded);
            Assert.Equal(expected, serviceItem.ServiceIdPercentEncoded);
        }

        [Fact]
        public void ServiceItemIdIsGuidEncoded()
        {
            // we now use a custom encoding into a GUID, because the new-format
            // codes do not neatly convert.
            var code = "4611018PADTON__";
            var expected = BaseServiceItem.ToGuid(code);
            var serviceItem = new BaseServiceItem
            {
                serviceID = code
            };
            Assert.Equal(expected, serviceItem.ServiceIdGuid);
        }

        [Fact]
        public void ServiceItemIdIsUrlSafeEncoded()
        {
            var code = "4629324MNCRPIC_";
            var serviceItem = new BaseServiceItem
            {
                serviceID = code
            };
            var expected = WebEncoders.Base64UrlEncode(
                System.Text.Encoding.UTF8.GetBytes(code)
            );
            Assert.Equal(expected, serviceItem.ServiceIdUrlSafe);
        }
    }
}
