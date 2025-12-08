namespace TramTimes.Search.Stops.Tools;

public static class DateTimeTools
{
    public static DateTime GetTargetDate(DateTime now)
    {
        #region build target

        var target = new DateOnly(
            year: now.Year,
            month: now.Month,
            day: now.Day);

        #endregion

        #region process target

        if (now.TimeOfDay > TimeSpan.FromHours(hours: 12))
            target = target.AddDays(value: 1);

        #endregion

        #region build result

        var result = target.ToDateTime(time: new TimeOnly(
            hour: 12,
            minute: 0,
            second: 0));

        #endregion

        return result;
    }
}