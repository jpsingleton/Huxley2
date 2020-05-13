// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huxley2.Interfaces
{
    public interface ICrsService
    {
        IEnumerable<CrsRecord> GetLondonTerminals();
        IEnumerable<CrsRecord> GetStations(string? query);
        string MakeCrsCode(string query);
        Task LoadCrsCodes();

    }
}
