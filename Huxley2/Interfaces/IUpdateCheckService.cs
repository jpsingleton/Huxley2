// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;
using System.Threading.Tasks;

namespace Huxley2.Interfaces
{
    public interface IUpdateCheckService
    {
        public bool UpdateAvailable { get; }
        public string CurrentVersion { get; }
        public string AvailableVersion { get; }
        public Uri? UpdateUrl { get; }
        Task CheckForUpdates();
    }
}
