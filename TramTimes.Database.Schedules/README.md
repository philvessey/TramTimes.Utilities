# Database.Schedules

A .NET console application that creates structured JSON files with 
weekly schedules for tram stops. The schedules are used to test the 
output of the database builder.

## Features

- 📅 Generates weekly schedules in structured JSON format
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

The application generates JSON files containing weekly schedules with 
the following structure:

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

Each file is named after the stop ID and contains:
- **departure_time**: Scheduled departure time in HH:MM:SS format
- **destination_name**: Final destination of the tram service
- **route_name**: Route identifier (e.g., BLUE, PURP, YELL)

## License

This project is licensed under the MIT License - see 
the [LICENSE](./LICENSE) file for details.