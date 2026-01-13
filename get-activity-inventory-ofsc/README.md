# Get Activity Inventory OFSC

.NET 10 console application to read CSV files with activity IDs and retrieve inventories from the OFSC API.

## Requirements

- .NET 10 SDK or higher

## Configuration

1. Copy `appsettings.example.json` to `appsettings.json` and edit it with your Oracle Field Service Cloud credentials:
   - `clientId`: Your OFSC client ID (used as username for Basic Auth)
   - `clientSecret`: Your OFSC client secret (used as password for Basic Auth)
   - `endpoint_ofsc_act_inv`: OFSC endpoint URL (can use `{0}` as a placeholder for activityId)

The application uses HTTP Basic Authentication with `clientId` as the username and `clientSecret` as the password.

## Build and Run

```bash
# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run <input_file> <output_file>
```

### Command Line Arguments

- `input_file`: Path to the input CSV file with activities (must exist)
- `output_file`: Path to the output CSV file (can use `{0}` as a placeholder for timestamp)

### Input CSV Format

The input CSV file must contain a column named `ActivityId` with the activity IDs to process.

Example (`input.example.csv`):
```csv
ActivityId
12345
67890
11111
```

### Example

```bash
dotnet run input.example.csv output_{0}.csv
```

## Project Structure

```
get-activity-inventory-ofsc/
├── Program.cs                  # Application entry point
├── Logic.cs                    # Business logic to read CSV, call API and save results
├── Model.cs                    # Data models (Inv, Invs)
├── appsettings.json            # Application configuration (create from appsettings.example.json)
├── appsettings.example.json   # Example configuration file
├── InvOFSC.csproj              # Project file
├── input.example.csv           # Example input CSV file
└── README.md                   # This file
```

**Version:** 1.0.0  
**Last Update:** January, 2026