# Cache.Stops

A .NET console application that creates C# quartz jobs from a template 
for tram stops. The jobs are used to cache service and trip data.

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
using AutoMapper;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using StackExchange.Redis;
using TramTimes.Cache.Jobs.Models;

namespace TramTimes.Cache.Jobs.Workers.Stops;

public class _9400ZZSYMAL1(
    NpgsqlDataSource dataSource,
    IConnectionMultiplexer cacheService,
    ILogger<_9400ZZSYMAL1> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            #region get cache feed
            
            var cacheFeed = await cacheService
                .GetDatabase()
                .StringGetAsync(key: "southyorkshire:stop:9400ZZSYMAL1");
            
            List<CacheStopPoint> mappedResults = [];
            
            if (!cacheFeed.IsNullOrEmpty)
                mappedResults = mapper.Map<List<CacheStopPoint>>(
                    source: JsonSerializer.Deserialize<List<WorkerStopPoint>>(
                        json: cacheFeed.ToString()));
            
            #endregion
            
            #region check cache feed
            
            if (mappedResults.LastOrDefault()?.DepartureDateTime > DateTime.Now.AddHours(value: 4))
                return;
            
            #endregion
            
            #region get database feed
            
            var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var databaseResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYMAL1",
                target: DateTime.Now,
                offset: TimeSpan.FromMinutes(value: -60),
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromHours(value: 12),
                results: 250);
            
            #endregion
            
            #region set cache feed
            
            await cacheService
                .GetDatabase()
                .StringSetAsync(
                    key: "southyorkshire:stop:9400ZZSYMAL1",
                    value: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults)),
                    expiry: TimeSpan.FromHours(value: 12));
            
            #endregion
            
            #region get trip feed
            
            var tripFeed = databaseResults
                .Select(selector: s => s.TripId)
                .ToList();
            
            #endregion
            
            #region set cache feed
            
            foreach (var item in tripFeed)
            {
                databaseResults = await databaseFeed.GetServicesByTripAsync(
                    id: item,
                    target: DateTime.Now,
                    offset: TimeSpan.FromMinutes(value: -60),
                    comparison: ComparisonType.Exact,
                    tolerance: TimeSpan.FromHours(value: 12),
                    results: 250);
                
                await cacheService
                    .GetDatabase()
                    .StringSetAsync(
                        key: $"southyorkshire:trip:{item}",
                        value: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults)),
                        expiry: TimeSpan.FromHours(value: 12));
            }
            
            #endregion
        }
        catch (Exception e)
        {
            if (logger.IsEnabled(logLevel: LogLevel.Error))
                logger.LogError(
                    message: "Exception: {exception}",
                    args: e.ToString());
        }
    }
}
```

Each file is named after the stop ID and contains:
- Cached data retrieved from Redis
- Fresh data retrieved from the database
- Uploaded to Azure Blob Storage

## License

This project is licensed under the MIT License - see 
the [LICENSE](./LICENSE) file for details.