using System;
using System.Collections.Generic;

namespace Corlib.Threading {
    public static class RetrySequence {
        public static IEnumerable<TimeSpan> Create () {
            var random = new Random (Environment.TickCount);
            var timeSpan = TimeSpan.FromMilliseconds (random.Next (0, 100));

            foreach (var value in GetInitalSequence ())
                yield return timeSpan + value;

            while (true)
                yield return timeSpan + TimeSpan.FromMinutes (1);
        }

        private static IEnumerable<TimeSpan> GetInitalSequence () {
            yield return TimeSpan.FromMilliseconds (150);
            yield return TimeSpan.FromMilliseconds (750);
            yield return TimeSpan.FromSeconds (1);
            yield return TimeSpan.FromSeconds (3);
            yield return TimeSpan.FromSeconds (13);
            yield return TimeSpan.FromSeconds (49);
        }
    }
}