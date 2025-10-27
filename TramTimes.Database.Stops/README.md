# Database.Stops

A .NET console application that creates C# quartz jobs from a template 
for tram stops. The jobs are used to test the output of the database 
builder.

## Features

- 📅 Generates C# quartz jobs from standard templates
- 🚊 Support for multiple tram networks via data files
- 📊 Beautiful console interface with progress tracking

## Prerequisites

- Download and install .NET 9.0 SDK

## Usage

### 1. Run the Application

```dotnet
dotnet run
```

### 2. Output

The application generates C# quartz jobs with the following structure:

```csharp
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Workers.Stops;

public class _9400ZZSYMAL1(
    BlobContainerClient containerClient,
    NpgsqlDataSource dataSource,
    ILogger<_9400ZZSYMAL1> logger) : IJob {
    
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region get stop schedule
            
            var activeSchedule = JsonSerializer.Deserialize<WorkerSchedule>(json: await File.ReadAllTextAsync(path: Path.Combine(
                path1: "Workers",
                path2: "Schedules",
                path3: "_9400ZZSYMAL1.json")));
            
            #endregion
            
            #region get database feed
            
            var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var databaseResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYMAL1",
                target: context.FireTimeUtc.Date,
                offset: TimeSpan.FromHours(value: 12),
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 59));
            
            #endregion
            
            #region check database feed
            
            switch (context.FireTimeUtc.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    if (databaseResults.Count != activeSchedule?.Monday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (databaseResults.Count != activeSchedule?.Tuesday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (databaseResults.Count != activeSchedule?.Wednesday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (databaseResults.Count != activeSchedule?.Thursday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (databaseResults.Count != activeSchedule?.Friday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (databaseResults.Count != activeSchedule?.Saturday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (databaseResults.Count != activeSchedule?.Sunday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
                }
                default:
                    logger.LogWarning(message: "9400ZZSYMAL1: Database service count does not match schedule");
                    
                    break;
            }
            
            #endregion
            
            #region process test results
            
            List<WorkerStopPoint> testResults = [];
            
            switch (context.FireTimeUtc.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    for (var i = 0; i < activeSchedule?.Monday?.Count; i++)
                    {
                        var value = activeSchedule.Monday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)){
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    for (var i = 0; i < activeSchedule?.Tuesday?.Count; i++)
                    {
                        var value = activeSchedule.Tuesday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    for (var i = 0; i < activeSchedule?.Wednesday?.Count; i++)
                    {
                        var value = activeSchedule.Wednesday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    for (var i = 0; i < activeSchedule?.Thursday?.Count; i++)
                    {
                        var value = activeSchedule.Thursday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    for (var i = 0; i < activeSchedule?.Friday?.Count; i++)
                    {
                        var value = activeSchedule.Friday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    for (var i = 0; i < activeSchedule?.Saturday?.Count; i++)
                    {
                        var value = activeSchedule.Saturday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    for (var i = 0; i < activeSchedule?.Sunday?.Count; i++)
                    {
                        var value = activeSchedule.Sunday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        var departureTime = "unknown";
                        var routeName = "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            departureTime = databaseResults.ElementAt(index: i).DepartureTime.ToString() ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is not null)
                            routeName = databaseResults.ElementAt(index: i).RouteName ?? "unknown";
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            !departureTime.Equals(value: value.DepartureTime) ||
                            !routeName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                default:
                    logger.LogWarning(message: "9400ZZSYMAL1: Service not found in schedule");
                    
                    break;
            }
            
            #endregion
            
            #region build test results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYMAL1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(
                    value: testResults,
                    options: Options));
            
            await containerClient
                .GetBlobClient(blobName: Path.Combine(
                    path1: "database",
                    path2: "9400ZZSYMAL1.json"))
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "application/json"
                        }
                    });
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        finally
        {
            storage.Delete(recursive: true);
        }
    }
}
```

Each file is named after the stop ID and contains:
- Database results
- Storage results
- Test results

## License

This project is licensed under the MIT License - see 
the [LICENSE](./LICENSE) file for details.