using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string authentication = ConfigurationManager.AppSettings["authentication"];
        public List<Inv> getInventoryAsync(string activityId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(ConfigurationManager.AppSettings["endpoint_ofsc_act_inv"], activityId));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(authentication)));
            var response = client.SendAsync(request).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Invs>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult())?.items?.ToList() ?? new List<Inv>();
        }

        public List<Inv> readCSV()
        {
            var output = new List<Inv>();
            using (var reader = new StreamReader(ConfigurationManager.AppSettings["input_file"]))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();
                records.ForEach(r =>
                {
                    var result = new Op().getInventoryAsync($"{r.ID_DE_ACTIVIDAD}");
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

            using (var writer = new StreamWriter(string.Format((string)ConfigurationManager.AppSettings["output_file"], DateTime.UtcNow.Ticks.ToString())))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                csvWriter.WriteRecords(invs);
            }
        }
    }
}
