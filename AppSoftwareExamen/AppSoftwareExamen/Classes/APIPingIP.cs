using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSoftwareExamen.Classes
{
    public class APIPingIP
    {
        private const string ApiUrlBase = "http://141.95.16.251:46521/ping/";

        public class PingResult
        {
            [JsonPropertyName("ip")]
            public string IP { get; set; }

            [JsonPropertyName("bytes")]
            public int Bytes { get; set; }

            [JsonPropertyName("time_ms")]
            public double TimeMs { get; set; }

            [JsonPropertyName("ttl")]
            public int TTL { get; set; }

            [JsonPropertyName("packet_loss_percentage")]
            public int PacketLossPercentage { get; set; }
        }

        public static async Task<string> PingIPAsync(string ipAddress)
        {
            try
            {
                string apiUrl = ApiUrlBase + ipAddress;

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    else
                    {
                        throw new Exception("Failed to ping IP address.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while pinging IP address.", ex);
            }
        }
    }
}
