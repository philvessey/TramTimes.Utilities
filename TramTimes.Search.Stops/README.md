# Search.Stops

A .NET console application that creates C# quartz jobs from a template 
for tram stops. The jobs are used to index stop and service data.

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
using AutoMapper;
using Elastic.Clients.Elasticsearch;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Workers.Stops;

public class _9400ZZSYMAL1(
    NpgsqlDataSource dataSource,
    ElasticsearchClient searchService,
    ILogger<_9400ZZSYMAL1> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            #region get search feed
            
            var searchFeed = await searchService.GetAsync<SearchStop>(request: new GetRequest
            {
                Id = "9400ZZSYMAL1",
                Index = "southyorkshire"
            });
            
            List<SearchStopPoint> mappedResults = [];
            
            if (searchFeed.Source is not null)
                mappedResults = searchFeed.Source.Points ?? [];
            
            #endregion
            
            #region check search feed
            
            if (mappedResults.LastOrDefault()?.DepartureDateTime > DateTime.Now.AddHours(value: 4))
                return;
            
            #endregion
            
            #region get database feed
            
            var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var stopResults = await databaseFeed.GetStopsByIdAsync(
                id: "9400ZZSYMAL1",
                comparison: ComparisonType.Exact);
            
            var serviceResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYMAL1",
                target: DateTime.Now,
                offset: TimeSpan.FromMinutes(value: -60),
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromHours(value: 12),
                results: 250);
            
            var databaseResults = mapper.Map<List<SearchStop>>(source: stopResults).FirstOrDefault() ?? new SearchStop();
            databaseResults.Points = mapper.Map<List<SearchStopPoint>>(source: serviceResults) ?? [];
            
            #endregion
            
            #region check database feed
            
            if (databaseResults is { Latitude: not null, Longitude: not null })
                databaseResults.Location = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation 
                {
                    Lat = databaseResults.Latitude.Value,
                    Lon = databaseResults.Longitude.Value
                });
            
            #endregion
            
            #region set search feed
            
            await searchService.IndexAsync(request: new IndexRequest<SearchStop>
            {
                Document = databaseResults,
                Id = databaseResults.Id ?? "9400ZZSYMAL1",
                Index = "southyorkshire"
            });
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception: {exception}",
                args: e.ToString());
        }
    }
}
```

Each file is named after the stop ID and contains:
- Indexed data retrieved from Elasticsearch
- Fresh data retrieved from the database
- Uploaded to Azure Blob Storage

## License

This project is licensed under the MIT License - see 
the [LICENSE](./LICENSE) file for details.