// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using FakeItEasy;
using Huxley2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Huxley2Tests.Services
{
    public class UpdateCheckServiceTests
    {
        [Fact]
        public void UpdateCheckServiceReturnsCurrentVersion()
        {
            var service = new UpdateCheckService(
                A.Fake<ILogger<UpdateCheckService>>(),
                A.Fake<IConfiguration>(),
                A.Fake<HttpClient>()
                );

            Assert.Equal(Assembly.GetEntryAssembly()?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion, service.CurrentVersion);
        }

        [Fact]
        public async Task UpdateCheckServiceDoesNotCheckForUpdate()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "false";
            config["UpdateCheckStableOnly"] = "false";

            var service = GetService(config, "not.current.version-beta1");

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);
        }

        [Fact]
        public async Task UpdateCheckServiceChecksForUpdate()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "true";
            config["UpdateCheckStableOnly"] = "false";

            var service = GetService(config, "not.current.version-beta1");

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.True(service.UpdateAvailable);
            Assert.Equal("not.current.version-beta1", service.AvailableVersion);
            Assert.Equal("http://example.com/", service.UpdateUrl.AbsoluteUri);
        }

        [Fact]
        public async Task UpdateCheckServiceChecksForUpdateNoneAvailable()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "true";
            config["UpdateCheckStableOnly"] = "false";

            var service = GetService(config);

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);
        }

        [Fact]
        public async Task UpdateCheckServiceChecksForStableUpdate()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "true";
            config["UpdateCheckStableOnly"] = "true";

            var service = GetService(config, "not.current.version");

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.True(service.UpdateAvailable);
            Assert.Equal("not.current.version", service.AvailableVersion);
            Assert.Equal("http://example.com/", service.UpdateUrl.AbsoluteUri);
        }

        [Fact]
        public async Task UpdateCheckServiceChecksForStableUpdateNoneAvailable()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "true";
            config["UpdateCheckStableOnly"] = "true";

            var service = GetService(config);

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);
        }

        [Fact]
        public async Task UpdateCheckServiceChecksForStableUpdateOnlyBetaAvailable()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "true";
            config["UpdateCheckStableOnly"] = "true";

            var service = GetService(config, "not.current.version-beta1");

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);
        }

        [Fact]
        public async Task UpdateCheckServiceChecksForStableUpdateBetaAndStableAvailable()
        {
            var config = A.Fake<IConfiguration>();
            config["UpdateCheckUrl"] = "http://example.com";
            config["EnableUpdateCheck"] = "true";
            config["UpdateCheckStableOnly"] = "true";

            var service = GetService(config, "not.current.version", "not.current.version-beta1");

            Assert.False(service.UpdateAvailable);
            Assert.Equal(string.Empty, service.AvailableVersion);
            Assert.Null(service.UpdateUrl);

            await service.CheckForUpdates();

            Assert.True(service.UpdateAvailable);
            Assert.Equal("not.current.version", service.AvailableVersion);
            Assert.Equal("http://example.com/", service.UpdateUrl.AbsoluteUri);
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Tests will fail and DI handles client life-cycle elsewhere")]
        private UpdateCheckService GetService(IConfiguration config, params string[] versions)
        {
            var handler = A.Fake<FakeHttpMessageHandler>(f => f.CallsBaseMethods());
            var content = new StringContent("[" + string.Join(',', versions.Select(version =>
                   $"{{\"version\":\"{version}\",\"link\":\"http://example.com/\"}}")) + "]",
                Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content,
            };
            var client = new HttpClient(handler);
            A.CallTo(() => handler.Send(A<HttpRequestMessage>._)).Returns(response);
            return new UpdateCheckService(
                A.Fake<ILogger<UpdateCheckService>>(),
                config,
                client);
        }
    }
}
