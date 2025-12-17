# TramTimes.Utilities

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A collection of .NET console applications and support utilities for generating code and data structures for TramTimes
projects.

## 📋 Table of Contents

- [Overview](#-overview)
- [Projects](#-projects)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [License](#-license)

## 🎯 Overview

This repository contains utility applications that automate the generation of:

- C# Quartz.NET job schedulers for tram stop data caching and indexing
- JSON-formatted weekly schedules for tram stops
- Database builder output testing tools

These utilities support multiple tram networks and provide an interactive console interface for easy operation.

## 📦 Projects

### [Cache.Stops](./TramTimes.Cache.Stops/README.md)

A .NET console application that generates C# Quartz.NET jobs from templates for tram stops. These jobs are designed to
cache service and trip data in real-time systems.

**Key Features:**

- 📅 Template-based C# job generation
- 🚊 Multi-network support
- 📊 Interactive console interface

### [Database.Schedules](./TramTimes.Database.Schedules/README.md)

A .NET console application that creates structured JSON files containing weekly schedules for tram stops. These
schedules are used to validate and test database builder output.

**Key Features:**

- 📅 Weekly schedule generation in JSON format
- 🚊 Multi-network support
- 📊 Interactive console interface

### [Database.Stops](./TramTimes.Database.Stops/README.md)

A .NET console application that generates C# Quartz.NET jobs from templates for tram stops. These jobs are specifically
designed to test database builder functionality.

**Key Features:**

- 📅 Template-based C# job generation
- 🚊 Multi-network support
- 📊 Interactive console interface

### [Search.Stops](./TramTimes.Search.Stops/README.md)

A .NET console application that generates C# Quartz.NET jobs from templates for tram stops. These jobs are used to index
stop and service data for search functionality.

**Key Features:**

- 📅 Template-based C# job generation
- 🚊 Multi-network support
- 📊 Interactive console interface

## ✅ Prerequisites

- [Git](https://git-scm.com/downloads)
- [GitHub CLI](https://cli.github.com/)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## 🚀 Getting Started

### Clone the Repository

```bash
git clone https://github.com/philvessey/TramTimes.Utilities.git
cd TramTimes.Utilities
```

### Build All Projects

```bash
dotnet build
```

### Run a Specific Project

Navigate to the project directory and run:

```bash
cd TramTimes.Cache.Stops
dotnet run
```

Repeat for any other project you wish to run (`TramTimes.Database.Schedules`, `TramTimes.Database.Stops`, or
`TramTimes.Search.Stops`).

For detailed usage instructions, please refer to each project's individual README.

## 🛠 Technology Stack

- **.NET 10.0** - Core framework
- **C# 13** - Programming language
- **Spectre.Console** - Rich console UI library
- **HtmlAgilityPack** - HTML parsing and manipulation
- **Polly** - Resilience and transient-fault-handling library
- **JetBrains.Annotations** - Code annotations

## 📁 Project Structure

```
TramTimes.Utilities/
├── TramTimes.Cache.Stops/           # Cache job generator
├── TramTimes.Database.Schedules/    # Schedule JSON generator
├── TramTimes.Database.Stops/        # Database test job generator
├── TramTimes.Search.Stops/          # Search indexing job generator
├── Directory.Build.props            # Shared build configuration
├── Directory.Packages.props         # Centralized package management
└── TramTimes.slnx                   # Solution file
```

Each project follows a consistent structure:

- `Program.cs` - Application entry point
- `Builders/` - Code generation builders
- `Data/` - Input data files (network configurations)
- `Extensions/` - Utility extension methods
- `Tools/` - Helper utilities
- `Template/` - Code generation templates
- `output/` - Generated files (git-ignored)

## 📄 License

These projects are licensed under the MIT License - see the [LICENSE](LICENSE) file for details.