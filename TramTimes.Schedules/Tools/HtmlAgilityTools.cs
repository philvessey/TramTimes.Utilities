namespace TramTimes.Schedules.Tools;

public static class HtmlAgilityTools
{
    public static bool GetHeaderRow(
        string cell1,
        string cell2,
        string cell3) {
        
        #region build header
        
        var header = new List<string>
        {
            "Route",
            "Line",
            "To",
            "Destination",
            "Scheduled",
            "Expected"
        };
        
        #endregion
        
        #region build result
        
        var result = header.Any(predicate: value => 
            cell1.Contains(value: value) ||
            cell2.Contains(value: value) ||
            cell3.Contains(value: value) ||
            cell1.Length > 10 ||
            cell2.Length > 20 ||
            cell3.Length > 10);
        
        #endregion
        
        return result;
    }
}