// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Interfaces;
using System;

namespace Huxley2.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime LocalNow => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTimeOffset OffsetLocalNow => DateTimeOffset.Now;

        public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
    }
}
