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

## Requirements

- .NET 10 SDK or higher

## Contributing

Each utility is self-contained in its own directory with its own configuration and documentation.

**Version:** 1.0.0  
**Last Update:** January, 2026