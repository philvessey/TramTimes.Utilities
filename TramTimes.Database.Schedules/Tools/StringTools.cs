namespace TramTimes.Database.Schedules.Tools;

public static class StringTools
{
    public static bool GetTimeRange(string? departureTime)
    {
        #region check input
        
        if (string.IsNullOrWhiteSpace(value: departureTime))
            return false;
        
        #endregion
        
        #region build time
        
        var time = TimeSpan.Parse(s: departureTime);
        
        #endregion
        
        #region check time
        
        if (time == TimeSpan.Zero)
            return false;
        
        #endregion
        
        #region build result
        
        var result = time >= TimeSpan.FromHours(hours: 12) &&
                          time < TimeSpan.FromHours(hours: 13);
        
        #endregion
        
        return result;
    }
}