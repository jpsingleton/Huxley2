// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Services
{
    public class ServiceDetailsServiceTests
    {
        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsCallsClient()
        {
            var sid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            var restRequest = new ServiceRequest
            {
                ServiceId = sid,
                AccessToken = cat,
            };
            var config = A.Fake<IConfiguration>();
            config["ClientAccessToken"] = cat;
            config["DarwinAccessToken"] = dat;
            var tokenService = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var client = A.Fake<LDBServiceSoap>();
            var staffClient = A.Fake<OpenLDBSVWS.LDBSVServiceSoap>();

            var service = new ServiceDetailsService(
                A.Fake<ILogger<ServiceDetailsService>>(),
                tokenService,
                client,
                staffClient);

            await service.GetServiceDetailsAsync(restRequest);

            A.CallTo(() => client.GetServiceDetailsAsync(
                A<GetServiceDetailsRequest>.That.Matches(s =>
                    s.AccessToken.TokenValue == dat
                        && s.serviceID == sid)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsReturnsResult()
        {
            var sid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            var restRequest = new ServiceRequest
            {
                ServiceId = sid,
                AccessToken = cat,
            };
            var config = A.Fake<IConfiguration>();
            config["ClientAccessToken"] = cat;
            config["DarwinAccessToken"] = dat;
            var tokenService = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var client = A.Fake<LDBServiceSoap>();
            var staffClient = A.Fake<OpenLDBSVWS.LDBSVServiceSoap>();

            var service = new ServiceDetailsService(
                A.Fake<ILogger<ServiceDetailsService>>(),
                tokenService,
                client,
                staffClient);

            var result = new ServiceDetails();
            var response = new GetServiceDetailsResponse(result);

            A.CallTo(() => client.GetServiceDetailsAsync(
                A<GetServiceDetailsRequest>._)).Returns(response);

            var serviceDetails = await service.GetServiceDetailsAsync(restRequest);

            Assert.IsType<ServiceDetails>(serviceDetails);
            Assert.Equal(result, serviceDetails);
        }

        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsCallsStaffClient()
        {
            var sid = "012345678901234";
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            var restRequest = new ServiceRequest
            {
                ServiceId = sid,
                AccessToken = cat,
            };
            var config = A.Fake<IConfiguration>();
            config["ClientAccessToken"] = cat;
            config["DarwinStaffAccessToken"] = dat;
            var tokenService = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var client = A.Fake<LDBServiceSoap>();
            var staffClient = A.Fake<OpenLDBSVWS.LDBSVServiceSoap>();

            var service = new ServiceDetailsService(
                A.Fake<ILogger<ServiceDetailsService>>(),
                tokenService,
                client,
                staffClient);

            await service.GetServiceDetailsAsync(restRequest);

            A.CallTo(() => staffClient.GetServiceDetailsByRIDAsync(
                A<OpenLDBSVWS.GetServiceDetailsByRIDRequest>.That.Matches(s =>
                    s.AccessToken.TokenValue == dat
                        && s.rid == sid)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsReturnsStaffResult()
        {
            var sid = "012345678901234";
            var cat = Guid.NewGuid().ToString();
            var dat = Guid.NewGuid().ToString();
            var restRequest = new ServiceRequest
            {
                ServiceId = sid,
                AccessToken = cat,
            };
            var config = A.Fake<IConfiguration>();
            config["ClientAccessToken"] = cat;
            config["DarwinStaffAccessToken"] = dat;
            var tokenService = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);

            var client = A.Fake<LDBServiceSoap>();
            var staffClient = A.Fake<OpenLDBSVWS.LDBSVServiceSoap>();

            var service = new ServiceDetailsService(
                A.Fake<ILogger<ServiceDetailsService>>(),
                tokenService,
                client,
                staffClient);

            var result = new OpenLDBSVWS.ServiceDetails1();
            var response = new OpenLDBSVWS.GetServiceDetailsByRIDResponse(result);

            A.CallTo(() => staffClient.GetServiceDetailsByRIDAsync(
                A<OpenLDBSVWS.GetServiceDetailsByRIDRequest>._)).Returns(response);

            var serviceDetails = await service.GetServiceDetailsAsync(restRequest);

            Assert.IsType<OpenLDBSVWS.ServiceDetails1>(serviceDetails);
            Assert.Equal(result, serviceDetails);
        }

    }
}
