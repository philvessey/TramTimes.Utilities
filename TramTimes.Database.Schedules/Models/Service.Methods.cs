namespace TramTimes.Database.Schedules.Models;

public partial class Service : IEquatable<Service>
{
    public bool Equals(Service? other)
    {
        #region check input

        if (other is null)
            return false;

        #endregion

        #region build result

        var result = DepartureTime == other.DepartureTime &&
                          DestinationName == other.DestinationName &&
                          RouteName == other.RouteName;

        #endregion

        return result;
    }

    public override bool Equals(object? obj)
    {
        #region build result

        var result = Equals(other: obj as Service);

        #endregion

        return result;
    }

    public override int GetHashCode()
    {
        #region build result

        var result = HashCode.Combine(
            value1: DepartureTime,
            value2: DestinationName,
            value3: RouteName);

        #endregion

        return result;
    }
}