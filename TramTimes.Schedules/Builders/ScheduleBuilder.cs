using System.Text.Json;
using HtmlAgilityPack;
using Polly;
using Spectre.Console;
using TramTimes.Schedules.Models;
using TramTimes.Schedules.Tools;

namespace TramTimes.Schedules.Builders;

public static class ScheduleBuilder
{
    private static readonly Random Random = new();
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
    public static async Task BuildWeekAsync(
        string stop,
        DateTime targetDate,
        ProgressContext? context = null) {
        
        #region build result
        
        var result = new Schedule
        {
            Monday = [],
            Tuesday = [],
            Wednesday = [],
            Thursday = [],
            Friday = [],
            Saturday = [],
            Sunday = []
        };
        
        #endregion
        
        #region build task
        
        ProgressTask? task = null;
        
        if (context is not null)
            task = context.AddTask(
                description: $"[blue]Processing {stop}[/]",
                maxValue: 7);
        
        #endregion
        
        #region process task
        
        for (var i = 0; i < 7; i++)
        {
            var workingDate = targetDate.AddDays(value: i);
            
            if (task is not null)
                task.Description = $"[yellow]{workingDate:yyyy-MM-dd}[/]";
            
            var services = await BuildDayAsync(
                workingDate: workingDate,
                stop: stop);
            
            switch (workingDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    result.Monday.AddRange(collection: services);
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    result.Tuesday.AddRange(collection: services);
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    result.Wednesday.AddRange(collection: services);
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    result.Thursday.AddRange(collection: services);
                    break;
                }
                case DayOfWeek.Friday:
                {
                    result.Friday.AddRange(collection: services);
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    result.Saturday.AddRange(collection: services);
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    result.Sunday.AddRange(collection: services);
                    break;
                }
                default:
                    result.Monday.AddRange(collection: services);
                    break;
            }
            
            task?.Increment(value: 1);
            
            await Task.Delay(delay: TimeSpanTools.GetJitteredDelay(
                baseDelay: TimeSpan.FromMilliseconds(milliseconds: 500),
                jitterRandom: Random));
        }
        
        #endregion
        
        #region complete task
        
        await File.WriteAllTextAsync(
            path: Path.Combine(
                path1: "output",
                path2: $"_{stop.ToUpper()}.json"),
            contents: JsonSerializer.Serialize(
                value: result,
                options: Options));
        
        if (task is not null)
        {
            task.Description = $"[green]✅ {stop}[/]";
            task.StopTask();
        }
        
        #endregion
    }
    
    private static async Task<List<Service>> BuildDayAsync(
        DateTime workingDate,
        string stop) {
        
        #region build policy
        
        var policy = Policy.WrapAsync(policies:
        [
            PolicyBuilder.BuildBreaker(),
            PolicyBuilder.BuildRetry()
        ]);
        
        #endregion
        
        #region build client
        
        var client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        
        #endregion
        
        #region build headers
        
        client.DefaultRequestHeaders.Add(
            name: "User-Agent", 
            value: "TramTimes/1.0 (+https://tramtimes.net)");
        
        #endregion
        
        #region build url
        
        var url = $"https://bustimes.org/stops/{stop.ToUpper()}?date={workingDate:yyyy-MM-dd}&time=12%3A00";
        
        #endregion
        
        #region build response
        
        var response = await policy.ExecuteAsync(action: async () => await client.GetAsync(requestUri: url));
        
        #endregion
        
        #region build content
        
        var content = await response.Content.ReadAsStringAsync();
        
        #endregion
        
        #region build document
        
        var document = new HtmlDocument();
        document.LoadHtml(html: content);
        
        #endregion
        
        #region build rows
        
        var rows = document.DocumentNode.SelectNodes(xpath: "//table//tr[td]");
        
        #endregion
        
        #region build results
        
        var results = rows.Select(selector: row => row.SelectNodes(xpath: "td"))
            .Where(predicate: cells => cells.Count >= 3)
            .Where(predicate: cells => !string.IsNullOrEmpty(value: cells[0].InnerText.Trim()))
            .Where(predicate: cells => !string.IsNullOrEmpty(value: cells[1].InnerText.Trim()))
            .Where(predicate: cells => !string.IsNullOrEmpty(value: cells[2].InnerText.Trim()))
            .Where(predicate: cells => !HtmlAgilityTools.GetHeaderRow(
                cell1: cells[0].InnerText.Trim(),
                cell2: cells[1].InnerText.Trim(),
                cell3: cells[2].InnerText.Trim()))
            .Select(selector: cells => new Service
            {
                DepartureTime = cells[2].InnerText.Trim() + ":00",
                DestinationName = cells[1].InnerText.Trim(),
                RouteName = cells[0].InnerText.Trim()
            })
            .Where(predicate: service => StringTools.GetTimeRange(departureTime: service.DepartureTime))
            .Distinct()
            .ToList();
        
        #endregion
        
        #region build delay
        
        await Task.Delay(delay: TimeSpanTools.GetJitteredDelay(
            baseDelay: TimeSpan.FromMilliseconds(milliseconds: 500),
            jitterRandom: Random));
        
        #endregion
        
        return results;
    }
}