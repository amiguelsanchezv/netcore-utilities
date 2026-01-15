using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CreateServiceRequestOFSC
{
    public class ServiceRequestProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _inputFile;
        private readonly string _outputFile;
        private readonly string _endpoint;

        public ServiceRequestProcessor(IConfiguration configuration, string inputFile, string outputFile)
        {
            _configuration = configuration;
            _clientId = _configuration["clientId"] ?? throw new InvalidOperationException("clientId configuration is missing");
            _clientSecret = _configuration["clientSecret"] ?? throw new InvalidOperationException("clientSecret configuration is missing");
            _inputFile = inputFile ?? throw new ArgumentNullException(nameof(inputFile));
            _outputFile = outputFile ?? throw new ArgumentNullException(nameof(outputFile));
            _endpoint = _configuration["endpoint_ofsc_service_requests"] ?? throw new InvalidOperationException("endpoint_ofsc_service_requests configuration is missing");
        }

        public async Task<ServiceRequestResult> CreateServiceRequestAsync(ServiceRequestInput input)
        {
            try
            {
                using var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
                
                // Basic Authentication
                var credentials = $"{_clientId}:{_clientSecret}";
                var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                // Create request body
                var requestBody = new ServiceRequestRequest
                {
                    activityId = input.activityId,
                    requestType = input.requestType,
                    date = DateTime.Today.ToString("yyyy-MM-dd")
                };
                
                var jsonContent = JsonConvert.SerializeObject(requestBody);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    var serviceRequest = JsonConvert.DeserializeObject<ServiceRequestResponse>(responseContent);
                    return new ServiceRequestResult
                    {
                        activityId = input.activityId,
                        requestType = input.requestType,
                        requestId = serviceRequest?.requestId,
                        status = "Success",
                        created = serviceRequest?.created
                    };
                }
                else
                {
                    ErrorResponse? error = null;
                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                    }
                    catch
                    {
                        // If error response is not in expected format, use raw content
                    }
                    
                    return new ServiceRequestResult
                    {
                        activityId = input.activityId,
                        requestType = input.requestType,
                        status = $"Error {response.StatusCode}",
                        errorMessage = error?.detail ?? error?.title ?? responseContent
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceRequestResult
                {
                    activityId = input.activityId,
                    requestType = input.requestType,
                    status = "Exception",
                    errorMessage = ex.Message
                };
            }
        }

        public List<ServiceRequestInput> ReadCSV()
        {
            var output = new List<ServiceRequestInput>();
            using (var reader = new StreamReader(_inputFile))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8
            }))
            {
                var records = csv.GetRecords<ServiceRequestInput>().ToList();
                output.AddRange(records);
            }
            return output;
        }

        public async Task<List<ServiceRequestResult>> ProcessServiceRequestsAsync()
        {
            var inputs = ReadCSV();
            var results = new List<ServiceRequestResult>();
            
            Console.WriteLine($"Found {inputs.Count} service requests to create.");
            
            foreach (var input in inputs)
            {
                Console.WriteLine($"Processing activityId: {input.activityId}, requestType: {input.requestType}");
                var result = await CreateServiceRequestAsync(input);
                results.Add(result);
                
                if (result.status == "Success")
                {
                    Console.WriteLine($"  ✓ Success - RequestId: {result.requestId}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Failed - {result.status}: {result.errorMessage}");
                }
                
                // Small delay to avoid overwhelming the API
                await Task.Delay(100);
            }
            
            return results;
        }

        public void SaveToCsv(List<ServiceRequestResult> results)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8
            };

            var outputPath = _outputFile.Contains("{0}") 
                ? string.Format(_outputFile, DateTime.UtcNow.Ticks.ToString()) 
                : _outputFile;

            using (var writer = new StreamWriter(outputPath))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                csvWriter.WriteRecords(results);
            }
            
            Console.WriteLine($"\nResults saved to: {outputPath}");
        }
    }
}
