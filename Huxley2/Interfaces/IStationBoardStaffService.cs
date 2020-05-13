// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Models;
using OpenLDBSVWS;
using System.Threading.Tasks;

namespace Huxley2.Interfaces
{
    public interface IStationBoardStaffService
    {
        Task<BaseStationBoard> GetDepartureBoardAsync(StationBoardRequest request);
        Task<BaseStationBoard> GetArrivalBoardAsync(StationBoardRequest request);
        Task<BaseStationBoard> GetArrivalDepartureBoardAsync(StationBoardRequest request);
        Task<BaseStationBoard> GetNextDeparturesAsync(StationBoardRequest request);
        Task<BaseStationBoard> GetFastestDeparturesAsync(StationBoardRequest request);
    }
}
