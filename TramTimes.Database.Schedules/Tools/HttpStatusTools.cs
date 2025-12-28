using System.Net;

namespace TramTimes.Database.Schedules.Tools;

public static class HttpStatusTools
{
    public static bool GetStatusCode(HttpStatusCode statusCode)
    {
        #region build result

        var result = statusCode is
            HttpStatusCode.RequestTimeout or
            HttpStatusCode.TooManyRequests or
            HttpStatusCode.InternalServerError or
            HttpStatusCode.BadGateway or
            HttpStatusCode.ServiceUnavailable or
            HttpStatusCode.GatewayTimeout;

        #endregion

        return result;
    }
}