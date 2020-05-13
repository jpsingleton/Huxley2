// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace Huxley2Tests.Services
{
    public class AccessTokenServiceTests
    {
        [Fact]
        public void AccessTokenServiceMakesAccessTokenFromConfig()
        {
            var config = A.Fake<IConfiguration>();
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            config["ClientAccessToken"] = cat;
            config["DarwinAccessToken"] = dat;
            var request = new BaseRequest { AccessToken = cat };
            var service = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var token = service.MakeAccessToken(request);

            Assert.Equal(dat, token.TokenValue);
        }

        [Fact]
        public void AccessTokenServiceMakesAccessTokenFromRequest()
        {
            var config = A.Fake<IConfiguration>();
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            config["ClientAccessToken"] = "";
            config["DarwinAccessToken"] = dat;
            var request = new BaseRequest { AccessToken = cat };
            var service = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var token = service.MakeAccessToken(request);

            Assert.Equal(cat, token.TokenValue);
        }

        [Fact]
        public void AccessTokenServiceMakesStaffAccessTokenFromConfig()
        {
            var config = A.Fake<IConfiguration>();
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            config["ClientAccessToken"] = cat;
            config["DarwinStaffAccessToken"] = dat;
            var request = new BaseRequest { AccessToken = cat };
            var service = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var token = service.MakeStaffAccessToken(request);

            Assert.Equal(dat, token.TokenValue);
        }

        [Fact]
        public void AccessTokenServiceMakesStaffAccessTokenFromRequest()
        {
            var config = A.Fake<IConfiguration>();
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            config["ClientAccessToken"] = "";
            config["DarwinStaffAccessToken"] = dat;
            var request = new BaseRequest { AccessToken = cat };
            var service = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var token = service.MakeStaffAccessToken(request);

            Assert.Equal(cat, token.TokenValue);
        }

        [Fact]
        public void AccessTokenServiceTryMakeStaffAccessTokenSuccess()
        {
            var config = A.Fake<IConfiguration>();
            var dat = Guid.NewGuid().ToString();
            config["DarwinStaffAccessToken"] = dat;
            var service = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var success = service.TryMakeStaffAccessToken(out var token);

            Assert.True(success);
            Assert.Equal(dat, token.TokenValue);
        }

        [Fact]
        public void AccessTokenServiceTryMakeStaffAccessTokenFailure()
        {
            var config = A.Fake<IConfiguration>();
            var dat = Guid.NewGuid().ToString();
            config["DarwinAccessToken"] = dat;
            var service = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var success = service.TryMakeStaffAccessToken(out var token);

            Assert.False(success);
            Assert.True(string.IsNullOrWhiteSpace(token.TokenValue));
        }
    }

}
