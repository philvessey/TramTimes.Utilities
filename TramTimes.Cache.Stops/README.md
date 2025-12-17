# TramTimes.Cache.Stops

A .NET console application that generates C# Quartz.NET jobs from templates for tram stops. These jobs are designed to
cache service and trip data in real-time transit systems, fetching data from PostgreSQL and storing it in Redis for fast
access.

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

This utility automates the creation of Quartz.NET job classes for tram stop caching. Each generated job:

- Retrieves cached stop data from Redis
- Fetches fresh schedule data from a PostgreSQL database
- Updates the cache with the latest information
- Manages cache expiration and refresh logic

The generated jobs are intended for use in production TramTimes applications to ensure responsive API performance.

## ✨ Features

- 📅 Template-based C# Quartz.NET job generation
- 🚊 Support for multiple tram networks via data files
- 📊 Interactive console interface with progress tracking
- 🔄 Automatic cache refresh logic
- ⚡ Redis caching integration
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
   cd TramTimes.Cache.Stops
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

### Key Components

Each generated job includes:

- **Cache retrieval**: Fetches existing cached data from Redis
- **Cache validation**: Checks if cached data is still fresh (4-hour threshold)
- **Database query**: Retrieves latest schedule from PostgreSQL
- **Cache update**: Stores updated data in Redis with 12-hour expiry
- **Trip indexing**: Caches individual trip data for detailed queries
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
- **Redis** - Caching layer (in generated code)
- **PostgreSQL** - Database storage (in generated code)
- **AutoMapper** - Object mapping (in generated code)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.