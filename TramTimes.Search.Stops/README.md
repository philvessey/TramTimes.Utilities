# TramTimes.Search.Stops

A .NET console application that generates C# Quartz.NET jobs from templates for tram stops. These jobs are designed to
index stop and service data in Elasticsearch, enabling fast full-text search capabilities for tram stop lookups and
schedule queries.

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Usage](#-usage)
- [Output](#-output)
- [Configuration](#-configuration)
- [Technology Stack](#-technology-stack)
- [License](#-license)

## 🎯 Overview

This utility automates the creation of Quartz.NET job classes for tram stop search indexing. Each generated job:

- Retrieves existing indexed data from Elasticsearch
- Checks if the indexed data needs refreshing (4-hour threshold)
- Fetches stop metadata and service schedules from PostgreSQL
- Enriches data with geolocation information
- Updates the Elasticsearch index for fast search queries

The generated jobs enable real-time search functionality in TramTimes applications, allowing users to quickly find stops
and services.

## ✨ Features

- 📅 Template-based C# Quartz.NET job generation
- 🚊 Support for multiple tram networks via data files
- 📊 Interactive console interface with progress tracking
- 🔍 Elasticsearch integration for full-text search
- 📍 Geolocation support for proximity searches
- 🗄️ PostgreSQL database integration
- 🎨 Beautiful Spectre.Console UI

## ✅ Prerequisites

- [Git](https://git-scm.com/downloads)
- [GitHub CLI](https://cli.github.com/)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## 🚀 Getting Started

### Installation

1. Navigate to the project directory:
   ```bash
   cd TramTimes.Search.Stops
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

## 💻 Usage

### Run the Application

```bash
dotnet run
```

The application will:

1. Scan the `Data/` directory for `.txt` files containing stop IDs
2. Display an interactive progress bar
3. Generate C# job files in the `output/` directory

### Input Data Format

Create text files in the `Data/` directory with one stop ID per line:

```
9400ZZSYMAL1
9400ZZSYMHI1
9400ZZSYMHI2
```

## 📤 Output

The application generates C# Quartz.NET jobs with the following structure:

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
            if (logger.IsEnabled(logLevel: LogLevel.Error))
                logger.LogError(
                    message: "Exception: {exception}",
                    args: e.ToString());
        }
    }
}
```

### Key Components

Each generated job includes:

- **Search retrieval**: Fetches existing indexed data from Elasticsearch
- **Data validation**: Checks if indexed data is still fresh (4-hour threshold)
- **Stop metadata**: Retrieves stop information from PostgreSQL
- **Service data**: Fetches upcoming departure schedules
- **Geolocation enrichment**: Adds geographic coordinates for proximity searches
- **Index update**: Updates Elasticsearch with the latest information
- **Error handling**: Comprehensive try-catch with logging

### Example Output File

Generated files are named using the stop ID pattern (e.g., `_9400ZZSYMAL1.cs`) and placed in the `output/` directory.

## ⚙️ Configuration

### Data Files

Network configuration files are stored in the `Data/` directory:

- `manchester.txt` - Stop IDs for Manchester tram network
- `southyorkshire.txt` - Stop IDs for South Yorkshire tram network

Add new networks by creating additional `.txt` files with stop IDs.

### Template

The code generation template is located in the `Template/` directory and can be customized to modify the structure of
generated jobs.

## 🛠 Technology Stack

- **.NET 10.0** - Core framework
- **C# 13** - Programming language
- **Spectre.Console** - Rich console UI and progress tracking
- **Quartz.NET** - Job scheduling framework (in generated code)
- **Elasticsearch** - Search and analytics engine (in generated code)
- **PostgreSQL** - Database storage (in generated code)
- **AutoMapper** - Object mapping (in generated code)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.