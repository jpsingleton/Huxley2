// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Models;
using Huxley2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using Xunit;

namespace Huxley2Tests.Services
{
    public class ServiceDetailsServiceTests
    {
        private string dat;
        private ServiceRequest restRequest;
        private LDBServiceSoap client;
        private OpenLDBSVWS.LDBSVServiceSoap staffClient;
        private ServiceDetailsService service;

        public ServiceDetailsServiceTests()
        {
            var sid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var cat = Guid.NewGuid().ToString();
            dat = Guid.NewGuid().ToString();
            restRequest = new ServiceRequest
            {
                ServiceId = sid,
                AccessToken = cat,
            };
            var config = A.Fake<IConfiguration>();
            config["ClientAccessToken"] = cat;
            config["DarwinAccessToken"] = dat;
            config["DarwinStaffAccessToken"] = dat;
            var tokenService = new AccessTokenService(A.Fake<ILogger<AccessTokenService>>(), config);
            client = A.Fake<LDBServiceSoap>();
            staffClient = A.Fake<OpenLDBSVWS.LDBSVServiceSoap>();
            service = new ServiceDetailsService(
                A.Fake<ILogger<ServiceDetailsService>>(),
                tokenService,
                client,
                staffClient);
        }

        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsCallsClient()
        {
            await service.GetServiceDetailsAsync(restRequest);

            A.CallTo(() => client.GetServiceDetailsAsync(
                A<GetServiceDetailsRequest>.That.Matches(s =>
                    s.AccessToken.TokenValue == dat
                        && s.serviceID == restRequest.ServiceId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsReturnsResult()
        {
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
            restRequest.ServiceId = "012345678901234";

            await service.GetServiceDetailsAsync(restRequest);

            A.CallTo(() => staffClient.GetServiceDetailsByRIDAsync(
                A<OpenLDBSVWS.GetServiceDetailsByRIDRequest>.That.Matches(s =>
                    s.AccessToken.TokenValue == dat
                        && s.rid == restRequest.ServiceId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceDetailsServiceGetServiceDetailsReturnsStaffResult()
        {
            restRequest.ServiceId = "012345678901234";
            var result = new OpenLDBSVWS.ServiceDetails1();
            var response = new OpenLDBSVWS.GetServiceDetailsByRIDResponse(result);

            A.CallTo(() => staffClient.GetServiceDetailsByRIDAsync(
                A<OpenLDBSVWS.GetServiceDetailsByRIDRequest>._)).Returns(response);

            var serviceDetails = await service.GetServiceDetailsAsync(restRequest);

            Assert.IsType<OpenLDBSVWS.ServiceDetails1>(serviceDetails);
            Assert.Equal(result, serviceDetails);
        }

        [Fact]
        public void ServiceDetailsServiceGeneratesChecksum()
        {
            var result = new ServiceDetails();

            var checksum = service.GenerateChecksum(result);

            // SHA256 hash of empty object Base64 URL encoded _with_ extra quotes
            Assert.Equal("\"GS3unZdh0CxfctvkUtTupvZ_tBSX1ebzI9y1CBoscTQ\"", checksum);
        }

        [Fact]
        public void ServiceDetailsServiceGeneratesChecksumIgnoringGeneratedAt()
        {
            var result = new ServiceDetails { generatedAt = DateTime.Now };

            var checksum = service.GenerateChecksum(result);

            Assert.Equal("\"GS3unZdh0CxfctvkUtTupvZ_tBSX1ebzI9y1CBoscTQ\"", checksum);

            result.generatedAt = DateTime.Now.AddMinutes(5);

            checksum = service.GenerateChecksum(result);

            Assert.Equal("\"GS3unZdh0CxfctvkUtTupvZ_tBSX1ebzI9y1CBoscTQ\"", checksum);
        }

        [Fact]
        public void ServiceDetailsServiceGeneratesChecksumPreservingGeneratedAt()
        {
            var now = DateTime.Now;
            var result = new ServiceDetails { generatedAt = now };

            service.GenerateChecksum(result);

            Assert.Equal(now, result.generatedAt);
        }

        [Fact]
        public void ServiceDetailsServiceStaffGeneratesChecksum()
        {
            var result = new OpenLDBSVWS.ServiceDetails1();

            var checksum = service.GenerateChecksum(result);

            // SHA256 hash of empty object Base64 URL encoded _with_ extra quotes
            Assert.Equal("\"BFoY_BtC4N5l_qGGW9uT-cK32n1-K0xdXnWmQIG02WE\"", checksum);
        }

        [Fact]
        public void ServiceDetailsServiceStaffGeneratesChecksumIgnoringGeneratedAt()
        {
            var result = new OpenLDBSVWS.ServiceDetails1 { generatedAt = DateTime.Now };

            var checksum = service.GenerateChecksum(result);

            Assert.Equal("\"BFoY_BtC4N5l_qGGW9uT-cK32n1-K0xdXnWmQIG02WE\"", checksum);

            result.generatedAt = DateTime.Now.AddMinutes(5);

            checksum = service.GenerateChecksum(result);

            Assert.Equal("\"BFoY_BtC4N5l_qGGW9uT-cK32n1-K0xdXnWmQIG02WE\"", checksum);
        }

        [Fact]
        public void ServiceDetailsServiceStaffGeneratesChecksumPreservingGeneratedAt()
        {
            var now = DateTime.Now;
            var result = new OpenLDBSVWS.ServiceDetails1 { generatedAt = now };

            service.GenerateChecksum(result);

            Assert.Equal(now, result.generatedAt);
        }

    }
}
