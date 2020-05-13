// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;

namespace Huxley2.Interfaces
{
    public interface IDateTimeService
    {
        public DateTime LocalNow { get; }
        public DateTime UtcNow { get; }
        public DateTimeOffset OffsetLocalNow { get; }
        public DateTimeOffset OffsetUtcNow { get; }
    }
}
