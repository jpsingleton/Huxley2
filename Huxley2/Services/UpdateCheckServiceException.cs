// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;

namespace Huxley2.Services
{
    public class UpdateCheckServiceException : Exception
    {
        public UpdateCheckServiceException()
        {
        }

        public UpdateCheckServiceException(string message) : base(message)
        {
        }

        public UpdateCheckServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
