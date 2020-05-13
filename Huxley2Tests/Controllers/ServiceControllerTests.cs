// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Controllers;
using Huxley2.Interfaces;
using Huxley2.Models;
using Microsoft.Extensions.Logging;
using OpenLDBWS;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Controllers
{
    public class ServiceControllerTests
    {
        [Fact]
        public async Task ServiceControllerGetPassesRequestFromRouteToService()
        {
            var request = new ServiceRequest
            {
                ServiceId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            };
            var service = A.Fake<IServiceDetailsService>();
            var controller = new ServiceController(A.Fake<ILogger<ServiceController>>(), service);

            await controller.Get(request, new ServiceRequest());

            A.CallTo(() => service.GetServiceDetailsAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceControllerGetPassesRequestFromQueryToService()
        {
            var routeRequest = new ServiceRequest
            {
                ServiceId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            };
            var queryRequest = new ServiceRequest
            {
                ServiceId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            };
            var service = A.Fake<IServiceDetailsService>();
            var controller = new ServiceController(A.Fake<ILogger<ServiceController>>(), service);

            await controller.Get(routeRequest, queryRequest);

            A.CallTo(() => service.GetServiceDetailsAsync(queryRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ServiceControllerGetReturnsResponseFromService()
        {
            var request = new ServiceRequest
            {
                ServiceId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            };
            var response = new BaseServiceDetails();
            var service = A.Fake<IServiceDetailsService>();
            A.CallTo(() => service.GetServiceDetailsAsync(request)).Returns(response);
            var controller = new ServiceController(A.Fake<ILogger<ServiceController>>(), service);

            var result = await controller.Get(request, request);

            Assert.Equal(response, result);
        }

        [Fact]
        public async Task ServiceControllerGetThrowsExceptionIfNoServiceId()
        {
            var request = new ServiceRequest();
            var response = new BaseServiceDetails();
            var service = A.Fake<IServiceDetailsService>();
            A.CallTo(() => service.GetServiceDetailsAsync(request)).Returns(response);
            var controller = new ServiceController(A.Fake<ILogger<ServiceController>>(), service);

            await Assert.ThrowsAsync<Exception>(() => controller.Get(request, request));
        }
    }
}
