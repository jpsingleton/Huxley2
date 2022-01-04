// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using Huxley2.Models;

namespace Huxley2.Interfaces
{
    public interface IServiceDetailsService
    {
        Task<object> GetServiceDetailsAsync(ServiceRequest request);
        string GenerateChecksum(object service);
    }
}
