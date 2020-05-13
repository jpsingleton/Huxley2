// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;

namespace Huxley2.Services
{
    public class CrsServiceException : Exception
    {
        public CrsServiceException()
        {
        }

        public CrsServiceException(string message) : base(message)
        {
        }

        public CrsServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
