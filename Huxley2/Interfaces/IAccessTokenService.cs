// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Models;

namespace Huxley2.Interfaces
{
    public interface IAccessTokenService
    {
        OpenLDBWS.AccessToken MakeAccessToken(BaseRequest request);
        OpenLDBSVWS.AccessToken MakeStaffAccessToken(BaseRequest request);
        bool TryMakeStaffAccessToken(out OpenLDBSVWS.AccessToken accessToken);
    }
}
