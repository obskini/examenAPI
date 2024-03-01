using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppSoftwareExamen.Classes
{
    public class APIPingIP
    {
        private const string ApiUrlBase = "https://steakovercooked.com/api/ping/?host=";

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

        public class PingResult
        {
            public string Host { get; set; }
            public string Size { get; set; }
            public string TTL { get; set; }
            public string Time { get; set; }
            public string Sent { get; set; }
            public string Received { get; set; }
            public string PacketLoss { get; set; }
        }

        public static PingResult ParsePingResult(string line)
        {
            string[] parts = line.Split(new[] { ' ', '=', ':' }, StringSplitOptions.RemoveEmptyEntries);

            string host = parts[2];
            string size = parts[0];
            string ttl = parts[4]; 
            string time = parts[6];
            string sent = parts[9]; 
            string received = parts[11]; 
            string packetLoss = parts[13]; 

            return new PingResult
            {
                Host = host,
                Size = size,
                TTL = ttl,
                Time = time,
                Sent = sent,
                Received = received,
                PacketLoss = packetLoss
            };
        }

    }
}
