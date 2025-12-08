# TramTimes.Database.Stops

A .NET console application that generates C# Quartz.NET jobs from templates for tram stops. These jobs are designed to test and validate the output of the database builder by comparing actual database results against expected schedules.

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

This utility automates the creation of Quartz.NET job classes for testing tram stop database functionality. Each generated job:
- Loads reference schedules from JSON files
- Queries the PostgreSQL database for actual schedule data
- Compares service counts between expected and actual results
- Logs warnings when discrepancies are detected
- Uploads test results to Azure Blob Storage

The generated jobs are essential for continuous integration and quality assurance of the TramTimes database builder.

## ✨ Features

- 📅 Template-based C# Quartz.NET job generation
- 🚊 Support for multiple tram networks via data files
- 📊 Interactive console interface with progress tracking
- 🔍 Automated schedule validation
- ⚠️ Warning detection for schedule mismatches
- ☁️ Azure Blob Storage integration
- 🗄️ PostgreSQL database integration
- 🎨 Beautiful Spectre.Console UI

## ✅ Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- Input data files in the `Data/` directory (text files with stop IDs, one per line)

## 🚀 Getting Started

### Installation

1. Navigate to the project directory:
   ```bash
   cd TramTimes.Database.Stops
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
9400ZZSYMAG1
9400ZZSYMAG2
```

## 📤 Output

The application generates C# Quartz.NET jobs with the following structure:

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
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (databaseResults.Count != activeSchedule?.Tuesday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (databaseResults.Count != activeSchedule?.Wednesday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (databaseResults.Count != activeSchedule?.Thursday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (databaseResults.Count != activeSchedule?.Friday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (databaseResults.Count != activeSchedule?.Saturday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (databaseResults.Count != activeSchedule?.Sunday?.Count)
                        logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
                    break;
                }
                default:
                    logger.LogWarning(message: "9400ZZSYMAL1: Service count does not match schedule");
                    
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
                    path1: "schedules",
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
            if (logger.IsEnabled(logLevel: LogLevel.Error))
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

### Key Components

Each generated job includes:
- **Schedule loading**: Reads reference schedules from JSON files
- **Database query**: Fetches actual schedule data from PostgreSQL
- **Day-specific validation**: Compares service counts for each day of the week
- **Warning logging**: Logs discrepancies between expected and actual counts
- **Test result processing**: Identifies specific mismatches in departure times and routes
- **Blob storage upload**: Saves test results to Azure Blob Storage
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

The code generation template is located in the `Template/` directory and can be customized to modify the structure of generated jobs.

### Schedule Files

The generated jobs expect schedule JSON files to be present in the `Workers/Schedules/` directory at runtime. These schedules are typically generated by the [TramTimes.Database.Schedules](../TramTimes.Database.Schedules/README.md) utility.

## 🛠 Technology Stack

- **.NET 10.0** - Core framework
- **C# 13** - Programming language
- **Spectre.Console** - Rich console UI and progress tracking
- **Quartz.NET** - Job scheduling framework (in generated code)
- **Azure Blob Storage** - Cloud storage for test results (in generated code)
- **PostgreSQL** - Database storage (in generated code)
- **System.Text.Json** - JSON serialization

## 📄 License

This project is licensed under the MIT License - see 
the [LICENSE](./LICENSE) file for details.