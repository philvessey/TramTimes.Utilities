# TramTimes.Database.Schedules

A .NET console application that generates structured JSON files containing weekly schedules for tram stops. These
schedules are used to validate and test the output of the database builder, ensuring schedule accuracy across all days
of the week.

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

This utility creates comprehensive weekly schedules in JSON format for tram stops. The generated schedules provide a
reference dataset for:

- Testing database builder functionality
- Validating schedule accuracy
- Comparing expected vs actual departure times
- Ensuring data consistency across days of the week

## ✨ Features

- 📅 Generates weekly schedules in structured JSON format
- 🚊 Support for multiple tram networks via data files
- 📊 Interactive console interface with progress tracking
- 🔍 Comprehensive schedule validation data
- 📋 Organized by day of the week
- 🎨 Beautiful Spectre.Console UI

## ✅ Prerequisites

- [Git](https://git-scm.com/downloads)
- [GitHub CLI](https://cli.github.com/)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## 🚀 Getting Started

### Installation

1. Navigate to the project directory:
   ```bash
   cd TramTimes.Database.Schedules
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
3. Generate JSON schedule files in the `output/` directory

### Input Data Format

Create text files in the `Data/` directory with one stop ID per line:

```
9400ZZSYMAL1
9400ZZSYMHI1
9400ZZSYMHI2
```

## 📤 Output

The application generates JSON files containing weekly schedules with the following structure:

```json
{
  "monday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ],
  "tuesday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ],
  "wednesday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ],
  "thursday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ],
  "friday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ],
  "saturday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ],
  "sunday": [
    {
      "departure_time": "12:00:00",
      "destination_name": "Malin Bridge",
      "route_name": "BLUE"
    }
  ]
}
```

### JSON Structure

Each file is named after the stop ID (e.g., `_9400ZZSYMAL1.json`) and contains:

- **Day Arrays**: Separate arrays for each day of the week (monday through sunday)
- **Service Objects**: Each service includes:
    - `departure_time` - Scheduled departure time in HH:MM:SS format
    - `destination_name` - Final destination of the tram service
    - `route_name` - Route identifier (e.g., BLUE, PURP, YELL)

### Example Output File

Generated files are placed in the `output/` directory and can be used for:

- Automated testing of schedule accuracy
- Manual verification of timetables
- Data comparison with live systems
- Quality assurance processes

## ⚙️ Configuration

### Data Files

Network configuration files are stored in the `Data/` directory:

- `manchester.txt` - Stop IDs for Manchester tram network
- `southyorkshire.txt` - Stop IDs for South Yorkshire tram network

Add new networks by creating additional `.txt` files with stop IDs.

### Template

The JSON generation template is located in the `Template/` directory and can be customized to modify the structure of
generated schedules.

## 🛠 Technology Stack

- **.NET 10.0** - Core framework
- **C# 13** - Programming language
- **Spectre.Console** - Rich console UI and progress tracking
- **System.Text.Json** - JSON serialization
- **HtmlAgilityPack** - HTML parsing for schedule extraction
- **Polly** - Resilience and transient-fault-handling

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.