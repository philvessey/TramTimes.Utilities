using Polly;
using TramTimes.Database.Schedules.Tools;

namespace TramTimes.Database.Schedules.Builders;

public static class PolicyBuilder
{
    private static readonly Random _random = new();

    public static IAsyncPolicy<HttpResponseMessage> BuildBreaker()
    {
        #region build result

        var result = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<HttpResponseMessage>(resultPredicate: response => HttpStatusTools.GetStatusCode(statusCode: response.StatusCode))
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromMinutes(minutes: 2));

        #endregion

        return result;
    }

    public static IAsyncPolicy<HttpResponseMessage> BuildRetry()
    {
        #region build result

        var result = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<HttpResponseMessage>(resultPredicate: response => HttpStatusTools.GetStatusCode(statusCode: response.StatusCode))
            .WaitAndRetryAsync(
                sleepDurationProvider: retryAttempt => TimeSpanTools.GetJitteredDelay(
                    baseDelay: TimeSpan.FromMilliseconds(milliseconds: 1000),
                    retryAttempt: retryAttempt,
                    jitterRandom: _random),
                retryCount: 2);

        #endregion

        return result;
    }
}