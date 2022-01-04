// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Threading.Tasks;
using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using OpenLDBWS;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class ServiceControllerTests : BaseControllerTests
    {
        private new IServiceDetailsService service;
        private ServiceController controller;
        private new ServiceRequest request;

        public ServiceControllerTests()
        {
            service = A.Fake<IServiceDetailsService>();
            controller = new ServiceController(A.Fake<ILogger<ServiceController>>(), service)
            {
                ControllerContext = controllerContext
            };
            request = new ServiceRequest
            {
                ServiceId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            };
        }

        [Fact]
        public async Task ServiceControllerGetPassesRequestFromRouteToService()
        {
            await controller.Get(request, new ServiceRequest());

            A.CallTo(() => service.GetServiceDetailsAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceControllerGetPassesRequestFromQueryToService()
        {
            var queryRequest = new ServiceRequest
            {
                ServiceId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            };

            await controller.Get(request, queryRequest);

            A.CallTo(() => service.GetServiceDetailsAsync(queryRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceControllerGetReturnsResponseFromService()
        {
            var response = new BaseServiceDetails();
            A.CallTo(() => service.GetServiceDetailsAsync(request)).Returns(response);

            var result = await controller.Get(request, request);

            Assert.Equal(response, result);
        }

        [Fact]
        public async Task ServiceControllerGetThrowsExceptionIfNoServiceId()
        {
            request = new ServiceRequest();
            var response = new BaseServiceDetails();
            A.CallTo(() => service.GetServiceDetailsAsync(request)).Returns(response);

            var exception = await Assert.ThrowsAsync<Exception>(() => controller.Get(request, request));
            Assert.Equal("No Service ID provided", exception.Message);
        }

        [Fact]
        public async Task ServiceControllerSetsETag()
        {
            var response = new BaseServiceDetails();
            A.CallTo(() => service.GetServiceDetailsAsync(request)).Returns(response);
            A.CallTo(() => service.GenerateChecksum(response)).Returns(etag);

            await controller.Get(request, request);

            Assert.Equal(etag, httpResponse.Headers[HeaderNames.ETag]);
        }
    }
}
