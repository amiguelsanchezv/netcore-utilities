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

namespace InvOFSC
{
    public class Op
    {
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _inputFile;
        private readonly string _outputFile;
        private readonly string _endpointTemplate;

        public Op(IConfiguration configuration, string inputFile, string outputFile)
        {
            _configuration = configuration;
            _clientId = _configuration["clientId"] ?? throw new InvalidOperationException("clientId configuration is missing");
            _clientSecret = _configuration["clientSecret"] ?? throw new InvalidOperationException("clientSecret configuration is missing");
            _inputFile = inputFile ?? throw new ArgumentNullException(nameof(inputFile));
            _outputFile = outputFile ?? throw new ArgumentNullException(nameof(outputFile));
            _endpointTemplate = _configuration["endpoint_ofsc_act_inv"] ?? throw new InvalidOperationException("endpoint_ofsc_act_inv configuration is missing");
        }

        public List<Inv> getInventoryAsync(string activityId)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_endpointTemplate, activityId));
            
            var credentials = $"{_clientId}:{_clientSecret}";
            var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            
            var response = client.SendAsync(request).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Invs>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult())?.items?.ToList() ?? new List<Inv>();
        }

        public List<Inv> readCSV()
        {
            var output = new List<Inv>();
            using (var reader = new StreamReader(_inputFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();
                records.ForEach(r =>
                {
                    Console.WriteLine($"Processing activity {r.ActivityId}");
                    var result = getInventoryAsync($"{r.ActivityId}");
                    output.AddRange(result);
                });
            }
            return output;
        }

        public void saveToCsv(List<Inv> invs)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8
            };

            using (var writer = new StreamWriter(string.Format(_outputFile, DateTime.UtcNow.Ticks.ToString())))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                csvWriter.WriteRecords(invs);
            }
        }
    }
}
