namespace TramTimes.Database.Stops.Tools;

public static class TimeSpanTools
{
    public static TimeSpan GetJitteredDelay(
        TimeSpan baseDelay,
        double? jitterFactor = null,
        Random? jitterRandom = null) {

        #region build jitter

        var jitter = baseDelay.TotalMilliseconds * (jitterFactor ?? 0.5);

        #endregion

        #region build random

        var random = jitterRandom ?? new Random();

        #endregion

        #region process jitter

        jitter *= (random.NextDouble() * 2) - 1;

        #endregion

        #region build result

        var result = TimeSpan.FromMilliseconds(value: Math.Max(
            val1: baseDelay.TotalMilliseconds + jitter,
            val2: 100));

        #endregion

        return result;
    }
}