namespace TramTimes.Search.Stops.Builders;

public static class StopBuilder
{
    public static async Task<string[]> BuildAsync(string path)
    {
        #region build stops

        var stops = await File.ReadAllLinesAsync(path: path);

        #endregion

        #region build results

        var results = stops
            .Where(predicate: id => !string.IsNullOrWhiteSpace(value: id))
            .Select(selector: id => id.Trim())
            .ToArray();

        #endregion

        return results;
    }
}