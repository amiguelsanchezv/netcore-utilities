# Create Service Request OFSC

This tool allows you to create service requests in bulk in Oracle Field Service Cloud (OFSC) by reading data from a CSV file.

## Requirements

- .NET 8.0 SDK or higher

## Configuration

1. Copy `appsettings.example.json` to `appsettings.json` and configure your credentials:

```json
{
  "clientId": "your_client_id@your_ofsc_instance",
  "clientSecret": "your_client_secret",
  "endpoint_ofsc_service_requests": "https://your_ofsc_instance.fs.ocs.oraclecloud.com/rest/ofscCore/v1/serviceRequests"
}
```

## Input Format

The input CSV file must contain the following columns:

- `activityId`: Activity ID (integer)
- `requestType`: Request type (string, e.g.: "SR1", "IR")

Example (`input.example.csv`):

```csv
activityId,requestType
111473354,SR1
111473355,DRQ
111473356,IR
```

## Usage

```bash
dotnet run <input_file.csv> <output_file.csv>
```

Example:

```bash
dotnet run input.csv output_{0}.csv
```

The `{0}` placeholder in the output file name will be replaced by a timestamp.

## Output Format

The output CSV file contains the following fields:

- `activityId`: Processed activity ID
- `requestType`: Sent request type
- `requestId`: Created service request ID (if successful)
- `status`: Operation status ("Success", "Error XXX", "Exception")
- `errorMessage`: Error message (if there was any problem)
- `created`: Service request creation date and time (format: YYYY-MM-DD HH:MM:SS)

## API Reference

This tool uses the Oracle Field Service API:
- Endpoint: `POST /rest/ofscCore/v1/serviceRequests`
- Authentication: Basic Auth (clientId:clientSecret)
- Documentation: https://docs.oracle.com/en/cloud/saas/field-service/cxfsc/op-rest-ofsccore-v1-servicerequests-post.html

## Notes

- The tool processes requests sequentially with a small delay between each one to avoid overwhelming the API
- Errors are captured and logged in the output file for later review
- The output file is always generated, even if there are errors
- The `date` field is automatically set to today's date (YYYY-MM-DD format) in each request
