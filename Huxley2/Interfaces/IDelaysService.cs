// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System.Threading.Tasks;
using Huxley2.Models;

namespace Huxley2.Interfaces
{
    public interface IDelaysService
    {
        Task<DelaysResponse> GetDelaysAsync(StationBoardRequest request);
        string GenerateChecksum(DelaysResponse response);
    }
}
