# .NET Core Utilities

A collection of .NET Core utilities for various tasks.

## Utilities

### [Get Activity Inventory OFSC](./get-activity-inventory-ofsc/)

Reads CSV files with activity IDs and retrieves inventories from the OFSC API.

**Features:**
- Reads activity IDs from CSV files
- Fetches inventory data from OFSC API
- Exports results to CSV format

**Quick Start:**
```bash
cd get-activity-inventory-ofsc
dotnet restore
dotnet build
dotnet run input.example.csv output_{0}.csv
```

For more details, see the [utility's README](./get-activity-inventory-ofsc/README.md).

### [Create Service Request OFSC](./create-service-request-ofsc/)

Creates service requests in bulk in Oracle Field Service Cloud (OFSC) by reading data from a CSV file.

**Features:**
- Reads activity IDs and request types from CSV files
- Creates service requests via OFSC API
- Automatically sets the date field to today's date
- Exports results to CSV format with success/error status

**Quick Start:**
```bash
cd create-service-request-ofsc
dotnet restore
dotnet build
dotnet run input.example.csv output_{0}.csv
```

For more details, see the [utility's README](./create-service-request-ofsc/README.md).

## Requirements

- .NET 10 SDK or higher

## Contributing

Each utility is self-contained in its own directory with its own configuration and documentation.

**Version:** 1.0.0  
**Last Update:** January, 2026