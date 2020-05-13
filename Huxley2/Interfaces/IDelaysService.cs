// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Models;
using System.Threading.Tasks;

namespace Huxley2.Interfaces
{
    public interface IDelaysService
    {
        Task<DelaysResponse> GetDelaysAsync(StationBoardRequest request);
    }
}
