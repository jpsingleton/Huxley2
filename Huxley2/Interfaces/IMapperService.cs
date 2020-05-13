// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Models;
using OpenLDBWS;

namespace Huxley2.Interfaces
{
    public interface IMapperService
    {
        GetDepartureBoardRequest MapGetDepartureBoardRequest(StationBoardRequest request);
        GetDepBoardWithDetailsRequest MapGetDepBoardWithDetailsRequest(StationBoardRequest request);
        GetArrivalBoardRequest MapGetArrivalBoardRequest(StationBoardRequest request);
        GetArrBoardWithDetailsRequest MapGetArrBoardWithDetailsRequest(StationBoardRequest request);
        GetArrivalDepartureBoardRequest MapGetArrivalDepartureBoardRequest(StationBoardRequest request);
        GetArrDepBoardWithDetailsRequest MapGetArrDepBoardWithDetailsRequest(StationBoardRequest request);

        GetNextDeparturesRequest MapGetNextDeparturesRequest(StationBoardRequest request);
        GetNextDeparturesWithDetailsRequest MapGetNextDeparturesWithDetailsRequest(StationBoardRequest request);
        GetFastestDeparturesRequest MapGetFastestDeparturesRequest(StationBoardRequest request);
        GetFastestDeparturesWithDetailsRequest MapGetFastestDeparturesWithDetailsRequest(StationBoardRequest request);

        OpenLDBSVWS.GetDepartureBoardByCRSRequest MapGetDepartureBoardStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetDepBoardWithDetailsRequest MapGetDepBoardWithDetailsStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetArrivalBoardByCRSRequest MapGetArrivalBoardStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetArrBoardWithDetailsRequest MapGetArrBoardWithDetailsStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetArrivalDepartureBoardByCRSRequest MapGetArrivalDepartureBoardStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetArrDepBoardWithDetailsRequest MapGetArrDepBoardWithDetailsStaffRequest(StationBoardRequest request);

        OpenLDBSVWS.GetNextDeparturesRequest MapGetNextDeparturesStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetNextDeparturesWithDetailsRequest MapGetNextDeparturesWithDetailsStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetFastestDeparturesRequest MapGetFastestDeparturesStaffRequest(StationBoardRequest request);
        OpenLDBSVWS.GetFastestDeparturesWithDetailsRequest MapGetFastestDeparturesWithDetailsStaffRequest(StationBoardRequest request);
    }
}
